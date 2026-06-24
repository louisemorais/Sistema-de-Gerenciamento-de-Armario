namespace SistemaArmario
{
    partial class TelaCadastro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelNome = new Label();
            CampoNome = new TextBox();
            labelMatricula = new Label();
            CampoNumMatricula = new TextBox();
            RadioBtnFeminino = new RadioButton();
            radioBtnMasculino = new RadioButton();
            BackgroundArmario = new FlowLayoutPanel();
            TituloCadastro = new Label();
            BtnEnviar = new Button();
            SuspendLayout();
            // 
            // labelNome
            // 
            labelNome.AutoSize = true;
            labelNome.Location = new Point(62, 65);
            labelNome.Name = "labelNome";
            labelNome.Size = new Size(50, 20);
            labelNome.TabIndex = 0;
            labelNome.Text = "Nome";
            // 
            // CampoNome
            // 
            CampoNome.Location = new Point(62, 88);
            CampoNome.Name = "CampoNome";
            CampoNome.Size = new Size(125, 27);
            CampoNome.TabIndex = 1;
            CampoNome.TextChanged += CampoNome_TextChanged;
            // 
            // labelMatricula
            // 
            labelMatricula.AutoSize = true;
            labelMatricula.Location = new Point(427, 65);
            labelMatricula.Name = "labelMatricula";
            labelMatricula.Size = new Size(71, 20);
            labelMatricula.TabIndex = 2;
            labelMatricula.Text = "Matrícula";
            // 
            // CampoNumMatricula
            // 
            CampoNumMatricula.Location = new Point(427, 98);
            CampoNumMatricula.Name = "CampoNumMatricula";
            CampoNumMatricula.Size = new Size(125, 27);
            CampoNumMatricula.TabIndex = 3;
            CampoNumMatricula.TextChanged += CampoNumMatricula_TextChanged;
            // 
            // RadioBtnFeminino
            // 
            RadioBtnFeminino.AutoSize = true;
            RadioBtnFeminino.Location = new Point(62, 135);
            RadioBtnFeminino.Name = "RadioBtnFeminino";
            RadioBtnFeminino.Size = new Size(91, 24);
            RadioBtnFeminino.TabIndex = 4;
            RadioBtnFeminino.TabStop = true;
            RadioBtnFeminino.Text = "Feminino";
            RadioBtnFeminino.UseVisualStyleBackColor = true;
            RadioBtnFeminino.CheckedChanged += RadioBtnFeminino_CheckedChanged;
            // 
            // radioBtnMasculino
            // 
            radioBtnMasculino.AutoSize = true;
            radioBtnMasculino.Location = new Point(193, 136);
            radioBtnMasculino.Name = "radioBtnMasculino";
            radioBtnMasculino.Size = new Size(97, 24);
            radioBtnMasculino.TabIndex = 5;
            radioBtnMasculino.TabStop = true;
            radioBtnMasculino.Text = "Masculino";
            radioBtnMasculino.UseVisualStyleBackColor = true;
            radioBtnMasculino.CheckedChanged += radioBtnMasculino_CheckedChanged;
            // 
            // BackgroundArmario
            // 
            BackgroundArmario.BackColor = SystemColors.ControlDarkDark;
            BackgroundArmario.Location = new Point(62, 189);
            BackgroundArmario.Name = "BackgroundArmario";
            BackgroundArmario.Size = new Size(599, 207);
            BackgroundArmario.TabIndex = 6;
            BackgroundArmario.Paint += BackgroundArmario_Paint;
            // 
            // TituloCadastro
            // 
            TituloCadastro.AutoSize = true;
            TituloCadastro.Location = new Point(312, 9);
            TituloCadastro.Name = "TituloCadastro";
            TituloCadastro.Size = new Size(147, 20);
            TituloCadastro.TabIndex = 7;
            TituloCadastro.Text = "Cadastro de Armario";
            // 
            // BtnEnviar
            // 
            BtnEnviar.Location = new Point(588, 409);
            BtnEnviar.Name = "BtnEnviar";
            BtnEnviar.Size = new Size(94, 29);
            BtnEnviar.TabIndex = 8;
            BtnEnviar.Text = "Enviar";
            BtnEnviar.UseVisualStyleBackColor = true;
            BtnEnviar.Click += BtnEnviar_Click;
            // 
            // TelaCadastro
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BtnEnviar);
            Controls.Add(TituloCadastro);
            Controls.Add(BackgroundArmario);
            Controls.Add(radioBtnMasculino);
            Controls.Add(RadioBtnFeminino);
            Controls.Add(CampoNumMatricula);
            Controls.Add(labelMatricula);
            Controls.Add(CampoNome);
            Controls.Add(labelNome);
            Name = "TelaCadastro";
            Text = "TelaCadastro";
            Load += TelaCadastro_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelNome;
        private TextBox CampoNome;
        private Label labelMatricula;
        private TextBox CampoNumMatricula;
        private RadioButton RadioBtnFeminino;
        private RadioButton radioBtnMasculino;
        private FlowLayoutPanel BackgroundArmario;
        private Label TituloCadastro;
        private Button BtnEnviar;
    }
}