using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPruebas.Controladoras
{

    public class EntidadRecursosHumanos
    {
        private int cedula;
        private String nombre_completo;
        private String tel1;
        private String tel2;
        private String correo_electronico;
        private String usuario;
        private String clave;
        private String perfilAcceso;
        private String proyAsociado;
        private String rol;
        private int cedulaConsulta;



        public EntidadRecursosHumanos(Object[] datos)
        {
            this.cedula = Convert.ToInt32(datos[0].ToString());
            this.nombre_completo = datos[1].ToString();
            this.tel1 = datos[2].ToString();
            this.tel2 = datos[3].ToString();
            this.correo_electronico = datos[4].ToString();
            this.usuario = datos[5].ToString();
            this.clave = datos[6].ToString();
            this.perfilAcceso = datos[7].ToString();
            this.proyAsociado = datos[8].ToString();
            this.rol = datos[9].ToString();
            this.cedulaConsulta = Convert.ToInt32(datos[10].ToString());
        }

        //Metodos set y get para la variable Cedula
        public int Cedula
        {
            get { return cedula; }
            set { cedula = value; }
        }

        //Metodos set y get para la variable CedulaVieja
        public int CedulaVieja
        {
            get { return cedulaConsulta; }
            set { cedula = value; }

        }

        //Metodos set y get para la variable Nombre_Completo
        public String Nombre_Completo
        {
            get { return nombre_completo; }
            set { nombre_completo = value; }
        }

        //Metodos set y get para la variable Tel1
        public String Tel1
        {
            get { return tel1; }
            set { tel1 = value; }
        }

        //Metodos set y get para la variable Tel2
        public String Tel2
        {
            get { return tel2; }
            set { tel2 = value; }
        }

        //Metodos set y get para la variable Correo
        public String Correo
        {
            get { return correo_electronico; }
            set { correo_electronico = value; }
        }

        //Metodos set y get para la variable Usuario
        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        //Metodos set y get para la variable Clave
        public String Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        //Metodos set y get para la variable PerfilAcceso
        public String PerfilAcceso
        {
            get { return perfilAcceso; }
            set { perfilAcceso = value; }
        }

        //Metodos set y get para la variable ProyAsociado
        public String ProyAsociado
        {
            get { return proyAsociado; }
            set { proyAsociado = value; }
        }

        //Metodos set y get para la variable Rol
        public String Rol
        {
            get { return rol; }
            set { rol = value; }
        }
    }


}