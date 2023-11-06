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
        ((System.ComponentModel.ISupportInitialize)WebView).BeginInit();
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
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(631, 739);
        Controls.Add(WebView);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "MainWindow";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Gpt Wrapper";
        ((System.ComponentModel.ISupportInitialize)WebView).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private WebView2 WebView;
}
