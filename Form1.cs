namespace SistemaArmario
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkTelaCadastro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TelaCadastro cadastro = new TelaCadastro();
            cadastro.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CampoLogin.Text))
            {
                MessageBox.Show("Digite sua matrícula.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(CampoLogin.Text, out int matricula))
            {
                MessageBox.Show("Matrícula deve conter apenas números.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void CampoLogin_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
