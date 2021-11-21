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
    public class HistoricoDoencaController : Controller
    {
        private static Usuario usuario = new Usuario();
        private BDpwhs _banco;
        private static Historico historico;

        public IActionResult HistoricoDoenca(Usuario usu)
        {
            usuario = usu;
            historico = new Historico();
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            historico.listDoencas = new List<Doenca>();

            ObterHistorico(usuario.codigoEdicao);

            return View("HistoricoDoenca", historico);
        }

        public void ObterHistorico(string idDoenca)
        {
            DataSet DT = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DT = _banco.ObterHistoricoDoenca(idDoenca);

            _banco.desconectar();

            if (DT.Tables.Count > 0)
            {

                MontarListaDoenca(DT.Tables[0]);

            }

        }

        public void MontarListaDoenca(DataTable tbDoenca)
        {
            List<Doenca> listDoencas = new List<Doenca>();

            foreach (DataRow linha in tbDoenca.Rows)
            {
                Doenca al = new Doenca();

                al.codigoDoenca = linha.ItemArray[0].ToString();
                al.tipoDoenca = linha.ItemArray[1].ToString();
                al.nome = linha.ItemArray[2].ToString();
                al.observacao = linha.ItemArray[3].ToString();
                al.dataRegistro = DateTime.Parse(linha.ItemArray[4].ToString()).ToString("dd/MM/yyyy");

                al.usuario = linha.ItemArray[5].ToString();
                al.status = linha.ItemArray[7].ToString();


                listDoencas.Add(al);
            }

            historico.listDoencas = listDoencas;
        }

        [HttpPost]
        public IActionResult Cancelar()
        {
            usuario.codigoEdicao = null;
            return RedirectToAction("ConsultaHistorico", "ConsultaHistorico", usuario);
     

        }

        public IActionResult ChamarTelaInicial()
        {
            usuario.pessoaCPF = null;
            usuario.pessoaNome = null;
            return RedirectToAction("TelaInicial", "TelaInicial", usuario);
        }
    }
}