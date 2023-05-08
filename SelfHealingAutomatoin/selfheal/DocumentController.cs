using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Supremes;
using Supremes.Select;
using Supremes.Nodes;
using System.Xml.Linq;
using System.Net.Http.Headers;
using NUnit.Framework;
using FuzzySharp.Extractor;
using Microsoft.Playwright;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
using System.Runtime.InteropServices;
using System.Reflection;
using SelfHealingAutomatoin.selfheal;
using SelfHealingAutomatoin.pageobjects;
using System.Configuration;
using System.Reflection.Emit;
using System.IO;
using AventStack.ExtentReports;
using SelfHealingAutomatoin.scripts;

namespace SelfHealingAutomatoin.selfheal
{
    public class DocumentController : Base
    {
        private static DocumentController documentController;
        public static string folder = Environment.CurrentDirectory + "\\CSharpCornerAuthors.txt";
        private Task<string> html;
        private Document document;
        private Dictionary<string, Elements> elements = new Dictionary<string, Elements>();


        public DocumentController(Task<string> html)
        {
            this.html = html;
            document = Supremes.Dcsoup.Parse(html.Result);
            elementsByTag();
        }

        public enum Tag
        {
            [Description("input")]
            input,
            [Description("button")]
            button,
            [Description("link")]
            link,
            [Description("select")]
            select,
            [Description("radio")]
            radio,
            [Description("checkbox")]
            checkbox

        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            if (array != null && array.Length != 0)
            {
                return array[0].Description;
            }
            return value.ToString();
        }

        public enum Attribute
        {
            id, className, name, text, childText, placeholder
        }

        public static DocumentController getInstance(Task<string> html)
        {
            if (documentController == null) documentController = new DocumentController(html);
            return documentController;
        }

        public Elements getElementsByTag(string tag)
        {
            Elements a = elements[tag];
            return elements[tag];
        }

        private void elementsByTag()
        {
            Type Tags = typeof(Tag);

            foreach (Tag tag in Enum.GetValues(Tags))
            {
                elements.Add(tag.ToString(), document.GetElementsByTag(tag.ToString()));

            }
        }

        public string getLocator(Tag tag, string matcher, string locatorToDelete = null)
        {
            test.Log(Status.Info, $"Trying to get locator using Fuzzy Logic since cached locator either not found or did not work for tag : {tag} and label : {matcher}");

            // Initialize Result Set
            int score = 0;
            Elements tagElements = null;

            // Filter By Element Type
            if (tag.Equals(Tag.input))
            {
                tagElements = elements["input"];
                Elements checkboxElements = tagElements.Select("input[type!=hidden]");
                tagElements = checkboxElements;               
            }

            // Fuzzy Search by Inner Text
            string cssSelector = null;
            List<string> values = new List<string>();
            foreach (Element tagElement in tagElements)
            {
                values.Add(tagElement.Text);
            }

            ExtractedResult<string> result = FuzzySharp.Process.ExtractOne(matcher, values);
            score = result.Score;
            cssSelector = tagElements[result.Index].CssSelector;

            /*// Fuzzy Search by Label
            Elements labels = elements["label"];
            // if (labels != null && !labels.isEmpty()) 
            if (labels != null)
            {
                values = new List<string>();
                foreach (Element label in labels)
                {
                    values.Add(label.Text);
                }

                result = FuzzySharp.Process.ExtractOne(matcher, values);
                if (result.Score > score)
                {
                    score = result.Score;
                    string id = labels[result.Index].Attr("for");
                   // cssSelector = document.Attr("id", id).;
                }
            }
*/
            // Fuzzy Search by Attribute Value
            Type attributeType = typeof(Attribute);
            foreach (Attribute attr in Enum.GetValues(attributeType))
            {
                {
                    values = new List<string>();
                    foreach (Element tagElement in tagElements)
                    {
                        values.Add(tagElement.Attr(attr.ToString()));
                    }
                    result = FuzzySharp.Process.ExtractOne(matcher, values);
                    if (result.Score >= score)
                    {
                        score = result.Score;
                        cssSelector = tagElements[result.Index].CssSelector;
                        // cssSelector = tagElements[result.getInde].cssSelector();
                    }
                }
                if (cssSelector == null)
                {
                    throw new Exception("Element Not Found: " + tag + "=" + matcher);
                }
                

            }
            string key = RegistrationFormAutoDiscovery.flatten(tag, matcher);

            string filecontent = key + "|" + cssSelector;
            string[] inventoryData = File.ReadAllLines(folder);
            List<string> inventoryDataList = inventoryData.ToList();
            if(locatorToDelete != null)
            {
                if (inventoryDataList.Remove(locatorToDelete)) // rewrite file if one item was found and deleted.
                {
                    System.IO.File.WriteAllLines(folder, inventoryDataList.ToArray());
                    File.AppendAllText(folder, filecontent + Environment.NewLine);
                }
            }
            
            else
            {
                File.AppendAllText(folder, filecontent + Environment.NewLine);
            }
            
            test.Log(Status.Info, $"Locator found using Fuzzy Match  : {cssSelector}");
            return cssSelector;
        }


       

        }
    }
