using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GerenciamentoHotel.CheckoutTableAdapters;
using GerenciamentoHotel.Filters;
using GerenciamentoHotel.ListaConsumoTableAdapters;
using GerenciamentoHotel.ListaHospedeTableAdapters;
using GerenciamentoHotel.Models;
using Microsoft.Reporting.WebForms;
using Timesheet.Filters;

namespace GerenciamentoHotel.Controllers
{
    [Authenticated]
    [Permission]
    public class CheckoutController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#006699";
        // GET: Checkout
        public ActionResult Index()
        {
            ViewBag.color = color;
            var tb_checkout = db.tb_checkout.Include(t => t.tb_checkin);
            return View(tb_checkout.ToList());
        }

        // GET: Checkout/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_checkout tb_checkout = db.tb_checkout.Find(id);
            if (tb_checkout == null)
            {
                return HttpNotFound();
            }
            return View(tb_checkout);
        }

        // GET: Checkout/Create
        public ActionResult Create()
        {
            ViewBag.color = color;
            //ViewBag.codigo_checkin = new SelectList(db.tb_checkin.Include("tb_hospede").Where(x => x.status == 1), "codigo", "tb_hospede.nome");
            var formaPagamento = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Cartão", Value = "1"},
                        new SelectListItem {Text = "Dinheiro", Value = "2"}
                    };

            var checkin = db.tb_checkin.ToList();

            var listaCheckin = new List<SelectListItem>();

            foreach (var tbCheckin in checkin)
            {
                if (tbCheckin.status == 1)
                {
                    listaCheckin.Add(new SelectListItem { Text = tbCheckin.tb_hospede.nome, Value = tbCheckin.codigo.ToString() });
                }
            }


            ViewBag.listaCheckin = listaCheckin;

            ViewBag.formaPagamento = formaPagamento;
            return View();
        }

        // POST: Checkout/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GerenciamentoHotel.Models.Checkout checkout)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime data_saida = new DateTime(Convert.ToInt32(checkout.data_saida.Split('/')[2]), Convert.ToInt32(checkout.data_saida.Split('/')[1]), Convert.ToInt32(checkout.data_saida.Split('/')[0]));
                    tb_checkin tb_checkin = db.tb_checkin.FirstOrDefault(x => x.codigo == checkout.codigo_checkin);
                    tb_checkin.status = 2;
                    db.Entry(tb_checkin).State = EntityState.Modified;

                    if (string.IsNullOrEmpty(checkout.valor_telefonemas))
                    {
                        checkout.valor_telefonemas = "0";
                    }

                    decimal valor_diaria = 0M;
                    Decimal.TryParse(checkout.valor_diaria.Replace(".", ""), out valor_diaria);
                    decimal valor_telefonemas = 0M;
                    Decimal.TryParse(checkout.valor_telefonemas.Replace(".", ""), out valor_telefonemas);
                    decimal valor_consumo = 0M;
                    Decimal.TryParse(checkout.valor_consumo.Replace(".", ""), out valor_consumo);
                    decimal valor_final = 0M;
                    valor_final = valor_consumo + (checkout.numero_diarias * valor_diaria) + valor_telefonemas;
                    tb_checkout tb_checkout = new tb_checkout();
                    tb_checkout.codigo_checkin = checkout.codigo_checkin;
                    tb_checkout.data_saida = data_saida;
                    tb_checkout.forma_pagamento = checkout.forma_pagamento;
                    tb_checkout.hora_saida = checkout.hora_saida;
                    tb_checkout.numero_diarias = checkout.numero_diarias;
                    tb_checkout.valor_consumo = valor_consumo;
                    tb_checkout.valor_diária = valor_diaria;
                    tb_checkout.valor_telefonemas = valor_telefonemas;
                    tb_checkout.valor_total = valor_final;

                    db.tb_checkout.Add(tb_checkout);
                    db.SaveChanges();
                    var formaPagamento = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Cartão", Value = "1"},
                        new SelectListItem {Text = "Dinheiro", Value = "2"}
                    };

                    ViewBag.formaPagamento = formaPagamento;
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }

            ViewBag.codigo_checkin = new SelectList(db.tb_checkin.Where(x => x.status == 1).Include("tb_hospede"), "codigo", "tb_hospede.nome"); 
            return View(checkout);
        }

        // GET: Checkout/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_checkout tb_checkout = db.tb_checkout.Find(id);

            GerenciamentoHotel.Models.Checkout checkout = new Models.Checkout();

            if (tb_checkout == null)
            {
                return HttpNotFound();
            }
            var formaPagamento = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Cartão", Value = "1"},
                        new SelectListItem {Text = "Dinheiro", Value = "2"}
                    };
            var checkin = db.tb_checkin.ToList();

            var listaCheckin = new List<SelectListItem>();

            foreach (var tbCheckin in checkin)
            {
                if (tbCheckin.codigo == tb_checkout.codigo_checkin)
                {
                    listaCheckin.Add(new SelectListItem { Text = tbCheckin.tb_hospede.nome, Value = tbCheckin.codigo.ToString() });
                }
            }

            checkout.codigo = tb_checkout.codigo;
            checkout.codigo_checkin = tb_checkout.codigo_checkin;
            checkout.data_saida = tb_checkout.data_saida.Value.ToString("dd/MM/yyyy");
            checkout.forma_pagamento = (short)tb_checkout.forma_pagamento;
            checkout.hora_saida = tb_checkout.hora_saida;
            checkout.numero_diarias = (int)tb_checkout.numero_diarias;
            checkout.valor_consumo = tb_checkout.valor_consumo.ToString();
            checkout.valor_diaria = tb_checkout.valor_diária.ToString();
            checkout.valor_telefonemas = tb_checkout.valor_telefonemas.ToString();
            checkout.valor_total = tb_checkout.valor_total.ToString();

            ViewBag.listaCheckin = listaCheckin;


            ViewBag.listaCheckin = listaCheckin;
            ViewBag.formaPagamento = formaPagamento;
            ViewBag.codigo_checkin = new SelectList(db.tb_checkin.Where(x => x.status == 1).Include("tb_hospede"), "codigo", "tb_hospede.nome");
            return View(checkout);
        }

        // POST: Checkout/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GerenciamentoHotel.Models.Checkout model)
        {
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime data_saida = new DateTime(Convert.ToInt32(model.data_saida.Split('/')[2]), Convert.ToInt32(model.data_saida.Split('/')[1]), Convert.ToInt32(model.data_saida.Split('/')[0]));

                    tb_checkin tb_checkin = db.tb_checkin.FirstOrDefault(x => x.codigo == model.codigo_checkin);
                    tb_checkin.status = 2;
                    db.Entry(tb_checkin).State = EntityState.Modified;

                    tb_checkout tb_checkout = db.tb_checkout.Find(model.codigo);
                    if (string.IsNullOrEmpty(model.valor_telefonemas))
                    {
                        model.valor_telefonemas = "0";
                    }

                    decimal valor_diaria = 0M;
                    Decimal.TryParse(model.valor_diaria.Replace(".", ""), out valor_diaria);
                    decimal valor_telefonemas = 0M;
                    Decimal.TryParse(model.valor_telefonemas.Replace(".", ""), out valor_telefonemas);
                    decimal valor_consumo = 0M;
                    Decimal.TryParse(model.valor_consumo.Replace(".", ""), out valor_consumo);
                    decimal valor_final = 0M;
                    valor_final = valor_consumo + (model.numero_diarias * valor_diaria) + valor_telefonemas;


                    tb_checkout.codigo_checkin = model.codigo_checkin;
                    tb_checkout.data_saida = data_saida;
                    tb_checkout.forma_pagamento = model.forma_pagamento;
                    tb_checkout.hora_saida = model.hora_saida;
                    tb_checkout.numero_diarias = model.numero_diarias;
                    tb_checkout.valor_consumo = valor_consumo;
                    tb_checkout.valor_diária = valor_diaria;
                    tb_checkout.valor_telefonemas = valor_telefonemas;
                    tb_checkout.valor_total = valor_final;


                    tb_checkout.tb_checkin = tb_checkin;

                    db.Entry(tb_checkout).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }
            var formaPagamento = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Cartão", Value = "1"},
                        new SelectListItem {Text = "Dinheiro", Value = "2"}
                    };

            ViewBag.formaPagamento = formaPagamento;
            ViewBag.codigo_checkin = new SelectList(db.tb_checkin, "codigo", "hora_entrada", model.codigo_checkin);
            return View(model);
        }

        // GET: Checkout/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_checkout tb_checkout = db.tb_checkout.Find(id);
            if (tb_checkout == null)
            {
                return HttpNotFound();
            }
            return View(tb_checkout);
        }

        public ActionResult ListaConsumo(int? id)
        {
            Models.ListaConsumo listaConsumo = new Models.ListaConsumo();
            tb_checkout checkout = db.tb_checkout.Find(id);
            tb_checkin checkin = db.tb_checkin.Find(checkout.codigo_checkin);
            tb_acomodacao acomodacao = db.tb_acomodacao.Find(checkin.codigo_acomodacao);
            tb_hospede hospede = db.tb_hospede.Find(checkin.codigo_hospede);

            listaConsumo.nomeHospede = hospede.nome;
            listaConsumo.apartamento = acomodacao.numeracao;
            listaConsumo.dataEntrada = (DateTime)checkin.data_entrada;

            listaConsumo.dataSaida = (DateTime)checkout.data_saida;
            listaConsumo.horaSaida = checkout.hora_saida;
            listaConsumo.telefonemas = (decimal)checkout.valor_telefonemas;
            listaConsumo.diaria = (decimal)checkout.valor_diária;
            listaConsumo.numDiarias = (int)checkout.numero_diarias;
            listaConsumo.totalDiarias = listaConsumo.diaria * listaConsumo.numDiarias;

            listaConsumo.consumo = db.tb_consumo.SqlQuery("select * from tb_consumo inner join tb_itens_consumo on tb_itens_consumo.codigo=tb_consumo.codigo_item_consumo where tb_consumo.codigo_checkin="+checkout.codigo_checkin.ToString()+";").ToList();

            listaConsumo.consumo.ForEach(l =>
            {
                listaConsumo.totalHospedagem += (decimal)l.valor_final;
            });

            listaConsumo.totalHospedagem += listaConsumo.telefonemas + listaConsumo.totalDiarias;

            return View(listaConsumo);
        }

        public ActionResult ImprimirConsumo(int? id)
        {
            ViewBag.color = color;
            tb_checkout tb_checkout = db.tb_checkout.Find(id);

            ReportViewer relatorio = new ReportViewer();
            relatorio.ProcessingMode = ProcessingMode.Local;
            relatorio.LocalReport.ReportPath = Server.MapPath("~\\Relatorios\\ListaConsumo.rdlc");
            relatorio.LocalReport.Refresh();

            var dataTableProjetos = new tb_checkoutTableAdapter();
            var dataSetProjetos = new Checkout.tb_checkoutDataTable();

            dataTableProjetos.Fill(dataSetProjetos);

            var listaProjetos = dataSetProjetos.Select("codigo=" + id);
            var dtsProjetos = new ReportDataSource("DataSet1", listaProjetos);


            var dataTableConsumo = new ListaConsumoTableAdapter();
            var dataSetConsumo = new ListaConsumo.ListaConsumoDataTable();

            dataTableConsumo.Fill(dataSetConsumo);

            var listaConsumo = dataSetConsumo.Select("codigo_checkin=" + tb_checkout.codigo_checkin);
            var dtsConsumo = new ReportDataSource("DataSet2", listaConsumo);

            relatorio.LocalReport.DataSources.Add(dtsProjetos);
            relatorio.LocalReport.DataSources.Add(dtsConsumo);


            relatorio.ShowPrintButton = true;
            relatorio.LocalReport.Refresh();

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =
                "<DeviceInfo>" +
                " <OutputFormat>PDF</OutputFormat>" +
                " <PageWidth>9in</PageWidth>" +
                " <PageHeight>12in</PageHeight>" +
                " <MarginTop>0.2in</MarginTop>" +
                " <MarginLeft>.7in</MarginLeft>" +
                " <MarginRight>.8in</MarginRight>" +
                " <MarginBottom>0.2in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] bytes;

            //Renderiza o relatório em bytes
            bytes = relatorio.LocalReport.Render(
            reportType,
            deviceInfo,
            out mimeType,
            out encoding,
            out fileNameExtension,
            out streams,
            out warnings);

            return File(bytes, mimeType);
        }

        

        // POST: Checkout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;
            tb_checkout tb_checkout = db.tb_checkout.Find(id);
            try
            {
                db.tb_checkout.Remove(tb_checkout);
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
