using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace SistemaArmario
{
    internal class DBArmario
    {
        public static string path = Path.Combine(Directory.GetCurrentDirectory(), "setor_armarios.sqlite");

        private static SqliteConnection DataBaseconnection()
        {
            var connection = new SqliteConnection($"Data Source={path}");
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();
            }

            return connection;
        }

        // =============================================
        // CRIAÇÃO DO BANCO
        // =============================================

        public static void CriarDataBase()
        {
            string? dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
        }

        public static void CriarTabela()
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string[] sqls = {
                        @"CREATE TABLE IF NOT EXISTS vestiario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            vestiario TEXT NOT NULL UNIQUE
                        );",
                        @"CREATE TABLE IF NOT EXISTS armario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            numero_armario INTEGER NOT NULL,
                            vestiario_id INTEGER NOT NULL,
                            ocupado INTEGER NOT NULL DEFAULT 0,
                            FOREIGN KEY (vestiario_id) REFERENCES vestiario(id)
                        );",
                        "CREATE INDEX IF NOT EXISTS idx_armario_vestiario_id ON armario (vestiario_id);",
                        @"CREATE TABLE IF NOT EXISTS perfil (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            permissao TEXT NOT NULL UNIQUE,
                            funcao TEXT NOT NULL
                        );",
                        @"CREATE TABLE IF NOT EXISTS funcionario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            nome TEXT NOT NULL,
                            telefone TEXT,
                            armario_id INTEGER NOT NULL,
                            matricula INTEGER NOT NULL UNIQUE,
                            perfil_id INTEGER NOT NULL,
                            FOREIGN KEY (armario_id) REFERENCES armario(id),
                            FOREIGN KEY (perfil_id) REFERENCES perfil(id)
                        );",
                        "CREATE INDEX IF NOT EXISTS idx_funcionario_armario_id ON funcionario (armario_id);",
                        "CREATE INDEX IF NOT EXISTS idx_funcionario_perfil_id ON funcionario (perfil_id);"
                    };

                    foreach (var sql in sqls)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar tabelas: " + ex.Message);
                throw;
            }
        }

        // =============================================
        // PERFIL
        // =============================================

        public static DataTable? GetPerfis()
        {
            return ExecutarQuery("SELECT * FROM perfil");
        }

        public static void CriarPerfisPadrao()
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string[] sqls = {
                        "INSERT OR IGNORE INTO perfil (permissao, funcao) VALUES ('ADMIN', 'Administrador');",
                        "INSERT OR IGNORE INTO perfil (permissao, funcao) VALUES ('USER', 'Funcionário');"
                    };

                    foreach (var sql in sqls)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao criar perfis: " + ex.Message);
                throw;
            }
        }

        // =============================================
        // VESTIÁRIO
        // =============================================

        public static DataTable? GetVestiarios()
        {
            return ExecutarQuery("SELECT * FROM vestiario");
        }

        public static void InserirVestiario(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return;

            try
            {
                using (var conn = DataBaseconnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT OR IGNORE INTO vestiario (vestiario) VALUES (@nome)";
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir vestiário: " + ex.Message);
                throw;
            }
        }

        public static int GetVestiarioIdPorNome(string nome)
        {
            try
            {
                using (var conn = DataBaseconnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id FROM vestiario WHERE vestiario = @nome LIMIT 1";
                    cmd.Parameters.AddWithValue("@nome", nome);
                    var resultado = cmd.ExecuteScalar();
                    return resultado == null ? -1 : Convert.ToInt32(resultado);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar vestiário: " + ex.Message);
                throw;
            }
        }

        // =============================================
        // ARMÁRIO
        // =============================================

        public static DataTable? GetArmarios()
        {
            return ExecutarQuery("SELECT * FROM armario");
        }

        public static DataTable? GetArmariosComStatus(int vestiarioId)
        {
            try
            {
                using (var conn = DataBaseconnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT a.id, a.numero_armario,
                               CASE WHEN f.id IS NULL THEN 0 ELSE 1 END AS ocupado
                        FROM armario a
                        LEFT JOIN funcionario f ON f.armario_id = a.id
                        WHERE a.vestiario_id = @vestiario_id
                        ORDER BY a.numero_armario";

                    cmd.Parameters.AddWithValue("@vestiario_id", vestiarioId);

                    var table = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                        table.Load(reader);

                    return table;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar armários: " + ex.Message);
                throw;
            }
        }

        public static void PopularArmariosFixosExemplo(int vestiarioId, int quantidade = 10)
        {
            try
            {
                using (var conn = DataBaseconnection())
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "INSERT OR IGNORE INTO armario (numero_armario, vestiario_id, ocupado) VALUES (@num, @vid, 0)";
                            cmd.Transaction = transaction;

                            var pNum = cmd.Parameters.Add("@num", SqliteType.Integer);
                            var pVid = cmd.Parameters.Add("@vid", SqliteType.Integer);
                            pVid.Value = vestiarioId;

                            for (int numero = 1; numero <= quantidade; numero++)
                            {
                                pNum.Value = numero;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao popular armários: " + ex.Message);
                throw;
            }
        }

        // =============================================
        // FUNCIONÁRIO
        // =============================================

        public static DataTable? GetFuncionarios()
        {
            return ExecutarQuery("SELECT * FROM funcionario");
        }

        public static DataTable? GetFuncionarioById(int id)
        {
            try
            {
                using (var conn = DataBaseconnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT f.id, f.nome, f.telefone, f.matricula,
                               a.numero_armario, v.vestiario
                        FROM funcionario f
                        INNER JOIN armario a ON a.id = f.armario_id
                        INNER JOIN vestiario v ON v.id = a.vestiario_id
                        WHERE f.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    var table = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                        table.Load(reader);

                    return table;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar funcionário: " + ex.Message);
                throw;
            }
        }

        public static void InserirFuncionario(string nome, string? telefone, int armarioId, int matricula, int perfilId)
        {
            if (string.IsNullOrWhiteSpace(nome) || armarioId <= 0 || matricula <= 0 || perfilId <= 0)
                throw new ArgumentException("Dados inválidos para inserir funcionário.");

            try
            {
                using (var conn = DataBaseconnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO funcionario (nome, telefone, armario_id, matricula, perfil_id)
                        VALUES (@nome, @telefone, @armario_id, @matricula, @perfil_id)";

                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@telefone", (object?)telefone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@armario_id", armarioId);
                    cmd.Parameters.AddWithValue("@matricula", matricula);
                    cmd.Parameters.AddWithValue("@perfil_id", perfilId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir funcionário: " + ex.Message);
                throw; // Repassa para a tela tratar e mostrar mensagem adequada
            }
        }

        // =============================================
        // LOGIN
        // =============================================

        public static DataTable? LoginPorMatricula(int matricula)
        {
            try
            {
                using (var conn = DataBaseconnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT f.id, f.nome, f.telefone, f.armario_id, f.matricula, p.permissao
                        FROM funcionario f
                        INNER JOIN perfil p ON p.id = f.perfil_id
                        WHERE f.matricula = @matricula";

                    cmd.Parameters.AddWithValue("@matricula", matricula);

                    var table = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                        table.Load(reader);

                    return table;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao fazer login: " + ex.Message);
                throw;
            }
        }

        // =============================================
        // DADOS DE TESTE (só popula se vazio)
        // =============================================

        public static void PopularDadosTeste()
        {
            try
            {
                var funcionarios = GetFuncionarios();
                if (funcionarios != null && funcionarios.Rows.Count > 0)
                    return;

                var perfis = GetPerfis();
                if (perfis == null || perfis.Rows.Count == 0)
                    throw new InvalidOperationException("Perfis padrão não encontrados. Chame CriarPerfisPadrao() antes.");

                var armarios = GetArmarios();
                if (armarios == null || armarios.Rows.Count < 2)
                    throw new InvalidOperationException("Armários insuficientes. Popule vestiário/armários antes de criar dados de teste.");

                // ADMIN: matrícula 1111, armário 1
                InserirFuncionario("Administrador", null, 1, 1111, 1);

                // USER: matrícula 2222, armário 2
                InserirFuncionario("Funcionário Teste", null, 2, 2222, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao popular dados de teste: " + ex.Message);
                throw;
            }
        }

        // =============================================
        // AUXILIAR INTERNO
        // =============================================

        private static DataTable? ExecutarQuery(string sql)
        {
            try
            {
                using (var conn = DataBaseconnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    var table = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                        table.Load(reader);
                    return table;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar query: {ex.Message}");
                throw;
            }
        }
    }
}