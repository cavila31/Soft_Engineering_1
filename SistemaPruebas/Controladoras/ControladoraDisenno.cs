using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SistemaPruebas.Controladoras
{
    public class ControladoraDisenno
    {
        ControladoraBDDisenno controlBD;
        ControladoraProyecto controlProyecto = new ControladoraProyecto();
        ControladoraRecursosHumanos controlRH = new ControladoraRecursosHumanos();
        ControladoraRequerimiento controlReq = new ControladoraRequerimiento();
        public ControladoraDisenno()
        {
            controlBD = new ControladoraBDDisenno();            
        }

        /*
        Requiere: Object [ ] datos con los datos del diseño de prueba
        Modifica: Inserta un Diseño de proyecto, al enviarle a la controladora de bases de datos una entidad del diseño con los datos generales junto con sus proyectos asociados uno por uno.
        Retorna: int.
        */
        public int ingresaDiseno(object[] datos)
        {
            EntidadDisenno objDisenno = new EntidadDisenno(datos);
            int a = controlBD.InsertarDiseno(objDisenno);
            return a;
        }


        public String solicitarProyectos()
        {
            
            String proyectos = controlProyecto.Consultar_ID_Nombre_Proyecto();
            return proyectos;

        }

        public String solicitarNombreProyectoMiembro(int id_proyecto)
        {

            String proyectos = controlProyecto.ConsultarNombreProyectoPorId(id_proyecto);
            return proyectos;

        }


        public String solicitarResponsanles(int id_proyecto)
        {
            String responsables = controlRH.solicitarNombreRecursoPorProyecto(id_proyecto);
            return responsables;

        }

        public int solicitarProyecto_Id(string nomb_proyecto)
        {
            int proyectos = controlProyecto.ConsultarIdProyectoPorNombre2(nomb_proyecto);
            return proyectos;

        }

        public int solicitarResponsableCedula(string responsable)
        {
            int cedula = controlRH.solicitarCedulaRecurso(responsable);               
            return cedula;

        }

        public string solicitarNombreResponsable(int cedula)
        {
            return controlRH.solicitarNombreRecurso(cedula);
        }

        public bool loggeadoEsAdmin()
        {
            return controlRH.loggeadoEsAdmin();
        }

        public int solicitarProyecto_IdMiembro()
        {
            return controlRH.proyectosDelLoggeado();

        }

        public DataTable consultarReqNoenDiseno(int id_proyecto, int id_diseno)
        {
            return controlReq.consultarRequerimientoNoEnDiseno(id_proyecto, id_diseno);
        }

        public DataTable consultarReqEnDiseno(int id_proyecto, int id_diseno)
        {
            return controlReq.consultarRequerimientoEnDiseno(id_proyecto, id_diseno);
        }

        /*
        Requiere: id del diseño a consultar.
        Modifica: N/A.
        Retorna: Entidad disenno.
        */
        public EntidadDisenno consultarDisenno(int id_diseno)
        {
            DataTable dt = controlBD.consultarDisennoBD(1, id_diseno);
            if (dt.Rows.Count == 1)
            {
                Object[] datos = new Object[9];
                EntidadDisenno retorno;


                datos[0] = dt.Rows[0][0].ToString();
                datos[1] = dt.Rows[0][1].ToString();
                datos[2] = dt.Rows[0][2].ToString();
                datos[3] = dt.Rows[0][3].ToString();
                datos[4] = dt.Rows[0][4].ToString();
                datos[5] = dt.Rows[0][5].ToString();
                datos[6] = dt.Rows[0][6].ToString();
                datos[7] = dt.Rows[0][7].ToString();
                datos[8] = dt.Rows[0][8].ToString();
                retorno = new EntidadDisenno(datos);
                return retorno;
            }
            else return null;
        }

        /*
        Requiere: id del diseño a consultar.
        Modifica: N/A.
        Retorna: DataTable.
        */
        public DataTable consultarDisenoGrid(int id_proyecto)
        {
            return controlBD.consultarDisennoBD(2, id_proyecto);
        }

        /*
       Requiere: id del diseño a consultar.
       Modifica: N/A.
       Retorna: int.
       */
        public int consultarId_Disenno(String proposito)
        {
            return controlBD.consultarId_Disenno(proposito);
        }

        public String solicitarPropositoDiseno(int idProyecto)
        {
            return controlBD.solicitarPropositoDiseno(idProyecto);
        }

        /*
        Requiere: Object [ ] datos con los datos del diseño de prueba y una ListaRequerimientos req que tiene todos los requerimientos de este Diseño.
        Modifica: Modifica un Diseño de proyecto, al enviarle a la controladora de bases de datos una entidad del diseño con los datos generales y también uno por uno los requerimientos.
        Retorna: N/A.
        */
        public int modificarDiseno(int id_diseno, object[] datos)
        {
            EntidadDisenno objDisenno = new EntidadDisenno(datos);
            return controlBD.modificarDisennoBD(objDisenno, id_diseno);

        }

        /*
        Requiere: idDiseño del diseño a eliminar.
        Modifica: Elimina un diseño de pruebas junto con sus requerimientos asociados.
        Retorna: id.
        */
        public int eliminarDisenno(int id_diseno)
        {
            return controlBD.eliminarDisennoBD(id_diseno);
        }

        public List<string> infoDisenno()
        {
            SistemaPruebas.Intefaces.InterfazDiseno i = new SistemaPruebas.Intefaces.InterfazDiseno();
            return i.infoDisenno();
        }

        public DataTable consultarDisennoReq(int idDisenno)
        {
            return controlBD.consultarDisennoReq(idDisenno);
        }

        public int insertarDisennoReq(object []datos)
        {
             SistemaPruebas.Entidades.EntidadDisennoReq entidad = new Entidades.EntidadDisennoReq(datos);
             return controlBD.InsertarDisenoReq(entidad);

        }

        public int eliminarDisennoReq(int idDisenno, string idReg)
        {
            return controlBD.EliminarDisennoReq(idDisenno, idReg);
        }
    }
}