using HoneyComb.Core.Vault;
using HoneyComb.LiveDataNet;
using System;
using System.Threading.Tasks;

namespace HoneyComb.UI
{
    /// <summary>
    ///     Represents the concept of an Visibility state
    ///     to be used in a ViewModel.
    /// </summary>
    public sealed class Visibility : IDisposable, IRestorableUIState
    {
        private readonly MutableLiveData<bool> value;

        public Visibility(bool isVisible)
        {
            value = new MutableLiveData<bool>(isVisible);
        }

        public bool IsInvisible => !IsVisible;
        public bool IsVisible => Value;
        public string? RestorationIdentifier { get; set; } = typeof(Visibility).FullName;
        public LiveData<bool> Value => value;

        public static implicit operator bool(Visibility @this) => @this.Value;

        public void Dispose()
        {
            Value.Dispose();
        }

        public void OnPreservation(IBundleCoder savedInstanceState)
        {
            if (string.IsNullOrWhiteSpace(RestorationIdentifier))
                return;

            savedInstanceState.Add(RestorationIdentifier!, Value);
        }

        public void OnRestoration(IBundleCoder savedInstanceState)
        {
            if (string.IsNullOrWhiteSpace(RestorationIdentifier))
                return;

            value.Value = savedInstanceState.GetBoolean(RestorationIdentifier!);
        }

        public void SetValue(bool isVisible) => value.Value = isVisible;

        public async Task WaitVisibilityAsync()
        {
            if (IsVisible)
                return;

            //await Value.Where(_ => IsVisible);
            var taskSource = new TaskCompletionSource<bool>();
            using var subscription = Value.BindMethod(() =>
            {
                if (IsInvisible)
                    return;

                taskSource.SetResult(true);
            });

            await taskSource.Task;
        }
    }
}
