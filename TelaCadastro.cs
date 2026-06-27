using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaArmario
{
    public partial class TelaCadastro : Form
    {
        private int armarioSelecionadoId = -1;
        private Button? botaoSelecionado;

        public TelaCadastro()
        {
            InitializeComponent();
        }

        private void TelaCadastro_Load(object sender, EventArgs e)
        {
            // Nada por enquanto — usuário deve escolher o vestiário primeiro
        }

        private void RadioBtnFeminino_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioBtnFeminino.Checked)
            {
                int vestiarioId = DBArmario.GetVestiarioIdPorNome("Feminino");
                CarregarArmarios(vestiarioId);
            }
        }

        private void radioBtnMasculino_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnMasculino.Checked)
            {
                int vestiarioId = DBArmario.GetVestiarioIdPorNome("Masculino");
                CarregarArmarios(vestiarioId);
            }
        }

        private void CarregarArmarios(int vestiarioId)
        {
            armarioSelecionadoId = -1;
            botaoSelecionado = null;
            BackgroundArmario.Controls.Clear();

            DataTable? armarios = DBArmario.GetArmariosComStatus(vestiarioId);

            if (armarios == null || armarios.Rows.Count == 0)
            {
                MessageBox.Show("Nenhum armário disponível para este vestiário.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataRow linha in armarios.Rows)
            {
                int armarioId = Convert.ToInt32(linha["id"]);
                int numero = Convert.ToInt32(linha["numero_armario"]);
                bool ocupado = Convert.ToInt32(linha["ocupado"]) > 0;

                var btn = new Button
                {
                    Text = numero.ToString(),
                    Size = new Size(55, 55),
                    Tag = armarioId,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font(Font, FontStyle.Bold)
                };

                if (ocupado)
                {
                    btn.BackColor = Color.IndianRed;
                    btn.ForeColor = Color.White;
                    btn.Enabled = false;
                }
                else
                {
                    btn.BackColor = Color.MediumSeaGreen;
                    btn.ForeColor = Color.White;
                    btn.Click += BtnArmario_Click;
                }

                BackgroundArmario.Controls.Add(btn);
            }
        }

        private void BtnArmario_Click(object sender, EventArgs e)
        {
            if (sender is not Button btnClicado) return;

            // Desmarca seleção anterior
            if (botaoSelecionado != null)
                botaoSelecionado.BackColor = Color.MediumSeaGreen;

            // Marca novo selecionado
            btnClicado.BackColor = Color.DodgerBlue;
            botaoSelecionado = btnClicado;
            armarioSelecionadoId = btnClicado.Tag is int id ? id : -1;
        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(CampoNome.Text))
            {
                MessageBox.Show("Informe o nome.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(CampoNumMatricula.Text, out int matricula) || matricula <= 0)
            {
                MessageBox.Show("Matrícula inválida. Use apenas números.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!RadioBtnFeminino.Checked && !radioBtnMasculino.Checked)
            {
                MessageBox.Show("Selecione Feminino ou Masculino.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (armarioSelecionadoId == -1)
            {
                MessageBox.Show("Selecione um armário disponível (verde).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Perfil USER = id 2 (ADMIN = 1, conforme CriarPerfisPadrao)
            int perfilId = 2;

            try
            {
                DBArmario.InserirFuncionario(
                    nome: CampoNome.Text.Trim(),
                    telefone: null,
                    armarioId: armarioSelecionadoId,
                    matricula: matricula,
                    perfilId: perfilId
                );
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE"))
                    MessageBox.Show("Esta matrícula já está cadastrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Erro ao cadastrar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(
                $"Cadastro realizado!\n\nUse a matrícula {matricula} para fazer login.",
                "Sucesso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Busca o id do funcionário recém-cadastrado para redirecionar
            DataTable? usuario = DBArmario.LoginPorMatricula(matricula);
            if (usuario != null && usuario.Rows.Count > 0)
            {
                int idFuncionario = Convert.ToInt32(usuario.Rows[0]["id"]);
                var tela = new TelaDadosArmarioDoFuncionario(idFuncionario);
                tela.Show();
                tela.FormClosed += (s, ev) => this.Close();
                this.Hide();
            }
            else
            {
                this.Close();
            }
        }

        private void BackgroundArmario_Paint(object sender, PaintEventArgs e) { }
        private void CampoNome_TextChanged(object sender, EventArgs e) { }
        private void CampoNumMatricula_TextChanged(object sender, EventArgs e) { }
    }
}