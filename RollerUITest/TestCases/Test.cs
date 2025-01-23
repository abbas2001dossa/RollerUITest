using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;


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
        [TestCategory("ROLLER TEST")]
        public void RollerTest()
        {
            BasePage.Login();

            //click venue manager
            BasePage.Click(By.XPath("/html/body/app-root/app-launchpad/div[2]/div/div/app-link[1]/button"), "Opening Venue Manager");

            //open guest module
            //BasePage.Click(By.XPath("/html/body/app-root/mat-drawer-container/mat-drawer-content/aside/app-sidebar/div/button"),"Opening side nav");
            BasePage.Click(By.XPath("/html/body/app-root/mat-drawer-container/mat-drawer-content/aside/app-sidebar/div/div[2]/app-main-menu[7]/a"), "Opening Guest Module");
            BasePage.Click(By.XPath("/html/body/app-root/mat-drawer-container/mat-drawer-content/aside/app-sidebar/div/div[2]/app-main-menu[7]/div/app-menu-item[1]/a"), "");
            //open guest module
            BasePage.NavigateTo("https://manage.roller.app/guests");
            Thread.Sleep(8000);
            BasePage.Click(By.XPath("//*[@id=\"ng-app_rollerVenueApp\"]/div[3]/div[1]/div/roller-grid2/div[2]/div/div[6]/div/div/div[1]/div/table/tbody/tr[1]/td[1]/div"),"");

            //BasePage.Click(By.XPath("//*[@id=\"ng-app_rollerVenueApp\"]/div[3]/div[1]/div[1]/div[1]/div[2]/div/div[2]/div[2]/button[1]"), "Closing right nav view");
        }
    }
}
