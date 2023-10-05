using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Projector.Core;
using Projector.Core.Persons.DTO;
using Projector.Core.Projects.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Projector.Models
{
    public class AssignRemoveViewModel
    {
        private readonly IProjectsService _projectsService;
        private readonly IServiceProvider _httpService;

        public AssignRemoveViewModel() { }

        public AssignRemoveViewModel(IProjectsService projectsService, IServiceProvider httpService) {
            _projectsService = projectsService;
            _httpService = httpService;
        }
        public string Unassigned {  get; set; }
        public string Assigned {  get; set; }
        private MvcViewOptions options {  get; set; }
        private PartialViewResultExecutor executor { get; set; }
        public UnassignedPersonsData UnassignedPersons {  get; set; }
        public ProjectDetailsData Details { get; set; }

        public async Task Initialize(int projectId)
        {
            UnassignedPersons = new UnassignedPersonsData{
                Unassigned = await _projectsService.GetUnassigned(projectId),
                UnassignedProjectId = projectId
            };

            Details = new ProjectDetailsData{
                Assigned = await _projectsService.GetAssigned(projectId)
            };

            options = _httpService.GetRequiredService<IOptions<MvcViewOptions>>().Value;
            executor = (PartialViewResultExecutor)_httpService.GetRequiredService<IActionResultExecutor<PartialViewResult>>();
        }

        public async Task<string> Render(PartialViewResult result, ControllerContext context)
        {
            var viewEngineResult = executor.FindView(context, result);
            viewEngineResult.EnsureSuccessful(originalLocations: null);
            var view = viewEngineResult.View;
            using (view as IDisposable)
            {
                using var writer = new StringWriter();
                var viewContext = new ViewContext(
                    context,
                    view,
                    result.ViewData,
                    result.TempData,
                    writer,
                    options.HtmlHelperOptions);

                await view.RenderAsync(viewContext);
                await writer.FlushAsync();

                return writer.ToString();
            }
        }
    }
}
