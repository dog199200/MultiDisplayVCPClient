using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SuchByte.MacroDeck.GUI;
using System.Drawing;

namespace MultiDisplayVCPClient.GUI
{
    public partial class VcpConnectionList : Form
    {
        public List<VcpConnectionToggler> _Connections = new();

        private const int ITEM_HEIGHT = 41;
        private const int BORDER_HEIGHT = 2;
        private const int MAX_ITEMS_BEFORE_SCROLL = 4;
        private const int FORM_WIDTH = 340;

        public VcpConnectionList()
        {
            InitializeComponent();
            this.btnManage.Click += BtnManage_Click;
            this.flowLayoutPanel1.Resize += FlowLayoutPanel1_Resize;

            SetListPanelSize(0);
        }

        public VcpConnectionList(List<VcpClient> clients)
        {
            InitializeComponent();
            this.btnManage.Click += BtnManage_Click;
            this.flowLayoutPanel1.Resize += FlowLayoutPanel1_Resize;

            foreach (var client in clients)
            {
                AddConnection(client);
            }

            SetListPanelSize(clients.Count);
        }

        private void BtnManage_Click(object sender, System.EventArgs e)
        {
            this.Close();
            PluginInstance.Main.OpenConfigurator();
        }

        private void FlowLayoutPanel1_Resize(object sender, System.EventArgs e)
        {
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                c.Width = flowLayoutPanel1.ClientSize.Width;
            }
        }

        /// <summary>
        /// Calculates and sets the flowLayoutPanel1's height.
        /// </summary>
        private void SetListPanelSize(int itemCount)
        {
            int itemsToShow = System.Math.Min(itemCount, MAX_ITEMS_BEFORE_SCROLL);
            if (itemsToShow == 0) itemsToShow = 1;

            int listHeight = (itemsToShow * ITEM_HEIGHT) + BORDER_HEIGHT;

            // --- THIS IS THE FIX ---
            // Set the form width (which is constant)
            this.Width = FORM_WIDTH;
            // Set the LIST's height. The TLP and Form will AutoSize around this.
            this.flowLayoutPanel1.Size = new Size(FORM_WIDTH - 6, listHeight);
            // --- END FIX ---
        }

        public void AddConnection(VcpClient client)
        {
            this.InvokeIfRequired(() =>
            {
                var toggler = _Connections.FirstOrDefault(x => x.Client == client);

                if (toggler == null)
                {
                    toggler = new VcpConnectionToggler(client);
                    toggler.Width = flowLayoutPanel1.ClientSize.Width;
                    _Connections.Add(toggler);
                    flowLayoutPanel1.SuspendLayout();
                    flowLayoutPanel1.Controls.Add(toggler);
                    flowLayoutPanel1.ResumeLayout();
                }
            });
        }

        public void RemoveConnection(VcpClient client)
        {
            this.InvokeIfRequired(() =>
            {
                var toggler = _Connections.FirstOrDefault(x => x.Client == client);
                if (toggler == null) return;

                _Connections.Remove(toggler);
                flowLayoutPanel1.SuspendLayout();
                flowLayoutPanel1.Controls.Remove(toggler);
                flowLayoutPanel1.ResumeLayout();
            });
        }
    }
}