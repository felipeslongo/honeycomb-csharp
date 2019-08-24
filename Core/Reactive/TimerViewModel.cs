using System;
using System.Reactive.Disposables;

namespace Core.Reactive
{
    public class TimerViewModel : IDisposable
    {
        private readonly TimeSpan _interval;
        private IDisposable _timerSubscription;
        private MutableLiveData<TimeSpan> _elapsedTime = new MutableLiveData<TimeSpan>(TimeSpan.Zero);

        public LiveData<TimeSpan> ElapsedTime => _elapsedTime;
        public int Ticks { get; private set; } = 0;

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

        private void Reset()
        {
            _elapsedTime.Value = TimeSpan.Zero;
            Ticks = 0;
        }

        private void ObserveTimer()
        {
            var timer = new System.Timers.Timer(_interval.TotalMilliseconds)
            {
                AutoReset = true
            };
            timer.Elapsed += TimerOnElapsed;
            timer.Start();

            _timerSubscription = Disposable.Create(() =>
            {
                timer.Elapsed -= TimerOnElapsed;
                timer.Stop();
                timer.Dispose();
            });
        }

        private void TimerOnElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _elapsedTime.Value = ElapsedTime.Value.Add(_interval);
            Ticks++;
        }

        public void Dispose() => Stop();
    }
}
