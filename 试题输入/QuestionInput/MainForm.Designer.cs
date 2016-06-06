/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2016/4/18
 * 时间: 11:02
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace QuestionInput
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtKey;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDesc;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtTime;
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Label Tips;
		private System.Windows.Forms.Button btnInit;
		private System.Windows.Forms.RichTextBox txtOutput;
		private System.Windows.Forms.WebBrowser txtHtml;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtKey = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtTime = new System.Windows.Forms.TextBox();
			this.txtDesc = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnScan = new System.Windows.Forms.Button();
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.Tips = new System.Windows.Forms.Label();
			this.btnInit = new System.Windows.Forms.Button();
			this.txtOutput = new System.Windows.Forms.RichTextBox();
			this.txtHtml = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// txtName
			// 
			this.txtName.AccessibleDescription = "";
			this.txtName.Font = new System.Drawing.Font("宋体", 10F);
			this.txtName.Location = new System.Drawing.Point(71, 13);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(139, 23);
			this.txtName.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 22);
			this.label1.TabIndex = 1;
			this.label1.Text = "试卷名称*";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "数据库key*";
			// 
			// txtKey
			// 
			this.txtKey.Location = new System.Drawing.Point(79, 42);
			this.txtKey.Name = "txtKey";
			this.txtKey.Size = new System.Drawing.Size(131, 21);
			this.txtKey.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(2, 221);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(97, 18);
			this.label3.TabIndex = 4;
			this.label3.Text = "限制时间(min)*";
			// 
			// txtTime
			// 
			this.txtTime.Location = new System.Drawing.Point(105, 215);
			this.txtTime.Name = "txtTime";
			this.txtTime.Size = new System.Drawing.Size(45, 21);
			this.txtTime.TabIndex = 5;
			// 
			// txtDesc
			// 
			this.txtDesc.Location = new System.Drawing.Point(46, 146);
			this.txtDesc.Multiline = true;
			this.txtDesc.Name = "txtDesc";
			this.txtDesc.Size = new System.Drawing.Size(164, 63);
			this.txtDesc.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(2, 171);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 14);
			this.label4.TabIndex = 7;
			this.label4.Text = "描述";
			// 
			// btnScan
			// 
			this.btnScan.Location = new System.Drawing.Point(12, 300);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(99, 23);
			this.btnScan.TabIndex = 8;
			this.btnScan.Text = "题目扫描";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.BtnScanClick);
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(12, 329);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(99, 23);
			this.btnCreate.TabIndex = 9;
			this.btnCreate.Text = "生成试题数据";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.BtnCreateClick);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(126, 329);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(99, 23);
			this.btnSave.TabIndex = 11;
			this.btnSave.Text = "储存试卷";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.BtnSaveClick);
			// 
			// txtInput
			// 
			this.txtInput.Location = new System.Drawing.Point(248, 12);
			this.txtInput.Multiline = true;
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(390, 543);
			this.txtInput.TabIndex = 12;
			// 
			// Tips
			// 
			this.Tips.Location = new System.Drawing.Point(12, 251);
			this.Tips.Name = "Tips";
			this.Tips.Size = new System.Drawing.Size(229, 23);
			this.Tips.TabIndex = 14;
			this.Tips.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnInit
			// 
			this.btnInit.Location = new System.Drawing.Point(126, 299);
			this.btnInit.Name = "btnInit";
			this.btnInit.Size = new System.Drawing.Size(100, 23);
			this.btnInit.TabIndex = 18;
			this.btnInit.Text = "初始化";
			this.btnInit.UseVisualStyleBackColor = true;
			this.btnInit.Click += new System.EventHandler(this.BtnInitClick);
			// 
			// txtOutput
			// 
			this.txtOutput.Location = new System.Drawing.Point(653, 13);
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ReadOnly = true;
			this.txtOutput.Size = new System.Drawing.Size(378, 261);
			this.txtOutput.TabIndex = 19;
			this.txtOutput.Text = "";
			// 
			// txtHtml
			// 
			this.txtHtml.Location = new System.Drawing.Point(653, 281);
			this.txtHtml.MinimumSize = new System.Drawing.Size(20, 20);
			this.txtHtml.Name = "txtHtml";
			this.txtHtml.Size = new System.Drawing.Size(395, 274);
			this.txtHtml.TabIndex = 14;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1060, 567);
			this.Controls.Add(this.txtHtml);
			this.Controls.Add(this.txtOutput);
			this.Controls.Add(this.btnInit);
			this.Controls.Add(this.Tips);
			this.Controls.Add(this.txtInput);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtDesc);
			this.Controls.Add(this.txtTime);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtKey);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtName);
			this.Name = "MainForm";
			this.Tag = "";
			this.Text = "试卷输入";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
