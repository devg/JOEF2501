﻿using ChatBot.Clients.Core.Services.Navigation;
using ChatBot.Clients.Core.ViewModels;
using ChatBot.Clients.Core.ViewModels.Base;
using ChatBot.Clients.Core.Views;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChatBot.Clients.Core
{
    public partial class App : Application
    {
        #region Properties
        public static DocumentClient _clients;
        public static Uri _collectionLink;
        public static DocumentClient Clients
        {
            get
            {
                if (_clients == null)
                {
                    _clients = new DocumentClient(new Uri(AppSettings.EndpointUri), AppSettings.PrimaryKey);
                }
                return _clients;
            }
        }
        public static Uri CollectionLink
        {
            get
            {
                if (_collectionLink == null)
                {
                    _collectionLink = UriFactory.CreateDocumentCollectionUri(AppSettings.DatabaseName, AppSettings.CollectionName);
                }
                return _collectionLink;
            }
        }


        private static Locator _locator;

        public static Locator Locator
        {
            get { return _locator = _locator ?? new Locator(); }
        }
        #endregion

        static App()
        {
            BuildDependencies();
        }

        public App()
        {
            InitializeComponent();
            InitNavigation();

        }

        public static void BuildDependencies()
        {
            Locator.Instance.Build();
        }

        private Task InitNavigation()
        {
            var navigationService = Locator.Instance.Resolve<INavigationService>();
            return navigationService.NavigateToAsync<ExtendedSplashViewModel>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
