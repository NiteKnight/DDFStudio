Imports System.Xml
Imports System.ComponentModel
Imports Microsoft.Win32

Namespace Kernel
    Public Class DocumentManager
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Public Event NewDocumentAdded(sender As Object, guid As Guid)
        Public Event DocumentSaved(sender As Object, guid As Guid)
        Public Event DocumentChanged(sender As Object, guid As Guid)
        Public Event InformationItemChanged(sender As Object, guid As Guid)
        Public Event FilenameChanged(sender As Object, guid As Guid)
        Public Event RequestDocumentSelection(sender As Object, guid As Guid)
        Public Event FixtureDataChanged(sender As Object, guid As Guid)

        Private WithEvents obj_XMLManager As XMLManager
        Private saveDialog As SaveFileDialog
        Private loadDialog As OpenFileDialog


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
            RaiseEvent NewDocumentAdded(Me, profile.GUID)
        End Sub

        Public Sub selectDocument(guid As Guid)
            For Each doc As FixtureProfile In _Documents
                If doc.GUID = guid Then
                    ActiveProfile = doc
                    obj_XMLManager.Profile = _ActiveProfile
                    Exit For
                End If
            Next
        End Sub

        Public Sub loadDocument()
            If showOpenFileDialog() = True Then
                Dim tempProf As FixtureProfile = obj_XMLManager.openXMLFile(loadDialog.FileName)
                If tempProf IsNot Nothing Then
                    _Documents.Add(tempProf)
                    RaiseEvent NewDocumentAdded(Me, tempProf.GUID)
                End If
            End If
        End Sub

        Private Function showOpenFileDialog() As Boolean
            loadDialog = New OpenFileDialog
            loadDialog.Title = "Load DDF"
            loadDialog.Filter = "XML file (*.xml)|*.xml"
            Return loadDialog.ShowDialog
        End Function

        Public Sub saveDocument()
            If _ActiveProfile.Filename <> "unnamed.xml" Then
                performSave(_ActiveProfile)
            Else
                saveDocumentAs()
            End If
        End Sub

        Public Sub saveDocument(doc As FixtureProfile)
            If doc.Filename <> "unnamed.xml" Then
                performSave(doc)
            Else
                saveDocumentAs(doc)
            End If
        End Sub

        Public Sub saveDocumentAs()
            If showSaveAsDialog(_ActiveProfile) = True Then
                _ActiveProfile.Filename = saveDialog.FileName
                performSave(_ActiveProfile)
            End If
        End Sub

        Public Sub saveDocumentAs(doc As FixtureProfile)
            If showSaveAsDialog(doc) = True Then
                doc.Filename = saveDialog.FileName
                performSave(doc)
            End If
        End Sub

        Private Function showSaveAsDialog(doc As FixtureProfile) As Boolean
            'Show Save File Dialog here...
            saveDialog = New SaveFileDialog()
            saveDialog.Title = "Save DDF as"
            saveDialog.FileName = doc.Information(1).Value & " " & doc.Information(0).Value
            saveDialog.Filter = "XML file (*.xml)|*.xml"
            Return saveDialog.ShowDialog
        End Function

        Private Function performSave(doc As FixtureProfile) As Boolean
            obj_XMLManager.Profile = doc
            If obj_XMLManager.saveXML(doc.Filename) = True Then
                doc.HasBeenChanged = False
                RaiseEvent DocumentSaved(Me, doc.GUID)
                Return True
            Else
                Return False
            End If
        End Function

        Public Function checkForUnsavedDocuments() As Boolean
            For Each doc As FixtureProfile In _Documents
                If doc.HasBeenChanged = True Then
                    Return True
                End If
            Next
            Return False
        End Function

        Public Sub saveAll()
            For Each doc As FixtureProfile In _Documents
                If doc.HasBeenChanged = True Then
                    If doc.Filename = Nothing Then
                        RaiseEvent RequestDocumentSelection(Me, doc.GUID)
                    End If
                    saveDocument(doc)
                End If
            Next
        End Sub

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
            If e.PropertyName = "InformationItem" Then
                RaiseEvent InformationItemChanged(Me, _ActiveProfile.GUID)
                RaiseEvent FixtureDataChanged(Me, _ActiveProfile.GUID)
            End If
            If e.PropertyName = "Filename" Then
                RaiseEvent FilenameChanged(Me, _ActiveProfile.GUID)
            End If
            If e.PropertyName = "XMLRenderer" Then
                RaiseEvent FixtureDataChanged(Me, _ActiveProfile.GUID)
            End If
            If e.PropertyName = "XMLDocument" Then
                obj_XMLManager.XMLDocument = _ActiveProfile.XMLDocument
            End If
            RaiseEvent DocumentChanged(Me, _ActiveProfile.GUID)
        End Sub


    End Class
End Namespace

