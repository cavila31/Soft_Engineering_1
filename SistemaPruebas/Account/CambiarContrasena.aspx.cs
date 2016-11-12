using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaPruebas.Account
{
    public partial class CambiarContrasena : System.Web.UI.Page
    {
        Controladoras.ControladoraRecursosHumanos controladoraRH = new Controladoras.ControladoraRecursosHumanos();

        public static int cambiado = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /*
         * Requiere: Nombre de Usuario ingresado en la caja de texto correspondiente
         * y presión del botón de Aceptar, además de la contraseña ingresada.
         * También debe estar la contraseña nueva, dos veces por confirmación.
         * Modifica: Se realiza la validación de los datos ingresados, conforme a la información
         * que se posee de la base de datos.
         * Se cambiar la contraseña por la nueva si los datos ingresados son correctos.
         * Retorna: N/A.
         */
        protected void cambiarContrasena(object sender, EventArgs e)
        {
            Object[] datos = new Object[2];
            datos[0] = this.UserName.Text;
            datos[1] = this.PasswordOld.Text;

            if (controladoraRH.loggeado(datos[0].ToString()) == false)
            {
                if (controladoraRH.usuarioMiembroEquipo(datos))
                {
                    //llamar metodo con confirmacion de modificacion de contrasena exitosa
                    Object[] datos1 = new Object[2];
                    datos1[0] = this.UserName.Text;
                    datos1[1] = this.PasswordNew.Text;
                    controladoraRH.modificaContrasena(datos1);

                    cambiado = 1;
                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Contraseña y usuario no coinciden" + "');", true);
                }
            }
        }
    }
}