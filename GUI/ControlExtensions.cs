using System;
using System.Windows.Forms;

namespace MultiDisplayVCPClient.GUI // <-- Make sure this namespace matches your other GUI files
{
    public static class ControlExtensions
    {
        /// <summary>
        /// A helper method to safely update a UI control from a different thread.
        /// </summary>
        public static void InvokeIfRequired(this Control control, Action action)
        {
            // If the control's Invoke method is required (i.e., we're on a background thread),
            // use it to run the action.
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else // Otherwise, we're on the UI thread, so just run the action.
            {
                action.Invoke();
            }
        }
    }
}