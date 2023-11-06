using Microsoft.Web.WebView2.WinForms;

namespace GptWrapper;

partial class MainWindow
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
        WebView = new WebView2();
        settingsButton = new Button();
        settingsPanel = new Panel();
        settingsBox = new TextBox();
        ((System.ComponentModel.ISupportInitialize)WebView).BeginInit();
        settingsPanel.SuspendLayout();
        SuspendLayout();
        // 
        // WebView
        // 
        WebView.AllowExternalDrop = true;
        WebView.CreationProperties = null;
        WebView.DefaultBackgroundColor = Color.White;
        WebView.Dock = DockStyle.Fill;
        WebView.Location = new Point(0, 0);
        WebView.Name = "WebView";
        WebView.Size = new Size(631, 739);
        WebView.TabIndex = 0;
        WebView.ZoomFactor = 1D;
        // 
        // settingsButton
        // 
        settingsButton.BackColor = Color.FromArgb(52, 53, 65);
        settingsButton.FlatAppearance.BorderSize = 0;
        settingsButton.FlatStyle = FlatStyle.Flat;
        settingsButton.Image = (Image)resources.GetObject("settingsButton.Image");
        settingsButton.Location = new Point(569, 3);
        settingsButton.Name = "settingsButton";
        settingsButton.Size = new Size(38, 38);
        settingsButton.TabIndex = 1;
        settingsButton.UseVisualStyleBackColor = false;
        settingsButton.Click += button1_Click;
        // 
        // settingsPanel
        // 
        settingsPanel.Controls.Add(settingsBox);
        settingsPanel.Location = new Point(12, 42);
        settingsPanel.Name = "settingsPanel";
        settingsPanel.Size = new Size(607, 685);
        settingsPanel.TabIndex = 2;
        settingsPanel.Visible = false;
        // 
        // settingsBox
        // 
        settingsBox.BackColor = Color.FromArgb(52, 53, 65);
        settingsBox.BorderStyle = BorderStyle.None;
        settingsBox.Dock = DockStyle.Fill;
        settingsBox.ForeColor = Color.White;
        settingsBox.Location = new Point(0, 0);
        settingsBox.Multiline = true;
        settingsBox.Name = "settingsBox";
        settingsBox.Size = new Size(607, 685);
        settingsBox.TabIndex = 0;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(631, 739);
        Controls.Add(settingsButton);
        Controls.Add(settingsPanel);
        Controls.Add(WebView);
        Name = "MainWindow";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Gpt Wrapper";
        SizeChanged += MainWindow_SizeChanged;
        ((System.ComponentModel.ISupportInitialize)WebView).EndInit();
        settingsPanel.ResumeLayout(false);
        settingsPanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private WebView2 WebView;
    private Button settingsButton;
    private Panel settingsPanel;
    private TextBox settingsBox;
}
