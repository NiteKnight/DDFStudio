Imports System.Xml
Imports System.Xml.Schema


Namespace Kernel
    Public Class XMLManager

        Private _XMLDoc As XmlDocument
        Private _XMLSchema As XmlSchema
        Private _FixtureProfile As FixtureProfile

        Private Const _XMLString As String =
            "<?xml version=""1.0"" encoding=""utf-8""?>" & ControlChars.NewLine &
            "<device type=""DMXDevice"" dmxcversion=""3"" ddfversion=""1.0"" xmlns=""http://ddfstudio.lima-city.de"">" & ControlChars.NewLine &
            "<information></information>" & ControlChars.NewLine &
            "<procedures/>" & ControlChars.NewLine &
            "</device>"

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
            End Set
        End Property


        Public Event XMLSchemaValidationWarning(ByVal sender As Object, ByVal message As String)
        Public Event XMLSchemaValidationError(ByVal sender As Object, ByVal message As String)
        Public Event XMLSchemaReadException(ByVal sender As Object, ByVal message As String)

        Public Sub New()
            _XMLDoc = New XmlDocument()
        End Sub

        Public Sub refreshXML()
            XMLDocument = convertProfile2XML(_FixtureProfile)
        End Sub

        Private Function convertProfile2XML(profile As FixtureProfile) As XmlDocument
            Dim doc As XmlDocument = New XmlDocument()
            Dim currentNode As XmlNode
            Dim currentElement As XmlElement
            Dim newElement As XmlElement
            Dim nsManager As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
            Dim xPathString As String = ""
            nsManager.AddNamespace("ddf", "http://ddfstudio.lima-city.de")
            doc.LoadXml(_XMLString)

            'find "information" Tag and set it the current node...
            xPathString = "//ddf:device/ddf:information"
            currentNode = doc.DocumentElement.SelectSingleNode(xPathString, nsManager)
            currentElement = CType(currentNode, XmlElement)

            'create the subtags of the "information" node...
            newElement = doc.CreateElement("modell", "http://ddfstudio.lima-city.de")
            newElement.InnerText = profile.Information(0).Value
            currentElement.AppendChild(newElement)
            newElement = doc.CreateElement("vendor", "http://ddfstudio.lima-city.de")
            newElement.InnerText = profile.Information(1).Value
            currentElement.AppendChild(newElement)
            newElement = doc.CreateElement("author", "http://ddfstudio.lima-city.de")
            newElement.InnerText = profile.Information(2).Value
            currentElement.AppendChild(newElement)
            newElement = doc.CreateElement("comment", "http://ddfstudio.lima-city.de")
            newElement.InnerText = profile.Information(3).Value
            currentElement.AppendChild(newElement)
            Return doc
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
