using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSales.SalesCenter.Common
{
    public static class Dialog
    {
        public async static Task<ContentDialogResult> ConfirmarionContentDialog(string title,string content, string primaryText, string closeText )
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = closeText,
                PrimaryButtonText = primaryText,
                IsShadowEnabled = false
            };

            ContentDialogResult contentResult = await noWifiDialog.ShowAsync();
            return contentResult;
        }
        public async static Task<ContentDialogResult> InformationDialogResult(string title, string content)
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "Ok",
                IsShadowEnabled = false
            };

            ContentDialogResult contentResult = await noWifiDialog.ShowAsync();
            return contentResult;
        }
    }
}
