using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using System.Reflection;
namespace sandbox.Module
{
    [ReactModule]
    internal class Flyout_Item
    {

        [ReactMethod]
        public async void SetUpCommon(string tagId, int[] fn, int id )
        {
            // setup here
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var currentContent = Window.Current.Content;
                var allChildren = FindAllControlsByTag(currentContent, tagId);
                var container = allChildren.FirstOrDefault() as Panel;
                if(container == null) return;
                container.ContextFlyout = createFlyout(fn, id);
               
               // UIElement element = context; // Reference to your UIElement
               // context.PointerPressed += PointerRightClick;
               // Create the Flyout
               //flyout = new Flyout();

                // Create a ContentControl to hold the UIElement
                //ContentControl contentControl = new ContentControl();
                // contentControl.Content = element;

                // Set the ContentControl as the Flyout content
                // flyout.Content = contentControl;
            });
        }

        private MenuFlyout createFlyout(int[] fn, int id)
        {
            MenuFlyout contextFlyout = new MenuFlyout();


            // Create a TextBlock for the ContextFlyout content
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Settings";
            textBlock.Margin = new Thickness(10, 0, 0, 0);
     
            // Add the MenuFlyoutItem to the ContextFlyout
            //contextFlyout.Items.Add(MenuCreate());
            foreach (var data in fn)
            {
                MenuFlyoutItem item1 = MenuCreate(data.ToString(), id);
                contextFlyout.Items.Add(item1);
            }
            //contextFlyout.Bak
            // Add the button to your layout (like a Grid)
            return contextFlyout;
        }


        private MenuFlyoutItem MenuCreate(string name, int executeFn)
        {
            // Create an Icon (using FontIcon in this case)
            FontIcon fontIcon = new FontIcon();
            fontIcon.Glyph = "\uE10F"; // Example Glyph (e.g., from Segoe MDL2 Assets)
            fontIcon.FontSize = 24;
            //Windows.UI.Xaml.Controls.Image itemImage = new Windows.UI.Xaml.Controls.Image
            //{
            //    Source = new BitmapImage(new Uri("ms-appx:///Assets/LockScreenLogo.scale - 200.png")), // Set your image source (replace with your image)
            //    Width = 16, // Set the width of the image
            //    Height = 16 // Set the height of the image
            //};


            // Create a MenuFlyoutItem to represent a flyout item
            MenuFlyoutItem item = new MenuFlyoutItem
            {
               // Icon = new BitmapImage(new Uri("ms-appx:///Assets/another_icon.png")), // Replace with your image path
                //Icon = new BitmapImage(new Uri("ms-appx:///Assets/LockScreenLogo.scale - 200.png")),
                Text = name,
            };
            item.Click += (s,v)=> {
                Submit(executeFn);
            };
            return item;
        }

        [ReactEvent]
        public Action<int> Submit { get; set; }

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
                    result.Add(child as Windows.UI.Xaml.UIElement);

                }

                // Recursively check in the child controls
                result.AddRange(FindAllControlsByTag(child, tag));
            }

            return result;
        }
    }
}
