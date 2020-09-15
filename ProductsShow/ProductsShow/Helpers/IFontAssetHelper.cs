using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShow.Helpers
{
    public interface IFontAssetHelper
    {
        SKTypeface GetSkiaTypefaceFromAssetFont(string fontName);
    }
}
