using ModernWpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace PointOfSales.SalesCenter.Common
{
    internal class ApplicationSettings
    {
        public static string Name { get; set; } = "mzeePOS";
        public static string ApiBaseAddress { get; set; } = "https://localhost:44348/";

        public static ElementTheme Theme { get; set; } = ElementTheme.Dark;
    }
}
