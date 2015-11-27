using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class FuncionarioModel
    {
        public int codigo { get; set; }
        [Display(Name = "Funcionário")]
        public string nome { get; set; }
        [Display(Name = "Endereço")]
        public string endereco { get; set; }
        [Display(Name = "Telefone")]
        public string telefone { get; set; }
        [Display(Name = "Doc. Identif.")]
        public string documento_identificacao { get; set; }
        [Display(Name = "Tipo Documento")]
        public short? tipo_documento_identificacao { get; set; }
        [Display(Name = "Data de Nascimento")]
        public string data_nascimento { get; set; }
        [Display(Name = "Tipo")]
        public int? tipo_usuario { get; set; }
        [Display(Name = "Senha")]
        public string senha { get; set; }
    }
}