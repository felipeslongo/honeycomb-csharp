using System;
using System.Threading;

namespace HoneyComb.Core.Threading
{
    /// <summary>
    ///     Provides convenient factory methods to produce CancellationToken from common patterns.
    /// </summary>
    public class CancellationTokenFactory
    {
        /// <summary>
        ///     Produce an CancellationToken from a C# Event Pattern.
        ///     It will request cancellation and unsubscribe itself when the subscribed event is invoked.
        ///
        ///     WARNING: Use it without CancellationToken when you are sure that the subscribed
        ///     event will be invoked, otherwise the CancellationTokenSource will hang and be subscribed forever.
        ///     For a safer approach consider passing a CancellationToken.
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="subscribe"></param>
        /// <param name="unsubscribe"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static CancellationToken FromEvent<TEventArgs>(
            Action<EventHandler<TEventArgs>> subscribe,
            Action<EventHandler<TEventArgs>> unsubscribe,
            CancellationToken? token = null
        )
        {
            var tokenSource = new CancellationTokenSource();
            EventHandler<TEventArgs> eventHandler = (_, __) => { };
            eventHandler = (sender, args) =>
            {
                unsubscribe(eventHandler);
                tokenSource.Cancel();
            };
            
            token?.Register(() =>
            {
                unsubscribe(eventHandler);
                tokenSource.Cancel();
            });
            
            subscribe(eventHandler);
            return tokenSource.Token;
        }
    }
}
