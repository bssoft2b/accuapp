using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuApp.Services
{
    /// <summary>
    /// Renders email content based on razor templates
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Renders a template given the provided view model
        /// </summary>
        /// <typeparam name="object"></typeparam>
        /// <param name="filename">Filename of the template to render</param>
        /// <param name="viewModel">View model to use for rendering the template</param>
        /// <returns>Returns the rendered template content</returns>
        Task<string> RenderTemplateAsync(string filename, object viewModel);
    }
}
