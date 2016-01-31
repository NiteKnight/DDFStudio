Imports DDFStudio.Kernel
Imports System.Windows
Imports System.IO
Imports System
Imports Microsoft.Win32
'Imports DDFStudio

Class MainWindow
    Private WithEvents _FixtureProfile As Kernel.FixtureProfile
    Private WithEvents obj_XMLManager As XMLManager
    Private _IsShown4TheFirstTime As Boolean = False

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _FixtureProfile = New FixtureProfile
        obj_XMLManager = New XMLManager()
        obj_DataGrid_FixtureHeader.DataContext = _FixtureProfile

    End Sub

    Private Sub CloseCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub SaveCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)
        If _FixtureProfile.Filename IsNot Nothing Then
            If obj_XMLManager.saveXML(_FixtureProfile.Filename) = True Then
                _FixtureProfile.HasBeenChanged = False
            End If
        Else
            ApplicationCommands.SaveAs.Execute(Me, Me)
        End If
        e.Handled = True
    End Sub

    Private Sub SaveCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If (_FixtureProfile IsNot Nothing) Then
            If (_FixtureProfile.HasBeenChanged) Then
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
        saveDialog.FileName = _FixtureProfile.Information(1).Value & " " & _FixtureProfile.Information(0).Value
        saveDialog.Filter = "XML file (*.xml)|*.xml"
        If saveDialog.ShowDialog = True Then
            _FixtureProfile.Filename = saveDialog.FileName
            If obj_XMLManager.saveXML(_FixtureProfile.Filename) = True Then
                _FixtureProfile.HasBeenChanged = False
            End If
        End If
        e.Handled = True
    End Sub

    Private Sub SaveAsCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        If (_FixtureProfile IsNot Nothing) Then
            If (_FixtureProfile.HasBeenChanged) Then
                e.CanExecute = True
            Else
                e.CanExecute = False
            End If
        End If
    End Sub

    Private Sub obj_Grid_DataLayout_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles obj_Grid_DataLayout.SizeChanged
        LeftColumn.MaxWidth = obj_Grid_DataLayout.ActualWidth - RightColumn.MinWidth - SpliterColumn.ActualWidth
    End Sub

#Region "EventHandler XMLManager"
    Private Sub EventHandler_XMLManager_XMLSchemaReadException(ByVal sender As Object, ByVal message As String) Handles obj_XMLManager.XMLSchemaReadException
        Dim _MsgDlg As MessageDialog = New MessageDialog("Loading Error",
                                                         "Ooops... Something went wrong. The XML schema couldn't be loaded! See a detailed error message below.",
                                                         message, MessageDialog.MessageType.MsgError)
        _MsgDlg.ShowDialog()
    End Sub

    Private Sub EventHandler_XMLManager_XMLSchemaValidationWarning(ByVal sender As Object, ByVal message As String) Handles obj_XMLManager.XMLSchemaValidationWarning
        Dim _MsgDlg As MessageDialog = New MessageDialog("Validation Warning",
                                                         "There's a non severe issue with the XML schema definition. See a detailed error message below.",
                                                         message, MessageDialog.MessageType.MsgWarning)
        _MsgDlg.ShowDialog()
    End Sub

    Private Sub EventHandler_XMLManager_XMLSchemaValidationError(ByVal sender As Object, ByVal message As String) Handles obj_XMLManager.XMLSchemaValidationError
        Dim _MsgDlg As MessageDialog = New MessageDialog("Validation Error",
                                                         "Ooops... Something went wrong. The XML Schema couldn't be validated! See a detailed error message below.",
                                                         message, MessageDialog.MessageType.MsgError)
        _MsgDlg.ShowDialog()
    End Sub

#End Region

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        obj_XMLManager.loadXMLSchema()
        obj_XMLManager.Profile = _FixtureProfile
        obj_XMLManager.refreshXML()
        obj_XMLViewer.xmlDocument = obj_XMLManager.XMLDocument
    End Sub

    Private Sub Handler_FixtureProfile(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles _FixtureProfile.PropertyChanged
        obj_XMLViewer.xmlDocument = Nothing
        obj_XMLManager.refreshXML()
        obj_XMLViewer.xmlDocument = obj_XMLManager.XMLDocument
        If e.PropertyName = "HasBeenChanged" Then
            If _FixtureProfile.HasBeenChanged = True Then
                CType(obj_TabControl_EditorTabs.SelectedItem, TabItem).Header = "*" & _FixtureProfile.Information(1).Value & " - " & _FixtureProfile.Information(0).Value
            Else
                'Remove Asterisk from Tabheader here...
                CType(obj_TabControl_EditorTabs.SelectedItem, TabItem).Header = _FixtureProfile.Information(1).Value & " - " & _FixtureProfile.Information(0).Value
            End If
        End If
        If e.PropertyName = "Filename" Then
            If _FixtureProfile.Filename = Nothing Then
                Me.Title = "DDFStudio - (unnamed.xml)"
            Else
                Me.Title = "DDFStudio - (" & Path.GetFileName(_FixtureProfile.Filename) & ")"
            End If
        End If
        'CommandManager.InvalidateRequerySuggested()
    End Sub

    Private Sub MainWindow_StateChanged(sender As Object, e As EventArgs) Handles Me.StateChanged
        If (_IsShown4TheFirstTime = False) And (Me.WindowState = WindowState.Normal) Then
            _IsShown4TheFirstTime = True
        End If
    End Sub



End Class

