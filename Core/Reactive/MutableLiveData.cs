namespace Core.Reactive
{
    /// <summary>
    /// <see cref="LiveData{T}"/> which publicly exposes mutability
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

        public new void PostValue(T value) => base.PostValue(value);
    }
}