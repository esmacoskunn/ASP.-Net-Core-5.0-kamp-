﻿using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccsessLayer.Concrete;
using DataAccsessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{

    public class WriterController : Controller
    {
        WriterManager wm = new WriterManager(new EfWriterRepository());
        private readonly UserManager<AppUser> _usermanager;

        public WriterController(UserManager<AppUser> usermanager)
        {
            _usermanager = usermanager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var usermail = User.Identity.Name; 

            ViewBag.v = usermail;
            Context c = new Context();
            var aa = c.Writers.Where(x => x.WriterMail == usermail).Select(y => y.WriterName).FirstOrDefault();
            ViewBag.bb = aa;
            return View();
        }
        public IActionResult WriterProfile()
        {
            return View();
        }
        public IActionResult WriterMail()
        {
            return View();
        }


        public IActionResult Test() 
        {
            return View();
        }


        public PartialViewResult WriterNavbarPartial()  
        {
            return PartialView();
        }


        public PartialViewResult WriterFooterPartial()  
        {
            return PartialView();
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult WriterEditProfile()
        {

            Context c = new Context();
            var username = User.Identity.Name;
            var usermail = c.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();
            var writerID = c.Writers.Where(x => x.WriterMail == usermail).Select(y => y.WriterID).FirstOrDefault();
            var writervalues = wm.TGetById(writerID);
            return View(writervalues);

        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult WriterEditProfile(Writer p)  
        {
            WriterValidator wl = new WriterValidator(); 
            ValidationResult results = wl.Validate(p);  
            if (results.IsValid) 
            {
                wm.TUpdate(p); 
                return RedirectToAction("Index", "Dashboard"); 

            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View();

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult WriterAdd()
        {
            return View();
        }



        [AllowAnonymous]
        [HttpPost]
        public IActionResult WriterAdd(AddProfileImage p)
        {
            Writer w = new Writer();
            if (p.WriterImage != null)
            {
                var extension = Path.GetExtension(p.WriterImage.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WriterImageFiles/", newimagename);
                var stream = new FileStream(location, FileMode.Create);
                p.WriterImage.CopyTo(stream);
                w.WriterImage = newimagename;

            }
            w.WriterMail = p.WriterMail;
            w.WriterName = p.WriterName;
            w.WriterPassword = p.WriterPassword;
            w.WriterStatus = true;
            w.WriterAbout = p.WriterAbout;


            wm.TAdd(w);
            return RedirectToAction("Index", "Dashboard");
        }

        
    }
    
}
