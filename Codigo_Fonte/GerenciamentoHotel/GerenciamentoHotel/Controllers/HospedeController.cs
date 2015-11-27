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
    public class HospedeController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#339966";

        // GET: Hospede
        public ActionResult Index()
        {
            ViewBag.color = color;
            return View(db.tb_hospede.ToList());
        }

        // GET: Hospede/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_hospede tb_hospede = db.tb_hospede.Find(id);
            if (tb_hospede == null)
            {
                return HttpNotFound();
            }
            return View(tb_hospede);
        }

        // GET: Hospede/Create
        public ActionResult Create()
        {
            HospedeModel model = new HospedeModel();
            ViewBag.color = color;
            var items = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "CPF", Value = "1"},
                        new SelectListItem {Text = "RG", Value = "2"}
                    }; 

        ViewBag.tipoDocumento = items;
            return View(model);
        }

        // POST: Hospede/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HospedeModel model)
        {
            ViewBag.color = color;
            DateTime data_nascimento = new DateTime(Convert.ToInt32(model.data_nascimento.Split('/')[2]), Convert.ToInt32(model.data_nascimento.Split('/')[1]), Convert.ToInt32(model.data_nascimento.Split('/')[0]));
            if (ModelState.IsValid)
            {
                try
                {
                    tb_hospede tb_hospede = new Models.tb_hospede();

                    tb_hospede.data_nascimento = data_nascimento;
                    tb_hospede.documento_identificacao = model.documento_identificacao;
                    tb_hospede.email = model.email;
                    tb_hospede.endereco = model.endereco;
                    tb_hospede.nome = model.nome;
                    tb_hospede.nome_pai_mae = model.nome_pai_mae;
                    tb_hospede.tipo_documento_identificacao = model.tipo_documento_identificacao;

                    db.tb_hospede.Add(tb_hospede);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }

            return View(model);
        }

        // GET: Hospede/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.color = color;
            HospedeModel model = new HospedeModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_hospede tb_hospede = db.tb_hospede.Find(id);
            if (tb_hospede == null)
            {
                return HttpNotFound();
            }

            model.codigo = tb_hospede.codigo;
            model.data_nascimento = tb_hospede.data_nascimento.Value.ToString("dd/MM/yyyy");
            model.documento_identificacao = tb_hospede.documento_identificacao;
            model.email = tb_hospede.email;
            model.endereco = tb_hospede.endereco;
            model.nome = tb_hospede.nome;
            model.nome_pai_mae = tb_hospede.nome_pai_mae;
            model.tipo_documento_identificacao = tb_hospede.tipo_documento_identificacao;

            var items = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "CPF", Value = "1"},
                        new SelectListItem {Text = "RG", Value = "2"}
                    };

            ViewBag.tipoDocumento = items;
            return View(model);
        }

        // POST: Hospede/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HospedeModel model)
        {
            ViewBag.color = color;
            DateTime data_nascimento = new DateTime(Convert.ToInt32(model.data_nascimento.Split('/')[2]), Convert.ToInt32(model.data_nascimento.Split('/')[1]), Convert.ToInt32(model.data_nascimento.Split('/')[0]));
            if (ModelState.IsValid)
            {
                try
                {
                    tb_hospede tb_hospede = db.tb_hospede.Find(model.codigo);

                    tb_hospede.data_nascimento = data_nascimento;
                    tb_hospede.documento_identificacao = model.documento_identificacao;
                    tb_hospede.email = model.email;
                    tb_hospede.endereco = model.endereco;
                    tb_hospede.nome = model.nome;
                    tb_hospede.nome_pai_mae = model.nome_pai_mae;
                    tb_hospede.tipo_documento_identificacao = model.tipo_documento_identificacao;

                    db.Entry(tb_hospede).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }
            return View(model);
        }

        // GET: Hospede/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_hospede tb_hospede = db.tb_hospede.Find(id);
            if (tb_hospede == null)
            {
                return HttpNotFound();
            }
            return View(tb_hospede);
        }

        // POST: Hospede/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;
            tb_hospede tb_hospede = db.tb_hospede.Find(id);
            try
            {
                db.tb_hospede.Remove(tb_hospede);
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
