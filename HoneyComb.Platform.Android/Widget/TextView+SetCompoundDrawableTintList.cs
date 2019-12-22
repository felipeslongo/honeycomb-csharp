using Android.Content.Res;
using Android.Widget;
using HoneyComb.Platform.Android.Content.Res;
using HoneyComb.Platform.Android.Core.Graphics.Drawables;
using HoneyComb.Platform.Android.OS;

namespace HoneyComb.Platform.Android.Widget
{
    public static class TextViewSetCompoundDrawableTintList
    {
        /// <summary>
        ///     Sets the left, right, top and bottom drawable tint.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="colorResourceId">Color state list resource</param>
        public static void SetCompoundDrawableTintList(this TextView @this, int colorResourceId)
        {
            var color = ColorService.GetColorStateList(@this.Context, colorResourceId);
            if (BuildVersionService.IsBelowApi23())
            {
                @this.SetCompoundDrawableTintListForApisBelow23(color);
                return;
            }

            @this.CompoundDrawableTintList = color;
        }

        /// <summary>
        ///     Sets the the left, right, top and bottom drawable tint for devices below API 23
        /// </summary>
        /// <remarks>
        ///     Credits
        ///     https://stackoverflow.com/questions/41588148/android-textview-drawabletint-on-pre-v23-devices
        /// </remarks>
        /// <param name="this"></param>
        /// <param name="color"></param>
        private static void SetCompoundDrawableTintListForApisBelow23(this TextView @this, ColorStateList color)
        {
            TextViewDrawables.ApplyAndSet(@this, drawable => DrawableService.CreateDrawableWithTint(drawable, color));
        }
    }
}
