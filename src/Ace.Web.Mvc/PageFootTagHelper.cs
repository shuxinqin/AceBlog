using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Ace.Web.Mvc
{
    [HtmlTargetElement("pagefoot")]
    public class PageFootTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "tfoot";
            output.Content.AppendHtml("<tr>");
            output.Content.AppendHtml("    <td colspan=\"999\" align=\"center\">");
            output.Content.AppendHtml("         <div data-bind=\"pagger: pagger\"></div>");
            output.Content.AppendHtml("    </td>");
            output.Content.AppendHtml("</tr>");
        }
    }
}
