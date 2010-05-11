<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Follower
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
        Me.sFolder = New System.Windows.Forms.TextBox
        Me.lbl = New System.Windows.Forms.Label
        Me.tbStatus = New System.Windows.Forms.TextBox
        Me.btnGo = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'sFolder
        '
        Me.sFolder.Location = New System.Drawing.Point(52, 9)
        Me.sFolder.Name = "sFolder"
        Me.sFolder.Size = New System.Drawing.Size(161, 20)
        Me.sFolder.TabIndex = 0
        '
        'lbl
        '
        Me.lbl.AutoSize = True
        Me.lbl.Location = New System.Drawing.Point(7, 12)
        Me.lbl.Name = "lbl"
        Me.lbl.Size = New System.Drawing.Size(39, 13)
        Me.lbl.TabIndex = 1
        Me.lbl.Text = "Folder:"
        '
        'tbStatus
        '
        Me.tbStatus.Location = New System.Drawing.Point(11, 35)
        Me.tbStatus.Multiline = True
        Me.tbStatus.Name = "tbStatus"
        Me.tbStatus.Size = New System.Drawing.Size(260, 200)
        Me.tbStatus.TabIndex = 2
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(219, 9)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(52, 20)
        Me.btnGo.TabIndex = 3
        Me.btnGo.Text = "Follow"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'Follower
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(283, 239)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.tbStatus)
        Me.Controls.Add(Me.lbl)
        Me.Controls.Add(Me.sFolder)
        Me.Name = "Follower"
        Me.Text = "Follower"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents sFolder As System.Windows.Forms.TextBox
    Friend WithEvents lbl As System.Windows.Forms.Label
    Friend WithEvents tbStatus As System.Windows.Forms.TextBox
    Friend WithEvents btnGo As System.Windows.Forms.Button

End Class
