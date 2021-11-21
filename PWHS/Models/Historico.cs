using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PWHS.Models
{
    public class Historico
    {
        public PessoaFisica pessoa { get; set; }
        public string codigoPerfil { get; set; }
        public IEnumerable<Alergia> listAlergias { get; set; }
        public IEnumerable<Doenca> listDoencas{ get; set; }
        public IEnumerable<Fratura> listFraturas{ get; set; }
        public IEnumerable<Vacinacao> listVacinacoes{ get; set; }

    }
}
