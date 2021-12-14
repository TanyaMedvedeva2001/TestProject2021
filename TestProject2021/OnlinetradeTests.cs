using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace AutomatedTests
{
    public class OnlinetradeTests
    {
        IWebDriver driver;
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("https://www.onlinetrade.ru/");
        }

        [Test]
        public void TestPriceFilter()
        {
            driver.FindElement(By.CssSelector(".header__button")).Click();
            driver.FindElement(By.CssSelector(".mCM__item__link")).Click();
            driver.FindElement(By.CssSelector(".drawCats__item__image")).Click();
            driver.FindElement(By.CssSelector(".drawCats__item__image")).Click();
            driver.FindElement(By.XPath("//*[contains(text(), 'Ïîäîáðàòü ïî öåíå')]")).Click();
            driver.FindElement(By.Id("price1")).Clear();
            driver.FindElement(By.Id("price1")).SendKeys("10000");
            driver.FindElement(By.Id("price2")).Clear();
            driver.FindElement(By.Id("price2")).SendKeys("20000");
            driver.FindElement(By.Id("price2")).SendKeys(Keys.Enter);
            driver.FindElement(By.CssSelector(".js__filterResult_link")).Click();
            // Строка имеет вид ** ***Р, и чтобы из этого просто составить строку для парсинга вида ***** мы берем 0 и 1 символ и с 3 по 5
            int[] actualValues = Array.ConvertAll(driver.FindElements(By.CssSelector(".js__actualPrice"))
               .Select(webPrice => webPrice.Text.Trim()).ToArray(), s => int.Parse(s[0..^6] + s[3..^2]));
            actualValues.ToList().ForEach(actualPrice => Assert.True(actualPrice >= 10000 && actualPrice <= 20000, "Price filter works wrong. Actual price is " + actualPrice + ". But should be more or equal than 1000 and less or equal than 10000"));

        }
        [Test]
        public void TestTooltipText()
        {
            Assert.AreEqual("Êàòàëîã òîâàðîâ", driver.FindElement(By.CssSelector(".header__button")).GetAttribute("title"));

        }
        [Test]
        public void NegativeSignUpTest()
        {
            driver.FindElement(By.CssSelector(".huab__cell__link")).Click();
            driver.FindElement(By.CssSelector(".formLines__registerLink")).Click();
            driver.FindElement(By.Id("contact")).SendKeys("Anderson");
            driver.FindElement(By.Id("email")).SendKeys("best_producer@mail.ru");
            driver.FindElement(By.Id("account_myPasswordEdit_2ID")).SendKeys("abcdef12345");
            driver.FindElement(By.CssSelector(".button__orange ")).Click();
            Assert.IsTrue(driver.FindElements(By.CssSelector(".coloredMessage")).Any(),
                "Phone number confirmation button is enabel when phone number input has no value.");
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Quit();
        }
    }
}
