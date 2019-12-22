using Android.Widget;

namespace HoneyComb.Platform.Android.OS.Support
{
    /// <summary>
    /// It provides flags that determine if an TextView feature
    /// is supported by the current device Android OS version.
    /// </summary>
    public class TextViewSupportService
    {
        internal static TextViewSupportService Singleton = new TextViewSupportService();

        private TextViewSupportService()
        {
        }

        /// <summary>
        /// Gets if <see cref="TextView.CompoundDrawableTintList"/> setter is supported.
        /// </summary>
        /// <param name="textView">TextView instance</param>
        /// <returns>True if is supported, false otherwise</returns>
        public bool IsCompoundDrawableTintListSetterSupported(TextView textView) => BuildVersionService.IsAtLeastApi23();

        /// <summary>
        /// Gets if <see cref="TextView.CompoundDrawableTintList"/> setter is unsupported.
        /// </summary>
        /// <param name="textView">TextView instance</param>
        /// <returns>True if is unsupported, false otherwise</returns>
        public bool IsCompoundDrawableTintListSetterUnsupported(TextView textView) => BuildVersionService.IsBelowApi23();
    }
}
