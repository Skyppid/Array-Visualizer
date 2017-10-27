namespace CsTester
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
      this.buttonSystemArray = new System.Windows.Forms.Button();
      this.buttonStandard = new System.Windows.Forms.Button();
      this.buttonSharpDX = new System.Windows.Forms.Button();
      this.buttonBreak = new System.Windows.Forms.Button();
      this.buttonLargeArrays = new System.Windows.Forms.Button();
      this.buttonClose = new System.Windows.Forms.Button();
      this.buttonChart = new System.Windows.Forms.Button();
      this.buttonFsNatrix = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonSystemArray
      // 
      this.buttonSystemArray.Location = new System.Drawing.Point(12, 45);
      this.buttonSystemArray.Name = "buttonSystemArray";
      this.buttonSystemArray.Size = new System.Drawing.Size(126, 27);
      this.buttonSystemArray.TabIndex = 1;
      this.buttonSystemArray.Text = "System.Array";
      this.buttonSystemArray.UseVisualStyleBackColor = true;
      this.buttonSystemArray.Click += new System.EventHandler(this.buttonSystemArray_Click);
      // 
      // buttonStandard
      // 
      this.buttonStandard.Location = new System.Drawing.Point(12, 78);
      this.buttonStandard.Name = "buttonStandard";
      this.buttonStandard.Size = new System.Drawing.Size(126, 27);
      this.buttonStandard.TabIndex = 2;
      this.buttonStandard.Text = "Standard Arrays";
      this.buttonStandard.UseVisualStyleBackColor = true;
      this.buttonStandard.Click += new System.EventHandler(this.buttonStandard_Click);
      // 
      // buttonSharpDX
      // 
      this.buttonSharpDX.Location = new System.Drawing.Point(183, 45);
      this.buttonSharpDX.Name = "buttonSharpDX";
      this.buttonSharpDX.Size = new System.Drawing.Size(126, 27);
      this.buttonSharpDX.TabIndex = 5;
      this.buttonSharpDX.Text = "Sharp DX";
      this.buttonSharpDX.UseVisualStyleBackColor = true;
      this.buttonSharpDX.Click += new System.EventHandler(this.buttonSharpDX_Click);
      // 
      // buttonBreak
      // 
      this.buttonBreak.Location = new System.Drawing.Point(12, 12);
      this.buttonBreak.Name = "buttonBreak";
      this.buttonBreak.Size = new System.Drawing.Size(126, 27);
      this.buttonBreak.TabIndex = 0;
      this.buttonBreak.Text = "Break";
      this.buttonBreak.UseVisualStyleBackColor = true;
      this.buttonBreak.Click += new System.EventHandler(this.buttonBreak_Click);
      // 
      // buttonLargeArrays
      // 
      this.buttonLargeArrays.Location = new System.Drawing.Point(12, 111);
      this.buttonLargeArrays.Name = "buttonLargeArrays";
      this.buttonLargeArrays.Size = new System.Drawing.Size(126, 27);
      this.buttonLargeArrays.TabIndex = 3;
      this.buttonLargeArrays.Text = "Large Arrays";
      this.buttonLargeArrays.UseVisualStyleBackColor = true;
      this.buttonLargeArrays.Click += new System.EventHandler(this.buttonLargeArrays_Click);
      // 
      // buttonClose
      // 
      this.buttonClose.Location = new System.Drawing.Point(183, 173);
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.Size = new System.Drawing.Size(126, 27);
      this.buttonClose.TabIndex = 6;
      this.buttonClose.Text = "Exit";
      this.buttonClose.UseVisualStyleBackColor = true;
      this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
      // 
      // buttonChart
      // 
      this.buttonChart.Location = new System.Drawing.Point(183, 12);
      this.buttonChart.Name = "buttonChart";
      this.buttonChart.Size = new System.Drawing.Size(126, 27);
      this.buttonChart.TabIndex = 4;
      this.buttonChart.Text = "Chart";
      this.buttonChart.UseVisualStyleBackColor = true;
      this.buttonChart.Click += new System.EventHandler(this.buttonChart_Click);
      // 
      // buttonFsNatrix
      // 
      this.buttonFsNatrix.Location = new System.Drawing.Point(183, 78);
      this.buttonFsNatrix.Name = "buttonFsNatrix";
      this.buttonFsNatrix.Size = new System.Drawing.Size(126, 27);
      this.buttonFsNatrix.TabIndex = 5;
      this.buttonFsNatrix.Text = "FS Matrix";
      this.buttonFsNatrix.UseVisualStyleBackColor = true;
      this.buttonFsNatrix.Click += new System.EventHandler(this.buttonFsNatrix_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(321, 212);
      this.Controls.Add(this.buttonClose);
      this.Controls.Add(this.buttonLargeArrays);
      this.Controls.Add(this.buttonBreak);
      this.Controls.Add(this.buttonChart);
      this.Controls.Add(this.buttonFsNatrix);
      this.Controls.Add(this.buttonSharpDX);
      this.Controls.Add(this.buttonStandard);
      this.Controls.Add(this.buttonSystemArray);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.Text = "Array Visualizer Tester";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonSystemArray;
    private System.Windows.Forms.Button buttonStandard;
    private System.Windows.Forms.Button buttonSharpDX;
    private System.Windows.Forms.Button buttonBreak;
    private System.Windows.Forms.Button buttonLargeArrays;
    private System.Windows.Forms.Button buttonClose;
    private System.Windows.Forms.Button buttonChart;
    private System.Windows.Forms.Button buttonFsNatrix;
  }
}

