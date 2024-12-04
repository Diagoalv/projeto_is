using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using ProductsDatabaseAPI.Models;

namespace ProductsDatabaseAPI.Controllers
{
    public class ProductsController : ApiController
    {
        string connectionString = Properties.Settings.Default.connStringBD;

        // GET: api/Products
        [Route("api/products")]
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            List<Product> products = new List<Product>();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Prods";
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    //criar um Product
                    Product prod = new Product
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Category = (string)reader["Category"],
                        Price = (reader["Price"] == DBNull.Value) ? 
                                           0: Convert.ToDecimal(reader["Price"])
                    };
                    products.Add(prod); 
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                Debug.WriteLine("Ocoreu um erro!");
            }
                
            return products;
        }

        // GET: api/Products/5
        [Route("api/products/{id}")]
        [HttpGet]
        public IHttpActionResult GetProductById(int id)
        {
            try
            {
                string query = "SELECT * FROM Prods WHERE Id = @idProd";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@idProd", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Product prod = null;
                        if (reader.Read())
                        {
                            prod = new Product
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Category = (string)reader["Category"],
                                Price = (reader["Price"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["Price"])
                            };
                        }
                        //Product p = new Product();
                        //p.Id = (int)reader["Id"];
                        //p.Name = (string)reader["Name"];
                        if (prod == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return Ok(prod);
                        }

                    }
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // POST: api/Products
        [Route("api/products")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]Product value)
        {
            try
            {
                string query = "INSERT INTO Prods VALUES (@name, @cat, @price);";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@cat", value.Category);
                    command.Parameters.AddWithValue("@price", value.Price);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // PUT: api/Products/5
        [Route("api/products/{id}")]
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]Product value)
        {
            try
            {
                string query = "UPDATE Prods SET Name=@name, Category=@cat, Price=@price WHERE Id=@idProd";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@cat", value.Category);
                    command.Parameters.AddWithValue("@price", value.Price);
                    command.Parameters.AddWithValue("@idProd", id);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return Ok();
                    }
                    return NotFound();//BadRequest();
                }

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        // DELETE: api/Products/5
        [Route("api/products/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM Prods WHERE Id=@idProd";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idProd", id);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return Ok();
                    }
                    return NotFound();
                }

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
