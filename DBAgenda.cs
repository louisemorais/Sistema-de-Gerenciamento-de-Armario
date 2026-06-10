using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.IO;

namespace Agenda
{
    internal class DBAgenda
    {
        public static string path = Directory.GetCurrentDirectory() + "\\setor_armarios.sqlite";
        private static SQLiteConnection connection;

        private static SQLiteConnection DataBaseconnection()
        {
            connection = new SQLiteConnection("Data Source=" + path);
            connection.Open();

            // Habilita enforcement de foreign keys no SQLite
            using (var cmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
            {
                cmd.ExecuteNonQuery();
            }
            return connection;
        }

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
                Console.WriteLine("Erro ao criar o banco de dados: " + error.Message);
            }
        }

        public static void CriarTabela()
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = @"
                        CREATE TABLE IF NOT EXISTS perfil (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            permissao TEXT NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS setor (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            nome_setor TEXT NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS vestiario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            vestiario TEXT NOT NULL
                        );

                        CREATE TABLE IF NOT EXISTS armario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            numero_armario INTEGER NOT NULL,
                            id_vestiario INTEGER NULL,
                            perfil_id INTEGER NOT NULL,
                            FOREIGN KEY (id_vestiario) REFERENCES vestiario(id),
                            FOREIGN KEY (perfil_id) REFERENCES perfil(id)
                        );

                        CREATE INDEX IF NOT EXISTS idx_armario_id_vestiario ON armario (id_vestiario);
                        CREATE INDEX IF NOT EXISTS idx_armario_perfil_id ON armario (perfil_id);

                        CREATE TABLE IF NOT EXISTS funcionario (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            nome TEXT NOT NULL,
                            matricula INTEGER NOT NULL,
                            telefone INTEGER NULL,
                            id_perfil INTEGER NULL,
                            armario_id INTEGER NOT NULL,
                            vestiario_id INTEGER NOT NULL,
                            setor_id INTEGER NOT NULL,
                            FOREIGN KEY (armario_id) REFERENCES armario(id),
                            FOREIGN KEY (vestiario_id) REFERENCES vestiario(id),
                            FOREIGN KEY (setor_id) REFERENCES setor(id),
                            FOREIGN KEY (id_perfil) REFERENCES perfil(id)
                        );

                        CREATE INDEX IF NOT EXISTS idx_funcionario_id_perfil ON funcionario (id_perfil);
                        CREATE INDEX IF NOT EXISTS idx_funcionario_armario_id ON funcionario (armario_id);
                        CREATE INDEX IF NOT EXISTS idx_funcionario_vestiario_id ON funcionario (vestiario_id);
                        CREATE INDEX IF NOT EXISTS idx_funcionario_setor_id ON funcionario (setor_id);

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

        public static DataTable GetContatos()
        {
            SQLiteDataAdapter adapter = null;
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM Contatos";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar dados: " + error.Message);
                return null;
            }
        }

        public static DataTable GetContatoById(int id)
        {
            SQLiteDataAdapter adapter = null;
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM Contatos WHERE Id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar dado: " + error.Message);
                return null;
            }
        }

        public static void InserirContato(Contato contato)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO Contatos (Nome, Telefone) VALUES (@nome, @telefone)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", contato.Nome ?? string.Empty);
                        cmd.Parameters.AddWithValue("@telefone", contato.Telefone ?? string.Empty);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir dado: " + error.Message);
            }
        }

        public static void AlterarContato(Contato contato)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE Contatos SET Nome = @nome, Telefone = @telefone WHERE Id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", contato.Nome ?? string.Empty);
                        cmd.Parameters.AddWithValue("@telefone", contato.Telefone ?? string.Empty);
                        cmd.Parameters.AddWithValue("@id", contato.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro alterar dado: " + error.Message);
            }
        }

        public static void ExcluirContato(int id)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "DELETE FROM Contatos WHERE Id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro deletar dado: " + error.Message);
            }
        }

        // ========== MÉTODOS CRUD PARA PERFIL ==========
        public static DataTable GetPerfis()
        {
            SQLiteDataAdapter adapter = null;
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM perfil";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar perfis: " + error.Message);
                return null;
            }
        }

        public static void InserirPerfil(string permissao)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO perfil (permissao) VALUES (@permissao)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@permissao", permissao ?? string.Empty);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir perfil: " + error.Message);
            }
        }

        public static void AtualizarPerfil(int id, string permissao)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE perfil SET permissao = @permissao WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@permissao", permissao ?? string.Empty);
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

        // ========== MÉTODOS CRUD PARA SETOR ==========
        public static DataTable GetSetores()
        {
            SQLiteDataAdapter adapter = null;
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM setor";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar setores: " + error.Message);
                return null;
            }
        }

        public static void InserirSetor(string nomeSetor)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO setor (nome_setor) VALUES (@nome_setor)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome_setor", nomeSetor ?? string.Empty);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir setor: " + error.Message);
            }
        }

        public static void AtualizarSetor(int id, string nomeSetor)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE setor SET nome_setor = @nome_setor WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome_setor", nomeSetor ?? string.Empty);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro atualizar setor: " + error.Message);
            }
        }

        public static void ExcluirSetor(int id)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "DELETE FROM setor WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro deletar setor: " + error.Message);
            }
        }

        // ========== MÉTODOS CRUD PARA VESTIÁRIO ==========
        public static DataTable GetVestiarios()
        {
            SQLiteDataAdapter adapter = null;
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM vestiario";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar vestiários: " + error.Message);
                return null;
            }
        }

        public static void InserirVestiario(string vestiario)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO vestiario (vestiario) VALUES (@vestiario)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@vestiario", vestiario ?? string.Empty);
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
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE vestiario SET vestiario = @vestiario WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@vestiario", vestiario ?? string.Empty);
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
        public static DataTable GetArmarios()
        {
            SQLiteDataAdapter adapter = null;
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM armario";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar armários: " + error.Message);
                return null;
            }
        }

        public static void InserirArmario(int numeroArmario, int? idVestiario, int perfilId)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO armario (numero_armario, id_vestiario, perfil_id) VALUES (@numero_armario, @id_vestiario, @perfil_id)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@numero_armario", numeroArmario);
                        cmd.Parameters.AddWithValue("@id_vestiario", idVestiario ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@perfil_id", perfilId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir armário: " + error.Message);
            }
        }

        public static void AtualizarArmario(int id, int numeroArmario, int? idVestiario, int perfilId)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE armario SET numero_armario = @numero_armario, id_vestiario = @id_vestiario, perfil_id = @perfil_id WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@numero_armario", numeroArmario);
                        cmd.Parameters.AddWithValue("@id_vestiario", idVestiario ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@perfil_id", perfilId);
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

        // ========== MÉTODOS CRUD PARA FUNCIONÁRIO ==========
        public static DataTable GetFuncionarios()
        {
            SQLiteDataAdapter adapter = null;
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM funcionario";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar funcionários: " + error.Message);
                return null;
            }
        }

        public static DataTable GetFuncionarioById(int id)
        {
            SQLiteDataAdapter adapter = null;
            DataTable table = new DataTable();
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "SELECT * FROM funcionario WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        adapter = new SQLiteDataAdapter(cmd);
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro selecionar funcionário: " + error.Message);
                return null;
            }
        }

        public static void InserirFuncionario(string nome, int matricula, int? telefone, int? idPerfil, int armarioId, int vestiarioId, int setorId)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "INSERT INTO funcionario (nome, matricula, telefone, id_perfil, armario_id, vestiario_id, setor_id) VALUES (@nome, @matricula, @telefone, @id_perfil, @armario_id, @vestiario_id, @setor_id)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome ?? string.Empty);
                        cmd.Parameters.AddWithValue("@matricula", matricula);
                        cmd.Parameters.AddWithValue("@telefone", telefone ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@id_perfil", idPerfil ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@armario_id", armarioId);
                        cmd.Parameters.AddWithValue("@vestiario_id", vestiarioId);
                        cmd.Parameters.AddWithValue("@setor_id", setorId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Erro inserir funcionário: " + error.Message);
            }
        }

        public static void AtualizarFuncionario(int id, string nome, int matricula, int? telefone, int? idPerfil, int armarioId, int vestiarioId, int setorId)
        {
            try
            {
                using (var conn = DataBaseconnection())
                {
                    string sql = "UPDATE funcionario SET nome = @nome, matricula = @matricula, telefone = @telefone, id_perfil = @id_perfil, armario_id = @armario_id, vestiario_id = @vestiario_id, setor_id = @setor_id WHERE id = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome ?? string.Empty);
                        cmd.Parameters.AddWithValue("@matricula", matricula);
                        cmd.Parameters.AddWithValue("@telefone", telefone ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@id_perfil", idPerfil ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@armario_id", armarioId);
                        cmd.Parameters.AddWithValue("@vestiario_id", vestiarioId);
                        cmd.Parameters.AddWithValue("@setor_id", setorId);
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
