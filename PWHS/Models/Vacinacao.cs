using System;
using PWHS.Models;

namespace PWHS.Models
{
    public class Vacinacao
    {
        public string codigoVacinacao { get; set; }
        public string vacina { get; set; }
        public string laboratorio { get; set; }
        public string lote { get; set; }
        public string validade { get; set; }
        public string dataRegistro { get; set; }
        public string usuario { get; set; }
        public string pessoaFisica { get; set; }
    }
}
