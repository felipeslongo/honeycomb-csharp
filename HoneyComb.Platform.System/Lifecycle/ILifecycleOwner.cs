namespace HoneyComb.Platform.SystemH.Lifecycle
{
    /// <summary>
    /// A class that has an generic lifecycle.
    /// These events can be used by custom components
    /// to handle lifecycle changes without implementing
    /// any code inside the rue owner.
    /// </summary>
    /// <remarks>
    /// Credits:
    ///     https://developer.android.com/reference/androidx/lifecycle/LifecycleOwner.html
    /// </remarks>
    public interface ILifecycleOwner
    {
        Lifecycle Lifecycle { get; }
    }
}
