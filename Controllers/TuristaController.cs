using LocalWise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LocalWise.Controllers
{
    public class TuristaController : Controller
    {
        public Contexto db = new Contexto();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(CadastroTur cad)
        {

            if (ModelState.IsValid)
            {
                if (db.Turista.Where(x => x.Email == cad.Email).ToList().Count > 0)
                {
                    ModelState.AddModelError("Email", "E-mail já cadastrado");
                    return View(cad);
                }
                Turista tur = new Turista();
                tur.Tipo = 1;
                tur.Nome = cad.Nome;
                tur.Cpf = cad.Cpf;
                tur.Nascimento = cad.Nascimento;
                tur.Telefone = cad.Telefone;
                tur.Email = cad.Email;
                tur.Senha = Funcoes.HashTexto(cad.Senha, "SHA512");
                tur.ConfirmarSenha = Funcoes.HashTexto(cad.ConfirmaSenha, "SHA512");
                if (tur.Senha == tur.ConfirmarSenha)
                {
                    db.Turista.Add(tur);
                    db.SaveChanges();
                    TempData["MSG"] = "success|Cadastro realizado com sucesso";
                    return RedirectToAction("../Msg/MsgTurista");
                }
                else
                {
                    ModelState.AddModelError("ConfirmaSenha", "Senhas nao conhecidem");
                }
            }
            return View(cad);
        }

        public ActionResult Acesso()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Acesso(AcessoGuia ace, string url)
        {
            if (ModelState.IsValid)
            {
                string senhacrip = Funcoes.HashTexto(ace.Senha, "SHA512");
                var tur = db.Turista.Where(x => x.Email == ace.Email && x.Senha == senhacrip).ToList().FirstOrDefault();

                if (tur == null)
                {
                    ModelState.AddModelError("", "Usuário ou senha inválido");
                    return View(ace);
                }
                ViewBag.Turista = tur.Nome;
                FormsAuthentication.SetAuthCookie(tur.Id + "|" + tur.Nome + "|" + tur.Foto + "|" + tur.Tipo, false);
                if (!String.IsNullOrEmpty(url))
                {
                    return RedirectToAction("Pontos", "Guia", new { id = url });
                }
                return RedirectToAction("Index", "Home");
            }
            return View(ace);
        }

        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult DashBoard()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase arquivo)
        {
            int id = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
            Turista tur = db.Turista.Find(id);
            string valor = "";

            if (arquivo != null)
            {
                Funcoes.CriarDiretorio();
                string nomearq = "FotoPessoa" + tur.Id + ".png";
                valor = Funcoes.UploadArquivo(arquivo, nomearq);
                if (valor == "sucesso") //comparar se o valor é igual a sucesso
                {
                    tur.Foto = nomearq;
                    db.Entry(tur).State = EntityState.Modified; //estado da pessoa foi modificado
                    db.SaveChanges();
                    TempData["MSG"] = "success|Foto enviada com sucesso";
                    return RedirectToAction("DashBoard", "Turista");
                }
            }
            else
            {
                System.IO.File.Copy(Request.PhysicalApplicationPath + "Upload\\user-icon.png", Request.PhysicalApplicationPath +
                "Upload\\FotoPessoa" + tur.Id + ".png");
                tur.Foto = "FotoPessoa" + tur.Id + ".png";
                db.Entry(tur).State = EntityState.Modified;
                db.SaveChanges();
                TempData["MSG"] = "success|Foto enviada com sucesso"; //sweet alert
                return RedirectToAction("DashBoard", "Turista");
            }
            return View(tur);
        }
    }
}