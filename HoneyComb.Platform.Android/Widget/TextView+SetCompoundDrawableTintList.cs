using Android.Widget;
using HoneyComb.Platform.Android.Content.Res;

namespace HoneyComb.Platform.Android.Widget
{
    public static class TextViewSetCompoundDrawableTintList
    {
        public static void SetCompoundDrawableTintList(this TextView @this, int colorResourceId) =>
            @this.CompoundDrawableTintList = ColorService.GetColorStateList(@this.Context, colorResourceId);
    }
}
