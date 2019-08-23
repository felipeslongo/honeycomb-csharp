using System;
using System.Reactive.Linq;
using System.Threading;

namespace Core.Reactive
{
    public class TimerViewModel : IDisposable
    {
        private readonly TimeSpan _interval;
        private IDisposable _timerSubscription;
        private MutableLiveData<TimeSpan> _elapsedTime = new MutableLiveData<TimeSpan>(TimeSpan.Zero);

        public LiveData<TimeSpan> ElapsedTime => _elapsedTime;

        public TimerViewModel(TimeSpan interval)
        {
            _interval = interval;
        }
        
        public void Start()
        {
            Reset();
            Stop();
            ObserveTimer();
        }

        public void Stop() => _timerSubscription?.Dispose();

        private void Reset() => _elapsedTime.Value = TimeSpan.Zero;

        private void ObserveTimer()
        {
            var observableTimer = Observable
                .Interval(_interval)
                .Select(iteration => iteration * _interval);

            if (SynchronizationContext.Current != null)
                observableTimer = observableTimer.ObserveOn(SynchronizationContext.Current);
            
            _timerSubscription = observableTimer
                .Subscribe(elapsedTime => _elapsedTime.Value = elapsedTime);
        }

        public void Dispose() => Stop();
    }
}
