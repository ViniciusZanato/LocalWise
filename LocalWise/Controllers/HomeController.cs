using LocalWise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LocalWise.Controllers
{
    public class HomeController : Controller
    {

        public Contexto db = new Contexto();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(Cadastro cad)
        {

            if (ModelState.IsValid)
            {
                if (db.Guia.Where(x => x.Email == cad.Email).ToList().Count > 0)
                {
                    ModelState.AddModelError("Email", "E-mail já cadastrado");
                    return View(cad);
                }
                Guia gui = new Guia();
                gui.Nome = cad.Nome;
                gui.Cpf = cad.Cpf;
                gui.Nascimento = cad.Nascimento;
                gui.Telefone = cad.Telefone;
                gui.Email = cad.Email;
                gui.Senha = Funcoes.HashTexto(cad.Senha, "SHA512");

                db.Guia.Add(gui);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cad);
        }

        public ActionResult Acesso()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Acesso(Acesso ace)
        {
            if (ModelState.IsValid)
            {
                string senhacrip = Funcoes.HashTexto(ace.Senha, "SHA512");
                var gui = db.Guia.Where(x => x.Email == ace.Email && x.Senha == senhacrip).ToList().FirstOrDefault();

                if (gui == null)
                {
                    ModelState.AddModelError("", "E-mail ou senha inválido");
                    return View(ace);
                }

                else
                {
                    FormsAuthentication.SetAuthCookie(gui.Id + "|" + gui.Nome, false);
                    string permissoes = "";
                    foreach (var item in gui.Nome)
                    {
                        permissoes += item + ",";
                    }
                    permissoes = permissoes.Substring(0, permissoes.Length - 1);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, gui.Id + "|" + gui.Nome, DateTime.Now, DateTime.Now.AddMinutes(30), false, permissoes);
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index");
                }
            }
            return View(ace);
        }
    }
}