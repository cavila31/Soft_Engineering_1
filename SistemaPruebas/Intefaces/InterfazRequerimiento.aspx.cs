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
    public partial class InterfazRequerimiento : System.Web.UI.Page
    {

        ControladoraRequerimiento controladoraRequerimiento = new ControladoraRequerimiento();


        // Variables de sesion. Gets y Sets
        public static string modoREQ
        {
            get
            {
                object value = HttpContext.Current.Session["modoREQ"];
                return value == null ? "0" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["modoREQ"] = value;
            }
        }

        public static string idViejoREQ 
        {
            get
            {
                object value = HttpContext.Current.Session["id_modificando"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["id_modificando"] = value;
            }
        }

        public static string esAdminREQ
        {
            get
            {
                object value = HttpContext.Current.Session["esAdminREQ"];
                return value == null ? "false" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["esAdminREQ"] = value;
            }
        }

        public static string proyectoActual
        {
            get
            {
                object value = HttpContext.Current.Session["proyectoActual"];
                return value == null ? "0" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["proyectoActual"] = value;
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
                esAdminREQ = controladoraRequerimiento.PerfilDelLogeado().ToString();
                if (Convert.ToBoolean(esAdminREQ))
                {
                    //proyectoActual = controladoraRequerimiento.consultarIDProyMinimo().ToString();
                }
                else
                {
                    proyectoActual = ((controladoraRequerimiento.proyectosDelLoggeado()).ToString()).ToString();
                }
                volverAlOriginal();
            }
            if (Convert.ToBoolean(esAdminREQ))
            {
                proyectoActual = this.ProyectoAsociado.SelectedValue.ToString();
            }
            else
            {
                proyectoActual = ((controladoraRequerimiento.proyectosDelLoggeado()).ToString()).ToString();
            }
            llenarGrid();
            llenarProyectoResumen();
        }

        protected void llenarProyectoResumen()
        {
            ControladoraProyecto contP = new ControladoraProyecto();
            EntidadProyecto ent = contP.ConsultarProyecto(Convert.ToInt32(proyectoActual));

            nombre_sistema.Text = ent.Nombre_sistema;
            nombre_rep.Text = ent.Nombre_representante;
            objetivo_general.Text = ent.Objetivo_general;
            LiderProyecto.Text =  ent.LiderProyecto;
            string el_estado = ent.Estado;
            if (el_estado == "1") 
            {
                estado.Text = "Pendiente";
            }
            else if (el_estado == "2")
            {
                estado.Text = "Asignado";
            }
            else if (el_estado == "3")
            {
                estado.Text = "En Ejecución";
            }
            else if (el_estado == "4")
            {
                estado.Text = "Finalizado";
            }
            else if (el_estado == "5")
            {
                estado.Text = "Cerrado";
            }
        }

        /*
         * Requiere: N/A
         * Modifica: Designa un maximo de caracteres aceptados en los espacios.
         * Retorna: N/A.
         */
        protected void Restricciones_Campos()
        {
            TextBoxNombreREQ.MaxLength = 6;
            NombreTxtbox.MaxLength = 75;
            TextBoxPrecondicionesREQ.MaxLength = 150;
            TextBoxRequerimientosEspecialesREQ.MaxLength = 150;
        }

        /*
         * Requiere: N/A
         * Modifica: Carga el grid de consultar requerimiento..
         * Retorna: N/A.
         */
        protected void llenarGrid()       
        {
            
            DataTable Requerimiento = crearTablaREQ();
            DataTable dt;
            if (Convert.ToBoolean(esAdminREQ))
            {
                //dt = controladoraRequerimiento.consultarRequerimiento(1, ""); // en consultas tipo 1, no se necesita el id del proyecto asociado al usuario.
                proyectoActual = this.ProyectoAsociado.SelectedValue.ToString();
                dt = controladoraRequerimiento.consultarRequerimiento(3, Convert.ToString(proyectoActual));
            }
            else
            {
                dt = controladoraRequerimiento.consultarRequerimiento(3, Convert.ToString(controladoraRequerimiento.proyectosDelLoggeado()));
            }
            Object[] datos = new Object[3];
        

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[0];
                    int id = Convert.ToInt32(dr[2]);
                    String nomp = controladoraRequerimiento.solicitarNombreProyecto(id);
                    datos[1] = dr[1];
                    datos[2] = nomp;
                    Requerimiento.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                datos[2] = "-";
                Requerimiento.Rows.Add(datos);
            }
            gridRequerimiento.DataSource = Requerimiento;
            gridRequerimiento.DataBind();
            
        }

        /*
         * Requiere: Cédula
         * Modifica: Carga los datos del requerimiento consultado en sus respectivas posisiones en la pantalla.
         * Retorna: N/A.
         */
        void llenarDatosRequerimiento(String cedula)
        {
            DataTable dt = controladoraRequerimiento.consultarRequerimiento(2, cedula); // Consulta tipo 2, para llenar los campos de un requerimiento

            BotonREQEliminar.Enabled = true;
            BotonREQModificar.Enabled = true;
            try
            {
                TextBoxNombreREQ.Text = dt.Rows[0].ItemArray[0].ToString();
                TextBoxPrecondicionesREQ.Text = dt.Rows[0].ItemArray[1].ToString();
                TextBoxRequerimientosEspecialesREQ.Text = dt.Rows[0].ItemArray[2].ToString();
                NombreTxtbox.Text = dt.Rows[0].ItemArray[3].ToString();
                if (!Convert.ToBoolean(esAdminREQ))
                {
                    ProyectoAsociado.ClearSelection();
                    ProyectoAsociado.Items.FindByValue((controladoraRequerimiento.proyectosDelLoggeado()).ToString()).Selected = true;
                }
                else
                {
                    ProyectoAsociado.ClearSelection();
                    ProyectoAsociado.Items.FindByValue(dt.Rows[0].ItemArray[4].ToString()).Selected = true;
                }
            }
            catch
            {
                EtiqErrorLlaves.Text = "Ha habido problemas para consultar este requerimiento. Por favor intente mas tarde. ";
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                EtiqErrorLlaves.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
            }
            llenarGrid();

        }

        /*
         * Requiere: evento click en el boton insertar.
         * Modifica: Intenta insertar una tupla y despliega el respectivo mensaje de exito u error
         * Retorna: N/A.
         */

        protected void BotonREQInsertar_Click(object sender, EventArgs e)
        {
            modoREQ = Convert.ToString(1);
            habilitarCampos();
            if (!Convert.ToBoolean(esAdminREQ))
            {
                ProyectoAsociado.ClearSelection();
                ProyectoAsociado.Items.FindByValue((controladoraRequerimiento.proyectosDelLoggeado()).ToString()).Selected = true;
            }
            else
            {
                llenarDDProyecto();
                ProyectoAsociado.ClearSelection();
                ProyectoAsociado.Items.FindByValue((proyectoActual).ToString()).Selected = true;
            }
            desactivarErrores();
            BotonREQAceptar.Visible = true;
            BotonREQAceptar.Enabled = true;
            BotonREQAceptarModificar.Visible = false;
            BotonREQCancelar.Enabled = true;
            BotonREQInsertar.Enabled = false;
            BotonREQModificar.Enabled = false;
            BotonREQInsertar.Enabled = false;
            BotonREQEliminar.Enabled = false;
            TextBoxNombreREQ.Text = "";
            TextBoxPrecondicionesREQ.Text = "";
            TextBoxRequerimientosEspecialesREQ.Text = "";
            NombreTxtbox.Text = "";
            marcarBoton(ref BotonREQInsertar);
            deshabilitarGrid();
            llenarGrid();
        }

        /*
         * Requiere: Evento de click en boton cancelar.
         * Modifica: Borra los cambios que el usuario hizo y vuelve a como estaba antes de que el usuario intentara insertar o modificar una tupla de requerimiento
         * Retorna: N/A.
         */
        protected void BotonREQCancelar_Click(object sender, EventArgs e)
        {
            if (modoREQ == 2.ToString())
            {
                controladoraRequerimiento.UpdateUsoREQ(idViejoREQ, 0);    //ya no está en uso
            }
            desmarcarBotones();
            deshabilitarCampos();
            if (modoREQ==2.ToString())
            {
                    volverAlOriginal();
                    BotonREQEliminar.Enabled = true;
                    BotonREQModificar.Enabled = true;
                    llenarDatosRequerimiento(idViejoREQ);
            }
            else if (modoREQ==1.ToString())
            {
                volverAlOriginal();
            }
            modoREQ=0.ToString();
        }


        /*
         * Requiere: N/A.
         * Modifica: Vuelve al inicio de requerimiento.
         * Retorna: N/A.
         */
        protected void volverAlOriginal()
        {
            botonesInicio();
            desactivarErrores();
            deshabilitarCampos();
            llenarDDProyecto();
            modoREQ = Convert.ToString(0);
            if (!Convert.ToBoolean(esAdminREQ))
            {
                ProyectoAsociado.ClearSelection();
                ProyectoAsociado.Items.FindByValue((controladoraRequerimiento.proyectosDelLoggeado()).ToString()).Selected = true;
            }
            else
            {
                ProyectoAsociado.ClearSelection();
                ProyectoAsociado.Items.FindByValue((proyectoActual).ToString()).Selected = true;
            }
                TextBoxNombreREQ.Text = ".";
                TextBoxPrecondicionesREQ.Text = "";
                TextBoxRequerimientosEspecialesREQ.Text = "";
                NombreTxtbox.Text = "";
                BotonREQAceptarModificar.Visible = false;
                BotonREQAceptar.Visible = true;
                BotonREQAceptarModificar.Enabled = false;
                BotonREQEliminar.Enabled = false;
                habilitarGrid();
                llenarGrid();
                if (!Convert.ToBoolean(esAdminREQ))
                {
                    ProyectoAsociado.ClearSelection();
                    ProyectoAsociado.Items.FindByValue((controladoraRequerimiento.proyectosDelLoggeado()).ToString()).Selected = true;
                } else
            {
                ProyectoAsociado.ClearSelection();
                ProyectoAsociado.Items.FindByValue((proyectoActual).ToString()).Selected = true;
            }
            llenarGrid();
        }

        /*
         * Requiere: Evento click en boton aceptar de insertar.
         * Modifica: Intenta insertar una tupla de requerimiento en la base de datos y despliega el respectivo mensaje de error o exito.
         * Retorna: N/A.
         */
        protected void BotonREQAceptar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                Object[] datosNuevos = new Object[6];
                datosNuevos[0] = this.TextBoxNombreREQ.Text;
                datosNuevos[1] = this.TextBoxPrecondicionesREQ.Text;
                datosNuevos[2] = this.TextBoxRequerimientosEspecialesREQ.Text;
                datosNuevos[3] = this.ProyectoAsociado.SelectedValue;
                datosNuevos[4] = "1";
                datosNuevos[5] = this.NombreTxtbox.Text;

                int insercion = controladoraRequerimiento.insertarRequerimiento(datosNuevos);
                if (insercion == 1)
                {
                    modoREQ = Convert.ToString(0);
                    desmarcarBotones();
                    deshabilitarCampos();
                    BotonREQInsertar.Enabled = true;
                    BotonREQModificar.Enabled = true;
                    BotonREQEliminar.Enabled = true;
                    BotonREQCancelar.Enabled = false;
                    BotonREQAceptar.Enabled = false;
                    habilitarGrid();
                    llenarGrid();
                    EtiqErrorLlaves.Text = "El requerimiento se ha insertado correctamente";
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    EtiqErrorLlaves.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                    desmarcarBotones();
                    resaltarNuevo(this.TextBoxNombreREQ.Text);
                    llenarGrid();
                }
                else if(insercion == 2627)
                {
                    EtiqErrorLlaves.Text = "Hay un requerimiento con este id. Por favor cambielo y vuelva a intentarlo. ";
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                    EtiqErrorLlaves.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                    llenarGrid();
                }
                else
                {
                    EtiqErrorLlaves.Text = "Ha habido problemas para insertar este requerimiento. Por favor intente nuevamente. ";
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                    EtiqErrorLlaves.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                    llenarGrid();
                }
            }
        }

        /*
        * Requiere: Evento click en boton aceptar de modificar.
        * Modifica: Intenta modificar una tupla de requerimiento en la base de datos y despliega el respectivo mensaje de error o exito.
        * Retorna: N/A.
        */
        protected void BotonREQAceptarModificar_Click(object sender, EventArgs e)
        {
            modificarReq();
            llenarGrid();
        }
        /*
        * Requiere: N/A.
        * Modifica: Es el metodo en el que se encuentra la logica de lo que se debe hacer para modificar un requerimiento una vez que el usuario da aceptar.
        * Retorna: N/A.
        */
        protected void modificarReq()
        {
            if (validarCampos())
            {

                Object[] datosNuevos = new Object[6];
                datosNuevos[0] = this.TextBoxNombreREQ.Text;//id_Req
                datosNuevos[1] = this.TextBoxPrecondicionesREQ.Text;
                datosNuevos[2] = this.TextBoxRequerimientosEspecialesREQ.Text;
                datosNuevos[3] = this.ProyectoAsociado.SelectedValue;
                datosNuevos[4] = idViejoREQ;
                datosNuevos[5] = this.NombreTxtbox.Text;

                if (controladoraRequerimiento.modificarRequerimiento(datosNuevos) == 1)
                {
                    desmarcarBotones();
                    deshabilitarCampos();
                    BotonREQModificar.Enabled = true;
                    BotonREQCancelar.Enabled = false;
                    BotonREQAceptarModificar.Enabled = false;
                    modoREQ = Convert.ToString(0);
                    habilitarGrid();
                    BotonREQInsertar.Enabled = true;
                    BotonREQEliminar.Enabled = true;
                    llenarGrid();
                    controladoraRequerimiento.UpdateUsoREQ(TextBoxNombreREQ.Text.ToString(), 0);//ya fue modificado el REQ
                    resaltarNuevo(this.TextBoxNombreREQ.Text);
                    EtiqErrorLlaves.Text = "El requerimiento se ha modificado correctamente";
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    EtiqErrorLlaves.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                    llenarGrid();

                }
                else
                {
                    //mensaje de error
                    EtiqErrorLlaves.Text = "Ha habido problemas para modificar este requerimiento. Por favor intente mas tarde. ";
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                    EtiqErrorLlaves.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                }
            }
        }

        /*
        * Requiere: Evento cambia la opcion de proyecto seleccionada.
        * Modifica: Despliega en el grid los requerimientos asociados a ese proyecto.
        * Retorna: N/A.
        */
        protected void ProyectoAsociado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(esAdminREQ))
            {
                proyectoActual = this.ProyectoAsociado.SelectedValue.ToString();
            }
            else
            {
            }
            volverAlOriginal();
            llenarGrid();
            llenarProyectoResumen();
        }

        /*
         * Requiere: N/A.
         * Modifica: Llena el dropdownlist de proyecto.
         * Retorna: N/A.
         */
        protected void llenarDDProyecto()
        {

            this.ProyectoAsociado.Items.Clear();

            String proyectos = controladoraRequerimiento.solicitarProyectos();
            String[] pr = proyectos.Split(';');

            foreach (String p1 in pr)
            {
                String[] p2 = p1.Split('_');
                try
                {
                    if (Convert.ToInt32(p2[1]) > -1)
                    {
                        this.ProyectoAsociado.Items.Add(new ListItem(p2[0], p2[1]));
                    }
                }
                catch (Exception e)
                {

                }
                
            }
        }



        /*
         * Requiere: N/A.
         * Modifica: Habilita los campos para que el usuario pueda editar la informacion.
         * Retorna: N/A.
         */
        protected void habilitarCampos()
        {
            TextBoxNombreREQ.Enabled = true;
            TextBoxPrecondicionesREQ.Enabled = true;
            TextBoxRequerimientosEspecialesREQ.Enabled = true;
            NombreTxtbox.Enabled = true;
            BotonREQCancelar.Enabled = true;
            BotonREQAceptar.Enabled = true;
            if (Convert.ToBoolean(esAdminREQ))
            {
                ProyectoAsociado.Enabled = true;
                
            }
            else
            {
                ProyectoAsociado.Enabled = false;
            }
        }

        /*
         * Requiere: N/A.
         * Modifica: Deshabilita los campos para que el usuario pueda no editar la informacion.
         * Retorna: N/A.
         */
        protected void deshabilitarCampos()
        {

            TextBoxNombreREQ.Enabled = false;
            TextBoxPrecondicionesREQ.Enabled = false;
            TextBoxRequerimientosEspecialesREQ.Enabled = false;
            NombreTxtbox.Enabled = false;
            BotonREQCancelar.Enabled = false;
            if (Convert.ToBoolean(esAdminREQ))
            {
                ProyectoAsociado.Enabled = true;

            }
            else
            {
                ProyectoAsociado.Enabled = false;
            }
            BotonREQAceptar.Enabled = false;
            BotonREQAceptarModificar.Enabled = false;
            
        }

        /*
         * Requiere: N/A.
         * Modifica: Resetea los botones para que vuelvan a estar como al inicio de todo.
         * Retorna: N/A.
         */
        protected void botonesInicio()
        {
                BotonREQCancelar.Enabled = false;
                BotonREQEliminar.Enabled = false;
                BotonREQModificar.Enabled = false;
                BotonREQAceptar.Enabled = false;
                BotonREQInsertar.Enabled = true;
        }

        /*
         * Requiere: N/A.
         * Modifica: Activa y desactiva los botones al cancelar.
         * Retorna: N/A.
         */
        protected void botonesCancelar() //Estado de los botones después de apretar 
        {
            desmarcarBotones();
                BotonREQInsertar.Enabled = true;
                if (gridRequerimiento.Rows.Count > 0)
                {
                    BotonREQModificar.Enabled = true;
                    BotonREQEliminar.Enabled = true;
                }
              
        }

        /*
         * Requiere: N/A.
         * Modifica: Habilita los botones de Modificar y Eiminar.
         * Retorna: N/A.
         */
        protected void habilitarBotonesME()
        {
            BotonREQEliminar.Enabled = true;
            BotonREQModificar.Enabled = true;
            BotonREQAceptar.Enabled = false;
            BotonREQCancelar.Enabled = false;
        }

        /*
         * Requiere: N/A.
         * Modifica: N/A.
         * Retorna: N/A.
         */
        protected bool validarCampos()
        {
            desactivarErrores();
            bool todosValidos = true;
            return todosValidos;
        }

        /*
         * Requiere: N/A.
         * Modifica: Desactiva todos los errores.
         * Retorna: N/A.
         */
        protected void desactivarErrores()
        {

            EtiqErrorPrecondiciones.Visible = false;
            EtiqErrorReqEsp.Visible = false;
            EtiqErrorNombre.Visible = false;
            EtiqErrorEliminar.Visible = false;
            EtiqErrorInsertar.Visible = false;
            EtiqErrorModificar.Visible = false;
            EtiqErrorLlaves.Visible = false;
            EtiqErrorConsultar.Visible = false;
        }



        /*
         * Requiere: Evento click en boton eliminar.
         * Modifica: Habilita y deshabilita los espacios y botones que se requieren para que el usuario sea capaz de modificar segun el tipo de perfil que tiene.
         * Retorna: N/A.
         */
        protected void BotonREQModificar_Click(object sender, EventArgs e)
        {
			if (controladoraRequerimiento.ConsultarUsoREQ(TextBoxNombreREQ.Text.ToString()) == false)
			{
				controladoraRequerimiento.UpdateUsoREQ(TextBoxNombreREQ.Text.ToString(), 1);//está siendo modificado el requerimiento
				
				modoREQ = Convert.ToString(2);
				marcarBoton(ref BotonREQModificar);
				BotonREQModificar.Enabled = false;
				desactivarErrores();
				BotonREQAceptarModificar.Visible = true;
				BotonREQAceptarModificar.Enabled = true;
				idViejoREQ = TextBoxNombreREQ.Text;
				BotonREQAceptar.Visible = false;
				BotonREQCancelar.Enabled = true;
				BotonREQInsertar.Enabled = false;
				BotonREQEliminar.Enabled = false;
				if (Convert.ToBoolean(esAdminREQ))
				{
					deshabilitarGrid();
				}
				habilitarCampos();
				
			} else {	
                EtiqErrorLlaves.Text = "Otro usuario esta modificando este requerimiento por lo que pude ser eliminado. Por favor intente mas tarde. ";
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                EtiqErrorLlaves.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
	        } 
        }

        /*
         * Requiere: N/A.
         * Modifica: Inicializa y llena el grid de requerimientos.
         * Retorna: N/A.
         */
        protected DataTable crearTablaREQ()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID Requerimiento", typeof(String));
            dt.Columns.Add("Nombre del Requerimiento", typeof(String));
            dt.Columns.Add("Nombre Proyecto");
            return dt;
        }

        /*
         * Requiere: Evento seleccionar un requerimiento.
         * Modifica: Carga los datos del REQ seleccionado en pantalla.
         * Retorna: N/A.
         */
        protected void gridRequerimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = gridRequerimiento.SelectedRow.RowIndex;
            String ced = gridRequerimiento.SelectedRow.Cells[0].Text;
            llenarDatosRequerimiento(ced);
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
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridRequerimiento, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"]       = "cursor:pointer";

            }

        }

        /*
         * Requiere: Evento click en boton eliminar.
         * Modifica: Intenta insertar una tupla de requerimiento en la base de datos y despliega el respectivo mensaje de error o exito.
         * Retorna: N/A.
         */
        protected void BotonREQEliminar_Click(object sender, EventArgs e)
        {
            marcarBoton(ref BotonREQEliminar);
            modoREQ = Convert.ToString(3);
            idViejoREQ=TextBoxNombreREQ.Text;
        }

       

        /*
         * Requiere: Evento de pasar de página en el grid.
         * Modifica: Pasa de página y llena el grid con las n tuplas que siguen, siendo n el tamaño de la página.
         * Retorna: N/A. 
        */
        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridRequerimiento.PageIndex = e.NewPageIndex;
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
            desmarcarBoton(ref BotonREQInsertar);
            desmarcarBoton(ref BotonREQModificar);
            desmarcarBoton(ref BotonREQEliminar);
        }

        /*
         * Requiere: N/A.
         * Modifica: Deshabilita el grid de ser seleccionado.
         * Retorna: N/A.
         */
        protected void deshabilitarGrid()
        {
            gridRequerimiento.Enabled = false;
            foreach(GridViewRow row in gridRequerimiento.Rows)
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
            gridRequerimiento.Enabled = true;
            foreach (GridViewRow row in gridRequerimiento.Rows)
            {
                row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridRequerimiento, "Select$" + row.RowIndex);
                row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";           
                row.Attributes["style"] = "cursor:pointer";

            }
        }

        /*
         * Requiere: ID del requerimiento más recientemente insertado o modificado.
         * Modifica: Mueve el grid para que se seleccione inmediatamente la página donde está la tupla agregada o modificada.
         * Retorna: N/A.      
        */

        protected void resaltarNuevo(String cedulaNuevo)
        {
            gridRequerimiento.AllowPaging = false;
            gridRequerimiento.DataBind();
            int i = 0;
            foreach (GridViewRow row in gridRequerimiento.Rows)
            {
                if (row.Cells[0].Text == cedulaNuevo)
                {
                    int pos = i / gridRequerimiento.PageSize;
                    gridRequerimiento.PageIndex = pos;
                    row.BackColor = System.Drawing.Color.Crimson;
                }
                i++;
            }
            gridRequerimiento.AllowPaging = true;
            gridRequerimiento.DataBind();
        }
        /*
         * Requiere: Que el usuario de clic en el aceptar cuando le aparece el modal.
         * Modifica: Elimina un requerimiento, luego de la confirmacion.
         * Retorna: N/A.      
        */

        protected void aceptarModal_Click(object sender, EventArgs e)
        {
            idViejoREQ = TextBoxNombreREQ.Text.ToString();
            if (controladoraRequerimiento.ConsultarUsoREQ(TextBoxNombreREQ.Text.ToString()) == false)
            {
                desmarcarBoton(ref BotonREQEliminar);
                int proyecto = Convert.ToInt32( this.ProyectoAsociado.SelectedValue);
                Console.WriteLine("Eliminar");
                if (controladoraRequerimiento.eliminarRequerimiento(this.TextBoxNombreREQ.Text.ToString(),proyecto) == 1)
                {
                    volverAlOriginal();
                    EtiqErrorLlaves.Text = "El requerimiento se ha eliminado correctamente";
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    EtiqErrorLlaves.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                    idViejoREQ = "";
                    
                }
                else
                {
                    EtiqErrorLlaves.Text = "El requerimiento no pudo ser eliminado, ocurrió un error. "+idViejoREQ;
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                    EtiqErrorLlaves.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                    idViejoREQ = "";
                    controladoraRequerimiento.UpdateUsoREQ(idViejoREQ, 0);
                }
                llenarGrid();
                llenarGrid();
            }
            else
            {
                EtiqErrorLlaves.Text = "Otro usuario esta modificando este requerimiento por lo que pude ser eliminado. Por favor intente mas tarde. ";
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                EtiqErrorLlaves.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
            }
        }

        /*
         * Requiere: Que el usuario de clic en el cancelar cuando le aparece el modal.
         * Modifica: Vuelve al estado en el que estaba antes de que el usuario haya intentado eliminar, modificar o insertar.
         * Retorna: N/A.      
        */
        protected void cancelarModal_Click(object sender, EventArgs e)
        {
            desmarcarBotones();
            deshabilitarCampos();
            if(modoREQ==Convert.ToString(3)){
                modoREQ=Convert.ToString(0);
                controladoraRequerimiento.UpdateUsoREQ(idViejoREQ,0);
                llenarDatosRequerimiento(idViejoREQ);
            }
            else if (modoREQ == Convert.ToString(2))
            {
                controladoraRequerimiento.UpdateUsoREQ(idViejoREQ, 0);    //ya no está en uso
                volverAlOriginal();
                BotonREQEliminar.Enabled = true;
                BotonREQModificar.Enabled = true;
                llenarDatosRequerimiento(idViejoREQ);
            }
            else if (modoREQ == 1.ToString())
            {
                volverAlOriginal();
            }
            modoREQ = 0.ToString();
            llenarGrid();
            desmarcarBoton(ref BotonREQEliminar);
            desmarcarBoton(ref BotonREQInsertar);
            desmarcarBoton(ref BotonREQModificar);
        }
        /*
         * Requiere: N/A. 
         * Modifica: N/A. 
         * Retorna: N/A.      
        */
        protected void siModalCancelar_Click(object sender, EventArgs e)
        {
            
        }

    }
}
