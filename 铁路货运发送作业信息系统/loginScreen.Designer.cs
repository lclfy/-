namespace 铁路货运发送作业信息系统
{
    partial class loginScreen
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.registerButton = new System.Windows.Forms.Button();
            this.forgetPasswordButton = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.Label();
            this.userNameText = new System.Windows.Forms.TextBox();
            this.passwordText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.staffRegisterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(317, 248);
            this.registerButton.Margin = new System.Windows.Forms.Padding(2);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(97, 38);
            this.registerButton.TabIndex = 0;
            this.registerButton.Text = "注册";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // forgetPasswordButton
            // 
            this.forgetPasswordButton.Location = new System.Drawing.Point(461, 347);
            this.forgetPasswordButton.Margin = new System.Windows.Forms.Padding(2);
            this.forgetPasswordButton.Name = "forgetPasswordButton";
            this.forgetPasswordButton.Size = new System.Drawing.Size(97, 38);
            this.forgetPasswordButton.TabIndex = 0;
            this.forgetPasswordButton.Text = "忘记密码";
            this.forgetPasswordButton.UseVisualStyleBackColor = true;
            this.forgetPasswordButton.Click += new System.EventHandler(this.forgetPasswordButton_Click);
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.name.Location = new System.Drawing.Point(151, 133);
            this.name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(76, 19);
            this.name.TabIndex = 1;
            this.name.Text = "用户名:";
            this.name.Click += new System.EventHandler(this.label1_Click);
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.password.Location = new System.Drawing.Point(170, 191);
            this.password.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(57, 19);
            this.password.TabIndex = 2;
            this.password.Text = "密码:";
            // 
            // userNameText
            // 
            this.userNameText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userNameText.Location = new System.Drawing.Point(240, 130);
            this.userNameText.Margin = new System.Windows.Forms.Padding(2);
            this.userNameText.Name = "userNameText";
            this.userNameText.Size = new System.Drawing.Size(174, 29);
            this.userNameText.TabIndex = 3;
            // 
            // passwordText
            // 
            this.passwordText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.passwordText.Location = new System.Drawing.Point(240, 188);
            this.passwordText.Margin = new System.Windows.Forms.Padding(2);
            this.passwordText.Name = "passwordText";
            this.passwordText.PasswordChar = '*';
            this.passwordText.Size = new System.Drawing.Size(174, 29);
            this.passwordText.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(105, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "铁路货物发送作业信息系统";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(174, 248);
            this.loginButton.Margin = new System.Windows.Forms.Padding(2);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(97, 38);
            this.loginButton.TabIndex = 6;
            this.loginButton.Text = "登录";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // staffRegisterButton
            // 
            this.staffRegisterButton.Location = new System.Drawing.Point(25, 347);
            this.staffRegisterButton.Margin = new System.Windows.Forms.Padding(2);
            this.staffRegisterButton.Name = "staffRegisterButton";
            this.staffRegisterButton.Size = new System.Drawing.Size(97, 38);
            this.staffRegisterButton.TabIndex = 7;
            this.staffRegisterButton.Text = "职工注册";
            this.staffRegisterButton.UseVisualStyleBackColor = true;
            this.staffRegisterButton.Click += new System.EventHandler(this.staffRegisterButton_Click);
            // 
            // loginScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 402);
            this.Controls.Add(this.staffRegisterButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.passwordText);
            this.Controls.Add(this.userNameText);
            this.Controls.Add(this.password);
            this.Controls.Add(this.name);
            this.Controls.Add(this.forgetPasswordButton);
            this.Controls.Add(this.registerButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "loginScreen";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Button forgetPasswordButton;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.TextBox userNameText;
        private System.Windows.Forms.TextBox passwordText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button staffRegisterButton;
    }
}

