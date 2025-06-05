using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SourceGit.Converters
{
    public static class BoolConverters
    {
        public static readonly FuncValueConverter<bool, double> ToPageTabWidth =
            new FuncValueConverter<bool, double>(x => x ? 200 : double.NaN);

        public static readonly FuncValueConverter<bool, FontWeight> IsBoldToFontWeight =
            new FuncValueConverter<bool, FontWeight>(x => x ? FontWeight.Bold : FontWeight.Normal);

        public static readonly FuncValueConverter<bool, Avalonia.Media.Imaging.BitmapInterpolationMode> DisableImageFiltering =
            new FuncValueConverter<bool, Avalonia.Media.Imaging.BitmapInterpolationMode>(x => x ? Avalonia.Media.Imaging.BitmapInterpolationMode.None : Avalonia.Media.Imaging.BitmapInterpolationMode.HighQuality);
    }
}
