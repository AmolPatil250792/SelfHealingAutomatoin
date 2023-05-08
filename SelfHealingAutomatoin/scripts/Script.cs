using Microsoft.Playwright;
using NUnit.Framework;
using SelfHealingAutomatoin.pageobjects;
using SelfHealingAutomatoin.selfheal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHealingAutomatoin.scripts
{
    public class Script
    {
        IPage page;
        [Test]
        public async Task ScriptToTest() {

            RegistrationFormAutoDiscovery objregistrationform = new RegistrationFormAutoDiscovery(page);
            string abc = await objregistrationform.getPageSource();
            //await objregistrationform.enterText("Enter your email", "amol");
            await objregistrationform.enterText("username", "amol");
            //await objregistrationform.enterTextWithoutSelfHeal("username", "amol");
        

        }
    }
}
