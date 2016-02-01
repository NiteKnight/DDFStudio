Imports System.Xml
Imports System.ComponentModel

Namespace Kernel
    Public Class DocumentManager
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Public Event NewDocumentAdded(sender As Object)

        Private WithEvents obj_XMLManager As XMLManager


        Private _ActiveXML As XmlDocument
        Public Property ActiveXML() As XmlDocument
            Get
                Return _ActiveXML
            End Get
            Set(ByVal value As XmlDocument)
                _ActiveXML = value
                OnPropertyChanged("ActiveXML")
            End Set
        End Property

        Private WithEvents _ActiveProfile As FixtureProfile
        Public Property ActiveProfile() As FixtureProfile
            Get
                Return _ActiveProfile
            End Get
            Set(ByVal value As FixtureProfile)
                _ActiveProfile = value
                OnPropertyChanged("ActiveProfile")
            End Set
        End Property

        Private _Documents As New List(Of FixtureProfile)
        Public Property Documents() As List(Of FixtureProfile)
            Get
                Return _Documents
            End Get
            Set(ByVal value As List(Of FixtureProfile))
                _Documents = value
            End Set
        End Property

        Public Sub New()
            obj_XMLManager = New XMLManager()
            obj_XMLManager.loadXMLSchema()
        End Sub

        Public Sub newDocument()
            Dim profile As New FixtureProfile()
            _Documents.Add(profile)
            RaiseEvent NewDocumentAdded(Me)
        End Sub

        Public Sub selectDocument(index As Integer)
            ActiveProfile = _Documents(index)
            obj_XMLManager.Profile = _ActiveProfile
            obj_XMLManager.refreshXML()
            ActiveXML = obj_XMLManager.XMLDocument
        End Sub

        Public Function saveActiveDocument() As Boolean
            If obj_XMLManager.saveXML(_ActiveProfile.Filename) = True Then
                _ActiveProfile.HasBeenChanged = False
                Return True
            Else
                Return False
            End If
        End Function

        Protected Sub OnPropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
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

        Private Sub ActiveProfile_PropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs) Handles _ActiveProfile.PropertyChanged
            obj_XMLManager.refreshXML()
            ActiveXML = obj_XMLManager.XMLDocument
            OnPropertyChanged("ActiveProfile")
        End Sub


    End Class
End Namespace

