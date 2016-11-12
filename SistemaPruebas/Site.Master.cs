using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SistemaPruebas
{
    public partial class SiteMaster : MasterPage
    {
        public static string info
        {
            get
            {
                object value = HttpContext.Current.Session["info"];
                return value == null ? "" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["info"] = value;
            }
        }

        Controladoras.ControladoraRecursosHumanos controladoraRH = new Controladoras.ControladoraRecursosHumanos();
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // El código siguiente ayuda a proteger frente a ataques XSRF
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Utilizar el token Anti-XSRF de la cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generar un nuevo token Anti-XSRF y guardarlo en la cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Establecer token Anti-XSRF
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validar el token Anti-XSRF
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Error de validación del token Anti-XSRF.");
                }
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();  
            if (Account.Login.id_logeado == "")
            {
                nombre.Visible = false;
                makeInVisible();
            }
            else if (Account.Login.loggeado == 1)
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + "Loggeo correcto" + "');", true);
                MensajesGenerales.Text = "Ingreso correcto al sistema";
                MensajesGenerales.ForeColor = System.Drawing.Color.DarkSeaGreen;
                MensajesGenerales.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
               // ((Label)this.Master.FindControl("MensajesGenerales")).Text = "Ingreso correcto al sistema";
                makeVisible();
                nombre.Visible = true;
                //nombre.InnerHtml = Account.Login.id_logeado;
                Nombres.InnerText = Account.Login.id_logeado;
                Account.Login.loggeado = 0;
            }
            if (Account.Login.id_logeado == "")
            {
                LOGIN.Visible = true;
                LOGOUT.Visible = false;
            }
            else if (controladoraRH.loggeado(Account.Login.id_logeado) == false)
            {
                LOGIN.Visible = true;
                LOGOUT.Visible = false;
            }
            else
            {
                LOGIN.Visible = false;
                nombre.Visible = true;
                Nombres.Visible = true;
                Nombres.InnerText = Account.Login.id_logeado;
                info = @"Nombre: " + controladoraRH.solicitarNombreRecurso(controladoraRH.idDelLoggeado()) + "      Cédula: " + controladoraRH.idDelLoggeado().ToString() + "       Perfil: " + controladoraRH.perfilDelLoggeado();
                if (controladoraRH.loggeadoEsAdmin() == false)
                {
                    info += "       Proyecto Asociado: " + controladoraRH.solicitarNombreProyecto(controladoraRH.proyectosDelLoggeado());
                }

                Nombres.Attributes["data-content"] = info ;
                LOGOUT.Visible = true;
            }
        }

        public void makeVisible()
        {
            try
            {
                A1.Visible = true;
                A2.Visible = true;
                A3.Visible = true;
                A4.Visible = true;
                A5.Visible = true;
//                nombre.InnerText = Account.Login.id_logeado;
                nombre.Visible = true;
                Nombres.Visible = true;
                Nombres.InnerText = Account.Login.id_logeado;
                info = @"Nombre: " + controladoraRH.solicitarNombreRecurso(controladoraRH.idDelLoggeado()) + "      Cédula: " + controladoraRH.idDelLoggeado().ToString() + "       Perfil: " + controladoraRH.perfilDelLoggeado();
                if (controladoraRH.loggeadoEsAdmin() == false)
                {
                    info += "       Proyecto Asociado: " + controladoraRH.solicitarNombreProyecto(controladoraRH.proyectosDelLoggeado());
                }

                Nombres.Attributes["data-content"] = info;

                Li1.Visible = true;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("IOException source: {0}", e.Source);
            }

        }
        public void makeInVisible()
        {
            try
            {
                A1.Visible = false;
                A2.Visible = false;
                A3.Visible = false;
                Li1.Visible = false;
                A4.Visible = false;
                A5.Visible = false;
                nombre.Visible = false;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("IOException source: {0}", e.Source);
            }

        }

        protected void LogOut(object sender, EventArgs e)
        {
            Session.Abandon();
            //controladoraRH.estadoLoggeado(Account.Login.el_logeado, "0");
            //Account.Login.el_logeado = "";
            makeInVisible();           
            Response.Redirect("~/Default");
            
        }

        protected void yourControlToBeClicked_Click(object sender, EventArgs e)
        {

        }
    }

}