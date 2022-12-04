using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LocalWise.Models;

namespace LocalWise.Controllers
{
    public class PontosTuristicosController : Controller
    {
        private Contexto db = new Contexto();

        // GET: PontosTuristicos
        public ActionResult Index()
        {
            return View(db.PontosTuristicos.ToList());
        }

        // GET: PontosTuristicos/Details/5
        public ActionResult Details(int? id)
        {
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

        // GET: PontosTuristicos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PontosTuristicos/Create
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Foto,Nome,Descricao,Tipo")] PontosTuristicos pontosTuristicos)
        {
            if (ModelState.IsValid)
            {
                PontosGuias pog = new PontosGuias();
                pog.GuiaId = Convert.ToInt32(User.Identity.Name.Split('|')[0]);
                pog.PontosTuristicos = pontosTuristicos;

                db.PontosGuias.Add(pog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pontosTuristicos);
        }

        // GET: PontosTuristicos/Edit/5
        public ActionResult Edit(int? id)
        {
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

        // POST: PontosTuristicos/Edit/5
        // Para se proteger de mais ataques, habilite as propriedades específicas às quais você quer se associar. Para 
        // obter mais detalhes, veja https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Foto,Nome,Descricao,Tipo")] PontosTuristicos pontosTuristicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pontosTuristicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pontosTuristicos);
        }

        // GET: PontosTuristicos/Delete/5
        public ActionResult Delete(int? id)
        {
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
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
