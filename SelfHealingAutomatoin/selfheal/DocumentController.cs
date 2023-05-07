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

namespace SelfHealingAutomatoin.selfheal
{
    public class DocumentController
    {
        private static DocumentController documentController;

        private  Task<string> html;
        private  Document document;
        private  Dictionary<String, Elements> elements = new Dictionary<string, Elements>();


        public DocumentController(Task<String> html)
        {
            this.html = html;
            document = Supremes.Dcsoup.Parse(html.Result);
            elementsByTag();
        }

        public  static DocumentController  getInstance(Task<String> html)
        {
            if (documentController == null) documentController = new DocumentController(html);
            return documentController;
        }

      
         private void elementsByTag(Tag abc)
         {
             foreach (Tag tag in abc.)
             {
                 elements.Add(tag.ToString(), document.GetElementsByTag(tag.ToString()));
             }           
         }
    }
}
