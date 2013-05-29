using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;

using NUnit.Framework;
using PageObjectFactory = Selenium.Tools.PageFactory;
using WebTest.TestCases;
using Quna.WebTest.TestFramework;
using System.Threading;

namespace WebTest.TestCases
{
    [TestFixture]
    public class BVT : QunaWebTestBase
    {
        [Test]
        [TestCase("北京", "天津", true)]
        public void OneWayTest(string cityFrom, string cityTo, bool needVerifyQvt)
        {
            InternalTestLogic(cityFrom, cityTo, needVerifyQvt, true);
        }

        [Test]
        [TestCase("北京", "西雅图", false)]
        public void OneWayTest2(string cityFrom, string cityTo, bool needVerifyQvt)
        {
            InternalTestLogic(cityFrom, cityTo, needVerifyQvt, false);
        }

        private void InternalTestLogic(string cityFrom, string CityTo, bool needVerifyQvt, bool isInland)
        {
            FirefoxDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://flight.qunar.com/");

            PageObjectFactory.UIMapFilePath = @"..\..\..\QunaWebTestFramework\UIMaps";

            HomePage homePage = PageObjectFactory.InitPage<HomePage>(driver);
            DateTime serverDaysAfter = DateTime.Now.AddDays(7);
            homePage.OneWaySearch(cityFrom, CityTo, serverDaysAfter.ToString("yyyy-MM-dd"));

            Thread.Sleep(1000*10);
            SearchResultPage searchResultPage = PageObjectFactory.InitPage<SearchResultPage>(driver);
            int maxRetryTimes = 0;
            while (searchResultPage.ResultInfo != null)
            {
                if (maxRetryTimes == 60)
                {
                    Assert.Fail("Fail to get search result!!");
                }

                Thread.Sleep(1000);
                try
                {
                    if (searchResultPage.ResultInfo.Text.Contains("搜索结束"))
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                }
                searchResultPage = PageObjectFactory.InitPage<SearchResultPage>(driver);
                maxRetryTimes++;
            }

            if (isInland)
            {
                searchResultPage.ClickOrderButtonForInland(0);
            }
            else
            {
                searchResultPage.ClickOrderButton(0);
            }

            if (needVerifyQvt)
            {
                Assert.IsTrue(searchResultPage.QvtWarning(0, isInland));
                Assert.IsTrue(searchResultPage.VerifyTransfer(0, isInland));
            }
            else
            {
                Assert.IsTrue(searchResultPage.VerifyPriceRange(0));
            }
            this.AddTestCleanup("Close Browser",
                           () => { driver.Close(); });
        }
    }
}
