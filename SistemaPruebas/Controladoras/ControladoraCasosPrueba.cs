using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraCasosPrueba
    {
        ControladoraBDCasosPrueba controladoraBDCasosPrueba;

        public ControladoraCasosPrueba()
        {
            controladoraBDCasosPrueba = new ControladoraBDCasosPrueba();
        }


        /*
        * Requiere: atributos del Caso de Prueba
        * Modifica: hace llamado al método que hace acceso a base de Datos para ingresar los datos del Caso de Prueba correspondiente. Con base a lo que regresa este método, devuelve como valor de retorno el valor de éxito de la inserción.
        * Retorna: entero
        */
        public int insertarCasosPrueba(object[] datos)
        {
            EntidadCasosPrueba casoPrueba = new EntidadCasosPrueba(datos);
            int ret = controladoraBDCasosPrueba.ingresarCasosPrueba(casoPrueba);
            return ret;
        }

       /*
        * Requiere: atributos del Caso de Prueba
        * Modifica: hace llamado al método que modifica en la base de datos el Caso de Prueba en base a la información ingresada. Regresa 1 si la modificación respectiva fue exitosa y 0 si no.
        * Retorna: entero
        */
        public int modificarCasosPrueba(Object[] datos)
        {
            EntidadCasosPrueba objCasoPrueba = new EntidadCasosPrueba(datos);
            int ret = controladoraBDCasosPrueba.modificarCasosPrueba(objCasoPrueba);
            return ret;
        }

        /*
         * Requiere: identificador del Caso de Prueba
         * Modifica: hace el llamado al método que eliminia en la base de datos el Caso de Prueba en base al identificador recibido. Regresa 1 si la inserción fue exitosa y 0 si no.
         * Retorna: entero       
         */
        public int eliminarCasosPrueba(String id)
        {
            int ret = controladoraBDCasosPrueba.eliminarCasosPrueba(id);
            return ret;
        }


        /*
         * Requiere: N/A
         * Modifica: hace llamado que regresa en forma de un Data Table toda la información consultada respectiva, con los datos de todos los Casos de Prueba en la base de datos.
         * Retorna: DataTable
        */
        public DataTable consultarCasosPrueba(int tipo, String id)
        {
            DataTable dt = controladoraBDCasosPrueba.consultarCasosPrueba(tipo, id);
            return dt;

        }

       /*
        * Requiere: id del Diseño deseado.
        * Modifica: Solicita todos los casos de prueba asociados a idDiseno a la controladora de bases de datos de Casos de Prueba.
        * Retorna: hilera.
        */
        public String solicitarCasosdePrueba(int idDiseno)
        {
            String casosDePrueba = controladoraBDCasosPrueba.solicitarCasosdePrueba(idDiseno);
            return casosDePrueba;
        }

        /*
         * Requiere: id del requerimiento.
         * Modifica: Solicita el caso de prueba asociado a idReq.
         * Retorna: arreglo de hileras
         */
        public string[] consultarCasoPorRequerimiento(String idReq)
        {
            string[] retorno = null;
            DataTable dt = controladoraBDCasosPrueba.consultarCasoPorRequerimiento(idReq);
            int i = 0;
            retorno = new string[dt.Rows.Count];
            foreach (DataRow dr in dt.Rows)
            {
                retorno[i] = dr[0].ToString();
                i++;
            }
            return retorno;
        }
    }
}