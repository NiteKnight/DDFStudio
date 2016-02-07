Imports System.Drawing
Imports System.Text
Imports System.Xml

Namespace Kernel

    Module Utilities

        Public Function convertIcon2ImageSource(icon As Icon) As ImageSource
            Dim BitmapStream As System.IO.MemoryStream = New IO.MemoryStream()
            icon.Save(BitmapStream)
            Dim decoder As BitmapDecoder = BitmapDecoder.Create(BitmapStream, BitmapCreateOptions.None, BitmapCacheOption.None)
            Dim source As BitmapSource = decoder.Frames(0)
            Return source
        End Function

        Public Function Beautify(doc As XmlDocument) As String
            Dim sb As New StringBuilder()
            Dim settings As New XmlWriterSettings()
            settings.Indent = True
            settings.IndentChars = "  "
            settings.NewLineChars = vbCr & vbLf
            settings.NewLineHandling = NewLineHandling.Replace

            Using writer As XmlWriter = XmlWriter.Create(sb, settings)
                doc.Save(writer)
            End Using
            Return sb.ToString()
        End Function
    End Module
End Namespace

