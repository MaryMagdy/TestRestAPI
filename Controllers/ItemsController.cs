using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRestAPI.Data;
using TestRestAPI.Data.Models;
using TestRestAPI.Models;
using static Azure.Core.HttpHeader;

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


        [HttpGet("ItemsWithCategory/{IdCategory}")]
        public async Task<IActionResult> ItemsWithCategory(int IdCategory)
        {
            var item = await _db.Items.Where(x => x.CategoryId == IdCategory).ToListAsync();
            if (item == null)
            {
                return NotFound($"Category Id {IdCategory} Has No Items");
            }
            return Ok(item);
        }


        [HttpDelete("id")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x=>x.Id == id); 
            if (item == null)
            {
                return NotFound($"item id {id} not found");
            }

            _db.Items.Remove(item); return Ok(item);    
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] mdlItem mdl)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            { return NotFound($"item id {id} not found"); }

            var isCategoExist=await _db.Categories.SingleOrDefaultAsync(x => x.Id == mdl.CategoryId); 
            if(isCategoExist == null) {
                return NotFound($"Category id {mdl.CategoryId} not found"); 
            }

            if (mdl.Image != null)
            {
                using var stream = new MemoryStream();
                await mdl.Image.CopyToAsync(stream);
                 item.Image=stream.ToArray();
            }
            item.Name = mdl.Name;
            item.Price = mdl.Price;
            item.Notes = mdl.Notes;
            item.CategoryId = mdl.CategoryId;

            _db.SaveChanges();
            return Ok(item);
        }
    }
}
