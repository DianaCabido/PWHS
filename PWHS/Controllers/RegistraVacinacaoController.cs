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
    public class RegistraVacinacaoController : Controller
    {
        public static CadastroVacinacao model = new CadastroVacinacao();

        private static Usuario usuario = new Usuario();

        private BDpwhs _banco;



        public IActionResult RegistraVacinacao(Usuario usu)
        {
            usuario = usu;
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            MontarTela();
            return View(model);
        }

        [HttpPost]
        public IActionResult Salvar(string submitBtt)
        {
            if (submitBtt == "cadastra")
            {
                if (ValidaVacinacao())
                {

                    DataSet DT = new DataSet();
                    _banco = new BDpwhs();
                    _banco.conectar();

                    DT = _banco.RegistrarVacina(model);

                    _banco.desconectar();

                    if (DT.Tables.Count > 0)
                    {

                        foreach (DataRow linha in DT.Tables[0].Rows)
                        {
                            if (linha.ItemArray[0].ToString() == "0")
                            {
                                ViewData["Erro"] = null;
                                ViewData["Sucesso"] = "Registro de Vacinação realizado com sucesso!";
                                model = new CadastroVacinacao();
                                MontarTela();
                            }
                            else
                            {

                                ViewData["Erro"] = "Erro ao Registrar Vacinação!";
                                ViewData["Sucesso"] = null;
                            }
                        }
                    }


                    return View("RegistraVacinacao", model);
                }
                else
                {
                    return View("RegistraVacinacao", model);
                }
            }
            else
            {
                return RedirectToAction("ConsultaHistorico", "ConsultaHistorico", usuario);
            }

        }


        public bool ValidaVacinacao()
        {
            string msg = "";

            model.laboratorio = HttpContext.Request.Form["laboratorio"].ToString().Trim();
            model.lote = HttpContext.Request.Form["lote"].ToString().Trim();
            model.IdVacina = HttpContext.Request.Form["vacinaSelect"].ToString().Trim();
            model.validade = HttpContext.Request.Form["validade"].ToString().Trim();
            model.IdUsuario = usuario.cpf;
            model.IdPessoa = usuario.pessoaCPF;


            DateTime dataAtual = DateTime.Now;

            if (model.laboratorio.Length < 5)
            {
                msg = "O campo Laboratório deve conter ao menos 5 caracteres!";
            }
            else if (model.lote.Length < 5)
            {
                msg = "O campo Lote deve conter ao menos 5 caracteres!";
            }
            else if (DateTime.Parse(model.validade) < dataAtual)
            {
                msg = "Vacina fora da validade!";
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

        public void MontarTela()
        {
            model.Vacinas = new List<SelectListItem>();
            DataSet DTVacinas = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DTVacinas = _banco.ConsultarVacinas();

            _banco.desconectar();


            model.Pessoas = new List<SelectListItem>();
            model.Pessoas.Add(new SelectListItem() { Text = usuario.pessoaCPF + " - " + usuario.pessoaNome, Value = usuario.pessoaCPF });

            if (DTVacinas.Tables.Count > 0)
            {

                foreach (DataRow linha in DTVacinas.Tables[0].Rows)
                {
                    model.Vacinas.Add(new SelectListItem() { Text = linha.ItemArray[1].ToString(), Value = linha.ItemArray[0].ToString() });
                }
            }


            model.Usuarios = new List<SelectListItem>();
            model.Usuarios.Add(new SelectListItem() { Text = usuario.nome, Value = usuario.cpf });
        }
    }
}
