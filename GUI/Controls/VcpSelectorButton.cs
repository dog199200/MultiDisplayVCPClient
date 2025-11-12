using SuchByte.MacroDeck.GUI;
using System.Drawing;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI.Controls
{
    /// <summary>
    /// A base picture box control that adds selection and notification painting.
    /// </summary>
    public class VcpContentButton : PictureBox
    {
        private bool _notification;
        private bool _selected;

        /// <summary>
        /// Sets the notification dot visibility on the control.
        /// </summary>
        public void SetNotification(bool notification)
        {
            _notification = notification;
            Invalidate();
        }

        /// <summary>
        /// Gets or sets whether the control is in a selected state.
        /// </summary>
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Initializes a new instance of the VcpContentButton.
        /// </summary>
        public VcpContentButton()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            BackgroundImageLayout = ImageLayout.Stretch;
            ForeColor = Color.White;
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Text = "";
            Height = 44;
            Width = 44;
            Margin = new Padding(left: 0, top: 3, right: 0, bottom: 3);
            Cursor = Cursors.Hand;
            MouseEnter += MouseEnterEvent;
            MouseLeave += MouseLeaveEvent;
        }

        /// <summary>
        /// Handles the MouseEnter event to trigger a repaint.
        /// </summary>
        private void MouseEnterEvent(object? sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Handles the MouseLeave event to trigger a repaint.
        /// </summary>
        private void MouseLeaveEvent(object? sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Custom painting logic for notifications and selection highlights.
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (_notification)
            {
                pe.Graphics.FillEllipse(Brushes.Red, Width - 12, 5, 10, 10);
            }
            if (ClientRectangle.Contains(PointToClient(Cursor.Position)) && !_selected)
            {
                pe.Graphics.FillRectangle(Brushes.White, Width - 3, 8, 3, Height - 16);
            }
            if (_selected)
            {
                pe.Graphics.FillRectangle(new SolidBrush(Colors.AccentColor), Width - 3, 4, 3, Height - 8);
            }
        }
    }

    /// <summary>
    /// A specialized VcpContentButton that adds a text-based alert indicator.
    /// </summary>
    public partial class VcpSelectorButton : VcpContentButton
    {
        /// <summary>
        /// The text to display in the alert circle (e.g., "1").
        /// </summary>
        public string AlertText { get; set; }

        /// <summary>
        /// The background color of the alert circle.
        /// </summary>
        public Color AlertBackgroundColor { get; set; } = Color.CornflowerBlue;

        /// <summary>
        /// The foreground color of the alert text.
        /// </summary>
        public Color AlertForeColor { get; set; } = Color.White;

        /// <summary>
        /// Initializes a new instance of the VcpSelectorButton.
        /// </summary>
        public VcpSelectorButton()
        {
            InitializeComponent();
            AlertText = string.Empty;
        }

        /// <summary>
        /// Custom painting logic to draw the alert circle and text.
        /// </summary>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var indicatorSize = 18;
            var indicatorRectangle = new RectangleF(
                ClientRectangle.Width - indicatorSize * 1.25f,
                ClientRectangle.Height - indicatorSize * 1.5f,
                indicatorSize,
                indicatorSize
            );

            using var indicatorBrush = new SolidBrush(AlertBackgroundColor);
            pe.Graphics.FillEllipse(indicatorBrush, indicatorRectangle);
            using var brush = new SolidBrush(AlertForeColor);
            using var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoWrap,
                Trimming = StringTrimming.None
            };
            pe.Graphics.DrawString(AlertText, Font, brush, indicatorRectangle, format);
        }
    }
}