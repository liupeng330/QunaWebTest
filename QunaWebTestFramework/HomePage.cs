using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Quna.WebTest.TestFramework
{
    public class HomePage : PageBase
    {
        public IWebElement OneWayRadioButton { get; set; }
        public IWebElement CityFromInput { get; set; }
        public IWebElement CityToInput { get; set; }
        public IWebElement DepartureTimeInput { get; set; }
        public IWebElement SearchButton { get; set; }

        public HomePage(IWebDriver driver)
        {
            this.webDriver = driver;
        }

        public void OneWaySearch(string cityFrom, string cityTo, string departureTime)
        {
            this.OneWayRadioButton.Click();

            this.CityFromInput.Clear();
            this.CityFromInput.SendKeys(cityFrom);

            this.CityToInput.Clear();
            this.CityToInput.SendKeys(cityTo);

            this.DepartureTimeInput.Clear();
            this.DepartureTimeInput.SendKeys(departureTime);

            this.SearchButton.Submit();
        }
    }
}
