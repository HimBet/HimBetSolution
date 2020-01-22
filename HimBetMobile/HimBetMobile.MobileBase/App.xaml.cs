﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HimBetMobile.Services;
using HimBetMobile.Views;

namespace HimBetMobile
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5001" : "http://localhost:5001";
        public static bool UseMockDataStore = false;

        public App()
        {
            InitializeComponent();

            /* if (UseMockDataStore)
                 DependencyService.Register<MockDataStore>();
             else
                 DependencyService.Register<AzureDataStore>();*/
            DependencyService.Register<PlayerDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
