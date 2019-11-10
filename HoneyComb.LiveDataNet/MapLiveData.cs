using System;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Internal <see cref="LiveData{TOut}"/> that observes changes to another <see cref="LiveData{TIn}"/>
    /// and updates itself with the new value using an <see cref="map"/> function.
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    internal class MapLiveData<TIn, TOut> : LiveData<TOut>
    {
        private readonly LiveData<TIn> source;
        private readonly Func<TIn, TOut> map;

        public MapLiveData(LiveData<TIn> source, Func<TIn, TOut> map)
        {
            this.source = source;
            this.map = map;
            Init();
        }

        private void Init()
        {
            source.PropertyChanged += SourceOnPropertyChanged;
            UpdateValue();
        }

        private void SourceOnPropertyChanged(object sender, EventArgs e) => UpdateValue();

        private void UpdateValue() => Value = map(source.Value);

        public override void Dispose()
        {
            base.Dispose();
            source.PropertyChanged -= SourceOnPropertyChanged;
        }
    }
}