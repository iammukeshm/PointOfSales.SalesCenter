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
        public async static Task<ContentDialogResult> InformationDialog(string title, string content)
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
        public async static Task<ContentDialogResult> UserNotAuthorizedDialog()
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "Not Authorized!",
                Content = $"You are not authorized to view this page! Please contact the Administrator.",
                CloseButtonText = "Ok",
                IsShadowEnabled = false
            };

            ContentDialogResult contentResult = await noWifiDialog.ShowAsync();
            return contentResult;
        }
        public async static Task<ContentDialogResult> UserNotAuthorizedToUseApiEndpointDialog(string endPoint)
        {
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "Not Authorized!",
                Content = $"You are not authorized to access the API Endpoint - {endPoint} ! Please contact the Administrator.",
                CloseButtonText = "Ok",
                IsShadowEnabled = false
            };

            ContentDialogResult contentResult = await noWifiDialog.ShowAsync();
            return contentResult;
        }
    }
}
