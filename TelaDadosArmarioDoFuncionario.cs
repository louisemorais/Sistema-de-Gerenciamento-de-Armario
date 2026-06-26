using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaArmario
{
    public partial class TelaDadosArmarioDoFuncionario : Form
    {
        private readonly int _idFuncionario;

        public TelaDadosArmarioDoFuncionario(int idFuncionario)
        {
            InitializeComponent();
            _idFuncionario = idFuncionario;
        }

        private void TelaDadosArmarioDoFuncionario_Load(object sender, EventArgs e)
        {
            CarregarDados();
        }

        private void CarregarDados()
        {
            DataTable? dados = DBArmario.GetFuncionarioById(_idFuncionario);

            if (dados == null || dados.Rows.Count == 0)
            {
                MessageBox.Show("Funcionário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            DataRow row = dados.Rows[0];

            labelNome.Text = "Nome: " + row["nome"].ToString();
            labelMatricula.Text = "Matrícula: " + row["matricula"].ToString();
            labelVestiario.Text = "Vestiário: " + row["vestiario"].ToString();
            labelNumArmario.Text = "Armário nº " + row["numero_armario"].ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}