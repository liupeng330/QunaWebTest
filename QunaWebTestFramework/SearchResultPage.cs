using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Selenium.Tools;

namespace Quna.WebTest.TestFramework
{
    public class SearchResultPage : PageBase
    {
        public IWebElement ResultInfo { get; set; }
        public IWebElement ResultTable { get; set; }

        public SearchResultPage(IWebDriver driver)
        {
            this.webDriver = driver;
        }

        private IEnumerable<IWebElement> GetResultElements()
        {
            return this.ResultTable.FindElements(By.TagName("table"));
        }

        private IEnumerable<IWebElement> GetResultElementsForInland()
        {
            return this.ResultTable.FindElements(By.TagName("div"));
        }

        public void ClickOrderButton(int lineNumber)
        {
            IWebElement ret = GetResultElements().ToArray()[lineNumber];
            IWebElement button = ret.
                FindElement(By.XPath("//td[@class='m3']")).
                FindElement(By.TagName("a"));
            button.Click();
        }

        public void ClickOrderButtonForInland(int lineNumber)
        {
            IWebElement ret = GetResultElementsForInland().ToArray()[lineNumber];
            IWebElement button = ret.
                FindElement(By.XPath("//div[@class='a_booking']")).
                FindElement(By.TagName("a"));
            button.Click();
        }

        public bool QvtWarning(int lineNumber, bool isInland)
        {
            IWebElement ret;
            if (isInland)
            {
                ret = GetResultElementsForInland().ToArray()[lineNumber];
            }
            else
            { 
                ret = GetResultElements().ToArray()[lineNumber];
            }
            IWebElement warn;
            try
            {
                warn = ret.FindElement(By.XPath("//div[@class='e_qvt_warn']"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                return false;
            }
            return warn.Text.Contains("需分别缴纳税费");
        }

        public bool VerifyTransfer(int lineNumber, bool isInland)
        { 
            IWebElement ret;
            if (isInland)
            {
                ret = GetResultElementsForInland().ToArray()[lineNumber];
            }
            else
            { 
                ret = GetResultElements().ToArray()[lineNumber];
            }

            var routers = ret.
                FindElement(By.XPath("//div[@class='b_qvt_lst']")).
                FindElements(By.XPath("//div[@class='e_qvt_route']"));
            if (routers.Count != 2)
            {
                return false;
            }
            return routers[0].Text.Contains("第一程") && routers[1].Text.Contains("第二程");
        }

        public bool VerifyPriceRange(int lineNumber)
        { 
            IWebElement ret = GetResultElements().ToArray()[lineNumber];
            IWebElement price; 
            try
            {
                price = ret.FindElement(By.XPath("//div[@class='lnk_more']"));
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                return false;
            }
            return price.Text.Contains("报价范围");
        }
    }
}
