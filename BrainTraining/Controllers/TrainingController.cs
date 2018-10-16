using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrainTraining.Models;

namespace BrainTraining.Controllers
{
    public class TrainingController : Controller
    {
        public static string Name = "";
        public static string UserEmail = "";
        public static string Page = "";
        public static int Round = 0;
        public static int Points = 0;
        public static int PTZ = 0;
        public static int RND = 0;
        public static int RoundMaxScore = 0;
        public static int RoundInterval = 0;

        [Authorize]
        public ActionResult MemoryPower() { Page = "MemoryPower"; UserCreds(); return View(); }

        [Authorize]
        public ActionResult LeftOrRight() { Page = "LeftOrRight"; UserCreds(); return View(); }

        [Authorize]
        public ActionResult MultiTasking() { UserCreds(); return View(); }

        [Authorize]
        public ActionResult RememberFaces() { UserCreds(); return View(); }

        //Abyssus abyssum vocat - უბსკრული უბსკრულს უხმობს
        [HttpPost]
        public JsonResult GetIcons()
        {
            List<string> BaseIconNames = new List<string>();
            List<string> IconsToShow = new List<string>();
            BaseIconNames.Add("fa fa-wifi fa-stack-1x fa-inverse");
            BaseIconNames.Add("fa fa-user-secret fa-stack-1x fa-inverse");
            BaseIconNames.Add("fa fa-bug fa-stack-1x fa-inverse");
            BaseIconNames.Add("fa fa-video-camera fa-stack-1x fa-inverse");
            BaseIconNames.Add("fa fa-coffee fa-stack-1x fa-inverse");
            BaseIconNames.Add("fa fa-rocket fa-stack-1x fa-inverse");
            BaseIconNames.Add("fa fa-at fa-stack-1x fa-inverse");
            BaseIconNames.Add("fa fa-film fa-stack-1x fa-inverse");
            BaseIconNames.Add("fa fa-desktop fa-stack-1x fa-inverse");

            Random rand = new Random();
            int Count = 0;

            if (Round == 1) { Count = 3; }
            if (Round == 2) { Count = 4; }
            if (Round == 3) { Count = 5; }
            if (Round == 4) { Count = 6; }
            if (Round == 5) { Count = 7; }
            if (Round == 6) { Count = 8; }
            if (Round == 7) { Count = 9; }
            if (Round == 8) { Count = 10; }

            List<int> possible = Enumerable.Range(0, 9).ToList();
            List<int> listNumbers = new List<int>();

            for (int i = 0; i < Count; i++)
            {
                int index = rand.Next(0, possible.Count);
                listNumbers.Add(possible[index]);
                possible.RemoveAt(index);
            }

            for (int k = 0; k < Count; k++) { IconsToShow.Add(BaseIconNames[listNumbers[k]]); }

            IconModel icon = new IconModel { Icons = IconsToShow, IconsCount = Count, PTZ = Points, Round = Round };
            return Json(icon);
        }

        [HttpPost]
        public JsonResult GetColor()
        {
            List<string> Colors = new List<string>();
            Colors.Add("red");
            Colors.Add("green");
            Colors.Add("gray");
            Colors.Add("pink");
            Colors.Add("orange");
            Colors.Add("white");

            List<string> ColorText = new List<string>();
            ColorText.Add("yellow");
            ColorText.Add("black");
            ColorText.Add("blue");
            ColorText.Add("orange");
            ColorText.Add("purple");
            ColorText.Add("green");
            ColorText.Add("gray");
            ColorText.Add("pink");
            ColorText.Add("violet");
            ColorText.Add("red");
            ColorText.Add("gold");

            Random rn = new Random();
            int RandomIndex = rn.Next(0, 5);
            int ColorValRnd = rn.Next(0, 5);
            int ColorTXTRnd = rn.Next(0, 10);

            string UpperTXT = ColorText[ColorTXTRnd];
            string Upper = char.ToUpper(UpperTXT[0]) + UpperTXT.Substring(1);

            if (Round == 1) { RoundMaxScore = 20; RoundInterval = 300; }
            if (Round == 2) { RoundMaxScore = 30; RoundInterval = 250; }
            if (Round == 3) { RoundMaxScore = 40; RoundInterval = 200; }
            if (Round == 4) { RoundMaxScore = 50; RoundInterval = 100; }
            if (Round == 5) { RoundMaxScore = 60; RoundInterval = 100; }
            if (Round == 6) { RoundMaxScore = 70; RoundInterval = 95; }

            ColorsModel Color = new ColorsModel
            {
                ColorToShow = Colors[RandomIndex],
                ColorText = Upper,
                ColorVal = ColorText[ColorTXTRnd],
                RoundMaxScore = RoundMaxScore,
                RoundInterval = RoundInterval
            };

            return Json(Color);
        }

        [HttpPost]
        public JsonResult Save(int userPTZ, int userRound, string Game)
        {
            Points = userPTZ;
            Round = userRound;

            using (BrainTrainingEntities bte = new BrainTrainingEntities())
            {
                if (Game == "MemoryPower")
                {
                    var UserSaves = bte.GameSaves.SingleOrDefault(mail => mail.UserId == UserEmail);

                    if (UserSaves != null)
                    {
                        UserSaves.MemoryPowerPoints = Points;
                        UserSaves.MemoryPowerRound = Round;
                        bte.SaveChanges();
                    }
                }

                else if (Game == "LeftOrRight")
                {
                    var UserSaves = bte.GameSaves.SingleOrDefault(mail => mail.UserId == UserEmail);
                    if (UserSaves != null)
                    {
                        UserSaves.LeftOrRightPoints = userPTZ;
                        UserSaves.LeftOrRightRound = userRound;
                        bte.SaveChanges();
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public void UserCreds()
        {
            using (BrainTrainingEntities bte = new BrainTrainingEntities())
            {
                UserEmail = HttpContext.User.Identity.Name;
                var UserCredentials = bte.Users.Where(a => a.Email == UserEmail).FirstOrDefault();
                if (UserCredentials != null) { Name = UserCredentials.FirstName; }

                if (Page == "MemoryPower")
                {
                    var UserStats = bte.GameSaves.Where(a => a.UserId == UserEmail).FirstOrDefault();
                    Points = UserStats.MemoryPowerPoints;
                    Round = UserStats.MemoryPowerRound;
                }
                else if (Page == "LeftOrRight")
                {
                    var UserStats = bte.GameSaves.Where(a => a.UserId == UserEmail).FirstOrDefault();
                    Points = UserStats.LeftOrRightPoints;
                    Round = UserStats.LeftOrRightRound;
                }
            }
        }
    }
}