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
    public class TelaInicialEntidadeController : Controller
    {
        private static Entidade usuario = new Entidade();
        private static TelaInicialEntidade model = new TelaInicialEntidade();
        private BDpwhs _banco;

        #region TELA LOGIN


        public IActionResult TelaInicialEntidade(Entidade usu)
        {

            DataSet DT = new DataSet();
            _banco = new BDpwhs();
            usuario = usu;

            model.grid = new List<GridConsultaUsuario>();
            ViewData["MsgLayout"] = usuario.nome;
            return View("TelaInicialEntidade", model);
        }

        #endregion


        #region BOTÕES TELA LOGIN

        public IActionResult FiltrarPessoa(string submitBtt)
        {
            if (submitBtt == "pesquisa")
            {
                ViewData["MsgLayout"] = usuario.nome;
                string cpf = HttpContext.Request.Form["cpf"].ToString().Trim();
                string nome = HttpContext.Request.Form["nome"].ToString().Trim();

                if (cpf.Length > 0 && cpf.Length < 11)
                {
                    ViewData["Erro"] = "CPF deve conter 11 dígitos!";
                    return View("TelaInicialEntidade", model);
                }
                else if (cpf.Length > 0 && (ValidaCPF(cpf) == false))
                {
                    ViewData["Erro"] = "CPF inválido!";
                    return View("TelaInicialEntidade", model);
                }
                else
                {

                    model.grid = new List<GridConsultaUsuario>();
                    DataSet DT = new DataSet();
                    _banco = new BDpwhs();
                    _banco.conectar();

                    DT = _banco.FiltrarUsuarios(cpf, nome,usuario.cnpj);


                    _banco.desconectar();

                    if (DT.Tables.Count > 0)
                    {

                        foreach (DataRow index in DT.Tables[0].Rows)
                        {
                            GridConsultaUsuario linha = new GridConsultaUsuario();
                            linha.cpf = index.ItemArray[0].ToString();
                            linha.nome = index.ItemArray[1].ToString();
                            linha.email = index.ItemArray[4].ToString();


                            switch (index.ItemArray[7].ToString())
                            {
                                case "1":
                                    linha.perfil = "OPERADOR";
                                    break;
                                case "2":
                                    linha.perfil = "ENFERMEIRO";
                                    break;
                                case "3":
                                    linha.perfil = "MÉDICO";
                                    break;
                              
                            }
                            model.grid.Add(linha);
                        }
                    }

                    return View("TelaInicialEntidade", model);
                }

            }      
            else
            {
                usuario.codigoEdicao = null;
                return RedirectToAction("CadastroUsuario", "CadastroUsuario", usuario);
            }


        }
        public IActionResult AcaoBotao(string submitEdicao)
        {
            usuario.codigoEdicao = null;

                usuario.codigoEdicao = submitEdicao;
                return RedirectToAction("CadastroUsuario", "CadastroUsuario", usuario);

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
            Usuario us = new Usuario();
            us.cpf = usuario.cnpj;
            return RedirectToAction("AlterarSenha", "AlterarSenha", us);
        }


        #endregion
    }
}
