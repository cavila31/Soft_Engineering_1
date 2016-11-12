using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using SistemaPruebas.Controladoras;


namespace SistemaPruebas.Controladoras
{
    public class ControladoraReportes
    {
        ControladoraProyecto controlProy;
        ControladoraDisenno controlDis;
        ControladoraCasosPrueba controlCasos;
        ControladoraEjecucionPrueba controlEjec;
        ControladoraRecursosHumanos controlRH;
        ControladoraRequerimiento controlReq;
        /*
         * Requiere: N/A
         * Modifica: Inicializa las controladoras.
         * Retorna: N/A.
         */
        public ControladoraReportes()
        {
            controlProy = new ControladoraProyecto();
            controlDis = new ControladoraDisenno();
            controlCasos = new ControladoraCasosPrueba();
            controlEjec = new ControladoraEjecucionPrueba();
            controlRH = new ControladoraRecursosHumanos();
            controlReq = new ControladoraRequerimiento();
        }
        /*
         * Requiere: N/A
         * Modifica: N/A.
         * Retorna: El tipo de perfil del loggeado.
         */
        public string PerfilDelLogeado()
        {
            return controlRH.perfilDelLoggeado();
        }
        /*
         * Requiere: N/A
         * Modifica: N/A.
         * Retorna: El id de los proyectos del loggeado.
         */
        public int proyectosDelLoggeado()
        {
            return controlRH.proyectosDelLoggeado();
        }
        /*
         * Requiere: N/A
         * Modifica: N/A.
         * Retorna: Un DataTable con los proyectos del loggeado.
         */
        public DataTable consultarProyecto()
        {
            return controlProy.ConsultarProyectoIdNombre();
        }

        /*
         * Requiere: String del nombre del proyecto
         * Modifica: N/A.
         * Retorna: Un String con los miembros del proyecto seleccionado.
         */
        public String consultarMiembrosProyecto(String nombProy)
        {
            DataTable dt = controlRH.consultarMiembrosProyecto(nombProy);
            String miembros = "";
            foreach (DataRow dr in dt.Rows)
            {
                miembros = miembros + "\n " + dr[0] + ",\n ";
                // dtGrid.Rows.Add(datos);
            }
            return miembros;
        }
        /*
         * Requiere: String nombreProyecto
         * Modifica: N/A.
         * Retorna: Entidad proyecto.
         */
        public EntidadProyecto consultarProyecto(string nombre)
        {
            return controlProy.ConsultarProyecto(controlProy.ConsultarIdProyectoPorNombre(nombre));
        }

        /*
         * Requiere: String nombre del requerimiento
         * Modifica: N/A.
         * Retorna: String de casos de prueba.
         */
        public string consultarCasosPrueba(string nombre)
        {
            return controlCasos.solicitarCasosdePrueba(controlDis.consultarId_Disenno(nombre));
        }


        /*
         * Requiere: String nombreProyecto
         * Modifica: N/A.
         * Retorna: DataTable con los modulos asociados a ese proyecto.
         */
        public DataTable consultarModulos(string nombre)
        {
            return controlReq.consultarModulos(controlProy.ConsultarIdProyectoPorNombre(nombre).ToString());
        }
        /*
         * Requiere: String nombreProyecto y nombre del modulo
         * Modifica: N/A.
         * Retorna: DataTable con los requerimientos asociados a ese modulo.
         */
        public DataTable consultarRequerimientos(string nombre, string modulo)
        {
            return controlReq.consultarReqPorNombre(modulo, controlProy.ConsultarIdProyectoPorNombre(nombre).ToString());
        }


        /*
         * Requiere: String nombreProyecto y nombre del modulo
         * Modifica: N/A.
         * Retorna: List<object> con los datos del proyecto solicitado.
         */
        public List<object> reporteProyecto(EntidadProyecto entidad)
        {

            List<Object> retorno = new List<object>();

            retorno.Add(entidad.Nombre_sistema);
            retorno.Add(entidad.Objetivo_general);
            retorno.Add(entidad.Fecha_asignacion);
            string estado = "";
            if (entidad.Estado != "")
            {
                switch (Int32.Parse(entidad.Estado))
                {

                    case 1:
                        {
                            estado = "Pendiente";
                        }
                        break;
                    case 2:
                        {
                            estado = "Asignado";
                        }
                        break;
                    case 3:
                        {
                            estado = "En ejecución";
                        }
                        break;
                    case 4:
                        {
                            estado = "Finalizado";
                        }
                        break;
                    case 5:
                        {
                            estado = "Cerrado";
                        }
                        break;
                }
            }
            retorno.Add(estado);
            retorno.Add(entidad.Oficina_representante);
            retorno.Add(entidad.Telefono_representante);
            retorno.Add(entidad.Nombre_representante);
            retorno.Add(entidad.LiderProyecto);
            retorno.Add(controlRH.solicitarNombreRecursosPorProyecto(Int32.Parse(entidad.Id_proyecto)));

            return retorno;
        }


        /*
         * Requiere: String nombreProyecto y nombre del modulo
         * Modifica: N/A.
         * Retorna: List<object> con los datos de las mediciones del requerimiento solicitado.
         */
        public List<Object> medicionRequerimiento(List<Object> retorno, string idReq)
        {
            int exitosCant = 0;
            int sinEnvaluarCant = 0;
            List<string> idCasosExitosos = new List<string>();
            List<string> idCasosSinEvaluar = new List<string>();
            Dictionary<string, int> noConformidad = new Dictionary<string, int>();
            string[] casosPrueba = controlCasos.consultarCasoPorRequerimiento(idReq);
            retorno.Add(idReq);
            try
            {
                if (casosPrueba[0] != "")
                {
                    foreach (string casito in casosPrueba)
                    {
                        string[] estado = controlEjec.retornarEstado(casito).Split(';');
                        if (estado[0] != "")
                        {
                            foreach (string estadito in estado)
                            {

                                if (estadito.Split(',')[0] == "Satisfactorio")//Aún no se ha terminado, hay que realizar una consulta en ejecución.//Se supone caso exitoso
                                {
                                    exitosCant++;
                                    idCasosExitosos.Add(casito);
                                }
                                else if (estadito.Split(',')[0] == "Fallido" && estadito.Length > 1)//Se supone caso de no conformidad
                                {
                                    string key = estadito.Split(',')[1];
                                    if (noConformidad.ContainsKey(key))//Se suma una nueva no conformidad
                                    {
                                        noConformidad[key]++;
                                    }
                                    else//Se agrega nueva no conformidad
                                    {
                                        noConformidad.Add(key, 1);
                                    }
                                }
                                else if (estadito.Split(',')[0] == "Pendiente")//casos de prueba que no han sido evaluados aún
                                {
                                    sinEnvaluarCant++;
                                    idCasosSinEvaluar.Add(casito);
                                }
                            }
                        }                        
                    }
                    string CasosEx = "";
                    if (exitosCant > 0)
                    {
                        CasosEx = "Cantidad de casos exitosos: " + exitosCant.ToString() + "\nCasos que son exitosos:";
                        foreach (string rr in idCasosExitosos){
                            CasosEx += "\n\t" + rr+", ";
                        }
                    }
                        
                    else
                        CasosEx = "No hay casos de prueba exitosos.";
                    retorno.Add(CasosEx);

                    string noConf = "";
                    
                    double[] fallidosInt = new double[noConformidad.Count];
                    double[] fallidosPorc = new double[noConformidad.Count];
                    string[] fallidosString = new string[noConformidad.Count];
                    double total = 0;

                    int i = 0;
                    foreach (KeyValuePair<string, int> entry in noConformidad)
                    {

                        fallidosInt[i] = entry.Value;
                        total += entry.Value;
                        fallidosString[i] = entry.Key;
                        i++;
                    }

                    if (total > 0)
                    {
                        i = 0;
                        foreach (double d in fallidosInt)
                        {
                            fallidosPorc[i] = (d / total) * 100;
                            i++;
                        }

                        noConf = "Tipos de no conformidad:\n";
                        for (int j = 0; j < fallidosInt.Length; j++)
                        {
                            noConf += "\t" + fallidosString[j] + ": " + fallidosInt[j] + " - " + fallidosPorc[j] + "%, \n";
                        }                      
                    }
                    else
                        noConf = "No hay casos de prueba con no conformidades.";
                    retorno.Add(noConf);
                    string sinEv = "";
                    if (sinEnvaluarCant > 0)
                    {
                        sinEv = "Casos de prueba pendientes:";
                        foreach (string sin in idCasosSinEvaluar)
                        {
                            sinEv += "\n\t" + sin+",";
                        }                        
                    }
                    else
                        sinEv = "No hay casos de prueba pendientes.";
                    retorno.Add(sinEv);
                }
                else
                {
                    retorno.Add("");
                    retorno.Add("");
                    retorno.Add("");
                }
            }
            catch {
                retorno.Add("");
                retorno.Add("");
                retorno.Add("");
            }
            return retorno;

        }
    }
}