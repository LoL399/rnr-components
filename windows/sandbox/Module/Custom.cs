﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
                context = allChildren.First();
                UIElement element = context; // Reference to your UIElement

                // Create the Flyout
                //flyout = new Flyout();

                // Create a ContentControl to hold the UIElement
                //ContentControl contentControl = new ContentControl();
                //contentControl.Content = element;

                // Set the ContentControl as the Flyout content
                //flyout.Content = contentControl;
            });
        }


        [ReactMethod]
        public async void Init(string tagId)
        {
            // Ensure code runs on the UI thread
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                FindComponent(tagId);
            });
        }

        private void FindComponent(string tagId)
        {
            var currentContent = Window.Current.Content;
            var allChildren = FindAllControlsByTag(currentContent, tagId);

            foreach (var control in allChildren)
            {
                control.PointerEntered += PointerIn;
                control.PointerExited += PointerOut;
                control.PointerPressed += PointerRightClick;
            }
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

                // Show the flyout at the pointer position
                flyout.ShowAt(sender as FrameworkElement);
                //(context as Viewbox).Set
                //context.
                //var flyout = createFlyout();
                //var pointerPosition = e.GetCurrentPoint(sender as UIElement).Position;
                //var flyoutTarget = new Viewbox()
                //{
                //    HorizontalAlignment = HorizontalAlignment.Left,
                //    VerticalAlignment = VerticalAlignment.Top,

                //};
                //flyoutTarget.Margin = new Windows.UI.Xaml.Thickness(pointerPosition.X, pointerPosition.Y, 0, 0);

                // Show the Flyout
                //flyout.ShowAt(sender as FrameworkElement);

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
