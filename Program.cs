namespace SistemaArmario
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                DbInitializer.Inicializar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao inicializar o banco de dados:\n\n{ex.Message}",
                    "Erro Crítico",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.Run(new Form1());
        }
    }
}