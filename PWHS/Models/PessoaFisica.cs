using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PWHS.Models
{
    public class PessoaFisica
    {
        public string cpfPF { get; set; }
        public string cpfUsuario { get; set; }
        public string nomePF { get; set; }
        public string dataNascimento { get; set; }
        public string sexo { get; set; }
        public string tipoSanguineo { get; set; }
        public string email { get; set; }
        public string cep { get; set; }
        public string logadouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string telefone { get; set; }
        public string ddd { get; set; }
        public IEnumerable<SelectListItem> listSexo { get; set; }
        public int IdSexo { get; set; }
        public IEnumerable<SelectListItem> listEstado { get; set; }
        public int IdEstado { get; set; }
        public IEnumerable<SelectListItem> listTipoSanguineo { get; set; }
        public int IdTipoSanguineo { get; set; }



    }
}
