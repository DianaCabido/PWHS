using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PWHS.Models
{
    public class CadastroUsuario
    {
        public string cpf { get; set; }
        public string cnpj { get; set; }
        public string nomeUs { get; set; }
        public int perfil { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string cep { get; set; }
        public string logadouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public IEnumerable<SelectListItem> listEstado { get; set; }
        public int IdEstado { get; set; }
        public string telefone { get; set; }
        public string ddd { get; set; }
        public IEnumerable<SelectListItem> listPerfil { get; set; }
       



    }
}
