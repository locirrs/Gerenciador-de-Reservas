using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class Reserva
    {
        public int codigo { get; set; }
        [Display(Name = "Data de Entrada")]
        public string data_entrada { get; set; }
        [Display(Name = "Hora de Entrada")]
        public string hora_entrada { get; set; }
        [Display(Name = "Data de Saída")]
        public string data_saida { get; set; }
        [Display(Name = "Hora de Saída")]
        public string hora_saida { get; set; }
        [Display(Name = "Hóspede")]
        public int codigo_hospede { get; set; }
        [Display(Name = "Acomodação")]
        public int codigo_acomodacao { get; set; }
        [Display(Name = "Valor da Diária")]
        public string valor_diaria { get; set; }
        [Display(Name = "Funcionário")]
        public int codigo_funcionario { get; set; }
        [Display(Name = "Status da Reserva")]
        public int status { get; set; }
        [Display(Name = "Qtde de Adultos")]
        public int qtd_adultos { get; set; }
        [Display(Name = "Qtde de Crianças")]
        public int qtd_criancas { get; set; }

        public tb_hospede tb_hospede { get; set; }
    }
}