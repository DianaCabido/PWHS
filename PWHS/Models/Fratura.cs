using System;
using PWHS.Models;


namespace PWHS.Models
{
    public class Fratura
    {
        public string codigoFratura { get; set; }
        public string tipoLesao { get; set; }
        public string causaFratura { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string dataRegistro { get; set; }
        public string usuario { get; set; }
        public string pessoaFisica { get; set; }
        public string status { get; set; }
    }
}
