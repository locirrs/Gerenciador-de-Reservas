using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class Checkout
    {
        public int codigo { get; set; }
        [Display(Name = "Código Check-in")]
        public int codigo_checkin { get; set; }
        [Display(Name = "Data de Saída")]
        public string data_saida { get; set; }
        [Display(Name = "Hora de Saída")]
        public string hora_saida { get; set; }
        [Display(Name = "Número de Diárias")]
        public int numero_diarias { get; set; }
        [Display(Name = "Valor da Diária")]
        public string valor_diaria { get; set; }
        [Display(Name = "Valor de Telefonemas")]
        public string valor_telefonemas { get; set; }
        [Display(Name = "Valor de Consumo")]
        public string valor_consumo { get; set; }
        [Display(Name = "Valor Total")]
        public string valor_total { get; set; }
        [Display(Name = "Forma de Pagamento")]
        public short forma_pagamento { get; set; }
}
}