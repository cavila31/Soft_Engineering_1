using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using SistemaPruebas.Models;
using System.Data;
using System.Web.UI.WebControls;


namespace SistemaPruebas.Account
{
    public partial class Login : Page
    {
        public static string id_logeado
        {
            get
            {
                object value = HttpContext.Current.Session["id_logeado"];
                return value == null ? "" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["id_logeado"] = value;
            }
        }

        public static int loggeado
        {
            get
            {
                object value = HttpContext.Current.Session["loggeado"];
                return value == null ? 0 : (Int32.Parse(value.ToString()));
            }
            set
            {
                HttpContext.Current.Session["loggeado"] = value;
            }
        }
        //static public int loggeado = 0;
        Controladoras.ControladoraRecursosHumanos controladoraRH = new Controladoras.ControladoraRecursosHumanos();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Account.CambiarContrasena.cambiado == 1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Cambio de contraseña hecho." + "');", true);
                Account.CambiarContrasena.cambiado = 0;
            }
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        /*
         * Requiere: Nombre de Usuario ingresado en la caja de texto correspondiente
         * y presión del botón de Aceptar, además de la contraseña ingresada.
         * Modifica: Se realiza la validación de los datos ingresados, conforme a la información
         * que se posee de la base de datos. Se hace la operación de loggear al usuario.
         * Retorna: N/A.
         */
        protected void LogIn(object sender, EventArgs e)
        {
            Object[] datos = new Object[2];
            datos[0] = this.UserName.Text;
            datos[1] = this.Password.Text;
            if (controladoraRH.loggeado(datos[0].ToString()) == false)
            {
                if (controladoraRH.usuarioMiembroEquipo(datos))
                {
                    //regreso true
                    id_logeado = datos[0].ToString();

                    controladoraRH.estadoLoggeado(datos[0].ToString(), "1");

                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Loggeo correcto" + "');", true);
                    loggeado = 1;
                    Response.Redirect("~/Default");

                }
                else
                {
                    //regreso false
                    EtiqErrorLlaves.Text = "Usuario no coincide con contraseña";
                    EtiqErrorLlaves.Visible = true;
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    //((Label)this.Master.FindControl("MensajesGenerales")).Text = "Usuario no coincide con contraseña";
                    //((Label)this.Master.FindControl("MensajesGenerales")).ForeColor = System.Drawing.Color.LightSalmon;
                    //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Usuario no coincide con contraseña" + "');", true);
                }
            }
            else
            {
                //esta loggeado en otro lado
                EtiqErrorLlaves.Text = "Está loggeado en otro lado. Será cerrará la sesión";
                EtiqErrorLlaves.Visible = true;
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //((Label)this.Master.FindControl("MensajesGenerales")).Text = "Está loggeado en otro lado. Será cerrará la sesión.";
                //((Label)this.Master.FindControl("MensajesGenerales")).ForeColor = System.Drawing.Color.LightSalmon;
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Está loggeado en otro lado. Será cerrará la sesión." + "');", true);
                controladoraRH.estadoLoggeado(datos[0].ToString(), "0");


            }

        }

    }
}