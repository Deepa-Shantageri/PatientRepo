using System.Collections;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_api.Data;
using Product_api.model;

namespace Product_api.Controller
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class ProductController:ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context=context;   
        }
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(){
           return await _context.Products.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductsById(int id)
        {
            var product=await _context.Products.FindAsync(id);
           if(product==null)
           {
            return NotFound();
           }
           return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutProduct(int id,Product product)
        {
            if(id!=product.Id)
            {
                return BadRequest();
            }
            _context.Entry(product).State=EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id){
            
            var product= await _context.Products.FindAsync(id);
            if(product==null)
            return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}