﻿using FluentHub.Uwp.Services;
using FluentHub.Uwp.ViewModels.Users;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace FluentHub.Uwp.Views.Users
{
    public sealed partial class DiscussionsPage : Page
    {
        public DiscussionsPage()
        {
            InitializeComponent();

            var provider = App.Current.Services;
            ViewModel = provider.GetRequiredService<DiscussionsViewModel>();
        }

        public DiscussionsViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as Models.FrameNavigationArgs;
            ViewModel.Login = param.Login;

            if (param.Parameters.ElementAtOrDefault(0) as string is "AsViewer")
            {
                ViewModel.DisplayTitle = true;
            }

            var command = ViewModel.LoadUserDiscussionsPageCommand;
            if (command.CanExecute(null))
                command.Execute(null);
        }
    }
}
