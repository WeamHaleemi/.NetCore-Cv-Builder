using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    
    public class EditCV : PageModel
    {
        private readonly DatabaseContext _context;
        [BindProperty(SupportsGet = true)]
        [FromQuery]
        public int Id { get; set; }
        public User? user { get; set; }
        public EditCV(DatabaseContext Context)
        {
            this._context = Context;
        }
        public async Task<IActionResult> OnGet()
        {
            user = await _context.UserTable.Where(u => u.UserId == Id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
