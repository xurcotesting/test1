﻿using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
