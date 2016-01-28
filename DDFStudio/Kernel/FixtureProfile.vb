Imports System.ComponentModel
Imports System.Collections.ObjectModel

Namespace Kernel
    Public Class InformationItem
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _Key As String
        Public Property Key() As String
            Get
                Return _Key
            End Get
            Set(ByVal value As String)
                _Key = value
                OnPropertyChanged("Key")
            End Set
        End Property

        Private _Value As String
        Public Property Value() As String
            Get
                Return _Value
            End Get
            Set(ByVal value As String)
                _Value = value
                OnPropertyChanged("")
            End Set
        End Property

        Protected Sub OnPropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Sub New(ByVal key As String, ByVal value As String)
            Me.Key = key
            Me.Value = value
        End Sub
    End Class

    Public Class InformationList
        Inherits ObservableCollection(Of InformationItem)

        Public Sub New()
            MyBase.Add(New InformationItem("Model", "New Model"))
            MyBase.Add(New InformationItem("Vendor", "New Vendor"))
            MyBase.Add(New InformationItem("Author", "New Author"))
            MyBase.Add(New InformationItem("Comment", "New Comment"))
        End Sub

    End Class

    Public Class FixtureProfile
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


        Private _Information As InformationList
        Public Property Information() As InformationList
            Get
                Return _Information
            End Get
            Set(ByVal value As InformationList)
                _Information = value
                OnPropertyChanged("Information")
            End Set
        End Property

        Protected Sub OnPropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Sub New()
            Information = New InformationList
        End Sub

    End Class

End Namespace

