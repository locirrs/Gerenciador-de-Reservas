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
    public class ReservaController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#993333";

        // GET: Reserva
        public ActionResult Index()
        {
            ViewBag.color = color;
            var tb_reserva = db.tb_reserva.OrderBy(t => t.data_entrada).Include(t => t.tb_acomodacao).Include(t => t.tb_hospede);
            return View(tb_reserva.ToList());
        }

        // GET: Reserva/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_reserva tb_reserva = db.tb_reserva.Find(id);
            if (tb_reserva == null)
            {
                return HttpNotFound();
            }
            return View(tb_reserva);
        }

        // GET: Reserva/Create
        public ActionResult Create()
        {
            ReservaModel model = new ReservaModel();
            ViewBag.color = color;
            ViewBag.codigo_acomodacao = new SelectList(db.tb_acomodacao, "codigo", "descricao");
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome");

            if (TempData["erro"] != null)
                ViewBag.UserFail = true;
            else
            {
                ViewBag.UserFail = false;
                TempData["erro"] = null;
            }

            return View(model);
        }

        // POST: Reserva/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservaModel model)
        {
            ViewBag.color = color;
            try
            {
                DateTime data_entrada = new DateTime(Convert.ToInt32(model.data_entrada.Split('/')[2]), Convert.ToInt32(model.data_entrada.Split('/')[1]), Convert.ToInt32(model.data_entrada.Split('/')[0]));
                DateTime data_saida = new DateTime(Convert.ToInt32(model.data_saida.Split('/')[2]), Convert.ToInt32(model.data_saida.Split('/')[1]), Convert.ToInt32(model.data_saida.Split('/')[0]));


                var query = from l in db.tb_reserva
                            where (data_entrada >= l.data_entrada && data_entrada <= l.data_saida) && l.codigo_acomodacao == model.codigo_acomodacao
                            select l;

                if (!query.Any())
                {

                    if (ModelState.IsValid)
                    {
                        ViewBag.UserFail = false;
                        TempData["erro"] = null;

                        tb_reserva tb_reserva = new Models.tb_reserva();
                        tb_reserva.codigo_acomodacao = (int)model.codigo_acomodacao;
                        tb_reserva.codigo_hospede = (int)model.codigo_hospede;
                        tb_reserva.data_entrada = data_entrada;
                        tb_reserva.data_saida = data_saida;
                        tb_reserva.qtd_adultos = (int)model.qtd_adultos;
                        tb_reserva.qtd_criancas = (int)model.qtd_criancas;

                        db.tb_reserva.Add(tb_reserva);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.UserFail = true;
                    TempData["erro"] = "erro";
                }
            }
            catch (Exception ex) { }

            ViewBag.codigo_acomodacao = new SelectList(db.tb_acomodacao, "codigo", "descricao", model.codigo_acomodacao);
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", model.codigo_hospede);
            return View(model);
        }

        // GET: Reserva/Edit/5
        public ActionResult Edit(int? id)
        {
            ReservaModel model = new ReservaModel();
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_reserva tb_reserva = db.tb_reserva.Find(id);
            if (tb_reserva == null)
            {
                return HttpNotFound();
            }

            model.codigo = tb_reserva.codigo;
            model.codigo_acomodacao = tb_reserva.codigo_acomodacao;
            model.codigo_hospede = tb_reserva.codigo_hospede;
            model.data_entrada = tb_reserva.data_entrada.Value.ToString("dd/MM/yyyy");
            model.data_saida = tb_reserva.data_saida.Value.ToString("dd/MM/yyyy");
            model.qtd_adultos = tb_reserva.qtd_adultos;
            model.qtd_criancas = tb_reserva.qtd_criancas;
            
            if (TempData["erro"] != null)
                ViewBag.UserFail = true;
            else
            {
                ViewBag.UserFail = false;
                TempData["erro"] = null;
            }

            ViewBag.codigo_acomodacao = new SelectList(db.tb_acomodacao, "codigo", "descricao", tb_reserva.codigo_acomodacao);
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", tb_reserva.codigo_hospede);
            return View(model);
        }

        // POST: Reserva/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservaModel model)
        {
            ViewBag.color = color;

            DateTime data_entrada = new DateTime(Convert.ToInt32(model.data_entrada.Split('/')[2]), Convert.ToInt32(model.data_entrada.Split('/')[1]), Convert.ToInt32(model.data_entrada.Split('/')[0]));
            DateTime data_saida = new DateTime(Convert.ToInt32(model.data_saida.Split('/')[2]), Convert.ToInt32(model.data_saida.Split('/')[1]), Convert.ToInt32(model.data_saida.Split('/')[0]));

            try
            {
                var query = from l in db.tb_reserva
                            where (l.data_entrada>=data_entrada && l.data_saida<=data_entrada) && l.codigo_acomodacao == model.codigo_acomodacao && l.codigo != model.codigo
                            select l;

                if (!query.Any())
                {

                    if (ModelState.IsValid)
                    {
                        ViewBag.UserFail = false;
                        TempData["erro"] = null;

                        tb_reserva tb_reserva = db.tb_reserva.Find(model.codigo);
                        tb_reserva.codigo_acomodacao = (int)model.codigo_acomodacao;
                        tb_reserva.codigo_hospede = (int)model.codigo_hospede;
                        tb_reserva.data_entrada = data_entrada;
                        tb_reserva.data_saida = data_saida;
                        tb_reserva.qtd_adultos = (int)model.qtd_adultos;
                        tb_reserva.qtd_criancas = (int)model.qtd_criancas;

                        db.Entry(tb_reserva).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.UserFail = true;
                    TempData["erro"] = "erro";
                }
            }
            catch (Exception ex) { }


            ViewBag.codigo_acomodacao = new SelectList(db.tb_acomodacao, "codigo", "descricao", model.codigo_acomodacao);
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", model.codigo_hospede);
            return View(model);
        }

        // GET: Reserva/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_reserva tb_reserva = db.tb_reserva.Find(id);
            if (tb_reserva == null)
            {
                return HttpNotFound();
            }
            return View(tb_reserva);
        }

        // POST: Reserva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;
            tb_reserva tb_reserva = db.tb_reserva.Find(id);
            try
            {
                db.tb_reserva.Remove(tb_reserva);
                db.SaveChanges();
            }
            catch (Exception ex) { }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ListaReserva()
        {
            List<tb_reserva> reservas = db.tb_reserva.SqlQuery("select * from tb_reserva inner join tb_hospede on tb_hospede.codigo=tb_reserva.codigo_hospede inner join tb_acomodacao on tb_acomodacao.codigo=tb_reserva.codigo_acomodacao order by data_entrada;").ToList();
            return View(reservas);
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
