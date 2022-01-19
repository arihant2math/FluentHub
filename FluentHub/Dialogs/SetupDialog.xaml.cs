﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;
using FluentHub.Views;
using FluentHub.Services.Auth;
using System.Threading.Tasks;
using FluentHub.ViewModels;


namespace FluentHub.Dialogs
{
    public sealed partial class SetupDialog : ContentDialog
    {
        public SetupDialog()
        {
            this.InitializeComponent();
            AuthOptionCombobox.Items.Add("Basic authentication");
            AuthOptionCombobox.Items.Add("OAuth token authentication");
            AuthOptionCombobox.Items.Add("Web authentication");
            AuthOptionCombobox.SelectedIndex = 2;
        }

        private void AuthOptionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BasicAuth.Visibility = Visibility.Collapsed;
            OAuthTokenAuth.Visibility = Visibility.Collapsed;
            ShouldNotUseBasicAuthWarningInfoBar.IsOpen = false;
            ShouldNotUseAccessTokenAuthWarningInfoBar.IsOpen = false;

            var selectedIndex = AuthOptionCombobox.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    BasicAuth.Visibility = Visibility.Visible;
                    ContinueButton.Content = "Authorize with basic loggin";
                    ShouldNotUseBasicAuthWarningInfoBar.IsOpen = true;

                    if (UsernameTextBox.Text != "" && PaddwordTextBox.Password != "")
                        ContinueButton.IsEnabled = true;
                    else
                        ContinueButton.IsEnabled = false;
                    break;

                case 1:
                    OAuthTokenAuth.Visibility = Visibility.Visible;
                    ContinueButton.Content = "Authorize with OAuth token";
                    ShouldNotUseAccessTokenAuthWarningInfoBar.IsOpen = true;

                    if (TokenTextBox.Text != "")
                        ContinueButton.IsEnabled = true;
                    else
                        ContinueButton.IsEnabled = false;
                    break;

                case 2:
                    ContinueButton.Content = "Open with your browser";
                    ContinueButton.IsEnabled = true;
                    break;
            }
        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UsernameTextBox.Text != "" && PaddwordTextBox.Password != "")
                ContinueButton.IsEnabled = true;
            else
                ContinueButton.IsEnabled = false;
        }

        private void PaddwordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text != "" && PaddwordTextBox.Password != "")
                ContinueButton.IsEnabled = true;
            else
                ContinueButton.IsEnabled = false;
        }

        private void TokenTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TokenTextBox.Text != "" && TokenTextBox.Text.Length == 40)
                ContinueButton.IsEnabled = true;
            else
                ContinueButton.IsEnabled = false;
        }

        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = AuthOptionCombobox.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    App.Client.Credentials = new Octokit.Credentials(UsernameTextBox.Text, PaddwordTextBox.Password);
                    App.settings.Set("username", UsernameTextBox.Text);
                    App.settings.Set("password", PaddwordTextBox.Password);
                    break;

                case 1:
                    App.Client.Credentials = new Octokit.Credentials(TokenTextBox.Text);
                    App.settings.Set("accessToken", TokenTextBox.Text);
                    break;

                case 2:
                    RequestAuthorization request = new RequestAuthorization();
                    await request.RequestGitHubIdentity();
                    this.Hide();
                    App.settings.SetupProgress = true;
                    return;
            }

            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
            this.Hide();
            App.settings.SetupCompleted = true;
            return;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}