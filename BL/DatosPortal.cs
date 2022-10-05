using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DatosPortal
    {
        //SQLCLIENT
        public static ML.Result Add(ML.DatosPortal datosPortal)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    string query = "DatosAdd";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter[] collection = new SqlParameter[14];

                    collection[0] = new SqlParameter("@IdFolioDeServicio", System.Data.SqlDbType.VarChar);
                    collection[0].Value = datosPortal.IdFolioDeServicio;

                    collection[1] = new SqlParameter("@Prioridad", System.Data.SqlDbType.VarChar);
                    collection[1].Value = datosPortal.Prioridad;

                    collection[2] = new SqlParameter("@TipoServicio", System.Data.SqlDbType.VarChar);
                    collection[2].Value = datosPortal.TipoServicio;

                    collection[3] = new SqlParameter("@SucursalConsignatario", System.Data.SqlDbType.VarChar);
                    collection[3].Value = datosPortal.SucursalConsignatario;

                    collection[4] = new SqlParameter("@FechaCaptura", System.Data.SqlDbType.VarChar);
                    collection[4].Value = datosPortal.FechaCaptura;

                    collection[5] = new SqlParameter("@FechaRealizarServicio", System.Data.SqlDbType.VarChar);
                    collection[5].Value = DateTime.ParseExact(datosPortal.FechaRealizarServicio, "dd/mm/yyyy", CultureInfo.InvariantCulture);

                    collection[6] = new SqlParameter("@OrdenServicio", System.Data.SqlDbType.VarChar);
                    collection[6].Value = datosPortal.OrdenServicio;

                    collection[7] = new SqlParameter("@Importe", System.Data.SqlDbType.Decimal);
                    collection[7].Value = datosPortal.Importe;

                    collection[8] = new SqlParameter("@Divisa", System.Data.SqlDbType.VarChar);
                    collection[8].Value = datosPortal.Divisa;

                    collection[9] = new SqlParameter("@Te", System.Data.SqlDbType.VarChar);
                    collection[9].Value = datosPortal.Te;

                    collection[10] = new SqlParameter("@HoraEnvio", System.Data.SqlDbType.VarChar);
                    collection[10].Value = datosPortal.HoraEnvio;

                    collection[11] = new SqlParameter("@Actualizacion", System.Data.SqlDbType.VarChar);
                    collection[11].Value = datosPortal.Actualizacion;

                    collection[12] = new SqlParameter("@Estatus", System.Data.SqlDbType.VarChar);
                    collection[12].Value = datosPortal.Estatus;

                    collection[13] = new SqlParameter("@IdSucursal", System.Data.SqlDbType.Int);
                    collection[13].Value = datosPortal.IdSucursal;

                    cmd.Parameters.AddRange(collection);
                    cmd.Connection.Open();
                    int RowsAffected = cmd.ExecuteNonQuery();
                    if (RowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;

        }

        public static ML.Result GetById(string IdFolioDeServicio)
        {
            ML.Result result = new ML.Result();
            try
            {

                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    string query = "GetByIdDatos";
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = context;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        SqlParameter[] collection = new SqlParameter[1];

                        collection[0] = new SqlParameter("@IdFolioDeServicio", System.Data.SqlDbType.VarChar);
                        collection[0].Value = IdFolioDeServicio;

                        cmd.Parameters.AddRange(collection);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {

                            DataTable datosTable = new DataTable();
                            da.Fill(datosTable);
                            cmd.Connection.Open();

                            if (datosTable.Rows.Count > 0)
                            {
                                result.Objects = new List<object>();
                                DataRow row1 = datosTable.Rows[0];
                                ML.DatosPortal datosPortal = new ML.DatosPortal();
                                datosPortal.IdFolioDeServicio = row1[0].ToString();
                                datosPortal.Prioridad = row1[1].ToString();
                                datosPortal.TipoServicio = row1[2].ToString();
                                datosPortal.SucursalConsignatario = row1[3].ToString();                       
                                string sub = row1[4].ToString();                             
                                string FechaCaptura = sub.Substring(0, 16);
                                datosPortal.FechaCaptura = FechaCaptura;
                                string sub3 = row1[5].ToString();
                                string FechaRealizarServicio = sub3.Substring(0, 10);
                                datosPortal.FechaRealizarServicio = FechaRealizarServicio;
                                datosPortal.OrdenServicio = row1[6].ToString();
                                datosPortal.Importe = decimal.Parse(row1[7].ToString());
                                datosPortal.Divisa = row1[8].ToString();
                                datosPortal.Te = row1[9].ToString();
                                string sub1 = row1[10].ToString();
                                string HoraEnvio = sub1.Substring(0, 5);
                                string Hre = "00:00";
                                if (Hre.Equals(HoraEnvio))
                                {
                                    string hrs ="";
                                    HoraEnvio = hrs; ;
                                }
                                datosPortal.HoraEnvio = HoraEnvio;

                                string sub2 = row1[11].ToString();
                                string Actualización = sub2.Substring(0, 16);                                
                                datosPortal.Actualizacion = Actualización;
                                datosPortal.Estatus = row1[12].ToString();

                                //result.Objects.Add(datosPortal);
                                result.Object = datosPortal;

                                result.Correct = true;
                            }
                            else
                            {
                                result.Correct = false;
                                result.ErrorMessage = "No se encontraron registros";
                            }
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;

            }
            return result;

        }
        public static ML.Result Update(ML.DatosPortal datosPortal)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnection()))
                {
                    string query = "DatosUpdate";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = context;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter[] collection = new SqlParameter[13];

                    collection[0] = new SqlParameter("@IdFolioDeServicio", System.Data.SqlDbType.VarChar);
                    collection[0].Value = datosPortal.IdFolioDeServicio;

                    collection[1] = new SqlParameter("@Prioridad", System.Data.SqlDbType.VarChar);
                    collection[1].Value = datosPortal.Prioridad;

                    collection[2] = new SqlParameter("@TipoServicio", System.Data.SqlDbType.VarChar);
                    collection[2].Value = datosPortal.TipoServicio;

                    collection[3] = new SqlParameter("@SucursalConsignatario", System.Data.SqlDbType.VarChar);
                    collection[3].Value = datosPortal.SucursalConsignatario;

                    collection[4] = new SqlParameter("@FechaCaptura", System.Data.SqlDbType.VarChar);
                    collection[4].Value = datosPortal.FechaCaptura;

                    collection[5] = new SqlParameter("@FechaRealizarServicio", System.Data.SqlDbType.VarChar);
                    collection[5].Value = datosPortal.FechaRealizarServicio;

                    collection[6] = new SqlParameter("@OrdenServicio", System.Data.SqlDbType.VarChar);
                    collection[6].Value = datosPortal.OrdenServicio;

                    collection[7] = new SqlParameter("@Importe", System.Data.SqlDbType.Decimal);
                    collection[7].Value = datosPortal.Importe;

                    collection[8] = new SqlParameter("@Divisa", System.Data.SqlDbType.VarChar);
                    collection[8].Value = datosPortal.Divisa;

                    collection[9] = new SqlParameter("@Te", System.Data.SqlDbType.VarChar);
                    collection[9].Value = datosPortal.Te;

                    collection[10] = new SqlParameter("@HoraEnvio", System.Data.SqlDbType.VarChar);
                    collection[10].Value = datosPortal.HoraEnvio;

                    collection[11] = new SqlParameter("@Actualizacion", System.Data.SqlDbType.VarChar);
                    collection[11].Value = datosPortal.Actualizacion;                

                    collection[12] = new SqlParameter("@Estatus", System.Data.SqlDbType.VarChar);
                    collection[12].Value = datosPortal.Estatus;

                    cmd.Parameters.AddRange(collection);
                    cmd.Connection.Open();
                    int RowsAffected = cmd.ExecuteNonQuery();
                    if (RowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;

            }
            return result;

        }
    }
}



