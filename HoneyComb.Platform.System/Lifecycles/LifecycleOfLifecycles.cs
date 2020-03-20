using System;

namespace HoneyComb.Core.Lifecycles
{
    public sealed class LifecycleOfLifecycles : Lifecycle
    {
        private Lifecycle? lifecycle;

        public LifecycleOfLifecycles(ILifecycleOwner owner) : base(owner)
        {
        }

        public Lifecycle? GetCurrentLifecycle()
        {
            return lifecycle;
        }

        public void SetLifecycle(Lifecycle lifecycle)
        {
            if (this.lifecycle != null)
                UnsubscribeFromLifecycleEvents(this.lifecycle);
            this.lifecycle = lifecycle;
            SubscribeToLifecycleEvents(this.lifecycle);
            SynchronizeUpToCurrentState(this.lifecycle.CurrentState);
        }

        public override void Dispose()
        {
            NotifyStateChange(LifecycleState.Disposed);
            if (lifecycle != null)
            {
                UnsubscribeFromLifecycleEvents(lifecycle);
                lifecycle = null;
            }

            base.Dispose();
        }

        private void SubscribeToLifecycleEvents(Lifecycle lifecycle)
        {
            lifecycle.OnActive += OnLifecycleActive;
            lifecycle.OnInactive += OnLifecycleInactive;
            lifecycle.OnDisposed += OnLifecycleDisposed;
        }

        private void UnsubscribeFromLifecycleEvents(Lifecycle lifecycle)
        {
            lifecycle.OnActive -= OnLifecycleActive;
            lifecycle.OnInactive -= OnLifecycleInactive;
            lifecycle.OnDisposed -= OnLifecycleDisposed;
        }

        private void OnLifecycleActive(object sender, EventArgs e)
        {
            if (lifecycle is null)
                return;
            SynchronizeUpToCurrentState(lifecycle.CurrentState);
        }

        private void OnLifecycleInactive(object sender, EventArgs e)
        {
            if (lifecycle is null)
                return;
            SynchronizeUpToCurrentState(lifecycle.CurrentState);
        }

        private void OnLifecycleDisposed(object sender, EventArgs e)
        {
            if (lifecycle is null)
                return;
            SynchronizeUpToCurrentState(lifecycle.CurrentState);
            lifecycle = null;
        }

        private void SynchronizeUpToCurrentState(LifecycleState state)
        {
            switch (state)
            {
                case LifecycleState.Initialized:
                case LifecycleState.Active:
                case LifecycleState.Inactive:
                    NotifyStateChange(state);
                    break;
                case LifecycleState.Disposed:
                    NotifyStateChange(LifecycleState.Inactive);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
