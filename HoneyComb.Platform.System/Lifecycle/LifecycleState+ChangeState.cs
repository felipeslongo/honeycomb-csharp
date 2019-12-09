using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoneyComb.Platform.SystemH.Lifecycle
{
    public static class LifecycleState_ChangeState
    {
        private static LifecycleState[] activeFromStates = new LifecycleState[] { LifecycleState.Initialized, LifecycleState.Inactive };

        /// <summary>
        /// Validates is the transition from the current state
        /// to the new state is valid.
        /// </summary>
        /// <param name="this">Current from state</param>
        /// <param name="toState">New to State</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If the transition is not valid.</exception>
        public static LifecycleState ChangeState(this LifecycleState @this, LifecycleState toState)
        {
            if (@this == toState)
                return toState;

            if (toState == LifecycleState.Initialized)
                throw CreateCannotChangeStateException(@this, toState);

            if (toState == LifecycleState.Active && !activeFromStates.Contains(@this))
                throw CreateCannotChangeStateException(@this, toState);

            if (toState == LifecycleState.Inactive && @this != LifecycleState.Active)
                throw CreateCannotChangeStateException(@this, toState);

            if (toState == LifecycleState.Disposed && @this != LifecycleState.Inactive)
                throw CreateCannotChangeStateException(@this, toState);

            if (@this == LifecycleState.Disposed)
                throw CreateCannotChangeStateException(@this, toState);

            return toState;
        }

        private static InvalidOperationException CreateCannotChangeStateException(LifecycleState fromState, LifecycleState toState) =>
            throw new InvalidOperationException($"Cannot change state from {fromState.ToString()} to {toState.ToString()}.");
    }
}
