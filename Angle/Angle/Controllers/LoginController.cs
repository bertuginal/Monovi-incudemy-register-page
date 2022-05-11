using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Angle.Models;


namespace Angle.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginModel()
        {
            var model = new LoginModel();

            return View(model);
        }
        [HttpPost]
        public IActionResult LoginModel(LoginModel model)
        {

            return View(model);
        }
    }
}
