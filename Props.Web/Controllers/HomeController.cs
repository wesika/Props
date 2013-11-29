using Playtomic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Props.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var list = ShowScores();
            GetAchievements();
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void AddAchievement()
        {
            var achievement = new PlayerAchievement
            {
                achievement = "Punched Tass",
                achievementkey = "abcdefgh",
                playerid = "1",
                playername = "ben",
                fields = new Hashtable
                {
                  { "difficulty" , "easy"}
                }
            };

            Playtomic.Achievements.Save(achievement, r =>
            {

                if (r.success)
                {
                    // note that we could still have a failure errorcode here when the player
                    // already has an achievement
                }
                else
                {
                    // response failed because of response.errormessage
                }

            });
        }

        public void GetAchievements()
        {
            // just the achievements
            Playtomic.Achievements.List(null, (achievements, response) =>
            {
                if (response.success)
                {
                    var json = new JavaScriptSerializer();
                    for (var i = 0; i < achievements.Count; i++)
                    {
                        Console.WriteLine(json.Serialize(achievements[i].achievement));
                        // will output { achievement: "Punched Tass" }
                    }
                }
            });
        }

        public List<PlayerScore> ShowScores()
        {
            // table options
            var table = new Hashtable {
                {"table", "scores"},
                {"page", 1},
                {"perpage", 10},
                {"highest", true}
  };
            var results = new List<PlayerScore>();
            Playtomic.Leaderboards.List(table, (scores, numscores, response) =>
            {
                if (response.success)
                {
                    results = scores;
                }
                else
                {
                    // score listing failed because of response.errormessage with response.errorcode
                    throw new Exception("High Scroes not Returned");
                }
            });

            return results;
        }
    }
}


