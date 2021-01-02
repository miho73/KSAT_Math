using System;
using System.Diagnostics;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Kloc
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            MainPg.Background = new ImageBrush 
            { 
                ImageSource = new BitmapImage(new Uri(BaseUri, "Assets/bg.png")), 
                Stretch = Stretch.None 
            };

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Colors.White;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonInactiveBackgroundColor = Colors.White;
            titleBar.ButtonInactiveForegroundColor = Colors.Black;
            titleBar.ButtonPressedBackgroundColor = Colors.White;
            titleBar.ButtonPressedForegroundColor = Colors.Black;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            
            Window.Current.SetTitleBar(Base);

            Time.Text = limit.ToString(@"hh\:mm\:ss");
        }

        private readonly DateTime target = new DateTime(2021, 1, 1, 0, 0, 0);
        private TimeSpan limit = new TimeSpan(0, 100, 0);
        private DateTime Before;
        public async void UpdateTime()
        {
            Before = DateTime.Now;
            while (true)
            {
                //DateTime now = DateTime.Now;
                //TimeSpan offset = target.Subtract(now);
                await Time.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Time.Text = limit.ToString(@"hh\:mm\:ss");
                });
                if (limit.TotalSeconds == 0)
                {
                    await over.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        over.Text = "OVER";
                    });
                }
                limit = limit.Subtract(new TimeSpan(0, 0, 1));
                Thread.Sleep(1000);
            }
        }

        private void MainPg_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //Don't work...
            if(e.Key == VirtualKey.Escape)
            {
                Application.Current.Exit();
            }
        }
        private void Start(object senderm, RoutedEventArgs e)
        {
            Thread Updater = new Thread(new ThreadStart(UpdateTime));
            HideBtn.Begin();
            Updater.Start();
        }
    }
}
