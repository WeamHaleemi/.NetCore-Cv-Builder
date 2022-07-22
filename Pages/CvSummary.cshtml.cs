using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class CvSummaryModel : PageModel
    {
        private readonly DatabaseContext _context;
        [BindProperty(SupportsGet =true)]
        [FromQuery]
        public int Id { get; set; }
        public User? user { get; set; }
        public CvSummaryModel(DatabaseContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> OnGet()
        {
            user =await _context.UserTable.Where(u => u.UserId == Id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
