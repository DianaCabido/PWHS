
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
    public class HistoricoFraturaController : Controller
    {
        private static Usuario usuario = new Usuario();
        private BDpwhs _banco;
        private static Historico historico;

        public IActionResult HistoricoFratura(Usuario usu)
        {
            usuario = usu;
            historico = new Historico();
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            historico.listFraturas = new List<Fratura>();

            ObterHistorico(usuario.codigoEdicao);

            return View("HistoricoFratura", historico);
        }

        public void ObterHistorico(string idFratura)
        {
            DataSet DT = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DT = _banco.ObterHistoricoFratura(idFratura);

            _banco.desconectar();

            if (DT.Tables.Count > 0)
            {

                MontarListaFratura(DT.Tables[0]);

            }

        }

        public void MontarListaFratura(DataTable tbFratura)
        {
            List<Fratura> listFraturas = new List<Fratura>();

            foreach (DataRow linha in tbFratura.Rows)
            {
                Fratura al = new Fratura();

                al.codigoFratura = linha.ItemArray[0].ToString();


                al.tipoLesao = linha.ItemArray[1].ToString();

                al.causaFratura = linha.ItemArray[2].ToString();


                al.nome = linha.ItemArray[3].ToString();
                al.descricao = linha.ItemArray[4].ToString();
                al.dataRegistro = DateTime.Parse(linha.ItemArray[5].ToString()).ToString("dd/MM/yyyy");
                al.usuario = linha.ItemArray[6].ToString();

                al.status = linha.ItemArray[8].ToString();


                listFraturas.Add(al);
            }

            historico.listFraturas = listFraturas;
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