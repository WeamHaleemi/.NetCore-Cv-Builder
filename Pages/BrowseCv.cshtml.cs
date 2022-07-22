using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class BrowseCv : PageModel
    {
        private readonly DatabaseContext _context;
        public List<User> userList { get; set; }
        [BindProperty(SupportsGet = true)]
       
        public int Id { get; set; }
        public BrowseCv(DatabaseContext Context)
        {
            this._context = Context;

        }
        public void OnGet()
        {
            userList = _context.UserTable.ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            User user = _context.UserTable.Where(u => u.UserId==Id).FirstOrDefault();
           _context.Remove(user);
           await _context.SaveChangesAsync();
            userList = _context.UserTable.ToList();
            return Page();
        }
    }
}
