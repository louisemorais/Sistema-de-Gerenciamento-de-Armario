using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace SistemaArmario
{
    internal class DBArmario
    {
        public static string path = Directory.GetCurrentDirectory() + "\\setor_armarios.sqlite";

        // ✅ CORRIGIDO: Removida variável estática que causava vazamento
        private static SQLiteConnection DataBaseconnection()
        {
            var connection = new SQLiteConnection("Data Source=" + path);
            connection.Open();

            // Habilita enforcement de foreign keys no SQLite
            using (var cmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
            {
                cmd.ExecuteNonQuery();
            }
            return connection;
        }

        // ✅ CORRIGIDO: Adicionado bloco catch que faltava
        public static void CriarDataBase()
        {
            try
            {
                if (!File.Exists(path))
                {
                    SQLiteConnection.CreateFile(path);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro ao criar banco de dados: " + error.Message);
            }
        }

        public static void PopularArmariosFixosExemplo(int vestiarioId)
        {
            // números fixos e status (true = ocupado, false = livre)
            var armariosFixos = new Dictionary<int, bool>
            {
                { 1, false },
                { 2, true },
                { 3, false },
                { 4, false },
                { 5, true },
                { 6, false },
                { 7, true },
                { 8, false },
                { 9, false },
                { 10, true }
            };

            InserirArmariosFixos(vestiarioId, armariosFixos);
        }

        // Insere múltiplos armários com informações fixas (número e ocupado)
        // armarios: dicionário onde a chave é o número do armário e o valor indica se está ocupado (true = ocupado)
        public static void InserirArmariosFixos(int vestiarioId, System.Collections.Generic.Dictionary<int, bool> armarios)
        {
            if (armarios == null || armarios.Count == 0) return;

            try
            {
                using (var conn = DataBaseconnection())
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = "INSERT INTO armario (numero_armario, vestiario_id, ocupado) VALUES (@numero, @vestiario_id, @ocupado)";
                        using (var cmd = new SQLiteCommand(sql, conn))
                        {
                            var pNumero = cmd.Parameters.Add("@numero", System.Data.DbType.Int32);
                            var pVestiario = cmd.Parameters.Add("@vestiario_id", System.Data.DbType.Int32);
                            var pOcupado = cmd.Parameters.Add("@ocupado", System.Data.DbType.Int32);

                            pVestiario.Value = vestiarioId;

                            foreach (var item in armarios)
                            {
                                pNumero.Value = item.Key;
                                pOcupado.Value = item.Value ? 1 : 0;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception error)
                    {
                        transaction.Rollback();  // ✅ ROLLBACK em caso de erro
                        throw;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir armários fixos: " + error.Message);
            }
        }

        public static void CriarTabela()
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = @"
                        CREATE TABLE IF NOT EXISTS vestiario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            vestiario TEXT NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS armario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            numero_armario INTEGER NOT NULL,
                            vestiario_id INTEGER NOT NULL,
                            ocupado INTEGER NOT NULL DEFAULT 0,
                            FOREIGN KEY (vestiario_id) REFERENCES vestiario(id)
                        );

                        CREATE INDEX IF NOT EXISTS idx_armario_vestiario_id ON armario (vestiario_id);

                        CREATE TABLE IF NOT EXISTS perfil (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            permissao TEXT NOT NULL,
                            funcao TEXT NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS funcionario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            nome TEXT NOT NULL,
                            telefone TEXT UNIQUE,
                            armario_id INTEGER NOT NULL,
                            matricula INTEGER NOT NULL,
                            perfil_id INTEGER NOT NULL,
                            FOREIGN KEY (armario_id) REFERENCES armario(id),
                            FOREIGN KEY (perfil_id) REFERENCES perfil(id)
                        );

                        CREATE INDEX IF NOT EXISTS idx_funcionario_armario_id ON funcionario (armario_id);
                        CREATE INDEX IF NOT EXISTS idx_funcionario_perfil_id ON funcionario (perfil_id);

                        CREATE TABLE IF NOT EXISTS Contatos (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nome TEXT NOT NULL,
                            Telefone TEXT
                        );
                    ";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro ao criar a tabela: " + error.Message);
            }
        }

        // ✅ CORRIGIDO: DataAdapter agora está dentro de using()
        public static DataTable? GetContatos()
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM Contatos";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar dados: " + error.Message);
                return null;
            }
        }

        // ✅ CORRIGIDO: DataAdapter agora está dentro de using()
        public static DataTable? GetContatoById(int id)
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM Contatos WHERE Id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar dado: " + error.Message);
                return null;
            }
        }

        // ========== MÉTODOS CRUD PARA PERFIL ==========
        // ✅ CORRIGIDO: DataAdapter agora está dentro de using()
        public static DataTable? GetPerfis()
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM perfil";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar perfis: " + error.Message);
                return null;
            }
        }

        // ✅ CORRIGIDO: Adicionada validação de entrada
        public static void InserirPerfil(string permissao, string funcao)
        {
            if (string.IsNullOrWhiteSpace(permissao) || string.IsNullOrWhiteSpace(funcao))
            {
                Console.WriteLine("Erro: Permissão e função não podem estar vazias");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO perfil (permissao, funcao) VALUES (@permissao, @funcao)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@permissao", permissao);
                        cmd.Parameters.AddWithValue("@funcao", funcao);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir perfil: " + error.Message);
            }
        }

        public static void AtualizarPerfil(int id, string permissao, string funcao)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(permissao) || string.IsNullOrWhiteSpace(funcao))
            {
                Console.WriteLine("Erro: ID inválido ou dados vazios");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE perfil SET permissao = @permissao, funcao = @funcao WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@permissao", permissao);
                        cmd.Parameters.AddWithValue("@funcao", funcao);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro atualizar perfil: " + error.Message);
            }
        }

        public static void ExcluirPerfil(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("Erro: ID inválido");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "DELETE FROM perfil WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro deletar perfil: " + error.Message);
            }
        }

        // ========== MÉTODOS CRUD PARA VESTIÁRIO ==========
        // ✅ CORRIGIDO: DataAdapter agora está dentro de using()
        public static DataTable? GetVestiarios()
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM vestiario";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar vestiários: " + error.Message);
                return null;
            }
        }

        // ✅ CORRIGIDO: Adicionada validação de entrada
        public static void InserirVestiario(string vestiario)
        {
            if (string.IsNullOrWhiteSpace(vestiario))
            {
                Console.WriteLine("Erro: Nome do vestiário não pode estar vazio");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO vestiario (vestiario) VALUES (@vestiario)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@vestiario", vestiario);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir vestiário: " + error.Message);
            }
        }

        public static void AtualizarVestiario(int id, string vestiario)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(vestiario))
            {
                Console.WriteLine("Erro: ID inválido ou vestiário vazio");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE vestiario SET vestiario = @vestiario WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@vestiario", vestiario);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro atualizar vestiário: " + error.Message);
            }
        }

        public static void ExcluirVestiario(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("Erro: ID inválido");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "DELETE FROM vestiario WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro deletar vestiário: " + error.Message);
            }
        }

        // ========== MÉTODOS CRUD PARA ARMÁRIO ==========
        // ✅ CORRIGIDO: DataAdapter agora está dentro de using()
        public static DataTable? GetArmarios()
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM armario";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar armários: " + error.Message);
                return null;
            }
        }

        public static void InserirArmario(int numeroArmario, int vestiarioId)
        {
            if (numeroArmario <= 0 || vestiarioId <= 0)
            {
                Console.WriteLine("Erro: Número do armário e ID do vestiário devem ser maiores que zero");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO armario (numero_armario, vestiario_id) VALUES (@numero_armario, @vestiario_id)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@numero_armario", numeroArmario);
                        cmd.Parameters.AddWithValue("@vestiario_id", vestiarioId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir armário: " + error.Message);
            }
        }

        public static void AtualizarArmario(int id, int numeroArmario, int vestiarioId)
        {
            if (id <= 0 || numeroArmario <= 0 || vestiarioId <= 0)
            {
                Console.WriteLine("Erro: ID, número do armário e ID do vestiário devem ser maiores que zero");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE armario SET numero_armario = @numero_armario, vestiario_id = @vestiario_id WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@numero_armario", numeroArmario);
                        cmd.Parameters.AddWithValue("@vestiario_id", vestiarioId);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro atualizar armário: " + error.Message);
            }
        }

        public static void ExcluirArmario(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("Erro: ID inválido");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "DELETE FROM armario WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro deletar armário: " + error.Message);
            }
        }

        public static int GetVestiarioIdPorNome(string nomeVestiario)
        {
            if (string.IsNullOrWhiteSpace(nomeVestiario))
            {
                Console.WriteLine("Erro: Nome do vestiário não pode estar vazio");
                return -1;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT id FROM vestiario WHERE vestiario = @nome LIMIT 1";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeVestiario);
                        var resultado = cmd.ExecuteScalar();

                        if (resultado == null)
                            return -1;

                        return Convert.ToInt32(resultado);
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro buscar vestiário por nome: " + error.Message);
                return -1;
            }
        }

        // ✅ CORRIGIDO: DataAdapter agora está dentro de using()
        public static DataTable? GetArmariosComStatus(int vestiarioId)
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = @"
                SELECT a.id, a.numero_armario,
                       CASE WHEN f.id IS NULL THEN 0 ELSE 1 END AS ocupado
                FROM armario a
                LEFT JOIN funcionario f ON f.armario_id = a.id
                WHERE a.vestiario_id = @vestiario_id
                ORDER BY a.numero_armario";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@vestiario_id", vestiarioId);
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar armários com status: " + error.Message);
                return null;
            }
        }

        // ========== MÉTODOS CRUD PARA FUNCIONÁRIO ==========
        // ✅ CORRIGIDO: DataAdapter agora está dentro de using()
        public static DataTable? GetFuncionarios()
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM funcionario";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar funcionários: " + error.Message);
                return null;
            }
        }

        // ✅ CORRIGIDO: DataAdapter agora está dentro de using()
        public static DataTable? GetFuncionarioById(int id)
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM funcionario WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar funcionário: " + error.Message);
                return null;
            }
        }

        // ✅ CORRIGIDO: Adicionada validação de entrada
        public static void InserirFuncionario(string nome, string telefone, int armarioId, int matricula, int perfilId)
        {
            if (string.IsNullOrWhiteSpace(nome) || armarioId <= 0 || matricula <= 0 || perfilId <= 0)
            {
                Console.WriteLine("Erro: Dados inválidos ou incompletos");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO funcionario (nome, telefone, armario_id, matricula, perfil_id) VALUES (@nome, @telefone, @armario_id, @matricula, @perfil_id)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@telefone", telefone ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@armario_id", armarioId);
                        cmd.Parameters.AddWithValue("@matricula", matricula);
                        cmd.Parameters.AddWithValue("@perfil_id", perfilId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir funcionário: " + error.Message);
            }
        }

        public static void AtualizarFuncionario(int id, string nome, string telefone, int armarioId, int matricula, int perfilId)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(nome) || armarioId <= 0 || matricula <= 0 || perfilId <= 0)
            {
                Console.WriteLine("Erro: Dados inválidos ou incompletos");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE funcionario SET nome = @nome, telefone = @telefone, armario_id = @armario_id, matricula = @matricula, perfil_id = @perfil_id WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@telefone", telefone ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@armario_id", armarioId);
                        cmd.Parameters.AddWithValue("@matricula", matricula);
                        cmd.Parameters.AddWithValue("@perfil_id", perfilId);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro atualizar funcionário: " + error.Message);
            }
        }

        public static void ExcluirFuncionario(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("Erro: ID inválido");
                return;
            }

            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "DELETE FROM funcionario WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro deletar funcionário: " + error.Message);
            }
        }

    }
}