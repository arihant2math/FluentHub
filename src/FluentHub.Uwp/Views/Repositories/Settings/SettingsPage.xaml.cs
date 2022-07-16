﻿using FluentHub.Uwp.Services;
using FluentHub.Uwp.ViewModels.Repositories.Discussions;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace FluentHub.Uwp.Views.Repositories.Settings
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            var provider = App.Current.Services;
            navigationService = provider.GetRequiredService<INavigationService>();
        }

        private readonly INavigationService navigationService;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var url = e.Parameter as string;
            var uri = new Uri(url);
            var pathSegments = uri.AbsolutePath.Split("/").ToList();
            pathSegments.RemoveAt(0);

            var currentItem = navigationService.TabView.SelectedItem.NavigationHistory.CurrentItem;
            currentItem.Header = "Settings";
            currentItem.Description = "Settings";

            currentItem.Url = url;
            currentItem.DisplayUrl = $"{pathSegments[0]} / {pathSegments[1]} / Settings";

            currentItem.Url = $"{url}";
            currentItem.DisplayUrl = $"{pathSegments[0]} / {pathSegments[1]} / Settings";

            currentItem.Icon = new muxc.ImageIconSource
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Icons/Settings.png"))
            };
        }
    }
}