using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PWHS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PWHS.wwwroot.criptografia;
using Microsoft.AspNetCore.Mvc.Rendering;
using PWHS.Database;
using System.Data;

namespace PWHS.Controllers
{
    public class RegistraFraturaController : Controller
    {
        public static CadastroFratura model = new CadastroFratura();

        private static Usuario usuario = new Usuario();
        private BDpwhs _banco;



        public IActionResult RegistraFratura(Usuario usu)
        {
            usuario = usu;
            model = new CadastroFratura();
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            if (usuario.codigoEdicao == null)
            {
                MontarTelaCadastro();
            }
            else
            {
                MontarTelaEdicao();
            }

            return View("RegistraFratura", model);
        }


        [HttpPost]
        public IActionResult Salvar(string submitBtt)
        {
            if (submitBtt == "cadastra")
            {
                if (usuario.codigoEdicao == null)
                {
                    if (ValidaCadastroFratura())
                    {
                        DataSet DT = new DataSet();
                        _banco = new BDpwhs();
                        _banco.conectar();

                        DT = _banco.RegistrarFratura(model);

                        _banco.desconectar();

                        if (DT.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DT.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() == "0")
                                {
                                    ViewData["Erro"] = null;
                                    ViewData["Sucesso"] = "Registro de Fratura realizado com sucesso!";
                                    model = new CadastroFratura();
                                    MontarTelaCadastro();
                                }
                                else
                                {

                                    ViewData["Erro"] = "Erro ao Registrar Fratura!";
                                    ViewData["Sucesso"] = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ValidaEditarFratura())
                    {
                        DataSet DT = new DataSet();
                        _banco = new BDpwhs();
                        _banco.conectar();

                        DT = _banco.EditarFratura(model);

                        _banco.desconectar();

                        if (DT.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DT.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() == "0")
                                {
                                    ViewData["Erro"] = null;
                                    ViewData["Sucesso"] = "Edição de Fratura realizada com sucesso!";
                                    model = new CadastroFratura();
                                    MontarTelaCadastro();
                                    usuario.codigoEdicao = null;
                                }
                                else
                                {

                                    ViewData["Erro"] = "Erro ao Editar Fratura!";
                                    ViewData["Sucesso"] = null;
                                }
                            }
                        }
                    }
                }
                    return View("RegistraFratura", model);

            }
            else
            {
                return RedirectToAction("ConsultaHistorico", "ConsultaHistorico", usuario);
            }


        }


        public bool ValidaCadastroFratura()
        {
            string msg = "";

            model.nomeFratura = HttpContext.Request.Form["nomeFratura"];
            model.descricao = HttpContext.Request.Form["descricao"];
            model.IdUsuario = usuario.cpf;
            model.IdPessoa = usuario.pessoaCPF;
            model.IdStatus = int.Parse(HttpContext.Request.Form["IdStatus"].ToString().Trim());
            model.IdCausaFratura = int.Parse(HttpContext.Request.Form["IdCausaFratura"].ToString().Trim());
            model.IdTipoLesao = int.Parse(HttpContext.Request.Form["IdTipoLesao"].ToString().Trim());



            if (model.nomeFratura.Length < 10)
            {
                msg = "O campo Fratura deve conter ao menos 10 caracteres!";
            }
            else if (model.descricao.Length < 10)
            {
                msg = "O campo Descrição deve conter ao menos 10 caracteres!";
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

        public bool ValidaEditarFratura()
        {
            string msg = "";

            model.nomeFratura = HttpContext.Request.Form["nomeFratura"];
            model.descricao = HttpContext.Request.Form["descricao"];
            model.IdUsuario = usuario.cpf;
            model.IdPessoa = usuario.pessoaCPF;
            model.IdStatus = int.Parse(HttpContext.Request.Form["IdStatus"].ToString().Trim());



            if (model.nomeFratura.Length < 10)
            {
                msg = "O campo Fratura deve conter ao menos 10 caracteres!";
            }
            else if (model.descricao.Length < 10)
            {
                msg = "O campo Descrição deve conter ao menos 10 caracteres!";
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
        public void MontarTelaCadastro()
        {
            @ViewData["Edicao"] = null;
            model.Status = new List<SelectListItem>();
            DataSet DTStatus = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DTStatus = _banco.ConsultarStatus();

            _banco.desconectar();

            model.Pessoas = new List<SelectListItem>();
            model.Pessoas.Add(new SelectListItem() { Text = usuario.pessoaCPF + " - " + usuario.pessoaNome, Value = usuario.pessoaCPF });

            if (DTStatus.Tables.Count > 0)
            {

                foreach (DataRow linha in DTStatus.Tables[0].Rows)
                {
                    model.Status.Add(new SelectListItem() { Text = linha.ItemArray[1].ToString(), Value = linha.ItemArray[0].ToString() });
                }
            }

            model.Usuarios = new List<SelectListItem>();
            model.Usuarios.Add(new SelectListItem() { Text = usuario.nome, Value = usuario.cpf });

            model.CausaFratura = new List<SelectListItem>();
            model.CausaFratura.Add(new SelectListItem() { Text = "Fratura traumática", Value = "1" });
            model.CausaFratura.Add(new SelectListItem() { Text = "Fratura patológica", Value = "2" });

            model.TipoLesao = new List<SelectListItem>();
            model.TipoLesao.Add(new SelectListItem() { Text = "Fratura simples", Value = "1" });
            model.TipoLesao.Add(new SelectListItem() { Text = "Fratura exposta", Value = "2" });
            model.TipoLesao.Add(new SelectListItem() { Text = "Fratura complicada", Value = "3" });


            model.IdTipoLesao = int.Parse(model.TipoLesao[0].Value);
            model.IdCausaFratura = int.Parse(model.CausaFratura[0].Value);
            model.IdStatus = int.Parse(model.Status[0].Value);

        }
        public void MontarTelaEdicao()
        {
            @ViewData["Edicao"] = true;
            model.Status = new List<SelectListItem>();
            DataSet DTStatus = new DataSet();
            DataSet DTEditar = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DTStatus = _banco.ConsultarStatus();
            DTEditar = _banco.ObterFratura(usuario.codigoEdicao);

            _banco.desconectar();

            model.Pessoas = new List<SelectListItem>();
            model.Pessoas.Add(new SelectListItem() { Text = usuario.pessoaCPF + " - " + usuario.pessoaNome, Value = usuario.pessoaCPF });

            if (DTStatus.Tables.Count > 0)
            {

                foreach (DataRow linha in DTStatus.Tables[0].Rows)
                {
                    model.Status.Add(new SelectListItem() { Text = linha.ItemArray[1].ToString(), Value = linha.ItemArray[0].ToString() });
                }
            }
            if (DTEditar.Tables.Count > 0)
            {

                foreach (DataRow linha in DTEditar.Tables[0].Rows)
                {
                    model.codigoFratura = linha.ItemArray[0].ToString();
                    model.IdTipoLesao = int.Parse(linha.ItemArray[1].ToString());
                    model.IdCausaFratura = int.Parse(linha.ItemArray[2].ToString());
                    model.nomeFratura = linha.ItemArray[3].ToString();
                    model.descricao = linha.ItemArray[4].ToString();
                    model.IdStatus = int.Parse(linha.ItemArray[7].ToString());
                }
            }

            model.Usuarios = new List<SelectListItem>();
            model.Usuarios.Add(new SelectListItem() { Text = usuario.nome, Value = usuario.cpf });

            model.CausaFratura = new List<SelectListItem>();
            model.CausaFratura.Add(new SelectListItem() { Text = "Fratura traumática", Value = "1" });
            model.CausaFratura.Add(new SelectListItem() { Text = "Fratura patológica", Value = "2" });

            model.TipoLesao = new List<SelectListItem>();
            model.TipoLesao.Add(new SelectListItem() { Text = "Fratura simples", Value = "1" });
            model.TipoLesao.Add(new SelectListItem() { Text = "Fratura exposta", Value = "2" });
            model.TipoLesao.Add(new SelectListItem() { Text = "Fratura complicada", Value = "3" });
        }

    }
}
