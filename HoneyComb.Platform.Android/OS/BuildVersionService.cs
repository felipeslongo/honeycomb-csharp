using Android.OS;

namespace HoneyComb.Platform.Android.OS
{
    /// <summary>
    ///     Provides helpers and utilities to check the device running Android OS version.
    /// </summary>
    /// <remarks>
    ///     Credits
    ///     https://stackoverflow.com/questions/3093365/how-can-i-check-the-system-version-of-android
    /// </remarks>
    public static class BuildVersionService
    {
        /// <summary>
        ///     Is at least API 23 - marshmallow - Android 6.0
        /// </summary>
        /// <returns></returns>
        public static bool IsAtLeastApi23()
        {
            return Build.VERSION.SdkInt >= BuildVersionCodes.M;
        }

        /// <summary>
        ///     Is below API 23 - marshmallow - Android 6.0
        /// </summary>
        /// <returns></returns>
        public static bool IsBelowApi23()
        {
            return IsAtLeastApi23() == false;
        }

        public new static string ToString()
        {
            return $"Android SDK: {Build.VERSION.SdkInt} ({Build.VERSION.Release})";
        }
    }
}
