using System;
using System.Globalization;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MultiDisplayVCPClient
{
    /// <summary>
    /// Represents the connection status of a VcpClient.
    /// </summary>
    public enum ConnectionState
    {
        Offline,
        Connecting,
        Connected
    }

    /// <summary>
    /// Manages a network connection to a single VCP server.
    /// </summary>
    public class VcpClient(string name, string ipAddress, int port, string password)
    {
        /// <summary>
        /// The user-defined friendly name for this connection.
        /// </summary>
        public string Name { get; } = name;
        private readonly string _ipAddress = ipAddress;
        private readonly int _port = port;
        private readonly string _password = password;

        /// <summary>
        /// The current connection state of the client.
        /// </summary>
        public ConnectionState State { get; private set; } = ConnectionState.Offline;

        /// <summary>
        /// Fires when the client's connection state changes.
        /// </summary>
        public event EventHandler<ConnectionState>? ConnectionStateChanged;

        /// <summary>
        /// Checks if the provided settings match the client's current settings.
        /// </summary>
        /// <returns>True if settings are identical, otherwise false.</returns>
        public bool HasSameSettings(string ipAddress, int port, string password)
        {
            return _ipAddress == ipAddress && _port == port && _password == password;
        }

        /// <summary>
        /// The core method for sending any command to the server.
        /// </summary>
        private async Task<string> InternalSendCommand(string command, bool isTest = false)
        {
            var readTimeout = isTest ? 5000 : 60000;

            try
            {
                using var client = new TcpClient();
                Task connectTask = client.ConnectAsync(_ipAddress, _port);
                if (await Task.WhenAny(connectTask, Task.Delay(2000)) != connectTask)
                {
                    throw new Exception("Connection timed out.");
                }

                await using NetworkStream stream = client.GetStream();
                using var cts = new CancellationTokenSource(readTimeout);

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

                await stream.WriteAsync(data.AsMemory(0, data.Length), cts.Token);

                byte[] buffer = new byte[8192];
                int bytesRead = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cts.Token);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                if (isTest)
                {
                    if (response.StartsWith("ERROR: Invalid Hash"))
                    {
                        throw new Exception("Invalid password (hash mismatch).");
                    }
                    return "PING_SUCCESS";
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                var error = $"Operation timed out ({readTimeout / 1000}s).";
                return $"ERROR: {error}";
            }
            catch (Exception ex)
            {
                if (isTest) throw;
                return $"ERROR: {ex.Message}";
            }
        }

        /// <summary>
        /// Runs a fast "PING" test to connect and validate the connection.
        /// </summary>
        public async Task<bool> ConnectAsync()
        {
            if (State == ConnectionState.Connecting) return false;

            try
            {
                State = ConnectionState.Connecting;
                ConnectionStateChanged?.Invoke(this, State);

                string response = await InternalSendCommand("PING", isTest: true);

                if (response == "PING_SUCCESS")
                {
                    State = ConnectionState.Connected;
                    ConnectionStateChanged?.Invoke(this, State);
                    return true;
                }
                else
                {
                    throw new Exception($"Unexpected PING response: {response}");
                }
            }
            catch (Exception)
            {
                Disconnect();
                throw;
            }
        }

        /// <summary>
        /// Sets the client's state to Offline and fires the state change event.
        /// </summary>
        public void Disconnect()
        {
            if (State == ConnectionState.Offline) return;

            State = ConnectionState.Offline;
            ConnectionStateChanged?.Invoke(this, State);
        }

        /// <summary>
        /// Sends a raw command to the server.
        /// </summary>
        public async Task<string> SendCommandAsync(string command)
        {
            return await InternalSendCommand(command);
        }

        /// <summary>
        /// Fetches and deserializes the full VCP capabilities list from the server.
        /// </summary>
        public async Task<ServerStatus> GetCapabilitiesAsync()
        {
            string jsonResponse = await SendCommandAsync("GET_CAPS");

            if (jsonResponse.StartsWith("ERROR:"))
            {
                return new ServerStatus { Message = jsonResponse };
            }

            try
            {
                ServerStatus? status = JsonSerializer.Deserialize<ServerStatus>(jsonResponse);
                return status ?? throw new Exception("Deserialization returned null.");
            }
            catch (Exception ex)
            {
                return new ServerStatus { Message = $"ERROR: Failed to parse JSON response. {ex.Message}" };
            }
        }
    }
}