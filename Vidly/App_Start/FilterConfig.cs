using System.Web;
using System.Web.Mvc;

namespace Vidly
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //To add [Authorized] for all the sites
            //filters.Add(new AuthorizeAttribute());

            //To avoid non-https connections
            //filters.Add(new RequireHttpsAttribute());
            //To enable Facebook, enable startup.auth after register in Facebook for developers website
        }
    }
}
