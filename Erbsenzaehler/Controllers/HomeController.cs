﻿using System.Web.Mvc;

namespace Erbsenzaehler.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Reports");
            return View();
        }

    }
}