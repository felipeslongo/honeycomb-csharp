namespace Core.Reactive
{
    /// <summary>
    /// <see cref="LiveData{T}"/> which publicly exposes setters
    /// </summary>
    /// <see cref="https://developer.android.com/reference/androidx/lifecycle/MutableLiveData.html"/>
    public class MutableLiveData<T> : LiveData<T>
    {
        public MutableLiveData(T value) : base(value)
        {
        }

        public MutableLiveData() : base()
        {
        }

        public new T Value 
        { 
            get => base.Value;
            set => base.Value = value;
        }
    }
}