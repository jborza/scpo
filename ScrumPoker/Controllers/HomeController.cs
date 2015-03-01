using System;
using System.Web.Mvc;

namespace ScrumPoker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Poker(int? roomID)
        {
            if (roomID == null)
                return RedirectToAction("Index");
            ViewBag.RoomID = roomID.Value;
            return View();
        }

        public ActionResult Create()
        {
            //TODO use room manager to create a new room
            int id = new Random().Next();
            return RedirectToAction("Poker", new { roomID = id });
        }
    }
}