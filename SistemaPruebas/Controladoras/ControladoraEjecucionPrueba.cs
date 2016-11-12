using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraEjecucionPrueba
    {
        ControladoraBDEjecucionPrueba controladoraBDEjecucionPrueba = new ControladoraBDEjecucionPrueba();
        ControladoraProyecto controladoraProyecto = new ControladoraProyecto();
        ControladoraDisenno  controladoraDisenno  = new ControladoraDisenno();
        ControladoraCasosPrueba controladoraCasosPrueba = new ControladoraCasosPrueba();
        ControladoraRecursosHumanos controladoraRecursosHumanos = new ControladoraRecursosHumanos();


        /*
         * Requiere: Los datos de Ejecución de Pruebas junto a las no conformidades, recibidos desde la interfaz.
         * Modifica: Manda los datos a la Controladora Base de Datos para hacer la inserción de ellos.
         * Retorna: Hilera.
         */
        public String insertarEjecucion(Object[] datosEjecucion, List<Object[]> datosNoConformidades)
        {
            EntidadEjecucionPrueba ejecucionPrueba = new EntidadEjecucionPrueba(datosEjecucion);
            string ret = controladoraBDEjecucionPrueba.insertarBDEjecucion(ejecucionPrueba);
            insertarNoConformidades(datosNoConformidades, ret);
            return ret;
        }

        /*
         * Requiere: Lista de objetos que contiene cada no conformidad asociada a idEjecución
         * Modifica: Manda los datos a la Controladora Base de Datos para hacer que se inserte cada no conformidad asociada al idEjecución
         * Retorna: entero.
         */
        public int insertarNoConformidades(List <Object []> datosNoConformidades, String idEjecucion)
        {
            foreach (Object[] dato in datosNoConformidades)
            {
                    dato[6] = idEjecucion;
                    EntidadNoConformidad noConformidad = new EntidadNoConformidad(dato);
                    controladoraBDEjecucionPrueba.insertarBDnoConformidad(noConformidad);
                
            }
            return 0;
        }


       /*
        * Requiere: N/A.
        * Modifica: Solicita todos los nombres de proyectos a la controladora de Proyectos.
        * Retorna: hilera.
        */
        public String solicitarProyectos()
        {
            String proyectos = controladoraProyecto.consultarProyectosConCaso();
            return proyectos;

        }

       /*
        * Requiere: id del Proyecto deseado.
        * Modifica: Solicita todos los diseños de prueba asociados a idProyecto a la controladora de Diseño.
        * Retorna: hilera.
        */
        public String solicitarPropositoDiseno(int idProyecto)
        {
            String disenos = controladoraDisenno.solicitarPropositoDiseno(idProyecto);
            return disenos;
        }

       /*
        * Requiere: id del Diseño deseado.
        * Modifica: Solicita todos los casos de prueba asociados a idDisenno a la controladora de Casos de Prueba.
        * Retorna: hilera.
        */
        public String solicitarCasosdePrueba(int idDisenno)
        {
            String casosDePrueba = controladoraCasosPrueba.solicitarCasosdePrueba(idDisenno);
            return casosDePrueba;
        }

        /*
        * Requiere: id del Proyecto deseado.
        * Modifica: Solicita todos los miembros de equipo asociados a idProyecto a la controladora de Recursos Humanos.
        * Retorna: hilera.
        */
        public String solicitarResponsables(int idProyecto)
        {
            return controladoraRecursosHumanos.solicitarNombreRecursoPorProyecto(idProyecto);
        }

        public String modificarEjecucion(Object[] datos, List<Object[]> datosNoConformidades)
        {
            EntidadEjecucionPrueba objEjecucion = new EntidadEjecucionPrueba(datos);
            String ret = controladoraBDEjecucionPrueba.modificarEjecucionPrueba(objEjecucion);
            modificarNoConformidades(datosNoConformidades, ret);
            return ret;
        }

       /*
        * Requiere: Los datos de Ejecución de Pruebas junto a las no conformidades, recibidos desde la interfaz.
        * Modifica: Manda los datos a la Controladora Base de Datos para hacer la modificación de ellos.
        * Retorna: entero.
        */
        public int modificarNoConformidades(List<Object[]> datosNoConformidades, String idEjecucion)
        {
            foreach (Object[] dato in datosNoConformidades)
            {
                dato[6] = idEjecucion;
                EntidadNoConformidad noConformidad = new EntidadNoConformidad(dato);
                controladoraBDEjecucionPrueba.modificarBDNoConformidad(noConformidad);
            }
            return 0;
        }


       /*
        * Requiere: id de ejecución de prueba
        * Modifica: Envía a la controladora de ejecución de pruebas el id de la ejecución para que sea borrada del sistema.
        * Retorna: entero.
        */
        public int eliminarEjecucionPrueba(String id)
        {
            int ret = controladoraBDEjecucionPrueba.eliminarEjecucionPrueba(id);
            return ret;
        }

        /*
        * Requiere: id de ejecución de prueba y el tipo de consulta
        * Modifica: Solicita datos a la controladora de base de datos de ejecución de pruebas, de todas las tuplas o solo de una.
        * Retorna: DataTable.
        */
        public DataTable consultarEjecucion(int tipo, String id)
        {
            DataTable dt = controladoraBDEjecucionPrueba.consultarEjecucionPrueba(tipo, id);
            return dt;

        }

       /*
        * Requiere: fecha de ejecución de prueba
        * Modifica: Solicita datos a la controladora de base de datos de ejecución de pruebas, de las no conformidades asociadas a fecha.
        * Retorna: DataTable.
        */
        public DataTable consultarNoConformidades(String fecha)
        {
            DataTable dt = controladoraBDEjecucionPrueba.consultarBDNoConformidad(fecha);
            return dt;
        }


       /*
        * Requiere: id de caso de prueba
        * Modifica: Solicita datos a la controladora casos de prueba el estado asociado a idCasoPrueba.
        * Retorna: hilera.
        */
        public String retornarEstado(String idCasoPrueba)
        {
            return controladoraBDEjecucionPrueba.retornarEstado(idCasoPrueba);
        }


        /*
        * Requiere: id de no conformidad.
        * Modifica: Manda id_noConformidad a la controladora de base de datos para que sea eliminada la tupla asociada.
        * Retorna: entero.
        */
        public int eliminarBDNoConformidad(string id_noConformidad)
        {
            return controladoraBDEjecucionPrueba.eliminarBDNoConformidad(id_noConformidad);
        }

       /*
        * Requiere: id de ejecución.
        * Modifica: Manda id_ejecución a la controladora de base de datos para que retorne la cantidad de no conformidades asociada.
        * Retorna: entero.
        */
        public int cantidadNoConformidades(string id_ejecucion)
        {
            return controladoraBDEjecucionPrueba.cantidadNoConformidades(id_ejecucion);
        }
    }
}