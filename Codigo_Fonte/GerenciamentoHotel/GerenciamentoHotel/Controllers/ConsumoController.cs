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
    public class ConsumoController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#cc9933";

        // GET: Consumo
        public ActionResult Index()
        {
            ViewBag.color = color;
            var tb_consumo = db.tb_consumo.Include(t => t.tb_checkin).Include(t => t.tb_itens_consumo);
            return View(tb_consumo.ToList());
        }

        // GET: Consumo/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_consumo tb_consumo = db.tb_consumo.Find(id);
            if (tb_consumo == null)
            {
                return HttpNotFound();
            }
            return View(tb_consumo);
        }

        // GET: Consumo/Create
        public ActionResult Create()
        {
            ViewBag.color = color;
            Consumo model = new Consumo();
            ViewBag.codigo_checkin = new SelectList(db.tb_checkin.Where(x => x.status == 1).Include("tb_hospede"), "codigo", "tb_hospede.nome");
            ViewBag.codigo_item_consumo = new SelectList(db.tb_itens_consumo, "codigo", "descricao");
            return View(model);
        }

        // POST: Consumo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Consumo consumo)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    decimal valor_unitario = 0M;
                    Decimal.TryParse(consumo.valor_unitario.Replace(".", ""), out valor_unitario);
                    decimal valor_final = 0M;

                    valor_final = consumo.quantidade * valor_unitario;

                    tb_consumo tb_consumo = new tb_consumo();
                    tb_consumo.codigo_checkin = consumo.codigo_checkin;
                    tb_consumo.codigo_item_consumo = consumo.codigo_item_consumo;
                    tb_consumo.data_consumo = consumo.data_consumo;
                    tb_consumo.quantidade = consumo.quantidade;
                    tb_consumo.valor_final = valor_final;
                    tb_consumo.valor_unitario = valor_unitario;

                    db.tb_consumo.Add(tb_consumo);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }

            ViewBag.codigo_checkin = new SelectList(db.tb_checkin, "codigo", "hora_entrada", consumo.codigo_checkin);
            ViewBag.codigo_item_consumo = new SelectList(db.tb_itens_consumo, "codigo", "descricao", consumo.codigo_item_consumo);
            return View(consumo);
        }

        // GET: Consumo/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_consumo tb_consumo = db.tb_consumo.Find(id);
            if (tb_consumo == null)
            {
                return HttpNotFound();
            }

            Consumo consumo = new Consumo();

            consumo.codigo = tb_consumo.codigo;
            consumo.codigo_checkin = tb_consumo.codigo_checkin;
            consumo.codigo_item_consumo = tb_consumo.codigo_item_consumo;
            consumo.data_consumo = tb_consumo.data_consumo;
            consumo.quantidade = (int)tb_consumo.quantidade;
            consumo.valor_final = tb_consumo.valor_final.ToString();
            consumo.valor_unitario = tb_consumo.valor_unitario.ToString();


            ViewBag.codigo_checkin = new SelectList(db.tb_checkin.Where(x => x.status == 1).Include("tb_hospede"), "codigo", "tb_hospede.nome");
            ViewBag.codigo_item_consumo = new SelectList(db.tb_itens_consumo, "codigo", "descricao", tb_consumo.codigo_item_consumo);
            return View(consumo);
        }

        // POST: Consumo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Consumo consumo)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    decimal valor_unitario = 0M;
                    Decimal.TryParse(consumo.valor_unitario.Replace(".", ""), out valor_unitario);
                    decimal valor_final = 0M;

                    valor_final = consumo.quantidade * valor_unitario;

                    tb_consumo tb_consumo = db.tb_consumo.Find(consumo.codigo);
                    tb_consumo.codigo_checkin = consumo.codigo_checkin;
                    tb_consumo.codigo_item_consumo = consumo.codigo_item_consumo;
                    tb_consumo.data_consumo = consumo.data_consumo;
                    tb_consumo.quantidade = consumo.quantidade;
                    tb_consumo.valor_final = valor_final;
                    tb_consumo.valor_unitario = valor_unitario;

                    db.Entry(tb_consumo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }
            ViewBag.codigo_checkin = new SelectList(db.tb_checkin, "codigo", "hora_entrada", consumo.codigo_checkin);
            ViewBag.codigo_item_consumo = new SelectList(db.tb_itens_consumo, "codigo", "descricao", consumo.codigo_item_consumo);
            return View(consumo);
        }

        // GET: Consumo/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_consumo tb_consumo = db.tb_consumo.Find(id);
            if (tb_consumo == null)
            {
                return HttpNotFound();
            }
            return View(tb_consumo);
        }

        // POST: Consumo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;
            tb_consumo tb_consumo = db.tb_consumo.Find(id);
            try
            {
                db.tb_consumo.Remove(tb_consumo);
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
