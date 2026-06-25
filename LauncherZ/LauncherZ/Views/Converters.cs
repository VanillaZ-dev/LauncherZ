using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using LauncherZ.Models;

namespace LauncherZ.Views
{
    public class PingToColorConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is int ping)
            {
                string key = ping < 60 ? "PingGoodBrush" : ping < 120 ? "PingMidBrush" : "PingBadBrush";
                return Application.Current.Resources[key] as Brush ?? Brushes.Gray;
            }
            return Brushes.Gray;
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c) =>
            (v is int i ? i > 0 : v is true) ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }

    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c) =>
            (v is int i ? i > 0 : v is true) ? Visibility.Collapsed : Visibility.Visible;
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }

    public class PlayerCountToColorConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c) =>
            Application.Current.Resources["TextPrimaryBrush"] as Brush ?? Brushes.White;
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }

    public class BoolToFavColorConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c) =>
            v is true
                ? new SolidColorBrush(Color.FromRgb(0xe8, 0xa0, 0x20))
                : Application.Current.Resources["TextVeryDimBrush"] as Brush ?? Brushes.Gray;
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }

    public class TagBgConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is ServerInfo s)
            {
                string key = s.IsOfficial ? "TagOffBgBrush" : "TagModBgBrush";
                return Application.Current.Resources[key] as Brush ?? Brushes.Transparent;
            }
            return Brushes.Transparent;
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }

    public class TagBdrConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is ServerInfo s)
            {
                string key = s.IsOfficial ? "TagOffBdrBrush" : "TagModBdrBrush";
                return Application.Current.Resources[key] as Brush ?? Brushes.Transparent;
            }
            return Brushes.Transparent;
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }

    public class TagTxtConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is ServerInfo s)
            {
                string key = s.IsOfficial ? "TagOffTxtBrush" : "TagModTxtBrush";
                return Application.Current.Resources[key] as Brush ?? Brushes.White;
            }
            return Brushes.White;
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }

    public class TagLabelConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, CultureInfo c)
        {
            if (v is ServerInfo s)
                return s.IsOfficial ? "OFFICIAL" : "COMMUNITY";
            return string.Empty;
        }
        public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
    }
}
