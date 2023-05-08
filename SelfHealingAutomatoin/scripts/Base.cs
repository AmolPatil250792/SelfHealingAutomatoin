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
using System.Reflection;

namespace SelfHealingAutomatoin.scripts
{
    public class Base
    {
        public ExtentReports extent;
        public static ExtentTest test;

        [OneTimeSetUp]
        public void ExtentStart()
        {
            extent = new ExtentReports();
            var htmlreporter = new ExtentV3HtmlReporter($"{System.Environment.CurrentDirectory}\\..\\Logs\\SelfHeal{DateTime.Now.ToString("_MMddyyyy_hhmmtt")}.html");
            extent.AttachReporter(htmlreporter);
        }

        [OneTimeTearDown]
        public void ExtentClose()
        {
            extent.Flush();
        }
    }
}
