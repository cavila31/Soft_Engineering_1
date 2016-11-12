using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaPruebas.Entidades
{
    public class EntidadDisennoReq
    {

        private int idPrueba;
        private string idReq;
        private int idDisenno;

        public EntidadDisennoReq (object []datos)
        {
            this.idPrueba= Int32.Parse(datos[0].ToString());
            this.idReq = datos[1].ToString();
            this.idDisenno = Int32.Parse(datos[2].ToString());
        }


        public int IdPrueba
        {
            get { return idPrueba; }
            set { idPrueba = value; }
        }
        public string IdReq
        {
            get { return idReq; }
            set { idReq = value; }
        }
        public int IdDisenno
        {
            get { return idDisenno; }
            set { idDisenno = value; }
        }

    }
}