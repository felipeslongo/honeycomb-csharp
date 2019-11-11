using Android.Widget;
using HoneyComb.Platform.Android.Content.Res;
using System;
namespace HoneyComb.Platform.Android.Widget
{
    public static class TextViewSetTextColorStateList
    {
        /// <summary>
        /// Sets the text color.
        /// </summary>
        /// <param name="this">Extended object.</param>
        /// <param name="colorResourceId">Color state list resource</param>
        public static void SetTextColorStateList(this TextView @this, int colorResourceId)
        {
            var color = ColorService.GetColorStateList(@this.Context, colorResourceId);
            @this.SetTextColor(color);
        }
    }
}
