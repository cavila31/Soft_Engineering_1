using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;


using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.IO;



namespace SistemaPruebas.Intefaces
{
    public partial class InterfazReporte : System.Web.UI.Page
    {
        //Variables:
        Controladoras.ControladoraReportes controladoraGR = new Controladoras.ControladoraReportes();

        public static string modoGR
        {
            get
            {
                object value = HttpContext.Current.Session["modoGR"];
                return value == null ? "0" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["modoGR"] = value;
            }
        }

        public static string idViejoGR
        {
            get
            {
                object value = HttpContext.Current.Session["idViejoGR"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["idViejoGR"] = value;
            }
        }

        public static string esAdminGR
        {
            get
            {
                object value = HttpContext.Current.Session["esAdminGR"];
                return value == null ? "false" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["esAdminGR"] = value;
            }
        }

        public static string proyectoActualGR
        {
            get
            {
                object value = HttpContext.Current.Session["proyectoActualGR"];
                return value == null ? "0" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["proyectoActualGR"] = value;
            }
        }
        public static string modActualGR
        {
            get
            {
                object value = HttpContext.Current.Session["modActualGR"];
                return value == null ? "" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["modActualGR"] = value;
            }
        }
        public static string reqActualGR
        {
            get
            {
                object value = HttpContext.Current.Session["reqActualGR"];
                return value == null ? "" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["reqActualGR"] = value;
            }
        }
        public static string PPindexViejo
        {
            get
            {
                object value = HttpContext.Current.Session["PPindexViejo"];
                return value == null ? "0" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["PPindexViejo"] = value;
            }
        }
        /*
         * Requiere: Que suceda el evento de refrescar la pagina
         * Modifica: Refresca la pagina.
         * Retorna: N/A.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)// ES SOLO LA PRIMERA VEZ
            {
                volverAlOriginal();
                llenarGridPP();
                llenarGridMod("");
                llenarGridReq("", "");
                DataTable dt = new DataTable();
                dt.Columns.Add("Nombre del Requerimiento.", typeof(String));
                llenarDDArchivo();
                proyectoSeleccionadoLabel.Visible = false;
                moduloSeleccionadoLabel.Visible = false;
                reqSeleccionadoLabel.Visible = false;

                proyectoSeleccionado.Visible = false;
                modSeleccionado.Visible = false;
                reqSeleccionado.Visible = false;
            }
            EtiqErrorGR.Text = "";

        }
        /*
         * Requiere: N/A.
         * Modifica: Vuelve al inicio de generar reportes.
         * Retorna: N/A.
         */
        protected void volverAlOriginal()
        {
            EtiqErrorGR.Text = "";
            modoGR = Convert.ToString(0);
            Generar.Text = "Generar Reporte";
            deselTodos_CheckedChanged(null, null);
            enabledChecks();
            CheckBoxNombReq.Checked = true;
            CheckBoxNombModulo.Checked = true;
            CheckBoxNombreProyecto.Checked = true;
            llenarGridPP();
            proyectoActualGR = "";
            modActualGR = "";
            reqActualGR = "";
            proyectoSeleccionado.Text = "";
            modSeleccionado.Text = "";
            reqSeleccionado.Text = "";
            proyectoSeleccionadoLabel.Visible = false;
            moduloSeleccionadoLabel.Visible = false;
            reqSeleccionadoLabel.Visible = false;
            proyectoSeleccionado.Visible = false;
            modSeleccionado.Visible = false;
            reqSeleccionado.Visible = false;
            limpiarPreGrid();
        }
        /*
         * Requiere: N/A
         * Modifica: Exporta el grid de vista previa a un documento de word (.doc).
         * Retorna: N/A.
         */
        private void exportarWord()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "ReporteDoroteos.doc";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/msword";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            preGrid.GridLines = GridLines.Both;
            preGrid.HeaderStyle.Font.Bold = true;
            preGrid.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        /*
         * Requiere: N/A
         * Modifica: N/A.
         * Retorna: N/A.
         */
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the
            //specified ASP.NET server control at run time.
        }

        /*
         * Requiere: N/A
         * Modifica: Exporta el grid de vista previa a un documento de excel.
         * Retorna: N/A.
         */
        protected void generarReporteExcel(object sender, EventArgs e)
        {
            ExcelPackage package = new ExcelPackage();
            package.Workbook.Worksheets.Add("Proyectos");
            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
            worksheet.Cells.Style.Font.Size = 12;
            worksheet.Cells.Style.Font.Name = "Calibri";
            // Poner un titulo.
            worksheet.Cells[1, 1].Value = "Reporte de proyecto los doroteos " + DateTime.Today.ToString("(dd/MM/yyyy).");
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Row(1).Style.Font.Size = 14;
            // Rellenar los datos.
            int c = 1;
            int r = 2;
            // Poner el header.
            foreach (System.Web.UI.WebControls.TableCell cell in preGrid.HeaderRow.Cells)
            {
                worksheet.Cells[r, c++].Value = HttpUtility.HtmlDecode(cell.Text);
            }
            // Dar formato al header.
            worksheet.Row(r).Style.Font.Bold = true;
            worksheet.Row(r).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Row(r).Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            r++;
            // Poner el resto de los datos.
            foreach (System.Web.UI.WebControls.TableRow row in preGrid.Rows)
            {
                c = 1;
                foreach (System.Web.UI.WebControls.TableCell cell in row.Cells)
                {                  
                    worksheet.Cells[r, c++].Value = HttpUtility.HtmlDecode(cell.Text);
                }
                // Coloreamos las filas.
                if (0 == r % 2)
                {
                    worksheet.Row(r).Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Row(r).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }
                r++;
            }
            // Ajustamos el ancho de las columnas.
            worksheet.DefaultColWidth = 10;
            worksheet.Cells.AutoFitColumns();
            Response.Clear();
            Response.Buffer = true;
            Response.BinaryWrite(package.GetAsByteArray());
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Reporte.xlsx");
            Response.Flush();
            Response.End();
        }


        /*
         * Requiere: N/A
         * Modifica: Llena el grid de proyecto.
         * Retorna: N/A.
         */
        protected void llenarGridPP()
        {
            GridPP.SelectedIndex = -1;
            DataTable dtGrid = crearTablaPP();
            DataTable dt = controladoraGR.consultarProyecto();
            Object[] datos = new Object[2];


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[1];
                    datos[1] = dr[2];
                    dtGrid.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                dtGrid.Rows.Add(datos);
            }
            GridPP.DataSource = dtGrid;
            GridPP.DataBind();
            llenarGridMod("");
            llenarGridReq("", "");
        }
        /*
         * Requiere: N/A.
         * Modifica: Crea el DataTable de proyecto.
         * Retorna: N/A.
         */
        protected DataTable crearTablaPP()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Nombre del Proyecto.", typeof(String));
            dt.Columns.Add("        Líder.      ", typeof(String));
            return dt;
        }
        /*
         * Requiere: String nombreProyecto.
         * Modifica: Llena el grid de modulos.
         * Retorna: N/A.
         */
        protected void llenarGridMod(String nomProyecto)
        {
            GridMod.SelectedIndex = -1;
            DataTable dtGrid = crearTablaMod();
            DataTable dt = controladoraGR.consultarModulos(nomProyecto);
            Object[] datos = new Object[1];


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[0];
                    dtGrid.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                dtGrid.Rows.Add(datos);
            }
            GridMod.DataSource = dtGrid;
            GridMod.DataBind();
        }
        /*
         * Requiere: N/A.
         * Modifica: Llena el dropdownlist del tipo de Archivo.
         * Retorna: N/A.
         */
        protected void llenarDDArchivo()
        {
            this.DDLTipoArchivo.Items.Clear();

            try
            {
                this.DDLTipoArchivo.Items.Add(new System.Web.UI.WebControls.ListItem("Tipo de Archivo", Convert.ToString(0)));
                this.DDLTipoArchivo.Items.Add(new System.Web.UI.WebControls.ListItem("Excel", Convert.ToString(1)));
                this.DDLTipoArchivo.Items.Add(new System.Web.UI.WebControls.ListItem("Word", Convert.ToString(2)));
                this.DDLTipoArchivo.Items.Add(new System.Web.UI.WebControls.ListItem("PDF", Convert.ToString(3)));
            }
            catch (Exception e)
            {
            }

        }
        /*
         * Requiere: N/A.
         * Modifica: N/A.
         * Retorna: Crea la tabla de modulos.
         */
        protected DataTable crearTablaMod()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("       Módulo.      ", typeof(String));
            return dt;
        }

        /*
         * Requiere: String nombreProyecto y String nombreModulo.
         * Modifica: Llena el grid de requerimiento.
         * Retorna: N/A.
         */
        protected void llenarGridReq(String nomProyecto, String nomModulo)
        {
            GridReq.SelectedIndex = -1;
            DataTable dtGrid = crearTablaReq();
            DataTable dt = controladoraGR.consultarRequerimientos(nomProyecto, nomModulo);
            Object[] datos = new Object[1];


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[0];
                    dtGrid.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                dtGrid.Rows.Add(datos);
            }
            GridReq.DataSource = dtGrid;
            GridReq.DataBind();
        }
        /*
         * Requiere: DataTable.
         * Modifica: Llena el grid de reporte.
         * Retorna: N/A.
         */
        protected void llenarGridGR(DataTable dt)
        {

            Object[] datos = new Object[1];


            if (dt.Rows.Count > 0)
            {

            }
            else
            {
                datos[0] = "-";
                dt.Rows.Add(datos);
            }
            GridGR.DataSource = dt;
            GridGR.DataBind();
        }
        /*
         * Requiere: N/A.
         * Modifica: Crea el DataTable de requerimiento.
         * Retorna: N/A.
         */
        protected DataTable crearTablaReq()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Nombre del Requerimiento.", typeof(String));
            return dt;
        }

        /*
         * Requiere: N/A.
         * Modifica: Deshabilta las checkbox.
         * Retorna: N/A.
         */
        protected void deshabilitarPP()
        {
            CheckBoxEstadoProyecto.Enabled = false;
            CheckBoxFechAsignacionProyecto.Enabled = false;
            CheckBoxMiembrosProyecto.Enabled = false;
            CheckBoxNombreProyecto.Enabled = false;
            CheckBoxObjetivoProyecto.Enabled = false;
            CheckBoxOficinaProyecto.Enabled = false;
            CheckBoxResponsableProyecto.Enabled = false;
            CheckBoxCantNoConf.Enabled = false;
            CheckBoxExitos.Enabled = false;
            CheckBoxNombModulo.Enabled = false;
            CheckBoxTipoNoConf.Enabled = false;
            CheckBoxNombReq.Enabled = false;
        }

        /*
         * Requiere: N/A.
         * Modifica: N/A.
         * Retorna: Un array de booleanos que indican que checkbox marco el usuario.
         */
        protected bool[] datosProy()
        {
            bool[] proyecto = new bool[12];
            proyecto[0] = CheckBoxNombreProyecto.Checked;
            proyecto[1] = CheckBoxNombModulo.Checked;
            proyecto[2] = CheckBoxNombReq.Checked;
            proyecto[3] = CheckBoxFechAsignacionProyecto.Checked;
            proyecto[4] = CheckBoxOficinaProyecto.Checked;
            proyecto[5] = CheckBoxResponsableProyecto.Checked;
            proyecto[6] = CheckBoxObjetivoProyecto.Checked;
            proyecto[7] = CheckBoxEstadoProyecto.Checked;
            proyecto[8] = CheckBoxMiembrosProyecto.Checked;
            proyecto[9] = CheckBoxExitos.Checked;
            proyecto[10] = CheckBoxTipoNoConf.Checked;
            proyecto[11] = CheckBoxCantNoConf.Checked;

            return proyecto;
        }
        /*
         * Requiere: Cuando se presiona el boton generar reporte.
         * Modifica: Se llaman los metodos de la controladora que devuelven los datos necesarions para generar un reporte, despues se despliegan en el grid de vista previa.
         * Retorna: N/A.
         */
        protected void BotonGE_Click(object sender, EventArgs e)
        {
            //revisar como se llaman los metodos de la controladora.
            bool[] proyecto = datosProy();
            CheckBox[] checks = { CheckBoxNombreProyecto, CheckBoxObjetivoProyecto, CheckBoxFechAsignacionProyecto, CheckBoxEstadoProyecto, CheckBoxOficinaProyecto, CheckBoxResponsableProyecto, CheckBoxMiembrosProyecto, CheckBoxNombModulo, CheckBoxNombReq, CheckBoxExitos, CheckBoxCantNoConf, CheckBoxTipoNoConf };
            if (Generar.Text == "Generar Reporte")
            {

                if (proyectoActualGR != "")
                {
                    unenabledChecks();
                    Generar.Text = "Agregar a Reporte";
                    modoGR = Convert.ToString(1);
                    DataTable dt = headerPreGrid(checks);
                    List<Object> proyectoDatos = controladoraGR.reporteProyecto(controladoraGR.consultarProyecto(proyectoActualGR));
                    if (GridMod.SelectedIndex != -1)//Un módulo
                    {
                        proyectoDatos.Add(modSeleccionado.Text);
                        if (GridReq.SelectedIndex != -1)//Un solo requerimiento
                        {
                            proyectoDatos = controladoraGR.medicionRequerimiento(proyectoDatos, reqSeleccionado.Text);
                            ProyectoPreGrid(proyectoDatos, dt, checks);
                        }
                        else//Todos los requerimientos de un módulo
                        {
                            if (checks[7].Checked || checks[8].Checked || checks[9].Checked || checks[10].Checked)
                            {

                                foreach (GridViewRow id in GridReq.Rows)
                                {
                                    List<Object> comodin = new List<object>(proyectoDatos);
                                    comodin = controladoraGR.medicionRequerimiento(comodin, id.Cells[0].Text);
                                    ProyectoPreGrid(comodin, dt, checks);
                                    //comodin.Clear();
                                }
                            }
                            else
                            {
                                ProyectoPreGrid(proyectoDatos, dt, checks);
                            }
                        }
                    }
                    else//Todos los módulos de un proyecto
                    {

                        foreach (GridViewRow dr in GridMod.Rows)
                        {
                            List<Object> comodin = new List<object>(proyectoDatos);
                            if (dr.Cells[0].Text != "-" || dr.Cells[0].Text != "")
                            {

                                comodin.Add(dr.Cells[0].Text);
                                if (checks[7].Checked || checks[8].Checked || checks[9].Checked || checks[10].Checked)
                                {
                                    if (controladoraGR.consultarRequerimientos(proyectoActualGR, dr.Cells[0].Text).Rows.Count > 0)
                                    {
                                        foreach (DataRow id in controladoraGR.consultarRequerimientos(proyectoActualGR, dr.Cells[0].Text).Rows)
                                        {
                                            List<Object> comodinReq = new List<object>(comodin);
                                            comodinReq = controladoraGR.medicionRequerimiento(comodinReq, id[0].ToString());
                                            ProyectoPreGrid(comodinReq, dt, checks);
                                        }
                                    }
                                    else
                                    {
                                        ProyectoPreGrid(comodin, dt, checks);
                                    }
                                }
                                else
                                {
                                    ProyectoPreGrid(comodin, dt, checks);
                                }
                            }
                            else
                            {
                                comodin.Add("N/A");
                                ProyectoPreGrid(comodin, dt, checks);
                            }

                        }
                    }                    
                }
                else //NO proyecto marcado
                {
                    EtiqErrorGR.Text = "*Primero debe primero eligir algún proyecto.";
                    EtiqErrorGR.ForeColor = System.Drawing.Color.Salmon;
                    EtiqErrorGR.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                }
            }
            else //Segunda o más líneas
            {
                DataTable dt = fromGridToDT();
                if (proyectoActualGR != "")
                {
                    //Generar.Text = "Agregar a Reporte";
                    //modoGR = Convert.ToString(1);
                    //DataTable dt = headerPreGrid(checks);
                    List<Object> proyectoDatos = controladoraGR.reporteProyecto(controladoraGR.consultarProyecto(proyectoActualGR));
                    if (GridMod.SelectedIndex != -1)//Un módulo
                    {
                        proyectoDatos.Add(modSeleccionado.Text);
                        if (GridReq.SelectedIndex != -1)//Un solo requerimiento
                        {
                            proyectoDatos = controladoraGR.medicionRequerimiento(proyectoDatos, reqSeleccionado.Text);
                            ProyectoPreGrid(proyectoDatos, dt, checks);
                        }
                        else//Todos los requerimientos de un módulo
                        {
                            if (checks[7].Checked || checks[8].Checked || checks[9].Checked || checks[10].Checked)
                            {

                                foreach (GridViewRow id in GridReq.Rows)
                                {
                                    List<Object> comodin = new List<object>(proyectoDatos);
                                    comodin = controladoraGR.medicionRequerimiento(comodin, id.Cells[0].Text);
                                    ProyectoPreGrid(comodin, dt, checks);
                                    //comodin.Clear();
                                }
                            }
                            else
                            {
                                ProyectoPreGrid(proyectoDatos, dt, checks);
                            }
                        }
                    }
                    else//Todos los módulos de un proyecto
                    {

                        foreach (GridViewRow dr in GridMod.Rows)
                        {
                            List<Object> comodin = new List<object>(proyectoDatos);
                            if (dr.Cells[0].Text != "-" || dr.Cells[0].Text != "")
                            {

                                comodin.Add(dr.Cells[0].Text);
                                if (checks[7].Checked || checks[8].Checked || checks[9].Checked || checks[10].Checked)
                                {
                                    if (controladoraGR.consultarRequerimientos(proyectoActualGR, dr.Cells[0].Text).Rows.Count > 0)
                                    {
                                        foreach (DataRow id in controladoraGR.consultarRequerimientos(proyectoActualGR, dr.Cells[0].Text).Rows)
                                        {
                                            List<Object> comodinReq = new List<object>(comodin);
                                            comodinReq = controladoraGR.medicionRequerimiento(comodinReq, id[0].ToString());
                                            ProyectoPreGrid(comodinReq, dt, checks);
                                        }
                                    }
                                    else
                                    {
                                        ProyectoPreGrid(comodin, dt, checks);
                                    }
                                }
                                else
                                {
                                    ProyectoPreGrid(comodin, dt, checks);
                                }
                            }
                            else
                            {
                                comodin.Add("N/A");
                                ProyectoPreGrid(comodin, dt, checks);
                            }

                        }
                    }                    
                }
                else //NO proyecto marcado
                {
                    EtiqErrorGR.Text = "*Primero debe primero eligir algún proyecto.";
                    EtiqErrorGR.ForeColor = System.Drawing.Color.Salmon;
                    EtiqErrorGR.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                }
                
            }
        }




        /*
         * Requiere: El evento de enlazar información de un datatable con el grid
         * Modifica: Establece el comportamiento del grid ante los diferentes eventos.
         * Retorna: N/A.
         */
        protected void PP_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridPP, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        /*
         * Requiere: Evento de pasar de página en el grid.
         * Modifica: Pasa de página y llena el grid con las n tuplas que siguen, siendo n el tamaño de la página.
         * Retorna: N/A. 
        */
        protected void PP_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridPP.PageIndex = e.NewPageIndex;
            this.llenarGridPP();
        }
        /*
         * Requiere: Cuando se cambia el proyecto seleccionado en el grid de proyecto.
         * Modifica: Se cambia la etiqueta que indica de cual proyecto se va a generar el reporte.
         * Retorna: N/A.
         */
        protected void PP_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridPP.SelectedRow.RowIndex;
            String ced = GridPP.SelectedRow.Cells[0].Text;
            if (ced != "-")
            {
                if (proyectoActualGR != ced.ToString())
                {
                    proyectoActualGR = ced.ToString();
                    reqActualGR = "";
                    modActualGR = "";
                    llenarGridMod(ced);
                    llenarGridReq("", "");
                    proyectoSeleccionado.Text = ced;
                    proyectoSeleccionado.Visible = true;
                    proyectoSeleccionadoLabel.Visible = true;
                    moduloSeleccionadoLabel.Visible = true;
                    modSeleccionado.Visible = true;
                    modSeleccionado.Text = "Todos";
                    reqSeleccionadoLabel.Visible = true;
                    reqSeleccionado.Visible = true;
                    reqSeleccionado.Text = "Todos";
                    modActualGR = "";
                    reqActualGR = "";
                }
            }
        }
        /*
         * Requiere: El evento de enlazar información de un datatable con el grid
         * Modifica: Establece el comportamiento del grid ante los diferentes eventos.
         * Retorna: N/A.
         */
        protected void Mod_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridMod, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        /*
         * Requiere: Evento de pasar de página en el grid.
         * Modifica: Pasa de página y llena el grid con las n tuplas que siguen, siendo n el tamaño de la página.
         * Retorna: N/A. 
         */
        protected void Mod_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridMod.PageIndex = e.NewPageIndex;
        }




        /*
         * Requiere: Cuando se cambia el estado del checkbox nombre del re.
         * Modifica: Se cambia la etiqueta que indica de cual modulo se va a generar el reporte.
         * Retorna: N/A.
         */
        protected void Mod_SelectedIndexChanged(object sender, EventArgs e)
        {
            String ced = GridMod.SelectedRow.Cells[0].Text;
            if (ced != "-")
            {
                if (modActualGR != ced.ToString())
                {
                    modActualGR = ced.ToString();
                    reqActualGR = "";
                    llenarGridReq(proyectoActualGR, modActualGR);
                    modSeleccionado.Text = ced;
                    moduloSeleccionadoLabel.Visible = true;
                    modSeleccionado.Visible = true;
                }
            }
            reqSeleccionadoLabel.Visible = true;
            reqSeleccionado.Visible = true;
            reqSeleccionado.Text = "Todos";
        }




        /*
         * Requiere: El evento de enlazar información de un datatable con el grid
         * Modifica: Establece el comportamiento del grid ante los diferentes eventos.
         * Retorna: N/A.
         */
        protected void Req_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridReq, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        /*
         * Requiere: Evento de pasar de página en el grid.
         * Modifica: Pasa de página y llena el grid con las n tuplas que siguen, siendo n el tamaño de la página.
         * Retorna: N/A. 
         */
        protected void Req_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridReq.PageIndex = e.NewPageIndex;
        }


        /*
        * Requiere: El evento de enlazar información de un datatable con el grid
        * Modifica: Establece el comportamiento del grid ante los diferentes eventos.
        * Retorna: N/A.
        */
        protected void Reporte_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#154b67'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridGR, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        /*
         * Requiere: Evento de pasar de página en el grid.
         * Modifica: Pasa de página y llena el grid con las n tuplas que siguen, siendo n el tamaño de la página.
         * Retorna: N/A. 
         */
        protected void Reporte_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridGR.PageIndex = e.NewPageIndex;
        }



        /*
         * Requiere: N/A
         * Modifica: N/A.
         * Retorna: N/A.
         */
        protected void Reporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridGR.SelectedRow.RowIndex;
            String ced = GridGR.SelectedRow.Cells[0].Text;
        }
        /*
         * Requiere: Cuando se selecciona un elemento del grid de requerimiento.
         * Modifica: Se cambia la etiqueta que indica de cual requerimiento se va a generar el reporte.
         * Retorna: N/A.
         */
        protected void Req_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GridReq.SelectedRow.RowIndex;
            String ced = GridReq.SelectedRow.Cells[0].Text;
            if (ced != "-")
            {
                if (reqActualGR != ced.ToString())
                {
                    reqActualGR = ced.ToString();
                    reqSeleccionado.Text = ced;
                    reqSeleccionadoLabel.Visible = true;
                    reqSeleccionado.Visible = true;
                }
            }
        }
        /*
         * Requiere: Cuando se cambia el estado del checkbox nombre proyecto.
         * Modifica: Se le añaden las columnas al pregrid.
         * Retorna: N/A.
         */
        protected void CheckBoxNombreProyecto_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxNombreProyecto.Checked)
            {
                BoundField test = new BoundField();
                test.DataField = sender.ToString();
                test.HeaderText = sender.ToString();
                preGrid.Columns.Add(test);
            }
        }
        /*
         * Requiere: N/A
         * Modifica: Llena el Header del grid de vista previa.
         * Retorna: DataTable con el Header que va a aparecer en el grid de vista previa.
         */
        protected DataTable headerPreGrid(CheckBox[] checks)
        {
            DataTable dt = new DataTable();
            foreach (CheckBox check in checks)
            {
                if (check.Checked)
                {
                    if (check.ID != "CheckBoxOficinaProyecto")
                        dt.Columns.Add(check.Text, typeof(String));
                    else
                    {
                        dt.Columns.Add("Oficina del representate", typeof(String));
                        dt.Columns.Add("Teléfono del representante", typeof(String));
                        dt.Columns.Add("Nombre del usuario representate", typeof(String));
                    }
                }

            }
            return dt;
        }
        /*
         * Requiere: N/A
         * Modifica: Llena el grid de vista previa.
         * Retorna: DataTable con la informacion solicitada por el grid de vista previa.
         */
        protected DataTable ProyectoPreGrid(List<Object> objeto, DataTable dt, CheckBox[] checks)
        {
            object[] datos = new object[dt.Columns.Count];
            DataRow row = dt.NewRow();
            int i = 0;
            int j = 0;
            foreach (CheckBox check in checks)
            {
                if (check.Checked)
                {
                    if (check.ID != "CheckBoxOficinaProyecto")
                    {
                        if (j < objeto.Count)
                            datos[i] = objeto[j].ToString();
                        else
                            datos[i] = "N/A";
                        row[i] = datos[i].ToString();
                        datos[i] = row[i];
                        i++;
                    }
                    else
                    {
                        datos[i] = objeto[j].ToString();
                        row[i] = datos[i].ToString();
                        datos[i] = row[i];
                        datos[i + 1] = objeto[j + 1].ToString();
                        row[i + 1] = datos[i + 1].ToString();
                        datos[i + 1] = row[i + 1];
                        datos[i + 2] = objeto[j + 2].ToString();
                        row[i + 2] = datos[i + 2].ToString();
                        datos[i + 2] = row[i + 2];
                        i += 3;
                    }
                }
                if (check.ID == "CheckBoxOficinaProyecto")
                    j += 2;
                j++;
            }
            dt.Rows.Add(datos);
            preGrid.DataSource = dt;
            preGrid.DataBind();
            return dt;
        }
        /*
         * Requiere: N/A
         * Modifica: Cuando pasa el evento de presionar el boton cancelar.
         * Retorna: N/A.
         */
        protected void BotonDescGR_Click(object sender, EventArgs e)
        {
            if (modoGR == Convert.ToString(1))
            {
                if (DDLTipoArchivo.SelectedItem.Text == "Tipo de Archivo")
                {
                    EtiqErrorGR.Text = "*Señale el tipo de archivo en que se exportará el reporte.";
                    EtiqErrorGR.ForeColor = System.Drawing.Color.Salmon;
                  
                }
                else if (DDLTipoArchivo.SelectedItem.Text == "Excel")
                {
                    EtiqErrorGR.Text = "*Achivo exportado a Excel.";
                    EtiqErrorGR.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    generarReporteExcel(sender, e);
                    volverAlOriginal();
                }
                else if (DDLTipoArchivo.SelectedItem.Text == "Word")
                {
                    EtiqErrorGR.Text = "*Archivo exportado a Word.";
                    EtiqErrorGR.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    exportarWord();
                    volverAlOriginal();
                }
                else if (DDLTipoArchivo.SelectedItem.Text == "PDF")
                {
                    EtiqErrorGR.Text = "*Archivo exportado a PDF.";
                    EtiqErrorGR.ForeColor = System.Drawing.Color.DarkSeaGreen;
                    exportarToPdf();
                    volverAlOriginal();
                }
                EtiqErrorGR.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
            }
            else
            {
                EtiqErrorGR.Text = "*Antes de descargar el reporte, debe hacer la vista previa. ";
                EtiqErrorGR.ForeColor = System.Drawing.Color.Salmon;
                EtiqErrorGR.Visible = true;
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
            volverAlOriginal();
        }
        /*
         * Requiere: Que suceda el eveto de dar click en el boton cancelar y luego cuando aparezca el modal de confirmacion presiona que no. 
         * Modifica: N/A. 
         * Retorna: N/A.      
        */
        protected void siModalCancelar_Click(object sender, EventArgs e)
        {

        }
        /*
         * Requiere: N/A
         * Modifica: Exporta el grid de vista previa a un PDF y abre una nueva pestaña en el browser que la previsualiza. Si el usuario lo desea puede descargar el documento desde ahi.
         * Retorna: N/A.
         */
        protected void exportarToPdf()
        {
            string nombreReporte = "Reporte Doroteos.pdf";


            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER);
            if (preGrid.Rows[0].Cells.Count > 4)
                doc.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate()); 

            var output = new System.IO.FileStream(Server.MapPath(nombreReporte), System.IO.FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, output);
            doc.Open();

            iTextSharp.text.Rectangle page = doc.PageSize;
            PdfPTable head = new PdfPTable(1);
            head.TotalWidth = page.Width;
            Phrase phrase = new Phrase("Reporte generado el: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " GMT", new iTextSharp.text.Font(iTextSharp.text.Font.COURIER, 8));
            PdfPCell c = new PdfPCell(phrase);
            c.Border = iTextSharp.text.Rectangle.NO_BORDER;
            c.VerticalAlignment = Element.ALIGN_TOP;
            c.HorizontalAlignment = Element.ALIGN_CENTER;
            head.AddCell(c);
            doc.Add(head);
            doc.AddCreationDate();
            iTextSharp.text.Font boldFont = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 24, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font boldFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.TIMES_ROMAN, 14, iTextSharp.text.Font.BOLD);
            doc.Add(new iTextSharp.text.Paragraph("Reporte de proyectos", boldFont));
            doc.Add(new iTextSharp.text.Paragraph(" ", boldFont));
          

            BaseFont fieldFontRoman = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font normalCell = new iTextSharp.text.Font(fieldFontRoman, 11, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font ff = new iTextSharp.text.Font(fieldFontRoman, 13, iTextSharp.text.Font.BOLD);

            /*Se agregan datos de proyecto, en caso de ser seleccionado*/

            PdfPTable table = new PdfPTable(preGrid.Rows[0].Cells.Count);

            for (int i = 0; i < preGrid.Rows[0].Cells.Count; i++)
            {
                Phrase p = new Phrase(HttpUtility.HtmlDecode(preGrid.HeaderRow.Cells[i].Text), ff);
                PdfPCell cell = new PdfPCell(p);
                float level = 0.80F;
                GrayColor gray = new GrayColor(level);
                cell.BackgroundColor = gray;                               
               
                bool tiene = cell.HasMinimumHeight();
                //Phrase p = new Phrase(quitarTildes(preGrid.HeaderRow.Cells[i].Text), ff);
                table.AddCell(cell);
            }

            foreach (GridViewRow row in preGrid.Rows)
            {
                for (int i = 0; i < preGrid.Rows[0].Cells.Count; i++)
                {
                    
                    Phrase p = new Phrase(HttpUtility.HtmlDecode(row.Cells[i].Text), normalCell);
                    //Phrase p = new Phrase(quitarTildes(row.Cells[i].Text), normalCell);
                    PdfPCell cell = new PdfPCell(p);
                    cell.PaddingBottom = 7.0F;
                    cell.PaddingTop = 7.0F;
                    table.AddCell(cell);
                }

            }

            doc.Add(table);

            //Se cierra documento
            doc.Close();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + nombreReporte + "','_newtab');", true);
        }
        /*
         * Requiere: N/A
         * Modifica: Click en el boton cancelar de la interfaz.
         * Retorna: N/A.
         */
        protected void Button2_Click(object sender, EventArgs e)
        {
            volverAlOriginal();
        }
        /*
         * Requiere: N/A
         * Modifica: Limpia el grid de vista previa.
         * Retorna: N/A.
         */
        protected void limpiarPreGrid()
        {
            preGrid.DataSource = null;
            preGrid.DataBind();
            preGrid.Columns.Clear();
        }
        /*
         * Requiere: N/A
         * Modifica: Selecciona todos los checkbox.
         * Retorna: N/A.
         */
        protected void selTodos_CheckedChanged(object sender, EventArgs e)
        {
            deselTodos.Checked = false;
            CheckBox[] checks = { CheckBoxNombreProyecto, CheckBoxObjetivoProyecto, CheckBoxFechAsignacionProyecto, CheckBoxEstadoProyecto, CheckBoxOficinaProyecto, CheckBoxResponsableProyecto, CheckBoxMiembrosProyecto, CheckBoxNombModulo, CheckBoxNombReq, CheckBoxExitos, CheckBoxCantNoConf, CheckBoxTipoNoConf };
            foreach (CheckBox check in checks)
            {
                check.Checked = true;
            }
           
        }
        /*
         * Requiere: N/A
         * Modifica: Deselecciona todos los checkbox.
         * Retorna: N/A.
         */
        protected void deselTodos_CheckedChanged(object sender, EventArgs e)
        {
            selTodos.Checked = false;
            CheckBox[] checks = { CheckBoxNombreProyecto, CheckBoxObjetivoProyecto, CheckBoxFechAsignacionProyecto, CheckBoxEstadoProyecto, CheckBoxOficinaProyecto, CheckBoxResponsableProyecto, CheckBoxMiembrosProyecto, CheckBoxNombModulo, CheckBoxNombReq, CheckBoxExitos, CheckBoxCantNoConf, CheckBoxTipoNoConf };
            foreach (CheckBox check in checks)
            {
                check.Checked = false;
            }

        }

        /*
         * Requiere: N/A
         * Modifica: Transforma el grid de previsualizacion en un DataTable.
         * Retorna: N/A.
         */
        protected DataTable fromGridToDT()
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < preGrid.Rows[0].Cells.Count; i++)
            {
                dt.Columns.Add(HttpUtility.HtmlDecode(preGrid.HeaderRow.Cells[i].Text));                
            }


            foreach (GridViewRow row in preGrid.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < preGrid.Rows[0].Cells.Count; i++)
                {
                    dr[i] = HttpUtility.HtmlDecode(row.Cells[i].Text);
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        /*
         * Requiere: N/A
         * Modifica: Deshabilita los checkbox.
         * Retorna: N/A.
         */
        protected void unenabledChecks()
        {
            CheckBox[] checks = { CheckBoxNombreProyecto, CheckBoxObjetivoProyecto, CheckBoxFechAsignacionProyecto, CheckBoxEstadoProyecto, CheckBoxOficinaProyecto, CheckBoxResponsableProyecto, CheckBoxMiembrosProyecto, CheckBoxNombModulo, CheckBoxNombReq, CheckBoxExitos, CheckBoxCantNoConf, CheckBoxTipoNoConf };
            foreach (CheckBox check in checks)
                check.Enabled = false;
            selTodos.Enabled = false;
            deselTodos.Enabled = false;
        }
        /*
         * Requiere: N/A
         * Modifica: Vuelve a habilitar los checkbox.
         * Retorna: N/A.
         */
        protected void enabledChecks()
        {
            CheckBox[] checks = { CheckBoxNombreProyecto, CheckBoxObjetivoProyecto, CheckBoxFechAsignacionProyecto, CheckBoxEstadoProyecto, CheckBoxOficinaProyecto, CheckBoxResponsableProyecto, CheckBoxMiembrosProyecto, CheckBoxNombModulo, CheckBoxNombReq, CheckBoxExitos, CheckBoxCantNoConf, CheckBoxTipoNoConf };
            foreach (CheckBox check in checks)
                check.Enabled = true;
            selTodos.Enabled = true;
            deselTodos.Enabled = true;
        }

    }

}






