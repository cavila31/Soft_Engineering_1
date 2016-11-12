using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SistemaPruebas.Intefaces
{
    public partial class InterfazProyecto : System.Web.UI.Page
    {

        //Variables:
        public static string button
        {
            get
            {
                object value = HttpContext.Current.Session["button"];
                return value == null ? "0" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["button"] = value;
            }
        }

        public static string id_Proyecto
        {
            get
            {
                object value = HttpContext.Current.Session["id_Proyecto"];
                return value == null ? "-1" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["id_Proyecto"] = value;
            }
        }

        public static string adm
        {
            get
            {
                object value = HttpContext.Current.Session["adm"];
                return value == null ? "false" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["adm"] = value;
            }
        }
        public static string modificando
        {
            get
            {
                object value = HttpContext.Current.Session["modificando"];
                return value == null ? "false" : (string)value;
            }
            set
            {
                HttpContext.Current.Session["modificando"] = value;
            }
        }

        public static string id_modificando
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

        Controladoras.ControladoraProyecto controladoraProyecto = new Controladoras.ControladoraProyecto();

        //Métodos:

        protected void Page_Load(object sender, EventArgs e)
        {
            
            adm = controladoraProyecto.PerfilDelLogeado().ToString();
            Restricciones_Campos();
            Deshabilitar_Campos();
            if (!Convert.ToBoolean(adm))
                Insertar.Enabled = false;
            aceptar.Enabled = false;
            cancelar.Enabled = false;
            Modificar.Enabled = false;
            Eliminar.Enabled = false;

            llenarGrid();
            EtiqErrorLlaves.Visible = false;
            txt_date.Attributes.Add("readonly", "readonly");
        }

        //Restricciones_Campos()
        //Requiere: N/A
        //Modifica: delimita algunas características de los componentes a mostrarse en la interfaz
        //Retorna: N/A
        protected void Restricciones_Campos()
        {

            nombre_proyecto.MaxLength = 20;
            obj_general.MaxLength = 50;
            nombre_rep.MaxLength = 30;
            tel_rep.MaxLength = 8;
            tel_rep2.MaxLength = 8;
            of_rep.MaxLength = 17;
            obj_general.Rows = 5;
            //gridProyecto.Columns[0].css = "Display:none";

        }

        //        Habilitar_Campos(); 
        //Requiere: El usuario ha presionado el botón de Insertar dentro de la pestaña de Proyecto
        //Modifica: Habilita  y despliega los campos de Nombre del Sistema, Objetivo General, Fecha de asignación, estado, Nombre de Oficina Usuaria, Teléfonos Oficina Usuaria, Representante, Líder de proyecto, botones de: asignar recurso, des-asignar recurso, aceptar, cancelar.
        //Retorna: N/A.

        protected void Habilitar_Campos()
        {
            nombre_proyecto.Enabled = true;
            obj_general.Enabled = true;
            estado.Enabled = true;
            nombre_rep.Enabled = true;
            tel_rep.Enabled = true;
            tel_rep2.Enabled = true;
            of_rep.Enabled = true;
            LiderProyecto.Enabled = true;
            txt_date.Enabled = true;

        }

        //Deshabilitar_Campos():
        //Requiere: El usuario haya presionado el botón aceptar o cancelar, dentro de una inserción, modificación, consulta o eliminación de un proyecto.
        //Modifica: Deshabilitan los campos de: Nombre_sistema, Objetivo_general, Fecha_asignación, estado, Nombre_representante, oficina_representante y telefono_representante.
        //Retorna:   N/A.
        protected void Deshabilitar_Campos()
        {
            nombre_proyecto.Enabled = false;
            obj_general.Enabled = false;
            estado.Enabled = false;
            nombre_rep.Enabled = false;
            tel_rep.Enabled = false;
            tel_rep2.Enabled = false;
            of_rep.Enabled = false;
            LiderProyecto.Enabled = false;
            txt_date.Enabled = false;

        }

        //Limpiar_campos():
        //Requiere: El usuario haya presionado el botón cancelar, dentro de una inserción, modificación, consulta o eliminación de un proyecto.
        //Modifica: Limpian todos los campos de: Nombre_sistema, Objetivo_general, Fecha_asignación, estado, Nombre_representante, oficina_representante y telefono_representante.
        //Retorna:   N/A.
        protected void Limpiar_Campos()
        {
            nombre_proyecto.Text = "";
            obj_general.Text = "";
            nombre_rep.Text = "";
            tel_rep.Text = "";
            tel_rep2.Text = "";
            of_rep.Text = "";
            llenarGrid();
            gridProyecto.Enabled = true;
            if (Convert.ToBoolean(adm))
                Insertar.Enabled = true;
            //cancelar.Enabled = false;
            //Modificar.Enabled = false;
            estado.ClearSelection();
            ListItem selectedListItems = estado.Items.FindByValue("1");
            LiderProyecto.Items.Clear();
            txt_date.Text = "";
        }

        //Insertar_button():
        //Requiere: El usuario haya presionado el botón de insertar un proyecto
        //Modifica: Prepara el proceso para insertar un proyecto, habilita y limpia los campos de: Nombre_sistema, Objetivo_general, Fecha_asignación, estado, Nombre_representante, oficina_representante y telefono_representante.Además habilita el botón aceptar y cancelar y deshabilita el botón de modificar, eliminar, y el grid de consultas.
        //Retorna:   N/A.

        protected void Insertar_button(object sender, EventArgs e)
        {
            marcarBoton(ref Insertar);
            button = "1";
            Limpiar_Campos();
            aceptar.Enabled = true;
            cancelar.Enabled = true;
            LiderProyecto.ClearSelection();
            cargarComboRH();
            gridProyecto.Enabled = false;
            Habilitar_Campos();
            UnenabledButtons();
            deshabilitarGrid();

        }

        //aceptar_Click():
        //Requiere: El usuario haya presionado el botón de aceptar dentro de proyecto.
        //Modifica: Dependiendo de si se quiere aceptar una inserción, una modificación o una eliminación; se procederá de distinta forma.Al aceptar un insertar, se verifica que los campos se hayan llenado correctamente, e invoca a la controladora Proyecto para que realice la inserción de la nueva información dentro de la base de datos.Al modificar, similar al proceso de insertar se envían los datos a la controladora Proyecto, pero para que actualice la información del proyecto. Al aceptar una eliminación, depende de si es un administrador o un Miembro de equipo, el administrador elimina la tupla del sistema, mientras que el miembro de equipo sigue un proceso para declarar como cancelado el proyecto “eliminado.”
        //Retorna:   N/A.
        protected void aceptar_Click(object sender, EventArgs e)
        {

            llenarGrid();
            habilitarGrid();
            switch (Int32.Parse(button))
            {
                case 1://Insertar
                    {
                        if (nombre_proyecto.Text != "" && obj_general.Text != "" && nombre_rep.Text != "" && tel_rep.Text != "" && of_rep.Text != "" && Convert.ToBoolean(adm))
                        {
                            desmarcarBoton(ref Insertar);
                            Console.WriteLine("Insertar");
                            string text = txt_date.Text;
                            string telefonos = "";
                            if (tel_rep2.Text != "")
                                 telefonos = tel_rep.Text+","+tel_rep2.Text;
                            else
                                 telefonos = tel_rep.Text;
                            object[] datos = new object[9] { 0, nombre_proyecto.Text, obj_general.Text, text, estado.SelectedValue, nombre_rep.Text, telefonos, of_rep.Text, LiderProyecto.SelectedValue};
                            int a = controladoraProyecto.IngresaProyecto(datos);
                            if (a == 1)
                            {
                                id_Proyecto = controladoraProyecto.ConsultarIdProyectoPorNombre(nombre_proyecto.Text).ToString();
                                EtiqErrorLlaves.Text = "El proyecto ha sido insertado con éxito";
                                EtiqErrorLlaves.ForeColor = System.Drawing.Color.DarkSeaGreen;
                                EtiqErrorLlaves.Visible = true;
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                                //((Label)this.Master.FindControl("MensajesGenerales")).Text = "El proyecto ha sido insertado con éxito";
                                //((Label)this.Master.FindControl("MensajesGenerales")).ForeColor = System.Drawing.Color.DarkSeaGreen;
                                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El proyecto ha sido insertado con éxito');", true);
                                llenarGrid();
                                Deshabilitar_Campos();
                                gridProyecto.Enabled = true;
                                EnabledButtons();
                            }

                            else
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Ha ocurrido un problema, el proyecto no fue insertado');", true);
                                switch (a)
                                {
                                    case 2627:
                                        EtiqErrorLlaves.Text = "El nombre del sistema ya ha sido ingresado anteriormente, por favor ingrese un nombre nuevo";
                                        nombre_proyecto.BorderColor = System.Drawing.Color.Red;

                                        break;
                                    case 547:
                                        EtiqErrorLlaves.Text = "El nombre del sistema sólo puede tener letras, no acepta números ni caracteres especiales";
                                        nombre_proyecto.BorderColor = System.Drawing.Color.Red;
                                        break;
                                    default:
                                        EtiqErrorLlaves.Text = "Se produjo un error al momento de insertar el proyecto, por favor intente luego";
                                        break;

                                };
                                Habilitar_Campos();
                                button = "1";
                                aceptar.Enabled = true;
                                cancelar.Enabled = true;
                                marcarBoton(ref Insertar);
                                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                                EtiqErrorLlaves.Visible = true;
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);

                            }
                        }
                    }
                    break;
                case 2:
                    {
                        desmarcarBoton(ref Modificar);
                        Console.WriteLine("Modificar");
                        string text = txt_date.Text;
                        string telefonos = "";
                        if (tel_rep2.Text != "")
                            telefonos = tel_rep.Text + "," + tel_rep2.Text;
                        else
                            telefonos = tel_rep.Text;
                        object[] datos = new object[9] { id_modificando, nombre_proyecto.Text, obj_general.Text, text, estado.SelectedValue, nombre_rep.Text, telefonos, of_rep.Text, LiderProyecto.SelectedValue};
                        int a = controladoraProyecto.ActualizarProyecto(datos);
                        if (a == 1)
                        {
                            EtiqErrorLlaves.Text = "El proyecto ha sido modificado con éxito";
                            EtiqErrorLlaves.ForeColor = System.Drawing.Color.DarkSeaGreen;
                            EtiqErrorLlaves.Visible = true;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                            id_Proyecto = controladoraProyecto.ConsultarIdProyectoPorNombre(nombre_proyecto.Text).ToString();
                            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El proyecto ha sido modificado con éxito');", true);
                            llenarGrid();
                            Deshabilitar_Campos();
                            gridProyecto.Enabled = true;
                            EnabledButtons();
                            controladoraProyecto.UpdateUsoProyecto(Int32.Parse(id_Proyecto), 0);
                            controladoraProyecto.QuitarEliminacion(Int32.Parse(id_modificando));
                            int i = -1;
                            id_modificando = i.ToString();
                            modificando = false.ToString();
                        }

                        else
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Ha ocurrido un problema, el proyecto no fue insertado');", true);
                            switch (a)
                            {
                                case 2601:
                                    {
                                        EtiqErrorLlaves.Text = "El nombre del sistema ya ha sido ingresado anteriormente, por favor ingrese un nombre nuevo";
                                        nombre_proyecto.BorderColor = System.Drawing.Color.Red;
                                    }
                                    break;
                                case 547:
                                    {
                                        EtiqErrorLlaves.Text = "El nombre del sistema sólo puede tener letras, no acepta números ni caracteres especiales";
                                        nombre_proyecto.BorderColor = System.Drawing.Color.Red;
                                    } break;
                                default:
                                    {
                                        EtiqErrorLlaves.Text = "Se produjo un error al momento de insertar el proyecto, por favor intente luego";
                                    } break;

                            };                           
                            Habilitar_Campos();
                            button = "2";
                            aceptar.Enabled = true;
                            cancelar.Enabled = true;
                            llenarGrid();
                            EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                            EtiqErrorLlaves.Visible = true;
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                        }
                    }
                    break;
                case 3:
                    {
                        //desmarcarBoton(ref Eliminar);
                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "confirm('¿Está seguro que desea eliminar?');", true);
                        //Console.WriteLine("Eliminar");
                        //int a = 0;
                        //if (Convert.ToBoolean(adm))
                        //{
                        //    a = controladoraProyecto.EliminarProyecto(id_modificando.ToString());
                        //}
                        //else
                        //{
                        //    a = controladoraProyecto.CancelarProyecto(id_modificando.ToString());
                        //}
                        //if (a == 1)
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El proyecto ha sido insertado con éxito');", true);
                        //}

                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Ha ocurrido un problema, el proyecto no fue insertado');", true);
                        //}

                        //Limpiar_Campos();
                        //controladoraProyecto.QuitarEliminacion(Int32.Parse(id_modificando));
                        //int i = -1;
                        //id_modificando = i.ToString();
                        //modificando = false.ToString();
                    }
                    break;

            };
        }

        protected void no_cancelar_Click(object sender, EventArgs e)
        {
            Habilitar_Campos();
            cancelar.Enabled = true;
            aceptar.Enabled = true;

        }

        protected void cancelar_Click(object sender, EventArgs e)
        {
            nombre_proyecto.BorderColor = System.Drawing.Color.LightGray;
            switch (Int32.Parse(button))
            {
                case 1://Insertar
                    {
                        EtiqErrorLlaves.Visible = false;
                    }
                    break;
                case 2:
                    {
                        controladoraProyecto.QuitarEliminacion(Int32.Parse(id_modificando));
                        int i = -1;
                        id_modificando = i.ToString();
                        modificando = false.ToString();
                        controladoraProyecto.UpdateUsoProyecto(Int32.Parse(id_Proyecto), 0);
                    }
                    break;
                case 3:
                    {
                        controladoraProyecto.QuitarEliminacion(Int32.Parse(id_modificando));
                        int i = -1;
                        id_modificando = i.ToString();
                        modificando = false.ToString();
                        controladoraProyecto.UpdateUsoProyecto(int.Parse(id_Proyecto), 0);
                    }
                    break;
            }
            Limpiar_Campos();
            Deshabilitar_Campos();
            aceptar.Enabled = false;
            cancelar.Enabled = false;
            gridProyecto.Enabled = true;
            desmarcarBoton(ref Insertar);
            desmarcarBoton(ref Modificar);
            desmarcarBoton(ref Eliminar);
            habilitarGrid();
        }

        //Requiere: N/A
        //Modifica: Carga el grid consultas de proyectos, con los datos del Nombre del sistema y el lider del proyecto.
        //retorna N/A
        protected void llenarGrid()
        {
            gridProyecto.Columns[0].Visible = true;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("conteo"), new DataColumn("Id Proyecto"), new DataColumn("Nombre del Sistema"), new DataColumn("Lider del Proyecto")});
            DataTable proyecto = controladoraProyecto.ConsultarProyectoIdNombre();
            Object[] datos = new Object[5];
            if (proyecto.Rows.Count > 0)
            {
                foreach (DataRow fila in proyecto.Rows)
                {
                    if (fila[2].ToString() != "dummy")
                        dt.Rows.Add(fila[0].ToString(), fila[0].ToString(), fila[1].ToString(), fila[2].ToString());
                    else
                        dt.Rows.Add(fila[0].ToString(), fila[0].ToString(), fila[1].ToString(), "");
                }
            }
            else
            {

                dt.Rows.Add("-", "-", "*");

            }
            this.gridProyecto.DataSource = dt;
            this.gridProyecto.DataBind();
            gridProyecto.Columns[0].Visible = false;
        }


        //Requiere: N/A
        //Modifica: Carga los datos en interfaz, del proyecto recién consultado.
        //retorna N/A
        public void Llenar_Datos_Conultados(int idProyecto)
        {
            Controladoras.EntidadProyecto entidadP = controladoraProyecto.ConsultarProyecto(idProyecto);
            this.nombre_proyecto.Text = entidadP.Nombre_sistema;
            this.obj_general.Text = entidadP.Objetivo_general;
            txt_date.Text = entidadP.Fecha_asignacion;
            estado.ClearSelection();
            ListItem selectedListItem = estado.Items.FindByValue(entidadP.Estado);
            if (selectedListItem != null)
            {
                selectedListItem.Selected = true;
            };
            this.nombre_rep.Text = entidadP.Nombre_representante;
            if (entidadP.Telefono_representante.ToString().Contains(","))
            {
                string[] tels = entidadP.Telefono_representante.ToString().Split(',');
                tel_rep.Text = tels[0];
                tel_rep2.Text = tels[1];
            }
            else
                this.tel_rep.Text = entidadP.Telefono_representante;
            this.of_rep.Text = entidadP.Oficina_representante;
            this.LiderProyecto.Items.Clear();
            if (entidadP.LiderProyecto != "" && !entidadP.LiderProyecto.Contains("dummy"))
                this.LiderProyecto.Items.Add(entidadP.LiderProyecto);
        }

        //protected void gridProyecto_SelectedIndexChanged(object sender, GridViewCommandEventArgs e)
        //{
        //    if (controladoraProyecto.ConsultarUsoProyecto(id_Proyecto) == 0)
        //    {
        //        Modificar.Enabled = true;
        //        Eliminar.Enabled = true;
        //        Insertar.Enabled = false;
        //        GridViewRow filaSeleccionada = this.gridProyecto.Rows[Convert.ToInt32(e.CommandArgument)];
        //        id_Proyecto = Convert.ToInt32(filaSeleccionada.Cells[0].Text);
        //        Llenar_Datos_Conultados(id_Proyecto);
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El proyecto consultado se encuentra actualmente en uso, se muestra solo con fines de lectura');", true);
        //        GridViewRow filaSeleccionada = this.gridProyecto.Rows[Convert.ToInt32(e.CommandArgument)];
        //        id_Proyecto = Convert.ToInt32(filaSeleccionada.Cells[0].Text);
        //        Llenar_Datos_Conultados(id_Proyecto);
        //        cancelar.Enabled = true;                
        //    }
        //}


        //Requiere: Hacer click en el botón de modificar.
        //Modifica: Se habilitan los campos para poder modificar un proyecto previamente consultado.
        //retorna N/A
        protected void Modificar_Click(object sender, EventArgs e)
        {
            deshabilitarGrid();
            if (controladoraProyecto.ConsultarUsoProyecto(Int32.Parse(id_Proyecto)) == 0)
            {
                marcarBoton(ref Modificar);
                //controladoraProyecto.UpdateUsoProyecto(Int32.Parse(id_Proyecto), 1);
                button = "2";
                Habilitar_Campos();
                UnenabledButtons();
                cargarComboRH();
                gridProyecto.Enabled = false;
                aceptar.Enabled = true;
                cancelar.Enabled = true;
                modificando = true.ToString();
                id_modificando = id_Proyecto;
                controladoraProyecto.AgregarModificacion(Int32.Parse(id_modificando));

            }
            else
            {
                EtiqErrorLlaves.Text = "El proyecto consultado se encuentra actualmente en uso";
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                EtiqErrorLlaves.Visible = true;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El proyecto consultado se encuentra actualmente en uso');", true);

            }

        }


        //Requiere: Hacer click en el botón de eliminar
        //Modifica: Se elimina un proyecto recién consultado.
        //retorna N/A
        protected void Eliminar_Click(object sender, EventArgs e)
        {
            //textModal.Text = "¿Desea Eliminar este proyecto?";
            deshabilitarGrid();
            marcarBoton(ref Eliminar);
            button = "3";
            UnenabledButtons();
            //aceptar.Enabled = true;
            //cancelar.Enabled = true;
            modificando = true.ToString();
            if (controladoraProyecto.ConsultarUsoProyecto(Int32.Parse(id_Proyecto)) == 0)
            {
                id_modificando = id_Proyecto;
                controladoraProyecto.AgregarModificacion(Int32.Parse(id_modificando));
            }
            else
            {
                EtiqErrorLlaves.Text = "El proyecto consultado se encuentra actualmente en uso, se muestra sólo con fines de lectura";
                EtiqErrorLlaves.Visible = true;
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('El proyecto consultado se encuentra actualmente en uso');", true);
            }
        }

        //Requiere: Dar click a una fila del grid de consultas de proyecto.
        //Modifica: Se solicitan los datos de un proyecto seleccionado del click de consultas.
        //retorna N/A
        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Accessing BoundField Column
           
                //modificando = false.ToString();
                int i = -1;
                id_modificando = i.ToString();
                controladoraProyecto.UpdateUsoProyecto(Int32.Parse(id_modificando), 0);
            
            try
            {
                id_Proyecto = gridProyecto.SelectedRow.Cells[0].Text;
                if (controladoraProyecto.ConsultarUsoProyecto(Int32.Parse(id_Proyecto)) == 0 && !Convert.ToBoolean(modificando))
                {
                    
                    Modificar.Enabled = true;
                    Eliminar.Enabled = true;
                    aceptar.Enabled = true;
                }
                else
                {
                    EtiqErrorLlaves.Text = "";
                    if (Convert.ToBoolean(modificando))
                    {
                        EtiqErrorLlaves.Text = "Actualmente usted se encuentra modificando/elimiando otro proyecto";

                        //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('');", true);
                    }

                    if (controladoraProyecto.ConsultarUsoProyecto(Int32.Parse(id_Proyecto)) == 1)
                    {// ScriptManager.RegisterStartpScript(this.Page, this.Page.GetType(), "err_msg", "alert('');", true);                        
                        if (EtiqErrorLlaves.Text != "")
                            EtiqErrorLlaves.Text += "<br/><br/>";
                        EtiqErrorLlaves.Text += "El proyecto consultado se encuentra actualmente en uso, se muestra sólo con fines de lectura";
                    }
                    EtiqErrorLlaves.Visible = true;
                    EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
                Limpiar_Campos();
                LiderProyecto.ClearSelection();
                Llenar_Datos_Conultados(Int32.Parse(id_Proyecto));
                cancelar.Enabled = true;
                if (!Convert.ToBoolean(adm))
                    Insertar.Enabled = false;
            }
            catch (Exception ex)
            {
                EtiqErrorLlaves.Text = ex.ToString();
                EtiqErrorLlaves.Visible = true;
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                Console.WriteLine(ex);
            }

        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProyecto.PageIndex = e.NewPageIndex;
            this.llenarGrid();
        }
        protected void EnabledButtons()
        {
            Modificar.Enabled = true;
            Eliminar.Enabled = true;
            if (Convert.ToBoolean(adm))
                Insertar.Enabled = true;
        }
        protected void UnenabledButtons()
        {
            Modificar.Enabled = false;
            Eliminar.Enabled = false;
            if (Convert.ToBoolean(adm))
                Insertar.Enabled = false;
        }

        protected void marcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.ColorTranslator.FromHtml("#20bcae");
            b.BackColor = System.Drawing.ColorTranslator.FromHtml("#20bcae");
            b.ForeColor = System.Drawing.Color.White;
        }

        protected void desmarcarBoton(ref Button b)
        {
            b.BorderColor = System.Drawing.Color.LightGray;
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = System.Drawing.Color.Black;

        }

        protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (gridProyecto.Enabled && e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='black'";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridProyecto, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";

            }
        }
        protected void deshabilitarGrid()
        {
            gridProyecto.Enabled = false;
            foreach (GridViewRow row in gridProyecto.Rows)
            {
                row.Attributes.Remove("onclick");
                row.Attributes.Remove("onmouseover");
                row.Attributes.Remove("style");
                row.Attributes.Remove("onmouseout");
            }
        }

        protected void habilitarGrid()
        {
            gridProyecto.Enabled = true;
            foreach (GridViewRow row in gridProyecto.Rows)
            {
                row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridProyecto, "Select$" + row.RowIndex);
                row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.background='#D3F3EB';;this.style.color='white'";
                row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.background='white';this.style.color='#84878e'";

                row.Attributes["style"] = "cursor:pointer";
            }
        }

        protected void nombre_proyecto_TextChanged(object sender, EventArgs e)
        {
            nombre_proyecto.BorderColor = System.Drawing.Color.LightGray;
        }

        protected void aceptarModal_Click(object sender, EventArgs e)
        {


            desmarcarBoton(ref Eliminar);

            Console.WriteLine("Eliminar");
            int a = 0;
            if (Convert.ToBoolean(adm))
            {
                a = controladoraProyecto.EliminarProyecto(id_modificando.ToString());
            }
            else
            {
                a = controladoraProyecto.CancelarProyecto(id_modificando.ToString());
            }
            if (a == 1)
            {
                EtiqErrorLlaves.Text = "El proyecto se ha eliminado correctamente";
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.DarkSeaGreen;
                EtiqErrorLlaves.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
            }

            else
            {
                EtiqErrorLlaves.Text = "El proyecto no pudo ser eliminado, ocurrió un error";
                EtiqErrorLlaves.ForeColor = System.Drawing.Color.Salmon;
                EtiqErrorLlaves.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "HideLabel();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Ha ocurrido un problema, el proyecto no fue insertado');", true);
            }

            Limpiar_Campos();
            controladoraProyecto.QuitarEliminacion(Int32.Parse(id_modificando));
            int i = -1;
            id_modificando = i.ToString();
            modificando = false.ToString();
        }

        protected void cancelarModal_Click(object sender, EventArgs e)
        {
            //controladoraProyecto.QuitarEliminacion(Int32.Parse(id_modificando));
            //int i = -1;
            //id_modificando = i.ToString();
            //modificando = false.ToString();
            //controladoraProyecto.UpdateUsoProyecto(int.Parse(id_Proyecto), 0);
            controladoraProyecto.QuitarEliminacion(Int32.Parse(id_modificando));
            int i = -1;
            id_modificando = i.ToString();
            modificando = false.ToString();
            if (button != "2" && button != "3")
                Limpiar_Campos();
            Deshabilitar_Campos();
            aceptar.Enabled = false;
            cancelar.Enabled = false;
            gridProyecto.Enabled = true;
            desmarcarBoton(ref Insertar);
            desmarcarBoton(ref Modificar);
            desmarcarBoton(ref Eliminar);
            habilitarGrid();
        }

        protected void cargarComboRH()
        {
            string seleccionado = LiderProyecto.SelectedValue;
            LiderProyecto.Items.Clear();
            if (!String.IsNullOrWhiteSpace(seleccionado) && !seleccionado.Contains("dummy"))               
             LiderProyecto.Items.Add(seleccionado);
            string nombres = controladoraProyecto.solicitarNombreRecursoSinProyecto();
            if (!String.IsNullOrEmpty(nombres))
            {
                string[] nombre = nombres.Split(';');                
                foreach (string n in nombre)
                {
                    if(!String.IsNullOrWhiteSpace(n) && !n.Contains("dummy"))
                        LiderProyecto.Items.Add(n);
                }
                
            }
            LiderProyecto.Items.Add("");
        }
    }

}
