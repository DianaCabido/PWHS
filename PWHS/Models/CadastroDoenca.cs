using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace PWHS.Models
{
    public class CadastroDoenca
    {

        public string codigoDoenca { get; set; }
        public string IdPessoa { get; set; }
        public List<SelectListItem> Pessoas { get; set; }
        public string IdUsuario { get; set; }
        public int IdStatus { get; set; }
        public int IdDoenca { get; set; }
        public string descricao{ get; set; }
        public string observacoes{ get; set; }
        public List<SelectListItem> Usuarios { get; set; }
        public List<SelectListItem> Doenca { get; set; }
        public List<SelectListItem> Status { get; set; }

    }
}
