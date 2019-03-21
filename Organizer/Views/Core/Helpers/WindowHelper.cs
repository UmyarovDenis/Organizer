using System;
using System.Windows;
using System.Windows.Input;

namespace Organizer.Views.Core.Helpers
{
    internal static class WindowHelper
    {
        public static bool? GetDialogResult(DependencyObject obj)
        {
            return (bool?)obj.GetValue(DialogResultProperty);
        }
        public static void SetDialogResult(DependencyObject obj, bool? value)
        {
            obj.SetValue(DialogResultProperty, value);
        }
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(WindowHelper), new UIPropertyMetadata
        {
            PropertyChangedCallback = (obj, e) =>
            {
                Window window = obj as Window;

                if (window == null)
                {
                    throw new InvalidOperationException("Can only use Window.DialogResult on a Window control");
                }

                window.KeyUp += (sender, e2) =>
                {
                    if (e2.Key == Key.Enter)
                    {
                        Window.GetWindow(window).DialogResult = GetDialogResult(window);
                    }
                    else if (e2.Key == Key.Escape)
                    {
                        Window.GetWindow(window).DialogResult = false;
                    }
                };
            }
        });
    }
}
