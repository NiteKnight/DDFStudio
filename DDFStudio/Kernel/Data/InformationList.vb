Imports System.Collections.ObjectModel

Namespace Kernel.Data

    Public Class InformationList
        Inherits ObservableCollection(Of InformationItem)

        Public Event ItemChanged()

        Public Sub New()
            setupInitialData()
        End Sub

        Public Sub AddItem(item As InformationItem)
            AddHandler item.PropertyChanged, AddressOf Handler_ItemChanged
            MyBase.Add(item)
        End Sub

        Private Sub setupInitialData()
            Dim item As New InformationItem("Model", "New Model", 1)
            AddHandler item.PropertyChanged, AddressOf Handler_ItemChanged
            MyBase.Add(item)
            item = New InformationItem("Manufacturer", "New Manufacturer", 2)
            AddHandler item.PropertyChanged, AddressOf Handler_ItemChanged
            MyBase.Add(item)
            item = New InformationItem("Author", "New Author", 3)
            AddHandler item.PropertyChanged, AddressOf Handler_ItemChanged
            MyBase.Add(item)
            item = New InformationItem("Comment", "New Comment", 4)
            AddHandler item.PropertyChanged, AddressOf Handler_ItemChanged
            MyBase.Add(item)
        End Sub

        Private Sub Handler_ItemChanged()
            RaiseEvent ItemChanged()
        End Sub

    End Class
End Namespace