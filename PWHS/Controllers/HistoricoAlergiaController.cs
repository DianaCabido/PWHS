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
    public class HistoricoAlergiaController : Controller
    {
        private static Usuario usuario = new Usuario();
        private BDpwhs _banco;
        private static Historico historico;

        public IActionResult HistoricoAlergia(Usuario usu)
        {
            usuario = usu;
            historico = new Historico();
            ViewData["MsgLayout"] = usuario.nome + " - " + usuario.nomeEntidade;
            historico.listAlergias = new List<Alergia>();

            ObterHistorico(usuario.codigoEdicao);

            return View("HistoricoAlergia", historico);
        }

        public void ObterHistorico(string idAlergia)
        {
            DataSet DT = new DataSet();
            _banco = new BDpwhs();
            _banco.conectar();

            DT = _banco.ObterHistoricoAlergia(idAlergia);

            _banco.desconectar();

            if (DT.Tables.Count > 0)
            {

                MontarListaAlergia(DT.Tables[0]);

            }

        }

        public void MontarListaAlergia(DataTable tbAlergia)
        {
            List<Alergia> listAlergias = new List<Alergia>();

            foreach (DataRow linha in tbAlergia.Rows)
            {
                Alergia al = new Alergia();

                al.codigo = linha.ItemArray[0].ToString();

          
                al.tipoAlergia = linha.ItemArray[1].ToString();
            

                al.nome = linha.ItemArray[2].ToString();
                al.descricao = linha.ItemArray[3].ToString();
                al.dataRegistro = DateTime.Parse(linha.ItemArray[4].ToString()).ToString("dd/MM/yyyy");
                al.status = linha.ItemArray[7].ToString();
                al.usuario = linha.ItemArray[5].ToString();
                  

                listAlergias.Add(al);
            }

            historico.listAlergias = listAlergias;
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