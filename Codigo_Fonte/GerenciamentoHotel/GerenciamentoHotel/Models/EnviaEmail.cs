﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciamentoHotel.Models
{
    public class EnviaEmail
    {
        public string fullname { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }
}