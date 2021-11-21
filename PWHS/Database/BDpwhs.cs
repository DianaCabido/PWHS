using System;
using System.Collections.Generic;
using System.Data;
using PWHS.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace PWHS.Database
{
    public class BDpwhs : DbContext
    {


        private static Microsoft.Data.SqlClient.SqlConnection objConection;
        public void conectar()
        {
            objConection = new SqlConnection("Data Source = LAPTOP-E1IPP5OM; Initial Catalog = BDpwhs; Integrated Security = True; ");
        }

        public void desconectar()
        {

            if (objConection != null)
            {
                if (objConection.State == ConnectionState.Open)
                {
                    objConection.Close();
                }
            }

        }


        public DataSet CadastrarPessoa(PessoaFisica pessoa)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[SalvarPessoaFisica]" + pessoa.cpfPF + "," + pessoa.cpfUsuario+ ",'" + pessoa.nomePF + "' ,'" + pessoa.dataNascimento + "','" + pessoa.IdSexo + "'," + pessoa.IdTipoSanguineo +
                    ",'" + pessoa.email + "','" + pessoa.cep + "','" + pessoa.logadouro + "'," + pessoa.numero + ",'" + pessoa.complemento + "','" + pessoa.bairro + "','" + pessoa.cidade + "','" + pessoa.IdEstado +
                    "','" + pessoa.ddd + pessoa.telefone + "'";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {
                
            }

            return DT;
        }

        public DataSet CadastrarUsuario(CadastroUsuario pessoa)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[SalvarUsuario] " + pessoa.cpf +",'" + pessoa.nomeUs + "' ," + pessoa.cnpj + ",'" + pessoa.senha + "','" + 
                    pessoa.email + "'," + pessoa.perfil + ",'" +pessoa.cep + "','" + pessoa.logadouro + "'," + pessoa.numero + ",'" + pessoa.complemento + "','" + pessoa.bairro + "','" + pessoa.cidade + "','" + pessoa.IdEstado +
                    "','" + pessoa.ddd + pessoa.telefone + "'";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }

        public DataSet RegistrarDoenca(CadastroDoenca doenca)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[SalvarDoenca] " + doenca.IdDoenca + ",'" + doenca.descricao + "' ,'" + doenca.observacoes + "','" + DateTime.Now.ToString() + "'," + doenca.IdUsuario +
                    "," + doenca.IdPessoa + "," + doenca.IdStatus ;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }

        public DataSet RegistrarAlergia(CadastroAlergia alergia)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[SalvarAlergia] " + alergia.IdTipoAlergia + ",'" + alergia.nomeAlergia + "' ,'" + alergia.descricao + "','" + DateTime.Now.ToString() + "'," + alergia.IdUsuario +
                    "," + alergia.IdPessoa + "," + alergia.IdStatus;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }

        public DataSet RegistrarVacina(CadastroVacinacao vacina)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[SalvarVacinacao] " + vacina.IdVacina + ",'" + vacina.laboratorio + "' ,'" + vacina.lote + "','" + vacina.validade + "','" + DateTime.Now.ToString() + "'," + vacina.IdUsuario +
                    "," + vacina.IdPessoa ;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }

        public DataSet RegistrarFratura(CadastroFratura fratura)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[SalvarFratura]  " + fratura.IdTipoLesao + "," + fratura.IdCausaFratura + ",'" + fratura.nomeFratura + "','" + fratura.descricao + "','" + DateTime.Now.ToString() + "'," + fratura.IdUsuario +
                    "," + fratura.IdPessoa + "," + fratura.IdStatus;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }



        public DataSet EditarDoenca(CadastroDoenca doenca)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[EditarDoenca]  " + doenca.codigoDoenca + "," + doenca.IdDoenca + ",'" + doenca.descricao + "' ,'" + doenca.observacoes + "','" + DateTime.Now.ToString() + "'," + doenca.IdUsuario +
                    "," + doenca.IdPessoa + "," + doenca.IdStatus;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet EditarAlergia(CadastroAlergia alergia)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[EditarAlergia] " + alergia.codigoAlergia +","+ alergia.IdTipoAlergia + ",'" + alergia.nomeAlergia + "' ,'" + alergia.descricao + "','" + DateTime.Now.ToString() + "'," + alergia.IdUsuario +
                    "," + alergia.IdPessoa + "," + alergia.IdStatus;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet EditarFratura(CadastroFratura fratura)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[EditarFratura]   " + fratura.codigoFratura+ "," + fratura.IdTipoLesao + "," + fratura.IdCausaFratura + ",'" + fratura.nomeFratura + "','" + fratura.descricao + "','" + DateTime.Now.ToString() + "'," + fratura.IdUsuario +
                    "," + fratura.IdPessoa + "," + fratura.IdStatus;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet EditarPessoa(PessoaFisica pessoa)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[EditarPessoaFisica]" + pessoa.cpfPF + "," + pessoa.cpfUsuario + ",'" + pessoa.nomePF + "' ,'" + pessoa.dataNascimento + "','" + pessoa.IdSexo + "'," + pessoa.IdTipoSanguineo +
                    ",'" + pessoa.email + "','" + pessoa.cep + "','" + pessoa.logadouro + "'," + pessoa.numero + ",'" + pessoa.complemento + "','" + pessoa.bairro + "','" + pessoa.cidade + "','" + pessoa.IdEstado +
                    "','" + pessoa.ddd + pessoa.telefone + "'";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet EditarUsuario(string cpf , string senha)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[EditarUsuario]" + cpf + ",'" + senha + "'";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet EditarEntidade(string cnpj, string senha)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[EditarEntidade]" + cnpj + ",'" + senha + "'";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet EditarUsuario(CadastroUsuario pessoa)
        {

            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[EditarUsuarioCompleto] " + pessoa.cpf + ",'" + pessoa.nomeUs + "' ," + pessoa.perfil + ",'" + pessoa.email + "','" + pessoa.cep + "','" + pessoa.logadouro + "'," + pessoa.numero + ",'" + pessoa.complemento + "','" + pessoa.bairro + "','" + pessoa.cidade + "','" + pessoa.IdEstado +
                    "','" + pessoa.ddd + pessoa.telefone + "'";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }



        public DataSet ValidarUsuario(string cpf)
        {
            DataSet DT = new DataSet();
            try
            {
                
                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[ValidarUsuario]" + cpf ;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }
            return DT;

        }
        public DataSet ValidarEntidade(string cnpj)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "execute [dbo].[ValidarEntidade]" + cnpj;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }
            return DT;

        }
        public DataSet FiltrarPessoas(string cpf, string nome)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();
                objCommand = objConection.CreateCommand();

                if (cpf != "" && nome == "")
                {
                    objCommand.CommandText = "select * from dbo.PessoaFisica where cpf = " + cpf;

                }
                else if (cpf == "" && nome != "")
                {
                    objCommand.CommandText = "select * from dbo.PessoaFisica where nome like '%" + nome + "%'";
                }
                else if (cpf != "" && nome != "")
                {
                    objCommand.CommandText = "select * from dbo.PessoaFisica where nome like '%" + nome + "%' and cpf = " + cpf;
                }
                else
                {
                    objCommand.CommandText = "select * from dbo.PessoaFisica";
                }



                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet FiltrarUsuarios(string cpf, string nome, string cnpj)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();
                objCommand = objConection.CreateCommand();

                if (cpf != "" && nome == "")
                {
                    objCommand.CommandText = "select * from dbo.Usuario where cpf = " + cpf + "and cnpjEntidade = " + cnpj;

                }
                else if (cpf == "" && nome != "")
                {
                    objCommand.CommandText = "select * from dbo.Usuario where nome like '%" + nome + "%'" + "and cnpjEntidade = " + cnpj;
                }
                else if (cpf != "" && nome != "")
                {
                    objCommand.CommandText = "select * from dbo.Usuario where nome like '%" + nome + "%' and cpf = " + cpf + "and cnpjEntidade = " + cnpj;
                }
                else
                {
                    objCommand.CommandText = "select * from dbo.Usuario where cnpjEntidade =" + cnpj;
                }



                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }



        public DataSet ObterHistorico(string cpf)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "USE [BDpwhs] execute [dbo].[ObterHistorico]" + cpf;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet ObterDoenca(string codDoenca)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select * from Doenca where codigoDoenca =" + codDoenca;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet ObterAlergia(string codAlergia)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select * from Alergia where codigoAlergia =" + codAlergia;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet ObterFratura(string codFratura)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select * from Fratura where codigoFratura =" + codFratura;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet ObterPessoaFisica(string cpf)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select cpf,nome ,dataNascimento, sexo,codigoTipoSanguineo ,email ,cep ,logadouro ,numero,complemento ," +
                    "bairro,cidade,estado,telefone from PessoaFisica pf inner join Endereco ed on pf.codigoEndereco = ed.codigoEndereco where cpf = " + cpf;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet ObterUsuario(string cpf)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select u.cpf,u.nome,u.email,u.codigoPerfil, e.cep,e.logadouro,e.numero,e.complemento,e.bairro,e.cidade,e.estado,e.telefone from Usuario u inner join Endereco e on u.codigoEndereco = e.codigoEndereco where u.cpf =" + cpf;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet ObterHistoricoAlergia(string idAlergia)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "USE [BDpwhs] execute [dbo].[ObterHistoricoAlergia]" + idAlergia;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet ObterHistoricoDoenca(string idDoenca)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "USE [BDpwhs] execute [dbo].[ObterHistoricoDoenca]" + idDoenca;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
        public DataSet ObterHistoricoFratura(string idFratura)
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "USE [BDpwhs] execute [dbo].[ObterHistoricoFratura]" + idFratura;

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }


        public DataSet ConsultarPessoas()
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select * from dbo.PessoaFisica";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }

        public DataSet ConsultarStatus()
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select * from dbo.Status";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }

        public DataSet ConsultarVacinas()
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select * from dbo.Vacina";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }

        public DataSet ConsultarTipoDoencas()
        {
            DataSet DT = new DataSet();
            try
            {

                SqlDataAdapter objDataAdapter = new SqlDataAdapter();
                SqlCommand objCommand = new SqlCommand();



                objCommand = objConection.CreateCommand();
                objCommand.CommandText = "select * from dbo.TipoDoenca";

                objDataAdapter = new SqlDataAdapter(objCommand);
                objDataAdapter.Fill(DT);

            }
            catch (Exception e)
            {

            }

            return DT;
        }
    }
}



