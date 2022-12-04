using LocalWise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocalWise.Controllers
{
    public class HomeController : Controller
    {

        public Contexto db = new Contexto();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string escolhaLocal, int qtdPessoas, string transporte, string tipoPasseio, string dia)
        {
            HttpCookie cookie = new HttpCookie("cookie");
            cookie.Values.Add("local", escolhaLocal);
            cookie.Values.Add("qtde", qtdPessoas.ToString());
            cookie.Values.Add("trans", transporte);
            cookie.Values.Add("tipo", tipoPasseio);
            cookie.Values.Add("data", dia);
            Response.Cookies.Add(cookie);
            var guia = db.Guia.Where(x => x.Cidade == escolhaLocal).ToList();
            ViewBag.Guia = guia;
            return View();
        }

        public ActionResult Dashboard(int guiaId)
        {
            int id = guiaId;//trocar
            var gui = db.Guia.Find(id);
            ViewBag.Guia = gui.Nome;
            return View();
        }

        public ActionResult Email()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Email(Mensagem msg)
        {
            if (ModelState.IsValid)
            {
                TempData["MSG"] = Funcoes.EnviarEmail(msg.Email, msg.Assunto, msg.CorpoMsg);
            }
            else
            {
                TempData["MSG"] = "warning|Preencha todos os campos";
            }
            return View(msg);
        }

        public ActionResult EsqueceuSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EsqueceuSenha(EsqueceuSenha esq)
        {
            if (ModelState.IsValid)
            {
                var tur = db.Turista.Where(x => x.Email == esq.Email).ToList().FirstOrDefault();
                if (tur != null)
                {
                    tur.Hash = Funcoes.Codifica(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                    db.Entry(tur).State = EntityState.Modified;
                    db.SaveChanges();
                    string msg = "<h3>Sistema</h3>";
                    msg += "Para alterar sua senha <a href='https://localhost:44374/Home/Redefinir/" + tur.Hash + "' target='_blank'>clique aqui</a>";
                    Funcoes.EnviarEmail(tur.Email, "Redefinição de senha", msg);
                    TempData["MSG"] = "success|Senha redefinida com sucesso!";
                    return RedirectToAction("Index");
                }
                TempData["MSG"] = "error|E-mail não encontrado";
                return View();
            }
            TempData["MSG"] = "warning|Preencha todos os campos";
            return View();
        }

        public ActionResult Redefinir(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var tur = db.Turista.Where(x => x.Hash == id).ToList().FirstOrDefault();
                if (tur != null)
                {
                    try
                    {
                        DateTime dt = Convert.ToDateTime(Funcoes.Decodifica(tur.Hash));
                        if (dt > DateTime.Now)
                        {
                            RedefinirSenha red = new RedefinirSenha();
                            red.Hash = tur.Hash;
                            red.Email = tur.Email;
                            return View(red);
                        }
                        TempData["MSG"] = "warning|Esse link já expirou!";
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        TempData["MSG"] = "error|Hash inválida!";
                        return RedirectToAction("Index");
                    }
                }
                TempData["MSG"] = "error|Hash inválida!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "error|Acesso inválido!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Redefinir(RedefinirSenha red)
        {
            if (ModelState.IsValid)
            {
                Contexto db = new Contexto();
                var tur = db.Turista.Where(x => x.Hash == red.Hash).ToList().FirstOrDefault();
                if (tur != null)
                {
                    tur.Hash = null;
                    tur.Senha = Funcoes.HashTexto(red.Senha, "SHA512");
                    db.Entry(tur).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["MSG"] = "success|Senha redefinida com sucesso!";
                    return RedirectToAction("Index");
                }
                TempData["MSG"] = "error|E-mail não encontrado";
                return View(red);
            }
            TempData["MSG"] = "warning|Preencha todos os campos";
            return View(red);
        }

        public ActionResult EmailGuia()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EmailGuia(MensagemGuia msg)
        {
            if (ModelState.IsValid)
            {
                TempData["MSG"] = Funcoes.EnviarEmail(msg.Email, msg.Assunto, msg.CorpoMsg);
            }
            else
            {
                TempData["MSG"] = "warning|Preencha todos os campos";
            }
            return View(msg);
        }

        public ActionResult EsqueceuSenhaGuia()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EsqueceuSenhaGuia(EsqueceuSenhaGuia esq)
        {
            if (ModelState.IsValid)
            {
                var gui = db.Guia.Where(x => x.Email == esq.Email).ToList().FirstOrDefault();
                if (gui != null)
                {
                    gui.Hash = Funcoes.Codifica(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                    db.Entry(gui).State = EntityState.Modified;
                    db.SaveChanges();
                    string msg = "<h3>Sistema</h3>";
                    msg += "Para alterar sua senha <a href='https://localhost:44374/Home/RedefinirGuia/" + gui.Hash + "' target='_blank'>clique aqui</a>";
                    Funcoes.EnviarEmail(gui.Email, "Redefinição de senha", msg);
                    TempData["MSG"] = "success|Senha redefinida com sucesso!";
                    return RedirectToAction("Index");
                }
                TempData["MSG"] = "error|E-mail não encontrado";
                return View();
            }
            TempData["MSG"] = "warning|Preencha todos os campos";
            return View();
        }

        public ActionResult RedefinirGuia(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var gui = db.Guia.Where(x => x.Hash == id).ToList().FirstOrDefault();
                if (gui != null)
                {
                    try
                    {
                        DateTime dt = Convert.ToDateTime(Funcoes.Decodifica(gui.Hash));
                        if (dt > DateTime.Now)
                        {
                            RedefinirSenha red = new RedefinirSenha();
                            red.Hash = gui.Hash;
                            red.Email = gui.Email;
                            return View(red);
                        }
                        TempData["MSG"] = "warning|Esse link já expirou!";
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        TempData["MSG"] = "error|Hash inválida!";
                        return RedirectToAction("Index");
                    }
                }
                TempData["MSG"] = "error|Hash inválida!";
                return RedirectToAction("Index");
            }
            TempData["MSG"] = "error|Acesso inválido!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RedefinirGuia(RedefinirSenha red)
        {
            if (ModelState.IsValid)
            {
                Contexto db = new Contexto();
                var gui = db.Guia.Where(x => x.Hash == red.Hash).ToList().FirstOrDefault();
                if (gui != null)
                {
                    gui.Hash = null;
                    gui.Senha = Funcoes.HashTexto(red.Senha, "SHA512");
                    db.Entry(gui).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["MSG"] = "success|Senha redefinida com sucesso!";
                    return RedirectToAction("Index");
                }
                TempData["MSG"] = "error|E-mail não encontrado";
                return View(red);
            }
            TempData["MSG"] = "warning|Preencha todos os campos";
            return View(red);
        }
    }
}