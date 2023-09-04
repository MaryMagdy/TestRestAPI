using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRestAPI.Data;
using TestRestAPI.Data.Models;

namespace TestRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        //create constarcor
        public CategoriesController(AppDBContext db)
        {
            _db = db;
        }
        private readonly AppDBContext _db;



        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var cats = await _db.Categories.ToListAsync();
            return Ok(cats);
        }


        [HttpPost]
        public async Task<IActionResult> AddCategory(string category)
        {
            Category c = new() { Name = category };
            await _db.Categories.AddAsync(c);
            _db.SaveChanges();
            return Ok(c);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            var c = await _db.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
            if (c == null)
            {
                return NotFound($"Category Id {category.Id} Not found");
            }
            c.Name = category.Name;
            _db.SaveChanges();
            return Ok(c);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var c= await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (c==null)
            { return NotFound($"Category Id {id} Not found"); }
            _db.Categories.Remove(c);
            _db.SaveChanges();
            return Ok(c);   
        }


    }
}
