using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GerenciamentoHotel.Filters;
using GerenciamentoHotel.ListaHospedeTableAdapters;
using GerenciamentoHotel.Models;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json.Linq;
using Timesheet.Filters;

namespace GerenciamentoHotel.Controllers
{
    [Authenticated]
    [Permission]
    public class CheckinController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#006699";

        // GET: Checkin
        public ActionResult Index()
        {
            ViewBag.color = color;
            var tb_checkin = db.tb_checkin.Include(t => t.tb_acomodacao).Include(t => t.tb_funcionario).Include(t => t.tb_hospede);            
            return View(tb_checkin.ToList());
        }

        public ActionResult ListaHospede()
        {
            var result = db.tb_checkin.SqlQuery("select * from tb_checkin cki left join tb_hospede hsp on (cki.codigo_hospede = hsp.codigo) inner join tb_acomodacao acom ON (acom.codigo=cki.codigo_acomodacao) where cki.status = 1;").ToList();


            return View(result);
        }


        public ActionResult RelatorioListaHospede()
        {
            try
            {
                ReportViewer relatorio = new ReportViewer();
                relatorio.ProcessingMode = ProcessingMode.Local;
                relatorio.LocalReport.ReportPath = Server.MapPath("~\\Relatorios\\ListaHospedeReport.rdlc");
                relatorio.LocalReport.Refresh();

                var dataTableProjetos = new ListaHospedeTableAdapter();
                var dataSetProjetos = new ListaHospede.ListaHospedeDataTable();

                dataTableProjetos.Fill(dataSetProjetos);

                var listaProjetos = dataSetProjetos.Select();
                var dtsProjetos = new ReportDataSource("DataSet1", listaProjetos);
                relatorio.LocalReport.DataSources.Add(dtsProjetos);

                relatorio.ShowPrintButton = true;
                relatorio.LocalReport.Refresh();

                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                    "<DeviceInfo>" +
                    " <OutputFormat>PDF</OutputFormat>" +
                    " <PageWidth>12in</PageWidth>" +
                    " <PageHeight>9in</PageHeight>" +
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

            catch (Exception ex)
            {

                return null;
            }

        }

        // GET: Checkin/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_checkin tb_checkin = db.tb_checkin.Find(id);
            if (tb_checkin == null)
            {
                return HttpNotFound();
            }
            return View(tb_checkin);
        }

        public JsonResult PesquisaValores(int idCheckin, string data)
        {
            ViewBag.color = color;
            var json = new JObject();

            var checkin = db.tb_checkin.FirstOrDefault(x => x.codigo == idCheckin);

            try
            {
                var datFinal = DateTime.Parse(data);

                json["valorDiaria"] = checkin.tb_acomodacao.preco_diaria;

                decimal valor = Enumerable.Sum(db.tb_consumo.Where(x => x.codigo_checkin == idCheckin), items => items.quantidade.Value * items.valor_unitario.Value);

                json["valorConsumo"] = valor;

                var result = datFinal - checkin.data_entrada.Value;
                json["numeroDiaria"] = result.Days;
            }
            catch (Exception ex)
            {

            }

            return Json(json.ToString(), JsonRequestBehavior.AllowGet);
        }

        // GET: Checkin/Create
        public ActionResult Create()
        {
            ViewBag.color = color;
            Reserva model = new Reserva();
            ViewBag.codigo_acomodacao = new SelectList(db.tb_acomodacao, "codigo", "descricao");
            ViewBag.codigo_funcionario = new SelectList(db.tb_funcionario, "codigo", "nome");
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome");

            var status = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Em Andamento", Value = "1"},
                        new SelectListItem {Text = "Finalizada", Value = "2"},
                        new SelectListItem {Text = "Não Realizada", Value = "3"}
                    };

            if (TempData["erro"] != null)
                ViewBag.UserFail = true;
            else
            {
                ViewBag.UserFail = false;
                TempData["erro"] = null;
            }

            ViewBag.status = status;

            return View(model);
        }

        // POST: Checkin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "codigo,data_entrada,hora_entrada,data_saida,hora_saida,codigo_hospede,codigo_acomodacao,valor_diaria,codigo_funcionario,status")] tb_checkin tb_checkin)
        public ActionResult Create(Reserva model)
        {

            DateTime data_entrada = new DateTime(Convert.ToInt32(model.data_entrada.Split('/')[2]), Convert.ToInt32(model.data_entrada.Split('/')[1]), Convert.ToInt32(model.data_entrada.Split('/')[0]));
            DateTime data_saida = new DateTime(Convert.ToInt32(model.data_saida.Split('/')[2]), Convert.ToInt32(model.data_saida.Split('/')[1]), Convert.ToInt32(model.data_saida.Split('/')[0]));

            ViewBag.color = color;
            var query = from l in db.tb_checkin
                        where (data_entrada >= l.data_entrada && data_entrada <= l.data_saida) && l.codigo_acomodacao == model.codigo_acomodacao
                        select l;

            if (!query.Any())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        decimal valor = 0M;
                        Decimal.TryParse(model.valor_diaria.Replace(".", ""), out valor);
                        tb_checkin checkin = new tb_checkin();
                        checkin.codigo_acomodacao = model.codigo_acomodacao;
                        checkin.codigo_funcionario = model.codigo_funcionario;
                        checkin.codigo_hospede = model.codigo_hospede;
                        checkin.data_entrada = data_entrada;
                        checkin.data_saida = data_saida;
                        checkin.hora_entrada = model.hora_entrada;
                        checkin.hora_saida = model.hora_saida;
                        checkin.valor_diaria = valor;
                        checkin.qtd_adultos = model.qtd_adultos;
                        checkin.qtd_criancas = model.qtd_criancas;
                        checkin.status = (short)model.status;
                        db.tb_checkin.Add(checkin);
                        db.SaveChanges();

                        tb_hospede_checkin hospedeCheckin = new tb_hospede_checkin();
                        hospedeCheckin.codigo_checkin = checkin.codigo;
                        hospedeCheckin.codigo_hospede = checkin.codigo_hospede;
                        db.tb_hospede_checkin.Add(hospedeCheckin);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                ViewBag.UserFail = true;
                TempData["erro"] = "erro";
            }

            ViewBag.codigo_acomodacao = new SelectList(db.tb_acomodacao, "codigo", "descricao", model.codigo_acomodacao);
            ViewBag.codigo_funcionario = new SelectList(db.tb_funcionario, "codigo", "nome", model.codigo_funcionario);
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", model.codigo_hospede);
            var status = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Em Andamento", Value = "1"},
                        new SelectListItem {Text = "Finalizada", Value = "2"},
                        new SelectListItem {Text = "Não Realizada", Value = "3"}
                    };

            ViewBag.status = status;
            return View(model);
        }

        // GET: Checkin/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.color = color;
            Reserva model = new Reserva();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_checkin tb_checkin = db.tb_checkin.Find(id);

            if (tb_checkin == null)
            {
                return HttpNotFound();
            }

            if (TempData["erro"] != null)
                ViewBag.UserFail = true;
            else
            {
                ViewBag.UserFail = false;
                TempData["erro"] = null;
            }

            model.codigo = tb_checkin.codigo;
            model.codigo_acomodacao = tb_checkin.codigo_acomodacao;
            model.codigo_funcionario = tb_checkin.codigo_funcionario;
            model.codigo_hospede = tb_checkin.codigo_hospede;
            model.data_entrada = tb_checkin.data_entrada.Value.ToString("dd/MM/yyyy");
            model.data_saida = tb_checkin.data_saida.Value.ToString("dd/MM/yyyy");
            model.hora_entrada = tb_checkin.hora_entrada;
            model.hora_saida = tb_checkin.hora_saida;
            model.status = (int)tb_checkin.status;
            model.tb_hospede = db.tb_hospede.Find(tb_checkin.codigo_hospede);
            model.valor_diaria = tb_checkin.valor_diaria.ToString();
            model.qtd_adultos = (int)tb_checkin.qtd_adultos;
            model.qtd_criancas = (int)tb_checkin.qtd_criancas;


            ViewBag.codigo_acomodacao = new SelectList(db.tb_acomodacao, "codigo", "descricao", tb_checkin.codigo_acomodacao);
            ViewBag.codigo_funcionario = new SelectList(db.tb_funcionario, "codigo", "nome", tb_checkin.codigo_funcionario);
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", tb_checkin.codigo_hospede);
            var status = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Em Andamento", Value = "1"},
                        new SelectListItem {Text = "Finalizada", Value = "2"},
                        new SelectListItem {Text = "Não Realizada", Value = "3"}
                    };

            ViewBag.status_reserva = status;

            return View(model);
        }

        // POST: Checkin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reserva model)
        {
            ViewBag.color = color;
            DateTime data_entrada = new DateTime(Convert.ToInt32(model.data_entrada.Split('/')[2]), Convert.ToInt32(model.data_entrada.Split('/')[1]), Convert.ToInt32(model.data_entrada.Split('/')[0]));
            DateTime data_saida = new DateTime(Convert.ToInt32(model.data_saida.Split('/')[2]), Convert.ToInt32(model.data_saida.Split('/')[1]), Convert.ToInt32(model.data_saida.Split('/')[0]));
            var query = from l in db.tb_checkin
                        where (data_entrada >= l.data_entrada && data_entrada <= l.data_saida) && l.codigo_acomodacao == model.codigo_acomodacao && l.codigo_hospede != model.codigo && l.codigo != model.codigo
                        select l;

            if (!query.Any())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        decimal valor = 0M;
                        Decimal.TryParse(model.valor_diaria.Replace(".", ""), out valor);

                        tb_checkin tb_checkin = db.tb_checkin.Find(model.codigo);

                        tb_checkin.codigo_acomodacao = model.codigo_acomodacao;
                        tb_checkin.codigo_funcionario = model.codigo_funcionario;
                        tb_checkin.codigo_hospede = model.codigo_hospede;
                        tb_checkin.data_entrada = data_entrada;
                        tb_checkin.data_saida = data_saida;
                        tb_checkin.hora_entrada = model.hora_entrada;
                        tb_checkin.hora_saida = model.hora_saida;
                        tb_checkin.valor_diaria = valor;
                        tb_checkin.qtd_adultos = model.qtd_adultos;
                        tb_checkin.qtd_criancas = model.qtd_criancas;
                        tb_checkin.status = (short)model.status;

                        db.Entry(tb_checkin).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                ViewBag.UserFail = true;
                TempData["erro"] = "erro";
            }



            var status = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Em Andamento", Value = "1"},
                        new SelectListItem {Text = "Finalizada", Value = "2"},
                        new SelectListItem {Text = "Não Realizada", Value = "3"}
                    };

            ViewBag.status_reserva = status;
            ViewBag.codigo_acomodacao = new SelectList(db.tb_acomodacao, "codigo", "descricao", model.codigo_acomodacao);
            ViewBag.codigo_funcionario = new SelectList(db.tb_funcionario, "codigo", "nome", model.codigo_funcionario);
            ViewBag.codigo_hospede = new SelectList(db.tb_hospede, "codigo", "nome", model.codigo_hospede);
            return View(model);
        }

        // GET: Checkin/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_checkin tb_checkin = db.tb_checkin.Find(id);
            if (tb_checkin == null)
            {
                return HttpNotFound();
            }
            return View(tb_checkin);
        }

        // POST: Checkin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;

            tb_hospede_checkin hospedeCheckin = null;
            try
            {
                hospedeCheckin = db.tb_hospede_checkin.Where(h => h.codigo_checkin == id).First();
            }
            catch (Exception ex)
            {
            }

            try
            {
                tb_checkin tb_checkin = db.tb_checkin.Find(id);
                if (hospedeCheckin != null)
                {
                    db.tb_hospede_checkin.Remove(hospedeCheckin);
                    db.SaveChanges();
                }


                db.tb_checkin.Remove(tb_checkin);
                db.SaveChanges();
                TempData["error"]="";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Não é possível excluir o check-in, existem informações (por exemplo, Consumo ou Check-out) vinculadas a ele. Exclua-as primeiramente para depois excluir o check-in.";
            }
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
