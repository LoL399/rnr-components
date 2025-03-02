using Microsoft.ReactNative;
using System;
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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace sandbox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var app = Application.Current as App;
            reactRootView.ReactNativeHost = app.Host;
        }

        public void FindTextBoxByTag()
        {
            // Traverse all children in the visual tree of the MainPage.
            var allChildren = FindAllControlsByTag(this, "MyTextBoxTag");

            foreach (var control in allChildren)
            {
                if (control is TextBox)
                {
                    //TextBox textBox = (TextBox)control;
                    //// You now have your TextBox and can access its properties
                    //textBox.Text = "Found and updated via Tag!";
                }
            }
        }

        private static IEnumerable<UIElement> FindAllControlsByTag(DependencyObject parent, string tag)
        {
            List<UIElement> result = new List<UIElement>();

            // Traverse the visual tree
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // Check if the child has the desired tag
                if (child is FrameworkElement element && element.Tag != null && element.Tag.ToString() == tag)
                {
                    //result.Add(child);
                }

                // Recursively check in the child controls
                result.AddRange(FindAllControlsByTag(child, tag));
            }

            return result;
        }

    }
}
