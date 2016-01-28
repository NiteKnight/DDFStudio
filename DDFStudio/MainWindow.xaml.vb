Imports DDFStudio.Kernel
'Imports DDFStudio

Class MainWindow
    Private _FixtureProfile As Kernel.FixtureProfile
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

    End Sub

    Private Sub SaveCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = False
    End Sub

    Private Sub SaveAsCommandHandler(sender As Object, e As ExecutedRoutedEventArgs)

    End Sub

    Private Sub SaveAsCommandCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = False
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

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        obj_XMLManager.loadXMLSchema()
    End Sub

    Private Sub MainWindow_StateChanged(sender As Object, e As EventArgs) Handles Me.StateChanged
        If (_IsShown4TheFirstTime = False) And (Me.WindowState = WindowState.Normal) Then
            _IsShown4TheFirstTime = True
        End If
    End Sub


#End Region

End Class

