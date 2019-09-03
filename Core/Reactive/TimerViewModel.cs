using System;
using System.Reactive.Disposables;

namespace Core.Reactive
{
    public class TimerViewModel : IDisposable
    {
        public TimerViewModel(TimeSpan interval)
        {
            
        }
        
        public TimeSpan Interval {get => throw new NotImplementedException(); set => throw new NotImplementedException();}
        public LiveData<TimeSpan> Timer => throw new NotImplementedException();
        public LiveData<string> TimerUi => throw new NotImplementedException();
        
        public void Start() => throw new NotImplementedException();
        public void Stop() => throw new NotImplementedException();
        public void Reset() => throw new NotImplementedException();
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
