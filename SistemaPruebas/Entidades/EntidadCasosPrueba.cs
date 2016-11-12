using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPruebas.Controladoras
{
    public class EntidadCasosPrueba
    {
       /* 
        * Variables correspondientes a la entidad cliente
        */
        private String id_caso_prueba;
        private String proposito;
        private String entrada_datos;
        private String resultado_esperado;
        private String flujo_central;
        private int id_disenno;
        private String idConsulta;

        /*
         * Requiere: Recibir un objeto con los datos de todos atributos de un Caso de Pruebas
         * Modifica: Encapsula los datos recibidos
         * Retorna: N/A
        */
        public EntidadCasosPrueba(Object[] datos)
        {  
            this.id_caso_prueba = datos[0].ToString();
            this.proposito = datos[1].ToString();
            this.entrada_datos = datos[2].ToString();
            this.resultado_esperado = datos[3].ToString();
            this.flujo_central = datos[4].ToString();
            this.id_disenno =  Convert.ToInt32(datos[5].ToString());
            this.idConsulta = datos[6].ToString();
        }

        /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo Id_caso_prueba
        * Modifica: N/A
        * Retorna: el valor del atributo Id_caso_prueba en un int
        * set();
        * Requiere: el atributo Id_caso_prueba
        * Modifica: el valor del atributo Id_caso_prueba
        * Retorna: N/A
        */
        public String Id_caso_prueba
        {
            get { return id_caso_prueba; }
            set { id_caso_prueba = value; }
        }

        /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo Proposito
        * Modifica: N/A
        * Retorna: el valor del atributo Proposito en un string
        * set();
        * Requiere: el atributo Proposito
        * Modifica: el valor del atributo Proposito
        * Retorna: N/A
        */
        public String Proposito
        {
            get { return proposito; }
            set { proposito = value; }
        }

        /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo Entrada_datos
        * Modifica: N/A
        * Retorna: el valor del atributo Entrada_datos en un string
        * set();
        * Requiere: el atributo Entrada_datos
        * Modifica: el valor del atributo Entrada_datos
        * Retorna: N/A
        */
        public String Entrada_datos
        {
            get { return entrada_datos; }
            set { entrada_datos = value; }
        }

       /*
        * Implementación de los metodos get() y set() de este atributo
        * get();
        * Requiere: el atributo Resultado_esperado
        * Modifica: N/A
        * Retorna: el valor del atributo Resultado_esperado en un string
        * set();
        * Requiere: el atributo Resultado_esperado
        * Modifica: el valor del atributo Resultado_esperado
        * Retorna: N/A
        */
        public String Resultado_esperado
        {
            get { return resultado_esperado; }
            set { resultado_esperado = value; }
        }

        /* 
         * Implementación de los metodos get() y set() de este atributo
         * get();
         * Requiere: el atributo Flujo_central
         * Modifica: N/A
         * Retorna: el valor del atributo Flujo_central en un string
         * set();
         * Requiere: el atributo Flujo_central
         * Modifica: el valor del atributo Flujo_central
         * Retorna: N/A
         */
        public String Flujo_central
        {
            get { return flujo_central; }
            set { flujo_central = value; }
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
        * Requiere: el atributo IdConsulta
        * Modifica: N/A
        * Retorna: el valor del atributo IdConsulta 
        * set();
        * Requiere: el atributo IdConsulta
        * Modifica: el valor del atributo IdConsulta
        * Retorna: N/A
        */
        public String IdConsulta
        {
            get { return idConsulta; }
            set { idConsulta = value; }
        }
    }
}