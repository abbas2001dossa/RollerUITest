using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace RollerUITest.TestCases
{
    [TestClass]
    public class Test
    {
        public TestContext instance;
        public TestContext TestContext
        {
            set { instance = value; }
            get { return instance; }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            BasePage.SeleniumInit("Chrome");
            BasePage.Test = BasePage.extentReports.CreateTest(TestContext.TestName);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            BasePage.SeleniumCleanup();
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            string resultFile = @"D:\ExtentReports\SampleTestRoller" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
            BasePage.CreateReport(resultFile);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BasePage.extentReports.Flush();
        }

        [TestMethod]
        [TestCategory("Basit Test")]
        public void BasitTest()
        {
            try
            {
                if (BasePage.driver == null)
                {
                    throw new Exception("WebDriver is not initialized. Please check SeleniumInit.");
                }

                BasePage.driver.Url = Utils.Constants.Url;


                WebDriverWait wait = new WebDriverWait(BasePage.driver, TimeSpan.FromSeconds(30));


                var emailElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"email\"]")));
                if (emailElement != null)
                {
                    BasePage.Write(By.XPath("//*[@id=\"email\"]"), Utils.Constants.email, "Email entered successfully");
                }
                else
                {
                    throw new Exception("Email element not found!");
                }
                Console.WriteLine("HERE");

                // Check if the password element is visible
                var passwordElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"password\"]")));
                if (passwordElement != null)
                {
                    BasePage.Write(By.XPath("//*[@id=\"password\"]"), Utils.Constants.password, "Password entered successfully");
                }
                else
                {
                    throw new Exception("Password element not found!");
                }


                // Check if the login button is clickable
                var loginButtonElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btn-login\"]")));
                if (loginButtonElement != null)
                {
                    BasePage.Click(By.XPath("//*[@id=\"btn-login\"]"), "Login button clicked");
                }
                else
                {
                    throw new Exception("Login button is not clickable!");
                }

                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@class='apps__button app--vm']")));
                BasePage.Click(By.XPath("//button[@class='apps__button app--vm']"), "Clicked Venue Manager");

                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[@id='guests']")));
                BasePage.Click(By.XPath("//a[@id='guests']"), "Clicked Guests from side bar");
                BasePage.NavigateTo("https://manage.roller.app/guests");

                // check from here 
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[3]/div[1]/div/roller-grid2/div[2]/div/div[6]/div/div/div[1]/div/table/tbody/tr[1]/td[1]/div/div")));
                BasePage.Click(By.XPath("/html/body/div[1]/div[3]/div[1]/div/roller-grid2/div[2]/div/div[6]/div/div/div[1]/div/table/tbody/tr[1]/td[1]/div/div"),"Clicking guest details ");


                //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"firstName\"]")));



                string firstName = BasePage.driver.FindElement(By.XPath("//*[@id=\"firstName\"]")).GetAttribute("value");
                string lastName = BasePage.driver.FindElement(By.XPath("//*[@id=\"lastName\"]")).GetAttribute("value");
                string email = BasePage.driver.FindElement(By.XPath("//*[@id=\"email\"]")).GetAttribute("value");
                string phoneNumber = BasePage.driver.FindElement(By.XPath("//*[@id=\"mobile\"]")).GetAttribute("value");
                string dateOfBirth = BasePage.driver.FindElement(By.XPath("//*[@id=\"ng-app_rollerVenueApp\"]/div[3]/div[1]/div[2]/div[1]/div[2]/ui-view/form/div[1]/div/section[1]/div[2]/ng-transclude/fieldset/div[4]/div[1]/div[1]/select")).GetAttribute("value");
                string gender = BasePage.driver.FindElement(By.XPath("//*[@id=\"ng-app_rollerVenueApp\"]/div[3]/div[1]/div[2]/div[1]/div[2]/ui-view/form/div[1]/div/section[1]/div[2]/ng-transclude/fieldset/div[4]/div[2]/div/select")).Text;


                // Create a Dictionary to hold the data
                Dictionary<string, string> guestDetails = new Dictionary<string, string>
                {
                    { "First Name", firstName },
                    { "Last Name", lastName },
                    { "Email", email },
                    { "Phone Number", phoneNumber },
                    { "Date of Birth", dateOfBirth },
                    { "Gender", gender }
                };

                // Save the details to a .docx file
                SaveToDoc("GuestDetails.docx", guestDetails);

                Console.WriteLine("Details saved successfully!");




            }
            catch (Exception ex)
            {
                // Log the error to the extent report
                BasePage.Test.Fail("Test failed due to: " + ex.Message);
                throw;
            }
        }
        static void SaveToDoc(string filePath, Dictionary<string, string> data)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Guest Details:");
                writer.WriteLine("==================");
                foreach (var detail in data)
                {
                    writer.WriteLine($"{detail.Key}: {detail.Value}");
                }
            }
        }
    }
}
