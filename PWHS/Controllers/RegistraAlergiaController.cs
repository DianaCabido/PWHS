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
    public class RegistraAlergiaController : Controller
    {
        public static CadastroAlergia model = new CadastroAlergia();
        private static Usuario usuario = new Usuario();
        private BDpwhs _banco;



        public IActionResult RegistraAlergia(Usuario usu)
        {
            usuario = usu;
            model = new CadastroAlergia();
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
                    if (ValidaCadastro())
                    {

                        DataSet DT = new DataSet();
                        _banco = new BDpwhs();
                        _banco.conectar();

                        DT = _banco.RegistrarAlergia(model);

                        _banco.desconectar();

                        if (DT.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DT.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() == "0")
                                {
                                    ViewData["Erro"] = null;
                                    ViewData["Sucesso"] = "Registro de Alergia realizado com sucesso!";
                                    model = new CadastroAlergia();
                                    MontarTelaCadastro();
                                }
                                else
                                {

                                    ViewData["Erro"] = "Erro ao Registrar Alergia!";
                                    ViewData["Sucesso"] = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ValidaEdicao())
                    {

                        DataSet DT = new DataSet();
                        _banco = new BDpwhs();
                        _banco.conectar();

                        DT = _banco.EditarAlergia(model);

                        _banco.desconectar();

                        if (DT.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DT.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() == "0")
                                {
                                    ViewData["Erro"] = null;
                                    ViewData["Sucesso"] = "Edição de Alergia realizada com sucesso!";
                                    model = new CadastroAlergia();
                                    MontarTelaCadastro();
                                    usuario.codigoEdicao = null;
                                }
                                else
                                {

                                    ViewData["Erro"] = "Erro ao Editar Alergia!";
                                    ViewData["Sucesso"] = null;
                                }
                            }
                        }
                    }
                }

                return View("RegistraAlergia", model);
            }
            else
            {
                return RedirectToAction("ConsultaHistorico", "ConsultaHistorico", usuario);
            }

        }


        public bool ValidaCadastro()
        {
            string msg = "";

            model.nomeAlergia = HttpContext.Request.Form["nomeAlergia"].ToString().Trim();
            model.descricao = HttpContext.Request.Form["descricao"].ToString().Trim();
            model.IdUsuario = usuario.cpf;
            model.IdPessoa = usuario.pessoaCPF;
            model.IdStatus = int.Parse(HttpContext.Request.Form["IdStatus"].ToString().Trim());
            model.IdTipoAlergia = int.Parse(HttpContext.Request.Form["IdTipoAlergia"].ToString().Trim());



            if (model.nomeAlergia.Length < 10)
            {
                msg = "O campo Alergia deve conter ao menos 10 caracteres!";
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

        public bool ValidaEdicao()
        {
            string msg = "";

            model.nomeAlergia = HttpContext.Request.Form["nomeAlergia"].ToString().Trim();
            model.descricao = HttpContext.Request.Form["descricao"].ToString().Trim();
            model.IdUsuario = usuario.cpf;
            model.IdPessoa = usuario.pessoaCPF;
            model.IdStatus = int.Parse(HttpContext.Request.Form["IdStatus"].ToString().Trim());         



            if (model.nomeAlergia.Length < 10)
            {
                msg = "O campo Alergia deve conter ao menos 10 caracteres!";
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
            DataSet DTPessoas = new DataSet();
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

            model.IdTipoAlergia = 1;
            model.IdStatus = int.Parse(model.Status[0].Value);

            model.TipoAlergia = new List<SelectListItem>();
            model.TipoAlergia.Add(new SelectListItem() { Text = "Alimentar", Value = "1" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Pele", Value = "2" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Nasal", Value = "3" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Respiratória", Value = "4" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Ocular", Value = "5" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Outro", Value = "6" });


            model.Usuarios = new List<SelectListItem>();
            model.Usuarios.Add(new SelectListItem() { Text = usuario.nome, Value = usuario.cpf });

        }

        public void MontarTelaEdicao()
        {
            @ViewData["Edicao"] = true;
            model.Status = new List<SelectListItem>();
            DataSet DTPessoas = new DataSet();
            DataSet DTStatus = new DataSet();
            DataSet DTEditar = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DTStatus = _banco.ConsultarStatus();
            DTEditar = _banco.ObterAlergia(usuario.codigoEdicao);

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
                    model.codigoAlergia = linha.ItemArray[0].ToString();
                    model.IdTipoAlergia = int.Parse(linha.ItemArray[1].ToString());
                    model.nomeAlergia = linha.ItemArray[2].ToString();
                    model.descricao = linha.ItemArray[3].ToString();
                    model.IdStatus = int.Parse(linha.ItemArray[6].ToString());
                }
            }

            model.TipoAlergia = new List<SelectListItem>();
            model.TipoAlergia.Add(new SelectListItem() { Text = "Alimentar", Value = "1" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Pele", Value = "2" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Nasal", Value = "3" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Respiratória", Value = "4" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Ocular", Value = "5" });
            model.TipoAlergia.Add(new SelectListItem() { Text = "Outro", Value = "6" });


            model.Usuarios = new List<SelectListItem>();
            model.Usuarios.Add(new SelectListItem() { Text = usuario.nome, Value = usuario.cpf });

        }

    }
}
