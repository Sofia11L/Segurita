using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; 

namespace BL
{
    public class Cliente
    {

        public static async Task<ML.Result> GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (SqlConnection context =new SqlConnection(DL.Conexion.GetStringConnection()))
                {
                    string query = "SELECT Id, Nombre, FechaCreacion, FechaModificacion FROM Clientes";

                    using (SqlCommand cmd = new SqlCommand(query, context))
                    {
                        await context.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            result.Objects = new List<object>();

                            while (await reader.ReadAsync())
                            {
                                ML.Cliente cliente = new ML.Cliente();

                                cliente.Id = Convert.ToInt32(reader["Id"]);
                                cliente.Nombre = reader["Nombre"].ToString();
                                cliente.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);

                                if (reader["FechaModificacion"] != DBNull.Value)
                                {
                                    cliente.FechaModificacion =
                                        Convert.ToDateTime(reader["FechaModificacion"]);
                                }

                                result.Objects.Add(cliente);
                            }

                            result.Correct = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        public static async Task<ML.Result> Add(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.SLerdoSeguritasEntities context =new DL.SLerdoSeguritasEntities())
                {
                    DL.Cliente cliente1 = new DL.Cliente();

                    cliente1.Nombre = cliente.Nombre;
                    cliente1.FechaCreacion = DateTime.Now;
                    cliente1.FechaModificacion = null;

                    context.Clientes.Add(cliente1);
                    int queryInsert = await context.SaveChangesAsync();

                    if (queryInsert > 0)
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
            }

            return result;
        }

        public static async Task<ML.Result> GetById(int id)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (SqlConnection context = new SqlConnection(DL.Conexion.GetStringConnection()))
                {
                    string query = @"SELECT Id, Nombre, FechaCreacion, FechaModificacion FROM Clientes WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, context))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        await context.OpenAsync();

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                ML.Cliente cliente = new ML.Cliente();

                                cliente.Id = Convert.ToInt32(reader["Id"]);
                                cliente.Nombre = reader["Nombre"].ToString();
                                cliente.FechaCreacion =
                                    Convert.ToDateTime(reader["FechaCreacion"]);

                                if (reader["FechaModificacion"] != DBNull.Value)
                                {
                                    cliente.FechaModificacion =
                                        Convert.ToDateTime(reader["FechaModificacion"]);
                                }

                                result.Object = cliente;
                                result.Correct = true;
                            }
                            else
                            {
                                result.ErrorMessage = "No se encontró el cliente.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        public static async Task<ML.Result> Update(ML.Cliente cliente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.SLerdoSeguritasEntities context =new DL.SLerdoSeguritasEntities())
                {
                    var cliente1 = new DL.Cliente();
                    cliente1.Id = cliente.Id;

                    context.Clientes.Attach(cliente1);

                    cliente1.Nombre = cliente.Nombre;
                    cliente1.FechaModificacion = DateTime.Now;

                    context.Entry(cliente1)
                        .Property(c => c.Nombre).IsModified = true;

                    context.Entry(cliente1)
                        .Property(c => c.FechaModificacion).IsModified = true;

                    int queryUpdate = await context.SaveChangesAsync();

                    if (queryUpdate > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.ErrorMessage = "No se modificó el cliente.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        public static async Task<ML.Result> Delete(int id)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.SLerdoSeguritasEntities context = new DL.SLerdoSeguritasEntities())
                {
                    DL.Cliente cliente = new DL.Cliente();
                    cliente.Id = id;

                    context.Clientes.Attach(cliente);
                    context.Clientes.Remove(cliente);

                    int queryDelete = await context.SaveChangesAsync();

                    if (queryDelete > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.ErrorMessage = "No se eliminó el cliente.";
                    }
                }
            }
            catch (Exception ex)
            {
                

                result.Correct = false;
            }

            return result;
        }
    }
}
