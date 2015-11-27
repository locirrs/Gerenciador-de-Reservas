using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GerenciamentoHotel.ListaHospedeTableAdapters;
using Microsoft.Reporting.WebForms;

namespace GerenciamentoHotel
{
    public partial class ListaHospedePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowReport();
            }
        }


        private void ShowReport()
        {
            relatorio.Reset();


            var dataTableProjetos = new ListaHospedeTableAdapter();
            var dataSetProjetos = new ListaHospede.ListaHospedeDataTable();

            dataTableProjetos.Fill(dataSetProjetos);

            var listaProjetos = dataSetProjetos.Select();
            var dtsProjetos = new ReportDataSource("DataSet1", listaProjetos);
            relatorio.LocalReport.DataSources.Add(dtsProjetos);

            relatorio.LocalReport.ReportPath = Server.MapPath("Relatorios\\ListaHospedeReport.rdlc");
            relatorio.ShowPrintButton = true;
            relatorio.LocalReport.Refresh();


        }

    }
}