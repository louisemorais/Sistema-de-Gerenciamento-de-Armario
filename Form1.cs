using System;
using System.Data;
using System.Windows.Forms;

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
            AbrirTelaCadastro();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clique detectado!");
            Login();
        }

        private void CampoLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // ==========================
        // MÉTODOS DE NEGÓCIO
        // ==========================

        private void Login()
        {
            if (!ValidarMatricula(out int matricula))
                return;

            DataTable? usuario =
                DBArmario.LoginPorMatricula(matricula);

            MessageBox.Show(
                usuario == null
                    ? "NULL"
                    : usuario.Rows.Count.ToString()
            );

            if (!UsuarioExiste(usuario))
                return;

            AbrirTelaPorPerfil(usuario);
        }

        private bool ValidarMatricula(out int matricula)
        {
            matricula = 0;

            if (string.IsNullOrWhiteSpace(CampoLogin.Text))
            {
                MessageBox.Show(
                    "Digite sua matrícula.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            if (!int.TryParse(CampoLogin.Text, out matricula))
            {
                MessageBox.Show(
                    "Matrícula deve conter apenas números.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private bool UsuarioExiste(DataTable? usuario)
        {
            if (usuario == null || usuario.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Matrícula não encontrada.",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void AbrirTelaPorPerfil(DataTable usuario)
        {
            string permissao =
                usuario.Rows[0]["permissao"].ToString();

            int idFuncionario =
                Convert.ToInt32(usuario.Rows[0]["id"]);

            Form tela;

            if (permissao.Trim().ToUpper() == "ADMIN")
            {
                tela = new TelaAdmin();
            }
            else
            {
                tela = new TelaDadosArmarioDoFuncionario(idFuncionario);
            }

            tela.Show();
            this.Hide();
        }

        private void AbrirTelaCadastro()
        {
            TelaCadastro cadastro = new TelaCadastro();
            cadastro.Show();
            this.Hide();
        }

        // Handler ligado ao botão 'inserir' e ao Load (conforme Designer): cria DB e tabela e garante um usuário padrão.
        private void botaoinfo_Click(object sender, EventArgs e)
        {
            DBArmario.CriarDataBase();
            DBArmario.CriarTabela();

            //DBArmario.PopularDadosTeste();

            MessageBox.Show("Dados de teste criados!");
        }

    }
}