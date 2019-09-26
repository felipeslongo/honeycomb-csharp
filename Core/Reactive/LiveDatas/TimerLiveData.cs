using System;
using System.Timers;

namespace Core.Reactive.LiveDatas
{
    /// <summary>
    /// Generates recurring events after each interval tick.
    /// </summary>
    public class TimerLiveData : LiveData<TimeSpan>
    {
        private static readonly TimeSpan zero = TimeSpan.Zero;
        private Timer _timer;

        public TimerLiveData(TimeSpan interval) : base(zero)
        {
            Init();
            Interval = interval;
        }

        private void Init()
        {
            _timer = new Timer()
            {
                AutoReset = true,
            };
            _timer.Elapsed += TimerOnElapsed;
        }

        public TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(_timer.Interval);
            set => _timer.Interval = value.TotalMilliseconds;
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        public void Reset()
        {
            Stop();
            Value = zero;
            Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e) => Value = Value.Add(Interval);

        public override void Dispose()
        {
            Stop();
            base.Dispose();
            _timer.Dispose();
            _timer = null;
        }
    }
}
