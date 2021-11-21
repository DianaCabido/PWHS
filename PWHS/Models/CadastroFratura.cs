using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace PWHS.Models
{
    public class CadastroFratura
    {
        public string codigoFratura { get; set; }
        public string nomeFratura { get; set; }
        public string descricao { get; set; }
        public string IdPessoa { get; set; }
        public string IdUsuario{ get; set; }
        public int IdStatus{ get; set; }
        public int IdTipoLesao { get; set; }
        public int IdCausaFratura { get; set; }
        public List<SelectListItem> Pessoas { get; set; }
        public List<SelectListItem> Usuarios { get; set; }
        public List<SelectListItem> TipoLesao { get; set; }
        public List<SelectListItem> CausaFratura { get; set; }
        public List<SelectListItem> Status { get; set; }
   
    }
}
