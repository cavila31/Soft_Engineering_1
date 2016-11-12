using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraBDEjecucionPrueba
    {
        Acceso.Acceso acceso = new Acceso.Acceso();


        /*
        * Requiere: Entidad de ejecución de pruebas recibida desde la controladora de ejecución de pruebas.
        * Modifica: Realiza la consulta con los datos recibidos para insertar una nueva ejecución de pruebas.
        * Retorna: Hilera.
        */
        public String insertarBDEjecucion(EntidadEjecucionPrueba ejecucion)
        {
            String consulta =
                "INSERT INTO Ejecucion(fecha, responsable, incidencias, id_disenno, fechaUltimo) values('" +
                ejecucion.Fecha + "','" + ejecucion.Responsable + "','" + ejecucion.Incidencias + "'," +
                ejecucion.Id_disenno + ", getDate()" + ");";
            int ret = acceso.Insertar(consulta);

            String fecha_regresa = "";
            if(ret != 2627)
            {
                DataTable dt=acceso.ejecutarConsultaTabla("select fecha from Ejecucion where fechaUltimo = (select max(e.fechaUltimo) from Ejecucion e)");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fecha_regresa = dr[0].ToString();
                    }
                }
            }
            else
            {
                fecha_regresa = "-";
            }
            return fecha_regresa;
        }


       /*
        * Requiere: Entidad de no conformidad recibida desde la controladora de ejecución de pruebas.
        * Modifica: Realiza la consulta con los datos recibidos para insertar una nueva no conformidad.
        * Retorna: entero.
        */
        public int insertarBDnoConformidad(EntidadNoConformidad noConformidad)
        {
            String consulta = "INSERT INTO noConformidad (tipo, idCaso, descripcion, justificacion,imagen, estado, fecha) VALUES ('" + noConformidad.Tipo               + "','"
                                                                                                                                     + noConformidad.Caso               + "','"
                                                                                                                                     + noConformidad.Descripcion        + "','"
                                                                                                                                     + noConformidad.Justificacion      + "', @img, '"
                                                                                                                                     + noConformidad.Estado             + "','"
                                                                                                                                     + noConformidad.Id_ejecucion       + "');";
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = consulta;
                cmd.Parameters.Add("@img", SqlDbType.Image, noConformidad.Imagen.Length).Value = noConformidad.Imagen;
                acceso.Insertar_Proced_Almacenado(cmd);
            }
            return 0;
        }


       /*
        * Requiere: Entidad de ejecución de pruebas recibida desde la controladora de ejecución de pruebas.
        * Modifica: Realiza la consulta con los datos recibidos para modificar la ejecución de pruebas.
        * Retorna: Hilera.
        */
        public String modificarEjecucionPrueba(EntidadEjecucionPrueba ejecucion)
        {
            String consulta = "UPDATE ejecucion SET fecha = '" + ejecucion.Fecha +
                                "', responsable = '" + ejecucion.Responsable +
                                "', incidencias = '" + ejecucion.Incidencias +
                                "', id_disenno = '" + ejecucion.Id_disenno +
                                "', fechaUltimo=getDate()" +
                                " WHERE fecha = '" + ejecucion.FechaConsulta + "';";
            int ret = acceso.Insertar(consulta);

            String fecha_regresa = "";
            if (ret != 2627)
            {
                DataTable dt = acceso.ejecutarConsultaTabla("select fecha from Ejecucion where fechaUltimo = (select max(e.fechaUltimo) from Ejecucion e)");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fecha_regresa = dr[0].ToString();
                    }
                }
            }
            else
            {
                fecha_regresa = "-";
            }
            return fecha_regresa;
        }


       /*
        * Requiere: Entidad de no conformidad recibida desde la controladora de ejecución de pruebas.
        * Modifica: Realiza la consulta con los datos recibidos para modificar la no conformidad.
        * Retorna: entero.
        */
        public int modificarBDNoConformidad(EntidadNoConformidad noConformidad)
        {
            DataTable dt = acceso.ejecutarConsultaTabla("if ((select id_noConformidad from noConformidad where id_noConformidad = '" + noConformidad.Id_noConformidad +
                                                        "' and  fecha = '" + noConformidad.Id_ejecucion +"')is null) select 0 else select 1");
            String consulta="";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    consulta = "UPDATE noConformidad SET " + "tipo = '" + noConformidad.Tipo + "', " +
                                                        "idCaso = '" + noConformidad.Caso + "', " +
                                                        "descripcion = '" + noConformidad.Descripcion + "', " +
                                                        "justificacion = '" + noConformidad.Justificacion + "', " +
                                                        "estado = '" + noConformidad.Estado + "' " +
                                                        "WHERE fecha = '" + noConformidad.Id_ejecucion + "'  " +
                                                        "AND id_noConformidad = '" + noConformidad.Id_noConformidad + "';";

                }
            }

            

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = consulta;
                cmd.Parameters.Add("@img", SqlDbType.Image, noConformidad.Imagen.Length).Value = noConformidad.Imagen;
                acceso.Insertar_Proced_Almacenado(cmd);
            }
            return 0;
        }


        /*
        * Requiere: id de Ejecución de preuba
        * Modifica: Realiza la consulta con el id recibido para eliminar a la ejecución de prueba asociada.
        * Retorna: entero.
        */
        public int eliminarEjecucionPrueba(String id)
        {
            return acceso.Insertar("DELETE FROM ejecucion WHERE fecha = '" + id + "';");
        }

        /*
        * Requiere: tipo de consulta e id de ejecución de prueba.
        * Modifica: Realiza la consulta con el id recibido para retornar sus datos dependiendo del tipo de consulta ( todas las tuplas o solo una).
        * Retorna: DataTable.
        */
        public DataTable consultarEjecucionPrueba(int tipo, String id)
        {
            DataTable dt = null;
            String consulta = "";
            if (tipo == 1)//consulta para llenar grid, no ocupa la cedula pues los consulta a todos
            {
                consulta = "SELECT fecha, responsable, '" + id + "' AS Diseño" + " FROM Ejecucion WHERE id_disenno=(select id_disenno from Disenno_Prueba where proposito='" + id + "') order by fechaUltimo desc";
            }
            else if (tipo == 2)
            {
                consulta = "SELECT fecha, responsable, incidencias, id_disenno FROM Ejecucion WHERE fecha = '" + id + "';";

            }
            dt = acceso.ejecutarConsultaTabla(consulta);

            return dt;
        }


       /*
        * Requiere: fecha de no conformidad
        * Modifica: Realiza la consulta con la fecha recibida para retornar los datos asociados.
        * Retorna: DataTable.
        */
        public DataTable consultarBDNoConformidad(String fecha)
        {
            DataTable dt = null;
            String consulta = "SELECT tipo, idCaso, descripcion, justificacion, imagen, estado, id_noConformidad FROM noConformidad WHERE fecha = '" + fecha + "';";
            dt = acceso.ejecutarConsultaTabla(consulta);
            return dt;
        }

       /*
        * Requiere: id de caso de prueba
        * Modifica: Realiza la consulta para retornar el estado asociado a casoPrueba.
        * Retorna: hilera.
        */
        public String retornarEstado(String casoPrueba)
        {
            DataTable retorno = acceso.ejecutarConsultaTabla("select estado, tipo from noConformidad where fecha= (select max(fecha) from noConformidad where idCaso='"+casoPrueba +"') and  idCaso= '" + casoPrueba + "'");
            String hilera = "";
           
            for (int i = 0; i < retorno.Rows.Count; i++)
            {
                hilera += retorno.Rows[i].ItemArray[0].ToString()+",";
                if (i == retorno.Rows.Count - 1)
                {
                    hilera += retorno.Rows[i].ItemArray[1].ToString();
                }
                else
                {
                    hilera += retorno.Rows[i].ItemArray[1].ToString() + ";";
                }                   
            }           
            return hilera;
        }


        /*
        * Requiere: id de la no conformidad.
        * Modifica: Realiza la consulta con id_noConformidad para eliminar la tupla asociada.
        * Retorna: entero.
        */
        public int eliminarBDNoConformidad(string id_noConformidad)
        {
            return acceso.Insertar("DELETE FROM noConformidad WHERE id_noConformidad = '" + id_noConformidad + "';");
        }


        /*
        * Requiere: id de la ejecución de prueba.
        * Modifica: Realiza la consulta con id_ejecución para retornar la cantidad de no conformidades asociada.
        * Retorna: entero.
        */
        public int cantidadNoConformidades(string id_ejecucion)
        {
            DataTable dt = acceso.ejecutarConsultaTabla("select count(*) from noConformidad where fecha='" + id_ejecucion + "'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    return Convert.ToInt32(dr[0]);
                }
            }
            return 0;
        }

    }
}