using Microsoft.Playwright;
using SelfHealingAutomatoin.selfheal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static SelfHealingAutomatoin.selfheal.DocumentController;
using static System.Net.Mime.MediaTypeNames;

namespace SelfHealingAutomatoin.pageobjects
{
    public class RegistrationFormAutoDiscovery
    {
        protected static IPage page;
        private static DocumentController documentController;

        public RegistrationFormAutoDiscovery(IPage pageobject) 
        {
           page = pageobject;
            
            documentController = DocumentController.getInstance(getPageSource());
        }

        public async Task<string> getPageSource()
        {
            var playwright = await Playwright.CreateAsync();
            var chromebrowserTypeLaunchOptions = new BrowserTypeLaunchOptions()
            {
                Headless = false,
                Channel = "chrome",
                Args = new string[] { "--start-maximized" }
            };
            //Launch Browser         
            var chromeBrowser = await playwright.Chromium.LaunchAsync(chromebrowserTypeLaunchOptions);
            IBrowserContext context = await chromeBrowser.NewContextAsync(new BrowserNewContextOptions()
            {
                ViewportSize = ViewportSize.NoViewport,
                AcceptDownloads = true
            });
            page = await context.NewPageAsync();
            await page.GotoAsync("C:\\Users\\apatil2\\Desktop\\loginpage.html");
            return await page.ContentAsync();
        }
        public async Task goToPage()
        {

            await page.GotoAsync("http://localhost:7800/bootstrap1.html#");
        }

        public async Task enterText(string label, string value)
        {
            string locator = documentController.getLocator(Tag.input, label);
            //actions.enterText(locator, value);
        }
    }

}

