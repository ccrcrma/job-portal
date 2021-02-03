using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace job_portal.Areas.Identity.Pages.Conventions
{
    public class CustomPageRouteModelConvention : IPageRouteModelConvention
    {
        public void Apply(PageRouteModel model)
        {
            if (model.RelativePath.StartsWith("/Areas/Identity"))
            {
                foreach (var selector in model.Selectors)
                {
                    selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template.Replace("Identity", string.Empty);
                }
            }
        }
    }
}