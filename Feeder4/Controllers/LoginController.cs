using Feeder4.Dtos;
using Feeder4.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;

namespace Feeder4.Controllers
{
    public class LoginController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserRegisterDto userRegister)
        {
            var users = await GetAll();
            var user = users.SingleOrDefault(x => x.Email == userRegister.Email);
            
            if (user is not null) 
            {
                return RedirectToAction("Index", "User", new { id = user.UserId});
            }
            else
            {
                var admin = await GetAdmin();
                if (admin is not null && admin.Password == userRegister.Password)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("NotRegister");
            }
        }

        public async Task<IActionResult> NotRegister()
        {
            return View();
        }

        private async Task<List<UserViewModel>> GetAll()
        {
            using var reader = new StreamReader("Jsons/Users.json");
            var usersString = await reader.ReadToEndAsync();

            var users = JsonSerializer.Deserialize<List<UserViewModel>>(usersString);
            return users;
        }

        private async Task<AdminViewModel> GetAdmin()
        {
            using var reader = new StreamReader("Jsons/Admin.json");
            var adminString = await reader.ReadToEndAsync();

            var admin = JsonSerializer.Deserialize<AdminViewModel>(adminString);
            return admin;
        }
    }
}
