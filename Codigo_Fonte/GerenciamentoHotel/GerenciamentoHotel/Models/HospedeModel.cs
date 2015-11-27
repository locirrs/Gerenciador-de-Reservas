using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class HospedeModel
    {
        public int codigo { get; set; }
        [Display(Name = "Hóspede")]
        public string nome { get; set; }
        [Display(Name = "Endereço")]
        public string endereco { get; set; }
        [Display(Name = "E-mail")]
        public string email { get; set; }
        [Display(Name = "Doc. Identif.")]
        public string documento_identificacao { get; set; }
        [Display(Name = "Tipo Doc.")]
        public short? tipo_documento_identificacao { get; set; }
        [Display(Name = "Data de Nascimento")]
        public string data_nascimento { get; set; }
        [Display(Name = "Filiação")]
        public string nome_pai_mae { get; set; }
    }
}