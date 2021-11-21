using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using PWHS.Models;

namespace PWHS.Models
{
    public class TelaInicial
    {

        public List<GridConsulta> grid { get; set; }
        public string codigoPerfilUsuario { get; set; }


    }
}
