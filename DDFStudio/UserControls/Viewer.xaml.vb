
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Xml

Namespace XMLViewer

    ''' <summary>
    ''' Interaction logic for Viewer.xaml
    ''' </summary>
    Partial Public Class Viewer
        Inherits UserControl
        Private _xmldocument As XmlDocument
        Public Sub New()
            InitializeComponent()
        End Sub

        Public Property xmlDocument() As XmlDocument
            Get
                Return _xmldocument
            End Get
            Set
                _xmldocument = Value
                BindXMLDocument()
            End Set
        End Property

        Private Sub BindXMLDocument()
            If _xmldocument Is Nothing Then
                xmlTree.ItemsSource = Nothing
                Return
            End If

            Dim provider As New XmlDataProvider()
            provider.Document = _xmldocument
            Dim binding As New Binding()
            binding.Source = provider
            binding.XPath = "child::node()"
            xmlTree.SetBinding(TreeView.ItemsSourceProperty, binding)
        End Sub
    End Class

End Namespace
