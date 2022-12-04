using LocalWise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LocalWise.Controllers
{
    public class GuiaController : Controller
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
        public ActionResult Cadastro(CadastroGuia cad)
        {
            if (ModelState.IsValid)
            {
                if (db.Guia.Where(x => x.Email == cad.Email).ToList().Count > 0)
                {
                    ModelState.AddModelError("Email", "E-mail já cadastrado");
                    return View(cad);
                }
                Guia gui = new Guia();
                gui.Tipo = 2;
                gui.Preco = 50;
                gui.Cidade = cad.Cidade;
                gui.Nome = cad.Nome;
                gui.Cpf = cad.Cpf;
                gui.Nascimento = cad.Nascimento;
                gui.Telefone = cad.Telefone;
                gui.Email = cad.Email;
                gui.Senha = Funcoes.HashTexto(cad.Senha, "SHA512");
                gui.ConfirmarSenha = Funcoes.HashTexto(cad.ConfirmaSenha, "SHA512");
                if (gui.Senha == gui.ConfirmarSenha)
                {
                    db.Guia.Add(gui);
                    db.SaveChanges();
                    TempData["MSG"] = "success|Cadastro realizado com sucesso";
                    return RedirectToAction("../Msg/Analise");
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
        public ActionResult Acesso(AcessoGuia ace)
        {
            if (ModelState.IsValid)
            {
                string senhacrip = Funcoes.HashTexto(ace.Senha, "SHA512");
                var gui = db.Guia.Where(x => x.Email == ace.Email && x.Senha == senhacrip).ToList().FirstOrDefault();

                if (gui == null)
                {
                    ModelState.AddModelError("", "Usuário ou senha inválido");
                    return View(ace);
                }
                ViewBag.Guia = gui.Nome;
                FormsAuthentication.SetAuthCookie(gui.Id + "|" + gui.Nome + "|" + gui.Foto + "|" + gui.Tipo, false);
                return RedirectToAction("../Guia/Pontos");
            }
            return View(ace);
        }

        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult EditPreco(int? id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPreco([Bind(Include = "Preco")] Guia guia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Pontos");
            }
            return View(guia);
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
            Guia guia = db.Guia.Find(id);
            string valor = "";

            if (arquivo != null)
            {
                Funcoes.CriarDiretorio();
                string nomearq = "FotoPessoa" + guia.Id + ".png";
                valor = Funcoes.UploadArquivo(arquivo, nomearq);
                if (valor == "sucesso") //comparar se o valor é igual a sucesso
                {
                    guia.Foto = nomearq;
                    db.Entry(guia).State = EntityState.Modified; //estado da pessoa foi modificado
                    db.SaveChanges();
                    TempData["MSG"] = "success|Foto enviada com sucesso";
                    return RedirectToAction("Pontos", "Guia");
                }
            }
            else
            {
                System.IO.File.Copy(Request.PhysicalApplicationPath + "Upload\\user-icon.png", Request.PhysicalApplicationPath +
                "Upload\\FotoPessoa" + guia.Id + ".png");
                guia.Foto = "FotoPessoa" + guia.Id + ".png";
                db.Entry(guia).State = EntityState.Modified;
                db.SaveChanges();
                TempData["MSG"] = "success|Foto enviada com sucesso"; //sweet alert
                return RedirectToAction("Pontos", "Guia");
            }
            return View(guia);
        }

        [Authorize]
        public ActionResult CreatePontos()
        {
            int guiaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
            Guia guia = db.Guia.Find(guiaId);
            ViewBag.Guia = guia;

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePontos([Bind(Include = "Id,Nome,Descricao,Tipo")] PontosTuristicos pontosTuristicos, HttpPostedFileBase arquivo)
        {
            int guiaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
            Guia guia = db.Guia.Find(guiaId);
            ViewBag.Guia = guia;

            if (ModelState.IsValid)
            {
                PontosGuias pog = new PontosGuias();
                pog.GuiaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
                pog.PontosTuristicos = pontosTuristicos;

                string valor = "";

                if (arquivo != null)
                {
                    Funcoes.CriarDiretorio();
                    string nomearq = "PontoTuristico" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
                    valor = Funcoes.UploadArquivo(arquivo, nomearq);
                    if (valor == "sucesso") //comparar se o valor é igual a sucesso
                    {
                        pog.PontosTuristicos.Foto = nomearq;
                        //db.Entry(guia).State = EntityState.Modified; //estado da pessoa foi modificado
                        db.PontosGuias.Add(pog);
                        db.SaveChanges();
                        TempData["MSG"] = "success|Foto enviada com sucesso";
                        return RedirectToAction("Pontos");
                    }
                }
            }
            return View(pontosTuristicos);
        }

        [Authorize]
        public ActionResult EditPonto(int? id)
        {
            int guiaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
            Guia guiaA = db.Guia.Find(guiaId);
            ViewBag.Guia = guiaA;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var guia = Convert.ToInt32(User.Identity.Name.Split('|')[0]);

            PontosTuristicos pontosTuristicos = db.PontosTuristicos.Where(x => x.PontosGuias.Any(y => y.GuiaId == guia) && x.Id == id).ToList().FirstOrDefault();
            if (pontosTuristicos == null)
            {
                return HttpNotFound();
            }
            return View(pontosTuristicos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPonto([Bind(Include = "Id,Foto,Nome,Descricao,Tipo")] PontosTuristicos pontosTuristicos, HttpPostedFileBase arquivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pontosTuristicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Pontos");
            }
            return View(pontosTuristicos);
        }

        public ActionResult Delete(int? id)
        {
            int guiaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
            Guia guia = db.Guia.Find(guiaId);
            ViewBag.Guia = guia;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PontosTuristicos pontosTuristicos = db.PontosTuristicos.Find(id);
            if (pontosTuristicos == null)
            {
                return HttpNotFound();
            }
            return View(pontosTuristicos);
        }

        // POST: PontosTuristicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PontosTuristicos pontosTuristicos = db.PontosTuristicos.Find(id);
            db.PontosTuristicos.Remove(pontosTuristicos);
            db.SaveChanges();
            return RedirectToAction("Pontos");
        }

        [Authorize]
        public ActionResult Passeios(int? id)
        {
            Guia guia;
            if (id == null && User.Identity.IsAuthenticated)
            {
                int guiaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
                guia = db.Guia.Find(guiaId);
            }
            else if (id != null)
            {
                guia = db.Guia.Find(id);
            }
            else
            {
                guia = null;
            }
            return View(guia);
        }

        //[Authorize]
        public ActionResult Pontos(int? id)
        {
            var dados = HttpContext.Request.Cookies["cookie"];
            if (dados != null)
            {
                Pedido ped = new Pedido();
                foreach (var item in dados.Values.ToString().Split('&'))
                {
                    if (item.Split('=')[0] == "local")
                        ped.Cidade = item.Split('=')[1];
                    if (item.Split('=')[0] == "qtde")
                        ped.QtdPessoas = Convert.ToInt32(item.Split('=')[1]);
                    if (item.Split('=')[0] == "trans")
                        ped.Locomocao = item.Split('=')[1];
                    if (item.Split('=')[0] == "tipo")
                        ped.Tipo = item.Split('=')[1];
                    if (item.Split('=')[0] == "data")
                        ped.Data = item.Split('=')[1];

                }
                ViewBag.Dados = ped;
            }
            List<PontosGuias> pontosGuia = new List<PontosGuias>();
            //int id = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
            if (id != null)
            {
                pontosGuia = db.PontosGuias.Where(x => x.GuiaId == id).ToList();
                Guia guia = db.Guia.Find(id);
                ViewBag.Guia = guia;
                return View(pontosGuia);
            }
            else if (User.Identity.IsAuthenticated)
            {
                int guiaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
                Guia guia = db.Guia.Find(guiaId);
                ViewBag.Guia = guia;
                pontosGuia = db.PontosGuias.Where(x => x.GuiaId == guiaId).ToList();
                return View(pontosGuia);
            }
            else
            {
                return HttpNotFound();
            }
            //}

            //return View(pontosGuia);
        }

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public JsonResult Pedido(string local, int qtde, int guia, string transporte, string tipoPasseio, string dia)
        {
            Pedido ped = new Pedido();
            ped.Cidade = local;
            ped.QtdPessoas = qtde;
            ped.GuiaId = guia;
            ped.TuristaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
            ped.Locomocao = transporte;
            ped.Tipo = tipoPasseio;
            ped.Data = dia;
            ped.Status = 0;
            if (TryValidateModel(ped))
            {
                db.Pedido.Add(ped);
                db.SaveChanges();
                var cookie = HttpContext.Request.Cookies["cookie"];
                cookie.Expires = DateTime.Now.AddHours(-2);
                Response.Cookies.Add(cookie);
                return Json("OK");
            }
            return Json("ERRO");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public JsonResult Aceitar(string id)
        {
            Pedido ped = db.Pedido.Find(Convert.ToInt32(id));

            if (ped.GuiaId.ToString() == User.Identity.Name.Split('|')[0])
            {
                ped.Status = 1;
                db.Entry(ped).State = EntityState.Modified;
                db.SaveChanges();
                return Json("OK");
            }
            return Json("ERRO");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public JsonResult Recusar(string id)
        {
            Pedido ped = db.Pedido.Find(Convert.ToInt32(id));

            if (ped.GuiaId.ToString() == User.Identity.Name.Split('|')[0])
            {
                ped.Status = 2;
                db.Entry(ped).State = EntityState.Modified;
                db.SaveChanges();
                return Json("OK");
            }
            return Json("ERRO");
        }

        [Authorize]
        public ActionResult Avaliacoes()
        {
            return View();
        }

        [Authorize]
        public ActionResult Cobranca()
        {
            return View();
        }
    }
}