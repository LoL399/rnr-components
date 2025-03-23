using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.ReactNative;
using sandbox.Common;
using sandbox.Service;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Path = System.IO.Path;

namespace sandbox
{
    sealed partial class App : ReactApplication
    {
        public App()
        {
#if BUNDLE
            JavaScriptBundleFile = "index.windows";
            InstanceSettings.UseFastRefresh = false;
#else
            JavaScriptBundleFile = "index";
            InstanceSettings.UseFastRefresh = true;
#endif

#if DEBUG
            InstanceSettings.UseDirectDebugger = true;
            InstanceSettings.UseDeveloperSupport = true;
#else
            InstanceSettings.UseDirectDebugger = false;
            InstanceSettings.UseDeveloperSupport = false;
#endif

            Microsoft.ReactNative.Managed.AutolinkedNativeModules.RegisterAutolinkedNativeModulePackages(PackageProviders); // Includes any autolinked modules

            PackageProviders.Add(new ReactPackageProvider());
            LogService.Init();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Windows.UI.Xaml.Application.Current.UnhandledException += Current_UnhandledException;
            InitializeComponent();
        }

        private void crashLog(string lines)
        {
            string[] array = new string[10];
            Array.Fill(array, "=");
            string[] contentParticals = new string[]
            {
                string.Join(" ", array),
                DateTime.Now.ToString(),
                string.Join(" ", array)
            };
            string headerTime = string.Join(" ", contentParticals);
            File.AppendAllLines(LogService.filePath, new string[]{ headerTime, lines });
        }


        // Handles all unhandled exceptions from the application
        private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                // Log the exception or show a message (depending on your preference)
                crashLog(ex.ToString());
            }
        }

        // Handles unhandled exceptions specific to the UWP/WinUI framework
        private void Current_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            if (e.Exception is Exception ex)
            {
                crashLog(ex.ToString());
            }
            e.Handled = true;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(MainPage), e.Arguments);
        }

        /// <summary>
        /// Invoked when the application is activated by some means other than normal launching.
        /// </summary>
        protected override void OnActivated(Windows.ApplicationModel.Activation.IActivatedEventArgs e)
        {
            var preActivationContent = Window.Current.Content;
            base.OnActivated(e);
            if (preActivationContent == null && Window.Current != null)
            {
                // Display the initial content
                var frame = (Frame)Window.Current.Content;
                frame.Navigate(typeof(MainPage), null);
            }
        }

    }
}
