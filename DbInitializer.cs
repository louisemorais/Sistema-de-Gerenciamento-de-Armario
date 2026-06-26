using System;

namespace SistemaArmario
{
    internal static class DbInitializer
    {
        public static void Inicializar()
        {
            // 1. Garante que o arquivo do banco existe
            DBArmario.CriarDataBase();

            // 2. Cria as tabelas (CREATE IF NOT EXISTS — seguro rodar sempre)
            DBArmario.CriarTabela();

            // 3. Insere vestiários padrão se não existirem
            var vestiarios = DBArmario.GetVestiarios();
            if (vestiarios == null || vestiarios.Rows.Count == 0)
            {
                DBArmario.InserirVestiario("Feminino");
                DBArmario.InserirVestiario("Masculino");
            }

            // 4. Insere perfis padrão se não existirem
            var perfis = DBArmario.GetPerfis();
            if (perfis == null || perfis.Rows.Count == 0)
            {
                DBArmario.CriarPerfisPadrao();
            }

            // 5. Insere armários padrão para cada vestiário se não existirem
            int femininoId = DBArmario.GetVestiarioIdPorNome("Feminino");
            if (femininoId > 0)
            {
                var armarios = DBArmario.GetArmariosComStatus(femininoId);
                if (armarios == null || armarios.Rows.Count == 0)
                    DBArmario.PopularArmariosFixosExemplo(femininoId);
            }

            int masculinoId = DBArmario.GetVestiarioIdPorNome("Masculino");
            if (masculinoId > 0)
            {
                var armarios = DBArmario.GetArmariosComStatus(masculinoId);
                if (armarios == null || armarios.Rows.Count == 0)
                    DBArmario.PopularArmariosFixosExemplo(masculinoId);
            }

            // 6. Popula funcionários de teste se o banco estiver vazio
            DBArmario.PopularDadosTeste();
        }
    }
}