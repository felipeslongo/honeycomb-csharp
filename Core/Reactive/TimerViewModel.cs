using HoneyComb.LiveDataNet;
using System;
using System.Reactive.Linq;

namespace Core.Reactive
{
    public class TimerViewModel : IDisposable
    {
        private readonly TimerLiveData _timer;

        public TimerViewModel(TimeSpan interval) : this(interval, timeSpan => timeSpan.ToString())
        {
        }
        
        public TimerViewModel(TimeSpan interval, Func<TimeSpan, string> formatter)
        {
            _timer = new TimerLiveData(interval);
            TimerUi = Transformations.Map(_timer, formatter);
        }
        
        public TimeSpan Interval {get => _timer.Interval; set => _timer.Interval = value;}
        
        public LiveData<TimeSpan> Timer => _timer;
        
        public LiveData<string> TimerUi { get; }
        
        public void Start() => _timer.Start();
        
        public void Stop() => _timer.Stop();
        
        public void Reset() => _timer.Reset();
        
        public void Dispose() => _timer.Dispose();
    }
}
