using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Support.V4.Graphics.Drawable;

namespace HoneyComb.Platform.Android.Core.Graphics.Drawables
{
    /// <summary>
    ///     Provide helpers and utils for handling Drawables.
    /// </summary>
    public static class DrawableService
    {
        /// <summary>
        ///     Creates a new Drawable with a ColorStateList applied to it as Tint.
        /// </summary>
        /// <remarks>
        ///     Credits:
        ///     https://stackoverflow.com/questions/26788251/android-tint-using-drawablecompat
        ///     https://stackoverflow.com/questions/30872101/drawablecompat-tinting-does-not-work-on-pre-lollipop
        ///     https://stackoverflow.com/questions/29155463/drawable-tinting-for-api-21
        /// </remarks>
        /// <param name="drawable">Original drawable to be tinted</param>
        /// <param name="color">Tint to be applied</param>
        /// <returns>New Drawable instance with tint applied</returns>
        public static Drawable CreateDrawableWithTint(Drawable drawable, ColorStateList color)
        {
            var drawableWrapper = DrawableCompat.Wrap(drawable).Mutate();
            DrawableCompat.SetTintList(drawableWrapper, color);
            return drawableWrapper;
        }
    }
}
