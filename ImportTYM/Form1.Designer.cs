
using System;
using System.Windows.Forms;

namespace ImportPaczek
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.title1 = new System.Windows.Forms.Label();
            this.tB1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lb5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            this.labelData = new System.Windows.Forms.Label();
            this.dataPicker = new System.Windows.Forms.DateTimePicker();
            this.labelRejestr = new System.Windows.Forms.Label();
            this.rejestr = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // title1
            // 
            this.title1.AutoSize = true;
            this.title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F);
            this.title1.Location = new System.Drawing.Point(12, 19);
            this.title1.Name = "title1";
            this.title1.Size = new System.Drawing.Size(310, 34);
            this.title1.TabIndex = 1;
            this.title1.Text = "Proszę wybrać arkusz zawierający dane faktury \r\ndo dodania do paczki, datę i reje" +
    "str";
            this.title1.Click += new System.EventHandler(this.title1_Click);
            // 
            // tB1
            // 
            this.tB1.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.tB1.Location = new System.Drawing.Point(109, 141);
            this.tB1.Name = "tB1";
            this.tB1.ReadOnly = true;
            this.tB1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tB1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tB1.Size = new System.Drawing.Size(237, 20);
            this.tB1.TabIndex = 3;
            this.tB1.Text = "nie wybrano pliku";
            this.tB1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TextBox1_MouseDoubleClick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.5F);
            this.button1.Location = new System.Drawing.Point(18, 183);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(395, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "Wykonaj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // lb5
            // 
            this.lb5.AutoSize = true;
            this.lb5.ForeColor = System.Drawing.Color.Firebrick;
            this.lb5.Location = new System.Drawing.Point(15, 200);
            this.lb5.Name = "lb5";
            this.lb5.Size = new System.Drawing.Size(0, 13);
            this.lb5.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(352, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 22);
            this.button2.TabIndex = 0;
            this.button2.Text = "Wybierz";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F);
            this.label1.Location = new System.Drawing.Point(15, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Plik arkusza:";
            // 
            // pBar1
            // 
            this.pBar1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pBar1.Location = new System.Drawing.Point(18, 234);
            this.pBar1.Maximum = 400;
            this.pBar1.MaximumSize = new System.Drawing.Size(400, 100);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(396, 18);
            this.pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pBar1.TabIndex = 6;
            this.pBar1.Visible = false;
            // 
            // labelData
            // 
            this.labelData.AutoSize = true;
            this.labelData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F);
            this.labelData.Location = new System.Drawing.Point(15, 110);
            this.labelData.Name = "labelData";
            this.labelData.Size = new System.Drawing.Size(42, 17);
            this.labelData.TabIndex = 2;
            this.labelData.Text = "Data:";
            // 
            // dataPicker
            // 
            this.dataPicker.Location = new System.Drawing.Point(109, 107);
            this.dataPicker.Name = "dataPicker";
            this.dataPicker.Size = new System.Drawing.Size(237, 20);
            this.dataPicker.TabIndex = 7;
            // 
            // labelRejestr
            // 
            this.labelRejestr.AutoSize = true;
            this.labelRejestr.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F);
            this.labelRejestr.Location = new System.Drawing.Point(15, 78);
            this.labelRejestr.Name = "labelRejestr";
            this.labelRejestr.Size = new System.Drawing.Size(57, 17);
            this.labelRejestr.TabIndex = 2;
            this.labelRejestr.Text = "Rejestr:";
            // 
            // rejestr
            // 
            this.rejestr.Location = new System.Drawing.Point(109, 78);
            this.rejestr.Name = "rejestr";
            this.rejestr.Size = new System.Drawing.Size(237, 21);
            this.rejestr.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 278);
            this.Controls.Add(this.title1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lb5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tB1);
            this.Controls.Add(this.pBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelData);
            this.Controls.Add(this.dataPicker);
            this.Controls.Add(this.labelRejestr);
            this.Controls.Add(this.rejestr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ONE: Import arkusza excel -> Paczka przelewów";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion
        private System.Windows.Forms.TextBox tB1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lb5;
        private System.Windows.Forms.ProgressBar pBar1;
        private Button button2;
        private Label label1;
        private Label title1;
        private Label labelData;
        private DateTimePicker dataPicker;
        private Label labelRejestr;
        private ComboBox rejestr;
   

}
}

