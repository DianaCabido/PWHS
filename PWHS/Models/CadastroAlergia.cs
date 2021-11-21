using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace PWHS.Models
{
    public class CadastroAlergia
    {
        public string codigoAlergia { get; set; }
        public string nomeAlergia { get; set; }
        public string descricao { get; set; }
        public string IdPessoa { get; set; }
        public string IdUsuario{ get; set; }
        public int IdTipoAlergia{ get; set; }
        public int IdStatus{ get; set; }
        public List<SelectListItem> Pessoas { get; set; }
        public List<SelectListItem> Usuarios { get; set; }
        public List<SelectListItem> TipoAlergia { get; set; }
        public List<SelectListItem> Status { get; set; }
       

    }
}
