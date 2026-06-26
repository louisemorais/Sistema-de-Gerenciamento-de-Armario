namespace SistemaArmario
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnLogin = new Button();
            labelTituloLogin = new Label();
            linkTelaCadastro = new LinkLabel();
            CampoLogin = new TextBox();
            labelMatriculaLogin = new Label();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(345, 232);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(94, 29);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "Entrar";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // labelTituloLogin
            // 
            labelTituloLogin.AutoSize = true;
            labelTituloLogin.Location = new Point(368, 94);
            labelTituloLogin.Name = "labelTituloLogin";
            labelTituloLogin.Size = new Size(46, 20);
            labelTituloLogin.TabIndex = 1;
            labelTituloLogin.Text = "Login";
            // 
            // linkTelaCadastro
            // 
            linkTelaCadastro.AutoSize = true;
            linkTelaCadastro.Location = new Point(307, 287);
            linkTelaCadastro.Name = "linkTelaCadastro";
            linkTelaCadastro.Size = new Size(184, 20);
            linkTelaCadastro.TabIndex = 2;
            linkTelaCadastro.TabStop = true;
            linkTelaCadastro.Text = "Cadastre seu Armário aqui";
            linkTelaCadastro.LinkClicked += linkTelaCadastro_LinkClicked;
            // 
            // CampoLogin
            // 
            CampoLogin.Location = new Point(333, 181);
            CampoLogin.Name = "CampoLogin";
            CampoLogin.Size = new Size(125, 27);
            CampoLogin.TabIndex = 3;
            CampoLogin.TextChanged += CampoLogin_TextChanged;
            // 
            // labelMatriculaLogin
            // 
            labelMatriculaLogin.AutoSize = true;
            labelMatriculaLogin.Location = new Point(357, 146);
            labelMatriculaLogin.Name = "labelMatriculaLogin";
            labelMatriculaLogin.Size = new Size(71, 20);
            labelMatriculaLogin.TabIndex = 4;
            labelMatriculaLogin.Text = "Matrícula";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelMatriculaLogin);
            Controls.Add(CampoLogin);
            Controls.Add(linkTelaCadastro);
            Controls.Add(labelTituloLogin);
            Controls.Add(btnLogin);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLogin;
        private Label labelTituloLogin;
        private LinkLabel linkTelaCadastro;
        private TextBox CampoLogin;
        private Label labelMatriculaLogin;
    }
}
