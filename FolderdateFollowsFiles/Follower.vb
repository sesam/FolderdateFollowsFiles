Imports System.Diagnostics
Imports System.Security.Permissions
Imports System.IO

Public Class Follower
    'testning: ~S\temp\T*\w2k\*.bat

    Public Shared Sub addLog(ByVal txt As String)
        Follower.tbStatus.Text = txt & vbCrLf & Follower.tbStatus.Text
        Follower.tbStatus.Update()
        'System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Shared Sub addTodo(ByVal txt As String)
        Follower.timer.Stop()    'ensure that the timer won't TICK immediately. This also ensures we modify the directories after file reset + write (2 operations) has finished.
        Follower.timer.Interval = Follower.shortInterval

        Follower.todo_list.Add(txt)
        Follower.todo.Enqueue(txt)

        Debug.Print("added " + txt + ". N:=" + Follower.todo_list.Count.ToString)

        Follower.timer.Start()
    End Sub

    Public Shared todo_list As New HashSet(Of String)
    Public Shared todo As New System.Collections.Generic.Queue(Of String)
    Private timer As New Timer

    'In live system, set the short interval to more than a slow/big file create + write operation. File transfer via VPN is sometimes SLOOOOW.
    Public shortInterval As Integer = 3000
    Public longInterval As Integer = 10000

    'http://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.aspx
    Private instance As New System.IO.FileSystemWatcher

    Private Sub sFolder_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles sFolder.MouseDoubleClick
        Me.sFolder.Text = "C:\Users\S"
    End Sub

    ' Define the event handlers.
    Private Shared Sub OnChanged(ByVal source As Object, ByVal e As FileSystemEventArgs)
        ' Specify what is done when a file is changed, created, or deleted.
        Dim msg = ("File: " & e.FullPath & " " & e.ChangeType)
        Follower.addLog(msg)
        Follower.addTodo(e.FullPath)
        System.Windows.Forms.Application.DoEvents()
        Debug.Print(msg)
    End Sub

    Public Shared Sub OnTick()
        'Betar av todo_list.
        'TODO:  Vid fel, lägg till sist i listan och vänta 1 minut. Sedan upprepar den framifrån, så den fördröjda hamnar sist.
        '       Eventuellt måste de som ger fel flyttas till todo_1fel, sedan till todo_2fel, sedan kastas bort oåtgärdade.

        Debug.Print("tick. N=" + Follower.todo.Count.ToString)
        While Follower.todo.Count > 0
            Dim x
            REM x = Follower.todo_list.First(): Follower.todo_list.Remove(x)
            x = Follower.todo.Dequeue()

            'Hoppa över x ifall det finns fler kö-rader med nyare datum. (Undviker dubbeljobb)
            While Follower.todo.Contains(x)
                x = Follower.todo.Dequeue()
            End While

            If Not Follower.updateFolders(x) Then
                'TODO: add failures to secondary todo_list?
            End If
            Debug.Print("tOck. N=" + Follower.todo.Count.ToString)
        End While

        'Nothing more to do (since count is now 0)
        Follower.timer.Interval = Follower.longInterval
    End Sub

    Public Shared Function updateFolders(ByVal file_path As String) As Boolean
        If Not My.Computer.FileSystem.FileExists(file_path) Then
            If My.Computer.FileSystem.DirectoryExists(file_path) Then Debug.Print("Directory!!!")
            Return True  'eliminate from hash, even though it failed
        End If

        Dim file_time = My.Computer.FileSystem.GetFileInfo(file_path).LastWriteTime
        Dim result As Boolean = False

        Dim dir_path = My.Computer.FileSystem.GetParentPath(file_path)
        While dir_path.Length > Follower.sFolder.Text.Length
            Dim dir_info = My.Computer.FileSystem.GetDirectoryInfo(dir_path)
            Dim timediff = (file_time - dir_info.LastWriteTime)
            If timediff.TotalSeconds > 5 Then
                Debug.Print("t_time=" + file_time.ToString + ", dir_time=" + dir_info.LastWriteTime.ToString + " -- diff (sec): " + (file_time - dir_info.LastWriteTime).TotalSeconds.ToString)
                Try
                    dir_info.LastWriteTime = file_time
                    Debug.Print("Updated " + dir_path + " to " + file_time.ToString)
                Catch ex As Exception
                    result = False
                    Debug.Print("FAILED to update " + dir_path + " to " + file_time.ToString)
                End Try

                'Dim dir_name = My.Computer.FileSystem.GetName(dir_path)
                'My.Computer.FileSystem.RenameDirectory(dir_path, "$" + dir_name + "$")
                'My.Computer.FileSystem.RenameDirectory(dir_path.Replace("\" + dir_name, "\$" + dir_name + "$"), dir_name)
            End If

            'To be used when IF is changed into WHILE:
            If dir_path.Length > 4 Then dir_path = My.Computer.FileSystem.GetParentPath(dir_path)
            Debug.Print("next folder became " + dir_path)
        End While
        Return result
    End Function

    Private Sub Follower_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        addLog("Enter a folder path above")

        ' Prepare timer that can handle updating folders (seems it must be done outside of the OnChange event!?)
        timer.Interval = Me.longInterval
        AddHandler timer.Tick, AddressOf OnTick
        timer.Enabled = True

        ' Add event handlers.
        AddHandler instance.Changed, AddressOf OnChanged
    End Sub

    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim t As String
        t = sFolder.Text
        If Len(t) > 5 And My.Computer.FileSystem.DirectoryExists(t) Then
            addLog("Following: " & t)
            instance.Path = t
            instance.Filter = "*.*"
            'instance.NotifyFilter = NotifyFilters.LastWrite
            instance.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)
            instance.IncludeSubdirectories = True

            ' Begin watching.
            instance.EnableRaisingEvents = True
        Else
            instance.EnableRaisingEvents = False
            addLog("Stopped")
        End If
    End Sub
End Class
