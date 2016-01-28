Imports System.Drawing

Namespace Kernel

    Module Utilities

        Public Function convertIcon2ImageSource(icon As Icon) As ImageSource
            Dim BitmapStream As System.IO.MemoryStream = New IO.MemoryStream()
            icon.Save(BitmapStream)
            Dim decoder As BitmapDecoder = BitmapDecoder.Create(BitmapStream, BitmapCreateOptions.None, BitmapCacheOption.None)
            Dim source As BitmapSource = decoder.Frames(0)
            Return source
        End Function


    End Module
End Namespace

