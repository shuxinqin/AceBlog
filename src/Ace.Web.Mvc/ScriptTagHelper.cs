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
    [HtmlTargetElement("script", Attributes = SrcAttributeName)]
    public class ScriptTagHelper : TagHelper
    {
        static string dtString = DateTime.Now.ToString("yyMMddHHmmss");

        private const string SrcAttributeName = "src";
        public ScriptTagHelper()
        {

        }
        [HtmlAttributeName(SrcAttributeName)]
        public string Src { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(this.Src))
            {
                return;
            }

            string src = this.Src;
            string _dt = dtString;
            if (src.IndexOf("?") > 0)
            {
                src = $"{src}&_dt={_dt}";
            }
            else
            {
                src = $"{src}?_dt={_dt}";
            }

            src = this.ViewContext.HttpContext.Content(src);
            output.Attributes.SetAttribute("src", src);
        }
    }
}
