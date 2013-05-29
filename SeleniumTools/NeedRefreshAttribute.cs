using System;

namespace Selenium.Tools
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NeedRefreshAttribute : Attribute
    { }
}
