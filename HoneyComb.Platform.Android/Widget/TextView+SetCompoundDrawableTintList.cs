using Android.Widget;
using HoneyComb.Platform.Android.Content.Res;

namespace HoneyComb.Platform.Android.Widget
{
    public static class TextViewSetCompoundDrawableTintList
    {
        /// <summary>
        /// Sets the left, right, top and bottom drawable tint.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="colorResourceId">Color state list resource</param>
        public static void SetCompoundDrawableTintList(this TextView @this, int colorResourceId) =>
            @this.CompoundDrawableTintList = ColorService.GetColorStateList(@this.Context, colorResourceId);
    }
}
