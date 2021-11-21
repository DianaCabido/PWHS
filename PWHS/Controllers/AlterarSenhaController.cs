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
    public class AlterarSenhaController : Controller
    {
        private BDpwhs _banco;
        private static AlterarSenha model = new AlterarSenha();
        private static Usuario usuario = new Usuario();

        #region TELA LOGIN

        public IActionResult AlterarSenha(Usuario usu)
        {
            usuario = usu;
            return View("AlterarSenha", model);
        }


        [HttpPost]
        public IActionResult Salvar(string submitBtt)
        {
            if (submitBtt == "cadastra")
            {

                if (ValidaDados())
                {
                    DataSet DT = new DataSet();
                    DataSet DT2 = new DataSet();

                    _banco = new BDpwhs();


                    if (usuario.cpf.Length == 11)
                    {
                        _banco.conectar();


                        DT = _banco.ValidarUsuario(usuario.cpf);

                        _banco.desconectar();


                        if (DT.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DT.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() != "0")
                                {
                                    Md5 md5 = new Md5();
                                    Boolean ComparaSenha = md5.ComparaMD5(model.senhaAtual, linha.ItemArray[2].ToString());

                                    if (ComparaSenha == false)
                                    {
                                        ViewData["Erro"] = "Senha Atual incorreta!";
                                        ViewData["Sucesso"] = null;
                                    }
                                    else
                                    {
                                        string senhaCod = md5.RetornarMD5(model.senhaNova);
                                        _banco = new BDpwhs();
                                        _banco.conectar();

                                        DT2 = _banco.EditarUsuario(usuario.cpf, senhaCod);

                                        _banco.desconectar();
                                        if (DT2.Tables.Count > 0)
                                        {

                                            foreach (DataRow l in DT2.Tables[0].Rows)
                                            {
                                                if (l.ItemArray[0].ToString() == "0")
                                                {
                                                    ViewData["Erro"] = null;
                                                    ViewData["Sucesso"] = "Senha alterada com sucesso!";
                                                    model = new AlterarSenha();
                                                }
                                                else
                                                {

                                                    ViewData["Erro"] = "Erro ao alterar Senha!";
                                                    ViewData["Sucesso"] = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }


                        }
                    }
                    else
                    {

                        _banco.conectar();


                        DT = _banco.ValidarEntidade(usuario.cpf);

                        _banco.desconectar();


                        if (DT.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DT.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() != "0")
                                {
                                    Md5 md5 = new Md5();
                                    Boolean ComparaSenha = md5.ComparaMD5(model.senhaAtual, linha.ItemArray[2].ToString());

                                    if (ComparaSenha == false)
                                    {
                                        ViewData["Erro"] = "Senha Atual incorreta!";
                                        ViewData["Sucesso"] = null;
                                    }
                                    else
                                    {
                                        string senhaCod = md5.RetornarMD5(model.senhaNova);
                                        _banco = new BDpwhs();
                                        _banco.conectar();

                                        DT2 = _banco.EditarEntidade(usuario.cpf, senhaCod);

                                        _banco.desconectar();
                                        if (DT2.Tables.Count > 0)
                                        {

                                            foreach (DataRow l in DT2.Tables[0].Rows)
                                            {
                                                if (l.ItemArray[0].ToString() == "0")
                                                {
                                                    ViewData["Erro"] = null;
                                                    ViewData["Sucesso"] = "Senha alterada com sucesso!";
                                                    model = new AlterarSenha();
                                                }
                                                else
                                                {

                                                    ViewData["Erro"] = "Erro ao alterar Senha!";
                                                    ViewData["Sucesso"] = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }


                        }
                    }
                }
                return View("AlterarSenha", model);
            }
            else

            {

                if (usuario.cpf.Length == 11)
                {
                    return RedirectToAction("TelaInicial", "TelaInicial", usuario);
                }
                else
                {
                    return RedirectToAction("TelaInicialEntidade", "TelaInicialEntidade", usuario);
                }
            }

        }


        public bool ValidaDados()
        {
            string msg = "";

            model.senhaAtual = HttpContext.Request.Form["senhaAtual"].ToString().Trim();
            model.senhaNova = HttpContext.Request.Form["senhaNova"].ToString().Trim();
            model.confirmacao = HttpContext.Request.Form["confirmacao"].ToString().Trim();

            if (model.senhaAtual == null || model.senhaAtual == "")
            {
                msg = "O preenchimento do campo Senha é obrigatório!";

            }
            else if (model.senhaNova == null || model.senhaNova == "")
            {
                msg = "O preenchimento do campo Nova Senha é obrigatório!";

            }
            else if (model.confirmacao == null || model.confirmacao == "")
            {
                msg = "O preenchimento do campo Confirme a Senha é obrigatório!";

            }
            else if (model.senhaNova != model.confirmacao)
            {
                msg = "As senhas não conferem!";

            }
            else if (model.senhaNova == model.senhaAtual)
            {
                msg = "A Nova Senha não pode ser igual a última!";

            }



            if (msg == "")
            {
                ViewData["Erro"] = null;
                ViewData["Sucesso"] = null;
                return true;
            }
            else
            {
                ViewData["Erro"] = msg;
                ViewData["Sucesso"] = null;
                return false;
            }
        }


        #endregion
  
    }
}
