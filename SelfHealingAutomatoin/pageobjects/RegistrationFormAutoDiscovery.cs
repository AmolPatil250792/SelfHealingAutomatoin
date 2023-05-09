using Microsoft.Playwright;
using NUnit.Framework;
using SelfHealingAutomatoin.selfheal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using static SelfHealingAutomatoin.selfheal.DocumentController;
using static System.Net.Mime.MediaTypeNames;
using System.Configuration;
using System.Reflection.Emit;
using SelfHealingAutomatoin.scripts;
using AventStack.ExtentReports;

namespace SelfHealingAutomatoin.pageobjects
{
    public class RegistrationFormAutoDiscovery : Base
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

        /*public async Task enterText(string label, string value)
        {
            string locator = documentController.getLocator(Tag.input, label);
            //actions.enterText(locator, value);
        }*/

        public async Task enterTextWithoutSelfHeal(string label, string value)
        {
            await page.Locator("#" + label).TypeAsync(value);
            test.Log(Status.Pass, $"UserName entered with locator #{label} without using Self Heal");
        }

        public async Task enterTextWithSelfHeal(string label, string value)
        {
            try
            {
                string catchedElement = RegistrationFormAutoDiscovery.getCachedLocator(Tag.input, label);
                string key = flatten(Tag.input, label);
                if (catchedElement != null)
                {
                    try
                    {
                        ILocator element = page.Locator(catchedElement);
                        await element.FillAsync(value);
                        test.Log(Status.Pass, $"UserName entered using cached locator : {catchedElement}");
                    }
                    catch (System.TimeoutException)
                    {
                        ILocator element = page.Locator(documentController.getLocator(Tag.input, label, locatorToDelete : $"{key}|{catchedElement}"));
                        await element.FillAsync(value);
                        test.Log(Status.Pass, $"UserName entered using Fuzzy Logic locator : {element}");
                    }
                }
                else
                {
                    ILocator element = page.Locator(documentController.getLocator(Tag.input, label));
                    await element.FillAsync(value);
                    test.Log(Status.Pass, $"UserName entered using Fuzzy Logic locator : {element}");
                }

                // return;
            
            }
            catch (Exception e)
            {
                throw e;
            }




        }

        /**
    *   flatten() -- Merge all characteristics into a key to use for the cache
    *
    * @param characteristics - The map of characteristics desired to find a specific locator in the DOM
    * @return - returns an encorded key As a String
    */
        public static string flatten(Tag tagname, string labelvalue)
        {
            StringBuilder result = new StringBuilder();
            result.Append(GetEnumDescription(tagname)).Append(labelvalue);
            return result.ToString().Replace(" ", "");
        }

        /**
         * getCachedLocator - Retrieve a Locator from the Cache
         * @param characteristics - Used to create a unique key
         * @return - The locator, should it exist in the cache
         * @throws CachedLocatorNotFound - If not found
         */
        public static string getCachedLocator(Tag tagName, string label)
        {

            test.Log(Status.Info, $"First trying to get locator from cache for tag : {tagName} and label : {label}");
            string key = flatten(tagName, label);
            string returnValue = null;
            if (File.Exists(folder))
            {
                // Read a text file line by line.
                string[] lines = File.ReadAllLines(folder);
                foreach (string line in lines)
                {
                    string[] splittedString = line.Split('|');
                    if (splittedString[0].Equals(key))
                    {
                        returnValue = splittedString[1];
                        break;
                    }
                }
            }
            test.Log(Status.Info, $"Cached locator found : {returnValue}");
            return returnValue;

        }








    }

}

