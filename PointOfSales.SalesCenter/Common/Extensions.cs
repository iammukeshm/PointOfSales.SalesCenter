using ModernWpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PointOfSales.SalesCenter.Common
{
    public static class Extensions
    {
        public static void ToggleTheme(this FrameworkElement element)
        {
            ElementTheme newTheme;
            if (ThemeManager.GetActualTheme(element) == ElementTheme.Dark)
            {
                newTheme = ElementTheme.Light;
            }
            else
            {
                newTheme = ElementTheme.Dark;
            }
            //ThemeManager.Current.AccentColor = Colors.Red;
            ThemeManager.SetRequestedTheme(element, newTheme);
        }
        public static void TransferThemeToAnotherWindow(this FrameworkElement source, ref FrameworkElement target)
        {
            ElementTheme newTheme;
            if (ThemeManager.GetActualTheme(source) == ElementTheme.Dark)
            {
                newTheme = ElementTheme.Dark;
            }
            else
            {
                newTheme = ElementTheme.Light;
            }
            ThemeManager.SetRequestedTheme(target, newTheme);
        }
    }
}
