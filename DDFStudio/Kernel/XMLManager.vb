Imports System.Xml
Imports System.Xml.Schema
Imports System.IO


Namespace Kernel
    Public Class XMLManager

        Private _XMLDoc As XmlDocument
        Private _XMLSchema As XmlSchema
        Private _FixtureProfile As FixtureProfile
        Private obj_XMLRenderer As XMLRenderer

        Public Property XMLDocument() As XmlDocument
            Get
                Return _XMLDoc
            End Get
            Set(ByVal value As XmlDocument)
                _XMLDoc = value
            End Set
        End Property

        Public Property Profile() As FixtureProfile
            Get
                Return _FixtureProfile
            End Get
            Set(ByVal value As FixtureProfile)
                _FixtureProfile = value
                _FixtureProfile.XmlRenderer = obj_XMLRenderer
            End Set
        End Property


        Private obj_XMLParser As XMLParser


        Public Event XMLSchemaValidationWarning(ByVal sender As Object, ByVal message As String)
        Public Event XMLSchemaValidationError(ByVal sender As Object, ByVal message As String)
        Public Event XMLSchemaReadException(ByVal sender As Object, ByVal message As String)

        Public Sub New()
            _XMLDoc = New XmlDocument()
            obj_XMLParser = New XMLParser
            obj_XMLRenderer = New XMLRenderer
        End Sub

        Public Function openXMLFile(filename As String) As FixtureProfile
            Dim doc As XmlDocument = loadXML(filename)
            If doc Is Nothing Then
                Return Nothing
            Else
                _XMLDoc = doc
            End If
            Dim prof As FixtureProfile = obj_XMLParser.parseXML(_XMLDoc)
            prof.XMLDocument = doc
            prof.Filename = filename
            Return prof
        End Function

        Private Function loadXML(filename As String) As XmlDocument
            Dim doc As New XmlDocument()
            Dim reader As XmlReader
            Dim settings As XmlReaderSettings = New XmlReaderSettings
            settings.Schemas.Add(_XMLSchema)
            AddHandler settings.ValidationEventHandler, AddressOf settings_ValidationEventHandler
            settings.ValidationFlags = (settings.ValidationFlags Or XmlSchemaValidationFlags.ReportValidationWarnings)
            settings.ValidationType = ValidationType.Schema
            Try
                reader = XmlReader.Create(filename, settings)
            Catch ex As System.IO.FileNotFoundException
                Dim _MsgDlg As MessageDialog = New MessageDialog("Error on loading XML data",
                                                 "An error occured while trying to load the XML data. See a detailed error message below.",
                                                 ex.Message, MessageDialog.MessageType.MsgError)
                _MsgDlg.ShowDialog()
                Return Nothing
            End Try
            'Return Nothing or XmlDocument...
            doc.PreserveWhitespace = False
            doc.Load(reader)
            reader.Close()
            Return doc
        End Function

        Private Sub settings_ValidationEventHandler(ByVal sender As Object, ByVal e As System.Xml.Schema.ValidationEventArgs)
            If (e.Severity = XmlSeverityType.Warning) Then
                Dim _MsgDlg As MessageDialog = New MessageDialog("Warning on XML validation",
                                                 "An warning occured while trying to validate the XML code to be loaded. See a detailed error message below.",
                                                 e.Message, MessageDialog.MessageType.MsgWarning)
                _MsgDlg.ShowDialog()
            ElseIf (e.Severity = XmlSeverityType.Error) Then
                Dim _MsgDlg As MessageDialog = New MessageDialog("Error on XML validation",
                                                 "An error occured while trying to validate the XML code to be loaded. See a detailed error message below.",
                                                 e.Message, MessageDialog.MessageType.MsgError)
                _MsgDlg.ShowDialog()
                Dim objectType As Type = sender.GetType
            End If
        End Sub

        Public Function saveXML(filename As String) As Boolean
            Try
                _XMLDoc.Save(filename)
                Return True
            Catch ex As XmlException
                'Add exception handling here...
                Dim _MsgDlg As MessageDialog = New MessageDialog("Error on saving",
                                                 "An error occured while trying to generate XML code for the DDF. See a detailed error message below.",
                                                 ex.Message, MessageDialog.MessageType.MsgError)
                _MsgDlg.ShowDialog()
                Return False
            Catch ex As Exception
                Dim _MsgDlg As MessageDialog = New MessageDialog("Error on saving",
                                                 "An error occured while trying to save the XML code for the DDF. See a detailed error message below.",
                                                 ex.Message, MessageDialog.MessageType.MsgError)
                _MsgDlg.ShowDialog()
                Return False
            End Try
            Return False
        End Function

        Public Sub loadXMLSchema()
            _XMLSchema = readXMLSchema()
        End Sub

        Private Function readXMLSchema() As XmlSchema
            Dim loadedXMLSchema As New XmlSchema
            Try
                Dim reader As XmlTextReader = New XmlTextReader(My.Application.Info.DirectoryPath.ToString + "/Resources/DDFSchema.xsd")
                loadedXMLSchema = XmlSchema.Read(reader, AddressOf ValidationCallback)
            Catch ex As Exception
                RaiseEvent XMLSchemaReadException(Me, ex.Message)
            End Try
            Return loadedXMLSchema
        End Function

        Private Sub ValidationCallback(ByVal sender As Object, ByVal args As ValidationEventArgs)
            Dim int_SeverityType As Integer = 0
            If args.Severity = XmlSeverityType.Warning Then
                RaiseEvent XMLSchemaValidationWarning(Me, args.Message)
            Else
                If args.Severity = XmlSeverityType.Error Then
                    RaiseEvent XMLSchemaValidationError(Me, args.Message)
                End If
            End If
        End Sub

    End Class

End Namespace
