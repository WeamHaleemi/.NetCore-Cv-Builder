using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Database;
using WebApplication1.Models;

namespace WebApplication1.Pages
{

    public class SendCV : PageModel
    {
        public string[] genderList = { "Male", "Female" };
        [BindProperty]
        public UserBindingModel userModel { get; set; }
        private List<Nationality> Nationalities;
        public List<Skill> Skills;
        public IEnumerable<SelectListItem> Items;
        protected readonly DatabaseContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public int X { get; set; }
        [BindProperty]
        public int Y { get; set; }

        public SendCV(DatabaseContext Context, IWebHostEnvironment webHostEnvironment)
        {
            _context = Context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public void Initialize()
        {
            Nationalities = _context.NationalityTable.ToList();
            Skills = _context.SkillsTable.ToList();
            Items = Nationalities.Select(i => new SelectListItem
            {
                Value = i.NationalityId.ToString(),
                Text = i.NationalityName
            });

        }

        public void OnGet()
        {

            this.Initialize();
        }

        public async Task<IActionResult> OnPost()
        {
            this.Initialize();
            if (!userModel.Email.Equals(userModel.ConfirmEmail))
            {
                ModelState.AddModelError("", "Emails dont match");
                return Page();
            }
            if (!(this.X + this.Y == this.userModel.Verification))
            {
                ModelState.AddModelError("", "X+Y is not equal to sum");
                return Page();
            }
            else
            {
                User user1 = getUser(userModel);
               await _context.AddAsync(user1);
               await _context.SaveChangesAsync();
                return Redirect("/CvSummary?Id="+user1.UserId);
            }


        }
        public int getGrade(UserBindingModel userModel)
        {
            int i = userModel.Skills.Length;
            i = i * 10;
            if (userModel.Gender == "Male")
                i += 5;
            else i += 10;
            return i;
        }


        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (userModel.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(userModel.Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    userModel.Image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }



        public User getUser(UserBindingModel userModel)
        {

            User user1 = new User()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                BirthDate = userModel.BirthDate,
                nationality = _context.NationalityTable.Where(n => n.NationalityId == Convert.ToInt32(userModel.Nationality)).FirstOrDefault(),
                Gender = userModel.Gender,

            };

            user1.Grade = this.getGrade(userModel);
            user1.Photo = this.ProcessUploadedFile();
            foreach (var skill in userModel.Skills)
            {
                user1.skills.Add(_context.SkillsTable.Where(s => s.SkillId == Convert.ToInt32(skill)).FirstOrDefault());
            }
            return user1;
        }
        //public void OnPostDownloadMyInformation()
        //{
        //    ;
        //}



    }
    public class UserBindingModel
    {
        [DisplayName("First Name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(255, ErrorMessage = "First Name is too long")]
        public String FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(255, ErrorMessage = "First Name too long")]
        public String LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Required")]
        public String Email { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Required")]
        public String ConfirmEmail { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public String Gender { get; set; }
        [Required(ErrorMessage = "Required")]
        public String[] Skills { get; set; }
        [Required(ErrorMessage = "Required")]
        public String Nationality { get; set; }
        [Required(ErrorMessage = "Required")]
        public int Verification { get; set; }
        [Required(ErrorMessage = "Required")]
        public IFormFile Image { get; set; }
    }
}