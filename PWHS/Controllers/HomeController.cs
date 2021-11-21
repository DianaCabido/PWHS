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
    public class HomeController : Controller
    {
        private BDpwhs _banco;
        private static Login model = new Login();

        #region TELA LOGIN

        public IActionResult Index()
        {
            model.usuario = "True";
            model.usuarioBool = true;
            model.entidade = "False";
            model.cpf = "";
            model.senha = "";
            return View("Index", model);
        }



        public IActionResult ValidarLogin()
        {

            model.cpf = HttpContext.Request.Form["cpf"];
            model.senha = HttpContext.Request.Form["senha"];
            model.usuario = HttpContext.Request.Form["true"];


            if (model.usuario == "true")
            {
                model.usuarioBool = true;
                model.entidadeBool = false;
                if (ValidaDadosUsuario(model.cpf, model.senha))
                {
                    DataSet DT = new DataSet();

                    _banco = new BDpwhs();
                    _banco.conectar();

                    DT = _banco.ValidarUsuario(model.cpf);

                    _banco.desconectar();


                    if (DT.Tables.Count > 0)
                    {

                        foreach (DataRow linha in DT.Tables[0].Rows)
                        {
                            if (linha.ItemArray[0].ToString() != "0")
                            {
                                Md5 md5 = new Md5();
                                Boolean ComparaSenha = md5.ComparaMD5(model.senha, linha.ItemArray[2].ToString());

                                if (ComparaSenha == false)
                                {
                                    ViewData["Erro"] = "Senha incorreta!";
                                    return View("Index", model);
                                }
                                else
                                {
                                    Usuario usua = new Usuario();

                                    usua.cpf = model.cpf;
                                    usua.nome = linha.ItemArray[1].ToString();
                                    usua.nomeEntidade = linha.ItemArray[3].ToString();
                                    usua.codigoPerfil = linha.ItemArray[4].ToString();

                                    return RedirectToAction("TelaInicial", "TelaInicial", usua);
                                }
                            }
                            else
                            {
                                ViewData["Erro"] = "CPF não cadastrado!";
                                return View("Index", model);
                            }
                        }
                        return View("Index", model);
                    }
                    else
                    {
                        return View("Index", model);
                    }

                }
                else
                {
                    return View("Index", model);
                }
            }
            else
            {
                model.usuarioBool = false;
                model.entidadeBool = true;
                if (ValidaDadosEntidade(model.cpf, model.senha))
                {
                    DataSet DT = new DataSet();

                    _banco = new BDpwhs();
                    _banco.conectar();

                    DT = _banco.ValidarEntidade(model.cpf);

                    _banco.desconectar();


                    if (DT.Tables.Count > 0)
                    {

                        foreach (DataRow linha in DT.Tables[0].Rows)
                        {
                            if (linha.ItemArray[0].ToString() != "0")
                            {
                                Md5 md5 = new Md5();
                                Boolean ComparaSenha = md5.ComparaMD5(model.senha, linha.ItemArray[2].ToString());

                                if (ComparaSenha == false)
                                {
                                    ViewData["Erro"] = "Senha incorreta!";
                                    return View("Index", model);
                                }
                                else
                                {
                                    Entidade usua = new Entidade();

                                    usua.cnpj = model.cpf;
                                    usua.nome = linha.ItemArray[1].ToString();

                                    return RedirectToAction("TelaInicialEntidade", "TelaInicialEntidade", usua);
                                }
                            }
                            else
                            {
                                ViewData["Erro"] = "CNPJ não cadastrado!";
                                return View("Index", model);
                            }
                        }
                        return View("Index", model);
                    }
                    else
                    {
                        return View("Index", model);
                    }

                }
                else
                {
                    return View("Index", model);
                }
            }


        }
        public bool ValidaDadosUsuario(string cpf, string senha)
        {
            string msg = "";



            if (cpf == null || cpf == "")
            {
                msg = "O campo CPF é obrigatório!";

            }
            else if (cpf.Length < 11)
            {
                msg = "O campo CPF deve conter 11 dígitos!";

            }
            else if (ValidaCPF(cpf) == false)
            {
                msg = "CPF inválido!";

            }
            else if (senha == null || senha == "")
            {
                msg = "O campo Senha é obrigatório!";
            }


            if (msg == "")
            {
                ViewData["Erro"] = null;
                return true;
            }
            else
            {
                ViewData["Erro"] = msg;
                return false;
            }
        }

        public bool ValidaDadosEntidade(string cpf, string senha)
        {
            string msg = "";



            if (cpf == null || cpf == "")
            {
                msg = "O campo CNPJ é obrigatório!";

            }
            else if (cpf.Length < 11)
            {
                msg = "O campo CNPJ deve conter 11 dígitos!";

            }
            else if (ValidaCNPJ(cpf) == false)
            {
                msg = "CNPJ inválido!";

            }
            else if (senha == null || senha == "")
            {
                msg = "O campo Senha é obrigatório!";
            }


            if (msg == "")
            {
                ViewData["Erro"] = null;
                return true;
            }
            else
            {
                ViewData["Erro"] = msg;
                return false;
            }
        }
        #endregion
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

        public static bool ValidaCNPJ(string cnpj)

        {

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);

        }
    }
}
