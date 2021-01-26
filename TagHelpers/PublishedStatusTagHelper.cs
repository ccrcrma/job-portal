using System.Text.Encodings.Web;
using job_portal.Types;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace job_portal.TagHelpers
{
    public class PublishedStatusTagHelper : TagHelper
    {
        public PublishedStatus Status { get; set; }
        public string ChangeUrl { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.AddClass("badge", htmlEncoder: HtmlEncoder.Default);
            output.AddClass("status", htmlEncoder: HtmlEncoder.Default);
            output.Attributes.SetAttribute("data-action", ChangeUrl);
            if (Status == PublishedStatus.Live)
            {
                output.AddClass("badge-success", HtmlEncoder.Default);
            }
            else
            {
                output.AddClass("badge-secondary", HtmlEncoder.Default);
            }
            output.Content.SetContent(Status.ToString());
        }
    }
}