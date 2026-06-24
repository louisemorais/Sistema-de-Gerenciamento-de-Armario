namespace SistemaArmario
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // ✅ Inicializar banco de dados antes de abrir a aplicação
            try
            {
                DbInitializer.Inicializar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inicializar aplicação: {ex.Message}", "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new Form1());
        }
    }
}