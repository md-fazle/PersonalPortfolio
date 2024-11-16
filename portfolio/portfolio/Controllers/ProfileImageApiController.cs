using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio.Context;

namespace portfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileImageApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileImageApiController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            try
            {
                var images = await _context.profileimg.Select(img => new
                {
                    img.Id,
                    img.Title,
                    ImgUrl = Url.Content(img.ImgPath)
                }).ToListAsync();

                return Ok(images);
            }
            catch (Exception ex)
            {
                // Log the exception and return a server error
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
