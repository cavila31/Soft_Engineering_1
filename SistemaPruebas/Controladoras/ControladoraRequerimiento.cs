using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraRequerimiento
    {

        ControladoraBDRequerimiento controladoraBDRequerimiento = new ControladoraBDRequerimiento();
        ControladoraProyecto controladoraProyecto = new ControladoraProyecto();
        ControladoraRecursosHumanos controladoraRecursosHumanos = new ControladoraRecursosHumanos();



        /*
         * Requiere: N/A.
         * Modifica: Hace el llamado al método que accede a la base de datos para regresar
           la cédula de quien tiene sesión abierta actualmente.
         * Retorna: número.
         */
        public int idDelLoggeado()
        {
            return controladoraRecursosHumanos.idDelLoggeado();
        }


        /*      
        Requiere: N/A
        Modifica: Usa la controladora de RH
        Retorna: un booleano que indica si esta loggeado un Admin o un Miembro de equipo
        */
        public bool PerfilDelLogeado()
        {
            bool retorno;
            string perfil = controladoraRecursosHumanos.perfilDelLoggeado();
            if (perfil == "Administrador")
            {
                retorno = true;
            }
            else
            {
                retorno = false;
            }
            return retorno;
        }

        /*
         *Requiere:  N/A
         *Modifica: Pregunta por el proyecto asociado del loggeado.
          Regresa verdadero si está en uso o falso si no.
         *Retorna: booleano.
         */
        public int proyectosDelLoggeado()
        {
            return controladoraRecursosHumanos.proyectosDelLoggeado();
        }
       

        /*
         *Requiere:  Número de cédula.
         *Modifica: Hace llamado al método que accede a la base de datos
          para hacer confirmación del uso del Requerimiento.
          Regresa verdadero si está en uso o falso si no.
         *Retorna: booleano.
         */
        public bool ConsultarUsoREQ(String id)		
        {		
            return controladoraBDRequerimiento.ConsultarUsoREQ(id);		
        }

        /*
         *Requiere:  Número de cédula y el estado del Uso actual.
         *Modifica: Se encarga de hacer el llamado al método que accede a la
          base de datos para cambiar el uso asociado al número de cédula.
         *Retorna: entero.
         */
        public int UpdateUsoREQ(String id, int use)		
        {		
            return controladoraBDRequerimiento.UpdateUsoREQ(id, use);		
        }


        /*
         * Requiere: Object[] datos
         * Modifica: Inserta un nuevo requerimiento en el sistema.
         * Retorna: int.
         */
        public int insertarRequerimiento(Object[] datos)
        {
            EntidadRequerimientos Requerimiento = new EntidadRequerimientos(datos);
            int ret = controladoraBDRequerimiento.insertarRequerimientoBD(Requerimiento);
            return ret;
        }

        /*
         * Requiere: Object[] datos
         * Modifica: Modifica un requerimiento en el sistema.
         * Retorna: int.
         */
        public int modificarRequerimiento(Object[] datos)
        {
            //datos[0];

            EntidadRequerimientos requerimiento = new EntidadRequerimientos(datos);
            int ret = controladoraBDRequerimiento.modificarRequerimientoBD(requerimiento);
            return ret;
        }

        /*
         * Requiere: Cédula
         * Modifica: Elimina un requerimiento del sistema.
         * Retorna: int.
         */
        public int eliminarRequerimiento(String cedula, int proyecto)
        {
            int ret = controladoraBDRequerimiento.eliminarRequerimientoBD(cedula, proyecto);
            return ret;
        }

        /*
         * Requiere: tipo de consulta y cédula.
         * Modifica: N/A.
         * Retorna: DataTable.
         */
        public DataTable consultarRequerimiento(int tipo, String cedula)
        {
            DataTable dt = controladoraBDRequerimiento.consultarRequerimientoBD(tipo, cedula);
            return dt;

        }

        /*
         * Requiere: N/A.
         * Modifica: N/A.
         * Retorna: String.
         */
        public String solicitarProyectos()
        {
            String proyectos = controladoraProyecto.Consultar_ID_Nombre_Proyecto();
            return proyectos;
        }

        /*
         * Requiere: N/A.
         * Modifica: N/A.
         * Retorna: String.
         */
        public String solicitarNombreProyecto(int id)
        {
            String proyecto = controladoraProyecto.ConsultarNombreProyectoPorId(id);
            return proyecto;
        }

        /*
         * Requiere: ID del diseño y del proyecto.
         * Modifica: N/A.
         * Retorna: DataTable.
         */
        public DataTable consultarRequerimientoNoEnDiseno(int id_proyecto, int id_diseno)
        {
            DataTable dt = controladoraBDRequerimiento.consultarRequerimientoNoEnDisenoBD(id_proyecto, id_diseno);
            return dt;
        }

        /*
         * Requiere: ID del diseño y del proyecto.
         * Modifica: N/A.
         * Retorna: DataTable.
         */
        public DataTable consultarRequerimientoEnDiseno(int id_proyecto, int id_diseno)
        {
            DataTable dt = controladoraBDRequerimiento.consultarRequerimientoEnDisenoBD(id_proyecto, id_diseno);
            return dt;
        }

        /*
         * Requiere: ID del diseño y del requerimiento.
         * Modifica: Desasocia un requerimiento de un diseño.
         * Retorna: int.
         */
        public int desasociarRequerimientoEnDiseno(int id_req, int id_diseno)
        {
            return controladoraBDRequerimiento.desasociarRequerimientoEnDisenoBD(id_req, id_diseno);//resultado de la eliminacion 
        }

        /*
        * Requiere: ID del diseño y del requerimiento.
        * Modifica: Asocia un requerimiento de un diseño.
        * Retorna: int.
        */
        public int asociarRequerimientoEnDiseno(int id_req, int id_diseno)
        {
            return controladoraBDRequerimiento.asociarRequerimientoEnDisenoBD(id_req, id_diseno);//resultado de la insersion
        }

        public DataTable consultarModulos(string id_req)
        {
            return controladoraBDRequerimiento.consultarRequerimientoBD(id_req);
            
        }

        public DataTable consultarReqPorNombre(string nombre, string id_proyecto)
        {
           return controladoraBDRequerimiento.consultarRequerimientoBD(nombre, id_proyecto);
          
        }

        public EntidadRequerimientos consultarReqUnico(string nombre, string id)
        {
            DataTable dt = controladoraBDRequerimiento.consultarReqUnico(nombre, id);
            if (dt.Rows.Count == 1)
            {
                Object[] datos = new Object[6];
                EntidadRequerimientos retorno;

                datos[0] = dt.Rows[0][0].ToString();
                datos[1] = dt.Rows[0][1].ToString();
                datos[2] = dt.Rows[0][2].ToString();
                datos[3] = dt.Rows[0][3].ToString();
                datos[4] = dt.Rows[0][4].ToString();
                datos[5] = dt.Rows[0][5].ToString();
               
                retorno = new EntidadRequerimientos(datos);
                return retorno;
            }
            else return null;
        }


    }
}
