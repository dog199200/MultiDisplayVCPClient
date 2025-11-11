using SuchByte.MacroDeck.Logging;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI
{
    public static class TableLayoutHelper
    {
        public static void RemoveArbitraryRow(TableLayoutPanel panel, int rowIndex)
        {
            if (rowIndex >= panel.RowCount)
            {
                MacroDeckLogger.Warning(PluginInstance.Main, $"Attempted to remove invalid row index {rowIndex}. RowCount is {panel.RowCount}.");
                return;
            }

            for (int i = 0; i < panel.ColumnCount; i++)
            {
                Control control = panel.GetControlFromPosition(i, rowIndex);
                panel.Controls.Remove(control);
            }

            for (int i = rowIndex + 1; i < panel.RowCount; i++)
            {
                for (int j = 0; j < panel.ColumnCount; j++)
                {
                    Control control = panel.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        panel.SetRow(control, i - 1);
                    }
                }
            }

            panel.RowCount--;
            MacroDeckLogger.Info(PluginInstance.Main, $"Successfully removed row {rowIndex} from TableLayoutPanel.");
        }
    }
}