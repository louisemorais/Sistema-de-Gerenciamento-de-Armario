using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SistemaArmario
{
    public partial class TelaCadastro : Form
    {
        private int armarioSelecionadoId = -1;
        private Button? botaoSelecionado;  // ✅ Nullable explícito

        public TelaCadastro()
        {
            InitializeComponent();
        }

        private void TelaCadastro_Load(object sender, EventArgs e)
        {
            
        }

        private void BackgroundArmario_Paint(object sender, PaintEventArgs e)
        {

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

                Button btnArmario = new Button();
                btnArmario.Text = numero.ToString();
                btnArmario.Size = new Size(50, 50);
                btnArmario.Tag = armarioId;
                btnArmario.FlatStyle = FlatStyle.Flat;
                btnArmario.Font = new Font(btnArmario.Font, FontStyle.Bold);

                if (ocupado)
                {
                    btnArmario.BackColor = Color.Red;
                    btnArmario.ForeColor = Color.White;
                    btnArmario.Enabled = false;
                }
                else
                {
                    btnArmario.BackColor = Color.LightGreen;
                    btnArmario.ForeColor = Color.Black;
                    btnArmario.Click += BtnArmario_Click;
                }

                BackgroundArmario.Controls.Add(btnArmario);
            }
        }

        private void BtnArmario_Click(object sender, EventArgs e)
        {
            if (sender is not Button btnClicado)
            {
                MessageBox.Show("Erro ao selecionar armário.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (botaoSelecionado != null)
            {
                botaoSelecionado.BackColor = Color.LightGreen;
            }

            btnClicado.BackColor = Color.DodgerBlue;
            btnClicado.ForeColor = Color.White;

            botaoSelecionado = btnClicado;
            if (btnClicado.Tag is int tagId)
            {
                armarioSelecionadoId = tagId;
            }
        }

        private void CampoNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void CampoNumMatricula_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CampoNome.Text))
            {
                MessageBox.Show("Informe o nome.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(CampoNumMatricula.Text, out int matricula))
            {
                MessageBox.Show("Matrícula inválida.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!RadioBtnFeminino.Checked && !radioBtnMasculino.Checked)
            {
                MessageBox.Show("Selecione Feminino ou Masculino.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (armarioSelecionadoId == -1)
            {
                MessageBox.Show("Selecione um armário disponível.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int perfilId = 1; // perfil padrão "Funcionario"

            DBArmario.InserirFuncionario(
                nome: CampoNome.Text,
                telefone: null,
                armarioId: armarioSelecionadoId,
                matricula: matricula,
                perfilId: perfilId
            );

            MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CampoNome.Clear();
            CampoNumMatricula.Clear();
            RadioBtnFeminino.Checked = false;
            radioBtnMasculino.Checked = false;
            BackgroundArmario.Controls.Clear();
            armarioSelecionadoId = -1;
            botaoSelecionado = null;
        }
    }
}