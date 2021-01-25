using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace job_portal.Controllers
{
    public class MonitorController : Controller
    {
        private readonly ILogger<MonitorController> _logger;
        private readonly IActionDescriptorCollectionProvider _provider;

        public MonitorController(IActionDescriptorCollectionProvider provider, ILogger<MonitorController> logger)
        {
            _logger = logger;
            _provider = provider;
        }

        [HttpGet("routes")]
        public IActionResult GetRoutes()
        {
            var routes = _provider.ActionDescriptors.Items.Select(x => new
            {
                Action = x.RouteValues["action"],
                Controller = x.RouteValues["controller"],
                Method = x.EndpointMetadata.OfType<HttpPostAttribute>().Count() == 0 ? "GET" : "POST",
                Name = x.AttributeRouteInfo?.Name,
                Template = x.AttributeRouteInfo?.Template
            }).ToList();

            return Ok(routes);
    }
}
}