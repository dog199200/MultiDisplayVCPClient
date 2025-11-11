using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SuchByte.MacroDeck.Logging;
using System.Threading;
using System.Security.Cryptography;
using System.Globalization;

namespace MultiDisplayVCPClient
{
    public enum ConnectionState
    {
        Offline,
        Connecting,
        Connected
    }

    public class VcpClient
    {
        public string Name { get; }
        private readonly string _ipAddress;
        private readonly int _port;
        private readonly string _password;

        public ConnectionState State { get; private set; } = ConnectionState.Offline;
        public event EventHandler<ConnectionState> ConnectionStateChanged;

        public VcpClient(string name, string ipAddress, int port, string password)
        {
            Name = name;
            _ipAddress = ipAddress;
            _port = port;
            _password = password;
        }

        public bool HasSameSettings(string ipAddress, int port, string password)
        {
            return _ipAddress == ipAddress && _port == port && _password == password;
        }

        private async Task<string> InternalSendCommand(string command, bool isTest = false)
        {
            // Use 5s timeout for "PING" test, 60s for real commands
            var readTimeout = isTest ? 5000 : 60000;

            MacroDeckLogger.Info(PluginInstance.Main, $"({Name}) Connecting to {_ipAddress}:{_port}...");
            try
            {
                using (var client = new TcpClient())
                {
                    Task connectTask = client.ConnectAsync(_ipAddress, _port);
                    if (await Task.WhenAny(connectTask, Task.Delay(2000)) != connectTask)
                    {
                        throw new Exception("Connection timed out.");
                    }

                    using (NetworkStream stream = client.GetStream())
                    {
                        using (var cts = new CancellationTokenSource(readTimeout))
                        {
                            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            string timestampStr = timestamp.ToString(CultureInfo.InvariantCulture);

                            string messageToHash = command + timestampStr;

                            string hashBase64;
                            using (var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(_password)))
                            {
                                byte[] computedHashBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(messageToHash));
                                hashBase64 = Convert.ToBase64String(computedHashBytes);
                            }

                            string message = $"{timestampStr}|{hashBase64}|{command}";
                            byte[] data = Encoding.ASCII.GetBytes(message);

                            await stream.WriteAsync(data, 0, data.Length, cts.Token);
                            MacroDeckLogger.Info(PluginInstance.Main, $"({Name}) Sent hash and command: {command}");

                            byte[] buffer = new byte[8192];
                            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                            string logResponse = response.Length > 100 ? response.Substring(0, 100) + "..." : response;
                            MacroDeckLogger.Info(PluginInstance.Main, $"({Name}) Received: '{logResponse}'");

                            // --- MODIFIED PING LOGIC ---
                            if (isTest)
                            {
                                // "Invalid Hash" is the *only* test failure
                                if (response.StartsWith("ERROR: Invalid Hash"))
                                {
                                    throw new Exception("Invalid password (hash mismatch).");
                                }
                                // Any other response, even "ERROR: Invalid Command",
                                // proves the connection and password are valid.
                                return "PING_SUCCESS";
                            }
                            // --- END MODIFIED ---

                            return response;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                var error = $"Operation timed out ({readTimeout / 1000}s).";
                MacroDeckLogger.Warning(PluginInstance.Main, $"({Name}) Command '{command}' failed: {error}");
                return $"ERROR: {error}";
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Warning(PluginInstance.Main, $"({Name}) Command '{command}' failed: {ex.Message}");
                if (isTest) throw; // Throw test failures
                return $"ERROR: {ex.Message}";
            }
        }

        // --- MODIFIED: This is now the FAST "PING" connection ---
        public async Task<bool> ConnectAsync()
        {
            if (State == ConnectionState.Connecting) return false;

            MacroDeckLogger.Info(PluginInstance.Main, $"({Name}) Testing connection with PING...");
            try
            {
                State = ConnectionState.Connecting;
                ConnectionStateChanged?.Invoke(this, State);

                // Send "PING" as a test.
                string response = await InternalSendCommand("PING", isTest: true);

                if (response == "PING_SUCCESS")
                {
                    State = ConnectionState.Connected;
                    ConnectionStateChanged?.Invoke(this, State);
                    MacroDeckLogger.Info(PluginInstance.Main, $"({Name}) PING test successful.");
                    return true;
                }
                else
                {
                    // This should not happen, but as a failsafe
                    throw new Exception($"Unexpected PING response: {response}");
                }
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Warning(PluginInstance.Main, $"({Name}) PING test failed: {ex.Message}");
                Disconnect();
                throw;
            }
        }
        // --- END MODIFIED ---

        public void Disconnect()
        {
            if (State == ConnectionState.Offline) return;
            MacroDeckLogger.Info(PluginInstance.Main, $"({Name}) Setting state to Disconnected.");

            State = ConnectionState.Offline;
            ConnectionStateChanged?.Invoke(this, State);
        }

        public async Task<string> SendCommandAsync(string command)
        {
            return await InternalSendCommand(command);
        }

        // --- This method is now our "slow fetch" ---
        public async Task<ServerStatus> GetCapabilitiesAsync()
        {
            string jsonResponse = await SendCommandAsync("GET_CAPS");

            if (jsonResponse.StartsWith("ERROR:"))
            {
                MacroDeckLogger.Warning(PluginInstance.Main, $"({Name}) Server returned error for GET_CAPS: {jsonResponse}");
                return new ServerStatus { Message = jsonResponse };
            }

            try
            {
                ServerStatus status = JsonSerializer.Deserialize<ServerStatus>(jsonResponse);
                MacroDeckLogger.Info(PluginInstance.Main, $"({Name}) Successfully deserialized GET_CAPS. Found {status.Monitors.Count} monitors.");
                return status;
            }
            catch (Exception ex)
            {
                MacroDeckLogger.Error(PluginInstance.Main, $"({Name}) Failed to parse JSON response: {ex.Message}\nResponse was: {jsonResponse}");
                return new ServerStatus { Message = $"ERROR: Failed to parse JSON response. {ex.Message}" };
            }
        }
    }
}