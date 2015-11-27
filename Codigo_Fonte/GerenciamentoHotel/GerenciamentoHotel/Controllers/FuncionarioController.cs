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
    public class FuncionarioController : Controller
    {
        private gerenciamento_hotelEntities db = new gerenciamento_hotelEntities();
        string color = "#cc9933";

        // GET: Funcionario
        public ActionResult Index()
        {
            ViewBag.color = color;
            return View(db.tb_funcionario.ToList());
        }

        // GET: Funcionario/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_funcionario tb_funcionario = db.tb_funcionario.Find(id);
            if (tb_funcionario == null)
            {
                return HttpNotFound();
            }
            return View(tb_funcionario);
        }

        // GET: Funcionario/Create
        public ActionResult Create()
        {
            FuncionarioModel model = new FuncionarioModel();
            ViewBag.color = color;
            var items = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "CPF", Value = "1"},
                        new SelectListItem {Text = "RG", Value = "2"}
                    };

            var tipoUsuario = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Administrador", Value = "1"},
                        new SelectListItem {Text = "Funcionário", Value = "2"}
                    };
            ViewBag.tipoUsuario = tipoUsuario;
            ViewBag.tipoDocumento = items;
            return View(model);
        }

        // POST: Funcionario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FuncionarioModel model)
        {
            DateTime data_nascimento = new DateTime(Convert.ToInt32(model.data_nascimento.Split('/')[2]), Convert.ToInt32(model.data_nascimento.Split('/')[1]), Convert.ToInt32(model.data_nascimento.Split('/')[0]));
            ViewBag.color = color;
            if (ModelState.IsValid)
            {
                try
                {
                    tb_funcionario tb_funcionario = new Models.tb_funcionario();
                    tb_funcionario.data_nascimento = data_nascimento;
                    tb_funcionario.documento_identificacao = model.documento_identificacao;
                    tb_funcionario.endereco = model.endereco;
                    tb_funcionario.nome = model.nome;
                    tb_funcionario.senha = model.senha;
                    tb_funcionario.telefone = model.telefone;
                    tb_funcionario.tipo_documento_identificacao = model.tipo_documento_identificacao;
                    tb_funcionario.tipo_usuario = model.tipo_usuario;

                    db.tb_funcionario.Add(tb_funcionario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }

            return View(model);
        }

        // GET: Funcionario/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.color = color;
            FuncionarioModel model = new FuncionarioModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_funcionario tb_funcionario = db.tb_funcionario.Find(id);
            if (tb_funcionario == null)
            {
                return HttpNotFound();
            }

            model.codigo = tb_funcionario.codigo;
            model.data_nascimento = tb_funcionario.data_nascimento.Value.ToString("dd/MM/yyyy");
            model.documento_identificacao = tb_funcionario.documento_identificacao;
            model.endereco = tb_funcionario.endereco;
            model.nome = tb_funcionario.nome;
            model.senha = tb_funcionario.senha;
            model.telefone = tb_funcionario.telefone;
            model.tipo_documento_identificacao = tb_funcionario.tipo_documento_identificacao;
            model.tipo_usuario = tb_funcionario.tipo_usuario;

            var items = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "CPF", Value = "1"},
                        new SelectListItem {Text = "RG", Value = "2"}
                    };
            var tipoUsuario = new List<SelectListItem>()
                    {
                        new SelectListItem {Text = "Selecione", Value = "0"},
                        new SelectListItem {Text = "Administrador", Value = "1"},
                        new SelectListItem {Text = "Funcionário", Value = "2"}
                    };
            ViewBag.tipoUsuario = tipoUsuario;
            ViewBag.tipoDocumento = items;
            return View(model);
        }

        // POST: Funcionario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FuncionarioModel model)
        {
            ViewBag.color = color;
            DateTime data_nascimento = new DateTime(Convert.ToInt32(model.data_nascimento.Split('/')[2]), Convert.ToInt32(model.data_nascimento.Split('/')[1]), Convert.ToInt32(model.data_nascimento.Split('/')[0]));
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(model.senha))
                    {
                        tb_funcionario tb_funcionario_ori = db.tb_funcionario.Find(model.codigo);
                        model.senha = tb_funcionario_ori.senha;
                        db.Entry(tb_funcionario_ori).State = EntityState.Detached;
                    }

                    tb_funcionario tb_funcionario = db.tb_funcionario.Find(model.codigo);
                    tb_funcionario.data_nascimento = data_nascimento;
                    tb_funcionario.documento_identificacao = model.documento_identificacao;
                    tb_funcionario.endereco = model.endereco;
                    tb_funcionario.nome = model.nome;
                    tb_funcionario.senha = model.senha;
                    tb_funcionario.telefone = model.telefone;
                    tb_funcionario.tipo_documento_identificacao = model.tipo_documento_identificacao;
                    tb_funcionario.tipo_usuario = model.tipo_usuario;

                    db.Entry(tb_funcionario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }
            return View(model);
        }
       

        // GET: Funcionario/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.color = color;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_funcionario tb_funcionario = db.tb_funcionario.Find(id);
            if (tb_funcionario == null)
            {
                return HttpNotFound();
            }
            return View(tb_funcionario);
        }

        // POST: Funcionario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.color = color;
            tb_funcionario tb_funcionario = db.tb_funcionario.Find(id);
            try
            {
                db.tb_funcionario.Remove(tb_funcionario);
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
