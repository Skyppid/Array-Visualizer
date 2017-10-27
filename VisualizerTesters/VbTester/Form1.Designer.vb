<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.buttonClose = New System.Windows.Forms.Button()
    Me.buttonLargeArrays = New System.Windows.Forms.Button()
    Me.buttonBreak = New System.Windows.Forms.Button()
    Me.buttonChart = New System.Windows.Forms.Button()
    Me.buttonSharpDX = New System.Windows.Forms.Button()
    Me.buttonStandard = New System.Windows.Forms.Button()
    Me.buttonSystemArray = New System.Windows.Forms.Button()
    Me.buttonFsNatrix = New System.Windows.Forms.Button()
    Me.SuspendLayout()
    '
    'buttonClose
    '
    Me.buttonClose.Location = New System.Drawing.Point(183, 173)
    Me.buttonClose.Name = "buttonClose"
    Me.buttonClose.Size = New System.Drawing.Size(126, 27)
    Me.buttonClose.TabIndex = 13
    Me.buttonClose.Text = "Exit"
    Me.buttonClose.UseVisualStyleBackColor = True
    '
    'buttonLargeArrays
    '
    Me.buttonLargeArrays.Location = New System.Drawing.Point(12, 111)
    Me.buttonLargeArrays.Name = "buttonLargeArrays"
    Me.buttonLargeArrays.Size = New System.Drawing.Size(126, 27)
    Me.buttonLargeArrays.TabIndex = 10
    Me.buttonLargeArrays.Text = "Large Arrays"
    Me.buttonLargeArrays.UseVisualStyleBackColor = True
    '
    'buttonBreak
    '
    Me.buttonBreak.Location = New System.Drawing.Point(12, 12)
    Me.buttonBreak.Name = "buttonBreak"
    Me.buttonBreak.Size = New System.Drawing.Size(126, 27)
    Me.buttonBreak.TabIndex = 7
    Me.buttonBreak.Text = "Break"
    Me.buttonBreak.UseVisualStyleBackColor = True
    '
    'buttonChart
    '
    Me.buttonChart.Location = New System.Drawing.Point(183, 12)
    Me.buttonChart.Name = "buttonChart"
    Me.buttonChart.Size = New System.Drawing.Size(126, 27)
    Me.buttonChart.TabIndex = 11
    Me.buttonChart.Text = "Chart"
    Me.buttonChart.UseVisualStyleBackColor = True
    '
    'buttonSharpDX
    '
    Me.buttonSharpDX.Location = New System.Drawing.Point(183, 45)
    Me.buttonSharpDX.Name = "buttonSharpDX"
    Me.buttonSharpDX.Size = New System.Drawing.Size(126, 27)
    Me.buttonSharpDX.TabIndex = 12
    Me.buttonSharpDX.Text = "Sharp DX"
    Me.buttonSharpDX.UseVisualStyleBackColor = True
    '
    'buttonStandard
    '
    Me.buttonStandard.Location = New System.Drawing.Point(12, 78)
    Me.buttonStandard.Name = "buttonStandard"
    Me.buttonStandard.Size = New System.Drawing.Size(126, 27)
    Me.buttonStandard.TabIndex = 9
    Me.buttonStandard.Text = "Standard Arrays"
    Me.buttonStandard.UseVisualStyleBackColor = True
    '
    'buttonSystemArray
    '
    Me.buttonSystemArray.Location = New System.Drawing.Point(12, 45)
    Me.buttonSystemArray.Name = "buttonSystemArray"
    Me.buttonSystemArray.Size = New System.Drawing.Size(126, 27)
    Me.buttonSystemArray.TabIndex = 8
    Me.buttonSystemArray.Text = "System.Array"
    Me.buttonSystemArray.UseVisualStyleBackColor = True
    '
    'buttonFsNatrix
    '
    Me.buttonFsNatrix.Location = New System.Drawing.Point(183, 78)
    Me.buttonFsNatrix.Name = "buttonFsNatrix"
    Me.buttonFsNatrix.Size = New System.Drawing.Size(126, 27)
    Me.buttonFsNatrix.TabIndex = 14
    Me.buttonFsNatrix.Text = "FS Matrix"
    Me.buttonFsNatrix.UseVisualStyleBackColor = True
    '
    'Form1
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(321, 212)
    Me.Controls.Add(Me.buttonFsNatrix)
    Me.Controls.Add(Me.buttonClose)
    Me.Controls.Add(Me.buttonLargeArrays)
    Me.Controls.Add(Me.buttonBreak)
    Me.Controls.Add(Me.buttonChart)
    Me.Controls.Add(Me.buttonSharpDX)
    Me.Controls.Add(Me.buttonStandard)
    Me.Controls.Add(Me.buttonSystemArray)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "Form1"
    Me.Text = "Array Visualizer Tester"
    Me.ResumeLayout(False)

  End Sub
  Private WithEvents buttonClose As System.Windows.Forms.Button
  Private WithEvents buttonLargeArrays As System.Windows.Forms.Button
  Private WithEvents buttonBreak As System.Windows.Forms.Button
  Private WithEvents buttonChart As System.Windows.Forms.Button
  Private WithEvents buttonSharpDX As System.Windows.Forms.Button
  Private WithEvents buttonStandard As System.Windows.Forms.Button
  Private WithEvents buttonSystemArray As System.Windows.Forms.Button
  Private WithEvents buttonFsNatrix As System.Windows.Forms.Button

End Class
