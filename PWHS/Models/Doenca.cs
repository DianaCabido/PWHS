using System;
using PWHS.Models;

namespace PWHS.Models
{
    public class Doenca
    {
        public string codigoDoenca { get; set; }
        public string tipoDoenca { get; set; }
        public string nome { get; set; }
        public string observacao { get; set; }
        public string dataRegistro { get; set; }
        public string usuario { get; set; }
        public string pessoaFisica { get; set; }
        public string status { get; set; }
    }
}
