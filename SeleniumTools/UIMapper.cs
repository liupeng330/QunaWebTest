using System;
using System.Linq;
using System.Reflection;

using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Selenium.Tools.xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Internal;

namespace Selenium.Tools
{
    public static class PageFactory
    {
        public static string UIMapFilePath { get; set; }

        private static bool DoesHasAttribute(PropertyInfo propertyInfo, IList<Type> attributes)
        {
            foreach (Type attribute in attributes)
            {
                var customAttributes = propertyInfo.GetCustomAttributes(attribute, true);
                if (customAttributes.Length != 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static Tpage ConstructPage<Tpage>(IWebDriver driver, IList<Type> ignoreAttributes, IList<Type> fetchAttributes) where Tpage : class
        { 
            Type type = typeof(Tpage);
            ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.HasThis,
                new Type[] { driver.GetType() },
                null);
            Tpage pageObject = constructorInfo.Invoke(new object[] { driver }) as Tpage;

            var properties = type.GetProperties(BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.ExactBinding |
                BindingFlags.SetProperty
                );

            foreach (var property in properties)
            {
                if (property.PropertyType.Name != "IWebElement" || DoesHasAttribute(property, ignoreAttributes))
                {
                    //Do not init webelement that marked as Ignore
                    continue;
                }
                //When fetchAttributes==null, all property without ignore attributes need to be set
                if (fetchAttributes == null || DoesHasAttribute(property, fetchAttributes))
                {
                    property.SetValue(
                        pageObject,
                        driver.FindElement(
                            ParseUIMaps(type.Name,
                                property.Name)),
                        null);
                }
            }
            return pageObject;
        }

        public static Tpage InitPage<Tpage>(IWebDriver driver) where Tpage : class
        {
            return ConstructPage<Tpage>(driver, new List<Type>() { typeof(IgnoreAttribute) }, null);
        }

        public static Tpage RefreshPage<Tpage>(IWebDriver driver) where Tpage : class
        {
            return ConstructPage<Tpage>(driver, new List<Type>() { typeof(IgnoreAttribute) }, new List<Type>() { typeof(NeedRefreshAttribute) });
        }

        private static By ParseUIMaps(string pageName, string uimapId)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Map));
            Map map = null;
            using (FileStream fileStream = new FileStream(Path.Combine(UIMapFilePath, pageName + ".xml"), FileMode.Open))
            {
                map = serializer.Deserialize(fileStream) as Map;
            }
            if (map == null)
            {
                throw new Exception("Fail to deserialize UIMap xml file!!");
            }

            var uimap = (from i in map.UIMaps where i.Id == uimapId select i).Single();
            return ConstructBy(uimap);
        }

        private static By ConstructBy(UIMap uimap)
        {
            By by = null;
            switch (uimap.By)
            {
                case "ClassName":
                    return By.ClassName(uimap.ToFind);
                case "CssSelector":
                    return By.CssSelector(uimap.ToFind);
                case "Id":
                    return By.Id(uimap.ToFind);
                case "LinkText":
                    return By.LinkText(uimap.ToFind);
                case "Name":
                    return By.Name(uimap.ToFind);
                case "PartialLinkText":
                    return By.PartialLinkText(uimap.ToFind);
                case "TagName":
                    return By.TagName(uimap.ToFind);
                case "XPath":
                    return By.XPath(uimap.ToFind);
            }
            return null;
        }
    }
}
