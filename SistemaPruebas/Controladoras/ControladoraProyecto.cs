using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraProyecto
    {
        ControladoraBDProyecto controlBD;
        ControladoraRecursosHumanos controlRH;
        private static Dictionary<int, string> modificados = new Dictionary<int, string>();
        /*      
        Requiere: N/A
        Modifica: Inicializa la instancia de la controladora de BD
        Retorna: N/A
        */
        public ControladoraProyecto()
        {
            controlBD = new ControladoraBDProyecto();            
        }     
        /*
        Requiere: Los siguientes object [].
        Modifica: Solicita a la entidad de proyecto crear un objeto con el parámetro y llama a controladoraBDProyecto para hacer la inserción. 
        Retorna: un 0 (cero) en caso de que se haya insertado correctamente, un -1(menos uno) en el caso contrario.
        */
        public int IngresaProyecto(object[] datos)
        {
            int a = 0;
            if (datos[8].ToString() != "")
            {
                datos[8] = controlRH.solicitarCedulaRecurso(datos[8].ToString()).ToString();
                controlRH.addProyecto(Int32.Parse(datos[8].ToString()), Int32.Parse(datos[0].ToString()));
                EntidadProyecto objProyecto = new EntidadProyecto(datos);
                a = controlBD.InsertarProyecto_SinLider(objProyecto);
            }
            else
            {
                datos[8] = "";
                EntidadProyecto objProyecto = new EntidadProyecto(datos);
                a = controlBD.InsertarProyecto_SinLider(objProyecto);
            }
            return a;            
        }
        /*      
        Requiere: Los datos ingresados atravez de la interfaz mediante un object[]
        Modifica: Actualiza la tupla correspondiente mediante la controladora de BD
        Retorna: N/A
        */
        public int ActualizarProyecto(object[] datos)
        {
            if (datos[8].ToString() != "")
            {
                datos[8] = controlRH.solicitarCedulaRecurso(datos[8].ToString()).ToString();
                controlRH.addProyecto(Int32.Parse(datos[8].ToString()), Int32.Parse(datos[0].ToString()));
            }
            else
                datos[8] = "";
            EntidadProyecto objProyecto = new EntidadProyecto(datos);
            int a = controlBD.ActualizarProyecto(objProyecto);
            return a;
        }
        /*      
      Requiere: N/A
      Modifica: Pide a la controladora de BD de proyecto el nombre y id de todos los proyectos
      Retorna: Un string con todos los nombres de proyecto y sus respectivos ids
      */
        public string Consultar_ID_Nombre_Proyecto()
        {

            return controlBD.Consultar_ID_Nombre_Proyecto();
        }
        /*
        Requiere: int id_proyecto
        Modifica: Se solicita el: objetivo_general, fecha_asignacion, estado, nombre_resp, telefono_resp y oficina_resp del proyecto con el id que concuerde con el ingresado por parámetro.
        Retorna: Lista de string con: objetivo_general, fecha_asignacion, estado, nombre_resp, telefono_resp y oficina_resp.
        */
        public EntidadProyecto ConsultarProyecto(int id_Proyecto)
        {

            DataTable dt = controlBD.ConsultarProyecto(id_Proyecto);
            if (dt.Rows.Count == 1)
            {
                Object[] datos = new Object[9];
                EntidadProyecto retorno;
                controlRH = new ControladoraRecursosHumanos();
                datos[0] = dt.Rows[0][0].ToString();
                datos[1] = dt.Rows[0][1].ToString();
                datos[2] = dt.Rows[0][2].ToString();
                datos[3] = dt.Rows[0][3].ToString();
                datos[4] = dt.Rows[0][4].ToString();
                datos[5] = dt.Rows[0][5].ToString();
                datos[6] = dt.Rows[0][6].ToString();
                datos[7] = dt.Rows[0][7].ToString();
                if (dt.Rows[0][9].ToString() != "")
                    datos[8] = controlRH.solicitarNombreRecurso(Int32.Parse(dt.Rows[0][9].ToString())).ToString();
                else
                    datos[8] = "";
                //datos[8] = dt.Rows[0][8].ToString();
                retorno = new EntidadProyecto(datos);
                return retorno;
            }
            else return null;
        }
        /*
        Requiere: N/A
        Modifica: Se solicita una lista del id y nombre de todos los proyectos almacenados en la BD
        Retorna: Lista con id y nombre de todos los proyectos almacenados en la BD
        */
        public DataTable ConsultarProyectoIdNombre()
        {
            controlRH = new ControladoraRecursosHumanos();
            int id = controlRH.proyectosDelLoggeado();
            if (controlRH.perfilDelLoggeado() == "Administrador")
            {
                return controlBD.ConsultarProyectoIdNombre();
            }
            else
            {
                if (id == -1)
                    return new DataTable();
                else
                    return controlBD.ConsultarProyectoIdNombre(id);
            }            
        }
        /*  Requiere: El nombre del proyecto
            Modifica: Invoca al método ConsultarIDProyecto() en controladoraBDProyecto.
            Retorna: Retorna un int con el id */
        public int ConsultarIdProyectoPorNombre(string nombre)
        {
            int retorno = -1;
           retorno = controlBD.ConsultarProyectoIdPorNombre(nombre);
            return retorno;
        }

        public int ConsultarIdProyectoPorNombre2(string nombre)
        {
            int retorno = -1;
            retorno = controlBD.ConsultarProyectoIdPorNombre(nombre);
            return retorno;
        }
        /*
        Requiere: string id_proyecto
        Modifica: Se cambia el estado del proyecto a “Cancelado”.
        Retorna: Un 0 (cero) en caso de que se haya modificado el estado correctamente, un -1 (menos uno) en caso de que se haya producido un error.
        */
        public int CancelarProyecto(string id)
        {
            int retorno = controlBD.CancelarProyecto(id);
            return retorno;
        }
        /*
        Requiere: string id_proyecto
        Modifica: Se elimina la tupla del proyecto.
        Retorna: Un 0 (cero) en caso de que se haya modificado el estado correctamente, un -1 (menos uno) en caso de que se haya producido un error.
        */
        public int EliminarProyecto(string id)
        {
            int retorno = controlBD.EliminarProyecto(id);
            return retorno;
        }

        public string solicitarNombreRecursoSinProyecto()
        {
            controlRH = new ControladoraRecursosHumanos();
            string regreso = controlRH.solicitarNombreRecursoSinProyecto();
            return regreso;
        }
        /*      
       Requiere: id del proyecto
       Modifica: N/A
       Retorna: un entero que indica si un proyecto esta siendo usado por alguien mas (0=no 1=si)
       */
        public int ConsultarUsoProyecto(int id)
        {
            int retorno = 0;

            if (modificados.ContainsKey(id))
                retorno = 1;
            return retorno;
        }
        /*      
       Requiere: id del proyecto, int de uso
       Modifica: mediante la controladora de BD modifica el estado del uso segun el id
       Retorna: N/A
       */
        public void UpdateUsoProyecto(int id, int use)
        {
             controlBD.UpdateUsoProyecto(id, use);
        }
        /*      
       Requiere: N/A
       Modifica: Usa la controladora de RH
       Retorna: un booleano que indica si esta loggeado un Admin o un Miembro de equipo
       */
        public bool PerfilDelLogeado()
        {
            controlRH = new ControladoraRecursosHumanos();
            bool retorno;
            string perfil = controlRH.perfilDelLoggeado();
            if (perfil == "Administrador")
                retorno = true;
            else
                retorno = false;
            return retorno;

        }
        /*      
       Requiere: id del proyecto
       Modifica: N/A
       Retorna: el string con el nombre de un proyecto en particular dado su id
       */
        public string ConsultarNombreProyectoPorId(int id)

        {
            string retorno = controlBD.ConsultarNombreProyectoPorId(id);
            return retorno;
        }
        /*      
       Requiere: int es el id_proyecto y string id es la cedula de la persona
       Modifica: lleva un registro de quien está modificando qué proyecto
       Retorna: N/A
       */
        public void AgregarModificacion(int i)
        {

            modificados.Add(i, IdLogeado());
            UpdateUsoProyecto(i, 1);

        }
        /*      
       Requiere: el id del proyecto
       Modifica: Libera el proyecto, cambia su uso=0
       Retorna: N/A
       */
        public void QuitarEliminacion(int i)
        {
            modificados.Remove(i);
            UpdateUsoProyecto(i, 0);

        }
        /*      
       Requiere: N/A
       Modifica: N/A
       Retorna: un string con la cedula del usuario logeado
       */
        public string IdLogeado()
        {
            controlRH = new ControladoraRecursosHumanos();
            return controlRH.idDelLoggeado().ToString();
        }
        /*      
       Requiere: N/A
       Modifica: Elimina las modificaciones hechas y no terminadas de un usuario al cual le expiro la sesion
       Retorna: N/A
       */
        public void LimpiarModificaciones(string nombre)
        {
            modificados.ToString();
            
            foreach (var item in modificados.Where(kvp => kvp.Value == nombre).ToList())
            {
                modificados.Remove(item.Key);
                UpdateUsoProyecto(item.Key, 0);
            }
        }

        public String consultarProyectosConCaso()
        {
            return controlBD.consultarProyectosConCaso();
        }

    }
}