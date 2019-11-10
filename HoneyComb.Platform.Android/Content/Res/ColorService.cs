using Android.Content;
using Android.Content.Res;
using Android.Support.V4.Content;

namespace HoneyComb.Platform.Android.Content.Res
{
    /// <summary>
    /// Provide Color helpers and utils.
    /// </summary>
    public static class ColorService
    {
        /// <summary>
        /// Gets an <see cref="ColorStateList"/> from a color resource id.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="colorResourceId">Android color resource id.</param>
        /// <returns><see cref="ColorStateList"/> instance.</returns>
        /// <exception cref="System.Exception">
        /// when <paramref name="context"/> is null.
        /// when <paramref name="colorResourceId"/> is not found.
        /// when <paramref name="colorResourceId"/> is not a color resource.
        /// </exception>
        public static ColorStateList GetColorStateList(Context context, int colorResourceId) =>
            ContextCompat.GetColorStateList(context, colorResourceId);
    }
}
