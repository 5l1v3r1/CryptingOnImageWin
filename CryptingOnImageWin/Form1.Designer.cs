namespace CryptingOnImageWin
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
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.Btn_ResimSec = new System.Windows.Forms.Button();
			this.Txt_Name = new System.Windows.Forms.TextBox();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.Btn_Select_Data_Text = new System.Windows.Forms.Button();
			this.Btn_ResimOku = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// Btn_ResimSec
			// 
			this.Btn_ResimSec.Location = new System.Drawing.Point(694, 12);
			this.Btn_ResimSec.Name = "Btn_ResimSec";
			this.Btn_ResimSec.Size = new System.Drawing.Size(94, 32);
			this.Btn_ResimSec.TabIndex = 0;
			this.Btn_ResimSec.Text = "Yaz";
			this.Btn_ResimSec.UseVisualStyleBackColor = true;
			this.Btn_ResimSec.Click += new System.EventHandler(this.Btn_ResimSec_Click);
			// 
			// Txt_Name
			// 
			this.Txt_Name.Location = new System.Drawing.Point(415, 14);
			this.Txt_Name.Name = "Txt_Name";
			this.Txt_Name.Size = new System.Drawing.Size(249, 30);
			this.Txt_Name.TabIndex = 1;
			this.Txt_Name.Text = "Yeni_Bitmap";
			// 
			// richTextBox1
			// 
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.richTextBox1.Location = new System.Drawing.Point(0, 119);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(800, 253);
			this.richTextBox1.TabIndex = 2;
			this.richTextBox1.Text = "AAAA+";
			this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
			// 
			// Btn_Select_Data_Text
			// 
			this.Btn_Select_Data_Text.Location = new System.Drawing.Point(12, 12);
			this.Btn_Select_Data_Text.Name = "Btn_Select_Data_Text";
			this.Btn_Select_Data_Text.Size = new System.Drawing.Size(214, 32);
			this.Btn_Select_Data_Text.TabIndex = 3;
			this.Btn_Select_Data_Text.Text = "Saklanacak Data Text";
			this.Btn_Select_Data_Text.UseVisualStyleBackColor = true;
			this.Btn_Select_Data_Text.Click += new System.EventHandler(this.Btn_Select_Data_Text_Click);
			// 
			// Btn_ResimOku
			// 
			this.Btn_ResimOku.Location = new System.Drawing.Point(694, 61);
			this.Btn_ResimOku.Name = "Btn_ResimOku";
			this.Btn_ResimOku.Size = new System.Drawing.Size(94, 29);
			this.Btn_ResimOku.TabIndex = 4;
			this.Btn_ResimOku.Text = "Oku";
			this.Btn_ResimOku.UseVisualStyleBackColor = true;
			this.Btn_ResimOku.Click += new System.EventHandler(this.Btn_ResimOku_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 372);
			this.Controls.Add(this.Btn_ResimOku);
			this.Controls.Add(this.Btn_Select_Data_Text);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.Txt_Name);
			this.Controls.Add(this.Btn_ResimSec);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenFileDialog openFileDialog1;
		private Button Btn_ResimSec;
		private TextBox Txt_Name;
		private RichTextBox richTextBox1;
		private Button Btn_Select_Data_Text;
		private Button Btn_ResimOku;
	}
}