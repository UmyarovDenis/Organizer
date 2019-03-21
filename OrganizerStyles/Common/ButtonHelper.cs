using System;
using System.Windows;
using System.Windows.Controls;

namespace OrganizerStyles.Common
{
    internal sealed class ButtonHelper
    {
        public static bool? GetDialogResult(DependencyObject obj)
        {
            return (bool?)obj.GetValue(DialogResultProperty);
        }
        public static void SetDialogResult(DependencyObject obj, bool? value)
        {
            obj.SetValue(DialogResultProperty, value);
        }
        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(ButtonHelper), new UIPropertyMetadata
        {
            PropertyChangedCallback = (obj, e) =>
            {
                Button button = obj as Button;

                if (button == null)
                {
                    throw new InvalidOperationException("Can only use ButtonHelper.DialogResult on a Button control");
                }

                button.Click += (sender, e2) =>
                {
                    Window.GetWindow(button).DialogResult = GetDialogResult(button);
                };
            }
        });
    }
}
