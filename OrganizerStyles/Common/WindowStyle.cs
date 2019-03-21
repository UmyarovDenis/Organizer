using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace OrganizerStyles.Common
{
    internal static class LocalExtensions
    {
        public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
        {
            Window window = ((FrameworkElement)templateFrameworkElement).TemplatedParent as Window;
            if (window != null) action(window);
        }

        public static IntPtr GetWindowHandle(this Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            return helper.Handle;
        }
    }
    public partial class WindowStyle
    {
        void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ((Window)sender).StateChanged += WindowStateChanged;
            WindowStateChanged(sender, null);
        }
        void WindowStateChanged(object sender, EventArgs e)
        {
            var w = ((Window)sender);
            var handle = w.GetWindowHandle();
            var containerBorder = (Border)w.Template.FindName("PART_Container", w);

            if (w.WindowState == WindowState.Maximized)
            {
                var screen = System.Windows.Forms.Screen.FromHandle(handle);
                if (screen.Primary)
                {
                    containerBorder.Padding = new Thickness(
                        SystemParameters.WorkArea.Left + 7,
                        SystemParameters.WorkArea.Top + 7,
                        (SystemParameters.PrimaryScreenWidth - SystemParameters.WorkArea.Right) + 7,
                        (SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Bottom) + 5);
                }
            }
            else
            {
                containerBorder.Padding = new Thickness(7, 7, 7, 5);
            }
        }
        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => SystemCommands.CloseWindow(w));
        }
        void MinButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => SystemCommands.MinimizeWindow(w));
        }
        void MaxButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w =>
            {
                if (w.WindowState == WindowState.Maximized) SystemCommands.RestoreWindow(w);
                else SystemCommands.MaximizeWindow(w);
            });
        }
        void DragWindow(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.DragMove());
        }
    }
}
