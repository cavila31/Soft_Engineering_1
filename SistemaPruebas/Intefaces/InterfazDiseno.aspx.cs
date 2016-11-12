using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SistemaPruebas.Intefaces
{
    public partial class InterfazDiseno : System.Web.UI.Page
    {
        static DataTable dtNoAsociados;
        static DataTable dtSiasociados;
        Controladoras.ControladoraDisenno controlDiseno = new Controladoras.ControladoraDisenno();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (controlDiseno.loggeadoEsAdmin())
                {
                    llenarComboboxProyectoAdmin();
                    deshabilitarCampos();
                    deshabilitarGridReq(1);
                    deshabilitarGridReq(2);
                }
                else
                {
                    llenarComboboxProyectoMiembro();
                    llenarGridDisenos();
                    llenarGridsReq(1);
                    llenarGridsReq(2);
                    cargarResponsablesMiembro();
                    deshabilitarCampos();
                }


                restriccionesCampos();
                txt_date.Attributes.Add("readonly", "readonly");
                llenarPrincipio();

            }

            
            if (controlDiseno.loggeadoEsAdmin())
            {
                Modificar.Enabled = false;
                Insertar.Enabled = false;
                Eliminar.Enabled = false;
            }
            else
            {
                Modificar.Enabled = false;
                Insertar.Enabled = true;
                Eliminar.Enabled = false;
            }
            if (camposLlenos == "true")
            {
                Modificar.Enabled = true;
                Insertar.Enabled = true;
                Eliminar.Enabled = true;
            }
            
        }


        //Definiciones de variables de sesion

        public static List<string> infDisenno
        {
            get
            {
                object value = HttpContext.Current.Session["infDisenno"];
                List<string> ids = HttpContext.Current.Session["infDisenno"] != null ? (List<string>)HttpContext.Current.Session["infDisenno"] : null;
                return ids;
            }
            set
            {
                HttpContext.Current.Session["infDisenno"] = value;
            }
        }

        public static List<string> infDisennoRegresar
        {
            get
            {
                object value = HttpContext.Current.Session["infDisennoRegresar"];
                List<string> ids = HttpContext.Current.Session["infDisennoRegresar"] != null ? (List<string>)HttpContext.Current.Session["infDisennoRegresar"] : null;
                return ids;
            }
            set
            {
                HttpContext.Current.Session["infDisennoRegresar"] = value;
            }
        }

        public static string el_proyecto
        {
            get
            {
                object value = HttpContext.Current.Session["el_proyecto"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["el_proyecto"] = value;
            }
        }

        public static string camposLlenos
        {
            get
            {
                object value = HttpContext.Current.Session["camposLlenos"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["camposLlenos"] = value;
            }
        }

        public static string id_req_asoc
        {
            get
            {
                object value = HttpContext.Current.Session["id_req_asoc"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["id_req_asoc"] = value;
            }
        }

        public static string id_req_noAsoc
        {
            get
            {
                object value = HttpContext.Current.Session["id_req_noAsoc"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["id_req_noAsoc"] = value;
            }
        }

        public static string nombre_req_asoc
        {
            get
            {
                object value = HttpContext.Current.Session["nombre_req_asoc"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["nombre_req_asoc"] = value;
            }
        }

        public static string nombre_req_NoAsoc
        {
            get
            {
                object value = HttpContext.Current.Session["nombre_req_asoc"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["nombre_req_asoc"] = value;
            }
        }

        public static string id_diseno_cargado
        {
            get
            {
                object value = HttpContext.Current.Session["id_diseno_cargado"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["id_diseno_cargado"] = value;
            }
        }


        public static string llenarProyecto
        {
            get
            {
                object value = HttpContext.Current.Session["llenarProyecto"];
                return value == null ? "True" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["llenarProyecto"] = value;
            }
        }

        public static string buttonDisenno
        {
            get
            {
                object value = HttpContext.Current.Session["buttonDisenno"];
                return value == null ? "0" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["buttonDisenno"] = value;
            }
        }

        protected void irAReq(object sender, EventArgs e)
        {
            Response.Redirect("InterfazRequerimiento.aspx");
        }

        //Definiciones de los metodos

        /*
        Requiere:  El usuario haya presionado el botón de insertar
        Modifica: Prepara el proceso para insertar un diseno, habilita y limpia los campos, habilita el botón aceptar y cancelar y deshabilita el botón de modificar, eliminar, y el grid de consultas.
        Retorna: N/A
        */
        protected void insertarClick(object sender, EventArgs e)
        {
            buttonDisenno = "1";
            limpiarCampos();
            Modificar.Enabled = false;
            Eliminar.Enabled = false;
            Insertar.Enabled = false;
            habilitarCampos();           
            deshabilitarGridDiseno();
            gridDisenos.Enabled = false;
            marcarBoton(ref Insertar);
            cancelar.Enabled = true;
            aceptar.Enabled = true;
            botonCP.Enabled = false;

            if (this.proyectoAsociado.SelectedItem.Text == "Seleccionar")
            {
                labelSeleccioneProyecto.Visible = true;

            }
            else
            {
                labelSeleccioneProyecto.Visible = false;
            }
            
        }

        /*
        Requiere: El usuario haya presionado el botón de modificar
        Modifica: Prepara el proceso para modificar un diseno, habilita y limpia los campos, habilita el botón aceptar y cancelar y deshabilita el botón de insertar, eliminar, y el grid de consultas.
        Retorna: N/A
        */
        protected void modificarClick(object sender, EventArgs e)
        {
            if (proyectoAsociado.SelectedItem.Text != "Seleccionar")
            {
                marcarBoton(ref Modificar);
                buttonDisenno = "2";
                habilitarCampos();
                Modificar.Enabled = false;
                Eliminar.Enabled = false;
                Insertar.Enabled = false;
                deshabilitarGridDiseno();
                aceptar.Enabled = true;
                cancelar.Enabled = true;
                if (this.proyectoAsociado.SelectedIndex == 0)
                {
                    labelSeleccioneProyecto.Visible = true;

                }
                else
                {
                    labelSeleccioneProyecto.Visible = false;
                }

                botonCP.Enabled = true;
                el_proyecto = proyectoAsociado.SelectedItem.Text;
            }
            if (proyectoAsociado.Items.Count == 1)
            {
                labelSeleccioneProyecto.Visible = false;
            }
        }

        /*
        Requiere: El usuario haya presionado el botón de eliminar  
        Modifica: Confirma con el usuario si desea eliminar un diseno
        Retorna: N/A
        */
        protected void eliminarClick(object sender, EventArgs e)
        {
            marcarBoton(ref Eliminar);
            Modificar.Enabled = false;
            Insertar.Enabled = false;
            deshabilitarGridDiseno();
            labelSeleccioneProyecto.Visible = true;
            if (this.proyectoAsociado.SelectedIndex == 0)
            {
                labelSeleccioneProyecto.Visible = true;
            }
            else
            {
                labelSeleccioneProyecto.Visible = false;
            }
            botonCP.Enabled = false;
            llenarGridDisenos();
        }

        /*
        Requiere:  N/A
        Modifica: delimita algunas características de los componentes a mostrarse en la interfaz
        Retorna: N/A
        */
        protected void restriccionesCampos()
        {
            propositoTxtbox.MaxLength = 80;
            ambienteTxtbox.MaxLength = 150;
            procedimientoTxtbox.MaxLength = 150;
            criteriosTxtbox.MaxLength = 150;

        }

        protected void aceptarModal_ClickEliminar(object sender, EventArgs e)
        {
           int a= controlDiseno.eliminarDisenno(Int32.Parse(id_diseno_cargado));
           if (a == 1)
           {
               EtiqErrorGen.Text = "El diseño ha sido eliminado con éxito";
               EtiqErrorGen.ForeColor = System.Drawing.Color.DarkSeaGreen;
               ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
               llenarGridDisenos();
               habilitarGridDiseno();
               deshabilitarCampos();
               desmarcarBoton(ref Eliminar);
               Modificar.Enabled = true;
               Eliminar.Enabled = true;
               Insertar.Enabled = true;
           }

        }

        protected void cancelarModal_ClickEliminar(object sender, EventArgs e)
        {
            desmarcarBoton(ref Eliminar);
            Modificar.Enabled = true;
            Eliminar.Enabled = true;
            Insertar.Enabled = true;
        }

        /*
        Requiere:  El usuario ha presionado el botón de Insertar dentro de la pestaña de Diseno
        Modifica: Habilita  y despliega los campos
        Retorna: N/A
        */
        protected void habilitarCampos()
        {
            //nombreReqTxtbox.Enabled = true;
            // precondicionReqTxtbox.Enabled = true;
            //reqEspecialesReqTxtbox.Enabled = true;
            iraRequerimientoBtn.Enabled = true;
            propositoTxtbox.Enabled = true;
            ambienteTxtbox.Enabled = true;
            procedimientoTxtbox.Enabled = true;
            criteriosTxtbox.Enabled = true;
            txt_date.Enabled = true;
            //proyectoAsociado.Enabled = true;
            Nivel.Enabled = true;
            Tecnica.Enabled = true;
          //  Tipo.Enabled = true;
            responsable.Enabled = true;
            aceptar.Enabled = true;
            cancelar.Enabled = true;
            habilitarGridReq(1);
            habilitarGridReq(2);
        }

        /*
        Requiere: El usuario haya presionado el botón aceptar o cancelar, dentro de una inserción, modificación, consulta o eliminación
        Modifica: Deshabilitan los campos de: 
        Retorna: N/A
        */

        protected void deshabilitarCampos()
        {
            iraRequerimientoBtn.Enabled = false;
             propositoTxtbox.Enabled = false;
            ambienteTxtbox.Enabled = false;
            procedimientoTxtbox.Enabled = false;
            criteriosTxtbox.Enabled = false;
            Nivel.Enabled = false;
            Tecnica.Enabled = false;
            txt_date.Enabled = false;
            responsable.Enabled = false;
            aceptar.Enabled = false;
            cancelar.Enabled = false;
            deshabilitarGridReq(1);
            deshabilitarGridReq(2);
        }

        /*
        Requiere: El usuario haya presionado el botón aceptar o cancelar, dentro de una inserción, modificación, consulta o eliminación 
        Modifica: Limpia los campos
        Retorna: N/A
        */
        protected void limpiarCampos()
        {
            propositoTxtbox.Text = "";
            ambienteTxtbox.Text = "";
            procedimientoTxtbox.Text = "";
            criteriosTxtbox.Text = "";
            txt_date.Text = "";
            Nivel.ClearSelection();
            ListItem selectedListItem1 = Nivel.Items.FindByValue("1");
            Tecnica.ClearSelection();
            ListItem selectedListItem2 = Tecnica.Items.FindByValue("1");
            responsable.ClearSelection();
            ListItem selectedListItem4 = responsable.Items.FindByValue("1");
            cancelar.Enabled = false;
            Modificar.Enabled = false;
            deshabilitarGridReq(1);
            deshabilitarGridReq(2);
            camposLlenos = "false";
        }

        /*
        Requiere: El usuario haya presionado el botón de aceptar dentro de Diseno
        Modifica: Al aceptar un insertar, se verifica que los campos se hayan llenado correctamente, e invoca a la controladora Diseno para que realice la inserción de la nueva información dentro de la base de datos. Al modificar, similar al proceso de insertar se envían los datos a la controladora Diseno, pero para que actualice la información del Diseno. Al aceptar una eliminación, elimina la tupla del sistema
        Retorna: N/A
        */
        protected void aceptarClick(object sender, EventArgs e)
        {

            deshabilitarCampos();
            switch (Int32.Parse(buttonDisenno))
            {
                case 1://Insertar
                    {
                       string fecha = txt_date.Text;                       
                        int cedula = 0;
                        int proyecto = controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text);
                        el_proyecto = proyecto.ToString();                        
                        object[] datos;

                        if (responsable.SelectedValue == "Seleccionar" || responsable.SelectedValue == "No Disponible")
                        {
                            datos = new object[9] { propositoTxtbox.Text, Nivel.SelectedValue, Tecnica.SelectedValue, ambienteTxtbox.Text, procedimientoTxtbox.Text, fecha, criteriosTxtbox.Text, -1, proyecto };
                        
                        }
                        else
                        {
                            cedula = controlDiseno.solicitarResponsableCedula(responsable.SelectedValue);
                            datos = new object[9] { propositoTxtbox.Text, Nivel.SelectedValue, Tecnica.SelectedValue, ambienteTxtbox.Text, procedimientoTxtbox.Text, fecha, criteriosTxtbox.Text, cedula, proyecto };
                        
                        }
                        int a = controlDiseno.ingresaDiseno(datos);
                        if (a == 1)
                        {
                            id_diseno_cargado = controlDiseno.consultarId_Disenno(propositoTxtbox.Text).ToString();
                            insertarGridReq();
                            EtiqErrorGen.Text = "El diseño ha sido insertado con éxito";
                            EtiqErrorGen.ForeColor = System.Drawing.Color.DarkSeaGreen;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            llenarGridDisenos();
                           // llenarGridsReq(1);
                           // llenarGridsReq(2);
                            habilitarGridDiseno();
                            deshabilitarCampos();
                            desmarcarBoton(ref Insertar);
                            Modificar.Enabled = false;
                            Eliminar.Enabled = false;
                            Insertar.Enabled = true;
                        }
                        else
                        {
                            EtiqErrorGen.Text = "Se produjo un error al momento de insertar el diseño, por favor intente luego";
                            EtiqErrorGen.ForeColor = System.Drawing.Color.Salmon;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        }
                    }
                    break;
                case 2://Modificar
                    {

                        string fecha = txt_date.Text;
                        int cedula = 0;
                        if (responsable.SelectedValue != "Seleccionar")
                        {
                            cedula = controlDiseno.solicitarResponsableCedula(responsable.SelectedValue); 
                        }
                                              
                        int proyecto = controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text);

                        object[] datos = new object[9] { propositoTxtbox.Text, Nivel.SelectedValue, Tecnica.SelectedValue, ambienteTxtbox.Text, procedimientoTxtbox.Text, fecha, criteriosTxtbox.Text, -1, proyecto };
                        int a = controlDiseno.modificarDiseno(Int32.Parse(id_diseno_cargado), datos);
                        if (a == 1)
                        {
                            ModificarGridReq();
                            EtiqErrorGen.Text = "El diseño ha sido modificado con éxito";
                            EtiqErrorGen.ForeColor = System.Drawing.Color.DarkSeaGreen;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            llenarGridDisenos();
                            llenarGridsReq(1);
                            llenarGridsReq(2);
                            habilitarGridDiseno();
                            deshabilitarCampos();
                            desmarcarBoton(ref Modificar);
                            Modificar.Enabled = true;
                            Eliminar.Enabled = true;
                            Insertar.Enabled = true;
                        }
                        else
                        {

                           EtiqErrorGen.Text = "Se produjo un error al momento de modificar el diseño, por favor intente luego";
                            EtiqErrorGen.ForeColor = System.Drawing.Color.Salmon;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        }
                    }
                    break;
            };
        }
        /*
        Requiere:  El usuario haya presionado el botón de inserción, modificación, consulta o eliminación
        Modifica: Confirma si el usurario desea cancelar e interrumpe la acción de inserción, modificación, consulta o eliminación
        Retorna: N/A
        */

        protected void cancelarClick(object sender, EventArgs e)
        {
            deshabilitarCampos();
            limpiarCampos();
            habilitarGridDiseno();
            desmarcarBoton(ref Insertar);
            desmarcarBoton(ref Modificar);
            desmarcarBoton(ref Eliminar);
            labelSeleccioneProyecto.Visible = false;
            Insertar.Enabled = true;

        }

        protected void llenarGridsReq(int tipo)
        {
            DataTable req = solicitarReqs(tipo);
            if (tipo == 1)
            {
                gridNoAsociados.DataSource = req;
                gridNoAsociados.DataBind();
            }
            else
            {
                gridAsociados.DataSource = req;
                gridAsociados.DataBind();
            }
        }
        

        protected DataTable solicitarReqs(int tipo)
        {
            DataTable req = new DataTable();
            DataTable dt = new DataTable();

            req.Columns.Add("Id");
            req.Columns.Add("Nombre");

            int proyecto = controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text);
            int diseno = -1;

            if (Int32.Parse(buttonDisenno) == 2)
            {
                diseno = Int32.Parse(id_diseno_cargado);
            }

            if (tipo == 1)
            {
                
                dt = controlDiseno.consultarReqNoenDiseno(proyecto, diseno);
                dtNoAsociados = dt;
            }
            else
            {                
                dt = controlDiseno.consultarDisennoReq(diseno);
                dtSiasociados = dt;

            }
            Object[] datos = new Object[2];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[0].ToString();
                    datos[1] = dr[1];
                    req.Rows.Add(datos);

                }
            }

            else
            {

                Object[] datos1 = new Object[2];
                datos1[0] = "-";
                datos1[1] = "-";
                req.Rows.Add(datos);
            }
            return req;       
        }
        protected void llenarGridsReqModificar(int tipo, DataTable req)
        {
            if (tipo == 1)
            {
                dtNoAsociados = req;
                if (req.Rows.Count > 0)
                {

                }
                else
                {
                    Object[] datos = new Object[1];
                    datos[0] = "-";
                    datos[1] = "-";
                    req.Rows.Add(datos);
                }
                gridNoAsociados.DataSource = req;
                gridNoAsociados.DataBind();
            }
            else
            {
                dtSiasociados = req;
                if (req.Rows.Count > 0)
                {

                }
                else
                {
                    Object[] datos = new Object[2];
                    datos[0] = "-";
                    datos[1] = "-";
                    req.Rows.Add(datos);
                }
                gridAsociados.DataSource = req;
                gridAsociados.DataBind();
            }
        }

        protected DataTable crearTablaREQ(/*int tipo*/)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(String));
            dt.Columns.Add("Nombre", typeof(String));
            return dt;
        }


        protected void OnSelectedIndexChangedNoAsoc(object sender, EventArgs e)
        {
            try
            {

                id_req_noAsoc = gridNoAsociados.SelectedRow.Cells[0].Text;
                nombre_req_NoAsoc = gridNoAsociados.SelectedRow.Cells[1].Text;
                dtNoAsociados = quitarElemento(dtNoAsociados, id_req_noAsoc);
                dtSiasociados = ponerElemento(dtSiasociados, id_req_noAsoc, nombre_req_NoAsoc);
                llenarGridsReqModificar(1, dtNoAsociados);
                llenarGridsReqModificar(2, dtSiasociados);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected void OnSelectedIndexChangedAsoc(object sender, EventArgs e)
        {
            try
            {
                id_req_asoc = gridAsociados.SelectedRow.Cells[0].Text;
                nombre_req_asoc = gridAsociados.SelectedRow.Cells[1].Text;
                dtSiasociados = quitarElemento(dtSiasociados, id_req_asoc);
                dtNoAsociados = ponerElemento(dtNoAsociados, id_req_asoc, nombre_req_asoc);
                llenarGridsReqModificar(1, dtNoAsociados);
                llenarGridsReqModificar(2, dtSiasociados);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected DataTable quitarElemento(DataTable dtVieja, String id)
        {

            DataTable dtNueva = crearTablaREQ();
            Object[] datos = new Object[2];

                if (dtVieja.Rows.Count > (0-1))
                {
                    foreach (DataRow dr in dtVieja.Rows)
                    {                        
                        datos[0] = dr[0];
                        datos[1] = dr[1];
                    if (Convert.ToString(datos[0]) != Convert.ToString(id))
                        {
                            dtNueva.Rows.Add(datos);
                        }
                    }                
            }
                else
                {
                    datos[0] = "-";
                    datos[1] = "-";
                    dtNueva.Rows.Add(datos);
                }
                
            return dtNueva;
        }
        protected DataTable ponerElemento(DataTable dtVieja, String id, string nombre)
        {

            DataTable dtNueva = crearTablaREQ();
            Object[] datos = new Object[2];

            datos[0] = id;
            datos[1] = nombre;
            dtNueva.Rows.Add(datos);
            foreach (DataRow dr in dtVieja.Rows)
                {

                    datos[0] = dr[0];
                    datos[1] = dr[1];
                    dtNueva.Rows.Add(datos);
                    
                }
            
            return dtNueva;
        }

        protected void OnPageIndexChangingNoAsoc(object sender, GridViewPageEventArgs e)
        {
            gridNoAsociados.PageIndex = e.NewPageIndex;
            this.llenarGridsReq(1);
        }

        protected void OnPageIndexChangingAsoc(object sender, GridViewPageEventArgs e)
        {
            gridAsociados.PageIndex = e.NewPageIndex;
            this.llenarGridsReq(2);
        }

        protected void OnRowDataBoundNoAsoc(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridNoAsociados, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

            }
        }

        protected void OnRowDataBoundAsoc(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridAsociados, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

            }
        }

        /*
        Requiere: N/A
        Modifica: Llena la tabla o grid de consultas con todos los disenos
        Retorna: N/A
        */
        protected void llenarGridDisenos()
        {

            DataTable disennos = new DataTable();

            disennos.Columns.Add("Propósito");
            disennos.Columns.Add("Nivel");
            disennos.Columns.Add("Técnica");
            disennos.Columns.Add("Responsable");

            int proyecto = controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text);
            DataTable dt = controlDiseno.consultarDisenoGrid(proyecto);
            Object[] datos = new Object[4];


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[0];
                    datos[1] = Nivel.Items.FindByValue(dr[1].ToString());
                    datos[2] = Tecnica.Items.FindByValue(dr[2].ToString());
                    datos[3] = dr[3];
                    disennos.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                datos[2] = "-";
                datos[3] = "-";
                disennos.Rows.Add(datos);
            }
            gridDisenos.DataSource = disennos;
            gridDisenos.DataBind();

        }

        /*
        Requiere: El usuario ha seleccionado el proyecto: habilita grid Diseños, ha seleccionado insertar o modificar: hablilita los grids de Requerimientos y Criterios
        Modifica: 
        Retorna: N/A
        */
        protected void habilitarGridDiseno()
        {
            gridDisenos.Enabled = true;
            foreach (GridViewRow row in gridDisenos.Rows)
            {
                row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridDisenos, "Select$" + row.RowIndex);
                row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                row.Attributes["style"] = "cursor:pointer";

            }
        }

        /*
        Requiere: El usuario ha seleccionado insertar o eliminar: deshabilita grid Diseños, el usuario esta consultado o eliminando: deshablilita los grids de Requerimientos y Criterios
        Modifica: Impide seleccionar tuplas de un grid al usuario
        Retorna: N/A
        */
        protected void habilitarGridReq(int tipo)
        {
            if (tipo == 1)
            {
                gridNoAsociados.Enabled = true;
                foreach (GridViewRow row in gridNoAsociados.Rows)
                {
                    row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridNoAsociados, "Select$" + row.RowIndex);
                    row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                    row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                    row.Attributes["style"] = "cursor:pointer";
                }
            }

            else
            {
                gridAsociados.Enabled = true;
                foreach (GridViewRow row in gridAsociados.Rows)
                {
                    row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridAsociados, "Select$" + row.RowIndex);
                    row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                    row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                    row.Attributes["style"] = "cursor:pointer";
                }
            }
        }

        //Requiere: N/A
        //Modifica: Se deshabilita el grid de consultas de diseño
        //retorna N/A
        protected void deshabilitarGridDiseno()
        {            
            foreach (GridViewRow row in gridDisenos.Rows)
            {
                row.Attributes.Remove("onclick");
                row.Attributes.Remove("onmouseover");
                row.Attributes.Remove("style");
                row.Attributes.Remove("onmouseout");
            }
            gridDisenos.Enabled = false;
        }

        //Requiere: entero con el tipo de grid
        //Modifica: Se deshabilita la edición de los grid de requerimiento.
        //Si ingresa 1, se deshabilita el grid de requerimientos disponibles
        //Si se ingresa un 2 se deshabilita la edición de grid de requerimientos en diseño
        //retorna N/A
        protected void deshabilitarGridReq(int tipo)
        {

            if (tipo == 1)
            {                
                foreach (GridViewRow row in gridNoAsociados.Rows)
            {
                row.Attributes.Remove("onclick");
                row.Attributes.Remove("onmouseover");
                row.Attributes.Remove("style");
                row.Attributes.Remove("onmouseout");
            }
                gridNoAsociados.Enabled = false;
            }

            else
            {
                gridAsociados.Enabled = false;
                foreach (GridViewRow row in gridAsociados.Rows)
                {
                    row.Attributes.Remove("onclick");
                    row.Attributes.Remove("onmouseover");
                    row.Attributes.Remove("style");
                    row.Attributes.Remove("onmouseout");
                }
            }
        }


        //Requiere: datos de textbox de interfaz
        //Modifica: Se almacenan los datos recién insertados, dentro de un objeto
        //retorna N/A
        protected void obtenerDatosInsertados()
        {

            string fecha = txt_date.Text;
            object[] datos = new object[6] { propositoTxtbox.Text, Nivel.SelectedValue, Tecnica.SelectedValue, ambienteTxtbox.Text, procedimientoTxtbox.Text, fecha };

        }


        //Requiere: N/A
        //Modifica: Coloca color al botón recién seleccionado
        //retorna N/A
        protected void marcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.ColorTranslator.FromHtml("#20bcae");
            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#20bcae");
            b.ForeColor = System.Drawing.Color.White;
        }


        //Requiere: N/A
        //Modifica: Quitar color al botón recién desmarcado
        //retorna N/A
        protected void desmarcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.Color.LightGray;
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;

        }

        //Requiere: N/A
        //Modifica: Carga el combobox de proyectos con todos los proyetos disponibles para un administrador
        //retorna N/A
        protected void llenarComboboxProyectoAdmin()
        {

            this.proyectoAsociado.Items.Clear();
            proyectoAsociado.Items.Add(new ListItem("Seleccionar"));
            String proyectos = controlDiseno.solicitarProyectos();
            String[] pr = proyectos.Split(';');

            foreach (String p1 in pr)
            {
                String[] p2 = p1.Split('_');
                try
                {
                    if (Convert.ToInt32(p2[1]) > -1)
                    {
                        this.proyectoAsociado.Items.Add(new ListItem(p2[0], p2[1]));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }

        //Requiere: N/A
        //Modifica: Çarga combo Box con todos los posibles proyectos en la BD, cuando es consultado por un miembro de equipo
        //retorna N/A
        protected void llenarComboboxProyectoMiembro()
        {
                      
            this.proyectoAsociado.Items.Clear();

                try
                {
                    this.proyectoAsociado.Items.Add(new ListItem(controlDiseno.solicitarNombreProyectoMiembro(controlDiseno.solicitarProyecto_IdMiembro())));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            
        }

        //Requiere: N/A
        //Modifica: Solicita al controlador todos los RH disponibles para ser responsables de un diseño
        //retorna N/A
        protected void cargarResponsablesMiembro()
        {
            int id_proyecto = controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text);
            this.responsable.Items.Clear();
            responsable.Items.Add(new ListItem("Seleccionar"));
            String responsables = controlDiseno.solicitarResponsanles(id_proyecto);

            if (responsables != null)
            {
                String[] pr = responsables.Split(';');

                foreach (String p1 in pr)
                {
                    try
                    {
                        if (p1 != pr[pr.Length - 1])
                            this.responsable.Items.Add(new ListItem(p1));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            else
            {
                this.responsable.Items.Clear();
                responsable.Items.Add(new ListItem("No Disponible"));
            }
        }

        //Requiere: N/A
        //Modifica: Modifica los comboBox y grid de consulta luego de seleccionar un proyecto
        //retorna N/A
        protected void proyectoAsociado_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpiarCampos();
            if (proyectoAsociado.SelectedItem.Text != "Seleccionar")
            {
                el_proyecto = proyectoAsociado.SelectedItem.Text;
                if (buttonDisenno == "1")
                {

                    habilitarCampos();
                    gridDisenos.Enabled = false;
                }
                else
                    llenarGridDisenos();
                llenarGridsReq(1);
                llenarGridsReq(2);
                int id_proyecto = controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text);
                this.responsable.Items.Clear();
                responsable.Items.Add(new ListItem("Seleccionar"));
                String responsables = controlDiseno.solicitarResponsanles(id_proyecto);

                if (responsables != null)
                {
                    String[] pr = responsables.Split(';');

                    foreach (String p1 in pr)
                    {
                        try
                        {
                            if (p1 != pr[pr.Length - 1])
                                this.responsable.Items.Add(new ListItem(p1));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
                else
                {
                    this.responsable.Items.Clear();
                    responsable.Items.Add(new ListItem("No Disponible"));
                }
                Modificar.Enabled = false;
                Insertar.Enabled = true;
                Eliminar.Enabled = false;
            }
            else
            {
                Modificar.Enabled = false;
                Insertar.Enabled = false;
                Eliminar.Enabled = false;
            }
        }

        protected int desasociarRequerimientoEnDiseno(int id_req, int id_diseno)
        {
            return 1;//resultado de la eliminacion 

        }

        protected int asociarRequerimientoEnDiseno(int id_req, int id_diseno)
        {
            return 1;//resultado de la insersion

        }


        //Requiere: N/A
        //Modifica: Carga en pantalla los datos de un deseño recién seleccionado en el grid de consulta.
        //retorna N/A
        protected void Llenar_Datos_Conultados(int id_diseno)
        {
            buttonDisenno = "2";
            Controladoras.EntidadDisenno entidad = controlDiseno.consultarDisenno(id_diseno);
            llenarGridsReq(1);
            llenarGridsReq(2);
            this.propositoTxtbox.Text = entidad.Proposito;
            

            Nivel.ClearSelection();
            ListItem selectedListItem = Nivel.Items.FindByValue(entidad.Nivel.ToString());
            if (selectedListItem != null)
            {
                selectedListItem.Selected = true;
            };

            Tecnica.ClearSelection();
            ListItem selectedListItem2 = Tecnica.Items.FindByValue(entidad.Tecnica.ToString());
            if (selectedListItem2 != null)
            {
                selectedListItem2.Selected = true;
            };
            this.ambienteTxtbox.Text = entidad.Ambiente;
            this.procedimientoTxtbox.Text = entidad.Procedimiento;
            this.txt_date.Text = entidad.FechaDeDisenno;
            this.criteriosTxtbox.Text = entidad.CriterioAceptacion;

            cargarResponsablesMiembro();
            responsable.ClearSelection();
            ListItem selectedListItem3 = responsable.Items.FindByText(controlDiseno.solicitarNombreResponsable(entidad.Responsable));
            if (selectedListItem3 != null)
            {
                selectedListItem3.Selected = true;
            }
            deshabilitarCampos();


            List<string> lista = new List<string>();
            lista.Add(entidad.Proposito);
            lista.Add(entidad.Nivel.ToString());
            lista.Add(entidad.Tecnica.ToString());
            lista.Add(entidad.Ambiente);
            lista.Add(entidad.Procedimiento);
            lista.Add(entidad.FechaDeDisenno);
            lista.Add(entidad.Responsable.ToString());

            infDisennoRegresar = lista;

        }

        //Requiere: N/A
        //Modifica: Despliega el proceso de coonsulta de un Diseño
        //retorna N/A
        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string proposito_Diseno = gridDisenos.SelectedRow.Cells[0].Text;
                int id_diseno = controlDiseno.consultarId_Disenno(proposito_Diseno);
                id_diseno_cargado = id_diseno.ToString();
                Insertar.Enabled = true;
                Modificar.Enabled = true;
                Eliminar.Enabled = true;
                aceptar.Enabled = true;                
                Llenar_Datos_Conultados(id_diseno);
                cancelar.Enabled = true;
                botonCP.Enabled = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        //Requiere: N/A
        //Modifica: Carga una nueva página en el grid de consulta
        //retorna N/A
        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridDisenos.PageIndex = e.NewPageIndex;
            this.llenarGridDisenos();
        }

        //Requiere: N/A
        //Modifica: Brinda formato a la fila seleccionada del grid.
        //retorna N/A
        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridDisenos, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

            }
        }

        //Requiere: N/A
        //Modifica: Al presionar el botón de cancelar, y aceptar la cancelación, este método heshabilita la edición de los campos de texto, limpia los textbox y desmarca los botones
        //retorna N/A
        protected void aceptarModal_ClickCancelar(object sender, EventArgs e)
        {
            deshabilitarCampos();
            limpiarCampos();
            habilitarGridDiseno();
            desmarcarBoton(ref Insertar);
            desmarcarBoton(ref Modificar);
            desmarcarBoton(ref Eliminar);
            Insertar.Enabled = true;
            Modificar.Enabled = false;
            Eliminar.Enabled = false;
            labelSeleccioneProyecto.Visible = false;
            botonCP.Enabled = false;

        }

        protected void cancelarModal_ClickCancelar(object sender, EventArgs e)
        {

        }

        //Requiere: N/A
        //Modifica: Se carga la interfaz de casos de prueba.
        //retorna N/A
        protected void irACasoPrueba(object sender, EventArgs e){
            List<string> lista=new List<string>();
            lista.Add(propositoTxtbox.Text.ToString());
            lista.Add(Nivel.SelectedValue.ToString());
            lista.Add(Tecnica.SelectedValue.ToString());
            lista.Add(ambienteTxtbox.Text.ToString());
            lista.Add(procedimientoTxtbox.Text.ToString());
            lista.Add(txt_date.Text.ToString());
            lista.Add(criteriosTxtbox.Text.ToString());
            lista.Add(controlDiseno.solicitarResponsableCedula(responsable.SelectedValue).ToString());
            lista.Add(controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text).ToString());
            infDisenno = lista;

            Response.Redirect("~/Intefaces/InterfazCasosDePrueba.aspx");
        }

        //Requiere: N/A
        //Modifica: Inserta, en la base de datos, una relación entre un requerimiento y el diseño
        //retorna N/A
        public void insertarGridReq()
        {
            for (int i = 0; i< gridAsociados.Rows.Count ; i++ )
            {
                string id_req = gridAsociados.Rows[i].Cells[0].Text;
                int proyecto = controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text);
                object[] datos = {proyecto, id_req, Int32.Parse(id_diseno_cargado)};
                controlDiseno.insertarDisennoReq(datos);
            }
        }
        
        
        //Requiere: N/A
        //Modifica: Elimina, en la base de datos, la relación que pueda existir entre los requerimientos que no están en el diseño y el diseño mismo, 
        //a la vez que crea una relación entre los que si están en el diseño y el diseño
        //retorna N/A

        public void ModificarGridReq()
        {     
            for (int i = 0; i < gridNoAsociados.Rows.Count; i++)
            {
                string value = gridNoAsociados.Rows[i].Cells[0].Text;                          
                controlDiseno.eliminarDisennoReq(Int32.Parse(id_diseno_cargado), value);
            }
            
            for (int i = 0; i < gridAsociados.Rows.Count; i++)
            {
                string value = gridAsociados.Rows[i].Cells[0].Text;
                int proyecto = controlDiseno.solicitarProyecto_Id(proyectoAsociado.SelectedItem.Text);
                controlDiseno.eliminarDisennoReq(Int32.Parse(id_diseno_cargado), value);
                object[] datos = {proyecto, value, Int32.Parse(id_diseno_cargado)};
                controlDiseno.insertarDisennoReq(datos);
            }
                            
        }

        public List<string> infoDisenno()
        {
            return infDisenno;
        }
        public void llenarPrincipio()
        {
            try {
                Modificar.Enabled = true;
                Eliminar.Enabled = true;
                this.propositoTxtbox.Text = infDisennoRegresar[0];


                Nivel.ClearSelection();
                ListItem selectedListItem = Nivel.Items.FindByValue(infDisennoRegresar[1]);
                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                };

                Tecnica.ClearSelection();
                ListItem selectedListItem2 = Tecnica.Items.FindByValue(infDisennoRegresar[2]);
                if (selectedListItem2 != null)
                {
                    selectedListItem2.Selected = true;
                };
                this.ambienteTxtbox.Text = infDisennoRegresar[3];
                this.procedimientoTxtbox.Text = infDisennoRegresar[4];
                this.txt_date.Text = infDisennoRegresar[5];
                this.criteriosTxtbox.Text = infDisennoRegresar[6];

                cargarResponsablesMiembro();
                responsable.ClearSelection();
                ListItem selectedListItem3 = responsable.Items.FindByText(controlDiseno.solicitarNombreResponsable(Convert.ToInt32(infDisennoRegresar[6])));
                if (selectedListItem3 != null)
                {
                    selectedListItem3.Selected = true;
                }
                camposLlenos = "true";         
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

    }

}

