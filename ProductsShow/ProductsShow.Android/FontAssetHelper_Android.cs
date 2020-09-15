using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ProductsShow.Droid;
using ProductsShow.Helpers;
using SkiaSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(FontAssetHelper_Android))]
namespace ProductsShow.Droid
{
    class FontAssetHelper_Android : IFontAssetHelper
    {
        public SKTypeface GetSkiaTypefaceFromAssetFont(string fontName)
        {
            SKTypeface typeFace;

            using (var asset = Android.App.Application.Context.Assets.Open(fontName))
            {
                var fontStream = new MemoryStream();
                asset.CopyTo(fontStream);
                fontStream.Flush();
                fontStream.Position = 0;
                typeFace = SKTypeface.FromStream(fontStream);
            }

            return typeFace;
        }
    }
}