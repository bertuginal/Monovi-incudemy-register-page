using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Angle.Models;
using Angle.Helpers;
using System.Configuration;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Angle.Controllers
{
    public class RegisterController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Useradd()
        {
            RegisterModel model = new RegisterModel();

            var prms = new Dictionary<string, object>();
            var result1 = DataHelper.ListFromStoredProcedure("Sp_University", prms);
            var result2 = DataHelper.ListFromStoredProcedure("Sp_Department", prms);
            var result3 = DataHelper.ListFromStoredProcedure("Sp_Class", prms);
            var result4 = DataHelper.ListFromStoredProcedure("Sp_AreaCode", prms);

            foreach (DataRow item in result1.Rows)
            {
                University u = new University();
                u.UniversityId = item.Field<int>("UniversityId");
                u.UniversityName = item.Field<string>("UniversityName");
                model.Universities.Add(u);
            }
            foreach (DataRow item in result2.Rows)
            {
                Departmant u = new Departmant();
                u.DepartmantId = item.Field<int>("DepartmantId");
                u.DepartmantName = item.Field<string>("DepartmantName");
                model.Departmants.Add(u);
            }
            foreach (DataRow item in result3.Rows)
            {
                Class u = new Class();
                u.ClassId = item.Field<int>("ClassId");
                u.ClassName = item.Field<string>("ClassName");
                model.Classes.Add(u);
            }
            foreach (DataRow item in result4.Rows)
            {
                AreaCode u = new AreaCode();
                u.AreaCodeId = item.Field<int>("AreaCodeId");
                u.AreaCodeName = item.Field<string>("AreaCodeName");
                model.AreaCodes.Add(u);
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult Useradd(UserAdd model, IFormFile UploadCV, IFormFile UploadLogo)
        {
            var prms = new Dictionary<string, object>();
            prms.Add("FirstName", model.Name);
            prms.Add("LastName", model.Surname);
            //prms.Add("GenderId", model.GenderId);
            prms.Add("UserEmail", model.Email);
            prms.Add("AreaCodeId", model.AreaCodeId);
            prms.Add("TelephoneNumber", model.Phone);
            //prms.Add("UserRoleId", model.role);
            prms.Add("UniversityId", model.UniversityId);
            prms.Add("DepartmantId", model.DepartmantId);
            prms.Add("ClassId", model.ClassId);
            prms.Add("UserPassword", model.Password);
            //prms.Add("UploadCV", model.UploadCV);

            if (model.Password == model.ConfirmPassword)
            {
                var a = DataHelper.RunFromStoredProcedure("Sp_Register", prms);
            }
            else if (model.Password != model.ConfirmPassword)
            {

                TempData["notice"] = "**PASSWORDS DOESN'T MATCH**";
                return Redirect("/");

            }

            return Redirect("Useradd");
        }
        [HttpPost]
        public IActionResult Student(bool hasPassport)
        {
            ViewBag.HasPassport = hasPassport;
            return View();
        }
    }
}
