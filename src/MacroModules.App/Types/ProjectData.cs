using MacroModules.Model.BoardElements;

namespace MacroModules.App.Managers
{
    public class ProjectData
    {
        public List<BoardElement> Elements { get; set; }

        public string Projectname { get; set; }

        public ProjectData(List<BoardElement> elements, string projectName)
        {
            Elements = elements;
            Projectname = projectName;
        }
    }
}
