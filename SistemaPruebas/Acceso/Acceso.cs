using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SistemaPruebas.Acceso
{
    public class Acceso
    {


        String conexion = "Data Source=eccibdisw; Initial Catalog=g2inge; Integrated Security=SSPI";
        //String conexion = "Data Source=LENOVO-PC\\SQLEXPRESS; Initial Catalog=inge1g2; Integrated Security=SSPI";
        //string conexion = "Data Source=(localdb)\\SQLOne; Initial Catalog=Sistema_Pruebas; Integrated Security=SSPI";
        //String conexion = "Data Source=RICARDO;Initial Catalog=PruebaInge;Integrated Security=True";
        
        public DataTable ejecutarConsultaTabla(String consulta)
        {
            SqlConnection sqlConnection = new SqlConnection(conexion);
            DataTable table = new DataTable();
            try
            {
                sqlConnection.Open();

                SqlCommand comando = new SqlCommand(consulta, sqlConnection);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(comando);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                //DataTable table = new DataTable();

                dataAdapter.Fill(table);
                try
                {
                    sqlConnection.Close();
                }
                catch (SqlException e)
                {
                    string mensajeError = e.ToString();
                    throw new Exception("Error al cerrar la conexión con la base de datos. " + e.Message);
                }
            }
            catch (SqlException e)
            {
                string mensajeError = e.ToString();
                //throw new Exception("Error al conectarse a la base de datos. " + e.Message);
                Console.WriteLine(e.Message);
            }
            return table;
        }

        public int Insertar(string consulta)
        {
            int a = -1;
            SqlConnection sqlConnection = new SqlConnection(conexion);
            try
            {
                sqlConnection.Open();


                SqlCommand comando = null;

                try
                {
                    comando = new SqlCommand(consulta, sqlConnection);
                    a = comando.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    string mensajeError = ex.ToString();

                    if (ex.Number == 2627)//Violación de llave primaria al insertar
                    {
                        a = ex.Number;
                    }

                    //throw new Exception("Error al insertar. " + ex.Message);

                    // System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=""JavaScript"">alert("Hello this is an Alert")</SCRIPT>")               
                }

                try
                {
                    sqlConnection.Close();
                }
                catch (SqlException e)
                {
                    string mensajeError = e.ToString();
                    throw new Exception("Error al cerrar la conexión con la base de datos. " + e.Message);

                    //    MessageBox.Show(mensajeError);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            return a;
        }


        public int Insertar_Proced_Almacenado(SqlCommand comando)

        {

            SqlConnection sqlConnection = new SqlConnection(conexion);
            int a = -1;
            try
            {
                sqlConnection.Open();



                comando.Connection = sqlConnection;

                try
                {
                    a = comando.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    //string mensajeError = ex.ToString();
                    string mensajeError = e.ToString();

                    if (e.Number == 2627)//Violación de llave primaria al insertar
                    {
                        a = e.Number;
                    }
                    a = e.Number;
                }

                try
                {
                    sqlConnection.Close();
                }
                catch (SqlException e)
                {
                    
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }

            return a;
        }


        public SqlDataReader Consultar(String consulta)
        {
            SqlConnection sqlConnection = new SqlConnection(conexion);
            SqlDataReader datos = null;
            try
            {
                sqlConnection.Open();

                //SqlDataReader datos = null;
                SqlCommand comando = null;

                try
                {
                    comando = new SqlCommand(consulta, sqlConnection);
                    datos = comando.ExecuteReader();
                }
                catch (SqlException ex)
                {
                    string mensajeError = ex.ToString();
                    throw new Exception("Error al consultar. " + ex.Message);

                    // System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=""JavaScript"">alert("Hello this is an Alert")</SCRIPT>")                            
                }

                try
                {
                    sqlConnection.Close();
                }
                catch (SqlException e)
                {
                    string mensajeError = e.ToString();
                    throw new Exception("Error al cerrar la conexión con la base de datos. " + e.Message);

                    //    MessageBox.Show(mensajeError);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            return datos;
        }

        public string Consultar_Proced_Almacenado(SqlCommand comando)
        {
            SqlConnection sqlConnection = new SqlConnection(conexion);
            string retorno = "";

            try
            {
                sqlConnection.Open();
                comando.Connection = sqlConnection;


                try
                {
                    SqlDataReader DR2 = comando.ExecuteReader();
                    while (DR2.Read())
                    {
                        if (retorno != "")
                            retorno += ";";
                        retorno += DR2[0].ToString();
                    }
                }
                catch (SqlException ex)
                {
                    string mensajeError = ex.ToString();
                    throw new Exception("Error al consultar. " + ex.Message);
                }

                try
                {
                    sqlConnection.Close();
                }
                catch (SqlException e)
                {
                    string mensajeError = e.ToString();
                    throw new Exception("Error al cerrar la conexión con la base de datos. " + e.Message);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            return retorno;
        }
        public int Update(string consulta)
        {
            int a = -1;
            SqlConnection sqlConnection = new SqlConnection(conexion);
            try
            {
                sqlConnection.Open();


                //try
                //{
                //    comando = new SqlCommand(consulta, sqlConnection);                
                //    a = comando.ExecuteNonQuery();
                //}
                //catch (SqlException ex)
                //{
                //    string mensajeError = ex.ToString();
                //    MessageBox.Show(mensajeError);
                //}

                //try
                //{
                //    sqlConnection.Close();
                //}
                //catch (SqlException e)
                //{
                //    string mensajeError = e.ToString();
                //    MessageBox.Show(mensajeError);
                //}
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            return a;

        }

        public int EliminarProyecto(string consulta)
        {
            int a = -1;
            SqlConnection sqlConnection = new SqlConnection(conexion);
            try
            {
                sqlConnection.Open();


                SqlCommand comando = null;

                try
                {
                    comando = new SqlCommand(consulta, sqlConnection);
                    a = comando.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    string mensajeError = ex.ToString();
                    throw new Exception("Error al cancelar el proyecto. " + ex.Message);

                    // System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=""JavaScript"">alert("Hello this is an Alert")</SCRIPT>")               
                }

                try
                {
                    sqlConnection.Close();
                }
                catch (SqlException e)
                {
                    string mensajeError = e.ToString();
                    throw new Exception("Error al cerrar la conexión con la base de datos. " + e.Message);

                    //    MessageBox.Show(mensajeError);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            return a;

        }
    }
}
