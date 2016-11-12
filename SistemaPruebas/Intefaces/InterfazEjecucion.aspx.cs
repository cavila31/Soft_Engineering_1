using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using SistemaPruebas.Controladoras;
using System.Data;
using System.Net;

namespace SistemaPruebas.Intefaces
{
    public partial class InterfazEjecucion : System.Web.UI.Page
    {


        ControladoraEjecucionPrueba controladoraEjecucionPrueba = new ControladoraEjecucionPrueba();
        /*
            Variable de sesion
        */

        /*
         * Requiere: N/A
         * Modifica: Declara variable global de modoEP (tipo de operación en ejecución)
         * Retorna: entero.
         */
        public static int modoEP
        {
            get
            {
                object value = HttpContext.Current.Session["modoEP"];
                return value == null ? 0 : (int)value;
            }
            set
            {
                HttpContext.Current.Session["modoEP"] = value;
            }
        }

        /*
         * Requiere: N/A
         * Modifica: Declara variable global que guarda el id del caso de prueba siendo modificado
         * Retorna: hilera.
         */
        public static String idMod
        {
            get
            {
                object value = HttpContext.Current.Session["idmod"];
                return value == null ? "" : (String)value;
            }
            set
            {
                HttpContext.Current.Session["idmod"] = value;
            }
        }

        /*
         * Requiere: N/A
         * Modifica: Variable global que guarda las no conformidades
         * Retorna: Lista de objetos.
         */
        public static List<Object[]> listaNC
        {
            get
            {
                object value = HttpContext.Current.Session["listaNC"];
                return value == null ? null : (List<Object[]>)value;
            }
            set
            {
                HttpContext.Current.Session["listaNC"] = value;
            }
        }

        /*
        * Requiere: N/A
        * Modifica: DataTable global usado para llenar el grid de No Conformidades al cargarse la página
        * Retorna: DataTable.
        */
        public static DataTable dtNoConformidades
        {
            get
            {
                object value = HttpContext.Current.Session["noConformidades"];
                return value == null ? null : (DataTable)value;
            }
            set
            {
                HttpContext.Current.Session["noConformidades"] = value;
            }
        }

        /*
         * Requiere: N/A
         * Modifica: Variable global indica en cual diseño se estan haciendo operaciones sobre sus ejecuciones de prueba 
         * Retorna: Hilera.
         */
        public static String disennoSeleccionado
        {
            get
            {
                object value = HttpContext.Current.Session["disennoSeleccionado"];
                return value == null ? "" : (String)value;
            }
            set
            {
                HttpContext.Current.Session["disennoSeleccionado"] = value;
            }
        }


       /*
        * Requiere: N/A
        * Modifica: Variable global indica la cantidad de no conformidades actualmente en el grid de No Conformidades 
        * Retorna: entero.
        */
        public static int cantidadConformidades
        {
            get
            {
                object value = HttpContext.Current.Session["cantidadConformidades"];
                return value == null ? 0 : (int)value;
            }
            set
            {
                HttpContext.Current.Session["cantidadConformidades"] = value;
            }
        }


        /* 
            Page load
        */

        /*
        * Requiere: N/A
        * Modifica: Maneja los eventos a ocurrir cada vez que se carga la página
        * Retorna: N/A.
        */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                estadoInicial();
                inicializarListaNC();
                inicializarDTnoConformidades();
                gridEjecucion.DataBind();
            }
            EtiqMensajeOperacion.Visible = false;
            gridEjecucion.Enabled = true;
            gridEjecucion.DataBind();
        }


        /* 
            Inicializaciones y estados       
        */


        /*
        * Requiere: N/A
        * Modifica: Inicializa la lista de no conformidades.
        * Retorna: N/A
        */
        protected void inicializarListaNC()
        {
            listaNC = new List<Object[]>();
        }
       /*
        * Requiere: N/A
        * Modifica: Establece el valor por defecto de la variable "modo" a 0.
        * Retorna: N/A
        */
        protected void inicializarModo()
        {
            modoEP = 0;
        }

        /*
        * Requiere: N/A
        * Modifica: Establece estado inicial de la página, indicando cuales controles van a estar habilitados la primera vez que se carga la página
        * Retorna: N/A
        */
        protected void estadoInicial()
        {
            //DatosEjecucion.Enabled = false;
            ResponsableEP.Enabled =false;
            DropDownResponsable.Enabled=false;
            FechaEP.Enabled=false;
            Incidencias.Enabled = false;
            llenarDDProyectoAdmin();
            inicializarModo();
            BotonEPInsertar.Enabled = false;
            BotonEPModificar.Enabled = false;
            BotonEPEliminar.Enabled = false;
            gridEjecucion.Enabled = true;

            EtiqMensajeOperacion.Visible = false;
            cantidadConformidades = 0;
        }

       /*
        * Requiere: N/A
        * Modifica: Establece el estado de la pantalla inmediatamente después de presionar el botón "Insertar".
        * Retorna: N/A
        */
        protected void estadoInsertar()
        {
            marcarBoton(ref BotonEPInsertar);
            limpiarCampos();
            habilitarCampos();
            BotonEPAceptar.Enabled = true;
            BotonEPCancelar.Enabled = true;
            BotonEPModificar.Enabled = false;
            BotonEPEliminar.Enabled = false;
            //deshabilitarGrid(ref gridEjecucion);
        }


       /*
        * Requiere: N/A
        * Modifica: Establece el estado de la pantalla inmediatamente después de presionar el botón "Modificar".
        * Retorna: N/A
        */
        protected void estadoModificar()
        {
            marcarBoton(ref BotonEPModificar);
            limpiarCampos();
            habilitarCampos();
            BotonEPAceptar.Enabled = true;
            BotonEPCancelar.Enabled = true;
            BotonEPInsertar.Enabled = false;
            BotonEPModificar.Enabled = true;
            BotonEPEliminar.Enabled = false;
            //deshabilitarGrid(ref gridEjecucion);
            llenarDatosEjecucionPrueba(gridEjecucion.SelectedRow.Cells[0].Text);

        }

        /*
         * Requiere: N/A
         * Modifica: Establece el estado de la pantalla inmediatamente después de efectuar una operación de inserción, modificación o eliminación.
         * Retorna: N/A
         */
        protected void estadoPostOperacion()
        {
            modoEP = 0;
            habilitarCampos();
            BotonEPInsertar.Enabled = true;
            BotonEPModificar.Enabled = false;
            BotonEPEliminar.Enabled = false;
            BotonEPCancelar.Enabled = false;
            BotonEPAceptar.Enabled = false;
            llenarGridEjecucion();
        }


        /*
            Llenado de DrodDownLists
        */

        /*
        * Requiere: N/A
        * Modifica: Se encarga de llenar el dropdownlist de proyectos solicitando la información a la controladora de Ejecución para que esta se la solicite a la de Proyectos
        * Retorna: N/A
        */
        protected void llenarDDProyectoAdmin()
        {

            this.DropDownProyecto.Items.Clear();
            DropDownProyecto.Items.Add(new ListItem("Seleccionar"));
            String proyectos = controladoraEjecucionPrueba.solicitarProyectos();
            String[] pr = proyectos.Split(';');

            foreach (String p1 in pr)
            {
                String[] p2 = p1.Split('_');
                try
                {
                    if (Convert.ToInt32(p2[1]) > -1)
                    {
                        this.DropDownProyecto.Items.Add(new ListItem(p2[0], p2[1]));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }


        /*
        * Requiere: N/A
        * Modifica: Se encarga de llenar el dropdownlist de diseños asociados al proyecto seleccionado solicitando la información a la controladora de Ejecución para que esta se la solicite a la de Diseño.
        * Retorna: N/A
        */
        protected void llenarDDDisseno()
        {
            this.DropDownDiseno.Items.Clear();
            DropDownDiseno.Items.Add(new ListItem("Seleccionar"));
            int idProyecto = Convert.ToInt32(DropDownProyecto.SelectedItem.Value);
            String disenos = controladoraEjecucionPrueba.solicitarPropositoDiseno(idProyecto);
            //Response.Write(disenos);
            String[] pr = disenos.Split(';');

            foreach (String p1 in pr)
            {
                String[] p2 = p1.Split('_');
                try
                {
                    if (Convert.ToInt32(p2[1]) > -1)
                    {
                        this.DropDownDiseno.Items.Add(new ListItem(p2[0], p2[1]));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }

        /*
        * Requiere: N/A
        * Modifica: Se encarga de llenar el dropdownlist de casos de pruebas asociados al diseño seleccionado solicitando la información a la controladora de Ejecución para que esta se la solicite a la de Casos de Prueba.
        * Retorna: N/A
        */
        protected void llenarDDCasoPrueba(ref DropDownList dd)
        {
            dd.Items.Clear();
            dd.Items.Add(new ListItem("Seleccionar"));
            int idDiseno = Convert.ToInt32(DropDownDiseno.SelectedItem.Value);
            String casosPrueba = controladoraEjecucionPrueba.solicitarCasosdePrueba(idDiseno);
            String[] pr = casosPrueba.Split(';');
            foreach (String p1 in pr)
            {
                try
                {
                    dd.Items.Add(new ListItem(p1));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

        }

        /*
         * Requiere: N/A
         * Modifica: Se encarga de llenar el dropdownlist de miembros asociados al proyecto seleccionado solicitando la información a la controladora de Ejecución para que esta se la solicite a la de Proyectos.
         * Retorna: N/A
         */
        protected void llenarDDResponsables()
        {
            this.DropDownResponsable.Items.Clear();
            DropDownResponsable.Items.Add(new ListItem("Seleccionar", "1"));
            int idProyecto = Convert.ToInt32(DropDownProyecto.SelectedItem.Value);
            String responsables = controladoraEjecucionPrueba.solicitarResponsables(idProyecto);

            if (responsables != null)
            {
                String[] pr = responsables.Split(';');

                foreach (String p1 in pr)
                {
                    try
                    {
                        if (p1 != pr[pr.Length - 1])
                            this.DropDownResponsable.Items.Add(new ListItem(p1));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            else
            {
                this.DropDownResponsable.Items.Clear();
                DropDownResponsable.Items.Add(new ListItem("No Disponible"));
            }
        }


      /* 
         Eventos DropDownLists
      */

      /*
      * Requiere: Evento de cambiar de opción en el DropDown de Proyecto.
      * Modifica: Provoca el llenado del DropDown de Diseño.
      * Retorna: N/A. 
      */
        protected void DropDownProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownProyecto.SelectedItem.Text != "Seleccionar")
            {
                DropDownDiseno.Enabled = true;
                llenarDDDisseno();
                llenarDDResponsables();
            }
            else
            {
                DropDownDiseno.Text = "Seleccionar";
                DropDownDiseno.SelectedItem.Text = "Seleccionar";
                BotonesPrincipales.Enabled = false;
                //DatosEjecucion.Enabled = false;
                ResponsableEP.Enabled = false;
                DropDownResponsable.Enabled = false;
                FechaEP.Enabled = false;
                Incidencias.Enabled = false;
                BotonEPInsertar.Enabled = false;
                BotonEPModificar.Enabled = false;
                BotonEPEliminar.Enabled = false;
            }
        }

      /*
      * Requiere: Evento de cambiar de opción en el DropDown de Diseño.
      * Modifica: Provoca el llenado del grid de ejecuciones de prueba, así como habilitar la interfaz para la inserción de una nueva ejecución de pruebas, asociada al diseño seleeccionado.
      * Retorna: N/A. 
      */
        protected void DropDownDiseno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownDiseno.SelectedItem.Text != "Seleccionar")
            {
                BotonesPrincipales.Enabled = true;
                disennoSeleccionado = DropDownDiseno.SelectedItem.Text.ToString();
                gridEjecucion.Enabled = true;
                llenarGridEjecucion();
                gridEjecucion.Enabled = true;

                BotonEPInsertar.Enabled = true;
                BotonEPModificar.Enabled = false;
                BotonEPEliminar.Enabled = false;
            }
            else
            {
                BotonesPrincipales.Enabled = false;
                //DatosEjecucion.Enabled = false;
                ResponsableEP.Enabled = false;
                DropDownResponsable.Enabled = false;
                FechaEP.Enabled = false;
                Incidencias.Enabled = false;
                BotonEPInsertar.Enabled = false;
                BotonEPModificar.Enabled = false;
                BotonEPEliminar.Enabled = false;

            }

        }

        /*
         * Requiere: Evento de cambiar de opción en el DropDown de Casos de Prueba.
         * Modifica: N/A.
         * Retorna: N/A. 
         */
        protected void DropDownCasoDePrueba_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        /*
            Control de campos y botones
        */

        /*
        * Requiere: N/A
        * Modifica: Se encarga de borrar toda la información de los controles seleccionados
        * Retorna: N/A
        */
        protected void limpiarCampos()
        {
            DropDownResponsable.SelectedValue = "1";
            FechaEP.Text = "";
            TextBoxIncidencias.Text = "";
            inicializarDTnoConformidades();

        }

        /*
        * Requiere: N/A
        * Modifica: Habilita los controles de la pantalla: Textbox, DropDownList y Grid.
        * Retorna: N/A
        */
        protected void habilitarCampos()
        {
            DatosEjecucion.Enabled = true;
            ResponsableEP.Enabled = true;
            DropDownResponsable.Enabled = true;
            FechaEP.Enabled = true;
            Incidencias.Enabled = true;
            gridEjecucion.Enabled = true;
        }

        /*
         * Requiere: Botón.
         * Modifica: Se encarga de marcar el botón pasado por parámetro.
         * Retorna: N/A
         */
        protected void marcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.ColorTranslator.FromHtml("#18c0a8");
            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#18c0a8");
            b.ForeColor = System.Drawing.Color.White;
        }

        /*
         * Requiere: Botón.
         * Modifica: Se encarga de desmarcar el botón pasado por parámetro.
         * Retorna: N/A
         */
        protected void desmarcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.Color.LightGray;
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;
        }


        /*
            Eventos Grid no conformidades        
        */

       /*
       * Requiere: El evento de enlazar información de un datatable con el Grid.
       * Modifica: Establece el comportamiento del Grid ante los diferentes eventos, en este caso llama al método de llenado de los dropdowlists de casos de prueba
       * Retorna: N/A.
       */
        protected void gridNoConformidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DropDownList dropDownCasos = (e.Row.FindControl("ddlIdCaso") as DropDownList);
                llenarDDCasoPrueba(ref dropDownCasos);
            }
        }


        /*
        * Requiere: N/A
        * Modifica: Establece las columnas que poseerá el datatable de "No Conformidades".
        * Retorna: N/A
        */
        protected void inicializarDTnoConformidades()
        {
            try
            {
                dtNoConformidades = null;
                DataTable dt = new DataTable();

                dt.Columns.AddRange(

                   new DataColumn[]
                   {

                        new DataColumn("Id", typeof(int)),
                        new DataColumn("Tipo", typeof(String)),
                        new DataColumn("IdCaso",typeof(String)),
                        new DataColumn("Descripcion", typeof(String)),
                        new DataColumn("Justificacion", typeof(String)),
                        new DataColumn("Resultado", typeof(String)),
                        new DataColumn("Estado", typeof(String))
                   }
                 );

                DataRow drRow = dt.NewRow();

                drRow["Id"] = 1;
                drRow["Tipo"] = String.Empty;
                drRow["IdCaso"] = String.Empty;
                drRow["Descripcion"] = String.Empty;
                drRow["Justificacion"] = String.Empty;
                drRow["Resultado"] = String.Empty;
                drRow["Estado"] = string.Empty;

                dt.Rows.Add(drRow);
                dtNoConformidades = dt;
                gridNoConformidades.DataSource = dtNoConformidades;
                gridNoConformidades.DataBind();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*
        * Requiere: Evento de dar click al boton de agregar fila
        * Modifica: Se encarga de agregar una fila nueva con todos los controles repectivos al grid de no conformidades para que se pueda agregar una nueva no conformidad
        * Retorna: N/A
        */
        protected void AgregarFIla_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridNoConformidades.Rows)
            {
                if (row.RowIndex == gridNoConformidades.Rows.Count-1)
                {
                    Object[] noConformidad = new Object[8];

                    DropDownList ddl1 = row.FindControl("ddlTipo") as DropDownList;
                    DropDownList ddl2 = row.FindControl("ddlIdCaso") as DropDownList;
                    TextBox txt1 = row.FindControl("txtDescripcion") as TextBox;
                    DropDownList ddl3 = row.FindControl("ddlEstado") as DropDownList;

                    noConformidad[0] = ddl1.SelectedItem.Text;
                    noConformidad[1] = ddl2.SelectedItem.Text;
                    noConformidad[2] = txt1.Text;
                    noConformidad[5] = ddl3.SelectedItem.Text;


                    if (!(noConformidad[0].Equals("Seleccionar") || noConformidad[1].Equals("Seleccionar") ||
                        noConformidad[2].Equals("") || noConformidad[5].Equals("Seleccionar")))
                    {
                        try
                        {
                            agregarFilaGridNC();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            
        }

        /*
        * Requiere: N/A
        * Modifica: Se encarga de agregar una fila nueva con todos los controles repectivos al grid de no conformidades para que se pueda agregar una nueva no conformidad
        * Retorna: N/A
        */
        protected void agregarFilaGridNC()
        {
            try
            {
                int indiceFila = 0;
                if (dtNoConformidades != null)
                {
                    DataTable dtCurrentTable = dtNoConformidades;

                    DataRow drCurrentRow = null;

                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            DropDownList ddl1 = gridNoConformidades.Rows[indiceFila].FindControl("ddlTipo") as DropDownList;
                            DropDownList ddl2 = gridNoConformidades.Rows[indiceFila].FindControl("ddlIdCaso") as DropDownList;
                            TextBox txt1 = gridNoConformidades.Rows[indiceFila].FindControl("txtDescripcion") as TextBox;
                            TextBox txt2 = gridNoConformidades.Rows[indiceFila].FindControl("txtJustificacion") as TextBox;
                            System.Web.UI.WebControls.Image imagenRes = gridNoConformidades.Rows[indiceFila].FindControl("imagenSubida") as System.Web.UI.WebControls.Image;
                            DropDownList ddl3 = gridNoConformidades.Rows[indiceFila].FindControl("ddlEstado") as DropDownList;
                            drCurrentRow = dtCurrentTable.NewRow();
                            // drCurrentRow["RowNumber"] = i + 1;

                            dtCurrentTable.Rows[i - 1]["Tipo"] = ddl1.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["IdCaso"] = ddl2.SelectedValue;
                            dtCurrentTable.Rows[i - 1]["Descripcion"] = txt1.Text;
                            dtCurrentTable.Rows[i - 1]["Justificacion"] = txt2.Text;
                            dtCurrentTable.Rows[i - 1]["Resultado"] = imagenRes.ImageUrl.ToString();
                            dtCurrentTable.Rows[i - 1]["Estado"] = ddl3.SelectedValue;
                            indiceFila++;
                        }

                        dtCurrentTable.Rows.Add(drCurrentRow);
                        dtNoConformidades = dtCurrentTable;
                        gridNoConformidades.DataSource = dtCurrentTable;
                        gridNoConformidades.DataBind();
                    }
                }
                else
                {
                    Response.Write("ViewState is null");
                }

                conservarEstado();
                cantidadConformidades += 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /*
        * Requiere: N/A
        * Modifica: Se encarga de conservar los datos en los controles de cada fila del grid de no conformidades al darse una inserción o eliminación de alguna fila.
        * Retorna: N/A
        */
        protected void conservarEstado()
        {
            try
            {
                int indiceFila = 0;
                if (dtNoConformidades != null)
                {
                    DataTable dtCurrentTable = dtNoConformidades;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                        {
                            DropDownList ddl1 = gridNoConformidades.Rows[indiceFila].FindControl("ddlTipo") as DropDownList;
                            DropDownList ddl2 = gridNoConformidades.Rows[indiceFila].FindControl("ddlIdCaso") as DropDownList;
                            TextBox txt1 = gridNoConformidades.Rows[indiceFila].FindControl("txtDescripcion") as TextBox;
                            TextBox txt2 = gridNoConformidades.Rows[indiceFila].FindControl("txtJustificacion") as TextBox;
                            System.Web.UI.WebControls.Image imagenRes = gridNoConformidades.Rows[indiceFila].FindControl("imagenSubida") as System.Web.UI.WebControls.Image;
                            DropDownList ddl3 = gridNoConformidades.Rows[indiceFila].FindControl("ddlEstado") as DropDownList;

                            ddl1.SelectedValue = dtCurrentTable.Rows[i]["Tipo"].ToString();
                            ddl2.SelectedValue = dtCurrentTable.Rows[i]["IdCaso"].ToString();
                            txt1.Text = dtCurrentTable.Rows[i]["Descripcion"].ToString();
                            txt2.Text = dtCurrentTable.Rows[i]["Justificacion"].ToString();
                            imagenRes.ImageUrl = dtCurrentTable.Rows[i]["Resultado"].ToString();
                            ddl3.SelectedValue = dtCurrentTable.Rows[i]["Estado"].ToString();
                            indiceFila++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        * Requiere: Evento de dar click al boton de subir imagen
        * Modifica: Se encarga de subir una imagen seleccionada por el usuario y colocarla en el control de imagen de asp.net ubicado en el panel de cada fila del grid de no conformidades.
        * Retorna: N/A
        */
        protected void subirImagen_Click(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent.Parent;
            int index = gvRow.RowIndex;

            FileUpload fu = gridNoConformidades.Rows[index].FindControl("Uploader") as FileUpload;
            System.Web.UI.WebControls.Image imagenRes = gridNoConformidades.Rows[index].FindControl("imagenSubida") as System.Web.UI.WebControls.Image;

            if (fu.HasFile)
            {

                try
                {
                    int length = fu.PostedFile.ContentLength;
                    byte[] imgbyte = new byte[length];
                    HttpPostedFile img = fu.PostedFile;
                    img.InputStream.Read(imgbyte, 0, length);
                    string base64String = Convert.ToBase64String(imgbyte, 0, imgbyte.Length);
                    imagenRes.ImageUrl = "data:image/png;base64," + base64String;
                    imagenRes.Visible = true;
                    String base64 = imagenRes.ImageUrl.Replace("data:image/png;base64,", "");
                    String filename = Path.GetFileName(fu.PostedFile.FileName);

                }
                catch (Exception ex)
                {

                }
            }
        }

        /*
        * Requiere: Evento de comando de fila en el grid de no conformidades
        * Modifica: Se encarga de recuperar el número de fila en el grid sobre la cual hubo una operación en alguno de sus controles (Textbox o DropDownList).
        * Retorna: N/A
        */
        protected void gridNoConformidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvRow = gridNoConformidades.Rows[index];
            
        }

        /*
        * Requiere: N/A
        * Modifica: Se encarga de recuperar toda la información presente en cada fila del grid de no conformidades para una operación de inserción o modificación.
        * Retorna: N/A
        */
        protected void recuperarNoConformidades()
        {
            foreach (GridViewRow row in gridNoConformidades.Rows)
            {
                Object[] noConformidad = new Object[8];

                DropDownList ddl1 = row.FindControl("ddlTipo") as DropDownList;
                DropDownList ddl2 = row.FindControl("ddlIdCaso") as DropDownList;
                TextBox txt1 = row.FindControl("txtDescripcion") as TextBox;
                TextBox txt2 = row.FindControl("txtJustificacion") as TextBox;
                System.Web.UI.WebControls.
                Image imagenRes = row.FindControl("imagenSubida") as System.Web.UI.WebControls.
                                                                     Image;
                DropDownList ddl3 = row.FindControl("ddlEstado") as DropDownList;
                Label lbl = row.FindControl("lblIDNC") as Label;

                String base64 = imagenRes.ImageUrl.Replace("data:image/png;base64,", "");
                byte[] imgbyte = Convert.FromBase64String(base64);

                noConformidad[0] = ddl1.SelectedItem.Text;
                noConformidad[1] = ddl2.SelectedItem.Text;
                noConformidad[2] = txt1.Text;
                noConformidad[3] = txt2.Text;
                noConformidad[4] = imgbyte;
                noConformidad[5] = ddl3.SelectedItem.Text;
                noConformidad[7] = lbl.Text;
                listaNC.Add(noConformidad);
            }
        }


        /*
        * Requiere: Evento de darle click al botón de eliminar en alguna de las filas del grid de no conformidades.
        * Modifica: Se encarga de eliminar la fila a la cual pertenece el botón que generó el evento.
        * Retorna: N/A
        */
        protected void btnEliminarFila_Click(object sender, EventArgs e)
        {
            if ( modoEP!=2 && cantidadConformidades>0)
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int index = gvRow.RowIndex;

                dtNoConformidades.Rows[index].Delete();
                for (int i = 0; i < gridNoConformidades.Rows.Count; i++)
                {
                    DropDownList ddl1 = gridNoConformidades.Rows[i].FindControl("ddlTipo") as DropDownList;
                    DropDownList ddl2 = gridNoConformidades.Rows[i].FindControl("ddlIdCaso") as DropDownList;
                    TextBox txt1 = gridNoConformidades.Rows[i].FindControl("txtDescripcion") as TextBox;
                    TextBox txt2 = gridNoConformidades.Rows[i].FindControl("txtJustificacion") as TextBox;
                    System.Web.UI.WebControls.Image imagenRes = gridNoConformidades.Rows[i].FindControl("imagenSubida") as System.Web.UI.WebControls.Image;
                    DropDownList ddl3 = gridNoConformidades.Rows[i].FindControl("ddlEstado") as DropDownList;
                    Label lbl = gridNoConformidades.Rows[i].FindControl("lblIDNC") as Label;

                    int pos = i;
                    if (pos != index)
                    {
                        if (pos > index)
                        {
                            pos--;
                        }
                        dtNoConformidades.Rows[pos]["Tipo"] = ddl1.SelectedValue;
                        dtNoConformidades.Rows[pos]["IdCaso"] = ddl2.SelectedValue;
                        dtNoConformidades.Rows[pos]["Descripcion"] = txt1.Text;
                        dtNoConformidades.Rows[pos]["Justificacion"] = txt2.Text;
                        dtNoConformidades.Rows[pos]["Resultado"] = imagenRes.ImageUrl.ToString();
                        dtNoConformidades.Rows[pos]["Estado"] = ddl3.SelectedValue;
                    }

                }
                gridNoConformidades.DataSource = dtNoConformidades;
                gridNoConformidades.DataBind();
                conservarEstado();
                cantidadConformidades -= 1;
            }
        }

        /*
        * Requiere: N/A.
        * Modifica: Se encarga de limpiar toda la información presente en el DataTable de no conformidades.
        * Retorna: N/A
        */
        protected void limpiarNC(String fecha)
        {

            dtNoConformidades = new DataTable();
            dtNoConformidades.Columns.AddRange
            (
                new DataColumn[]
                {
                    new DataColumn("Id", typeof(int)),
                    new DataColumn("Tipo", typeof(String)),
                    new DataColumn("IdCaso",typeof(String)),
                    new DataColumn("Descripcion", typeof(String)),
                    new DataColumn("Justificacion", typeof(String)),
                    new DataColumn("Resultado", typeof(String)),
                    new DataColumn("Estado", typeof(String))
                }
            );

            if (controladoraEjecucionPrueba.cantidadNoConformidades(fecha) == 0 || fecha=="")
            {
                DataRow drRow = dtNoConformidades.NewRow();

                drRow["Id"] = 1;
                drRow["Tipo"] = String.Empty;
                drRow["IdCaso"] = String.Empty;
                drRow["Descripcion"] = String.Empty;
                drRow["Justificacion"] = String.Empty;
                drRow["Resultado"] = String.Empty;
                drRow["Estado"] = string.Empty;

                dtNoConformidades.Rows.Add(drRow);
                dtNoConformidades = dtNoConformidades;
                gridNoConformidades.DataSource = dtNoConformidades;
                gridNoConformidades.DataBind();
            }
            

            gridNoConformidades.DataSource = dtNoConformidades;
            gridNoConformidades.DataBind();

        }


        /*
        * Requiere: N/A.
        * Modifica: Se encarga llenar el grid de no conformidades dependiendo de la ejecución seleccionada mediante la fecha de esta.
        * Retorna: N/A
        */
        protected void llenarGridNCConsulta(String fecha)
        {
            DataTable dtNC = controladoraEjecucionPrueba.consultarNoConformidades(fecha);
            limpiarNC(fecha);

            if (dtNC.Rows.Count > 0)
            {
                int numRow = dtNC.Rows.Count;
                for (int i = 0; i < numRow; i++)
                {
                    DataRow drCurrentRow = dtNoConformidades.NewRow();
                    dtNoConformidades.Rows.Add(drCurrentRow);
                    gridNoConformidades.DataSource = dtNoConformidades;
                    gridNoConformidades.DataBind();
                    // ddl3.Items.FindByText(dtNC.Rows[i].ItemArray[5].ToString()).Selected = true;
                }
                leerNC(dtNC);

            }

        }


       /*
        * Requiere: N/A.
        * Modifica: Se encarga de llenar el grid de no conformidades (fila por fila, control por control) con las no conformidades asociadas a la ejecución de pruebas seleccionada desde base de datos.
        * Retorna: N/A
        */
        protected void leerNC(DataTable dtNC)
        {
            for (int i = 0; i < dtNoConformidades.Rows.Count; i++)
            {
                DropDownList ddl1 = gridNoConformidades.Rows[i].FindControl("ddlTipo") as DropDownList;
                DropDownList ddl2 = gridNoConformidades.Rows[i].FindControl("ddlIdCaso") as DropDownList;
                TextBox txt1 = gridNoConformidades.Rows[i].FindControl("txtDescripcion") as TextBox;
                TextBox txt2 = gridNoConformidades.Rows[i].FindControl("txtJustificacion") as TextBox;
                System.Web.UI.WebControls.Image imagenRes = gridNoConformidades.Rows[i].FindControl("imagenSubida") as System.Web.UI.WebControls.Image;
                DropDownList ddl3 = gridNoConformidades.Rows[i].FindControl("ddlEstado") as DropDownList;
                Label lbl = gridNoConformidades.Rows[i].FindControl("lblIDNC") as Label;

                dtNoConformidades.Rows[i]["Tipo"] = ddl1.SelectedValue;
                dtNoConformidades.Rows[i]["IdCaso"] = ddl2.SelectedValue;
                dtNoConformidades.Rows[i]["Descripcion"] = txt1.Text;
                dtNoConformidades.Rows[i]["Justificacion"] = txt2.Text;
                dtNoConformidades.Rows[i]["Resultado"] = imagenRes.ImageUrl.ToString();
                dtNoConformidades.Rows[i]["Estado"] = ddl3.SelectedValue;

                ddl1.ClearSelection();
                ddl1.Items.FindByText(dtNC.Rows[i].ItemArray[0].ToString()).Selected = true;
                ddl2.ClearSelection();
                ddl2.Items.FindByText(dtNC.Rows[i].ItemArray[1].ToString()).Selected = true;
                txt1.Text = dtNC.Rows[i].ItemArray[2].ToString();
                txt2.Text = dtNC.Rows[i].ItemArray[3].ToString();
                byte[] imgbyte = (byte[])dtNC.Rows[i].ItemArray[4];
                String base64String = Convert.ToBase64String(imgbyte, 0, imgbyte.Length);
                String B64 = base64String.Substring(36);
                imagenRes.ImageUrl = "data:image/png;base64," + B64;
                ddl3.ClearSelection();
                ddl3.Items.FindByText(dtNC.Rows[i].ItemArray[5].ToString()).Selected = true;
                lbl.Text = dtNC.Rows[i].ItemArray[6].ToString();

            }
        }


       /*
        * Requiere: Evento de pasar de página en el Grid.
        * Modifica: Pasa de página y llena el Grid con las n tuplas que siguen, siendo n el tamaño de la página.
        * Retorna: N/A. 
        */
        protected void OnGridEjecucionPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridEjecucion.PageIndex = e.NewPageIndex;
            this.llenarGridEjecucion();
            gridEjecucion.DataBind();
        }


        /*
        * Requiere: El evento de enlazar información de un datatable con el Grid de "Ejecuciones de Prueba".
        * Modifica: Establece el comportamiento del Grid ante los diferentes eventos.
        * Retorna: N/A.
        */
        protected void OnGridEjecucionRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridEjecucion, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

            }
        }

       /*
        * Requiere: Evento de seleccionar un Caso de Prueba.
        * Modifica: Se encarga de controlar qué fila está seleccionada en el Grid de Ejecución de Prueba para realizar el llenado de campos correspondiente.
        * Retorna: N/A
        */
        protected void GridEjecucion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                //SELECT fecha, responsable, incidencias, id_disenno FROM Ejecucion WHERE fecha = '" + id + "';";
                string fecha = gridEjecucion.SelectedRow.Cells[0].Text;
                llenarDatosEjecucionPrueba(fecha);
                BotonEPModificar.Enabled = true;
                BotonEPEliminar.Enabled = true;

                cantidadConformidades = controladoraEjecucionPrueba.cantidadNoConformidades(fecha);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

       /*
        * Requiere: N/A
        * Modifica: Se encarga de llenar los datos en los Textbox, correspondientes a la ejecución de prueba consultado desde el Grid.
        * Retorna: N/A
        */
        protected void llenarDatosEjecucionPrueba(String fecha)
        {
            DataTable dtEjecucion = controladoraEjecucionPrueba.consultarEjecucion(2, fecha);
            this.ControlFecha.Text = dtEjecucion.Rows[0].ItemArray[0].ToString();
            this.DropDownResponsable.Text = dtEjecucion.Rows[0].ItemArray[1].ToString();
            this.TextBoxIncidencias.Text = dtEjecucion.Rows[0].ItemArray[2].ToString();
            this.DropDownDiseno.Text = dtEjecucion.Rows[0].ItemArray[3].ToString();
            llenarGridNCConsulta(fecha);

            //ver cantidad de noconformidades
        }


        /*
        * Requiere: N/A
        * Modifica: Se encarga de llenar el Grid de ejecución de pruebas pidiendo la lista de ejecuciones disponibles a la Controladora de Ejecución de Prueba.
        * Retorna: N/A
        */
        protected void llenarGridEjecucion()
        {
            DataTable ejecuciones = crearTablaGridEjecuciones();
            DataTable dt = controladoraEjecucionPrueba.consultarEjecucion(1, disennoSeleccionado);

            Object[] datos = new Object[3];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[0];
                    datos[1] = dr[1];
                    datos[2] = dr[2];
                    ejecuciones.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                datos[2] = "-";
                ejecuciones.Rows.Add(datos);
            }
            gridEjecucion.DataSource = ejecuciones;
            gridEjecucion.DataBind();
        }


       /*
        * Requiere: N/A
        * Modifica: Se encarga de inicializar las columnas del datatable del cual se llenará el Grid de Ejecuciones de Prueba.
        * Retorna: DataTable.
        */
        protected DataTable crearTablaGridEjecuciones()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Fecha", typeof(String));
            dt.Columns.Add("Responsable", typeof(String));
            dt.Columns.Add("Propósito Diseño", typeof(String));
            return dt;
        }

        /*
        * Requiere: Evento de dar click al botón "Aceptar".
        * Modifica: Se encarga de efectuar la inserción o modificación llamando a la Controladora de Ejecución de Prueba, usando los datos ingresados en los campos y confirmando al usuario el resultado de la operación.
        * Retorna: N/A
        */
        protected void BotonEPAceptar_Click(object sender, EventArgs e)
        {       
            Object[] datosNuevos = new Object[5];
            datosNuevos[0] = this.ControlFecha.Text;
            datosNuevos[1] = this.DropDownResponsable.SelectedValue.ToString();
            datosNuevos[2] = this.TextBoxIncidencias.Text;
            datosNuevos[3] = this.DropDownDiseno.SelectedItem.Value.ToString();
            recuperarNoConformidades();

            int operacion = -1;

            String res = "";
            if (modoEP == 1)
            {
                datosNuevos[4] = "";
                res = controladoraEjecucionPrueba.insertarEjecucion(datosNuevos, listaNC);
               
            }
            else if (modoEP == 2)
            {
                datosNuevos[4] = idMod;
                res = controladoraEjecucionPrueba.modificarEjecucion(datosNuevos, listaNC);
            }
            if (res != "-") operacion = 1;
            if (res == "-") operacion = 2627;

            if (operacion == 1)
            {
                switch (modoEP)
                {
                    case 1:
                        {
                            EtiqMensajeOperacion.Text = "La ejecución de prueba ha sido insertada con éxito";
                            desmarcarBoton(ref BotonEPInsertar);
                            estadoInicial();
                            break;
                        }
                    case 2:
                        {
                            EtiqMensajeOperacion.Text = "La ejecución de prueba ha sido modificada con éxito";
                            desmarcarBoton(ref BotonEPModificar);
                            break;
                        }
                }
                EtiqMensajeOperacion.ForeColor = System.Drawing.Color.DarkSeaGreen;
                llenarGridEjecucion();
               estadoPostOperacion();
            }
            else
            {
                switch (operacion)
                {
                    case 2627:
                        {
                            EtiqMensajeOperacion.Text = "Ya se hizo una ejecución asociada al diseño elegido en esta fecha.";
                            //TextBoxID.BorderColor = System.Drawing.Color.Red;
                            break;
                        }
                    default:
                        {
                            EtiqMensajeOperacion.Text = "Ocurrió un problema en la operación.";
                            break;
                        }
                }
                EtiqMensajeOperacion.ForeColor = System.Drawing.Color.Salmon;
            }
            EtiqMensajeOperacion.Visible = true;
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
        }


       /*
        * Requiere: Evento de dar click al botón "Insertar".
        * Modifica: Se encarga de preparar los controles de la aplicación para efectuar una inserción.
        * Retorna: N/A
        */
        protected void BotonEPInsertar_Click(object sender, EventArgs e)
        {
            modoEP = 1;
            estadoInsertar();
        }

       /*
        * Requiere: Evento de dar click al botón "Modificar".
        * Modifica: Se encarga de preparar los controles de la aplicación para efectuar una modificación.
        * Retorna: N/A
        */
        protected void BotonEPModificar_Click(object sender, EventArgs e)
        {
            modoEP = 2;
            idMod = ControlFecha.Text;
            estadoModificar();

        }


       /*
        * Requiere: Evento de dar click al botón "Eliminar".
        * Modifica: Se encarga de preparar los controles de la aplicación para efectuar una eliminación.
        * Retorna: N/A
        */
        protected void BotonEPEliminar_Click(object sender, EventArgs e)
        {
            

        }


        /*
         * Requiere: Evento de dar click al botón "Acepar" en el modal.
         * Modifica: Se encarga de efectuar la eliminación de la ejecución de prueba elegido y confirmar al usuario si esta fue exitosa o no.
         * Retorna: N/A
         */
        protected void eliminarAceptarModal(object sender, EventArgs e)
        {
            desmarcarBoton(ref BotonEPEliminar);
            int eliminacion = controladoraEjecucionPrueba.eliminarEjecucionPrueba(ControlFecha.Text.ToString());

            if (eliminacion == 1)
            {
                EtiqMensajeOperacion.Text = "El caso de prueba se ha eliminado correctamente";
                EtiqMensajeOperacion.ForeColor = System.Drawing.Color.DarkSeaGreen;
                EtiqMensajeOperacion.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                estadoPostOperacion();
                
            }
            else
            {
                EtiqMensajeOperacion.Text = "El caso de prueba no pudo ser eliminado, ocurrió un error";
                EtiqMensajeOperacion.ForeColor = System.Drawing.Color.Salmon;
                EtiqMensajeOperacion.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);

            }
        }

        /*
         * Requiere: Evento de dar click al botón "Si" en en el modal de cancelar.
         * Modifica: Se de cancelar la operación en ejecución, retornando la pantalla a como estaba antes de elegir la operación.
         * Retorna: N/A
         */
        protected void cancelarBotonSiModal_Click(object sender, EventArgs e)
        {
            if (modoEP == 1)
            {
                estadoInicial();
                desmarcarBoton(ref BotonEPInsertar);
                limpiarCampos();
                limpiarNC("");

            }
            else if (modoEP == 2)
            {
                llenarDatosEjecucionPrueba(DropDownDiseno.SelectedItem.Text.ToString());
                estadoPostOperacion();
                desmarcarBoton(ref BotonEPModificar);
            }
            
        }

       /*
        * Requiere: GridView.
        * Modifica: Se encarga de deshabilitar las funciones del Grid pasado por parámetro.
        * Retorna: N/A
        */
        protected void deshabilitarGrid(ref GridView grid)
        {
            grid.Enabled = false;
            foreach (GridViewRow row in grid.Rows)
            {
                row.Attributes.Remove("onclick");
                row.Attributes.Remove("onmouseover");
                row.Attributes.Remove("style");
                row.Attributes.Remove("onmouseout");
            }
        }

        
    }


}
