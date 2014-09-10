namespace Chat_Test
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
            this.Client1 = new System.Windows.Forms.GroupBox();
            this.Portlabel1 = new System.Windows.Forms.Label();
            this.IPlabel1 = new System.Windows.Forms.Label();
            this.textLocalPort = new System.Windows.Forms.TextBox();
            this.textLocalIp = new System.Windows.Forms.TextBox();
            this.Client2 = new System.Windows.Forms.GroupBox();
            this.Portlabel2 = new System.Windows.Forms.Label();
            this.IPlabel2 = new System.Windows.Forms.Label();
            this.textFriendPort = new System.Windows.Forms.TextBox();
            this.textFriendIp = new System.Windows.Forms.TextBox();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.MessageList = new System.Windows.Forms.ListBox();
            this.startButton = new System.Windows.Forms.Button();
            this.sendButton = new System.Windows.Forms.Button();
            this.Client1.SuspendLayout();
            this.Client2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Client1
            // 
            this.Client1.Controls.Add(this.Portlabel1);
            this.Client1.Controls.Add(this.IPlabel1);
            this.Client1.Controls.Add(this.textLocalPort);
            this.Client1.Controls.Add(this.textLocalIp);
            this.Client1.Location = new System.Drawing.Point(13, 13);
            this.Client1.Name = "Client1";
            this.Client1.Size = new System.Drawing.Size(200, 100);
            this.Client1.TabIndex = 0;
            this.Client1.TabStop = false;
            this.Client1.Text = "Client1";
            // 
            // Portlabel1
            // 
            this.Portlabel1.AutoSize = true;
            this.Portlabel1.Location = new System.Drawing.Point(10, 46);
            this.Portlabel1.Name = "Portlabel1";
            this.Portlabel1.Size = new System.Drawing.Size(26, 13);
            this.Portlabel1.TabIndex = 3;
            this.Portlabel1.Text = "Port";
            // 
            // IPlabel1
            // 
            this.IPlabel1.AutoSize = true;
            this.IPlabel1.Location = new System.Drawing.Point(10, 21);
            this.IPlabel1.Name = "IPlabel1";
            this.IPlabel1.Size = new System.Drawing.Size(17, 13);
            this.IPlabel1.TabIndex = 2;
            this.IPlabel1.Text = "IP";
            // 
            // textLocalPort
            // 
            this.textLocalPort.Location = new System.Drawing.Point(92, 47);
            this.textLocalPort.Name = "textLocalPort";
            this.textLocalPort.Size = new System.Drawing.Size(100, 20);
            this.textLocalPort.TabIndex = 1;
            // 
            // textLocalIp
            // 
            this.textLocalIp.Location = new System.Drawing.Point(92, 20);
            this.textLocalIp.Name = "textLocalIp";
            this.textLocalIp.Size = new System.Drawing.Size(100, 20);
            this.textLocalIp.TabIndex = 0;
            // 
            // Client2
            // 
            this.Client2.Controls.Add(this.Portlabel2);
            this.Client2.Controls.Add(this.IPlabel2);
            this.Client2.Controls.Add(this.textFriendPort);
            this.Client2.Controls.Add(this.textFriendIp);
            this.Client2.Location = new System.Drawing.Point(220, 13);
            this.Client2.Name = "Client2";
            this.Client2.Size = new System.Drawing.Size(200, 100);
            this.Client2.TabIndex = 1;
            this.Client2.TabStop = false;
            this.Client2.Text = "Client2";
            // 
            // Portlabel2
            // 
            this.Portlabel2.AutoSize = true;
            this.Portlabel2.Location = new System.Drawing.Point(19, 47);
            this.Portlabel2.Name = "Portlabel2";
            this.Portlabel2.Size = new System.Drawing.Size(26, 13);
            this.Portlabel2.TabIndex = 3;
            this.Portlabel2.Text = "Port";
            // 
            // IPlabel2
            // 
            this.IPlabel2.AutoSize = true;
            this.IPlabel2.Location = new System.Drawing.Point(16, 21);
            this.IPlabel2.Name = "IPlabel2";
            this.IPlabel2.Size = new System.Drawing.Size(17, 13);
            this.IPlabel2.TabIndex = 2;
            this.IPlabel2.Text = "IP";
            // 
            // textFriendPort
            // 
            this.textFriendPort.Location = new System.Drawing.Point(94, 46);
            this.textFriendPort.Name = "textFriendPort";
            this.textFriendPort.Size = new System.Drawing.Size(100, 20);
            this.textFriendPort.TabIndex = 1;
            // 
            // textFriendIp
            // 
            this.textFriendIp.Location = new System.Drawing.Point(94, 19);
            this.textFriendIp.Name = "textFriendIp";
            this.textFriendIp.Size = new System.Drawing.Size(100, 20);
            this.textFriendIp.TabIndex = 0;
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(13, 230);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(386, 20);
            this.textMessage.TabIndex = 2;
            // 
            // MessageList
            // 
            this.MessageList.FormattingEnabled = true;
            this.MessageList.Location = new System.Drawing.Point(13, 120);
            this.MessageList.Name = "MessageList";
            this.MessageList.Size = new System.Drawing.Size(522, 95);
            this.MessageList.TabIndex = 3;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(446, 29);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(446, 230);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 5;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 262);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.MessageList);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.Client2);
            this.Controls.Add(this.Client1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Client1.ResumeLayout(false);
            this.Client1.PerformLayout();
            this.Client2.ResumeLayout(false);
            this.Client2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox Client1;
        private System.Windows.Forms.Label Portlabel1;
        private System.Windows.Forms.Label IPlabel1;
        private System.Windows.Forms.TextBox textLocalPort;
        private System.Windows.Forms.TextBox textLocalIp;
        private System.Windows.Forms.GroupBox Client2;
        private System.Windows.Forms.Label Portlabel2;
        private System.Windows.Forms.Label IPlabel2;
        private System.Windows.Forms.TextBox textFriendPort;
        private System.Windows.Forms.TextBox textFriendIp;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.ListBox MessageList;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button sendButton;
    }
}

