using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace StockManagement.Web.Services
{
    public class RazorViewToStringRenderer
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public RazorViewToStringRenderer(
            ICompositeViewEngine viewEngine,
            IServiceProvider serviceProvider,
            ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _viewEngine = viewEngine;
            _serviceProvider = serviceProvider;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            using var sw = new StringWriter();
            var viewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: false);

            if (!viewResult.Success)
            {
                throw new FileNotFoundException($"View {viewName} not found.");
            }

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            // Create a TempDataDictionary using the factory
            var tempData = _tempDataDictionaryFactory.GetTempData(actionContext.HttpContext);
            var viewContext = new ViewContext(actionContext, viewResult.View, viewData, tempData, sw, new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
