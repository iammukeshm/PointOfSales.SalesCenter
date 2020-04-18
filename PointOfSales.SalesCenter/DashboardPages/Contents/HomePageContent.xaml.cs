using PointOfSales.SalesCenter.Common;
using System.Windows;
using System.Windows.Controls;

namespace PointOfSales.SalesCenter.DashboardPages.Contents
{
    /// <summary>
    /// Interaction logic for WelcomePageContent.xaml
    /// </summary>
    public partial class HomePageContent : UserControl
    {
        public HomePageContent()
        {

            InitializeComponent();
            DataContext = new DC();
        }
        #region Title

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(HomePageContent),
            new PropertyMetadata(string.Empty));

        #endregion

        public class DC
        {
            public string welcomeTitle { get; set; }

            public DC()
            {
                welcomeTitle = $"Signed in as {SessionContext.Name}";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new SalesCenter
            {
                Owner = Window.GetWindow(this)
            };

            window.ShowDialog();
        }
    }
}
