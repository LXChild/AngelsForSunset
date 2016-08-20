using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MyApp
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        public static bool IsHardwareButtonsAPIPresent;
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            IsHardwareButtonsAPIPresent = Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
            if (IsHardwareButtonsAPIPresent)
            {
                HardwareButtons.BackPressed += OnBackpressed;
            }
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {

//#if DEBUG
//            if (System.Diagnostics.Debugger.IsAttached)
//            {
//                this.DebugSettings.EnableFrameRateCounter = true;
//            }
//#endif
            //设置状态栏
            if(Util.IsRunningOnPhone())
            {
                StatusBar bar = StatusBar.GetForCurrentView();
                //bar.ForegroundColor = Colors.Black;
                bar.BackgroundColor = Colors.LightGray;
                bar.BackgroundOpacity = 100d;
                //await bar.HideAsync();
                await bar.ShowAsync();
            }

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();
                rootFrame.CacheSize = 3;
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated ||
                    e.PreviousExecutionState == ApplicationExecutionState.ClosedByUser)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                    //读出导航状态
                    var data = Windows.Storage.ApplicationData.Current.LocalSettings;
                    if (data.Values.ContainsKey("state"))
                    {
                        string state = data.Values["state"] as string;
                        rootFrame.SetNavigationState(state);
                    }
                }
                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

        private void OnBackpressed(object sender, BackPressedEventArgs e)
        {
            //获取Frame对象实例
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                //如果Frame对象可以进行后退操作
                //则应该组织时间传播到系统
                //并调用GoBack方法
                if (rootFrame.CanGoBack)
                {
                    e.Handled = true;
                    rootFrame.GoBack();
                }
            }
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            Frame root = Window.Current.Content as Frame;
            if(root != null)
            {
                //读出导航状态
                string state = root.GetNavigationState();
                //写入应用设置容器中
                var data = Windows.Storage.ApplicationData.Current.LocalSettings;
                data.Values["state"] = state;
            }
            deferral.Complete();
        }
    }
}
