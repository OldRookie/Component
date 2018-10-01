using System.Web.Mvc;

namespace Component.UI.MVC.Areas.Bootstrap
{
    public class BootstrapAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Bootstrap";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Bootstrap_default",
                "Bootstrap/{controller}/{action}",
               defaults: new { controller = "forms", action = "Index" }
            );

            context.MapRoute(
              "Bootstrap_Language",
              "Bootstrap/{lang}/{controller}/{action}",
              defaults: new { controller = "forms", action = "Index"}
          );
        }
    }
}