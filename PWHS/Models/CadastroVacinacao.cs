using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace PWHS.Models
{
    public class CadastroVacinacao
    {
        public string IdPessoa { get; set; }
        public List<SelectListItem> Pessoas { get; set; }
        public SelectListItem pessoaSelect { get; set; }
        public string laboratorio{ get; set; }
        public string lote{ get; set; }
        public string validade{ get; set; }
        public string IdUsuario{ get; set; }
        public string IdVacina{ get; set; }
        public List<SelectListItem> Usuarios { get; set; }
        public SelectListItem usuarioSelect { get; set; }
        public SelectListItem vacinaSelect { get; set; }
        public List<SelectListItem> Vacinas { get; set; }

       
    }
}
