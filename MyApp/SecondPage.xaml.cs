using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SecondPage : Page
    {
        public SecondPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            ValueSet p = e.Parameter as ValueSet;
            if(p != null)
            {
                string name = p["name"].ToString();
                string city = p["city"].ToString();
                tb.Text = $"{name} + {city}";
            }
            
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + "的OnNavigatedTo方法被调用");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + "的OnNavigatedFrom方法被调用");
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + "的OnNavigatingFrom方法被调用");
        }

        private void OnClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
