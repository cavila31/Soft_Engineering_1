using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaPruebas;
using SistemaPruebas.Controladoras;
using System.Collections.Generic;

namespace PruebasReq
{
    [TestClass]
    public class UnitTest1
    {
      
        [TestMethod]
        public void TestInsercionExitosa()
        {
            ControladoraEjecucionPrueba controlEjecucion = new ControladoraEjecucionPrueba();
            bool esperado = true;
            Object[] datos = new Object[5];
            datos[0]="12/2/2015";
            datos[1]="Caro";
            datos[2]="";
            datos[3]="1";
            datos[4]="";

            List<Object[]> listaNC =new List<Object[]>();
            Object[] noConformidad = new Object[8];
            noConformidad[0] = "Errores de usabilidad";
            noConformidad[1] = "Curso-RQ1-1";
            noConformidad[2] = "Una descripcion";
            noConformidad[3] = "Una justificacion";
            byte[] imgbyte = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; ;
            noConformidad[4] = imgbyte;
            noConformidad[5] = "Fallido";
            noConformidad[7] = "0";
            listaNC.Add(noConformidad);

            string resultado = controlEjecucion.insertarEjecucion(datos, listaNC);
            Assert.AreNotEqual("-", resultado);
        }

        [TestMethod]
        public void TestModificacinExitosa()
        {
            ControladoraEjecucionPrueba controlEjecucion = new ControladoraEjecucionPrueba();
            Object[] datos = new Object[5];
            datos[0] = "12/2/2015";
            datos[1] = "Ricardo Muñoz";
            datos[2] = "";
            datos[3] = "1";
            datos[4] = "12/2/2015";

            List<Object[]> listaNC = new List<Object[]>();
            Object[] noConformidad = new Object[8];
            noConformidad[0] = "Errores de usabilidad";
            noConformidad[1] = "Curso-RQ1-1";
            noConformidad[2] = "Una descripcion";
            noConformidad[3] = "Una justificacion";
            byte[] imgbyte = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; ;
            noConformidad[4] = imgbyte;
            noConformidad[5] = "Fallido";
            noConformidad[7] = "0";
            listaNC.Add(noConformidad);

            string resultado = controlEjecucion.modificarEjecucion(datos, listaNC);
            Assert.AreNotEqual("-", resultado);
        }

        [TestMethod]
        public void TestModificacionFalla()
        {
            ControladoraEjecucionPrueba controlEjecucion = new ControladoraEjecucionPrueba();
            Object[] datos = new Object[5];
            datos[0] = "12/2/2015";
            datos[1] = "Ricardo Muñoz";
            datos[2] = "";
            datos[3] = "1";
            datos[4] = "1/15/2016";     //repetido

            List<Object[]> listaNC = new List<Object[]>();
            Object[] noConformidad = new Object[8];
            noConformidad[0] = "Errores de usabilidad";
            noConformidad[1] = "Curso-RQ1-1";
            noConformidad[2] = "Una descripcion";
            noConformidad[3] = "Una justificacion";
            byte[] imgbyte = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; ;
            noConformidad[4] = imgbyte;
            noConformidad[5] = "Fallido";
            noConformidad[7] = "0";
            listaNC.Add(noConformidad);

            string resultado = controlEjecucion.modificarEjecucion(datos, listaNC);
            Assert.AreEqual("-", resultado);
        }

        [TestMethod]
        public void TestEliminacionExitosa()
        {
            ControladoraEjecucionPrueba controlEjecucion = new ControladoraEjecucionPrueba();
            int resultado = controlEjecucion.eliminarEjecucionPrueba("11/1/2015");
            Assert.AreEqual(1, resultado);


        }

        [TestMethod]
        public void TestEliminacionFalla()
        {
            ControladoraEjecucionPrueba controlEjecucion = new ControladoraEjecucionPrueba();
            int resultado = controlEjecucion.eliminarEjecucionPrueba("11/1/2015");
            Assert.AreNotEqual(1, resultado);


        }
    }
}
