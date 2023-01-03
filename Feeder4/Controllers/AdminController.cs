using Feeder4.Dtos;
using Feeder4.Entities;
using Feeder4.Enums;
using Feeder4.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Feeder4.Controllers
{
    public class AdminController : Controller
    {
        private AdminViewModel _admin;
        public async Task<IActionResult> Index()
        {
            _admin = await GetAdmin();
            return View(_admin);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto user)
        {
            AddUser(user);
            return RedirectToAction("Register");
        }

        [HttpGet]
        public async Task<IActionResult> AddFeeder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFeeder(FeederRegisterDto feeder)
        {
            var feederId = Guid.NewGuid();

            AddFeederToFile(feederId, feeder);
            AddFeederToUser(feederId, feeder);
            //AddCat(feederId);

            return RedirectToAction("AddFeeder");
        }

        private async Task AddUser(UserRegisterDto user)
        {
            List<UserViewModel> users;
            using (var reader = new StreamReader("Jsons/Users.json"))
            {
                var usersStringDes = await reader.ReadToEndAsync();

                users = JsonSerializer.Deserialize<List<UserViewModel>>(usersStringDes);
                users.Add(new UserViewModel
                {
                    UserId = Guid.NewGuid(),
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                });
            }

            var usersString = JsonSerializer.Serialize(users, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            System.IO.File.WriteAllTextAsync("Jsons/Users.json", usersString);
        }

        private async Task AddFeederToUser(Guid feederId, FeederRegisterDto feeder)
        {
            List<UserViewModel> users;
            using (var reader = new StreamReader("Jsons/Users.json"))
            {
                var usersStringRead = await reader.ReadToEndAsync();

                users = JsonSerializer.Deserialize<List<UserViewModel>>(usersStringRead);

                var user = users.FirstOrDefault(x => x.UserId == feeder.UserId);
                if (user is not null)
                {
                    user.FeedersId.Add(feederId);
                }
            }
               
            var usersStringWrite = JsonSerializer.Serialize(users, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            System.IO.File.WriteAllTextAsync("Jsons/Users.json", usersStringWrite);
        }

        private async Task AddFeederToFile(Guid feederId, FeederRegisterDto feeder)
        {
            List<Feeder> feeders;
            using (var reader = new StreamReader("Jsons/Feeders.json"))
            {
                var feedersStringRead = await reader.ReadToEndAsync();

                feeders = JsonSerializer.Deserialize<List<Feeder>>(feedersStringRead);
                feeders.Add(new Feeder
                {
                    FeederId = feederId,
                    Name = feeder.Name,
                    Mark = null,
                    TimetableId = null,
                    State = false
                });
            }

            var feedersStringWrite = JsonSerializer.Serialize(feeders, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
            System.IO.File.WriteAllTextAsync("Jsons/Feeders.json", feedersStringWrite);
        }

        private async Task<AdminViewModel> GetAdmin()
        {
            using var reader = new StreamReader("Jsons/Admin.json");
            var adminString = await reader.ReadToEndAsync();

            var admin = JsonSerializer.Deserialize<AdminViewModel>(adminString);
            return admin;
        }

        public async Task<IActionResult> Logs()
        {
            List<string> logs;
            using (var reader = new StreamReader("Jsons/Logs.json"))
            {
                var logsString = reader.ReadToEnd();
                logs = JsonSerializer.Deserialize<List<string>>(logsString);
            }

            return View(logs);
        }

        private void AddCat(Guid id)
        {
            var cat = new Cat
            {
                CatId = Guid.NewGuid(),
                FeederId = id,
                Name = "Barsik"
            };

            List<Feeder> feeders;
            Feeder feeder;
            using (var reader = new StreamReader("Jsons/Feeders.json"))
            {
                var feedersString = reader.ReadToEnd();
                feeders = JsonSerializer.Deserialize<List<Feeder>>(feedersString);

                feeder = feeders.Find(x => x.FeederId == id);
            }

            if (feeder.State == true)
            {
                feeder.State = false;
            }

            using (var writer = new StreamWriter("Jsons/Feeders.json"))
            {
                var feedersStringWrite = JsonSerializer.Serialize(feeders, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                writer.Write(feedersStringWrite);
            }
        }
    }
}
