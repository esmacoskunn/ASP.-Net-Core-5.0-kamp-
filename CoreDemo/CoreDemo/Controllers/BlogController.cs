﻿using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccsessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System; 
using System.Collections.Generic; 
using System.Linq;  

namespace CoreDemo.Controllers
{
    
    public class BlogController : Controller
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());

        public IActionResult Index()
        {
           
            var values = bm.GetBlogListWithCategory();  
            return View(values);
        }

        public IActionResult BlogReadAll(int id)
        {
            ViewBag.i = id; 
            var values = bm.GetBlogByID(id);   
            return View(values);
        }




        public IActionResult BlogListByWriter()  
        {
            var values = bm.GetListWithCategoryByWriterbm(1); 
            return View(values);

        }



        [HttpGet]
        public IActionResult BlogAdd() 
        {
          
            List<SelectListItem> categoryvalues = (from x in cm.GetList() 
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString() 

                                                   }).ToList();
            ViewBag.cv = categoryvalues; 
            return View();
        }



        [HttpPost]
        public IActionResult BlogAdd(Blog p) 
        {
           
            BlogValidator bv = new BlogValidator(); 
            ValidationResult results = bv.Validate(p);
            if (results.IsValid)
            {
                p.BlogStatus = true;
                p.BlogCreateDate = DateTime.Now.ToShortDateString();
                bm.TAdd(p); 

                return RedirectToAction("BlogListByWriter", "Blog");

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




        public IActionResult DeleteBlog(int id) 
        {
            var blogvalue = bm.TGetById(id); 
            bm.TDelete(blogvalue);
            return RedirectToAction("BlogListByWriter");
        }
        




        [HttpGet] 
        public IActionResult EditBlog(int id)
        {
            var blogvalue = bm.TGetById(id); 
            List<SelectListItem> categoryvalues = (from x in cm.GetList() 
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName, 
                                                       Value = x.CategoryID.ToString()

                                                   }).ToList(); 
            ViewBag.cv = categoryvalues;  
            return View(blogvalue);  
        }

        [HttpPost]  
        public IActionResult EditBlog(Blog p)
        {
            p.WriterID = 1;
            p.BlogCreateDate = DateTime.Now.ToShortDateString();
            p.BlogStatus = true;
            bm.TUpdate(p);
            return RedirectToAction("BlogListByWriter");
        }





    }
}
