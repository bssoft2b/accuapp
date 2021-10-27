using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuApp.Services
{
    public class TemplateService : ITemplateService
    {
        private IRazorViewEngine _viewEngine;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;

        public TemplateService(IRazorViewEngine viewEngine, IServiceProvider serviceProvider, ITempDataProvider tempDataProvider)
        {
            _viewEngine = viewEngine;
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderTemplateAsync(string filename, object model)
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };

            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var outputWriter = new StringWriter())
            {
                var viewResult = _viewEngine.GetView(null, filename, false);
                
                if (!viewResult.Success)
                {
                    throw new TemplateServiceException($"Failed to render template {filename} because it was not found.");
                }

                try
                {
                    var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };

                    var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary,
                        new TempDataDictionary(actionContext.HttpContext, _tempDataProvider), outputWriter, new HtmlHelperOptions());

                    await viewResult.View.RenderAsync(viewContext);
                }
                catch (Exception ex)
                {
                    throw new TemplateServiceException("Failed to render template due to a razor engine failure", ex);
                }

                return outputWriter.ToString();
            }
        }
    }

    /// <summary>
    /// Gets thrown when the template service was unable to render the template
    /// </summary>
    public class TemplateServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TemplateServiceException"/>
        /// </summary>
        /// <param name="message"></param>
        public TemplateServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="TemplateServiceException"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TemplateServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
