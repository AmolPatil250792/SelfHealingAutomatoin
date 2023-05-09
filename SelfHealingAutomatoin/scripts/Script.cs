using AventStack.ExtentReports;
using Microsoft.Playwright;
using NUnit.Framework;
using SelfHealingAutomatoin.pageobjects;
using SelfHealingAutomatoin.selfheal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports.Reporter;

namespace SelfHealingAutomatoin.scripts
{
    public class Script : Base
    {
        IPage page;
       

        [Test]
        public async Task EnterUserNameWithSelfHeal() {

            try
            {
                test = extent.CreateTest("With Self Heal - Launch page and enter username in Email field.").Info("With Self Heal - Launch page and enter username in Email field.");
                RegistrationFormAutoDiscovery objregistrationform = new RegistrationFormAutoDiscovery(page);
                //await objregistrationform.enterText("Enter your email", "amol");
                await objregistrationform.enterTextWithSelfHeal("username", "amol.patil@yopmail.com");
                //await objregistrationform.enterTextWithoutSelfHeal("username", "amol");
                test.Log(Status.Pass, "Test Passed");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, ex.Message);
                Assert.Fail("Test Failed");
            }
        }

        [Test]
        public async Task EnterUserNameWithOutSelfHeal()
        {
            try
            {
                test = extent.CreateTest("WithOut Self Heal - Launch page and enter username in Email field.").Info("WithOut Self Heal - Launch page and enter username in Email field.");
                RegistrationFormAutoDiscovery objregistrationform = new RegistrationFormAutoDiscovery(page);
                //await objregistrationform.enterText("Enter your email", "amol");
                //await objregistrationform.enterText("username", "amol");
                await objregistrationform.enterTextWithoutSelfHeal("username", "amol.patil@yopmail.com");
                test.Log(Status.Pass, "Test Passed");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, ex.Message);
                Assert.Fail("Test Failed");
            }
        }

    }
}
