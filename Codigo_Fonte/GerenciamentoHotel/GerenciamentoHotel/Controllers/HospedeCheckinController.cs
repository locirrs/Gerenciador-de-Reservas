using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GerenciamentoHotel.Filters;
using GerenciamentoHotel.Models;
using Timesheet.Filters;

namespace GerenciamentoHotel.Controllers
{
    [Authenticated]
    [Permission]
    public class HospedeCheckinController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#999933";

        // GET: HospedeCheckin
        public ActionResult Index()
        {
            ViewBag.color = color;
            var tb_hospede_checkin = db.tb_hospede_checkin.Include(t => t.tb_checkin).Include(t => t.tb_hospede);
            return View(tb_hospede_checkin.ToList());
        }

        // GET: HospedeCheckin/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_hospede_checkin tb_hospede_checkin = db.tb_hospede_checkin.Find(id);
            if (tb_hospede_checkin == null)
            {
                return HttpNotFound();
            }
            return View(tb_hospede_checkin);
        }

        // GET: HospedeCheckin/Create
        public ActionResult Create()
        {
            ViewBag.color = color;
            ViewBag.codigo_checkin = new SelectList(db.tb_checkin.Where(x => x.status == 1).Include("tb_hospede"), "codigo", "tb_hospede.nome");
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome");
            return View();
        }

        // POST: HospedeCheckin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codigo,codigo_checkin,codigo_hospede")] tb_hospede_checkin tb_hospede_checkin)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    db.tb_hospede_checkin.Add(tb_hospede_checkin);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }

            ViewBag.codigo_checkin = new SelectList(db.tb_checkin.Where(x => x.status == 1).Include("tb_hospede"), "codigo", "tb_hospede.nome");
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", tb_hospede_checkin.codigo_hospede);
            return View(tb_hospede_checkin);
        }

        // GET: HospedeCheckin/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_hospede_checkin tb_hospede_checkin = db.tb_hospede_checkin.Find(id);
            if (tb_hospede_checkin == null)
            {
                return HttpNotFound();
            }
            ViewBag.codigo_checkin = new SelectList(db.tb_checkin, "codigo", "hora_entrada", tb_hospede_checkin.codigo_checkin);
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", tb_hospede_checkin.codigo_hospede);
            return View(tb_hospede_checkin);
        }

        // POST: HospedeCheckin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigo,codigo_checkin,codigo_hospede")] tb_hospede_checkin tb_hospede_checkin)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(tb_hospede_checkin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } catch (Exception ex) { }
            }
            ViewBag.codigo_checkin = new SelectList(db.tb_checkin, "codigo", "hora_entrada", tb_hospede_checkin.codigo_checkin);
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", tb_hospede_checkin.codigo_hospede);
            return View(tb_hospede_checkin);
        }

        // GET: HospedeCheckin/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_hospede_checkin tb_hospede_checkin = db.tb_hospede_checkin.Find(id);
            if (tb_hospede_checkin == null)
            {
                return HttpNotFound();
            }
            return View(tb_hospede_checkin);
        }

        // POST: HospedeCheckin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;
            tb_hospede_checkin tb_hospede_checkin = db.tb_hospede_checkin.Find(id);
            try
            {
                db.tb_hospede_checkin.Remove(tb_hospede_checkin);
                db.SaveChanges();
            }
            catch (Exception ex) { }
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
