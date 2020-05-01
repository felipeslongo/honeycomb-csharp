namespace HoneyComb.Platform.Android.OS
{
    /// <summary>
    ///     Utility to help standardize the use case of detecting
    ///     if an Activity or Fragment is being or has been
    ///     destroyed by the OS for recreation.
    /// </summary>
    public sealed class DestroyedForRecreation
    {
        public HasBeenDestroyedForRecreation HasBeen { get; } = new HasBeenDestroyedForRecreation();
        public IsBeingDestroyedForRecreation IsBeing { get; } = new IsBeingDestroyedForRecreation();
    }
}
