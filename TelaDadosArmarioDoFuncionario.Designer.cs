namespace SistemaArmario
{
    partial class TelaDadosArmarioDoFuncionario
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelTitulo = new System.Windows.Forms.Label();
            labelNome = new System.Windows.Forms.Label();
            labelMatricula = new System.Windows.Forms.Label();
            labelVestiario = new System.Windows.Forms.Label();
            labelNumArmario = new System.Windows.Forms.Label();
            panelCard = new System.Windows.Forms.Panel();

            panelCard.SuspendLayout();
            SuspendLayout();

            // panelCard — card centralizado
            panelCard.BackColor = System.Drawing.Color.White;
            panelCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelCard.Location = new System.Drawing.Point(200, 100);
            panelCard.Name = "panelCard";
            panelCard.Size = new System.Drawing.Size(400, 250);
            panelCard.TabIndex = 0;

            // labelTitulo
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            labelTitulo.ForeColor = System.Drawing.Color.FromArgb(30, 30, 80);
            labelTitulo.Location = new System.Drawing.Point(20, 20);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Text = "Meu Armário";

            // labelNome
            labelNome.AutoSize = true;
            labelNome.Font = new System.Drawing.Font("Segoe UI", 11F);
            labelNome.Location = new System.Drawing.Point(20, 70);
            labelNome.Name = "labelNome";
            labelNome.Text = "Nome: ...";

            // labelMatricula
            labelMatricula.AutoSize = true;
            labelMatricula.Font = new System.Drawing.Font("Segoe UI", 11F);
            labelMatricula.Location = new System.Drawing.Point(20, 105);
            labelMatricula.Name = "labelMatricula";
            labelMatricula.Text = "Matrícula: ...";

            // labelVestiario
            labelVestiario.AutoSize = true;
            labelVestiario.Font = new System.Drawing.Font("Segoe UI", 11F);
            labelVestiario.Location = new System.Drawing.Point(20, 140);
            labelVestiario.Name = "labelVestiario";
            labelVestiario.Text = "Vestiário: ...";

            // labelNumArmario
            labelNumArmario.AutoSize = true;
            labelNumArmario.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            labelNumArmario.ForeColor = System.Drawing.Color.FromArgb(30, 100, 30);
            labelNumArmario.Location = new System.Drawing.Point(20, 180);
            labelNumArmario.Name = "labelNumArmario";
            labelNumArmario.Text = "Armário nº ...";

            panelCard.Controls.Add(labelTitulo);
            panelCard.Controls.Add(labelNome);
            panelCard.Controls.Add(labelMatricula);
            panelCard.Controls.Add(labelVestiario);
            panelCard.Controls.Add(labelNumArmario);

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(panelCard);
            Name = "TelaDadosArmarioDoFuncionario";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Meu Armário";
            Load += TelaDadosArmarioDoFuncionario_Load;

            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelNome;
        private System.Windows.Forms.Label labelMatricula;
        private System.Windows.Forms.Label labelVestiario;
        private System.Windows.Forms.Label labelNumArmario;
    }
}