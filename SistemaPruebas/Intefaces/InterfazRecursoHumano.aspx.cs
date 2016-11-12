using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using SistemaPruebas.Controladoras;


namespace SistemaPruebas.Intefaces
{
    public partial class InterfazRecursoHumano : System.Web.UI.Page
    {

        ControladoraRecursosHumanos controladoraRecursosHumanos = new ControladoraRecursosHumanos();

        private static int modo=0;  //Numero para identificar accion del boton Aceptar
        //Opciones: 1. Insertar, 2. Modificar, 3. Eliminar, 4. Consultar
        static String cedulaConsulta = "";
        //private static bool esAdmin = false;
        //private static int cedulaLoggeado;
        //Controladoras.ControladoraProyecto controladoraProyecto = new Controladoras.ControladoraProyecto();

        public static string cedulaLoggeado
        {
            get
            {
                object value = HttpContext.Current.Session["cedulaLoggeado"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["cedulaLoggeado"] = value;
            }
        }
        public static bool esAdmin
        {
            get
            {
                object value = HttpContext.Current.Session["esAdmin"];
                return value == null ? true : Convert.ToBoolean(value);
            }
            set
            {
                HttpContext.Current.Session["esAdmin"] = value;
            }
        }


        /*
         * Requiere: Que suceda el evento de refrescar la pagina
         * Modifica: Refresca la pagina.
         * Retorna: N/A.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            Restricciones_Campos();
            if (!IsPostBack)
            {
                esAdmin = controladoraRecursosHumanos.loggeadoEsAdmin();
                cedulaLoggeado = controladoraRecursosHumanos.idDelLoggeado().ToString();
                volverAlOriginal();
                if (!esAdmin)
                {
                    RH.Visible = false;
                }
                else
                {
                    llenarGrid();
                }
            }            
           // RH.Enabled = false;
        }

        /*
         * Requiere: N/A
         * Modifica: Designa un maximo de caracteres aceptados en los espacios.
         * Retorna: N/A.
         */
        protected void Restricciones_Campos()
        {
            TextBoxCedulaRH.MaxLength = 9;
            TextBoxNombreRH.MaxLength = 50;
            TextBoxEmail.MaxLength = 30;
            TextBoxTel1.MaxLength = 8;
            TextBoxTel2.MaxLength = 8;
            TextBoxUsuario.MaxLength = 30;
            TextBoxClave.MaxLength = 12;
        }

        /*
         * Requiere: N/A
         * Modifica: Carga el grid de consultar recursos humanos.
         * Retorna: N/A.
         */
        protected void llenarGrid()        //se encarga de llenar el grid cada carga de pantalla
        {
            DataTable recursosHumanos = crearTablaRH();
            DataTable dt = controladoraRecursosHumanos.consultarRecursoHumano(1, 0); // en consultas tipo 1, no se necesita la cédula

            Object[] datos = new Object[4];
        

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[0];
                    datos[1] = dr[1];
                    datos[2] = dr[2];
                    int id = Convert.ToInt32(dr[3]);
                    String nomp = controladoraRecursosHumanos.solicitarNombreProyecto(id);
                    datos[3] = nomp;
                    recursosHumanos.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                datos[2] = "-";
                datos[3] = "-";
                recursosHumanos.Rows.Add(datos);
            }
            RH.DataSource = recursosHumanos;
            RH.DataBind();

        }

        /*
         * Requiere: Cédula
         * Modifica: Carga los datos del recurso humano consultado en sus respectivas posisiones en la pantalla.
         * Retorna: N/A.
         */
        void llenarDatosRecursoHumano(int cedula)
        {
            DataTable dt = controladoraRecursosHumanos.consultarRecursoHumano(2, cedula); // Consulta tipo 2, para llenar los campos de un recurso humano

            BotonRHEliminar.Enabled = true;
            BotonRHModificar.Enabled = true;
            try
            {
                TextBoxCedulaRH.Text = dt.Rows[0].ItemArray[0].ToString();
                TextBoxNombreRH.Text = dt.Rows[0].ItemArray[1].ToString();
                TextBoxTel1.Text = dt.Rows[0].ItemArray[2].ToString();
                TextBoxTel2.Text = dt.Rows[0].ItemArray[3].ToString();
                TextBoxEmail.Text = dt.Rows[0].ItemArray[4].ToString();
                TextBoxUsuario.Text = dt.Rows[0].ItemArray[5].ToString();
                TextBoxClave.Text = dt.Rows[0].ItemArray[6].ToString();
                PerfilAccesoComboBox.ClearSelection();
                PerfilAccesoComboBox.Items.FindByText(dt.Rows[0].ItemArray[7].ToString()).Selected = true;
                RolComboBox.ClearSelection();
                seleccionRolEnConsulta(dt.Rows[0].ItemArray[7].ToString());
                RolComboBox.Items.FindByText(dt.Rows[0].ItemArray[8].ToString()).Selected = true;
                ProyectoAsociado.ClearSelection();
                ProyectoAsociado.Items.FindByValue(dt.Rows[0].ItemArray[9].ToString()).Selected = true;
            }
            catch
            {
                EtiqErrorConsultar.Visible = true;
            }
            //Response.Write(dt.Rows.Co)

        }

        /*
         * Requiere: Evento de cambiar la opcion seleccionada
         * Modifica: Bloqua el dropdownlist de rol.
         * Retorna: N/A.
         */
        protected void PerfilAccesoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PerfilAccesoComboBox.SelectedItem.Text == "Administrador")
            {
                RolComboBox.Enabled = false;
                RolComboBox.Items.Clear();
                RolComboBox.Items.Add(new ListItem("Administrador"));
                ProyectoAsociado.Enabled = false;
                ProyectoAsociado.Items.Clear();
                ProyectoAsociado.Items.Add(new ListItem("No aplica", "-1"));
            }
            else
            {
                RolComboBox.Items.Clear();
                llenarDDRol();
                llenarDDProyecto();
                RolComboBox.Enabled = true;
                ProyectoAsociado.Enabled = true;
            }
        }

        /*
         * Requiere: tipo.
         * Modifica: Selecciona el rol en la consulta.
         * Retorna: N/A.
         */
        protected void seleccionRolEnConsulta(String tipo)
        {
            if(tipo == "Administrador")
            {
                RolComboBox.Items.Clear();
                RolComboBox.Items.Add(new ListItem("Administrador"));
                RolComboBox.Enabled = false;
            }
            else
            {
                RolComboBox.Items.Clear();
                llenarDDRol();
            }
        }

        /*
         * Requiere: evento click en el boton insertar.
         * Modifica: Intenta insertar una tupla y despliega el respectivo mensaje de exito u error
         * Retorna: N/A.
         */

        protected void BotonRHInsertar_Click(object sender, EventArgs e)
        {
            modo = 1;
            habilitarCampos();
            llenarDDPerfil();
            llenarDDRol();
            llenarDDProyecto();
            desactivarErrores();
            BotonRHAceptar.Visible = true;
            BotonRHAceptar.Enabled = true;
            BotonRHCancelar.Enabled = true;
            BotonRHInsertar.Enabled = false;
            BotonRHModificar.Enabled = false;
            BotonRHInsertar.Enabled = false;
            BotonRHEliminar.Enabled = false;
            TextBoxCedulaRH.Text = "";
            TextBoxNombreRH.Text = "";
            TextBoxEmail.Text = "";
            TextBoxTel1.Text = "";
            TextBoxTel2.Text = "";
            TextBoxUsuario.Text = "";
            TextBoxClave.Text = "";
            marcarBoton(ref BotonRHInsertar);
            deshabilitarGrid();
        }

        /*
         * Requiere: Evento de click en boton cancelar.
         * Modifica: Borra los cambios que el usuario hizo y vuelve a como estaba antes de que el usuario intentara insertar o modificar una tupla de recursos humanos
         * Retorna: N/A.
         */
        protected void BotonRHCancelar_Click(object sender, EventArgs e)
        {
            if(modo == 2)
                controladoraRecursosHumanos.UpdateUsoRH(Int32.Parse(cedulaConsulta), 0);    //ya no está en uso
            desmarcarBotones();
            deshabilitarCampos();
            if (modo==2)
            {
                if (esAdmin)
                {
                    volverAlOriginal();
                    BotonRHEliminar.Enabled = true;
                    BotonRHModificar.Enabled = true;
                    llenarDatosRecursoHumano(Int32.Parse(cedulaConsulta));

                }
                else
                {
                    volverAlOriginal();
                }
            }
            else if (modo==1)
            {
                volverAlOriginal();
            }
            modo=0;
           
        }

        /*
         * Requiere: N/A.
         * Modifica: Vuelve al inicio de Recursos Humanos.
         * Retorna: N/A.
         */
        protected void volverAlOriginal()
        {
            botonesInicio();
            desactivarErrores();
            deshabilitarCampos();
            llenarDDPerfil();
            llenarDDRol();
            llenarDDProyecto();
            if (esAdmin) {
                TextBoxCedulaRH.Text = "";
                TextBoxNombreRH.Text = "";
                TextBoxEmail.Text = "";
                TextBoxTel1.Text = "";
                TextBoxTel2.Text = "";
                TextBoxUsuario.Text = "";
                TextBoxClave.Text = "";
                BotonRHAceptarModificar.Visible = false;
                BotonRHAceptar.Visible = true;
                BotonRHAceptarModificar.Enabled = false;
                BotonRHEliminar.Enabled = false;
                habilitarGrid();
                llenarGrid();
            }
            else
            {
                //consulta y cargar datos del usuario actual
                this.llenarDatosRecursoHumano(controladoraRecursosHumanos.idDelLoggeado());
                BotonRHModificar.Enabled = true;
                BotonRHAceptarModificar.Visible = true;
                BotonRHAceptarModificar.Enabled = false;
                BotonRHCancelar.Enabled = false;
                BotonRHAceptar.Visible = false;
                BotonRHEliminar.Enabled = false;
                BotonRHEliminar.Visible = false;
                BotonRHInsertar.Visible = false;
                RH.Visible = false;
            } 




        }

        /*
         * Requiere: Evento click en boton aceptar de insertar.
         * Modifica: Intenta insertar una tupla de recurso humano en la base de datos y despliega el respectivo mensaje de error o exito.
         * Retorna: N/A.
         */
        protected void BotonRHAceptar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                Object[] datosNuevos = new Object[11];
                datosNuevos[0] = this.TextBoxCedulaRH.Text;//cedula
                datosNuevos[1] = this.TextBoxNombreRH.Text;//nombre
                datosNuevos[2] = this.TextBoxTel1.Text;
                datosNuevos[3] = this.TextBoxTel2.Text;
                datosNuevos[4] = this.TextBoxEmail.Text;
                datosNuevos[5] = this.TextBoxUsuario.Text;//nombre de usuario
                datosNuevos[6] = this.TextBoxClave.Text;
                datosNuevos[7] = this.PerfilAccesoComboBox.SelectedItem.Text.ToString();
                datosNuevos[8] = this.ProyectoAsociado.SelectedValue;
                datosNuevos[9] = this.RolComboBox.SelectedValue.ToString();
                datosNuevos[10] = 1;

                int insercion = controladoraRecursosHumanos.insertarRecursoHumano(datosNuevos);
                if (insercion == 1)
                {
                    modo = 0;
                    desmarcarBotones();
                    deshabilitarCampos();
                    BotonRHInsertar.Enabled = true;
                    BotonRHModificar.Enabled = true;
                    BotonRHEliminar.Enabled = true;
                    BotonRHCancelar.Enabled = false;
                    BotonRHAceptar.Enabled = false;
                    habilitarGrid();
                    llenarGrid();
                    EtiqErrorGen.Text = "El recurso humano ha sido insertado con éxito";
                    EtiqErrorGen.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El recurso humano ha sido insertado con éxito');", true);
                    desmarcarBotones();
                    resaltarNuevo(this.TextBoxCedulaRH.Text);
                }
                else if(insercion == 2627)
                {
                    EtiqErrorLlaves.Visible = true;
                }
                else
                {
                    EtiqErrorInsertar.Visible = true;
                }
            }
    
        }

        protected void ProyectoAsociado_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void RolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        /*
         * Requiere: N/A.
         * Modifica: Llena el dropdownlist de proyecto.
         * Retorna: N/A.
         */
        protected void llenarDDProyecto()
        {

            this.ProyectoAsociado.Items.Clear();

            String proyectos = controladoraRecursosHumanos.solicitarProyectos();
            String[] pr = proyectos.Split(';');

            foreach (String p1 in pr)
            {
                String[] p2 = p1.Split('_');
                try
                {
                    this.ProyectoAsociado.Items.Add(new ListItem(p2[0], p2[1]));
                }
                catch (Exception e)
                {

                }
                
            }
        }

        /*
         * Requiere: N/A.
         * Modifica: Llena el dropdownlist de perfil de acceso.
         * Retorna: N/A.
         */
        protected void llenarDDPerfil()
        {
            this.PerfilAccesoComboBox.Items.Clear();
            string[] tipos = new string[] { "Miembro de equipo", "Administrador" };

            for (int i = 0; i < tipos.Length; i++)
            {
                this.PerfilAccesoComboBox.Items.Add(new ListItem(tipos[i]));
            }

        }

        /*
         * Requiere: N/A.
         * Modifica: Llena el dropdownlist de Rol.
         * Retorna: N/A.
         */
        protected void llenarDDRol()
        {
            this.RolComboBox.Items.Clear();
            string[] tipos = new string[] { "No aplica", "Líder de desarrollo", "Líder de pruebas", "Programador", "Tester" };

            for (int i = 0; i < tipos.Length; i++)
            {
                this.RolComboBox.Items.Add(new ListItem(tipos[i]));
            }

        }

        /*
         * Requiere: N/A.
         * Modifica: Habilita los campos para que el usuario pueda editar la informacion.
         * Retorna: N/A.
         */
        protected void habilitarCampos()
        {
            TextBoxEmail.Enabled = true;
            TextBoxTel1.Enabled = true;
            TextBoxTel2.Enabled = true;
            BotonRHCancelar.Enabled = true;
            if (esAdmin)
            {
                TextBoxClave.Enabled = true;
                TextBoxUsuario.Enabled = true;
                TextBoxCedulaRH.Enabled = true;
                TextBoxNombreRH.Enabled = true;
                RolComboBox.Enabled = true;
                PerfilAccesoComboBox.Enabled = true;
                ProyectoAsociado.Enabled = true;
                BotonRHAceptar.Enabled = true;
            } else
            {
                BotonRHAceptarModificar.Enabled = true;
            }

        }

        /*
         * Requiere: N/A.
         * Modifica: Deshabilita los campos para que el usuario pueda no editar la informacion.
         * Retorna: N/A.
         */
        protected void deshabilitarCampos()
        {

            TextBoxEmail.Enabled = false;
            TextBoxTel1.Enabled = false;
            TextBoxTel2.Enabled = false;
            BotonRHCancelar.Enabled = false;
            TextBoxClave.Enabled = false;
            TextBoxUsuario.Enabled = false;
            TextBoxCedulaRH.Enabled = false;
            TextBoxNombreRH.Enabled = false;
            RolComboBox.Enabled = false;
            PerfilAccesoComboBox.Enabled = false;
            ProyectoAsociado.Enabled = false;
            BotonRHAceptar.Enabled = false;
            BotonRHAceptarModificar.Enabled = false;
            
        }

        /*
         * Requiere: N/A.
         * Modifica: Resetea los botones para que vuelvan a estar como al inicio de todo.
         * Retorna: N/A.
         */
        protected void botonesInicio()
        {
            BotonRHCancelar.Enabled = false;
            if (esAdmin)
            {
                BotonRHEliminar.Enabled = false;
                BotonRHModificar.Enabled = false;
                BotonRHAceptar.Enabled = false;
                BotonRHInsertar.Enabled = true;
            }
            else
            {
                BotonRHAceptarModificar.Enabled = false;
                BotonRHModificar.Enabled = true;
            }
        }

        /*
         * Requiere: N/A.
         * Modifica: Activa y desactiva los botones al cancelar.
         * Retorna: N/A.
         */
        protected void botonesCancelar() //Estado de los botones después de apretar 
             
        {
            desmarcarBotones();
            if (esAdmin)
            {
                BotonRHInsertar.Enabled = true;
                if (RH.Rows.Count > 0)
                {
                    BotonRHModificar.Enabled = true;
                    BotonRHEliminar.Enabled = true;
                }
            }
            else
            {
                BotonRHModificar.Enabled = true;
            }
              
        }

        /*
         * Requiere: N/A.
         * Modifica: Habilita los botones de Modificar y Eiminar.
         * Retorna: N/A.
         */
        protected void habilitarBotonesME()
        {
            BotonRHEliminar.Enabled = true;
            BotonRHModificar.Enabled = true;
            BotonRHAceptar.Enabled = false;
            BotonRHCancelar.Enabled = false;
        }

        /*
         * Requiere: N/A.
         * Modifica: Valida el campo de email.
         * Retorna: booleano
         */
        protected bool validarCampos()
        {
            desactivarErrores();
            bool todosValidos = true;

            Regex emailRE = new Regex("(([a-zA-z,.-_#%]+@[a-zA-z,.-_#%]+.[a-zA-z,.-_#%]+)?){0,29}");
            if ((TextBoxEmail.Text != "") &&
                (!Regex.IsMatch(TextBoxEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase)))//emailRE.IsMatch(TextBoxEmail.Text))
            {
                //todosValidos = false;
                //EmailVal.Visible = true;
            }

            return todosValidos;
        }

        /*
         * Requiere: N/A.
         * Modifica: Desactiva todos los errores.
         * Retorna: N/A.
         */
        protected void desactivarErrores()
        {

            //CedVal.Visible = false;
            ////NombVal.Visible = false;
            //TelVal1.Visible = false;
            ////TelVal2.Visible = false;
            //EmailVal.Visible = false;
            //UserVal.Visible = false;
            //ClaveVal.Visible = false;
            EtiqErrorEliminar.Visible = false;
            EtiqErrorInsertar.Visible = false;
            EtiqErrorModificar.Visible = false;
            EtiqErrorLlaves.Visible = false;
            EtiqErrorConsultar.Visible = false;
        }

        /*
         * Requiere: Evento click en boton aceptar de modificar.
         * Modifica: Intenta insertar una tupla de recurso humano en la base de datos y despliega el respectivo mensaje de error o exito.
         * Retorna: N/A.
         */
        protected void BotonRHAceptarModificar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                Object[] datosNuevos = new Object[11];
                datosNuevos[0] = this.TextBoxCedulaRH.Text;//cedula
                datosNuevos[1] = this.TextBoxNombreRH.Text;//nombre
                datosNuevos[2] = this.TextBoxTel1.Text;
                datosNuevos[3] = this.TextBoxTel2.Text;
                datosNuevos[4] = this.TextBoxEmail.Text;
                datosNuevos[5] = this.TextBoxUsuario.Text;//nombre de usuario
                datosNuevos[6] = this.TextBoxClave.Text;
                datosNuevos[7] = this.PerfilAccesoComboBox.SelectedItem.Text.ToString();
                datosNuevos[8] = this.ProyectoAsociado.SelectedValue.ToString();
                datosNuevos[9] = this.RolComboBox.SelectedValue.ToString();
                datosNuevos[10] = cedulaConsulta;

            if (controladoraRecursosHumanos.modificarRecursoHumano(datosNuevos) == 1)
            {
                desmarcarBotones();
                deshabilitarCampos();
                BotonRHModificar.Enabled = true;
                BotonRHCancelar.Enabled = false;
                BotonRHAceptarModificar.Enabled = false;
                modo = 0;
                if (esAdmin)
                {
                    habilitarGrid();
                    BotonRHInsertar.Enabled = true;
                    BotonRHEliminar.Enabled = true;
                    llenarGrid();
					controladoraRecursosHumanos.UpdateUsoRH(Int32.Parse(TextBoxCedulaRH.Text.ToString()), 0);//ya fue modificado el RH
                    EtiqErrorGen.Text = "El recurso humano ha sido modificado con éxito";
                    EtiqErrorGen.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El recurso humano ha sido modificado con éxito');", true);
                    resaltarNuevo(this.TextBoxCedulaRH.Text);
     
                }
                else
                {
					controladoraRecursosHumanos.UpdateUsoRH(Int32.Parse(TextBoxCedulaRH.Text.ToString()), 0);//ya fue modificado el RH
                    EtiqErrorGen.Text = "Su informacion ha sido actualizada exitosamente";
                    EtiqErrorGen.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Su informacion ha sido actualizada exitosamente');", true);
                }
                //habilitar consulta
            }
            else
            {
                EtiqErrorModificar.Visible = true;
                //mensaje de error
            }
            }
        }

        /*
         * Requiere: Evento click en boton eliminar.
         * Modifica: Habilita y deshabilita los espacios y botones que se requieren para que el usuario sea capaz de modificar segun el tipo de perfil que tiene.
         * Retorna: N/A.
         */
        protected void BotonRHModificar_Click(object sender, EventArgs e)
        {
			if (controladoraRecursosHumanos.ConsultarUsoRH(Int32.Parse(TextBoxCedulaRH.Text.ToString())) == false)
			{
				controladoraRecursosHumanos.UpdateUsoRH(Int32.Parse(TextBoxCedulaRH.Text.ToString()), 1);//está siendo modificado el recurso humano
				
				modo = 2;
				marcarBoton(ref BotonRHModificar);
				BotonRHModificar.Enabled = false;
				desactivarErrores();
				BotonRHAceptarModificar.Visible = true;
				BotonRHAceptarModificar.Enabled = true;
				cedulaConsulta = TextBoxCedulaRH.Text;
				BotonRHAceptar.Visible = false;
				BotonRHCancelar.Enabled = true;
				BotonRHInsertar.Enabled = false;
				BotonRHEliminar.Enabled = false;
				if (esAdmin)
				{
					deshabilitarGrid();
					PerfilAccesoComboBox.Enabled = false;
					if (PerfilAccesoComboBox.SelectedItem.Text == "Administrador")
					{
						RolComboBox.Enabled = false;
						ProyectoAsociado.Enabled = false;
					}
				}
				habilitarCampos();
				PerfilAccesoComboBox.Enabled = false;
				
			} else {
                EtiqErrorGen.Text = "El Recurso Humano consultado se encuentra actualmente en uso";
                EtiqErrorGen.ForeColor = System.Drawing.Color.Salmon;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El Recurso Humano consultado se encuentra actualmente en uso');", true);		            
	        } 
        }

        /*
         * Requiere: N/A.
         * Modifica: Inicializa y llena el grid de recursos humanos.
         * Retorna: dataTable
         */
        protected DataTable crearTablaRH()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Cédula", typeof(int));
            dt.Columns.Add("Nombre Completo", typeof(String));
            dt.Columns.Add("Rol", typeof(String));
            dt.Columns.Add("Nombre Proyecto");
            //dt.
            return dt;
        }

        /*
         * Requiere: Evento seleccionar un recurso humano.
         * Modifica: Carga los datos del RH seleccionado en pantalla.
         * Retorna: N/A.
         */
        protected void RH_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = RH.SelectedRow.RowIndex;
            String ced = RH.SelectedRow.Cells[0].Text;
            int cedula = Convert.ToInt32(ced);
            llenarDatosRecursoHumano(cedula);
            habilitarGrid();
        }

        /*
         * Requiere: El evento de enlazar información de un datatable con el grid
         * Modifica: Establece el comportamiento del grid ante los diferentes eventos.
         * Retorna: N/A.
         */
        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {   
        
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"]     =  Page.ClientScript.GetPostBackClientHyperlink(RH, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"]       = "cursor:pointer";


            }

        }

        /*
         * Requiere: Evento click en boton eliminar.
         * Modifica: Intenta insertar una tupla de recurso humano en la base de datos y despliega el respectivo mensaje de error o exito.
         * Retorna: N/A.
         */
        protected void BotonRHEliminar_Click(object sender, EventArgs e)
        {
            if (controladoraRecursosHumanos.ConsultarUsoRH(Int32.Parse(TextBoxCedulaRH.Text.ToString())) == false){
				if (controladoraRecursosHumanos.eliminarRecursoHumano(Convert.ToInt32(this.TextBoxCedulaRH.Text.ToString())) == 1)
				{
                    EtiqErrorGen.Text = "El recurso humano ha sido eliminado con éxito";
                    EtiqErrorGen.ForeColor = System.Drawing.Color.DarkSeaGreen;                   
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
					//ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El recurso humano ha sido eliminado con éxito');", true);
					volverAlOriginal();
					llenarGrid();
					llenarGrid();
				}
				else
				{
					EtiqErrorEliminar.Visible = true;
				}
			}
            else
            {
                EtiqErrorGen.Text = "El Recurso Humano consultado se encuentra actualmente en uso";
                EtiqErrorGen.ForeColor = System.Drawing.Color.Salmon;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('');", true);		
	        }
        }

        /*
         * Requiere: Evento de pasar de página en el grid.
         * Modifica: Pasa de página y llena el grid con las n tuplas que siguen, siendo n el tamaño de la página.
         * Retorna: N/A. 
        */
        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            RH.PageIndex = e.NewPageIndex;
            this.llenarGrid();
        }

        /*
         * Requiere: ref Button.
         * Modifica: Marca un boton.
         * Retorna: N/A.
         */
        protected void marcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.ColorTranslator.FromHtml("#20bcae");
            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#20bcae");
            b.ForeColor = System.Drawing.Color.White;
        }

        /*
         * Requiere: ref Button.
         * Modifica: Desmarca un boton.
         * Retorna: N/A.
         */
        protected void desmarcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.Color.LightGray;
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;

        }

        /*
         * Requiere: N/A..
         * Modifica: Desmarca todos los botones.
         * Retorna: N/A.
         */
        protected void desmarcarBotones()
        {
            desmarcarBoton(ref BotonRHInsertar);
            desmarcarBoton(ref BotonRHModificar);
            desmarcarBoton(ref BotonRHEliminar);
        }

        /*
         * Requiere: N/A.
         * Modifica: Deshabilita el grid de ser seleccionado.
         * Retorna: N/A.
         */
        protected void deshabilitarGrid()
        {
            RH.Enabled = false;
            foreach(GridViewRow row in RH.Rows)
            {
                row.Attributes.Remove("onclick");
                row.Attributes.Remove("onmouseover");
                row.Attributes.Remove("style");
                row.Attributes.Remove("onmouseout");
            }
        }

        /*
         * Requiere: N/A.
         * Modifica: Habilita la seleccion del grid.
         * Retorna: N/A.
         */
        protected void habilitarGrid()
        {
            RH.Enabled = true;
            foreach (GridViewRow row in RH.Rows)
            {
                row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(RH, "Select$" + row.RowIndex);
                row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";           
                row.Attributes["style"] = "cursor:pointer";
            }
        }

        /*
         * Requiere: Cedula del recurso humano más recientemente insertado o modificado.
         * Modifica: Mueve el grid para que se seleccione inmediatamente la página donde está la tupla agregada o modificada.
         * Retorna: N/A.      
        */

        protected void resaltarNuevo(String cedulaNuevo)
        {
            RH.AllowPaging = false;
            RH.DataBind();
            int i = 0;
            foreach (GridViewRow row in RH.Rows)
            {
                if (row.Cells[0].Text == cedulaNuevo)
                {
                    int pos = i / RH.PageSize;
                    RH.PageIndex = pos;
                    row.BackColor = System.Drawing.Color.Crimson;
                }
                i++;
            }
            RH.AllowPaging = true;
            RH.DataBind();
        }

        protected void aceptarModal_Click(object sender, EventArgs e)
        {
            if (controladoraRecursosHumanos.ConsultarUsoRH(Int32.Parse(TextBoxCedulaRH.Text.ToString())) == false)
            {
                if (controladoraRecursosHumanos.eliminarRecursoHumano(Convert.ToInt32(this.TextBoxCedulaRH.Text.ToString())) == 1)
                {
                    EtiqErrorGen.Text = "El recurso humano ha sido eliminado con éxito";
                    EtiqErrorGen.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El recurso humano ha sido eliminado con éxito');", true);
                    volverAlOriginal();
                    llenarGrid();
                    llenarGrid();
                }
                else
                {
                    EtiqErrorEliminar.Visible = true;
                }
            }
            else
            {
                EtiqErrorGen.Text = "El Recurso Humano consultado se encuentra actualmente en uso";
                EtiqErrorGen.ForeColor = System.Drawing.Color.Salmon;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('');", true);		
            }
        }

        protected void cancelarModal_Click(object sender, EventArgs e)
        {

        }

        protected void cancelar_Click(object sender, EventArgs e)
        {
            if (modo == 2)
                controladoraRecursosHumanos.UpdateUsoRH(Int32.Parse(cedulaConsulta), 0);    //ya no está en uso
            desmarcarBotones();
            deshabilitarCampos();
            if (modo == 2)
            {
                if (esAdmin)
                {
                    volverAlOriginal();
                    BotonRHEliminar.Enabled = true;
                    BotonRHModificar.Enabled = true;
                    llenarDatosRecursoHumano(Int32.Parse(cedulaConsulta));

                }
                else
                {
                    volverAlOriginal();
                }
            }
            else if (modo == 1)
            {
                volverAlOriginal();
            }
            modo = 0;
        }
    }
}