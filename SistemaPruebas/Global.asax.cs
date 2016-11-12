using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace SistemaPruebas
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~\\Default.aspx")); 
            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_Disposed(object sender, EventArgs e)
        {
           // Session.Abandon();
        }
      protected void Session_Start(Object sender, EventArgs e)
      {
          Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~\\Default.aspx")); 
      }
        
        protected void Session_End(Object sender, EventArgs e)
        {
            try
            {
                string id = Session["id_logeado"].ToString();
                Console.WriteLine("Finish");
                Controladoras.ControladoraRecursosHumanos controladoraRH = new Controladoras.ControladoraRecursosHumanos();
                controladoraRH.estadoLoggeado(id, "0");
                //Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~\\Default.aspx"));    
                //string id = HttpContext.Current.Session.SessionID;          
            }
            catch (NullReferenceException nn)
            {
                Console.WriteLine("Referencia a null "+nn.ToString());
            }
        }
    }
}