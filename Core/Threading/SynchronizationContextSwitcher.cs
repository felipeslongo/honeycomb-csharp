using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Core.Threading
{
    /// <summary>
    /// Helper that capture and replaces the <see cref="SynchronizationContext.Current"/>,
    /// then executes an block of code with the new <see cref="SynchronizationContext"/>
    /// and finally restore the original <see cref="SynchronizationContext.Current"/>
    /// </summary>
    public static class SynchronizationContextSwitcher
    {
        public static void RunWithoutContext(Action action) => RunWithContext(null, action);

        public static void RunWithContext(SynchronizationContext context, Action action)
        {
            var capturedContext = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(context);
                action();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(capturedContext);
            }
        }
    }
}
