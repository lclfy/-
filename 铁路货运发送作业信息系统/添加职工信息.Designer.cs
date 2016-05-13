namespace 铁路货运发送作业信息系统
{
    partial class 添加职工信息
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
            this.passwordText = new System.Windows.Forms.TextBox();
            this.userNameText = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.Label();
            this.deleteStaffButton = new System.Windows.Forms.Button();
            this.addStaffButton = new System.Windows.Forms.Button();
            this.StaffListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // passwordText
            // 
            this.passwordText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.passwordText.Location = new System.Drawing.Point(238, 432);
            this.passwordText.Margin = new System.Windows.Forms.Padding(2);
            this.passwordText.Name = "passwordText";
            this.passwordText.PasswordChar = '*';
            this.passwordText.Size = new System.Drawing.Size(128, 23);
            this.passwordText.TabIndex = 43;
            // 
            // userNameText
            // 
            this.userNameText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userNameText.Location = new System.Drawing.Point(91, 432);
            this.userNameText.Margin = new System.Windows.Forms.Padding(2);
            this.userNameText.Name = "userNameText";
            this.userNameText.Size = new System.Drawing.Size(128, 23);
            this.userNameText.TabIndex = 42;
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.password.Location = new System.Drawing.Point(278, 416);
            this.password.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(35, 14);
            this.password.TabIndex = 41;
            this.password.Text = "密码";
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.name.Location = new System.Drawing.Point(141, 416);
            this.name.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(35, 14);
            this.name.TabIndex = 40;
            this.name.Text = "名称";
            // 
            // deleteStaffButton
            // 
            this.deleteStaffButton.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.deleteStaffButton.Location = new System.Drawing.Point(238, 473);
            this.deleteStaffButton.Name = "deleteStaffButton";
            this.deleteStaffButton.Size = new System.Drawing.Size(75, 36);
            this.deleteStaffButton.TabIndex = 85;
            this.deleteStaffButton.Text = "删除";
            this.deleteStaffButton.UseVisualStyleBackColor = true;
            this.deleteStaffButton.Click += new System.EventHandler(this.deleteStaffButton_Click);
            // 
            // addStaffButton
            // 
            this.addStaffButton.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.addStaffButton.Location = new System.Drawing.Point(144, 473);
            this.addStaffButton.Name = "addStaffButton";
            this.addStaffButton.Size = new System.Drawing.Size(75, 36);
            this.addStaffButton.TabIndex = 84;
            this.addStaffButton.Text = "添加";
            this.addStaffButton.UseVisualStyleBackColor = true;
            this.addStaffButton.Click += new System.EventHandler(this.addStaffButton_Click);
            // 
            // StaffListView
            // 
            this.StaffListView.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StaffListView.Location = new System.Drawing.Point(41, 25);
            this.StaffListView.Name = "StaffListView";
            this.StaffListView.Size = new System.Drawing.Size(372, 377);
            this.StaffListView.TabIndex = 86;
            this.StaffListView.UseCompatibleStateImageBehavior = false;
            this.StaffListView.SelectedIndexChanged += new System.EventHandler(this.StaffListView_SelectedIndexChanged);
            // 
            // 添加职工信息
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 535);
            this.Controls.Add(this.StaffListView);
            this.Controls.Add(this.deleteStaffButton);
            this.Controls.Add(this.addStaffButton);
            this.Controls.Add(this.passwordText);
            this.Controls.Add(this.userNameText);
            this.Controls.Add(this.password);
            this.Controls.Add(this.name);
            this.Name = "添加职工信息";
            this.Text = "添加职工信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox passwordText;
        private System.Windows.Forms.TextBox userNameText;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Button deleteStaffButton;
        private System.Windows.Forms.Button addStaffButton;
        private System.Windows.Forms.ListView StaffListView;
    }
}