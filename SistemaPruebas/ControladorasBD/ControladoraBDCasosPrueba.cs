using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace SistemaPruebas.Controladoras
{
    public class ControladoraBDCasosPrueba
    {
        Acceso.Acceso acceso = new Acceso.Acceso();


       /*
        * Requiere: entidad Caso de Prueba
        * Modifica: inserta en la base de datos el Caso de Prueba en base a la información ingresada. Regresa 1 si la inserción fue exitosa y 0 si no.
        * Retorna: entero
        */
        public int ingresarCasosPrueba(EntidadCasosPrueba casoPrueba)
        {
            String consulta =
                "INSERT INTO Caso_Prueba(id_caso_prueba, proposito, entrada_de_datos, resultado_esperado, flujo_central, id_disenno, fechaUltimo) values('" +
                casoPrueba.Id_caso_prueba + "','" + casoPrueba.Proposito + "','" + casoPrueba.Entrada_datos + "','" + casoPrueba.Resultado_esperado + "','" +
                casoPrueba.Flujo_central + "'," + casoPrueba.Id_disenno +
                ", getDate()" + ");";
            int ret = acceso.Insertar(consulta);
            return ret;
        }

        /*
        * Requiere: entidad Caso de Prueba
        * Modifica: modifica en la base de datos el Caso de Prueba en base a la información ingresada. Regresa 1 si la modificación respectiva fue exitosa y 0 si no.
        * Retorna: entero
        */
        public int modificarCasosPrueba(EntidadCasosPrueba casoPrueba)
        {
            String consulta = "UPDATE Caso_Prueba SET id_caso_prueba ='" + casoPrueba.Id_caso_prueba +
                                "', proposito = '" + casoPrueba.Proposito +
                                "', entrada_de_datos = '" + casoPrueba.Entrada_datos +
                                "', resultado_esperado = '" + casoPrueba.Resultado_esperado +
                                "', flujo_central = '" + casoPrueba.Flujo_central +
                                "', fechaUltimo=getDate()" +
                                " WHERE id_caso_prueba = '" + casoPrueba.IdConsulta + "';";
            int ret = acceso.Insertar(consulta);
            return ret;

        }
        /*
        * Requiere: identificador del Caso de Prueba
        * Modifica: eliminia en la base de datos el Caso de Prueba en base al identificador recibido. Regresa 1 si la inserción fue exitosa y 0 si no.
        * Retorna: entero
        */
        public int eliminarCasosPrueba(String id)
        {
            return acceso.Insertar("DELETE FROM Caso_Prueba WHERE id_caso_prueba = '" + id + "';");
        }

        /*
         * Requiere: N/A
         * Modifica: regresa en forma de un Data Table toda la información consultada respectiva, con los datos de todos los Casos de Prueba en la base de datos.
         * Retorna: DataTable
         */
        public DataTable consultarCasosPrueba(int tipo, String id)
        {
            DataTable dt = null;
            String consulta = "";
            if (tipo == 1)//consulta para llenar grid, no ocupa la cedula pues los consulta a todos
            {
                consulta = "SELECT id_caso_prueba, proposito FROM Caso_Prueba ORDER BY fechaUltimo DESC;";
            }
            else if (tipo == 2)
            {
                consulta = "SELECT id_caso_prueba, proposito, entrada_de_datos, resultado_esperado, flujo_central FROM Caso_Prueba WHERE id_caso_prueba = '" + id + "';";
         
            }
            dt = acceso.ejecutarConsultaTabla(consulta);

            return dt;
        }

       /*
        * Requiere: id de diseño de pruebas.
        * Modifica: Solicita los casos de preuba asociados a idDiseno.
        * Retorna: hilera.
        */
        public String solicitarCasosdePrueba(int idDiseno)
        {
            using (SqlCommand comando = new SqlCommand("dbo.Consultar_Casos_Prueba"))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@id", idDiseno));
                return acceso.Consultar_Proced_Almacenado(comando);
            }
        }

        public DataTable consultarCasoPorRequerimiento(String id)
        {
            DataTable retorno = acceso.ejecutarConsultaTabla("select id_caso_prueba from caso_prueba where id_caso_prueba like '" + id + "-%';");
            return retorno;
        }
       
    }
}