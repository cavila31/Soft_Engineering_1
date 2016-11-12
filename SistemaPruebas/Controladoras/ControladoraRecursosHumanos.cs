using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraRecursosHumanos
    {
        ControladoraBDRecursosHumanos controladoraBDRecursosHumanos = new ControladoraBDRecursosHumanos();

        ControladoraProyecto controladoraProyecto = new ControladoraProyecto();

        /*
         * Requiere: Los datos de Nombre de Usuario y la contraseña ingresada para un usuario específico.
         * Modifica: Manda los datos a la Controladora Base de Datos para hacer el cambio
           sobre el estado de sesión abierta de un usuario. Regresa la confirmación del loggeo.
         * Retorna: booleano.
         */
        public bool usuarioMiembroEquipo(Object[] datos)
        {
            string nombres = controladoraBDRecursosHumanos.nombresContrasenas();

            if (nombres != null)
            {
                string nombreIngresado = datos[0].ToString();
                string contrasenaIngresada = datos[1].ToString();

                if (nombres.Contains(nombreIngresado)) //en caso de que si sea un nombre de usuario válido. Ahora se verifica si la contraseña ingresada pertenece a ese nombre
                {
                    String cont = controladoraBDRecursosHumanos.consultarContrasena(nombreIngresado);

                    if (contrasenaIngresada == cont)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
         * Requiere: El conjunto de Datos de un usuario,
           incluyendo el nombre del usuario y la contraseñapor la que va a cambiar.
         * Modifica: Hace el llamado al método que hará el cambio en la base de datos
           para la contraseña asociada a un usuario. Esto después de validar los datos de un usuario,
           con la contraseña anterior.
         * Retorna: booleano.
         */
        public bool modificaContrasena(Object[] datos)
        {
            return controladoraBDRecursosHumanos.modificaContrasena(datos[0].ToString(), datos[1].ToString());
        }

        /*
         * Requiere: El nombre de un usuario.
         * Modifica: Se hace el llamado a la Controladora de la Base de Datos correspondiente,
           de manera que se pueda ver si un usuario está loggeado, según la información guardada.
         * Retorna: booleano.
         */
        public bool loggeado(string nombre)
        {
            return controladoraBDRecursosHumanos.loggeado(nombre);
        }

        /*
         * Requiere: El nombre de usuario y el estado de sesión abierta o cerrada que va a cambiarse.
         * Modifica: Se hace el cambio en el estado de loggeo para un usuario,
           llamando al método que modifica los datos dentro de la base de datos.
         * Retorna: booleano.
         */
        public bool estadoLoggeado(string nombre, string estado)
        {
            if (estado == "0")
                controladoraProyecto.LimpiarModificaciones(nombre);
            return controladoraBDRecursosHumanos.estadoLoggeado(nombre, estado);
        }

        /*
         * Requiere: N/A.
         * Modifica: Hace el llamado al método que accede a la base de datos para regresar
           los proyectos de quien tiene sesión abierta actualmente.
         * Retorna: número.
         */
        public int proyectosDelLoggeado()
        {
            return controladoraBDRecursosHumanos.proyectosDelLoggeado(Account.Login.id_logeado);
        }

        /*
         * Requiere: N/A.
         * Modifica: Hace el llamado al método que accede a la base de datos para regresar
           la cédula de quien tiene sesión abierta actualmente.
         * Retorna: número.
         */
        public int idDelLoggeado()
        {
            return controladoraBDRecursosHumanos.idDelLoggeado(Account.Login.id_logeado);
        }

        /*
         * Requiere: N/A.
         * Modifica: Hace el llamado al método que accede a la base de datos para regresar
           el perfil de quien tiene sesión abierta actualmente.
         * Retorna: hilera.
         */
        public string perfilDelLoggeado()
        {
            return controladoraBDRecursosHumanos.perfilDelLoggeado(Account.Login.id_logeado);
        }

        /*
         *Requiere:  N/A.
         *Modifica: Hace llamado a método que consulta si la persona loggeada
          es un administrador, según el sistema y los datos dentro de él, o no.
         *Retorna: booleano.
        */
        public bool loggeadoEsAdmin()
        {
            bool retorno;
            string perfil = this.perfilDelLoggeado();
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


        /**/

        /*
         *Requiere: Cedula del recurso humano.
         *Modifica: Hace llamado al método que accede a la base de datos
          para hacer confirmación del uso del Recurso Humano.
          Regresa verdadero si está en uso o falso si no.
         *Retorna: booleano.
         */
        public bool ConsultarUsoRH(int id)		
        {		
            return controladoraBDRecursosHumanos.ConsultarUsoRH(id);		
        }

        /*
         *Requiere:  Cedula del recurso humano y el estado del Uso actual.
         *Modifica: Se encarga de hacer el llamado al método que accede a la
          base de datos para cambiar el uso asociado al número de cédula.
         *Retorna: entero.
         */
        public int UpdateUsoRH(int id, int use)		
        {		
            return controladoraBDRecursosHumanos.UpdateUsoRH(id, use);		
        }

        /**/

        /*
         * Requiere: Object[] datos
         * Modifica: Inserta un nuevo recurso humano en el sistema.
         * Retorna: int.
         */
        public int insertarRecursoHumano(Object[] datos)
        {
            EntidadRecursosHumanos recursoHumano = new EntidadRecursosHumanos(datos);
            int ret = controladoraBDRecursosHumanos.insertarRecursoHumanoBD(recursoHumano);
            return ret;
        }

        /*
         * Requiere: Object[] datos
         * Modifica: Modifica un recurso humano en el sistema.
         * Retorna: int.
         */
        public int modificarRecursoHumano(Object[] datos)
        {
            EntidadRecursosHumanos recursoHumano = new EntidadRecursosHumanos(datos);
            int ret = controladoraBDRecursosHumanos.modificarRecursoHumanoBD(recursoHumano);
            return ret;
        }

        /*
         * Requiere: Cédula
         * Modifica: Elimina un recurso humano del sistema.
         * Retorna: int.
         */
        public int eliminarRecursoHumano(int cedula)
        {
            int ret = controladoraBDRecursosHumanos.eliminarRecursoHumanoBD(cedula);
            return ret;
        }

        /*
         * Requiere: tipo de consulta y cédula.
         * Modifica: N/A.
         * Retorna: DataTable.
         */
        public DataTable consultarRecursoHumano(int tipo, int cedula)
        {
            DataTable dt = controladoraBDRecursosHumanos.consultarRecursoHumanoBD(tipo, cedula);
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
         * Requiere: ID del proyecto al que pertenece.
         * Modifica: N/A.
         * Retorna: String.
         */
        public String solicitarNombreRecursoPorProyecto(int id_proyecto)
        {
            String proyecto = controladoraBDRecursosHumanos.solicitarNombreRecursoPorProyecto(id_proyecto);
           return proyecto;
        }

        /*
         * Requiere: Solicitar el nombre de las personas que no tienen un proyecto asociado.
         * Modifica: N/A.
         * Retorna: String.
         */
        public String solicitarNombreRecursoSinProyecto()
        {
            String proyecto = controladoraBDRecursosHumanos.solicitarNombreRecursoSinProyecto();
            return proyecto;
        }

        /*
        * Requiere: Nombre de la persona.
        * Modifica: N/A.
        * Retorna: Int (el id).
        */
        public int solicitarCedulaRecurso(string nombre)
        {
            return controladoraBDRecursosHumanos.solicitarCedulaRecurso(nombre);
        }

        /*
        * Requiere: Cedula de la persona.
        * Modifica: N/A.
        * Retorna: String nombre de las personas.
        */
        public string solicitarNombreRecurso(int cedula)
        {
            return controladoraBDRecursosHumanos.solicitarNombreRecurso(cedula);
        }

        /*
        * Requiere: Cedula de la persona y proyecto que se le quiere asociar.
        * Modifica: N/A.
        * Retorna: Resultado de la operacion.
        */
        public int addProyecto(int cedula, int id_proyecto)
        {
            return controladoraBDRecursosHumanos.addProyecto(cedula, id_proyecto);
        }

        public string solicitarNombreRecursosPorProyecto(int id)
        {
            return controladoraBDRecursosHumanos.solicitarNombreRecursosPorProyecto(id);
        }
        public DataTable consultarMiembrosProyecto(String nombProy)
        {
            return controladoraBDRecursosHumanos.consultarMiembrosProyecto(nombProy);
                //acceso.ejecutarConsultaTabla("Select nombre_completo from Recurso_Humano R join Proyecto P  on R.id_proyecto =P.id_proyecto where P.nombre_sistema = " + nombProy);
        }

    }
}