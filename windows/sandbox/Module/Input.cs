using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReactNative.Managed;
using Windows.ApplicationModel.Core;
using Windows.Data.Json;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace sandbox.Module
{
    [ReactModule]
    class Input
    {
        [ReactMethod]
        public async void Init(string tagId)
        {
            // Ensure code runs on the UI thread
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                FindTextBoxByTag(tagId);
            });
        }

        private void FindTextBoxByTag(string tagId)
        {
            var currentContent = Window.Current.Content;
            var allChildren = FindAllControlsByTag(currentContent, tagId);

            foreach (var control in allChildren)
            {
                if (control is TextBox )
                {
                    var textbox = control as TextBox;
                    textbox.KeyUp += handleKeyDownProgramatical;
                    textbox.AcceptsReturn = true;
                }
            }
        }
        private void handleKeyDownProgramatical(object sender,
            Windows.UI.Xaml.Input.KeyRoutedEventArgs e) {
            var textbox = sender as TextBox;

            if (e.Key == VirtualKey.Enter &&
                !(Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift)
                .HasFlag(CoreVirtualKeyStates.Down)) && textbox.AcceptsReturn
                )
            {
                textbox.Focus(FocusState.Programmatic);
                var end = textbox.SelectionStart;
                var text = textbox.Text.ToString().Remove(end - 1,1);
                Submit(textbox.Tag.ToString());
                Console.WriteLine("here");
                Console.WriteLine(text);
            }
        }

        [ReactEvent]
        public Action<string> Submit { get; set; }

        private static IEnumerable<UIElement> FindAllControlsByTag(DependencyObject parent, string tagId)
        {
            List<UIElement> result = new List<UIElement>();

            // Traverse the visual tree recursively
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is FrameworkElement element && element.Tag != null && element.Tag.ToString() == tagId)
                {
                    result.Add(child as Windows.UI.Xaml.UIElement);
                }

                // Recursively find children
                result.AddRange(FindAllControlsByTag(child, tagId));
            }

            return result;
        }
    }
}
