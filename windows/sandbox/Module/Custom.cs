using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace sandbox.Module
{
    class SetUp
    {
        public string backgroundClr { get; set; }
        public string hoverClr { get; set; }
    }

    [ReactModule]
    internal sealed class Custom
    {
        //set-up
        public Color backgroundClr = Color.White;
        public Color hoverClr = Color.Green;
        public UIElement context;
        public ScrollViewer _context;
        private Color HexToColor(string hex)
        {
            // Remove any leading '#' symbol if present
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            // Ensure that hex is 6 or 8 characters long (for RGB or ARGB)
            if (hex.Length == 6)
            {
                // Parse the RGB values from the hex string
                byte r = Convert.ToByte(hex.Substring(0, 2), 16);
                byte g = Convert.ToByte(hex.Substring(2, 2), 16);
                byte b = Convert.ToByte(hex.Substring(4, 2), 16);
                return Color.FromArgb(255, r, g, b); // Default to fully opaque (Alpha = 255)
            }
            else if (hex.Length == 8)
            {
                // Parse the ARGB values from the hex string
                byte a = Convert.ToByte(hex.Substring(0, 2), 16);
                byte r = Convert.ToByte(hex.Substring(2, 2), 16);
                byte g = Convert.ToByte(hex.Substring(4, 2), 16);
                byte b = Convert.ToByte(hex.Substring(6, 2), 16);
                return Color.FromArgb(a, r, g, b);
            }
            else
            {
                throw new ArgumentException("Hex color string must be 6 or 8 characters long.");
            }
        }
        Flyout flyout;

        [ReactMethod]
        public async void SetUpCommon(string tagId) {
            // setup here
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var currentContent = Window.Current.Content;
                var allChildren = FindAllControlsByTag(currentContent, tagId);
                var item = allChildren.FirstOrDefault() as ViewPanel;
                if (item == null) return;
                bool state = false;
                item.PointerEntered += (s, v) =>
                {
                };
                item.PointerMoved += MyBorder_PointerMoved;

                item.PointerExited += (s, v) =>
                {
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
                };
            });
        }

        private void MyBorder_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var myGrid = (ViewPanel)sender;
            var data = e.GetCurrentPoint(sender as UIElement).Position;
            Point pointerPosition = new Point((int)data.X, (int)data.Y);

            double gridWidth = myGrid.ActualWidth;
            double gridHeight = myGrid.ActualHeight;

            double cornerMargin = 50;

            if ( pointerPosition.Y >= gridHeight - cornerMargin)
            {
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.UpArrow, 0);
            }
            else
            {
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            }
        }

        private void ScrollPress(object sender, PointerRoutedEventArgs e)
        {
            ScrollBar scrollBar = sender as ScrollBar;
            if (scrollBar != null)
            {
                if (scrollBar.Orientation == Orientation.Vertical)
                {
                    // Enable vertical scrolling
                    _context.ChangeView(_context.HorizontalOffset, scrollBar.Value, null);
                }
                else if (scrollBar.Orientation == Orientation.Horizontal)
                {
                    // Enable horizontal scrolling
                    _context.ChangeView(scrollBar.Value, _context.VerticalOffset, null);
                }
            }
        }

        private void DeactivateScroll(object sender, PointerRoutedEventArgs e)
        {
            //_context.IsEnabled = false;
            _context.VerticalScrollMode = ScrollMode.Disabled;
        }

        private void ActiveScroll(object sender, PointerRoutedEventArgs e)
        {
            _context.VerticalScrollMode = ScrollMode.Auto;
            ScrollBar scrollBar = sender as ScrollBar;
            //scrollBar.en
            //throw new NotImplementedException();
        }


        public T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // First we check if the parent itself is the type we're looking for
            T foundChild = parent as T;

            // If the parent is not of the requested type, look in the visual tree for a match
            if (foundChild == null)
            {
                int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    foundChild = FindChild<T>(child, childName);

                    if (foundChild != null)
                        break;
                }
            }

            return foundChild;
        }

        
        [ReactMethod]
        public async void Init(string tagId)
        {
            // Ensure code runs on the UI thread
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //int x = 0;
                //int result = 5 / x;  // This will throw a DivideByZeroException
                FindComponent(tagId);
            });
        }

        private void FindComponent(string tagId)
        {
            var currentContent = Window.Current.Content;
            var allChildren = FindAllControlsByTag(currentContent, tagId);
            var container = allChildren.First() as Microsoft.ReactNative.ViewPanel;
            
            //foreach (var control in allChildren)
            //{
            //    control.PointerEntered += PointerIn;
            //    control.PointerExited += PointerOut;
            //    control.PointerPressed += PointerRightClick;
            //}
        }

        private void PointerIn(object sender, PointerRoutedEventArgs e)
        {
            var view = sender as Microsoft.ReactNative.ViewPanel;
            view.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red);

        }

        private void PointerOut(object sender, PointerRoutedEventArgs e)
        {
            var view = sender as Microsoft.ReactNative.ViewPanel;
            view.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Yellow);
        }

        private void PointerRightClick(object sender, PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(sender as UIElement).Properties.IsRightButtonPressed)
            {
                Console.WriteLine("right click");
                var data = e.GetCurrentPoint(sender as UIElement).Position;
                Point pointerPosition = new Point((int)data.X, (int)data.Y);
                // Cast sender to Panel and access the panel's Children collection
                var panel = (Windows.UI.Xaml.Controls.Panel)sender;

                // Get the pointer position relative to the panel
                var pointerPositionPanel = e.GetCurrentPoint (panel);
                // Show the flyout at the pointer position
                //var children = ((Windows.UI.Xaml.Controls.Panel)sender).Children;
                //var containerPointer = e.GetCurrentPoint(container);
                //var x = containerPointer.RawPosition.X;
                //var y = containerPointer.RawPosition.Y;
                //Button newButton = new Button
                //{
                //    Content = "Right Clicked",
                //    Width = 100,
                //    Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red),
                //    Height = 50,
                //    HorizontalAlignment = HorizontalAlignment.Left,
                //    VerticalAlignment = VerticalAlignment.Top,
                //    Margin = new Windows.UI.Xaml.Thickness((int)x, (int)y, 0, 0) // Positioning the button
                //};

                //// Add the new button to the Panel's Children collection
                //container.Children.Add(newButton);

            }
        }

        private Flyout createFlyout() {
            // Create StackPanel content for Flyout
            UIElement content = new StackPanel
            {
                Background = new SolidColorBrush(Windows.UI.Colors.White),
                Padding = new Windows.UI.Xaml.Thickness(10), // Padding for inner content
                Children =
            {
                // TextBlock for "Copy"
                new TextBlock
                {
                    Text = "Copy",
                    Padding = new Windows.UI.Xaml.Thickness(10),
                    Foreground = new SolidColorBrush(Windows.UI.Colors.Black)
                },

                // Icon - use FontIcon for simplicity (you can replace with your own icon)
                new FontIcon
                {
                    Glyph = "\uE8C8", // Glyph for the copy icon (Clipboard icon)
                    FontSize = 24,
                    Foreground = new SolidColorBrush(Windows.UI.Colors.Black)
                }
            }
            };

            // Create the Flyout and set the content
            Flyout flyout = new Flyout
            {
                Content = content,
                FlyoutPresenterStyle = new Windows.UI.Xaml.Style(typeof(FlyoutPresenter))
                {
                    Setters =
                {
                    new Windows.UI.Xaml.Setter(FlyoutPresenter.BackgroundProperty, new SolidColorBrush(Windows.UI.Colors.White)),
                    new Windows.UI.Xaml.Setter(FlyoutPresenter.BorderBrushProperty, new SolidColorBrush(Windows.UI.Colors.Black)),
                    new Windows.UI.Xaml.Setter(FlyoutPresenter.BorderThicknessProperty, new Windows.UI.Xaml.Thickness(1)),
                    new Windows.UI.Xaml.Setter(FlyoutPresenter.CornerRadiusProperty, new Windows.UI.Xaml.CornerRadius(20))
                }
                }
            };
            return flyout;

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
                    result.Add(child as Windows.UI.Xaml.UIElement);
                
                }

                // Recursively check in the child controls
                result.AddRange(FindAllControlsByTag(child, tag));
            }

            return result;
        }
    }
}
