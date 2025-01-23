using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using RollerUITest.Utils;

namespace RollerUITest
{
    public class BasePage
    {
        public static IWebDriver driver;
        public static string pathWithFileNameExtension;
        public static string dirpath;
        public static ExtentReports extentReports;
        public static ExtentTest Test;
        public static ExtentTest Step;

        public static void SeleniumInit(string browser)
        {
            try
            {
                // Log initialization
                Test = extentReports.CreateTest("Selenium Initialization");

                if (driver == null)
                {
                    if (browser.Equals("Chrome", StringComparison.OrdinalIgnoreCase))
                    {
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AddArgument("--start-maximized"); // Maximizes browser on startup
                        chromeOptions.AddArgument("--disable-notifications"); // Disables notifications
                        chromeOptions.AddArgument("--remote-allow-origins=*"); // Resolves specific WebDriver issues

                        driver = new ChromeDriver(chromeOptions);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                        Test.Log(Status.Pass, "Chrome driver initialized successfully.");
                    }
                    else
                    {
                        string errorMessage = $"Unsupported browser: {browser}";
                        Test.Log(Status.Fail, errorMessage);
                        throw new ArgumentException(errorMessage);
                    }
                }
                else
                {
                    string errorMessage = "Driver is already initialized.";
                    Test.Log(Status.Fail, errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                string errorDetails = $"Error initializing Selenium: {ex.Message}";
                Test.Log(Status.Fail, errorDetails);
                throw;
            }
        }

        public static void SeleniumCleanup()
        {
            if (driver != null)
            {
                driver.Close();
                driver = null;
            }
        }

        public static void Click(By by, string comment)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(by));
                element.Click();
                TakeScreenShot(Status.Pass, comment);
            }
            catch (Exception e)
            {
                TakeScreenShot(Status.Fail, "Failed to clicked: " + e);
                throw;
            }
        }

        public static void Write(By by, string txt, string comment)
        {
            try
            {
                driver.FindElement(by).SendKeys(txt);
                TakeScreenShot(Status.Pass, comment);
            }
            catch (Exception e)
            {
                TakeScreenShot(Status.Fail, "Failed to write: " + e);
                throw;
            }
        }

        public static void ClearAndWrite(By by, string txt, string comment)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement element = wait.Until(ExpectedConditions.ElementExists(by));
                element.Clear();
                element.SendKeys(txt);
                TakeScreenShot(Status.Pass, comment);
            }
            catch (Exception e)
            {
                TakeScreenShot(Status.Fail, "Failed to clear and write: " + e);
                throw;
            }
        }

        public static void AssertAreEqual(string expected, By by)
        {

            try
            {
                string actual = driver.FindElement(by).Text;
                Assert.AreEqual(expected, actual);
                TakeScreenShot(Status.Pass, $"Assertion Passed: Expected = {expected}, Actual = {actual}");
            }
            catch (Exception e)
            {
                TakeScreenShot(Status.Fail, $"Assertion Failed: Expected = {expected}, Actual = {driver.FindElement(by).Text}, Exception: {e.Message}");
                throw;
            }
        }

        public static void Select(By by, string txt, string comment)  // to choose an option
        {

            try
            {
                SelectElement select = new SelectElement(driver.FindElement(by));
                select.SelectByText(txt);
                TakeScreenShot(Status.Pass, comment);
            }
            catch (Exception e)
            {
                TakeScreenShot(Status.Fail, "Failed to select" + txt + "from dropdown");
                throw;
            }
        }

        public static bool SelectContains(By by, string txt, string comment)  // returns bool to verify if option is present 
        {


            try
            {
                SelectElement select = new SelectElement(driver.FindElement(by));
                bool containsUserType = select.Options.Any(option => option.Text == txt);
                TakeScreenShot(Status.Pass, comment);
                return containsUserType;
            }
            catch (Exception e)
            {
                TakeScreenShot(Status.Fail, "Dropdown doesnt contain " + txt);
                return false;
            }
        }

        public static void NavigateTo(string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
                TakeScreenShot(Status.Pass, "Navigated to URL: " + url);
            }
            catch (Exception e)
            {
                TakeScreenShot(Status.Fail, "Failed to navigate to URL: " + url + ". Exception: " + e.Message);
                throw;
            }
        }



        public static void CreateReport(string path)
        {
            extentReports = new ExtentReports();
            var sparkReporter = new ExtentSparkReporter(path);
            extentReports.AttachReporter(sparkReporter);
        }

        public static void TakeScreenShot(Status status, string detail)
        {
            string path = @"D:\ExtentReports\Images\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            File.WriteAllBytes(path, screenshot.AsByteArray);

            Step.Log(status, detail, MediaEntityBuilder.CreateScreenCaptureFromPath(path).Build());
        }

        //wait 
        public static bool WaitForElement(string xpath, int timeoutInSeconds)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                IWebElement element = wait.Until(ExpectedConditions.ElementExists(By.XPath(xpath)));
                element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
                TakeScreenShot(Status.Pass, "Element found: " + xpath);
                return true;
            }
            catch (Exception e)
            {
                TakeScreenShot(Status.Fail, "Failed to find element: " + xpath + ". Exception: " + e.Message);
                throw;
                return false;
            }
        }





        public static void Login()
        {
            Thread.Sleep(4000);

            Step = Test.CreateNode("Loggin In");
            driver.Url = Constants.Url;
            Write(By.XPath("//*[@id=\"email\"]"), Constants.email, "Email entered successfully");
            Write(By.XPath("//*[@id=\"password\"]"), Constants.password, "Password entered successfully");
            Click(By.XPath("//*[@id=\"btn-login\"]"), "Login button clicked ");
            Thread.Sleep(3000);
            //AssertAreEqual("", By.XPath(LoginPageObjects.assertLocatorXpath));

        }



        public static void logout()
        {
            Step = Test.CreateNode("Logging Out");
           // Click(By.XPath(HomePageObjects.MyAccountXpath), "My profile opened");
           // Click(By.XPath(HomePageObjects.LogoutXpath), "Logout button clicked");

            Thread.Sleep(1000);
        }
    }
}
