Imports System.Xml

Namespace Kernel

    Public Class XMLRenderer

        Private Const _XMLInitialString As String =
            "<?xml version=""1.0"" encoding=""utf-8""?>" & ControlChars.NewLine &
            "<device type=""DMXDevice"" dmxcversion=""3"" ddfversion=""1.0"" xmlns=""http://ddfstudio.lima-city.de"">" & ControlChars.NewLine &
            "<information></information>" & ControlChars.NewLine &
            "<functions/>" & ControlChars.NewLine &
            "<procedures/>" & ControlChars.NewLine &
            "</device>"
        Private Const _nsURI As String = "http://ddfstudio.lima-city.de"

        Public Sub New()

        End Sub

        Public Function convertProfile2XML(profile As FixtureProfile) As XmlDocument
            Dim doc As New XmlDocument
            Dim newNode As XmlNode
            Dim currentNode As XmlNode
            Dim nsManager As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
            nsManager.AddNamespace("ddf", _nsURI)
            doc.LoadXml(_XMLInitialString)

            'find information tag and add childs with data...
            currentNode = doc.SelectSingleNode("//ddf:information", nsManager)
            newNode = doc.CreateElement("modell", _nsURI)
            newNode.InnerText = profile.Information(0).Value
            currentNode.AppendChild(newNode)
            newNode = doc.CreateElement("vendor", _nsURI)
            newNode.InnerText = profile.Information(1).Value
            currentNode.AppendChild(newNode)
            newNode = doc.CreateElement("author", _nsURI)
            newNode.InnerText = profile.Information(2).Value
            currentNode.AppendChild(newNode)
            newNode = doc.CreateElement("comment", _nsURI)
            newNode.InnerText = profile.Information(3).Value
            currentNode.AppendChild(newNode)
            Return doc
        End Function

    End Class

End Namespace
