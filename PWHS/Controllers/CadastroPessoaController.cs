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
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PWHS.Controllers
{
    public class CadastroPessoaController : Controller
    {


        private BDpwhs _banco;
        private static PessoaFisica pessoa = new PessoaFisica();
        private static Usuario usuario = new Usuario();



        public IActionResult CadastroPessoa(Usuario usu)
        {
            usuario = usu;
            pessoa = new PessoaFisica();
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            ObterSexo();
            ObterTipoSanguineo();
            ObterEstado();

            @ViewData["Edicao"] = null;

            if (usuario.codigoEdicao != null)
            {
                MontarTelaEdicao();
            }

            return View("CadastroPessoa", pessoa);
        }

        [HttpPost]
        public IActionResult Salvar(string submitBtt)
        {

            if (submitBtt == "cadastra")
            {
                if (usuario.codigoEdicao == null)
                {

                    if (ValidaCadastroPessoa())
                    {

                        DataSet DTPessoas = new DataSet();
                        _banco = new BDpwhs();
                        _banco.conectar();

                        DTPessoas = _banco.CadastrarPessoa(pessoa);

                        _banco.desconectar();

                        if (DTPessoas.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DTPessoas.Tables[0].Rows)
                            {
                                if (linha.ItemArray[0].ToString() == "1")
                                {
                                    ViewData["Erro"] = null;
                                    ViewData["Sucesso"] = "Cadastro de Pessoa Fisica realizado com sucesso!";
                                    pessoa = new PessoaFisica();
                                }
                                else
                                {

                                    ViewData["Erro"] = "Já existe uma Pessoa Fisica cadastrada com esse CPF!";
                                    ViewData["Sucesso"] = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ValidaEdicaoPessoa())
                    {
                        DataSet DTPessoas = new DataSet();
                        _banco = new BDpwhs();
                        _banco.conectar();

                        DTPessoas = _banco.EditarPessoa(pessoa);

                        _banco.desconectar();

                        if (DTPessoas.Tables.Count > 0)
                        {

                            foreach (DataRow linha in DTPessoas.Tables[0].Rows)
                            {

                                if (linha.ItemArray[0].ToString() == "0")
                                {
                                    ViewData["Erro"] = null;
                                    ViewData["Sucesso"] = "Edição de Pessoa Física realizada com sucesso!";

                                    pessoa = new PessoaFisica();

                                    ObterSexo();
                                    ObterTipoSanguineo();
                                    ObterEstado();
                                    @ViewData["Edicao"] = null;

                                    usuario.codigoEdicao = null;
                                }
                                else
                                {

                                    ViewData["Erro"] = "Erro ao Editar Pessoa Física!";
                                    ViewData["Sucesso"] = null;
                                }

                            }
                        }
                    }

                }



                return View("CadastroPessoa", pessoa);

            }
            else
            {
                return RedirectToAction("TelaInicial", "TelaInicial", usuario);
            }
        }

        public void MontarTelaEdicao()
        {
            @ViewData["Edicao"] = true;
            DataSet DTPessoa = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DTPessoa = _banco.ObterPessoaFisica(usuario.codigoEdicao);

            _banco.desconectar();

            if (DTPessoa.Tables.Count > 0)
            {

                foreach (DataRow linha in DTPessoa.Tables[0].Rows)
                {

                    pessoa.cpfPF = linha.ItemArray[0].ToString();
                    pessoa.nomePF = linha.ItemArray[1].ToString();
                    pessoa.dataNascimento = DateTime.Parse(linha.ItemArray[2].ToString()).ToString("yyyy-MM-dd");
                    pessoa.IdSexo = int.Parse(linha.ItemArray[3].ToString());
                    pessoa.IdTipoSanguineo = int.Parse(linha.ItemArray[4].ToString());
                    pessoa.email = linha.ItemArray[5].ToString();
                    pessoa.cep = linha.ItemArray[6].ToString();
                    pessoa.logadouro = linha.ItemArray[7].ToString();
                    pessoa.numero = linha.ItemArray[8].ToString();
                    pessoa.complemento = linha.ItemArray[9].ToString();
                    pessoa.bairro = linha.ItemArray[10].ToString();
                    pessoa.cidade = linha.ItemArray[11].ToString();
                    pessoa.IdEstado = int.Parse(linha.ItemArray[12].ToString());
                    pessoa.ddd = linha.ItemArray[13].ToString().Substring(0, 3);
                    pessoa.telefone = linha.ItemArray[13].ToString().Substring(3, 9);
                }
            }
        }

        public void ObterSexo()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Selected = false, Text = "M", Value = "1" });
            list.Add(new SelectListItem() { Selected = false, Text = "F", Value = "2" });

            pessoa.listSexo = list;
        }

        public void ObterTipoSanguineo()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Selected = false, Text = "-A", Value = "1" });
            list.Add(new SelectListItem() { Selected = false, Text = "+A", Value = "2" });
            list.Add(new SelectListItem() { Selected = false, Text = "-B", Value = "3" });
            list.Add(new SelectListItem() { Selected = false, Text = "+B", Value = "4" });
            list.Add(new SelectListItem() { Selected = false, Text = "-AB", Value = "5" });
            list.Add(new SelectListItem() { Selected = false, Text = "+AB", Value = "6" });
            list.Add(new SelectListItem() { Selected = false, Text = "-O", Value = "7" });
            list.Add(new SelectListItem() { Selected = false, Text = "+O", Value = "8" });

            pessoa.listTipoSanguineo = list;
        }

        public void ObterEstado()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Selected = false, Text = "AC", Value = "1" });
            list.Add(new SelectListItem() { Selected = false, Text = "AL", Value = "2" });
            list.Add(new SelectListItem() { Selected = false, Text = "AP", Value = "3" });
            list.Add(new SelectListItem() { Selected = false, Text = "AM", Value = "4" });
            list.Add(new SelectListItem() { Selected = false, Text = "BA", Value = "5" });
            list.Add(new SelectListItem() { Selected = false, Text = "CE", Value = "6" });
            list.Add(new SelectListItem() { Selected = false, Text = "DF", Value = "7" });
            list.Add(new SelectListItem() { Selected = false, Text = "ES", Value = "8" });
            list.Add(new SelectListItem() { Selected = false, Text = "GO", Value = "9" });
            list.Add(new SelectListItem() { Selected = false, Text = "MA", Value = "10" });
            list.Add(new SelectListItem() { Selected = false, Text = "MT", Value = "11" });
            list.Add(new SelectListItem() { Selected = false, Text = "MS", Value = "12" });
            list.Add(new SelectListItem() { Selected = false, Text = "MG", Value = "13" });
            list.Add(new SelectListItem() { Selected = false, Text = "PA", Value = "14" });
            list.Add(new SelectListItem() { Selected = false, Text = "PB", Value = "15" });
            list.Add(new SelectListItem() { Selected = false, Text = "PE", Value = "16" });
            list.Add(new SelectListItem() { Selected = false, Text = "PI", Value = "17" });
            list.Add(new SelectListItem() { Selected = false, Text = "RJ", Value = "18" });
            list.Add(new SelectListItem() { Selected = false, Text = "RN", Value = "19" });
            list.Add(new SelectListItem() { Selected = false, Text = "RS", Value = "20" });
            list.Add(new SelectListItem() { Selected = false, Text = "RO", Value = "21" });
            list.Add(new SelectListItem() { Selected = false, Text = "RR", Value = "22" });
            list.Add(new SelectListItem() { Selected = false, Text = "SC", Value = "23" });
            list.Add(new SelectListItem() { Selected = false, Text = "SP", Value = "24" });
            list.Add(new SelectListItem() { Selected = false, Text = "SE", Value = "25" });
            list.Add(new SelectListItem() { Selected = false, Text = "TO", Value = "26" });


            pessoa.listEstado = list;
        }

        public bool ValidaCadastroPessoa()
        {
            string msg = "";

            pessoa.cpfPF = HttpContext.Request.Form["cpfPF"].ToString().Trim();
            pessoa.nomePF = HttpContext.Request.Form["nomePF"].ToString().Trim();
            pessoa.dataNascimento = HttpContext.Request.Form["dataNascimento"].ToString().Trim();
            pessoa.IdSexo = int.Parse(HttpContext.Request.Form["IdSexo"].ToString().Trim());
            pessoa.IdTipoSanguineo = int.Parse(HttpContext.Request.Form["IdTipoSanguineo"].ToString().Trim());
            pessoa.email = HttpContext.Request.Form["email"].ToString().Trim();
            pessoa.cep = HttpContext.Request.Form["cep"].ToString().Trim();
            pessoa.logadouro = HttpContext.Request.Form["logadouro"].ToString().Trim();
            pessoa.numero = HttpContext.Request.Form["numero"].ToString().Trim();
            pessoa.complemento = HttpContext.Request.Form["complemento"].ToString().Trim();
            pessoa.bairro = HttpContext.Request.Form["bairro"].ToString().Trim();
            pessoa.cidade = HttpContext.Request.Form["cidade"].ToString().Trim();
            pessoa.IdEstado = int.Parse(HttpContext.Request.Form["IdEstado"].ToString().Trim());
            pessoa.telefone = HttpContext.Request.Form["telefone"].ToString().Trim();
            pessoa.ddd = HttpContext.Request.Form["ddd"].ToString().Trim();
            pessoa.cpfUsuario = usuario.cpf;


            DateTime dataAtual = DateTime.Now;

            if (pessoa.cpfPF.Length < 11)
            {
                msg = "CPF deve conter 11 dígitos!";
            }
            else if (ValidaCPF(pessoa.cpfPF) == false)
            {
                msg = "CPF inválido!";
            }
            else if (pessoa.nomePF.Length < 10)
            {
                msg = "Nome deve conter ao menos 10 caracteres!";
            }
            else if (pessoa.dataNascimento.Length < 10)
            {
                msg = "Data de nascimento inválida!";
            }
            else if (DateTime.Parse(pessoa.dataNascimento) > dataAtual)
            {
                msg = "Data de nascimento não pode ser maior que a data atual!";
            }
            else if (IsValidEmail(pessoa.email) == false)
            {
                msg = "Email inválido!";
            }
            else if (pessoa.cep.Length < 8)
            {
                msg = "Cep inválido!";
            }
            else if (pessoa.logadouro == "")
            {
                msg = "É obrigatório o preenchimento do campo logadouro!";
            }
            else if (pessoa.numero == "")
            {
                msg = "É obrigatório o preenchimento do campo número!";
            }
            else if (pessoa.complemento == "")
            {
                msg = "É obrigatório o preenchimento do campo complemento!";
            }
            else if (pessoa.bairro == "")
            {
                msg = "É obrigatório o preenchimento do campo bairro!";
            }
            else if (pessoa.cidade == "")
            {
                msg = "É obrigatório o preenchimento do campo cidade!";
            }
            else if (pessoa.ddd.Length < 3)
            {
                msg = "DDD deve conter ao menos 3 dígitos!";
            }
            else if (pessoa.telefone.Length < 9)
            {
                msg = "Telefone deve conter ao menos 9 dígitos!";
            }

            if (msg == "")
            {
                ViewData["Erro"] = null;
                ViewData["Sucesso"] = null;
                return true;
            }
            else
            {
                ViewData["Sucesso"] = null;
                ViewData["Erro"] = msg;
                return false;
            }

        }
        public bool ValidaEdicaoPessoa()
        {
            string msg = "";

            pessoa.cpfPF = usuario.codigoEdicao;
            pessoa.nomePF = HttpContext.Request.Form["nomePF"].ToString().Trim();
            pessoa.dataNascimento = HttpContext.Request.Form["dataNascimento"].ToString().Trim();
            pessoa.IdSexo = int.Parse(HttpContext.Request.Form["IdSexo"].ToString().Trim());
            pessoa.IdTipoSanguineo = int.Parse(HttpContext.Request.Form["IdTipoSanguineo"].ToString().Trim());
            pessoa.email = HttpContext.Request.Form["email"].ToString().Trim();
            pessoa.cep = HttpContext.Request.Form["cep"].ToString().Trim();
            pessoa.logadouro = HttpContext.Request.Form["logadouro"].ToString().Trim();
            pessoa.numero = HttpContext.Request.Form["numero"].ToString().Trim();
            pessoa.complemento = HttpContext.Request.Form["complemento"].ToString().Trim();
            pessoa.bairro = HttpContext.Request.Form["bairro"].ToString().Trim();
            pessoa.cidade = HttpContext.Request.Form["cidade"].ToString().Trim();
            pessoa.IdEstado = int.Parse(HttpContext.Request.Form["IdEstado"].ToString().Trim());
            pessoa.telefone = HttpContext.Request.Form["telefone"].ToString().Trim();
            pessoa.ddd = HttpContext.Request.Form["ddd"].ToString().Trim();
            pessoa.cpfUsuario = usuario.cpf;


            DateTime dataAtual = DateTime.Now;

            if (pessoa.nomePF.Length < 10)
            {
                msg = "Nome deve conter ao menos 10 caracteres!";
            }
            else if (pessoa.dataNascimento.Length < 10)
            {
                msg = "Data de nascimento inválida!";
            }
            else if (DateTime.Parse(pessoa.dataNascimento) > dataAtual)
            {
                msg = "Data de nascimento não pode ser maior que a data atual!";
            }
            else if (IsValidEmail(pessoa.email) == false)
            {
                msg = "Email inválido!";
            }
            else if (pessoa.cep.Length < 8)
            {
                msg = "Cep inválido!";
            }
            else if (pessoa.logadouro == "")
            {
                msg = "É obrigatório o preenchimento do campo logadouro!";
            }
            else if (pessoa.numero == "")
            {
                msg = "É obrigatório o preenchimento do campo número!";
            }
            else if (pessoa.complemento == "")
            {
                msg = "É obrigatório o preenchimento do campo complemento!";
            }
            else if (pessoa.bairro == "")
            {
                msg = "É obrigatório o preenchimento do campo bairro!";
            }
            else if (pessoa.cidade == "")
            {
                msg = "É obrigatório o preenchimento do campo cidade!";
            }
            else if (pessoa.ddd.Length < 3)
            {
                msg = "DDD deve conter ao menos 3 dígitos!";
            }
            else if (pessoa.telefone.Length < 9)
            {
                msg = "Telefone deve conter ao menos 9 dígitos!";
            }

            if (msg == "")
            {
                ViewData["Erro"] = null;
                ViewData["Sucesso"] = null;
                return true;
            }
            else
            {
                ViewData["Sucesso"] = null;
                ViewData["Erro"] = msg;
                return false;
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

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

}

