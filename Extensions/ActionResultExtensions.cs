using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace job_portal.Extensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult WithSuccess(this IActionResult actionResult, string title, string body)
        {
            return new ActionResultDecorator(actionResult, "success", title, body);
        }

        public static IActionResult WithDanger(this IActionResult actionResult, string title, string body)
        {
            return new ActionResultDecorator(actionResult, "danger", title, body);
        }
    }

    internal class ActionResultDecorator : IActionResult
    {
        private readonly IActionResult _result;
        private readonly string _title;
        private readonly string _type;
        private readonly string _body;
        public ActionResultDecorator(IActionResult actionResult, string type, string title, string body)
        {
            _result = actionResult;
            _title = title;
            _type = type;
            _body = body;

        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = factory.GetTempData(context.HttpContext);
            tempData["alert-title"] = _title;
            tempData["alert-type"] = _type;
            tempData["alert-message"] = _body;
            await _result.ExecuteResultAsync(context);
        }
    }
}