using Denver.Common;

namespace Denver.PCL
{
    public class ProjectInfo
        :BaseEntityClass
    {
        public int BusinessValue { get; set; }
        public ProjectSize Size { get; set; }
        public int WorkersCount { get; set; }
        public bool IsInternationalProject { get; set; }
    }
}