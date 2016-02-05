Imports System.Xml
Imports System.Xml.XPath

Namespace Kernel

    Public Class XMLParser
        Private obj_XPathNavigator As XPathNavigator
        Private obj_NSManager As XmlNamespaceManager


        Public Sub New()

        End Sub

        Public Function parseXML(doc As XmlDocument) As FixtureProfile
            obj_XPathNavigator = doc.CreateNavigator()
            obj_NSManager = New XmlNamespaceManager(obj_XPathNavigator.NameTable)
            obj_NSManager.AddNamespace("ddf", "http://ddfstudio.lima-city.de")
            obj_XPathNavigator.MoveToFirstChild()
            Dim InfoData As InformationList = getInformationData()
            If InfoData IsNot Nothing Then
                Dim fixprof As New FixtureProfile()
                fixprof.Information = InfoData
                Return fixprof
            Else Return Nothing
            End If
        End Function

        Private Function getInformationData() As InformationList
            Dim iter As XPathNodeIterator = obj_XPathNavigator.Select("//ddf:information/*", obj_NSManager)
            If iter IsNot Nothing Then
                Dim infList As New InformationList
                infList.Clear()
                Do While iter.MoveNext
                    If iter.Current.Name = "modell" Then
                        infList.Add(New InformationItem("Model", iter.Current.Value, 1))
                    End If
                    If iter.Current.Name = "vendor" Then
                        infList.Add(New InformationItem("Manufacturer", iter.Current.Value, 2))
                    End If
                    If iter.Current.Name = "author" Then
                        infList.Add(New InformationItem("Author", iter.Current.Value, 3))
                    End If
                    If iter.Current.Name = "comment" Then
                        infList.Add(New InformationItem("Model", iter.Current.Value, 4))
                    End If
                Loop
                Return infList
            Else Return Nothing
            End If
        End Function
    End Class
End Namespace
