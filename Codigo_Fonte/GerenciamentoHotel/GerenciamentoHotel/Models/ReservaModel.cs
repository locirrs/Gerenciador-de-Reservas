using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class ReservaModel
    {
        [Display(Name="Código")]
        public int? codigo { get; set; }
        [Display(Name = "Hóspede")]
        public int? codigo_hospede { get; set; }
        [Display(Name = "Data Entrada")]
        public string data_entrada { get; set; }
        [Display(Name = "Data Saída")]
        public string data_saida { get; set; }
        [Display(Name = "Acomodação")]
        public int? codigo_acomodacao { get; set; }
        [Display(Name = "Adultos")]
        public int? qtd_adultos { get; set; }
        [Display(Name = "Crianças")]
        public int? qtd_criancas { get; set; }
    }
}