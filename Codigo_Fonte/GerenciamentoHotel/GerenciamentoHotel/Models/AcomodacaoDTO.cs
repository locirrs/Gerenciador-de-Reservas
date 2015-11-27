using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class AcomodacaoDTO
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public string numero { get; set; }
        public string diaria { get; set; }
        public int adultos { get; set; }
        public int criancas { get; set; }
    }
}