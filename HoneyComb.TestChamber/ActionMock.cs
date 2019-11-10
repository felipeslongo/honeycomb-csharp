using System;

namespace HoneyComb.TestChamber
{
    /// <summary>
    /// Simple object that Mocks a empty <see cref="Action"/> to help Assert invocations.
    /// </summary>
    public class ActionMock
    {
        public ActionMock()
        {
            MockedAction = () => Invocations++;
        }

        /// <summary>
        /// Empty <see cref="Action"/> that does nothing.
        /// </summary>
        public Action MockedAction;

        /// <summary>
        /// Gets the <see cref="MockedAction"/> invocation count
        /// </summary>
        public int Invocations { get; protected set; }

        /// <summary>
        /// Gets a <see cref="bool"/> that indicates if the <see cref="MockedAction"/> was invoked.
        /// </summary>
        public bool IsInvoked => Invocations > 0;
    }
}
