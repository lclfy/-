namespace 铁路货运发送作业信息系统
{
    partial class MonthPlanPreparation
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
            this.plannedEachVehiclePayloadText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.plannedTransmissionText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.plannedVehiclesCountText = new System.Windows.Forms.TextBox();
            this.goodsCategoryText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dailyPlanText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.doneButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // plannedEachVehiclePayloadText
            // 
            this.plannedEachVehiclePayloadText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plannedEachVehiclePayloadText.Location = new System.Drawing.Point(201, 183);
            this.plannedEachVehiclePayloadText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.plannedEachVehiclePayloadText.Name = "plannedEachVehiclePayloadText";
            this.plannedEachVehiclePayloadText.Size = new System.Drawing.Size(148, 23);
            this.plannedEachVehiclePayloadText.TabIndex = 77;
            this.plannedEachVehiclePayloadText.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.plannedEachVehiclePayloadText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.goodsText_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(26, 188);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 14);
            this.label8.TabIndex = 76;
            this.label8.Text = "计划一车净载重:";
            // 
            // plannedTransmissionText
            // 
            this.plannedTransmissionText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plannedTransmissionText.Location = new System.Drawing.Point(512, 140);
            this.plannedTransmissionText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.plannedTransmissionText.Name = "plannedTransmissionText";
            this.plannedTransmissionText.Size = new System.Drawing.Size(148, 23);
            this.plannedTransmissionText.TabIndex = 75;
            this.plannedTransmissionText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.goodsText_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(381, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 14);
            this.label5.TabIndex = 74;
            this.label5.Text = "计划发送量:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(36, 38);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(249, 20);
            this.label17.TabIndex = 71;
            this.label17.Text = "当前任务：铁路月计划编制";
            // 
            // plannedVehiclesCountText
            // 
            this.plannedVehiclesCountText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.plannedVehiclesCountText.Location = new System.Drawing.Point(201, 140);
            this.plannedVehiclesCountText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.plannedVehiclesCountText.Name = "plannedVehiclesCountText";
            this.plannedVehiclesCountText.Size = new System.Drawing.Size(148, 23);
            this.plannedVehiclesCountText.TabIndex = 67;
            this.plannedVehiclesCountText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.goodsText_KeyPress);
            // 
            // goodsCategoryText
            // 
            this.goodsCategoryText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.goodsCategoryText.Location = new System.Drawing.Point(201, 96);
            this.goodsCategoryText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.goodsCategoryText.Name = "goodsCategoryText";
            this.goodsCategoryText.Size = new System.Drawing.Size(148, 23);
            this.goodsCategoryText.TabIndex = 66;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(88, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 14);
            this.label4.TabIndex = 65;
            this.label4.Text = "计划车数:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(88, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 14);
            this.label3.TabIndex = 64;
            this.label3.Text = "货物品类:";
            // 
            // dailyPlanText
            // 
            this.dailyPlanText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dailyPlanText.Location = new System.Drawing.Point(512, 96);
            this.dailyPlanText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dailyPlanText.Name = "dailyPlanText";
            this.dailyPlanText.Size = new System.Drawing.Size(148, 23);
            this.dailyPlanText.TabIndex = 79;
            this.dailyPlanText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.goodsText_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(402, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 78;
            this.label1.Text = "计划日均:";
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(297, 248);
            this.doneButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(112, 54);
            this.doneButton.TabIndex = 87;
            this.doneButton.Text = "完成";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // MonthPlanPreparation
            // 
            this.AcceptButton = this.doneButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 320);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.dailyPlanText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.plannedEachVehiclePayloadText);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.plannedTransmissionText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.plannedVehiclesCountText);
            this.Controls.Add(this.goodsCategoryText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MonthPlanPreparation";
            this.Text = "MonthPlanPreparation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox plannedEachVehiclePayloadText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox plannedTransmissionText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox plannedVehiclesCountText;
        private System.Windows.Forms.TextBox goodsCategoryText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox dailyPlanText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button doneButton;
    }
}