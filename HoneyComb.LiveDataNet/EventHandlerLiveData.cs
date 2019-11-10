﻿using System;
using System.Threading;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Internal <see cref="LiveData{TEventArgs}"/> implementations
    /// that listens to a <see cref="EventHandler{TEventArgs}"/>.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
    internal class EventHandlerLiveData<TEventArgs> : LiveData<TEventArgs>
    {
        private readonly Action<EventHandler<TEventArgs>> addHandler;
        private readonly SynchronizationContext capturedContext = SynchronizationContext.Current;
        private readonly Action<EventHandler<TEventArgs>> removeHandler;

        public EventHandlerLiveData(
            Action<EventHandler<TEventArgs>> addHandler,
            Action<EventHandler<TEventArgs>> removeHandler
            )
        {
            this.addHandler = addHandler;
            this.removeHandler = removeHandler;
            Init();
        }

        public override void Dispose()
        {
            base.Dispose();
            UnsubscribeFromSourceEventHandler();
        }

        private void Init() => addHandler(SourceEventHandlerOnInvoke);

        private void SourceEventHandlerOnInvoke(object sender, TEventArgs args) => Value = args;

        private void UnsubscribeFromSourceEventHandler()
        {
            if (capturedContext != null)
            {
                capturedContext.Post(state => removeHandler(SourceEventHandlerOnInvoke), null);
                return;
            }
            removeHandler(SourceEventHandlerOnInvoke);
        }
    }
}