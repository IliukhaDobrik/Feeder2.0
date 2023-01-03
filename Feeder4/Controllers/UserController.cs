using Feeder4.Dtos;
using Feeder4.Entities;
using Feeder4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

namespace Feeder4.Controllers
{
    public class UserController : Controller
    {
        private UserViewModel _user;
        private Cat _cat;
        public async Task<IActionResult> Index(Guid id)
        {
            _user = await GetUser(id);
            return View(_user);
        }

        public async Task<IActionResult> ShowInfo(Guid id)
        {
            var feeder = await GetFeeder(id);
            return View(feeder);
        }

        private async Task<Feeder> GetFeeder(Guid id)
        {
            using (var reader = new StreamReader("Jsons/Feeders.json"))
            {
                var feedersString = await reader.ReadToEndAsync();

                var feeders = JsonSerializer.Deserialize<List<Feeder>>(feedersString);
                var feeder = feeders.Find(x => x.FeederId == id);

                return feeder;
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddTimetable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTimetable(TimetableDto timetable)
        {
            var timetableId = Guid.NewGuid();
            await AddTimetableToFeeder(timetableId, _user.UserId, timetable.FeederId);
            await AddTimetableToFile(timetableId, timetable);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ManualControl(Guid id)
        {
            List<Feeder> feeders;
            Feeder feeder;
            using (var reader = new StreamReader("Jsons/Feeders.json"))
            {
                var feedersString = await reader.ReadToEndAsync();
                feeders = JsonSerializer.Deserialize<List<Feeder>>(feedersString);

                feeder = feeders.Find(x => x.FeederId == id);

                if (feeder.State == true)
                {
                    return RedirectToAction("FeederFull");
                }
                feeder.State = true;
            }
              
            using (var writer = new StreamWriter("Jsons/Feeders.json"))
            {
                var feedersString = JsonSerializer.Serialize(feeders, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                writer.Write(feedersString);
            }

            var log = DateTime.UtcNow.ToString() + ", false -> true, " + "manual, " + "feederId: " + feeder.FeederId;
            AddLog(log);

            return View();
        }

        public async Task<IActionResult> Eat(Guid id)
        {
            List<Feeder> feeders;
            Feeder feeder;
            using (var reader = new StreamReader("Jsons/Feeders.json"))
            {
                var feedersString = await reader.ReadToEndAsync();
                feeders = JsonSerializer.Deserialize<List<Feeder>>(feedersString);

                feeder = feeders.Find(x => x.FeederId == id);

                if (feeder.State == false)
                {
                    return RedirectToAction("WasEating");
                }
                feeder.State = false;
            }

            using (var writer = new StreamWriter("Jsons/Feeders.json"))
            {
                var feedersString = JsonSerializer.Serialize(feeders, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                writer.Write(feedersString);
            }

            var log = DateTime.UtcNow.ToString() + ", true -> false, " + "manual, " + "feederId: " + feeder.FeederId;
            AddLog(log);

            return View();
        }

        private void AddLog(string log)
        {
            List<string> logs;
            using (var reader = new StreamReader("Jsons/Logs.json"))
            {
                var logsString = reader.ReadToEnd();
                logs = JsonSerializer.Deserialize<List<string>>(logsString);
            }

            logs.Add(log);

            using (var writer = new StreamWriter("Jsons/Logs.json"))
            {
                var logsStringWrite = JsonSerializer.Serialize(logs, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                writer.Write(logsStringWrite);
            }
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

        public async Task<IActionResult> FeederFull(Guid id)
        {
            return View();
        }
        
        public async Task<IActionResult> WasEating(Guid id)
        {
            return View();
        }

        public async Task<IActionResult> EditTimetable()
        {
            return View();
        }

        public async Task<IActionResult> DeleteTimetable()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddMark()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMark(Guid id, string mark)
        {
            List<Feeder> feeders;
            using (var reader = new StreamReader("Jsons/Feeders.json"))
            {
                var feedersStringRead = await reader.ReadToEndAsync();

                feeders = JsonSerializer.Deserialize<List<Feeder>>(feedersStringRead);
                var feeder = feeders.Find(x => x.FeederId == id);
                feeder.Mark = mark;
            }

            using (var writer = new StreamWriter("Jsons/Feeders.json"))
            {
                var feedersStringWrite = JsonSerializer.Serialize(feeders, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                writer.Write(feedersStringWrite);
            }
                
            return RedirectToAction("AddMark");
        }


        public async Task<IActionResult> Monitoring()
        {
            return View();
        }

        private async Task AddTimetableToFeeder(Guid timetableId, Guid userId, Guid feederId)
        {
            //using var reader = new StreamReader("Jsons/Users.json");
            //var usersString = await reader.ReadToEndAsync();

            //var users = JsonSerializer.Deserialize<List<Feeder>>(usersString);
            //var user = users.Find(x => x.UserId == userId);
            //var feeder = user.Feeders.Find(x => x.FeederId == feederId);
            //feeder.TimetableId = timetableId;

            //await System.IO.File.WriteAllTextAsync("Jsons/Users.json", JsonSerializer.Serialize(users,
            //    new JsonSerializerOptions
            //    {
            //        WriteIndented = true
            //    }));
        }
        private async Task AddTimetableToFile(Guid timetableId, TimetableDto timetableDto)
        {
            using var reader = new StreamReader("Jsons/Timetables.json");
            var timetablesString = await reader.ReadToEndAsync();

            var timetables = JsonSerializer.Deserialize<List<Timetable>>(timetablesString);
            var timetable = new Timetable
            {
                TimetableId = timetableId,
                Name = timetableDto.Name,
                FeederId = timetableDto.FeederId,
                DateTimes = timetableDto.DateTimes,
            };
            timetables.Add(timetable);

            await System.IO.File.WriteAllTextAsync("Jsons/Timetables.json",
                                                          JsonSerializer.Serialize(timetables, 
                                                          new JsonSerializerOptions
                                                          {
                                                              WriteIndented = true,
                                                          }));
        }

        private async Task<UserViewModel> GetUser(Guid id)
        {
            using var reader = new StreamReader("Jsons/Users.json");
            var userString = await reader.ReadToEndAsync();

            var users = JsonSerializer.Deserialize<List<UserViewModel>>(userString);
            var user = users.Find(x => x.UserId == id);

            return user;
        }
    }
}
