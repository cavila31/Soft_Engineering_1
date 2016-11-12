using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraBDRequerimiento
    {
        Acceso.Acceso acceso = new Acceso.Acceso();



        /*
         *Requiere:  Número de ID del requerimiento
         *Modifica: Hace acceso a base de datos para consultar el estado de
          Uso del requerimiento, si está siendo  manipulado en algún otro lugar.
         *Retorna: booleano.
        */
        public bool ConsultarUsoREQ(String id)		
        {		
            bool regresa = false;		
            int el_uso = 0;
            DataTable DR = acceso.ejecutarConsultaTabla("select esta_en_Uso from Requerimiento where id_requerimiento ='" + id+";	");	
            try		
            {		
                foreach (DataRow row in DR.Rows)		
                {		
                    el_uso = (int)row["esta_en_Uso"];		
                }		
            }		
            catch (System.InvalidOperationException)		
            {		
                regresa = false;		
            }		
            if (el_uso==0)		
            {		
                regresa = false;		
            }		
            else		
            {		
                regresa = true;		
            }		
            return regresa;		
        }		

        /*
         *Requiere:  ID del requerimiento y el estado de Uso actual.
         *Modifica: Con el ID del requerimiento que recibe, cambia en la base de datos
          el estado del Uso asociado a este. Este indicará que el requerimiento
          se encuentra o no en otro lado modificado.
         *Retorna: entero.
        */
        public int UpdateUsoREQ(String id, int use)		
        {		
            return acceso.Insertar("update Requerimiento set esta_en_Uso = " + use + " where id_requerimiento ='" + id+";	");
        }		


        /*
         * Requiere: Entidad de requerimiento
         * Modifica: Inserta un nuevo requerimiento en el sistema.
         * Retorna: int.
         */
        public int insertarRequerimientoBD(Controladoras.EntidadRequerimientos requerimiento)
        {   
            String consulta = "INSERT INTO Requerimiento(id_requerimiento,precondiciones,Requerimientos_especiales,id_proyecto, nombre, fechaUltimo) VALUES ('" + requerimiento.Id + "','" + requerimiento.Precondiciones + "','" + requerimiento.RequerimientosEspeciales+ "',"+requerimiento.Proyecto+ ",'" + requerimiento.Nombre + "', getDate());";
            int ret = acceso.Insertar(consulta);
            return ret;

        }

        /*
         * Requiere: Entidad de requerimiento
         * Modifica: Modifica un requerimiento previamente ingresado al sistema.
         * Retorna: int.
         */
        public int modificarRequerimientoBD(Controladoras.EntidadRequerimientos requerimiento)
        {
            String consulta = "UPDATE Requerimiento SET id_requerimiento='" + requerimiento.Id + "',precondiciones='" + requerimiento.Precondiciones + "',Requerimientos_especiales='" + requerimiento.RequerimientosEspeciales + "',id_proyecto='" + requerimiento.Proyecto + "', nombre='" + requerimiento.Nombre + "', fechaUltimo=getDate() WHERE id_requerimiento='" + requerimiento.IdViejo + "';";
            int ret = acceso.Insertar(consulta);
            return ret;
        }

        /*
         * Requiere: ID
         * Modifica: Elimina un requerimiento del sistema.
         * Retorna: int.
         */
        public int eliminarRequerimientoBD(String cedula, int id_proyecto)
        {
            return acceso.Insertar("DELETE FROM Requerimiento WHERE id_requerimiento = '" + cedula + "' and id_proyecto='" + id_proyecto + "';");
        }

        /*
         * Requiere: tipo de consulta y ID.
         * Modifica: N/A.
         * Retorna: DataTable.
         */
        public DataTable consultarRequerimientoBD(int tipo, String id)
        {
            DataTable dt = null;
            String consulta = "";
            if (tipo == 1)//consulta para llenar grid, no ocupa el ID pues los consulta a todos
            {
                consulta = "SELECT id_requerimiento,precondiciones,Requerimientos_especiales, nombre, id_proyecto from Requerimiento ORDER BY fechaUltimo desc;";
            }
            else if (tipo == 2)
            {
                consulta = "SELECT id_requerimiento,precondiciones,Requerimientos_especiales, nombre, id_proyecto from Requerimiento where id_requerimiento='" + id+"';";
            }
            else if (tipo == 3)
            {
                consulta = "SELECT id_requerimiento,nombre, id_proyecto from Requerimiento where id_proyecto='" + Convert.ToInt32(id) + "'  ORDER BY fechaUltimo desc;";
            }
                

            dt = acceso.ejecutarConsultaTabla(consulta);

            return dt;

        }

        public DataTable consultarRequerimientoBD(String id)
        {
            DataTable dt = null;
            String consulta = "";           
            consulta = "SELECT nombre from Requerimiento where id_proyecto =" + id + " order by nombre;"; 
            
            dt = acceso.ejecutarConsultaTabla(consulta);
            string[] modulos = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string[] array = dr[0].ToString().Split('-');
                dr[0] = array[0];
            }

            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            
            foreach (DataRow drow in dt.Rows)
            {
                if (hTable.Contains(drow[0]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[0], string.Empty);
            }

 
            foreach (DataRow dRow in duplicateList)
                dt.Rows.Remove(dRow);

           
            return dt;
        }




        public DataTable consultarRequerimientoBD(string nombre, String id)
        {
            DataTable dt = null;
            String consulta = "";
            consulta = "SELECT nombre from Requerimiento where id_proyecto =" + id + " and nombre like '"+nombre+"-%' order by nombre;";

            dt = acceso.ejecutarConsultaTabla(consulta);            
           
            
            return dt;
        }

        public DataTable consultarReqUnico(string nombre, string id)
        {
            DataTable dt = null;
            string consulta = "";
            consulta = "select * from Requerimiento wherer id_proyecto =" + id + " and nombre = '" + nombre + "';";

            dt = acceso.ejecutarConsultaTabla(consulta);

            return dt;
        }

        /*
         * Requiere: ID del diseño y del proyecto.
         * Modifica: N/A.
         * Retorna: DataTable.
         */
        public DataTable consultarRequerimientoNoEnDisenoBD(int id_proyecto, int id_diseno)
        { 
            String consulta = "select id_requerimiento, nombre from Requerimiento where id_proyecto=" + id_proyecto + " and id_requerimiento not in (select id_requerimiento from Prueba_Disenno_Req where id_proyecto=" + id_proyecto + " and id_disenno=" + id_diseno + ");";
            DataTable dt = acceso.ejecutarConsultaTabla(consulta);
            return dt;
        }

        /*
         * Requiere: ID del diseño y del proyecto.
         * Modifica: N/A.
         * Retorna: DataTable.
         */
        public DataTable consultarRequerimientoEnDisenoBD(int id_proyecto, int id_diseno)
        {
            String consulta = "select id_requerimiento from Prueba_Disenno_Req where id_proyecto = " + id_proyecto + " and id_disenno = " + id_diseno;

            DataTable dt = acceso.ejecutarConsultaTabla(consulta);
            return dt;

        }

        /*
         * Requiere: ID del diseño y del requerimiento.
         * Modifica: Desasocia un requerimiento de un diseño.
         * Retorna: int.
         */
        public int desasociarRequerimientoEnDisenoBD(int id_req, int id_diseno)
        {

            String consulta = "delete from Prueba_Disenno_Req where id_disenno=" + id_diseno + " and id_requerimiento='" + id_req + "';";
            int ret = acceso.Insertar(consulta);
            return ret;

        }

        /*
        * Requiere: ID del diseño y del requerimiento.
        * Modifica: Asocia un requerimiento de un diseño.
        * Retorna: int.
        */
        public int asociarRequerimientoEnDisenoBD(int id_req, int id_diseno)
        {

            String consulta = "insert Prueba_Disenno_Req (id_disenno, id_proyecto, id_requerimiento)values (" + id_diseno + ",(select id_proyecto from Disenno_Prueba where id_disenno=" + id_diseno + " ),'" + id_req + "');";
            int ret = acceso.Insertar(consulta);
            return ret;

        }      


        /*select id_requerimiento from Requerimiento
where id_requerimiento not in (select id_requerimiento
								from Prueba_Disenno_Req
								where id_proyecto=-1 and id_disenno=4)

select id_requerimiento from Prueba_Disenno_Req
where id_disenno=4;


insert Prueba_Disenno_Req (id_disenno, id_proyecto, id_requerimiento)values (4,(select id_proyecto from Disenno_Prueba where id_disenno=4 ),'req 3')*/

    }
}
