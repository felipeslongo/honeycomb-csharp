using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Core.Reactive
{
    public class TimerViewModel : IDisposable
    {
        private readonly TimerLiveData _timer;

        public TimerViewModel(TimeSpan interval)
        {
            _timer = new TimerLiveData(interval);
            TimerUi = Transformations.Map(_timer, timeSpan => timeSpan.ToString());
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
