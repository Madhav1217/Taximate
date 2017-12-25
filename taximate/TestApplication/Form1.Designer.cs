namespace TestApplication
{
    partial class Form1
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
            this.btnAvailableJobs = new System.Windows.Forms.Button();
            this.btnProcessAJ = new System.Windows.Forms.Button();
            this.btnAcceptJobs = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAvailableJobs
            // 
            this.btnAvailableJobs.Location = new System.Drawing.Point(26, 30);
            this.btnAvailableJobs.Name = "btnAvailableJobs";
            this.btnAvailableJobs.Size = new System.Drawing.Size(109, 23);
            this.btnAvailableJobs.TabIndex = 0;
            this.btnAvailableJobs.Text = "GetAvailableJobs";
            this.btnAvailableJobs.UseVisualStyleBackColor = true;
            this.btnAvailableJobs.Click += new System.EventHandler(this.btnAvailableJobs_Click);
            // 
            // btnProcessAJ
            // 
            this.btnProcessAJ.Location = new System.Drawing.Point(48, 71);
            this.btnProcessAJ.Name = "btnProcessAJ";
            this.btnProcessAJ.Size = new System.Drawing.Size(75, 23);
            this.btnProcessAJ.TabIndex = 1;
            this.btnProcessAJ.Text = "Process Available Jobs";
            this.btnProcessAJ.UseVisualStyleBackColor = true;
            this.btnProcessAJ.Click += new System.EventHandler(this.btnProcessAJ_Click);
            // 
            // btnAcceptJobs
            // 
            this.btnAcceptJobs.Location = new System.Drawing.Point(26, 112);
            this.btnAcceptJobs.Name = "btnAcceptJobs";
            this.btnAcceptJobs.Size = new System.Drawing.Size(109, 23);
            this.btnAcceptJobs.TabIndex = 2;
            this.btnAcceptJobs.Text = "AcceptJobs";
            this.btnAcceptJobs.UseVisualStyleBackColor = true;
            this.btnAcceptJobs.Click += new System.EventHandler(this.btnAcceptJobs_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(26, 161);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "CloseJob";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReject
            // 
            this.btnReject.Location = new System.Drawing.Point(154, 161);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 23);
            this.btnReject.TabIndex = 4;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAcceptJobs);
            this.Controls.Add(this.btnProcessAJ);
            this.Controls.Add(this.btnAvailableJobs);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAvailableJobs;
        private System.Windows.Forms.Button btnProcessAJ;
        private System.Windows.Forms.Button btnAcceptJobs;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnReject;
    }
}

