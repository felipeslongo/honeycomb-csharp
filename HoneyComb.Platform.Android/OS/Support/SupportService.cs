using Android.Widget;

namespace HoneyComb.Platform.Android.OS.Support
{
    /// <summary>
    /// It provides flags that determine if an Android component feature
    /// is supported by the current device Android OS version.
    /// </summary>
    public static class SupportService
    {
        /// <summary>
        /// Gets SupportService for TextView.
        /// </summary>
        /// <param name="textView">TextView instance.</param>
        /// <returns></returns>
        public static TextViewSupportService GetSupport(TextView textView) => TextViewSupportService.Singleton;
    }
}
