Imports System.Drawing

Public Class MessageDialog

    Private _HeaderMessage As String
    Public Property HeaderMessage() As String
        Get
            Return _HeaderMessage
        End Get
        Set(ByVal value As String)
            _HeaderMessage = value
        End Set
    End Property

    Private _ErrorMessage As String
    Public Property ErrorMessage() As String
        Get
            Return _ErrorMessage
        End Get
        Set(ByVal value As String)
            _ErrorMessage = value
        End Set
    End Property

    Private _MessageType As MessageType
    Public Property TypeOfMessage() As MessageType
        Get
            Return _MessageType
        End Get
        Set(ByVal value As MessageType)
            _MessageType = value
        End Set
    End Property

    Private _DialogResult As ClosingResult = ClosingResult.msCancel
    Public Property Result() As ClosingResult
        Get
            Return _DialogResult
        End Get
        Set(ByVal value As ClosingResult)
            _DialogResult = value
        End Set
    End Property

    Public Enum ClosingResult
        msOK = 0
        msCancel = 1
        msApply = 2
    End Enum

    Public Enum MessageType
        MsgWarning = 1
        MsgError = 2
    End Enum

    Public Sub New(title As String, headermessage As String, errormessage As String, messagetype As MessageType)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Me.Title = title
        _HeaderMessage = headermessage
        _ErrorMessage = errormessage
        _MessageType = messagetype
        obj_TextBlock_HeaderMessage.Text = _HeaderMessage
        obj_TextBox_Message.Text = _ErrorMessage
        Select Case _MessageType
            Case MessageType.MsgWarning
                obj_Image_IconImage.Source = Kernel.convertIcon2ImageSource(My.Resources.Icon_Warning)
            Case MessageType.MsgError
                obj_Image_IconImage.Source = Kernel.convertIcon2ImageSource(My.Resources.Icon_Error)
            Case Else

        End Select
    End Sub

    Private Sub MessageDialog_Activated(sender As Object, e As EventArgs) Handles Me.Activated
    End Sub

    Private Sub obj_Button_OKButton_Click(sender As Object, e As RoutedEventArgs) Handles obj_Button_OKButton.Click
        _DialogResult = ClosingResult.msOK
        Me.Close()
    End Sub
End Class
