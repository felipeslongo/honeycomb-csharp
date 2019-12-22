using System;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Widget;

namespace HoneyComb.Platform.Android.Widget
{
    /// <summary>
    ///     facilitates working with an TextView Compound Drawables.
    /// </summary>
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

        public bool HasTop => Top != null;
        public bool HasRight => Right != null;
        public bool HasLeft => Left != null;
        public bool HasBottom => Bottom != null;

        /// <summary>
        ///     Get all non null drawables.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drawable> GetAll()
        {
            if (HasLeft)
                yield return Left;

            if (HasTop)
                yield return Top;

            if (HasRight)
                yield return Right;

            if (HasBottom)
                yield return Bottom;
        }

        /// <summary>
        ///     Apply an transformation for each non null drawable.
        /// </summary>
        /// <param name="transformation">Transformation to be applied.</param>
        public void Apply(Action<Drawable> transformation)
        {
            foreach (var drawable in GetAll())
                transformation(drawable);
        }

        /// <summary>
        ///     Sets all drawables into the TextView.
        ///     Null will be passed as well.
        /// </summary>
        public void SetCompoundDrawables(TextView textView)
        {
            textView.SetCompoundDrawables(Left, Top, Right, Bottom);
        }

        /// <summary>
        ///     Creates a new instance using <see cref="TextView.GetCompoundDrawables" /> method.
        /// </summary>
        /// <param name="textView">TextView with drawables.</param>
        /// <returns></returns>
        public static TextViewDrawables CreateFromCompoundDrawables(TextView textView)
        {
            return new TextViewDrawables(textView.GetCompoundDrawables());
        }

        /// <summary>
        ///     Snippet for a combination of applying an transformation
        ///     for each non null drawable and set them back into the
        ///     textview.
        /// </summary>
        /// <param name="textView"></param>
        /// <param name="transformation"></param>
        public static void ApplyAndSet(TextView textView, Action<Drawable> transformation)
        {
            var drawables = CreateFromCompoundDrawables(textView);
            drawables.Apply(transformation);
            drawables.SetCompoundDrawables(textView);
        }
    }
}
