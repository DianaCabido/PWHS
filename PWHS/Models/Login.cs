using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace PWHS.Models
{
    public class Login
    {
        public string cpf { get; set; }
        public string senha { get; set; }
        public string usuario { get; set; }
        public string entidade { get; set; }
        public bool usuarioBool { get; set; }
        public bool entidadeBool { get; set; }

    }
}
