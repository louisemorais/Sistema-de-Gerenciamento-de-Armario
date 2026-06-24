using System;
using System.Collections.Generic;

namespace SistemaArmario
{
    /// <summary>
    /// Inicializa o banco de dados com estrutura e dados padrão
    /// </summary>
    internal static class DbInitializer
    {
        public static void Inicializar()
        {
            try
            {
                // 1. Criar o arquivo do banco de dados
                DBArmario.CriarDataBase();

                // 2. Criar as tabelas
                DBArmario.CriarTabela();

                // 3. Inserir dados padrão (vestiários e perfis)
                InserirDadosPadroes();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inicializar banco de dados: {ex.Message}");
                throw;
            }
        }

        private static void InserirDadosPadroes()
        {
            // Inserir vestiários padrão se não existirem
            var vestiarios = DBArmario.GetVestiarios();
            if (vestiarios == null || vestiarios.Rows.Count == 0)
            {
                DBArmario.InserirVestiario("Feminino");
                DBArmario.InserirVestiario("Masculino");
            }

            // Inserir perfis padrão se não existirem
            var perfis = DBArmario.GetPerfis();
            if (perfis == null || perfis.Rows.Count == 0)
            {
                DBArmario.InserirPerfil("Acesso básico", "Funcionario");
                DBArmario.InserirPerfil("Acesso administrativo", "Administrador");
            }

            // Inserir armários padrão para cada vestiário se não existirem
            var vestiariosFeminino = DBArmario.GetVestiarios();
            if (vestiariosFeminino != null && vestiariosFeminino.Rows.Count > 0)
            {
                // Vestiário Feminino
                int vestiarioFemininoId = DBArmario.GetVestiarioIdPorNome("Feminino");
                if (vestiarioFemininoId > 0)
                {
                    var armariosFeminino = DBArmario.GetArmariosComStatus(vestiarioFemininoId);
                    if (armariosFeminino == null || armariosFeminino.Rows.Count == 0)
                    {
                        DBArmario.PopularArmariosFixosExemplo(vestiarioFemininoId);
                    }
                }

                // Vestiário Masculino
                int vestiarioMasculinoId = DBArmario.GetVestiarioIdPorNome("Masculino");
                if (vestiarioMasculinoId > 0)
                {
                    var armariosMasculino = DBArmario.GetArmariosComStatus(vestiarioMasculinoId);
                    if (armariosMasculino == null || armariosMasculino.Rows.Count == 0)
                    {
                        DBArmario.PopularArmariosFixosExemplo(vestiarioMasculinoId);
                    }
                }
            }
        }
    }
}
