Imports DDFStudio.Kernel
Imports System.IO
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

    Private Sub SaveAllCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        obj_DocumentManager.saveAll()
    End Sub

    Private Sub SaveAllCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If obj_DocumentManager IsNot Nothing Then
            e.CanExecute = obj_DocumentManager.checkForUnsavedDocuments
        End If
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

    Private Sub DocumentManager_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles obj_DocumentManager.PropertyChanged
        If e.PropertyName = "ActiveXML" Then
            obj_XMLViewer.xmlDocument = obj_DocumentManager.ActiveXML
        End If

        If e.PropertyName = "ActiveProfile" Then
            obj_ActiveProfile = obj_DocumentManager.ActiveProfile
            obj_DataGrid_FixtureHeader.DataContext = obj_ActiveProfile
            If obj_ActiveProfile.Filename = Nothing Then
                Me.Title = "DDFStudio - (unnamed.xml)"
            Else
                Me.Title = "DDFStudio - (" & Path.GetFileName(obj_ActiveProfile.Filename) & ")"
            End If
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
                If Not tab.Header.ToString.ElementAt(0) = "*" Then
                    tab.Header = "*" & tab.Header.ToString
                End If
                Exit For
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
        obj_DocumentManager.newDocument()
    End Sub

    Private Sub obj_TabControl_EditorTabs_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles obj_TabControl_EditorTabs.SelectionChanged
        obj_DocumentManager.selectDocument(CType(obj_TabControl_EditorTabs.SelectedItem, TabItem).Tag)
    End Sub
End Class

