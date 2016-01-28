Imports System.Xml
Imports System.Xml.Schema


Namespace Kernel
    Public Class XMLManager

        Private _XMLDoc As XmlDocument
        Private _XMLSchema As XmlSchema

        Public Event XMLSchemaValidationWarning(ByVal sender As Object, ByVal message As String)
        Public Event XMLSchemaValidationError(ByVal sender As Object, ByVal message As String)
        Public Event XMLSchemaReadException(ByVal sender As Object, ByVal message As String)

        Public Sub New()
            _XMLDoc = New XmlDocument()
            _XMLDoc.CreateElement("book", "http://www.contoso.com/books")
        End Sub

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
