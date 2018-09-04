using System.Reflection;

namespace RoadProvider.Web.References
{
    public static class ReferencedAssemblies
    {
        public static Assembly Services
        {
            get { return Assembly.Load("RoadProvider.Services"); }
        }

        public static Assembly Repositories
        {
            get { return Assembly.Load("RoadProvider.Data"); }
        }

        public static Assembly Dto
        {
            get
            {
                return Assembly.Load("RoadProvider.Dto");
            }
        }

        public static Assembly Domain
        {
            get
            {
                return Assembly.Load("RoadProvider.Core");
            }
        }
    }
}
