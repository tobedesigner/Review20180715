using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class RazorController : Controller
    {
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult RazorView()
        {
            //30 隨堂測驗：練習透過 Razor 語法精準輸出指定格式
            return View();
        }

        public ActionResult Dashboard()
        {

            return View();
        }
    }
}