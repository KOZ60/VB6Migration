
namespace VBCompatible.VB6
{
    partial class EventEnumForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.vbLabel1 = new VBCompatible.VBLabel();
            this.cboAssembly = new VBCompatible.VBComboBox();
            this.vbLabel2 = new VBCompatible.VBLabel();
            this.cboControl = new VBCompatible.VBComboBox();
            this.txtEvents = new VBCompatible.VBTextBox();
            this.SuspendLayout();
            // 
            // vbLabel1
            // 
            this.vbLabel1.Location = new System.Drawing.Point(9, 19);
            this.vbLabel1.Name = "vbLabel1";
            this.vbLabel1.Size = new System.Drawing.Size(93, 17);
            this.vbLabel1.TabIndex = 0;
            this.vbLabel1.Text = "Assembly";
            // 
            // cboAssembly
            // 
            this.cboAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAssembly.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAssembly.FormattingEnabled = true;
            this.cboAssembly.Location = new System.Drawing.Point(77, 16);
            this.cboAssembly.Name = "cboAssembly";
            this.cboAssembly.Size = new System.Drawing.Size(395, 20);
            this.cboAssembly.TabIndex = 1;
            this.cboAssembly.SelectionChangeCommitted += new System.EventHandler(this.cboAssembly_SelectionChangeCommitted);
            // 
            // vbLabel2
            // 
            this.vbLabel2.Location = new System.Drawing.Point(9, 51);
            this.vbLabel2.Name = "vbLabel2";
            this.vbLabel2.Size = new System.Drawing.Size(93, 17);
            this.vbLabel2.TabIndex = 2;
            this.vbLabel2.Text = "Control";
            // 
            // cboControl
            // 
            this.cboControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboControl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboControl.FormattingEnabled = true;
            this.cboControl.Location = new System.Drawing.Point(77, 48);
            this.cboControl.Name = "cboControl";
            this.cboControl.Size = new System.Drawing.Size(395, 20);
            this.cboControl.TabIndex = 3;
            this.cboControl.SelectionChangeCommitted += new System.EventHandler(this.cboControl_SelectionChangeCommitted);
            // 
            // txtEvents
            // 
            this.txtEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEvents.Location = new System.Drawing.Point(9, 78);
            this.txtEvents.Multiline = true;
            this.txtEvents.Name = "txtEvents";
            this.txtEvents.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEvents.Size = new System.Drawing.Size(465, 205);
            this.txtEvents.TabIndex = 4;
            this.txtEvents.WordWrap = false;
            // 
            // EventEnumForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 292);
            this.Controls.Add(this.txtEvents);
            this.Controls.Add(this.cboControl);
            this.Controls.Add(this.vbLabel2);
            this.Controls.Add(this.cboAssembly);
            this.Controls.Add(this.vbLabel1);
            this.Name = "EventEnumForm";
            this.Text = "Event Enum";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VBLabel vbLabel1;
        private VBComboBox cboAssembly;
        private VBLabel vbLabel2;
        private VBComboBox cboControl;
        private VBTextBox txtEvents;
    }
}