using PointOfSales.SalesCenter.Common;
using System.Windows;
using System.Windows.Controls;

namespace PointOfSales.SalesCenter.DashboardPages.Contents
{
    /// <summary>
    /// Interaction logic for WelcomePageContent.xaml
    /// </summary>
    public partial class WelcomePageContent : UserControl
    {
        public WelcomePageContent()
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
            typeof(WelcomePageContent),
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

    }
}
