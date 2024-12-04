using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    public class ProductsController : ApiController
    {
        List<Product> lista = new List<Product>
        {
            new Product {Id= 1, Name = "Tomato Soup", Category = "Groceries", Price= 1 },
            new Product { Id = 2, Name = "Yo-Yo", Category= "Toys", Price= 3.75M },
            new Product { Id = 3, Name = "Hammer", Category= "Hardware", Price= 16.99M },
        };


        // GET: api/Products
        public IEnumerable<Product> GetAllProducts()
        {
            return lista;
        }

        // GET: api/Products/5
        [HttpGet]
        [Route("api/products/{id:int}")]
        public IHttpActionResult ObterProductByID(int id)
        {
            var prod = lista.FirstOrDefault( p=> p.Id == id);
            if (prod == null)
            {
                return NotFound(); 
            }
            return Ok(prod);
        }

        //[Route("api/products/category/{cat}")]
        [Route("api/products/{cat}")]
        public IHttpActionResult GetProductByCategory(string cat)
        {
            var prod = lista.FirstOrDefault(p => p.Category.Equals(cat));
            if (prod == null)
            {
                return NotFound();
            }
            return Ok(prod);
        }

        // POST: api/Products
        public IHttpActionResult Post([FromBody]Product value)
        {
            lista.Add(value);
            return Ok();
        }

        // PUT: api/Products/5
        public IHttpActionResult Put(int id, [FromBody]Product value)
        {
            return Ok();
        }

        // DELETE: api/Products/5
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
