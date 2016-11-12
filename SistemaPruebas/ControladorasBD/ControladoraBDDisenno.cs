using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraBDDisenno
    {
        Acceso.Acceso acceso = new Acceso.Acceso();

        //Requiere: Recibir la información de proyecto encapsulado
        //Modifica: Inserta un  nuevo proyecto en la base de datos
        //Retorna: N/A
        public int InsertarDiseno(EntidadDisenno datos)
        {
            using (SqlCommand comando = new SqlCommand("dbo.Insertar_Disenno"))
            {
     
                comando.CommandType = CommandType.StoredProcedure;
               
                comando.Parameters.Add(new SqlParameter("@proposito", datos.Proposito));

                comando.Parameters.Add(new SqlParameter("@nivel", datos.Nivel));

                comando.Parameters.Add(new SqlParameter("@tecnica", datos.Tecnica));

                comando.Parameters.Add(new SqlParameter("@ambiente", datos.Ambiente));

                comando.Parameters.Add(new SqlParameter("@procedimiento", datos.Procedimiento));

                comando.Parameters.Add(new SqlParameter("@fecha_de_disenno", datos.FechaDeDisenno));

                comando.Parameters.Add(new SqlParameter("@criterio_aceptacion", datos.CriterioAceptacion));

                comando.Parameters.Add(new SqlParameter("@cedula", datos.Responsable));

                comando.Parameters.Add(new SqlParameter("@id_proyecto", datos.ProyAsociado));
                return acceso.Insertar_Proced_Almacenado(comando);

            }
        }

        /*
         * Requiere: Entidad de Requerimiento
         * Modifica: Inserta un nuevo requerimiento de un disenno en el sistema.
         * Retorna: int.
         */
        public int insertarRequerimientoBD(EntidadRequerimientos Requerimiento)
        {
            String consulta = "INSERT INTO Requerimiento(id_requerimiento,precondiciones,Requerimientos_especiales) values('" + Requerimiento.Id + "','" + Requerimiento.Precondiciones + "','" + Requerimiento.Precondiciones + "', getDate())";
            int ret = acceso.Insertar(consulta);
            return ret;

        }

        /*
         * Requiere: Entidad de Disenno
         * Modifica: Modifica un disenno previamente ingresado al sistema.
         * Retorna: int.
        */
        public int modificarDisennoBD(EntidadDisenno Disenno, int id)
        {
            String consulta = "UPDATE Disenno_Prueba SET proposito = '" + Disenno.Proposito + "', nivel= " + Disenno.Nivel + " , tecnica= " + Disenno.Tecnica + ", ambiente= '" + Disenno.Ambiente + "', procedimiento= '" + Disenno.Procedimiento + "', fecha_de_disenno= '" + Disenno.FechaDeDisenno + "', criterio_aceptacion= '" + Disenno.CriterioAceptacion + "', cedula = " + Disenno.Responsable + ", fechaUltimo =getDate() WHERE id_disenno =" + id;
            int ret = acceso.Insertar(consulta);
            return ret;
        }
         
        /*
         * Requiere: Id del diseño
         * Modifica: Elimina un disenno del sistema.
         * Retorna: int.
         */

        public int eliminarDisennoBD(int id)
        {
            return acceso.Insertar("DELETE FROM Disenno_Prueba WHERE id_disenno = " + id + ";");
        }

        /*
         * Requiere: Id requerimiento.
         * Modifica: Elimina un requerimiento de un disenno del sistema.
         * Retorna: int.
         */

        public int eliminarRequerimientoBD(String id)
        {
            return acceso.Insertar("DELETE FROM Requerimiento WHERE id_requerimiento = '" + id + "';");
        }

        /*
         * Requiere: tipo de consulta y cédula.
         * Modifica: N/A.
         * Retorna: DataTable.
         */
        public DataTable consultarDisennoBD(int tipo, int id)
        {
            DataTable dt = null;
            String consulta = "";
            if (tipo == 1)
            {
                consulta = "select proposito, nivel, tecnica, ambiente, procedimiento, fecha_de_disenno, criterio_aceptacion, cedula, id_proyecto from Disenno_Prueba where id_disenno= " + id;             
                    }
            else if (tipo == 2)//consulta para llenar grid
            {
                consulta = "select D.proposito, D.nivel, D.tecnica, R.nombre_completo from Disenno_Prueba D, Recurso_Humano R where D.cedula=R.cedula AND D.id_proyecto=" + id + " ORDER BY D.fechaUltimo desc";
                    }

                dt = acceso.ejecutarConsultaTabla(consulta);

            return dt;

        }

        /*
         Requiere: propósito de un diseño
         Modifica: Hace acceso a la base de datos para obtener el id del diseño dado el propósito del mismo.
         Retorna: entero.
         */
        public int consultarId_Disenno(String proposito)
        {
            DataTable dt = new DataTable();
            dt = acceso.ejecutarConsultaTabla("select id_disenno from Disenno_Prueba where proposito = '" + proposito + "'");
            return Int32.Parse(dt.Rows[0][0].ToString());
        }


        /*
         * Métodos relacionados con la entidadDisennoReq, entidad compartida entre Diseño y Requerimiento
         */


        /*
         * Requiere: Id diseño.
         * Modifica: Consulta todos los requerimientos del sistema.
         * Retorna: int.       
        */
        public DataTable consultarDisennoReq()
        {
            DataTable dt = null;
            String consulta = "";           
            consulta = "select * from Prueba_Disenno_Req";          
            dt = acceso.ejecutarConsultaTabla(consulta);
            return dt;
        }

        /*
         * Requiere: Id diseño.
         * Modifica: Consulta un requerimiento de un disenno del sistema.
         * Retorna: dataTable.       
        */
        public DataTable consultarDisennoReq(int idDisenno)
        {
            DataTable dt = null;
            String consulta = "";
             consulta = "select p.id_requerimiento, r.nombre from Prueba_Disenno_Req p, requerimiento r where p.id_disenno = " + idDisenno + " AND p.id_requerimiento = r.id_requerimiento";

           // consulta = "select id_requerimiento from Prueba_Disenno_Req where id_disenno = " +idDisenno;
            dt = acceso.ejecutarConsultaTabla(consulta);
            return dt;
        }


        /*
         * Requiere: Id diseño.
         * Modifica: Inserta un requerimiento de un disenno del sistema.
         * Retorna: int.       
        */
        public int InsertarDisenoReq(SistemaPruebas.Entidades.EntidadDisennoReq datos)
        {
            using (SqlCommand comando = new SqlCommand("dbo.Insertar_Disenno_Req"))
            {

                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add(new SqlParameter("@idPrueba", datos.IdPrueba));
                comando.Parameters.Add(new SqlParameter("@idReq", datos.IdReq));
                comando.Parameters.Add(new SqlParameter("@idDisenno", datos.IdDisenno));                
                return acceso.Insertar_Proced_Almacenado(comando);
            }
        }


        /*
         * Requiere: Id diseño.
         * Modifica: Elimina un requerimiento de un disenno del sistema.
         * Retorna: int.       
        */

        public int EliminarDisennoReq(int idDisenno, string idReq)
        {          
            String consulta = "";
            consulta = "delete from Prueba_Disenno_Req where id_requerimiento = '" + idReq + "' and id_disenno = " + idDisenno;
            return acceso.Insertar(consulta);          
        }

        public String solicitarPropositoDiseno(int idProyecto)
        {
            using (SqlCommand comando = new SqlCommand("dbo.Consultar_Diseno_ID_Proyecto"))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add(new SqlParameter("@id", idProyecto));
                return acceso.Consultar_Proced_Almacenado(comando);

            }
        }
    }
}