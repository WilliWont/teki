#pragma checksum "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3ecf381fb5dfa1bfbcb4d2ef513ffce7352b5549"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Article_Detail), @"mvc.1.0.view", @"/Views/Article/Detail.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\a_self_proj\teki\TekiBlog\Views\_ViewImports.cshtml"
using TekiBlog;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\a_self_proj\teki\TekiBlog\Views\_ViewImports.cshtml"
using TekiBlog.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3ecf381fb5dfa1bfbcb4d2ef513ffce7352b5549", @"/Views/Article/Detail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2c2058f722980ddfc1d58dfa76a40287347254db", @"/Views/_ViewImports.cshtml")]
    public class Views_Article_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TekiBlog.Models.Article>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("title: ");
#nullable restore
#line 2 "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml"
  Write(Html.DisplayFor(model => model.Title));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>\r\nraw:");
#nullable restore
#line 4 "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml"
Write(Html.DisplayFor(model => model.ContentRaw));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>\r\nhtml:");
#nullable restore
#line 6 "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml"
Write(Html.Raw(@Model.ContentHtml));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>\r\nsummary:");
#nullable restore
#line 8 "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml"
   Write(Html.DisplayFor(model => model.Summary));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>\r\ndatepost;");
#nullable restore
#line 10 "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml"
    Write(Html.DisplayFor(model => model.DatePosted));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>\r\ncurrentvote:");
#nullable restore
#line 12 "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml"
       Write(Html.DisplayFor(model => model.CurrentVote));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>\r\nstatus:");
#nullable restore
#line 14 "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml"
  Write(Html.DisplayFor(model => model.Status));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>\r\nuserId:");
#nullable restore
#line 16 "D:\a_self_proj\teki\TekiBlog\Views\Article\Detail.cshtml"
  Write(Html.DisplayFor(model => model.User));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TekiBlog.Models.Article> Html { get; private set; }
    }
}
#pragma warning restore 1591