using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;

namespace Db
{
    class Program
    {
        static string connString;
        static DbProviderFactory factory;

        static void Main(string[] args)
        {
            factory = DbProviderFactories.GetFactory(ConfigurationManager.AppSettings["dbProvider"]);
            connString = ConfigurationManager.ConnectionStrings["dbConnString"].ConnectionString;

            using (DbConnection conn = factory.CreateConnection())
            {
                conn.ConnectionString = connString;
                conn.Open();

                //Produto produtoInsert = new Produto()
                //{
                //    Id = 2,
                //    Nome = "Cadeira",
                //    Descricao = "Cadeira gamer",
                //    Valor = 3
                //};

                //Update(conn, produtoInsert);
                Delete(conn, 2);

                List<Produto> produtos = List(conn);
                foreach (var produto in produtos)
                {
                    Console.WriteLine(produto);
                }

                //Produto p = Find(conn, 2);
                //Console.Write(p);
            }
        }

        static List<Produto> List(DbConnection conn)
        {
            List<Produto> produtos = new List<Produto>();

            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT id, nome, valor, descricao from produto ORDER BY id";

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Produto produto = new Produto();
                        produto.Id = (int)dr["id"];
                        produto.Nome = (string)dr["nome"];
                        produto.Valor = Convert.ToDouble((decimal)dr["valor"]);
                        produto.Descricao = (string)dr["descricao"];
                        produtos.Add(produto);
                    }
                }
            }

            return produtos;
        }

        static void Insert(DbConnection conn, Produto produto)
        {
            DbCommand cmd = factory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO produto (nome, valor, descricao) VALUES (@Nome, @Valor, @Descricao)";

            DbParameter param = factory.CreateParameter();
            param.ParameterName = "@Nome";
            param.Value = produto.Nome;
            cmd.Parameters.Add(param);

            param = factory.CreateParameter();
            param.ParameterName = "@Valor";
            param.Value = produto.Valor;
            cmd.Parameters.Add(param);

            param = factory.CreateParameter();
            param.ParameterName = "@Descricao";
            param.Value = produto.Descricao;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
        }

        static void Update(DbConnection conn, Produto produto)
        {
            DbCommand cmd = factory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE produto SET nome = @Nome, valor = @Valor, descricao = @Descricao WHERE id = @Id";

            DbParameter param = factory.CreateParameter();
            param.ParameterName = "@Nome";
            param.Value = produto.Nome;
            cmd.Parameters.Add(param);

            param = factory.CreateParameter();
            param.ParameterName = "@Valor";
            param.Value = produto.Valor;
            cmd.Parameters.Add(param);

            param = factory.CreateParameter();
            param.ParameterName = "@Descricao";
            param.Value = produto.Descricao;
            cmd.Parameters.Add(param);

            param = factory.CreateParameter();
            param.ParameterName = "@Id";
            param.Value = produto.Id;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
        }

        static void Delete(DbConnection conn, int id)
        {
            DbCommand cmd = factory.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM produto WHERE id = @Id";

            DbParameter param = factory.CreateParameter();
            param = factory.CreateParameter();
            param.ParameterName = "@Id";
            param.Value = id;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
        }

        static Produto Find(DbConnection conn, int id)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT id, nome, valor, descricao FROM produto WHERE id = @Id";

                DbParameter param = factory.CreateParameter();
                param = factory.CreateParameter();
                param.ParameterName = "@Id";
                param.Value = id;
                cmd.Parameters.Add(param);

                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    if(!dr.Read())
                    {
                        return null;
                    }

                    Produto produto = new Produto();
                    produto.Id = (int)dr["id"];
                    produto.Nome = (string)dr["nome"];
                    produto.Valor = Convert.ToDouble((decimal)dr["valor"]);
                    produto.Descricao = (string)dr["descricao"];

                    return produto;
                }
            }
        }

        static int Count(DbConnection conn)
        {
            using (DbCommand cmd = factory.CreateCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT COUNT(*) FROM produto";

                return (int)cmd.ExecuteScalar();
            }
        }
    }

    class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }

        public override string ToString() => string.Format("{0,-3}{1,-15}{2,-15:C}{3}", Id, Nome, Valor, Descricao);
    }
}
