using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ace.Web.Mvc
{
    [HtmlTargetElement("link", Attributes = HrefAttributeName)]
    public class LinkTagHelper : TagHelper
    {
        static string dtString = DateTime.Now.ToString("yyMMddHHmmss");

        private const string HrefAttributeName = "href";
        public LinkTagHelper()
        {

        }
        [HtmlAttributeName(HrefAttributeName)]
        public string Href { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(this.Href))
            {
                return;
            }

            string href = this.Href;
            string _dt = dtString;
            if (href.IndexOf("?") > 0)
            {
                href = $"{href}&_dt={_dt}";
            }
            else
            {
                href = $"{href}?_dt={_dt}";
            }

            href = this.ViewContext.HttpContext.Content(href);
            output.Attributes.SetAttribute("href", href);
        }
    }

}
