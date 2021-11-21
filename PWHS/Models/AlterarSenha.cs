using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace PWHS.Models
{
    public class AlterarSenha
    {
        public string senhaAtual { get; set; }
        public string senhaNova { get; set; }
        public string confirmacao { get; set; }
     
    }
}
