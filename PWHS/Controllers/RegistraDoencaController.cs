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
    public class RegistraDoencaController : Controller
    {
        public static CadastroDoenca model = new CadastroDoenca();

        private static Usuario usuario = new Usuario();

        private BDpwhs _banco;



        public IActionResult RegistraDoenca(Usuario usu)
        {
            usuario = usu;
            model = new CadastroDoenca();
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            if (usuario.codigoEdicao == null)
            {
                MontarTelaCadastro();
            }
            else
            {
                MontarTelaEdicao();
            }


            return View(model);
        }

        [HttpPost]
        public IActionResult Salvar(string submitBtt)
        {
            if (submitBtt == "cadastra")
            {
                if (usuario.codigoEdicao == null)
                {
                    if (ValidaCadastroDoenca())
                    {
                        DataSet DT = new DataSet();
                        _banco = new BDpwhs();
                        _banco.conectar();

                        DT = _banco.RegistrarDoenca(model);

                        _banco.desconectar();

                        if (DT.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DT.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() == "0")
                                {
                                    ViewData["Erro"] = null;
                                    ViewData["Sucesso"] = "Registro de Doença realizado com sucesso!";
                                    model = new CadastroDoenca();
                                    MontarTelaCadastro();

                                }
                                else
                                {

                                    ViewData["Erro"] = "Erro ao Registrar Doença!";
                                    ViewData["Sucesso"] = null;
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (ValidaEdicaoDoenca())
                    {
                        DataSet DT = new DataSet();
                        _banco = new BDpwhs();
                        _banco.conectar();

                        DT = _banco.EditarDoenca(model);

                        _banco.desconectar();

                        if (DT.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DT.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() == "0")
                                {
                                    ViewData["Erro"] = null;
                                    ViewData["Sucesso"] = "Edição de Doença realizada com sucesso!";

                                    model = new CadastroDoenca();
                                    MontarTelaCadastro();

                                    usuario.codigoEdicao = null;
                                }
                                else
                                {

                                    ViewData["Erro"] = "Erro ao Editar Doença!";
                                    ViewData["Sucesso"] = null;
                                }
                            }
                        }
                    }
                }

                return View("RegistraDoenca", model);
            }
            else
            {
                return RedirectToAction("ConsultaHistorico", "ConsultaHistorico", usuario);
            }
        }


        public bool ValidaCadastroDoenca()
        {
            string msg = "";

            model.observacoes = HttpContext.Request.Form["observacoes"];
            model.descricao = HttpContext.Request.Form["descricao"];
            model.IdDoenca = int.Parse(HttpContext.Request.Form["IdDoenca"].ToString().Trim());
            model.IdStatus = int.Parse(HttpContext.Request.Form["IdStatus"].ToString().Trim());
            model.IdPessoa = usuario.pessoaCPF;
            model.IdUsuario = usuario.cpf;



            if (model.descricao.Length < 10)
            {
                msg = "O campo Descrição deve conter ao menos 10 caracteres!";
            }
            else if (model.observacoes.Length < 10)
            {
                msg = "O campo Observações deve conter ao menos 10 caracteres!";
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
        public bool ValidaEdicaoDoenca()
        {
            string msg = "";

            model.observacoes = HttpContext.Request.Form["observacoes"];
            model.descricao = HttpContext.Request.Form["descricao"];
            model.IdStatus = int.Parse(HttpContext.Request.Form["IdStatus"].ToString().Trim());
            model.IdPessoa = usuario.pessoaCPF;
            model.IdUsuario = usuario.cpf;



            if (model.descricao.Length < 10)
            {
                msg = "O campo Descrição deve conter ao menos 10 caracteres!";
            }
            else if (model.observacoes.Length < 10)
            {
                msg = "O campo Observações deve conter ao menos 10 caracteres!";
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
            model.Pessoas = new List<SelectListItem>();
            model.Status = new List<SelectListItem>();
            model.Doenca = new List<SelectListItem>();
            DataSet DTPessoas = new DataSet();
            DataSet DTStatus = new DataSet();
            DataSet DTDoencas = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DTStatus = _banco.ConsultarStatus();
            DTDoencas = _banco.ConsultarTipoDoencas();

            _banco.desconectar();


            model.Pessoas.Add(new SelectListItem() { Text = usuario.pessoaCPF + " - " + usuario.pessoaNome, Value = usuario.pessoaCPF });


            if (DTStatus.Tables.Count > 0)
            {

                foreach (DataRow linha in DTStatus.Tables[0].Rows)
                {
                    model.Status.Add(new SelectListItem() { Text = linha.ItemArray[1].ToString(), Value = linha.ItemArray[0].ToString() });
                }
            }
            if (DTDoencas.Tables.Count > 0)
            {

                foreach (DataRow linha in DTDoencas.Tables[0].Rows)
                {
                    model.Doenca.Add(new SelectListItem() { Text = linha.ItemArray[1].ToString(), Value = linha.ItemArray[0].ToString() });
                }
            }

            model.IdDoenca = int.Parse(model.Doenca[0].Value);
            model.IdStatus = int.Parse(model.Status[0].Value);

            model.Usuarios = new List<SelectListItem>();
            model.Usuarios.Add(new SelectListItem() { Text = usuario.nome, Value = usuario.cpf });
        }
        public void MontarTelaEdicao()
        {
            @ViewData["Edicao"] = true;
            model.Pessoas = new List<SelectListItem>();
            model.Status = new List<SelectListItem>();
            model.Doenca = new List<SelectListItem>();
            DataSet DTPessoas = new DataSet();
            DataSet DTStatus = new DataSet();
            DataSet DTDoencas = new DataSet();
            DataSet DTEditar = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DTStatus = _banco.ConsultarStatus();
            DTEditar = _banco.ObterDoenca(usuario.codigoEdicao);
            DTDoencas = _banco.ConsultarTipoDoencas();

            _banco.desconectar();


            model.Pessoas.Add(new SelectListItem() { Text = usuario.pessoaCPF + " - " + usuario.pessoaNome, Value = usuario.pessoaCPF });


            if (DTStatus.Tables.Count > 0)
            {

                foreach (DataRow linha in DTStatus.Tables[0].Rows)
                {
                    model.Status.Add(new SelectListItem() { Text = linha.ItemArray[1].ToString(), Value = linha.ItemArray[0].ToString() });
                }
            }
            if (DTDoencas.Tables.Count > 0)
            {

                foreach (DataRow linha in DTDoencas.Tables[0].Rows)
                {
                    model.Doenca.Add(new SelectListItem() { Text = linha.ItemArray[1].ToString(), Value = linha.ItemArray[0].ToString() });
                }
            }
            if (DTEditar.Tables.Count > 0)
            {

                foreach (DataRow linha in DTEditar.Tables[0].Rows)
                {
                    model.codigoDoenca= linha.ItemArray[0].ToString();
                    model.IdDoenca= int.Parse(linha.ItemArray[1].ToString());
                    model.descricao= linha.ItemArray[2].ToString();
                    model.observacoes= linha.ItemArray[3].ToString();
                    model.IdStatus = int.Parse(linha.ItemArray[6].ToString());
                }
            }



            model.Usuarios = new List<SelectListItem>();
            model.Usuarios.Add(new SelectListItem() { Text = usuario.nome, Value = usuario.cpf });
        }

    }
}
