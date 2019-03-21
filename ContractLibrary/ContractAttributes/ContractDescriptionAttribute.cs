using System;

namespace ContractLibrary.ContractAttributes
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ContractDescriptionAttribute : Attribute
    {
        public string ContractName { get; set; }
    }
}
