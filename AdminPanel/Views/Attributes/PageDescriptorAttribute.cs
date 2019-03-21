using System;

namespace AdminPanel.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class PageDescriptorAttribute : Attribute
    {
        public string Label { get; set; }
        public string ImageSource { get; set; }
    }
}
