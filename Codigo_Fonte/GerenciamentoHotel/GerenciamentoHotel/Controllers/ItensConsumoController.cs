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
using Newtonsoft.Json;

namespace GerenciamentoHotel.Controllers
{
    [Authenticated]
    [Permission]
    public class ItensConsumoController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#cc9933";

        // GET: ItensConsumo
        public ActionResult Index()
        {
            ViewBag.color = color;
            return View(db.tb_itens_consumo.ToList());
        }

        // GET: ItensConsumo/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_itens_consumo tb_itens_consumo = db.tb_itens_consumo.Find(id);
            if (tb_itens_consumo == null)
            {
                return HttpNotFound();
            }
            return View(tb_itens_consumo);
        }

        // GET: ItensConsumo/Create
        public ActionResult Create()
        {
            ViewBag.color = color;
            ItemConsumo model = new ItemConsumo();
            return View(model);
        }

        // POST: ItensConsumo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "codigo,descricao,valor")] tb_itens_consumo tb_itens_consumo)
        public ActionResult Create(ItemConsumo model)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    decimal valor = 0M;
                    Decimal.TryParse(model.Valor.Replace(".", ""), out valor);
                    tb_itens_consumo item = new tb_itens_consumo();
                    item.descricao = model.Descricao;
                    item.valor = valor;
                    db.tb_itens_consumo.Add(item);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }

            //return View(tb_itens_consumo);
            return View(model);
        }

        // GET: ItensConsumo/Edit/5
        public ActionResult Edit(int? id)            
        {
            ViewBag.color = color;
            ItemConsumo model = new ItemConsumo();
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_itens_consumo tb_itens_consumo = db.tb_itens_consumo.Find(id);
            if (tb_itens_consumo == null)
            {
                return HttpNotFound();
            }
            else
            {
                model.Codigo = tb_itens_consumo.codigo;
                model.Descricao = tb_itens_consumo.descricao;
                model.Valor = tb_itens_consumo.valor.ToString();
            }
            return View(model);
        }

        // POST: ItensConsumo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemConsumo model)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                //db.Entry(tb_itens_consumo).State = EntityState.Modified;
                try
                {
                    decimal valor = 0M;
                    Decimal.TryParse(model.Valor.Replace(".", ""), out valor);
                    tb_itens_consumo tb_item_consumo = db.tb_itens_consumo.Find(model.Codigo);
                    tb_item_consumo.descricao = model.Descricao;
                    tb_item_consumo.valor = valor;
                    db.Entry(tb_item_consumo).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }
            return View(model);
        }

        // GET: ItensConsumo/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_itens_consumo tb_itens_consumo = db.tb_itens_consumo.Find(id);
            if (tb_itens_consumo == null)
            {
                return HttpNotFound();
            }
            return View(tb_itens_consumo);
        }

        [HttpGet]
        public JsonResult GetItemConsumo(int codigo)
        {
            string json = string.Empty;
            try
            {
                tb_itens_consumo item = db.tb_itens_consumo.Find(codigo);
                ItemConsumoDTO dto = new ItemConsumoDTO();
                dto.codigo = item.codigo;
                dto.descricao = item.descricao;
                dto.valor = item.valor.Value.ToString("n2");
                
                json = JsonConvert.SerializeObject(dto);
            }
            catch (Exception ex) { }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // POST: ItensConsumo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;
            tb_itens_consumo tb_itens_consumo = db.tb_itens_consumo.Find(id);
            try
            {
                db.tb_itens_consumo.Remove(tb_itens_consumo);
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
