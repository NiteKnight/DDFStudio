Imports DDFStudio.Kernel
Imports System.IO
Imports FastColoredTextBoxNS
Imports System.Drawing
Imports DDFStudio.Kernel.Data

Class MainWindow
    Private WithEvents obj_ActiveProfile As FixtureProfile
    Private WithEvents obj_DocumentManager As DocumentManager
    Private WithEvents obj_XMLPreview As FastColoredTextBox

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        obj_DocumentManager = New DocumentManager
        obj_XMLPreview = New FastColoredTextBox()
        obj_XMLPreview.ReadOnly = True
        obj_XMLPreview.Language = FastColoredTextBoxNS.Language.XML
        'obj_XMLPreview.BorderStyle = Forms.BorderStyle.FixedSingle
        obj_XMLPreview.BorderStyle = Forms.BorderStyle.FixedSingle
        obj_XMLPreview.IndentBackColor = Color.FromArgb(255, 215, 223, 253)
        obj_XMLPreview.BackColor = Color.FromArgb(128, 255, 237, 168)
        obj_XMLPreview.SelectionColor = Color.FromArgb(255, 55, 100, 255)
    End Sub

    Private Sub CloseCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub CloseProfileCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        If obj_DocumentManager.removeDocument(CType(obj_TabControl_EditorTabs.SelectedItem, TabItem).Tag) = True Then
            obj_TabControl_EditorTabs.Items.Remove(obj_TabControl_EditorTabs.SelectedItem)
            If obj_TabControl_EditorTabs.Items.Count = 0 Then
                obj_XMLPreview.Text = String.Empty
                obj_DataGrid_FixtureHeader.DataContext = Nothing
                obj_ActiveProfile = Nothing
            End If
        End If
    End Sub

    Private Sub CloseProfileCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If obj_TabControl_EditorTabs.Items.Count > 0 Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub

    Private Sub SaveAllCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        obj_DocumentManager.saveAll()
    End Sub

    Private Sub SaveAllCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If obj_DocumentManager IsNot Nothing Then
            e.CanExecute = obj_DocumentManager.checkForUnsavedDocuments
        End If
    End Sub

    Private Sub OpenCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        'Start loading action here...
        obj_DocumentManager.loadDocument()
    End Sub

    Private Sub OpenCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub NewCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        obj_DocumentManager.newDocument()
    End Sub

    Private Sub NewCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub SaveCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        obj_DocumentManager.saveDocument()
        e.Handled = True
    End Sub

    Private Sub SaveCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If (obj_ActiveProfile IsNot Nothing) Then
            If (obj_ActiveProfile.HasBeenChanged) Then
                e.CanExecute = True
            Else
                e.CanExecute = False
            End If
        Else
            e.CanExecute = False
        End If
    End Sub

    Private Sub SaveAsCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        obj_DocumentManager.saveDocumentAs()
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

    Private Sub DocumentManager_FixtureDataChanged(sender As Object, guid As Guid) Handles obj_DocumentManager.FixtureDataChanged
        obj_XMLPreview.Text = Utilities.Beautify(obj_ActiveProfile.XMLDocument)
    End Sub

    Private Sub DocumentManager_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles obj_DocumentManager.PropertyChanged
        If e.PropertyName = "ActiveProfile" Then
            obj_ActiveProfile = obj_DocumentManager.ActiveProfile
            obj_DataGrid_FixtureHeader.DataContext = obj_ActiveProfile
            Me.Title = "DDFStudio - (" & Path.GetFileName(obj_ActiveProfile.Filename) & ")"
        End If
    End Sub

    Private Sub DocumentManager_FilenameChanged(sender As Object, guid As Guid) Handles obj_DocumentManager.FilenameChanged
        Me.Title = "DDFStudio - (" & Path.GetFileName(obj_ActiveProfile.Filename) & ")"
    End Sub

    Private Sub DocumentManager_DocumentSaved(sender As Object, guid As Guid) Handles obj_DocumentManager.DocumentSaved
        For Each tab As TabItem In obj_TabControl_EditorTabs.Items
            If CType(tab.Tag, Guid) = guid Then
                'Remove Asterisk from Tabheader here...
                tab.Header = tab.Header.ToString.Substring(1)
                Exit For
            End If
        Next
    End Sub

    Private Sub DocumentManager_DocumentChanged(sender As Object, guid As Guid) Handles obj_DocumentManager.DocumentChanged
        For Each tab As TabItem In obj_TabControl_EditorTabs.Items
            If CType(tab.Tag, Guid) = guid Then
                'Add Asterisk to Tabheader here...
                If tab.Header IsNot Nothing Then
                    If Not tab.Header.ToString.ElementAt(0) = "*" Then
                        tab.Header = "*" & tab.Header.ToString
                    End If
                    Exit For
                End If
            End If
        Next
    End Sub

    Private Sub DocumentManager_InformationItemChanged(sender As Object, guid As Guid) Handles obj_DocumentManager.InformationItemChanged
        For Each tab As TabItem In obj_TabControl_EditorTabs.Items
            If CType(tab.Tag, Guid) = guid Then
                'Update Information in Tabheader here...
                tab.Header = obj_ActiveProfile.Information(1).Value & " - " & obj_ActiveProfile.Information(0).Value
                Exit For
            End If
        Next
    End Sub

    Private Sub DocumentManager_RequestDocumentSelection(sender As Object, guid As Guid) Handles obj_DocumentManager.RequestDocumentSelection
        Dim i As Integer = 0
        For Each tab As TabItem In obj_TabControl_EditorTabs.Items
            If CType(tab.Tag, Guid) = guid Then
                obj_TabControl_EditorTabs.SelectedIndex = i
                Exit For
            End If
            i = i + 1
        Next
    End Sub

    Private Sub DocumentManager_NewDocumentAdded(sender As Object, guid As Guid) Handles obj_DocumentManager.NewDocumentAdded
        obj_TabControl_EditorTabs.Items.Add(New TabItem())
        CType(obj_TabControl_EditorTabs.Items(obj_TabControl_EditorTabs.Items.Count - 1), TabItem).Tag = guid
        obj_TabControl_EditorTabs.SelectedIndex = obj_TabControl_EditorTabs.Items.Count - 1
        CType(obj_TabControl_EditorTabs.SelectedItem, TabItem).Header = obj_ActiveProfile.Information(1).Value & " - " & obj_ActiveProfile.Information(0).Value
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        obj_WinFormHost.Child = obj_XMLPreview
        obj_DocumentManager.newDocument()
    End Sub

    Private Sub obj_TabControl_EditorTabs_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles obj_TabControl_EditorTabs.SelectionChanged
        If obj_TabControl_EditorTabs.SelectedItem IsNot Nothing Then
            obj_DocumentManager.selectDocument(CType(obj_TabControl_EditorTabs.SelectedItem, TabItem).Tag)
        End If
    End Sub
End Class

