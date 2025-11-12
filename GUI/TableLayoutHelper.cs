using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI
{
    /// <summary>
    /// Provides helper methods for manipulating TableLayoutPanel controls.
    /// </summary>
    public static class TableLayoutHelper
    {
        /// <summary>
        /// Removes a specific row from a TableLayoutPanel and shifts all subsequent rows up.
        /// </summary>
        /// <param name="panel">The TableLayoutPanel to modify.</param>
        /// <param name="rowIndex">The index of the row to remove.</param>
        public static void RemoveArbitraryRow(TableLayoutPanel panel, int rowIndex)
        {
            if (rowIndex >= panel.RowCount)
            {
                return;
            }

            for (int i = 0; i < panel.ColumnCount; i++)
            {
                Control? control = panel.GetControlFromPosition(i, rowIndex);
                if (control != null)
                {
                    panel.Controls.Remove(control);
                    control.Dispose();
                }
            }

            for (int i = rowIndex + 1; i < panel.RowCount; i++)
            {
                for (int j = 0; j < panel.ColumnCount; j++)
                {
                    Control? control = panel.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        panel.SetRow(control, i - 1);
                    }
                }
            }

            if (panel.RowStyles.Count > rowIndex)
            {
                panel.RowStyles.RemoveAt(rowIndex);
            }

            panel.RowCount--;
        }
    }
}