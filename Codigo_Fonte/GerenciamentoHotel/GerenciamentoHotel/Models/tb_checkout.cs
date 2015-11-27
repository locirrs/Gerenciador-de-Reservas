//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GerenciamentoHotel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class tb_checkout
    {
        public int codigo { get; set; }
        public int codigo_checkin { get; set; }
        [Display(Name = "Data de Saída")]
        public Nullable<System.DateTime> data_saida { get; set; }
        [Display(Name = "Hora de Saída")]
        public string hora_saida { get; set; }
        [Display(Name = "Núm. Diárias")]
        public Nullable<int> numero_diarias { get; set; }
        [Display(Name = "Valor da Diária")]
        public Nullable<decimal> valor_diária { get; set; }
        [Display(Name = "Valor de Telefonemas")]
        public Nullable<decimal> valor_telefonemas { get; set; }
        [Display(Name = "Valor de Consumo")]
        public Nullable<decimal> valor_consumo { get; set; }
        [Display(Name = "Valor Total")]
        public Nullable<decimal> valor_total { get; set; }
        [Display(Name = "Forma de Pagamento")]
        public Nullable<short> forma_pagamento { get; set; }
    
        public virtual tb_checkin tb_checkin { get; set; }
    }
}