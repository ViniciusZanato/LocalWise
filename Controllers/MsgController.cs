using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalWise.Controllers
{
    public class MsgController : Controller
    {
        // GET: Msg
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Analise()
        {
            return View();
        }

        public ActionResult MsgGuia()
        {
            return View();
        }

        public ActionResult MsgTurista()
        {
            return View();
        }
    }
}