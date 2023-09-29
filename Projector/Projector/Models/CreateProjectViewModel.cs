using Projector.Core.Projects;

namespace Projector.Models
{
    public class CreateProjectViewModel
    {
        /* CONSTRUCTORS */
        public CreateProjectViewModel()
        {
            Errors = Enumerable.Empty<string>();
        }

        public CreateProjectViewModel(ProjectData project, IEnumerable<string> errors)
        {
            Project = project;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        /* PROPERTIES */
        public ProjectData Project { get; set; }

        public IEnumerable<string> Errors { get; set; }

    }
}
