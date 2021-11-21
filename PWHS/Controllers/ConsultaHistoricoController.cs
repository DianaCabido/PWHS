using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PWHS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PWHS.wwwroot.criptografia;
using System.Data;
using PWHS.Database;

namespace PWHS.Controllers
{
    public class ConsultaHistoricoController : Controller
    {
        private static Usuario usuario = new Usuario();
        private BDpwhs _banco;
        private static Historico historico= new Historico();

        public IActionResult ConsultaHistorico(Usuario usu)
        {
            usuario = usu;

            historico.pessoa = new PessoaFisica();
            historico.listVacinacoes = new List<Vacinacao>();
            historico.listFraturas = new List<Fratura>();
            historico.listDoencas = new List<Doenca>();
            historico.listAlergias = new List<Alergia>();

            ObterHistorico(usuario.pessoaCPF);

            usuario.pessoaNome = historico.pessoa.nomePF;
            usuario.pessoaCPF = historico.pessoa.cpfPF;
            ViewData["codigoPerfil"] = usuario.codigoPerfil;
            historico.codigoPerfil = usuario.codigoPerfil;
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            return View("ConsultaHistorico", historico);
        }
        public IActionResult AcaoBotaoDoenca(string submitEdicao, string submitHistorico)
        {
            usuario.codigoEdicao = null;

            if (submitEdicao != null)
            {
                usuario.codigoEdicao = submitEdicao;

                return RedirectToAction("RegistraDoenca", "RegistraDoenca", usuario);
            }
            else
            {
                usuario.codigoEdicao = submitHistorico;
                return RedirectToAction("HistoricoDoenca", "HistoricoDoenca", usuario);
            }


        }
        public IActionResult AcaoBotaoAlergia(string submitEdicao, string submitHistorico)
        {
            usuario.codigoEdicao = null;

            if (submitEdicao != null)
            {
                usuario.codigoEdicao = submitEdicao;

                return RedirectToAction("RegistraAlergia", "RegistraAlergia", usuario);
            }
            else
            {
                usuario.codigoEdicao = submitHistorico;
                return RedirectToAction("HistoricoAlergia", "HistoricoAlergia", usuario);
            }


        }

        public IActionResult AcaoBotaoFratura(string submitEdicao, string submitHistorico)
        {
            usuario.codigoEdicao = null;

            if (submitEdicao != null)
            {
                usuario.codigoEdicao = submitEdicao;

                return RedirectToAction("RegistraFratura", "RegistraFratura", usuario);
            }
            else
            {
                usuario.codigoEdicao = submitHistorico;
                return RedirectToAction("HistoricoFratura", "HistoricoFratura", usuario);
            }
        }

        public void ObterHistorico(string cpf)
        {
            DataSet DT = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DT = _banco.ObterHistorico(cpf);

            _banco.desconectar();

            if (DT.Tables.Count > 5)
            {
                MontarObjPessoa(DT.Tables[0], DT.Tables[1]);
                MontarListaAlergia(DT.Tables[2]);
                MontarListaDoenca(DT.Tables[3]);
                MontarListaFratura(DT.Tables[4]);
                MontarListaVacinacao(DT.Tables[5]);
            }

        }
        public void MontarObjPessoa(DataTable tbPessoa, DataTable tbEnd)
        {

            foreach (DataRow linha in tbPessoa.Rows)
            {
                historico.pessoa.cpfPF = linha.ItemArray[0].ToString();
                historico.pessoa.nomePF = linha.ItemArray[1].ToString();
                historico.pessoa.dataNascimento = DateTime.Parse(linha.ItemArray[2].ToString()).ToString("dd/MM/yyyy");
                historico.pessoa.email = linha.ItemArray[5].ToString();
                switch (linha.ItemArray[3].ToString())
                {
                    case "1":
                        historico.pessoa.sexo = "M";
                        break;
                    case "2":
                        historico.pessoa.sexo = "F";
                        break;
                }
                switch (linha.ItemArray[4].ToString())
                {
                    case "1":
                        historico.pessoa.tipoSanguineo = "-A";
                        break;
                    case "2":
                        historico.pessoa.tipoSanguineo = "+A";
                        break;
                    case "3":
                        historico.pessoa.tipoSanguineo = "-B";
                        break;
                    case "4":
                        historico.pessoa.tipoSanguineo = "+B";
                        break;
                    case "5":
                        historico.pessoa.tipoSanguineo = "-AB";
                        break;
                    case "6":
                        historico.pessoa.tipoSanguineo = "+AB";
                        break;
                    case "7":
                        historico.pessoa.tipoSanguineo = "-O";
                        break;
                    case "8":
                        historico.pessoa.tipoSanguineo = "+O";
                        break;
                }

            }
            foreach (DataRow linha in tbEnd.Rows)
            {
                historico.pessoa.cep = linha.ItemArray[1].ToString();
                historico.pessoa.logadouro = linha.ItemArray[2].ToString();
                historico.pessoa.numero = linha.ItemArray[3].ToString();
                historico.pessoa.complemento = linha.ItemArray[4].ToString().ToUpper();
                historico.pessoa.bairro = linha.ItemArray[5].ToString().ToUpper();
                historico.pessoa.cidade = linha.ItemArray[6].ToString().ToUpper();
                historico.pessoa.estado = linha.ItemArray[7].ToString().ToUpper();
                historico.pessoa.telefone = linha.ItemArray[9].ToString().ToUpper();


            }
        }
        public void MontarListaAlergia(DataTable tbAlergia)
        {
            List<Alergia> listAlergias = new List<Alergia>();

            foreach (DataRow linha in tbAlergia.Rows)
            {
                Alergia al = new Alergia();

                al.codigo = linha.ItemArray[0].ToString();

                switch (linha.ItemArray[1].ToString())
                {
                    case "1":
                        al.tipoAlergia = "Alimentar";
                        break;
                    case "2":
                        al.tipoAlergia = "Pele";
                        break;
                    case "3":
                        al.tipoAlergia = "Nasal";
                        break;
                    case "4":
                        al.tipoAlergia = "Respiratória";
                        break;
                    case "5":
                        al.tipoAlergia = "Ocular";
                        break;
                    case "6":
                        al.tipoAlergia = "Outro";
                        break;



                }

                al.nome = linha.ItemArray[2].ToString();
                al.descricao = linha.ItemArray[3].ToString();
                al.dataRegistro = DateTime.Parse(linha.ItemArray[4].ToString()).ToString("dd/MM/yyyy");

                switch (linha.ItemArray[6].ToString())
                {
                    case "1":
                        al.status = "Em tratamento";
                        break;
                    case "2":
                        al.status = "Tratado";
                        break;
                    case "3":
                        al.status = "Diagnósticado";
                        break;
                    case "4":
                        al.status = "Reincidente";
                        break;

                }

                listAlergias.Add(al);
            }

            historico.listAlergias = listAlergias;
        }
        public void MontarListaDoenca(DataTable tbDoenca)
        {
            List<Doenca> listDoencas = new List<Doenca>();

            foreach (DataRow linha in tbDoenca.Rows)
            {
                Doenca al = new Doenca();

                al.codigoDoenca = linha.ItemArray[0].ToString();

                switch (linha.ItemArray[1].ToString())
                {
                    case "1":
                        al.tipoDoenca = "Diabetes";
                        break;
                    case "2":
                        al.tipoDoenca = "Alzheimer";
                        break;
                    case "3":
                        al.tipoDoenca = "Hipertensão";
                        break;
                    case "4":
                        al.tipoDoenca = "Asma";
                        break;
                    case "5":
                        al.tipoDoenca = "AIDS";
                        break;
                    case "6":
                        al.tipoDoenca = "Câncer";
                        break;
                    case "7":
                        al.tipoDoenca = "Depressão";
                        break;
                    case "8":
                        al.tipoDoenca = "AVC";
                        break;
                    case "9":
                        al.tipoDoenca = "Mal de Parkinson";
                        break;
                    case "10":
                        al.tipoDoenca = "DPOC (Doença pulmonar obstrutiva crônica)";
                        break;


                }

                al.nome = linha.ItemArray[2].ToString();
                al.observacao = linha.ItemArray[3].ToString();
                al.dataRegistro = DateTime.Parse(linha.ItemArray[4].ToString()).ToString("dd/MM/yyyy");

                switch (linha.ItemArray[6].ToString())
                {
                    case "1":
                        al.status = "Em tratamento";
                        break;
                    case "2":
                        al.status = "Tratado";
                        break;
                    case "3":
                        al.status = "Diagnósticado";
                        break;
                    case "4":
                        al.status = "Reincidente";
                        break;

                }

                listDoencas.Add(al);
            }

            historico.listDoencas = listDoencas;
        }
        public void MontarListaFratura(DataTable tbFratura)
        {
            List<Fratura> listFraturas = new List<Fratura>();

            foreach (DataRow linha in tbFratura.Rows)
            {
                Fratura al = new Fratura();

                al.codigoFratura = linha.ItemArray[0].ToString();

                switch (linha.ItemArray[1].ToString())
                {
                    case "1":
                        al.tipoLesao = "Fratura simples";
                        break;
                    case "2":
                        al.tipoLesao = "Fratura exposta";
                        break;
                    case "3":
                        al.tipoLesao = "Fratura complicada";
                        break;

                }
                switch (linha.ItemArray[2].ToString())
                {
                    case "1":
                        al.causaFratura = "Fratura traumática";
                        break;
                    case "2":
                        al.causaFratura = "Fratura patológica";
                        break;

                }

                al.nome = linha.ItemArray[3].ToString();
                al.descricao = linha.ItemArray[4].ToString();
                al.dataRegistro = DateTime.Parse(linha.ItemArray[5].ToString()).ToString("dd/MM/yyyy");

                switch (linha.ItemArray[7].ToString())
                {
                    case "1":
                        al.status = "Em tratamento";
                        break;
                    case "2":
                        al.status = "Tratado";
                        break;
                    case "3":
                        al.status = "Diagnósticado";
                        break;
                    case "4":
                        al.status = "Reincidente";
                        break;

                }

                listFraturas.Add(al);
            }

            historico.listFraturas = listFraturas;
        }
        public void MontarListaVacinacao(DataTable tbVacinacoes)
        {
            List<Vacinacao> listVacinacoes = new List<Vacinacao>();

            foreach (DataRow linha in tbVacinacoes.Rows)
            {
                Vacinacao al = new Vacinacao();

                al.codigoVacinacao = linha.ItemArray[0].ToString();

                al.vacina = linha.ItemArray[1].ToString();

                al.laboratorio = linha.ItemArray[2].ToString();
                al.lote = linha.ItemArray[3].ToString();
                al.validade = DateTime.Parse(linha.ItemArray[4].ToString()).ToString("dd/MM/yyyy");
                al.dataRegistro = DateTime.Parse(linha.ItemArray[5].ToString()).ToString("dd/MM/yyyy");
                al.usuario = linha.ItemArray[6].ToString();

                listVacinacoes.Add(al);
            }

            historico.listVacinacoes = listVacinacoes;
        }

        public IActionResult RegistraDoenca()
        {
            usuario.codigoEdicao = null;
            return RedirectToAction("RegistraDoenca", "RegistraDoenca", usuario);
        }
        public IActionResult RegistraFratura()
        {
            usuario.codigoEdicao = null;
            return RedirectToAction("RegistraFratura", "RegistraFratura", usuario);
        }
        public IActionResult RegistraAlergia()
        {
            usuario.codigoEdicao = null;
            return RedirectToAction("RegistraAlergia", "RegistraAlergia", usuario);
        }
        public IActionResult RegistraVacinacao()
        {
            usuario.codigoEdicao = null;
            return RedirectToAction("RegistraVacinacao", "RegistraVacinacao", usuario);
        }
        public IActionResult ChamarTelaInicial()
        {
            usuario.pessoaCPF = null;
            usuario.pessoaNome = null;
            return RedirectToAction("TelaInicial", "TelaInicial", usuario);
        }
    }
}