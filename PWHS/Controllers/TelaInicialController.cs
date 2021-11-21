using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PWHS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PWHS.wwwroot.criptografia;
using PWHS.Database;
using System.Data;


namespace PWHS.Controllers
{
    public class TelaInicialController : Controller
    {
        private static Usuario usuario = new Usuario();
        private static TelaInicial model = new TelaInicial();
        private BDpwhs _banco;

        #region TELA LOGIN


        public IActionResult TelaInicial(Usuario usu)
        {

            DataSet DT = new DataSet();
            _banco = new BDpwhs();
            usuario = usu;
            usuario.pessoaCPF = usu.cpf; 
            model.codigoPerfilUsuario = usu.codigoPerfil;
            model.grid = new List<GridConsulta>();
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            return View("TelaInicial", model);
        }

        #endregion


        #region BOTÕES TELA LOGIN

        public IActionResult FiltrarPessoa(string submitBtt)
        {
            if (submitBtt == "pesquisa")
            {
                ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
                string cpf = HttpContext.Request.Form["cpf"].ToString().Trim();
                string nome = HttpContext.Request.Form["nome"].ToString().Trim();

                if (cpf.Length > 0 && cpf.Length < 11)
                {
                    ViewData["Erro"] = "CPF deve conter 11 dígitos!";
                    return View("TelaInicial", model);
                }
                else if (cpf.Length > 0 && (ValidaCPF(cpf) == false))
                {
                    ViewData["Erro"] = "CPF inválido!";
                    return View("TelaInicial", model);
                }
                else
                {

                    model.grid = new List<GridConsulta>();
                    DataSet DT = new DataSet();
                    _banco = new BDpwhs();
                    _banco.conectar();

                    DT = _banco.FiltrarPessoas(cpf, nome);


                    _banco.desconectar();

                    if (DT.Tables.Count > 0)
                    {

                        foreach (DataRow index in DT.Tables[0].Rows)
                        {
                            GridConsulta linha = new GridConsulta();
                            linha.cpf = index.ItemArray[0].ToString();
                            linha.nome = index.ItemArray[1].ToString();
                            linha.dataNascimento = DateTime.Parse(index.ItemArray[2].ToString()).ToString("dd/MM/yyyy");


                            switch (index.ItemArray[4].ToString())
                            {
                                case "1":
                                    linha.tipoSanguineo = "-A";
                                    break;
                                case "2":
                                    linha.tipoSanguineo = "+A";
                                    break;
                                case "3":
                                    linha.tipoSanguineo = "-B";
                                    break;
                                case "4":
                                    linha.tipoSanguineo = "+B";
                                    break;
                                case "5":
                                    linha.tipoSanguineo = "-AB";
                                    break;
                                case "6":
                                    linha.tipoSanguineo = "+AB";
                                    break;
                                case "7":
                                    linha.tipoSanguineo = "-O";
                                    break;
                                case "8":
                                    linha.tipoSanguineo = "+O";
                                    break;
                            }
                            model.grid.Add(linha);
                        }
                    }

                    return View("TelaInicial", model);
                }

            }      
            else
            {
                usuario.codigoEdicao = null;
                return RedirectToAction("CadastroPessoa", "CadastroPessoa", usuario);
            }


        }
        public IActionResult AcaoBotao(string submitEdicao, string submitHistorico)
        {
            usuario.codigoEdicao = null;

            if (submitHistorico != null)
            {
                usuario.pessoaCPF = submitHistorico.Trim();
                usuario.pessoaNome = model.grid.Find(x => x.cpf.Trim() == submitHistorico.Trim()).nome.Trim();
                return RedirectToAction("ConsultaHistorico", "ConsultaHistorico", usuario);
            }
            else
            {
                usuario.codigoEdicao = submitEdicao;
                return RedirectToAction("CadastroPessoa", "CadastroPessoa", usuario);
            }
        }


        public static bool ValidaCPF(string vrCPF)

        {

            string valor = vrCPF.Replace(".", "");

            valor = valor.Replace("-", "");



            if (valor.Length != 11)

                return false;



            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)

                if (valor[i] != valor[0])

                    igual = false;



            if (igual || valor == "12345678909")

                return false;



            int[] numeros = new int[11];



            for (int i = 0; i < 11; i++)

                numeros[i] = int.Parse(

                  valor[i].ToString());



            int soma = 0;

            for (int i = 0; i < 9; i++)

                soma += (10 - i) * numeros[i];



            int resultado = soma % 11;



            if (resultado == 1 || resultado == 0)

            {

                if (numeros[9] != 0)

                    return false;

            }

            else if (numeros[9] != 11 - resultado)

                return false;



            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += (11 - i) * numeros[i];



            resultado = soma % 11;



            if (resultado == 1 || resultado == 0)

            {

                if (numeros[10] != 0)

                    return false;

            }

            else

                if (numeros[10] != 11 - resultado)

                return false;



            return true;

        }

        public IActionResult Sair()
        {
            
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AlterarSenha()
        {
            usuario.codigoEdicao = null;
            return RedirectToAction("AlterarSenha", "AlterarSenha", usuario);
        }

        #endregion
    }
}
