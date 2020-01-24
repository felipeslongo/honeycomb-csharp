using System;

namespace HoneyComb.Core.Lifecycles
{
    /// <summary>
    ///     Provides advanced lifecycle usecases.
    /// </summary>
    public sealed class LifecycleAdvanced
    {
        internal LifecycleAdvanced(Lifecycle lifecycle)
        {
            SubscribeToLifecycleEvents(lifecycle);
        }

        /// <summary>
        ///     Invoked when the Lifecycle is in Active state again.
        /// </summary>
        public event EventHandler<EventArgs>? OnActiveAgain;

        /// <summary>
        ///     Invoked when the Lifecycle is in Inactive state again.
        /// </summary>
        public event EventHandler<EventArgs>? OnInactiveAgain;

        private void SubscribeToLifecycleEvents(Lifecycle lifecycle)
        {
            var onActiveAgain = false;
            var onInactiveAgain = false;

            lifecycle.OnActive += Lifecycle_OnActive;
            lifecycle.OnInactive += Lifecycle_OnInactive;

            void Lifecycle_OnActive(object sender, EventArgs e)
            {
                if (onActiveAgain == false)
                {
                    onActiveAgain = true;
                    return;
                }

                OnActiveAgain?.Invoke(sender, e);
            }

            void Lifecycle_OnInactive(object sender, EventArgs e)
            {
                if (onInactiveAgain == false)
                {
                    onInactiveAgain = true;
                    return;
                }

                OnInactiveAgain?.Invoke(sender, e);
            }
        }
    }
}
