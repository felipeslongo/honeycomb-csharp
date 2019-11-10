using Android.Widget;
using HoneyComb.Platform.Android.Content.Res;
using System;
namespace HoneyComb.Platform.Android.Widget
{
    public static class TextViewSetTextColorStateList
    {
        public static void SetTextColorStateList(this TextView @this, int colorResourceId)
        {
            var color = ColorService.GetColorStateList(@this.Context, colorResourceId);
            @this.SetTextColor(color);
        }
    }
}
