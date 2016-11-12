using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPruebas.Controladoras
{
    public class EntidadEjecucionPrueba
    {
        /* 
         * Variables correspondientes a la entidad EjecucionPrueba
         */
        private String fecha;
        private String responsable;
        private String incidencias;
        private int id_disenno;
        private String fechaConsulta;

        /*
         * Requiere: Recibir un objeto con los datos de todos atributos de una EjecucionPrueba
         * Modifica: Encapsula los datos recibidos
         * Retorna: N/A
        */
        public EntidadEjecucionPrueba(Object[] datos)
        {
            this.fecha = datos[0].ToString();
            this.responsable = datos[1].ToString();
            this.incidencias = datos[2].ToString();
            this.id_disenno = Convert.ToInt32(datos[3]);
            this.fechaConsulta = datos[4].ToString();
        }

        /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo fecha
        * Modifica: N/A
        * Retorna: el valor del atributo fecha en un string
        * set();
        * Requiere: el atributo fecha
        * Modifica: el valor del atributo fecha
        * Retorna: N/A
        */
        public String Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo responsable
        * Modifica: N/A
        * Retorna: el valor del atributo responsable en un string
        * set();
        * Requiere: el atributo responsable
        * Modifica: el valor del atributo responsable
        * Retorna: N/A
        */
        public String Responsable
        {
            get { return responsable; }
            set { responsable = value; }
        }

        /*
         * Implementación de los metodos get() y set() de este atributo
         * get();
         * Requiere: el atributo incidencias
         * Modifica: N/A
         * Retorna: el valor del atributo incidencias en un string
         * set();
         * Requiere: el atributo incidencias
         * Modifica: el valor del atributo incidencias
         * Retorna: N/A
         */
        public String Incidencias
        {
            get { return incidencias; }
            set { incidencias = value; }
        }


        /*
         * Implementación de los metodos get() y set() de este atributo
         * get();
         * Requiere: el atributo Id_disenno
         * Modifica: N/A
         * Retorna: el valor del atributo Id_disenno en un int
         * set();
         * Requiere: el atributo Id_disenno
         * Modifica: el valor del atributo Id_disenno
         * Retorna: N/A
         */
        public int Id_disenno
        {
            get { return id_disenno; }
            set { id_disenno = value; }
        }

        /*
         * Implementación de los metodos get() y set() de este atributo
         * get();
         * Requiere: el atributo de fecha consultado
         * Modifica: N/A
         * Retorna: el valor del atributo fecha consultado en un string
         * set();
         * Requiere: el atributo fecha consultado
         * Modifica: el valor del atributo fecha consultado
         * Retorna: N/A
         */
        public string FechaConsulta
        {
            get { return fechaConsulta; }
            set { fechaConsulta = value; }
        }
    }
}