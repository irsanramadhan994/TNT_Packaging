namespace Biocov
{
    partial class Dashboard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.lblTime = new System.Windows.Forms.Label();
            this.lblOpenedBatch = new System.Windows.Forms.Label();
            this.pnlBiocov = new System.Windows.Forms.Panel();
            this.lblCountBiocov = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.pnlDashboard = new System.Windows.Forms.Panel();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.avatar = new System.Windows.Forms.PictureBox();
            this.pbValidasi = new System.Windows.Forms.Button();
            this.pbGenerateCode = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.Receh = new System.Windows.Forms.Button();
            this.pbCommision = new System.Windows.Forms.Button();
            this.pbAggregation = new System.Windows.Forms.Button();
            this.pbDecomision = new System.Windows.Forms.Button();
            this.logo = new System.Windows.Forms.PictureBox();
            this.pbConfiguration = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlBiocov.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.pnlDashboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.avatar)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblTime.Location = new System.Drawing.Point(29, 29);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(47, 20);
            this.lblTime.TabIndex = 4;
            this.lblTime.Text = "Time";
            // 
            // lblOpenedBatch
            // 
            this.lblOpenedBatch.AutoSize = true;
            this.lblOpenedBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpenedBatch.Location = new System.Drawing.Point(324, 378);
            this.lblOpenedBatch.Name = "lblOpenedBatch";
            this.lblOpenedBatch.Size = new System.Drawing.Size(129, 20);
            this.lblOpenedBatch.TabIndex = 11;
            this.lblOpenedBatch.Text = "Opened Batch ";
            this.lblOpenedBatch.Click += new System.EventHandler(this.label2_Click);
            // 
            // pnlBiocov
            // 
            this.pnlBiocov.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pnlBiocov.Controls.Add(this.lblCountBiocov);
            this.pnlBiocov.Controls.Add(this.pictureBox3);
            this.pnlBiocov.Location = new System.Drawing.Point(237, 200);
            this.pnlBiocov.Margin = new System.Windows.Forms.Padding(20);
            this.pnlBiocov.Name = "pnlBiocov";
            this.pnlBiocov.Size = new System.Drawing.Size(292, 158);
            this.pnlBiocov.TabIndex = 9;
            this.pnlBiocov.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBiocov_Paint);
            // 
            // lblCountBiocov
            // 
            this.lblCountBiocov.AutoSize = true;
            this.lblCountBiocov.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountBiocov.Location = new System.Drawing.Point(197, 73);
            this.lblCountBiocov.Name = "lblCountBiocov";
            this.lblCountBiocov.Size = new System.Drawing.Size(19, 20);
            this.lblCountBiocov.TabIndex = 13;
            this.lblCountBiocov.Text = "0";
            this.lblCountBiocov.Click += new System.EventHandler(this.lblCountBiocov_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Biocov.Properties.Resources.batch_pentabio;
            this.pictureBox3.Location = new System.Drawing.Point(29, 22);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(125, 113);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblDate.Location = new System.Drawing.Point(29, 66);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(48, 20);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "Date";
            // 
            // pnlDashboard
            // 
            this.pnlDashboard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDashboard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(148)))), ((int)(((byte)(172)))));
            this.pnlDashboard.Controls.Add(this.lblDate);
            this.pnlDashboard.Controls.Add(this.lblTime);
            this.pnlDashboard.Controls.Add(this.lblRole);
            this.pnlDashboard.Controls.Add(this.lblUserId);
            this.pnlDashboard.Controls.Add(this.label1);
            this.pnlDashboard.Controls.Add(this.avatar);
            this.pnlDashboard.Location = new System.Drawing.Point(205, 0);
            this.pnlDashboard.Name = "pnlDashboard";
            this.pnlDashboard.Size = new System.Drawing.Size(808, 134);
            this.pnlDashboard.TabIndex = 8;
            this.pnlDashboard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDashboard_Paint);
            // 
            // lblRole
            // 
            this.lblRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRole.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblRole.Location = new System.Drawing.Point(701, 57);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(43, 20);
            this.lblRole.TabIndex = 3;
            this.lblRole.Text = "Role";
            // 
            // lblUserId
            // 
            this.lblUserId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserId.AutoSize = true;
            this.lblUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserId.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblUserId.Location = new System.Drawing.Point(701, 29);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(45, 20);
            this.lblUserId.TabIndex = 2;
            this.lblUserId.Text = "User";
            this.lblUserId.Click += new System.EventHandler(this.label4_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(281, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dashboard";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // avatar
            // 
            this.avatar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.avatar.Image = global::Biocov.Properties.Resources.user;
            this.avatar.Location = new System.Drawing.Point(628, 33);
            this.avatar.Name = "avatar";
            this.avatar.Size = new System.Drawing.Size(58, 49);
            this.avatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.avatar.TabIndex = 1;
            this.avatar.TabStop = false;
            // 
            // pbValidasi
            // 
            this.pbValidasi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.pbValidasi.CausesValidation = false;
            this.pbValidasi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbValidasi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbValidasi.ForeColor = System.Drawing.Color.Snow;
            this.pbValidasi.Location = new System.Drawing.Point(2, 191);
            this.pbValidasi.Name = "pbValidasi";
            this.pbValidasi.Size = new System.Drawing.Size(199, 65);
            this.pbValidasi.TabIndex = 3;
            this.pbValidasi.Text = "Carton Validation";
            this.pbValidasi.UseVisualStyleBackColor = false;
            this.pbValidasi.Click += new System.EventHandler(this.pbValidasi_Click);
            // 
            // pbGenerateCode
            // 
            this.pbGenerateCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.pbGenerateCode.CausesValidation = false;
            this.pbGenerateCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbGenerateCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbGenerateCode.ForeColor = System.Drawing.Color.Snow;
            this.pbGenerateCode.Location = new System.Drawing.Point(2, 126);
            this.pbGenerateCode.Name = "pbGenerateCode";
            this.pbGenerateCode.Size = new System.Drawing.Size(199, 65);
            this.pbGenerateCode.TabIndex = 2;
            this.pbGenerateCode.Text = "Serialization GS1 ID Carton\r\n";
            this.pbGenerateCode.UseVisualStyleBackColor = false;
            this.pbGenerateCode.Click += new System.EventHandler(this.pbGenerateCode_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.Receh);
            this.panel1.Controls.Add(this.pbCommision);
            this.panel1.Controls.Add(this.pbAggregation);
            this.panel1.Controls.Add(this.pbDecomision);
            this.panel1.Controls.Add(this.logo);
            this.panel1.Controls.Add(this.pbConfiguration);
            this.panel1.Controls.Add(this.pbGenerateCode);
            this.panel1.Controls.Add(this.pbValidasi);
            this.panel1.Location = new System.Drawing.Point(2, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(202, 749);
            this.panel1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.button1.CausesValidation = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Snow;
            this.button1.Location = new System.Drawing.Point(1, 584);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(199, 65);
            this.button1.TabIndex = 13;
            this.button1.Text = "Manual";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Receh
            // 
            this.Receh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.Receh.CausesValidation = false;
            this.Receh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Receh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Receh.ForeColor = System.Drawing.Color.Snow;
            this.Receh.Location = new System.Drawing.Point(1, 518);
            this.Receh.Name = "Receh";
            this.Receh.Size = new System.Drawing.Size(199, 65);
            this.Receh.TabIndex = 12;
            this.Receh.Text = "Substraction";
            this.Receh.UseVisualStyleBackColor = false;
            this.Receh.Click += new System.EventHandler(this.Receh_Click);
            // 
            // pbCommision
            // 
            this.pbCommision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.pbCommision.CausesValidation = false;
            this.pbCommision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbCommision.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbCommision.ForeColor = System.Drawing.Color.Snow;
            this.pbCommision.Location = new System.Drawing.Point(1, 387);
            this.pbCommision.Name = "pbCommision";
            this.pbCommision.Size = new System.Drawing.Size(199, 65);
            this.pbCommision.TabIndex = 8;
            this.pbCommision.Text = "Commision";
            this.pbCommision.UseVisualStyleBackColor = false;
            this.pbCommision.Click += new System.EventHandler(this.button1_Click);
            // 
            // pbAggregation
            // 
            this.pbAggregation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.pbAggregation.CausesValidation = false;
            this.pbAggregation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbAggregation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbAggregation.ForeColor = System.Drawing.Color.Snow;
            this.pbAggregation.Location = new System.Drawing.Point(2, 256);
            this.pbAggregation.Name = "pbAggregation";
            this.pbAggregation.Size = new System.Drawing.Size(199, 65);
            this.pbAggregation.TabIndex = 7;
            this.pbAggregation.Text = "Vial and Carton Agregation";
            this.pbAggregation.UseVisualStyleBackColor = false;
            this.pbAggregation.Click += new System.EventHandler(this.button3_Click);
            // 
            // pbDecomision
            // 
            this.pbDecomision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.pbDecomision.CausesValidation = false;
            this.pbDecomision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbDecomision.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbDecomision.ForeColor = System.Drawing.Color.Snow;
            this.pbDecomision.Location = new System.Drawing.Point(2, 322);
            this.pbDecomision.Name = "pbDecomision";
            this.pbDecomision.Size = new System.Drawing.Size(199, 65);
            this.pbDecomision.TabIndex = 6;
            this.pbDecomision.Text = "Decomision";
            this.pbDecomision.UseVisualStyleBackColor = false;
            this.pbDecomision.Click += new System.EventHandler(this.button2_Click);
            // 
            // logo
            // 
            this.logo.Image = global::Biocov.Properties.Resources.logo_biofarma;
            this.logo.InitialImage = global::Biocov.Properties.Resources.logo_biofarma;
            this.logo.Location = new System.Drawing.Point(29, 12);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(144, 96);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 2;
            this.logo.TabStop = false;
            // 
            // pbConfiguration
            // 
            this.pbConfiguration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(47)))), ((int)(((byte)(54)))));
            this.pbConfiguration.CausesValidation = false;
            this.pbConfiguration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pbConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbConfiguration.ForeColor = System.Drawing.Color.Snow;
            this.pbConfiguration.Location = new System.Drawing.Point(2, 452);
            this.pbConfiguration.Name = "pbConfiguration";
            this.pbConfiguration.Size = new System.Drawing.Size(199, 65);
            this.pbConfiguration.TabIndex = 5;
            this.pbConfiguration.Text = "Configuration";
            this.pbConfiguration.UseVisualStyleBackColor = false;
            this.pbConfiguration.Click += new System.EventHandler(this.pbConfiguration_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 755);
            this.Controls.Add(this.lblOpenedBatch);
            this.Controls.Add(this.pnlBiocov);
            this.Controls.Add(this.pnlDashboard);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Dashboard";
            this.Text = "2D Code Serialization And Aggregation";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Dashboard_FormClosed);
            this.pnlBiocov.ResumeLayout(false);
            this.pnlBiocov.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.pnlDashboard.ResumeLayout(false);
            this.pnlDashboard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.avatar)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblOpenedBatch;
        private System.Windows.Forms.Panel pnlBiocov;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Panel pnlDashboard;
        public System.Windows.Forms.Label lblRole;
        public System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.PictureBox avatar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Button pbValidasi;
        private System.Windows.Forms.Button pbGenerateCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button pbConfiguration;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label lblCountBiocov;
        private System.Windows.Forms.Button pbDecomision;
        private System.Windows.Forms.Button pbAggregation;
        private System.Windows.Forms.Button pbCommision;
        private System.Windows.Forms.Button Receh;
        private System.Windows.Forms.Button button1;

    }
}