using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRestAPI.Data;
using TestRestAPI.Data.Models;
using TestRestAPI.Models;

namespace TestRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public ItemsController(AppDBContext db)
        {
            _db = db;
        }
        private readonly AppDBContext _db;

        [HttpGet]
        public async Task<IActionResult> AllItems()
        {
            var items = await _db.Items.ToListAsync();
            return Ok(items);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> AllItems(int id)
        { 
        var item=await  _db.Items.SingleOrDefaultAsync(x=>x.Id == id);
            if (item == null)
            {
                return NotFound($"Item code {id} Not Found");
            }

            return Ok(item);    
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm]mdlItem mdl)
        {
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);

            var item = new Item
            {
                Name = mdl.Name,
                Price = mdl.Price,
                Notes = mdl.Notes,
                CategoryId = mdl.CategoryId,
                Image = stream.ToArray()
            };

            await _db.Items.AddAsync(item);
            _db.SaveChanges();
            return Ok(item);
        }
    }
}
