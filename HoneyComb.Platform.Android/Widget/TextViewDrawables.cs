using Android.Graphics.Drawables;
using Android.Widget;

namespace HoneyComb.Platform.Android.Widget
{
    public sealed class TextViewDrawables
    {
        private TextViewDrawables(Drawable[] drawables)
        {
            Left = drawables[0];
            Top = drawables[1];
            Right = drawables[2];
            Bottom = drawables[3];
        }

        public Drawable Top { get; }
        public Drawable Right { get; }
        public Drawable Left { get; }
        public Drawable Bottom { get; }

        public static TextViewDrawables CreateFromCompoundDrawables(TextView textView)
        {
            return new TextViewDrawables(textView.GetCompoundDrawables());
        }
    }
}
