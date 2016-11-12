using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using SistemaPruebas.Controladoras;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SistemaPruebas.Intefaces
{
    public partial class CasosDePrueba : System.Web.UI.Page
    {
        ControladoraCasosPrueba controladoraCasosPrueba = new ControladoraCasosPrueba();


       /*
        * Requiere: N/A
        * Modifica: Declara dataTable global de datos de entrada
        * Retorna: dataTable
        */
        public static DataTable dtDatosEntrada
        {
            get
            {
                object value = HttpContext.Current.Session["datosEntrada"];
                return value == null ? null : (DataTable)value;
            }
            set
            {
                HttpContext.Current.Session["datosEntrada"] = value;
            }
        }

      /*
       * Requiere: N/A
       * Modifica: Declara variable global de modo (tipo de operación en ejecución)
       * Retorna: entero.
       */
        public static int modo
        {
            get
            {
                object value = HttpContext.Current.Session["modo"];
                return value == null ? 0 : (int)value;
            }
            set
            {
                HttpContext.Current.Session["modo"] = value;
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
       * Modifica: Maneja los eventos a ocurrir cada vez que se carga la página
       * Retorna: N/A.
       */
        protected void Page_Load(object sender, EventArgs e)
        {
            llenarEtiquetasDiseno();
            if(!IsPostBack)
            {
                inicializarModo();
                inicializarDTDatosEntrada();
                estadoInicial();
            }
            ocultarErroresDeOperacion();
            EtiqMensajeOperacion.Visible = false;
            llenarGrid();
        }

        /*
         * Requiere: N/A
         * Modifica: Se encarga de pedir la información principal del diseño al cual se asocia el caso de prueba.
         * Retorna: Lista de hileras.
         */
        protected List<string> infoDisenno()
        {

            ControladoraDisenno cd = new ControladoraDisenno();
            ControladoraRequerimiento cr = new ControladoraRequerimiento();

            List<string> tabla = cd.infoDisenno();
            string proposito = tabla[0].ToString();
            string nivel = tabla[1].ToString();
            string tecnica = tabla[2].ToString();
            string ambiente = tabla[3].ToString();
            string procedimiento = tabla[4].ToString();
            string fecha = tabla[5].ToString();
            string criterios = tabla[6].ToString();
            string responsable = tabla[7].ToString();
            string proyecto = tabla[8].ToString();

            int id_diseno = cd.consultarId_Disenno(proposito);
            DataTable dt = cr.consultarRequerimientoEnDiseno(Int32.Parse(proyecto), id_diseno);

            string requerimientos = "";
            foreach (DataRow row in dt.Rows)
            {
                requerimientos = requerimientos + ";" + row["id_requerimiento"].ToString();
            }
            tabla.Add(requerimientos);
            return tabla;
        }

        /*
         * Requiere: N/A
         * Modifica: Se encarga de desplegar la información del diseño asociado, llenando las etiquetas en la parte superior de la pantalla.
         * Retorna: N/A
         */
        protected void llenarEtiquetasDiseno()
        {
            ControladoraRecursosHumanos crh = new ControladoraRecursosHumanos();

            List<string> la_lista = infoDisenno();
            Proposito.Text=la_lista[0];
            if (Convert.ToInt32(la_lista[1])==2)
            {
                Nivel.Text="Unitaria";
            }
            else if (Convert.ToInt32(la_lista[1]) == 3)
            {
                Nivel.Text="Integración";
            }
            else if (Convert.ToInt32(la_lista[1]) == 4)
            {
                Nivel.Text="Sistema";
            }
            else if (Convert.ToInt32(la_lista[1]) == 5)
            {
                Nivel.Text="Aceptación";
            }


            if (Convert.ToInt32(la_lista[2]) == 2)
            {
                Tecnica.Text="Caja Negra";
            }
            else if (Convert.ToInt32(la_lista[2]) == 3)
            {
                Tecnica.Text="Caja Blanca";
            }
            else if (Convert.ToInt32(la_lista[2]) == 4)
            {
                Tecnica.Text = "Exploratoria";
            }
            
            Proyecto.Text=crh.solicitarNombreProyecto(Convert.ToInt32(la_lista[8]));

            string[] esplit = la_lista[9].Split(';');
            string la_hilera = "";
            for (int i = 1; i < esplit.Length ; i++)
            {
                if (i == (esplit.Length)-1)
                {
                    la_hilera += esplit[i];
                }
                else
                {
                    la_hilera += esplit[i] + ",   ";
                }
                
            }
            Requerimientos.Text = la_hilera;
        }

        /*
         * Requiere: N/A
         * Modifica: Establece el estado de la pantalla la primera vez que se carga, habilitando y deshabilitando los controles elegidos.
         * Retorna: N/A
         */
        protected void estadoInicial()
        {
            ocultarErroresDeOperacion();
            botonesInicio();
            deshabilitarCampos();
            limpiarCampos();
        }

        /*
         * Requiere: N/A
         * Modifica: Establece el estado de la pantalla inmediatamente después de efectuar una operación de inserción, modificación o eliminación.
         * Retorna: N/A
         */
        protected void estadoPostOperacion()
        {
            modo = 0;
            deshabilitarCampos();
            habilitarGrid(ref CP);
            BotonCPInsertar.Enabled = true;
            BotonCPModificar.Enabled = true;
            BotonCPEliminar.Enabled = true;
            BotonCPCancelar.Enabled = false;
            BotonCPAceptar.Enabled = false;
        }

        /*
         * Requiere: N/A
         * Modifica: Establece el estado de la pantalla inmediatamente después de presionar el botón "Insertar".
         * Retorna: N/A
         */
        protected void estadoInsertar()
        {
            marcarBoton(ref BotonCPInsertar);
            limpiarCampos();
            habilitarCampos();
            BotonCPAceptar.Enabled = true;
            BotonCPCancelar.Enabled = true;
            BotonCPModificar.Enabled = false;
            BotonCPEliminar.Enabled = false;
            deshabilitarGrid(ref CP);
        }

        /*
         * Requiere: N/A
         * Modifica: Establece el estado de la pantalla inmediatamente después de presionar el botón "Modificar".
         * Retorna: N/A
         */
        protected void estadoModificar()
        {
            marcarBoton(ref BotonCPModificar);
            habilitarCampos();
            BotonCPAceptar.Enabled = true;
            BotonCPCancelar.Enabled = true;
            BotonCPInsertar.Enabled = false;
            BotonCPEliminar.Enabled = false;
            deshabilitarGrid(ref CP);
        }

        /*
         * Requiere: N/A
         * Modifica: Establece el valor por defecto de la variable "modo" a 0.
         * Retorna: N/A
         */
        protected void inicializarModo()
        {
            modo = 0;
        }

        /*
         * Requiere: N/A
         * Modifica: Establece las columnas que poseerá el datatable de "Entrada de Datos".
         * Retorna: N/A
         */
        protected void inicializarDTDatosEntrada()
        {
            dtDatosEntrada = new DataTable();
            dtDatosEntrada.Clear();
            dtDatosEntrada.Columns.Add("Tipo", typeof(String));
            dtDatosEntrada.Columns.Add("Datos", typeof(String));
        }

        /*
         * Requiere: N/A
         * Modifica: Se encarga de ocultar las etiquetas de error y confirmación al cargarse la página.
         * Retorna: N/A
         */
        protected void ocultarErroresDeOperacion()
        {
            EtiqErrorInsertar.Visible  = false;
            EtiqErrorConsultar.Visible = false;
            EtiqErrorModificar.Visible = false;
            EtiqErrorEliminar.Visible  = false;
        }

        /*
         * Requiere: N/A
         * Modifica: Habilita los botones que pueden ser presionados la primera vez que se carga la pantalla.
         * Retorna: N/A
         */
        protected void botonesInicio()
        {
            BotonCPInsertar.Enabled  = true;
            BotonCPModificar.Enabled = false;
            BotonCPEliminar.Enabled  = false;
            BotonCPAceptar.Enabled   = false;
            BotonCPCancelar.Enabled  = false;
        }

        /*
         * Requiere: N/A
         * Modifica: Habilita los controles de la pantalla: Textbox, DropDownList y Grid.
         * Retorna: N/A
         */
        protected void habilitarCampos()
        {
            TextBoxID.Enabled = true;
            TextBoxPropositoCP.Enabled = true;
            TextBoxResultadoCP.Enabled = true;
            TextBoxFlujoCentral.Enabled = true;
            habilitarCamposEntrada();
            habilitarGrid(ref CP);
        }

        /*
         * Requiere: N/A
         * Modifica: Deshabilita los controles de la pantalla: Textbox, DropDownList y Grid.
         * Retorna: N/A
         */
        protected void deshabilitarCampos()
        {
            TextBoxID.Enabled = false;
            TextBoxPropositoCP.Enabled  = false;
            TextBoxResultadoCP.Enabled  = false;
            TextBoxFlujoCentral.Enabled = false;
            deshabilitarCamposEntrada();
          //  deshabilitarGrid(ref CP);
            this.DECP.DataSource = null;
            this.DECP.DataBind();
        }

        /*
         * Requiere: N/A
         * Modifica: Deshabilita los controles de la pantalla correspondientes a "Entrada de Datos": Textbox, DropDownList y Grid.
         * Retorna: N/A
         */
        protected void deshabilitarCamposEntrada()
        {
            TextBoxDatos.Enabled = false;
            TextBoxDescripcion.Enabled = false;
            TipoEntrada.Enabled = false;
            AgregarEntrada.Enabled = false;
            EliminarEntrada.Enabled = false;
            deshabilitarGrid(ref DECP);
        }

       /*
        * Requiere: N/A
        * Modifica: Habilita los controles de la pantalla correspondientes a "Entrada de Datos": Textbox, DropDownList y Grid.
        * Retorna: N/A
        */
        protected void habilitarCamposEntrada()
        {
            TextBoxDatos.Enabled = true;
            TextBoxDescripcion.Enabled = true;
            TipoEntrada.Enabled = true;
            AgregarEntrada.Enabled = true;
            EliminarEntrada.Enabled = true;
            habilitarGrid(ref DECP);
        }

       /*
        * Requiere: N/A
        * Modifica: Se encarga de borrar toda la información de los Textbox y del Grid para que no se desplegue nada.
        * Retorna: N/A
        */
        protected void limpiarCampos()
        {
            TextBoxID.Text = "";
            TextBoxPropositoCP.Text  = "";
            TextBoxResultadoCP.Text  = "";
            TextBoxFlujoCentral.Text = "";
            TextBoxDescripcion.Text = "";
            TextBoxDatos.Text = "";
            dtDatosEntrada.Clear();
            DECP.DataSource = dtDatosEntrada;
            DECP.DataBind();
            
        }

       /*
        * Requiere: N/A
        * Modifica: Se encarga agregar nuevos datos al datatable global de "Entrada de datos" para que pueda llenarse el Grid de "Entrada de Datos".
        * Retorna: N/A
        */
        protected void agregarGridEntradaDatos()
        {
            DataRow row = dtDatosEntrada.NewRow();
            row["Tipo"] = TipoEntrada.SelectedItem.Text.ToString();
            row["Datos"] = TextBoxDatos.Text;
            dtDatosEntrada.Rows.Add(row);
            DECP.DataSource = dtDatosEntrada;
            DECP.DataBind();
        }

       /*
        * Requiere: N/A
        * Modifica: Se encarga de leer la información del datatable global de "Entrada de Datos" para enviarla a la controladora al insertar o modificar.
        * Retorna: hilera.
        */
        protected String datosEntrada()
        {
            String datosEntrada = "";
            String tipo = TipoEntrada.SelectedItem.Text;
            if (tipo == "No Aplica")
            {
                datosEntrada = "N/A";
            }
            else
            {
                int index = 0;
                foreach (DataRow row in dtDatosEntrada.Rows)
                {
                    if (index != 0)
                        datosEntrada += ",";

                    datosEntrada += "[";
                    datosEntrada += row["Tipo"].ToString()[0];
                    if(Regex.IsMatch(row["Datos"].ToString(), @"\d+"))
                    {
                        datosEntrada += "," + row["Datos"].ToString() + "]";
                    }
                    else
                    {
                        datosEntrada += "]";
                        datosEntrada += "\""+ row["Datos"].ToString() + "\"";
                    }
                    index++;
                }
                if(DECP.Rows.Count > 1)
                {
                   datosEntrada = "[" + datosEntrada + "]";
                }

                datosEntrada = datosEntrada + "_" + TextBoxDescripcion.Text;

            }
         
            return datosEntrada;
        }

       /*
        * Requiere: N/A
        * Modifica: Se encarga de llenar el Grid de casos de pruebas pidiendo la lista de casos disponibles a la Controladora de Casos de Prueba.
        * Retorna: N/A
        */
        protected void llenarGrid()        
        {
            DataTable casosPrueba = crearTablaCP();
            DataTable dt = controladoraCasosPrueba.consultarCasosPrueba(1,""); 
            
            Object[] datos = new Object[2];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    datos[0] = dr[0];
                    datos[1] = dr[1];
                    casosPrueba.Rows.Add(datos);
                }
            }
            else
            {
                datos[0] = "-";
                datos[1] = "-";
                casosPrueba.Rows.Add(datos);
            }
            CP.DataSource = casosPrueba;
            CP.DataBind();
        }

       /*
        * Requiere: N/A
        * Modifica: Se encarga de inicializar las columnas del datatable del cual se llenará el Grid de Casos de Prueba.
        * Retorna: dataTable.
        */
        protected DataTable crearTablaCP()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(String));
            dt.Columns.Add("Propósito", typeof(String));
            return dt;
        }

       /*
        * Requiere: N/A
        * Modifica: Se encarga de llenar los datos en los Textbox, correspondientes al caso de prueba consultado desde el Grid.
        * Retorna: N/A
        */
        void llenarDatosCasoPrueba(String id)
        {
            DataTable dt = controladoraCasosPrueba.consultarCasosPrueba(2, id); // Consulta tipo 2, para llenar los campos de un recurso humano
            BotonCPEliminar.Enabled = true;
            BotonCPModificar.Enabled = true;
            try
            {
                TextBoxID.Text = dt.Rows[0].ItemArray[0].ToString();
                TextBoxPropositoCP.Text = dt.Rows[0].ItemArray[1].ToString();            
                TextBoxResultadoCP.Text = dt.Rows[0].ItemArray[3].ToString();
                TextBoxFlujoCentral.Text = dt.Rows[0].ItemArray[4].ToString();
                String datosEntrada = dt.Rows[0].ItemArray[2].ToString();
                //datosEntrada = datosEntrada.Replace("_"," ");
                llenarGridEntradaDatos(transformarDatosEntrada(datosEntrada));
          
            }
            catch
            {
                EtiqErrorConsultar.Visible = true;
            }
            //Response.Write(dt.Rows.Co)

        }

       /*
        * Requiere: Hilera.
        * Modifica: Se encarga de transformar los datos de entrada a un formato desplegable en el Grid de "Entrada de Datos".
        * Retorna: Lista de hileras.
        */
        protected List<String> transformarDatosEntrada(String hilera)
        {
            if (hilera=="N/A"){
                List<String> regresa = new List<String>();
                regresa.Add(hilera);
                return regresa;
            }
            else
            {
                String[] descripcion = hilera.Split(new[] { "_" }, StringSplitOptions.None);
                String[] primeraDivision = descripcion[0].Split(new[] { ",[" }, StringSplitOptions.None);

                for (int i = 0; i < primeraDivision.Length; i++)
                {
                    primeraDivision[i] = primeraDivision[i].Replace("[", "");
                    primeraDivision[i] = primeraDivision[i].Replace("]", "");
                }
                List<String> regresa = new List<String>();
                for (int i = 0; i < primeraDivision.Length; i++)
                {
                    if (primeraDivision[i].Contains("\""))
                    {
                        String[] temp = primeraDivision[i].Split(new[] { "\"" }, StringSplitOptions.None);
                        regresa.Add(temp[0]);
                        regresa.Add(temp[1]);
                    }
                    else
                    {
                        String[] temp = primeraDivision[i].Split(new[] { "," }, StringSplitOptions.None);
                        regresa.Add(temp[0]);
                        regresa.Add(temp[1]);
                    }
                }

                regresa.Add(descripcion[1]);
                return regresa;
            }

            
        }

       /*
        * Requiere: Lista de hileras.
        * Modifica: Se encarga de llenar el Grid de "Entrada de Datos" pidiendolos a la Controladora de Casos de Prueba.
        * Retorna: N/A
        */
        protected void llenarGridEntradaDatos(List<String> lista_datos)
        {
            dtDatosEntrada.Clear();
            DECP.DataSource = dtDatosEntrada;
            DECP.DataBind();
            for (int i = 0; i < lista_datos.Count - 1; i+=2)
            {
                DataRow row = dtDatosEntrada.NewRow();
                if (lista_datos[i] == "V")
                {
                    row["Tipo"] = "Válido";
                }
                else if (lista_datos[i] == "I")
                {
                    row["Tipo"] = "Inválido";
                }
                //row["Tipo"] = lista_datos[i];

                if (Regex.IsMatch(lista_datos[i + 1], @"\d+"))
                {
                    row["Datos"] = lista_datos[i + 1];
                }
                else
                {
                    row["Datos"] = "\"" + lista_datos[i + 1] + "\"";
                }
                
                //row["Datos"] = lista_datos[i + 1];
                dtDatosEntrada.Rows.Add(row);
                DECP.DataSource = dtDatosEntrada;
                DECP.DataBind();
            }
            TextBoxDescripcion.Text = lista_datos[lista_datos.Count - 1];
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

       /*
        * Requiere: GridView.
        * Modifica: Se encarga de habilitar las funciones del Grid pasado por parámetro.
        * Retorna: N/A
        */
        protected void habilitarGrid(ref GridView grid)
        {
            grid.Enabled = true;
            foreach (GridViewRow row in grid.Rows)
            {
                row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grid, "Select$" + row.RowIndex);
                row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                row.Attributes["style"] = "cursor:pointer";

            }
        }

        /*
         * Requiere: Botón.
         * Modifica: Se encarga de marcar el botón pasado por parámetro.
         * Retorna: N/A
         */
        protected void marcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.ColorTranslator.FromHtml("#20bcae");
            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#20bcae");
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
        * Requiere: Evento de dar click al botón "Insertar".
        * Modifica: Se encarga de preparar los controles de la aplicación para efectuar una inserción.
        * Retorna: N/A
        */
        protected void BotonCPInsertar_Click(object sender, EventArgs e)
        {
            modo = 1;
            estadoInsertar();
        }

        /*
         * Requiere: Evento de dar click al botón "Modificar".
         * Modifica: Se encarga de preparar los controles de la aplicación para efectuar una modificación.
         * Retorna: N/A
         */
        protected void BotonCPModificar_Click(object sender, EventArgs e)
        {
            modo = 2;
            idMod = TextBoxID.Text;
            estadoModificar();
        }

        /*
         * Requiere: Evento de dar click al botón "Eliminar".
         * Modifica: Se encarga de preparar los controles de la aplicación para efectuar una eliminación.
         * Retorna: N/A
         */
        protected void BotonCPEliminar_Click(object sender, EventArgs e)
        {
            marcarBoton(ref BotonCPEliminar);
        }

        /*
         * Requiere: Evento de dar click al botón "Acepar" en el modal.
         * Modifica: Se encarga de efectuar la eliminación del caso de prueba elegido y confirmar al usuario si esta fue exitosa o no.
         * Retorna: N/A
         */
        protected void aceptarModalEliminar_Click(object sender, EventArgs e)
        {
            desmarcarBoton(ref BotonCPEliminar);
            int eliminacion = controladoraCasosPrueba.eliminarCasosPrueba(TextBoxID.Text);

            if (eliminacion == 1)
            {
                EtiqMensajeOperacion.Text = "El caso de prueba se ha eliminado correctamente";
                EtiqMensajeOperacion.ForeColor = System.Drawing.Color.DarkSeaGreen;
                EtiqMensajeOperacion.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                estadoPostOperacion();
                llenarGrid();
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
         * Requiere: Evento de dar click al botón "Cancelar" en el modal.
         * Modifica: Se de cancelar la operación en ejecución, retornando la pantalla a como estaba antes de elegir la operación.
         * Retorna: N/A
         */
        protected void cancelarModal_Click(object sender, EventArgs e)
        {
            TextBoxID.BorderColor = System.Drawing.Color.LightGray;
            if ( modo == 1)
            {
                estadoInicial();
                desmarcarBoton(ref BotonCPInsertar);

            }
            else if (modo == 2)
            {
                llenarDatosCasoPrueba(idMod);
                estadoPostOperacion();
                desmarcarBoton(ref BotonCPModificar);
            }
        }

        /*
        * Requiere: Evento de dar click al botón "Cancelar".
        * Modifica: Se encarga de la operación al presionar cancelar.
        * Retorna: N/A
        */
        protected void BotonCPCancelar_Click(object sender, EventArgs e)
        {
        }

        /*
         * Requiere: Evento de dar click al botón "Aceptar".
         * Modifica: Se encarga de efectuar la inserción o modificación llamando a la Controladora de Casos de Prueba, usando los datos ingresados en los campos y confirmando al usuario el resultado de la operación.
         * Retorna: N/A
         */
        protected void BotonCPAceptar_Click(object sender, EventArgs e)
        {
            ControladoraDisenno cd = new ControladoraDisenno();
            Object[] datosNuevos = new Object[7];
            datosNuevos[0] = this.TextBoxID.Text;
            datosNuevos[1] = this.TextBoxPropositoCP.Text;
            datosNuevos[2] = datosEntrada();
            datosNuevos[3] = this.TextBoxResultadoCP.Text;
            datosNuevos[4] = this.TextBoxFlujoCentral.Text;
            String prop = cd.infoDisenno()[0].ToString();
            datosNuevos[5] = cd.consultarId_Disenno(prop); //recordar modificar cuando se tenga el id del diseño
           
            int operacion = -1;
            if(modo == 1)
            {
                datosNuevos[6] = "";
                operacion = controladoraCasosPrueba.insertarCasosPrueba(datosNuevos);
            }
            else if( modo == 2)
            {
                datosNuevos[6] = idMod;
                operacion = controladoraCasosPrueba.modificarCasosPrueba(datosNuevos);
            }
            if (operacion == 1)
            {
                switch (modo)
                {
                    case 1:
                    {
                        EtiqMensajeOperacion.Text = "El caso de prueba ha sido insertado con éxito";
                        desmarcarBoton(ref BotonCPInsertar);
                        break;
                    }
                    case 2:
                    {
                        EtiqMensajeOperacion.Text = "El caso de prueba ha sido modificado con éxito";
                        desmarcarBoton(ref BotonCPModificar);
                        break;
                    }
                }
                EtiqMensajeOperacion.ForeColor = System.Drawing.Color.DarkSeaGreen;
                llenarGrid();
                estadoPostOperacion();
            }
            else
            {
                switch(operacion)
                {   
                    case 2627:
                    {
                        EtiqMensajeOperacion.Text = "Insertó un id de caso de prueba ya existente, debe modificarlo.";
                        TextBoxID.BorderColor = System.Drawing.Color.Red;
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
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
        }

        /*
         * Requiere: Evento de dar click al botón "Agregar" en "Entrada de Datos".
         * Modifica: Se encarga de consolidar una inserción al Grid de "Entrada de Datos".
         * Retorna: N/A
         */
        protected void AgregarEntrada_Click(object sender, EventArgs e)
        {
            agregarGridEntradaDatos();
            TextBoxDatos.Text = "";
        }

        /*
         * Requiere: Evento de dar click al botón "Eliminar" en "Entrada de Datos".
         * Modifica: Se encarga de eliminar al entrada del Grid de "Entrada de Datos" seleccionada.
         * Retorna: N/A
         */
        protected void EliminarEntrada_Click(object sender, EventArgs e)
        {
            for (int i = dtDatosEntrada.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtDatosEntrada.Rows[i];
                if (i == DECP.SelectedIndex)
                {
                    dr.Delete();
                }
                DECP.DataSource = dtDatosEntrada;
                DECP.DataBind();
            }
            EliminarEntrada.Enabled = false;
        }

        /*
         * Requiere: Evento de seleccionar un Caso de Prueba.
         * Modifica: Se encarga de controlar qué fila está seleccionada en el Grid de Casos de Prueba para realizar el llenado de campos correspondiente.
         * Retorna: N/A
         */
        protected void CP_SelectedIndexChanged(object sender, EventArgs e)
        {
            String id = CP.SelectedRow.Cells[0].Text;
            llenarDatosCasoPrueba(id);
        }

       /*
        * Requiere: El evento de enlazar información de un datatable con el Grid.
        * Modifica: Establece el comportamiento del Grid ante los diferentes eventos.
        * Retorna: N/A.
        */
        protected void OnCPRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(CP, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

            }
        }

        /*
         * Requiere: Evento de pasar de página en el Grid.
         * Modifica: Pasa de página y llena el Grid con las n tuplas que siguen, siendo n el tamaño de la página.
         * Retorna: N/A. 
        */
        protected void OnCPPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CP.PageIndex = e.NewPageIndex;
            this.llenarGrid();
        }

       /*
        * Requiere: Evento de seleccionar fila en el Grid de "Entrada de Datos".
        * Modifica: Resalta fila seleccionada.
        * Retorna: N/A. 
        */
        protected void DECP_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string tipo = DECP.SelectedRow.Cells[0].Text;
                tipo = tipo.Replace("&#225;", "á");
                string dato = DECP.SelectedRow.Cells[1].Text;
                dato = dato.Replace("&quot;","");

                TextBoxDatos.Text = dato;
                if (tipo == "Válido")
                {
                    TipoEntrada.SelectedIndex = 0;
                }
                else if (tipo == "Inválido")
                {
                    TipoEntrada.SelectedIndex = 1;
                }
                else if (tipo == "No Aplica") {
                    TipoEntrada.SelectedIndex = 2;
                }
            }
            catch
            {
                EtiqErrorConsultar.Visible = true;
            }
        }

       /*
        * Requiere: El evento de enlazar información de un datatable con el Grid de "Entrada de Datos".
        * Modifica: Establece el comportamiento del Grid ante los diferentes eventos.
        * Retorna: N/A.
        */
        protected void OnDECPRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='white'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(DECP, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

            }
        }

       /*
        * Requiere: Evento de pasar de página en el Grid.
        * Modifica: Pasa de página y llena el Grid con las n tuplas que siguen, siendo n el tamaño de la página.
        * Retorna: N/A. 
        */
        protected void OnDECPPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DECP.PageIndex = e.NewPageIndex;
            DECP.DataSource = dtDatosEntrada;
            DECP.DataBind();
        }

      /*
       * Requiere: Evento de cambiar de opción en el DropDown de "Entrada de Datos".
       * Modifica: Habilita y deshabilita el botón "Agregar" de "Entrada de Datos".
       * Retorna: N/A. 
      */
        protected void TipoEntrada_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(TipoEntrada.SelectedItem.Text == "No Aplica")
            {
                AgregarEntrada.Enabled = false;

            }
            else
            {
                AgregarEntrada.Enabled = true;
                EliminarEntrada.Enabled = true;

            }
        }

        /*
        * Requiere: Evento de dar click al botón de "Regresar a Diseño".
        * Modifica: Redirige al usuario a la página de Diseño de Prueba.
        * Retorna: N/A. 
        */
        protected void regresarADiseno(object sender, EventArgs e) {
            Response.Redirect("~/Intefaces/InterfazDiseno.aspx");
        }
        
    }
} 