using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace SistemaPruebas.Controladoras
{
    public class EntidadNoConformidad
    {
        /* 
          * Variables correspondientes a la entidad No Conformidad
          */
        private int id_noConformidad;
        private String tipo;
        private String caso;
        private String descripcion;
        private String justificacion;
        private byte[] imagen;
        private String estado;
        private String id_ejecucion;

        /*
         * Requiere: Recibir un objeto con los datos de todos atributos de una No Conformidad
         * Modifica: Encapsula los datos recibidos
         * Retorna: N/A
        */
        public EntidadNoConformidad(Object[] datos)
        {
            this.tipo = datos[0].ToString();
            this.caso = datos[1].ToString();
            this.descripcion = datos[2].ToString();
            this.justificacion = datos[3].ToString();
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, datos[4]);
                this.imagen = ms.ToArray();
            }
            this.estado = datos[5].ToString();
            this.id_ejecucion = datos[6].ToString();
            this.id_noConformidad = Convert.ToInt32(datos[7]);

        }

        /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo id_noConformidad
        * Modifica: N/A
        * Retorna: el valor del atributo id_noConformidad en un int
        * set();
        * Requiere: el atributo id_noConformidad
        * Modifica: el valor del atributo id_noConformidad
        * Retorna: N/A
        */
        public int Id_noConformidad
        {
            get { return id_noConformidad; }
            set { id_noConformidad = value; }
        }

        /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo tipo
        * Modifica: N/A
        * Retorna: el valor del atributo tipo en un string
        * set();
        * Requiere: el atributo tipo
        * Modifica: el valor del atributo tipo
        * Retorna: N/A
        */
        public String Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }


        public String Caso
        {
            get { return caso; }
            set { caso = value; }
        }

        /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo descripcion
        * Modifica: N/A
        * Retorna: el valor del atributo descripcion en un string
        * set();
        * Requiere: el atributo descripcion
        * Modifica: el valor del atributo descripcion
        * Retorna: N/A
        */
        public String Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }


        /*
         * Implementación de los metodos get() y set() de este atributo
         * get();
         * Requiere: el atributo justificacion
         * Modifica: N/A
         * Retorna: el valor del atributo justificacion en un string
         * set();
         * Requiere: el atributo justificacion
         * Modifica: el valor del atributo justificacion
         * Retorna: N/A
         */
        public String Justificacion
        {
            get { return justificacion; }
            set { justificacion = value; }
        }

        /* 
         * Implementación de los metodos get() y set() de este atributo
         * get();
         * Requiere: el atributo imagen
         * Modifica: N/A
         * Retorna: el valor del atributo imagen en un string
         * set();
         * Requiere: el atributo imagen
         * Modifica: el valor del atributo imagen
         * Retorna: N/A
         */
        public byte[] Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }

        /*
         * Implementación de los metodos get() y set() de este atributo
         * get();
         * Requiere: el atributo id_ejecucion
         * Modifica: N/A
         * Retorna: el valor del atributo id_ejecucion en un int
         * set();
         * Requiere: el atributo id_ejecucion
         * Modifica: el valor del atributo id_ejecucion
         * Retorna: N/A
         */
        public String Id_ejecucion
        {
            get { return id_ejecucion; }
            set { id_ejecucion = value; }
        }

        public String Estado
        {
            get { return estado; }
            set { estado = value; }
        }
    }
}