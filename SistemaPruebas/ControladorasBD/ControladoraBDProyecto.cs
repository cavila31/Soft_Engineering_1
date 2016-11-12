using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient; 

namespace SistemaPruebas.Controladoras
{
    public class ControladoraBDProyecto
    {
        Acceso.Acceso acceso_BD = new Acceso.Acceso();

        public ControladoraBDProyecto()
        {

        }

        //InsertarProyecto();
        //Requiere: Recibir la información de proyecto encapsulado
        //Modifica: Inserta un  nuevo proyecto en la base de datos
        //Retorna: N/A
        public int InsertarProyecto(EntidadProyecto datos)
        {
            using (SqlCommand comando = new SqlCommand("dbo.Insertar_Proyecto"))
            {
                //  comando.CommandText = "Insertar_Proyecto";
                comando.CommandType = CommandType.StoredProcedure;

                //  comando.Parameters.Add(new SqlParameter("@id_proyecto", datos.Id_proyecto));

                comando.Parameters.Add(new SqlParameter("@nombre_sistema", datos.Nombre_sistema));

                comando.Parameters.Add(new SqlParameter("@objetivo_general", datos.Objetivo_general));

                comando.Parameters.Add(new SqlParameter("@fecha_asignacion", datos.Fecha_asignacion));

                comando.Parameters.Add(new SqlParameter("@estado", datos.Estado));

                comando.Parameters.Add(new SqlParameter("@nombre_rep", datos.Nombre_representante));

                comando.Parameters.Add(new SqlParameter("@telefono_rep", datos.Telefono_representante));

                comando.Parameters.Add(new SqlParameter("@oficina_rep", datos.Oficina_representante));

                comando.Parameters.Add(new SqlParameter("@LiderProyecto", datos.LiderProyecto));

               return acceso_BD.Insertar_Proced_Almacenado(comando);

            }
        }

        public int InsertarProyecto_SinLider(EntidadProyecto datos)
        {
            using (SqlCommand comando = new SqlCommand("dbo.Insertar_Proyecto_SinLider"))
            {
                //  comando.CommandText = "Insertar_Proyecto";
                comando.CommandType = CommandType.StoredProcedure;

                //  comando.Parameters.Add(new SqlParameter("@id_proyecto", datos.Id_proyecto));

                comando.Parameters.Add(new SqlParameter("@nombre_sistema", datos.Nombre_sistema));

                comando.Parameters.Add(new SqlParameter("@objetivo_general", datos.Objetivo_general));

                comando.Parameters.Add(new SqlParameter("@fecha_asignacion", datos.Fecha_asignacion));

                comando.Parameters.Add(new SqlParameter("@estado", datos.Estado));

                comando.Parameters.Add(new SqlParameter("@nombre_rep", datos.Nombre_representante));

                comando.Parameters.Add(new SqlParameter("@telefono_rep", datos.Telefono_representante));

                comando.Parameters.Add(new SqlParameter("@oficina_rep", datos.Oficina_representante));

                return acceso_BD.Insertar_Proced_Almacenado(comando);

            }
        }




        //Consultar_ID_Nombre_Proyecto();
        //Requiere: N/A
        //Modifica: Se invoca al procedimiento Almacenado Consultar_ID_Nombre_Proyecto ubicado en la base datos, con el fin de que devuleva un string con el id de todos los proyectos.
        //Retorna: string con el valor de todos los proyectos.

        public string Consultar_ID_Nombre_Proyecto()
        {
            using (SqlCommand comando = new SqlCommand("dbo.Consultar_ID_Nombre_Proyecto"))
            {
                //  comando.CommandText = "Insertar_Proyecto";
                comando.CommandType = CommandType.StoredProcedure;

                //  comando.Parameters.Add(new SqlParameter("@id_proyecto", datos.Id_proyecto));

                return acceso_BD.Consultar_Proced_Almacenado(comando);

            }
        }

        //ConsultarProyecto
        //Requiere: un entero con el id de un proyecto a consultar
        //Modifica: Consulta por todos los datos del proyecto que corresponde al id de parámentro
        //Retorna: Un dataTable con los todos los datos de la tupla consultada.
        public DataTable ConsultarProyecto(int id_Proyecto)
        {
            string id= id_Proyecto.ToString();
            DataTable dt = new DataTable();
            dt = acceso_BD.ejecutarConsultaTabla("select * from Proyecto where id_proyecto= "+id);
            return dt;
        }

        public DataTable ConsultarProyecto()
        {
            DataTable dt = new DataTable();
            //dt = acceso_BD.ejecutarConsultaTabla("select id_proyecto, nombre_sistema, fecha_asignacion, estado, nombre_rep from Proyecto where id_proyecto >=0");
            return dt;
        }

        //ConsultarProyectoIdNombre();
        //Requiere: N/A
        //Modifica: Consulta el id y el nombre de todos los proyectos.
        //Retorna: Un dataTable con el nombre y Id de todos los proyectos.
        public DataTable ConsultarProyectoIdNombre()
        {
            DataTable dt = new DataTable();
            dt = acceso_BD.ejecutarConsultaTabla("select p.id_proyecto, p.nombre_sistema, r.nombre_completo from Proyecto p left outer join Recurso_Humano r on p.LiderProyecto=r.cedula where p.id_proyecto >=0 ORDER BY p.id_proyecto DESC");
            return dt;
        }
        
        //ConsultarProyectoIdNombre();
        //Requiere: Entero con el id de un proyecto a consultar.
        //Modifica: Consulta el nombre y el id del proyecto que concuerde con el atributo de ingreso.
        //Retorna: Un dataTable con el nombre y Id del proyecto que concuerde con el parámetro de ingreso.
        public DataTable ConsultarProyectoIdNombre(int id_Proyecto)
        {
            DataTable dt = new DataTable();
            dt = acceso_BD.ejecutarConsultaTabla("select id_proyecto, nombre_sistema, LiderProyecto from Proyecto where id_proyecto = " + id_Proyecto);
            return dt;
        }

        //ConsultarProyectoIdPorNombre();
        //requiere: String con el nombre de un proyecto a consultar.
        //Modifica: Consulta el id del proyecto cuyo nombre concuerde con el atributo de ingreso.
        //Retorna: Un entero con el id del proyecto consultado.
        public int ConsultarProyectoIdPorNombre(string nombre)
        {
            DataTable dt = new DataTable();
            dt = acceso_BD.ejecutarConsultaTabla("select id_proyecto from proyecto where nombre_sistema = '"+nombre+"' and id_proyecto >= 0");
            try
            {
                return Int32.Parse(dt.Rows[0][0].ToString());
            }
            catch
            {
                return -1;
            }
        }


        //CancelarProyecto
        //Requiere: String con el id del proyecto a consultar
        //Modifica: Modifica el estado del proyecto cuyo id concuerda con el parámetro de ingreso, a 5 (Cancelado).
        //Retorna: Un 1 si la operación fue exitosa, otro número en caso de un error.
        public int CancelarProyecto(string id)
        {
            return acceso_BD.EliminarProyecto("update Proyecto set estado = 5 where id_proyecto =" + id);
        }


        //Eliminar_Proyecto();
        //Requiere: String con el id del proyecto a eliminar
        //Modifica: Se elimina el proyecto cuyo id concuerda con el parámetro de ingreso. 
        //Retorna: Un 1 en caso de que la operación haya sido exitosa, otro número en caso de error.
            public int EliminarProyecto(string id)
        {
            return acceso_BD.EliminarProyecto("Delete from Proyecto where id_proyecto =" + id);
        }


        //ActualizarProyecto();
        //Requiere: Recibir la información de proyecto encapsulado
        //Modifica: Modifica los valores para el proyecto ingresado por parámetro.
        //Retorna: Un 1 en caso de que la operación haya sido exitosa, otro número en caso contrario.
        public int ActualizarProyecto(EntidadProyecto datos)
        {
            using (SqlCommand comando = new SqlCommand("dbo.Modificar_Proyecto"))
            {
                //  comando.CommandText = "Insertar_Proyecto";
                comando.CommandType = CommandType.StoredProcedure;

                //  comando.Parameters.Add(new SqlParameter("@id_proyecto", datos.Id_proyecto));

                comando.Parameters.Add(new SqlParameter("@id", datos.Id_proyecto));

                comando.Parameters.Add(new SqlParameter("@nombre_sistema", datos.Nombre_sistema));

                comando.Parameters.Add(new SqlParameter("@objetivo_general", datos.Objetivo_general));

                comando.Parameters.Add(new SqlParameter("@fecha_asignacion", datos.Fecha_asignacion));

                comando.Parameters.Add(new SqlParameter("@estado", datos.Estado));

                comando.Parameters.Add(new SqlParameter("@nombre_rep", datos.Nombre_representante));

                comando.Parameters.Add(new SqlParameter("@telefono_rep", datos.Telefono_representante));

                comando.Parameters.Add(new SqlParameter("@oficina_rep", datos.Oficina_representante));
                
                if(datos.LiderProyecto.ToString() != "")
                comando.Parameters.Add(new SqlParameter("@LiderProyecto", datos.LiderProyecto));
                else
                    comando.Parameters.Add(new SqlParameter("@LiderProyecto", DBNull.Value));    

                return acceso_BD.Insertar_Proced_Almacenado(comando);

            }
        }

        //ConsultarUsoProyecto();
        //Requiere: Entero con el id de un proyecto a consultar
        //Modifica: Se consulta sobre el estado de uso(si está siendo modificado o no), del proyecto cuyo id concuerde con el parámetro de ingreso.
        //Retorna: Un 1 en caso de que el proyecto esté siendo modificado, un 0 en caso contrario.
        public int ConsultarUsoProyecto(int id)
        {
            DataTable dt = new DataTable();
            dt = acceso_BD.ejecutarConsultaTabla("select Uso from proyecto where id_proyecto =" + id);
            return Int32.Parse(dt.Rows[0][0].ToString());
        }

        //UpdateUsoProyecto();
        //Requiere: Entero con el id de un proyecto a consultar y entero con el valor de uso(0 si no está siendo modificado, 1 en caso contrario)
        //Modifica: Actualiza la condición de uso(Proyecto que está siendo modificado o eliminado) de un proyecto.
        //Retorna: Un 1 en caso de que el proyecto se haya actualizado correctamente, otro número en caso contrario.
        public int UpdateUsoProyecto(int id, int use)
        {
            return acceso_BD.EliminarProyecto("update Proyecto set Uso = "+use+" where id_proyecto =" + id);            
        }


        //ConsultarNombreProyectoPorId();
        //Requiere: Entero con el id de un proyecto a consultar.
        //Modifica: Consulta el nombre del proyecto cuyo id concuerde con el atributo de ingreso.
        //Retorna: String con el nombre del proyecto.
        public string ConsultarNombreProyectoPorId(int id)
        {
            DataTable dt = new DataTable();
            dt = acceso_BD.ejecutarConsultaTabla("select nombre_sistema from proyecto where  id_proyecto=" + id);
            return dt.Rows[0][0].ToString();
        }

        public String consultarProyectosConCaso()
        {
            using (SqlCommand comando = new SqlCommand("dbo.Consultar_Proyecto_Con_Caso"))
            {
                comando.CommandType = CommandType.StoredProcedure;
                return acceso_BD.Consultar_Proced_Almacenado(comando);

            }
        }
    }
}