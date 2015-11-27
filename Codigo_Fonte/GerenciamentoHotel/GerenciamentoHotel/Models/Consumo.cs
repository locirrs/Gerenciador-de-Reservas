using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class Consumo
    {
        public int codigo { get; set; }
        [Display(Name = "Data do Consumo")]
        public string data_consumo { get; set; }
        [Display(Name = "Check-in")]
        public int codigo_checkin { get; set; }
        [Display(Name = "Item")]
        public int codigo_item_consumo { get; set; }
        [Display(Name = "Quantidade")]
        public int quantidade { get; set; }
        [Display(Name = "Valor unitário")]
        public string valor_unitario { get; set; }
        [Display(Name = "Valor Total")]
        public string valor_final { get; set; }
    }
}