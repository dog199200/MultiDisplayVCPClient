namespace MultiDisplayVCPClient.GUI
{
    public static class ControlExtensions
    {
        /// <summary>
        /// A helper method to safely update a UI control from a different thread.
        /// </summary>
        public static void InvokeIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}