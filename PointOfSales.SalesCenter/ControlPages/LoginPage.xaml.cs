using CoreLibrary.Helpers;
using ModernWpf;
using ModernWpf.Controls;
using PointOfSales.SalesCenter.Application.DTOs;
using PointOfSales.SalesCenter.Application.Models.Account;
using PointOfSales.SalesCenter.Common;
using System;
using System.Windows;

namespace PointOfSales.SalesCenter.ControlPages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        ~LoginPage()
        {
        }
        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            loginRing.IsActive = true;
            var data = new LoginUserCommand { Email = email.Text, Password = password.Password };
            ApiHelper api = new ApiHelper(ApplicationSettings.ApiBaseAddress);
            try
            {
                var result = await api.PostAsync<Result<LoggedInUser>>("api/Account/login", data);
                if (result.Succeeded)
                {

                    var accessToken = result.Data.Token;
                    api.AddJwtAuthorization(accessToken);
                    //await Dialog.InformationDialogResult("Success!", result.Messages[0]);

                    SessionContext.Authenticate(result.Data, accessToken);
                    Dashboard dashboard = new Dashboard();
                    //dashboard.header.Text = $"welcome, {email.Text}";
                    dashboard.Show();
                    var myWindow = Window.GetWindow(this); 
                    myWindow.Close();

                    this.SwitchTheme(this, ref dashboard);


                }
                else
                {
                    await Dialog.InformationDialog("Failed!", result.Messages[0]);
                    
                }
            }
            catch (Exception ex)
            {

                await Dialog.InformationDialog("Exception!", ex.Message);
            }
            finally
            {
                loginButton.IsEnabled = true;
                loginRing.IsActive = false;
            }
           
        }

        private void SwitchTheme(LoginPage loginPage,ref Dashboard dashboard)
        {
            ElementTheme newTheme;
            if (ThemeManager.GetActualTheme(loginPage) == ElementTheme.Dark)
            {
                newTheme = ElementTheme.Dark;
            }
            else
            {
                newTheme = ElementTheme.Light;
            }
            ThemeManager.SetRequestedTheme(dashboard, newTheme);
        }
    }
}
