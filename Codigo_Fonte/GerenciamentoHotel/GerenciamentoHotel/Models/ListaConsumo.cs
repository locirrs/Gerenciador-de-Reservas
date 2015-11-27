using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class ListaConsumo
    {

        public DateTime dataSaida { get; set; }
        public string horaSaida { get; set; }
        public decimal telefonemas { get; set; }
        public decimal diaria { get; set; }
        public int numDiarias { get; set; }
        public decimal totalDiarias { get; set; }
        public decimal totalHospedagem { get; set; }
        public List<tb_consumo> consumo { get; set; }

        public string nomeHospede { get; set; }
        public string apartamento { get; set; }
        public DateTime dataEntrada { get; set; }

    }
}