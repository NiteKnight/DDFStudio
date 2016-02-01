Imports DDFStudio.Kernel
Imports System.IO
Imports Microsoft.Win32
'Imports DDFStudio

Class MainWindow
    Private WithEvents obj_ActiveProfile As FixtureProfile
    Private WithEvents obj_DocumentManager As DocumentManager

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        obj_DocumentManager = New DocumentManager
        'obj_ActiveProfile = obj_DocumentManager.ActiveProfile

    End Sub

    Private Sub CloseCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub NewCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        obj_DocumentManager.newDocument()
    End Sub

    Private Sub NewCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub SaveCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        If obj_DocumentManager.saveActiveDocument() = False Then
            ApplicationCommands.SaveAs.Execute(Me, Me)
        End If
        e.Handled = True
    End Sub

    Private Sub SaveCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If (obj_ActiveProfile IsNot Nothing) Then
            If (obj_ActiveProfile.HasBeenChanged) Then
                e.CanExecute = True
            Else
                e.CanExecute = False
            End If
        End If
    End Sub

    Private Sub SaveAsCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        'Show Save File Dialog here...
        Dim saveDialog As SaveFileDialog = New SaveFileDialog()
        saveDialog.Title = "Save DDF as"
        saveDialog.FileName = obj_ActiveProfile.Information(1).Value & " " & obj_ActiveProfile.Information(0).Value
        saveDialog.Filter = "XML file (*.xml)|*.xml"
        If saveDialog.ShowDialog = True Then
            obj_ActiveProfile.Filename = saveDialog.FileName
            obj_DocumentManager.saveActiveDocument()
        End If
        e.Handled = True
    End Sub

    Private Sub SaveAsCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If (obj_ActiveProfile IsNot Nothing) Then
            If (obj_ActiveProfile.HasBeenChanged) Then
                e.CanExecute = True
            Else
                e.CanExecute = False
            End If
        End If
    End Sub

    Private Sub obj_Grid_DataLayout_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles obj_Grid_DataLayout.SizeChanged
        LeftColumn.MaxWidth = obj_Grid_DataLayout.ActualWidth - RightColumn.MinWidth - SpliterColumn.ActualWidth
    End Sub

    Private Sub DocumentManager_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles obj_DocumentManager.PropertyChanged
        If e.PropertyName = "ActiveXML" Then
            obj_XMLViewer.xmlDocument = obj_DocumentManager.ActiveXML
        End If

        If e.PropertyName = "ActiveProfile" Then
            obj_ActiveProfile = obj_DocumentManager.ActiveProfile
            obj_DataGrid_FixtureHeader.DataContext = obj_ActiveProfile
            If obj_ActiveProfile.HasBeenChanged = True Then
                CType(obj_TabControl_EditorTabs.SelectedItem, TabItem).Header = "*" & obj_ActiveProfile.Information(1).Value & " - " & obj_ActiveProfile.Information(0).Value
            Else
                'Remove Asterisk from Tabheader here...
                CType(obj_TabControl_EditorTabs.SelectedItem, TabItem).Header = obj_ActiveProfile.Information(1).Value & " - " & obj_ActiveProfile.Information(0).Value
            End If
            If obj_ActiveProfile.Filename = Nothing Then
                Me.Title = "DDFStudio - (unnamed.xml)"
            Else
                Me.Title = "DDFStudio - (" & Path.GetFileName(obj_ActiveProfile.Filename) & ")"
            End If
        End If
    End Sub

    Private Sub DocumentManager_NewDocumentAdded(sender As Object) Handles obj_DocumentManager.NewDocumentAdded
        obj_TabControl_EditorTabs.Items.Add(New TabItem())
        obj_TabControl_EditorTabs.SelectedIndex = obj_TabControl_EditorTabs.Items.Count - 1
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        obj_DocumentManager.newDocument()
    End Sub

    Private Sub obj_TabControl_EditorTabs_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles obj_TabControl_EditorTabs.SelectionChanged
        obj_DocumentManager.selectDocument(obj_TabControl_EditorTabs.SelectedIndex)
    End Sub
End Class

