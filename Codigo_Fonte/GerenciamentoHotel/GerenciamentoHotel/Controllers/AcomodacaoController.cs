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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace GerenciamentoHotel.Controllers
{
    [Authenticated]
    [Permission]
    public class AcomodacaoController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#cc9933";

        // GET: Acomodacao
        public ActionResult Index()
        {
            ViewBag.color = color;
            return View(db.tb_acomodacao.ToList());
        }

        // GET: Acomodacao/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_acomodacao tb_acomodacao = db.tb_acomodacao.Find(id);
            if (tb_acomodacao == null)
            {
                return HttpNotFound();
            }
            return View(tb_acomodacao);
        }

        // GET: Acomodacao/Create
        public ActionResult Create()
        {
            ViewBag.color = color;
            var items = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Luxo", Value = "1"},
                        new SelectListItem {Text = "Simples", Value = "2"}
                    };

            ViewBag.tipo = items;
            return View();
        }

        // POST: Acomodacao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codigo,descricao,tipo,preco_diaria,numeracao,qtd_pessoas_adultas,qtd_criancas")] tb_acomodacao tb_acomodacao)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    db.tb_acomodacao.Add(tb_acomodacao);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }

            return View(tb_acomodacao);
        }

        // GET: Acomodacao/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_acomodacao tb_acomodacao = db.tb_acomodacao.Find(id);
            if (tb_acomodacao == null)
            {
                return HttpNotFound();
            }
            var items = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Luxo", Value = "1"},
                        new SelectListItem {Text = "Simples", Value = "2"}
                    };

            ViewBag.ddltipo = items;
            return View(tb_acomodacao);
        }

        // POST: Acomodacao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigo,descricao,tipo,preco_diaria,numeracao,qtd_pessoas_adultas,qtd_criancas")] tb_acomodacao tb_acomodacao)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(tb_acomodacao).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }
            return View(tb_acomodacao);
        }

        // GET: Acomodacao/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_acomodacao tb_acomodacao = db.tb_acomodacao.Find(id);
            if (tb_acomodacao == null)
            {
                return HttpNotFound();
            }
            return View(tb_acomodacao);
        }

        // POST: Acomodacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;
            try
            {
                tb_acomodacao tb_acomodacao = db.tb_acomodacao.Find(id);
                db.tb_acomodacao.Remove(tb_acomodacao);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Disponibilidade(string data_entrada)
        {
            string json = string.Empty;

            string dataEntrada = data_entrada.Split('/')[2] + "-" + data_entrada.Split('/')[1] + "-" + data_entrada.Split('/')[0];

            string sql = "SELECT * FROM tb_acomodacao LEFT JOIN ( SELECT codigo_acomodacao FROM tb_reserva WHERE '"+dataEntrada+"'>=data_entrada AND '"+dataEntrada+"'<=data_saida union SELECT codigo_acomodacao FROM tb_checkin WHERE '"+dataEntrada+"'>=data_entrada AND '"+dataEntrada+"'<=data_saida) as tb on tb.codigo_acomodacao=tb_acomodacao.codigo WHERE tb.codigo_acomodacao is null;";

            try
            {
                List<AcomodacaoDTO> acomodacoes = new List<AcomodacaoDTO>();
                List<tb_acomodacao> acomodacoesDB = db.tb_acomodacao.SqlQuery(sql).ToList();
                acomodacoesDB.ForEach(a =>
                {
                    AcomodacaoDTO acomodacao = new AcomodacaoDTO();
                    acomodacao.codigo = a.codigo;
                    acomodacao.nome = a.descricao;
                    acomodacao.numero = a.numeracao;
                    acomodacao.diaria = a.preco_diaria;
                    acomodacao.adultos = (int)a.qtd_pessoas_adultas;
                    acomodacao.criancas = (int)a.qtd_criancas;
                    acomodacoes.Add(acomodacao);
                });
                json = JsonConvert.SerializeObject(acomodacoes,Formatting.None);
            }
            catch (Exception ex)
            {

            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAcomodacao(int codigo)
        {
            string json = string.Empty;
            try
            {
                tb_acomodacao a = db.tb_acomodacao.Find(codigo);
                AcomodacaoDTO acomodacao = new AcomodacaoDTO();
                acomodacao.codigo = a.codigo;
                acomodacao.nome = a.descricao;
                acomodacao.numero = a.numeracao;
                acomodacao.diaria = a.preco_diaria;
                acomodacao.adultos = (int)a.qtd_pessoas_adultas;
                acomodacao.criancas = (int)a.qtd_criancas;

                json = JsonConvert.SerializeObject(acomodacao);
            }
            catch (Exception ex) { }

            return Json(json, JsonRequestBehavior.AllowGet);
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
