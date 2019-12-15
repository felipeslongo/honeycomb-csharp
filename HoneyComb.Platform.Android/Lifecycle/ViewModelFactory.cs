using Android.Arch.Lifecycle;
using Java.Lang;
using System;
using JavaObject = Java.Lang.Object;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Implementation of <see cref="ViewModelProvider.IFactory"/> interface
    /// that receives a <see cref="Func{ViewModel}"/>
    /// that is responsible to instantiate ViewModels.
    ///
    /// Removes the boilerplate of having to inherit an interface.
    /// </summary>
    /// <remarks>
    /// Credits:
    ///     https://proandroiddev.com/view-model-creation-in-android-android-architecture-components-kotlin-ce9f6b93a46b
    /// </remarks>
    /// <typeparam name="TViewModel">Type of <see cref="global::Android.Arch.Lifecycle.ViewModel"/></typeparam>
    public class ViewModelFactory<TViewModel> : JavaObject, ViewModelProvider.IFactory
        where TViewModel : ViewModel
    {
        private readonly Func<TViewModel> _creator;

        /// <summary>
        /// Creates with the creator delegate
        /// </summary>
        /// <param name="creator">Delegate responsable for creating the viewmodel with all its dependencies injected.</param>
        public ViewModelFactory(Func<TViewModel> creator)
        {
            _creator = creator;
        }

        public JavaObject Create(Class javaClass)
        {
            return _creator();
        }
    }
}
