using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI
{
    /// <summary>
    /// A pop-up form that displays a list of VcpConnectionToggler controls.
    /// </summary>
    public partial class VcpConnectionList : Form
    {
        /// <summary>
        /// A list of the toggler controls currently displayed in the form.
        /// </summary>
        public List<VcpConnectionToggler> _Connections = [];

        private const int ITEM_HEIGHT = 41;
        private const int BORDER_HEIGHT = 2;
        private const int MAX_ITEMS_BEFORE_SCROLL = 4;
        private const int FORM_WIDTH = 340;

        /// <summary>
        /// Initializes a new instance of the VcpConnectionList form (Default constructor).
        /// </summary>
        public VcpConnectionList()
        {
            InitializeComponent();
            this.btnManage.Click += BtnManage_Click;
            this.flowLayoutPanel1.Resize += FlowLayoutPanel1_Resize;

            SetListPanelSize(0);
        }

        /// <summary>
        /// Initializes a new instance of the VcpConnectionList form with a list of clients.
        /// </summary>
        /// <param name="clients">The list of VcpClient instances to display.</param>
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

        /// <summary>
        /// Handles the Click event for the 'Manage' button. Closes the pop-up and opens the main plugin config.
        /// </summary>
        private void BtnManage_Click(object? sender, System.EventArgs e)
        {
            this.Close();
            PluginInstance.Main.OpenConfigurator();
        }

        /// <summary>
        /// Handles the Resize event of the flow panel to ensure all child controls fill its width.
        /// </summary>
        private void FlowLayoutPanel1_Resize(object? sender, System.EventArgs e)
        {
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                c.Width = flowLayoutPanel1.ClientSize.Width;
            }
        }

        /// <summary>
        /// Calculates and sets the flowLayoutPanel's height based on the number of items, up to a maximum.
        /// </summary>
        /// <param name="itemCount">The number of items in the list.</param>
        private void SetListPanelSize(int itemCount)
        {
            int itemsToShow = System.Math.Min(itemCount, MAX_ITEMS_BEFORE_SCROLL);
            if (itemsToShow == 0) itemsToShow = 1;

            int listHeight = (itemsToShow * ITEM_HEIGHT) + BORDER_HEIGHT;

            this.Width = FORM_WIDTH;
            this.flowLayoutPanel1.Size = new Size(FORM_WIDTH - 6, listHeight);
        }

        /// <summary>
        /// Adds a VcpConnectionToggler control for the given client to the list.
        /// </summary>
        /// <param name="client">The VcpClient to add.</param>
        public void AddConnection(VcpClient client)
        {
            this.InvokeIfRequired(() =>
            {
                var toggler = _Connections.FirstOrDefault(x => x.Client == client);

                if (toggler == null)
                {
                    toggler = new VcpConnectionToggler(client)
                    {
                        Width = flowLayoutPanel1.ClientSize.Width
                    };
                    _Connections.Add(toggler);
                    flowLayoutPanel1.SuspendLayout();
                    flowLayoutPanel1.Controls.Add(toggler);
                    flowLayoutPanel1.ResumeLayout();
                }
            });
        }

        /// <summary>
        /// Removes the VcpConnectionToggler control associated with the given client from the list.
        /// </summary>
        /// <param name="client">The VcpClient to remove.</param>
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