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
                OnPropertyChanged("Value")
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

        Public Event ItemChanged()

        Private WithEvents ModelItem As New InformationItem("Model", "New Model")
        Private WithEvents VendorItem As New InformationItem("Vendor", "New Vendor")
        Private WithEvents AuthorItem As New InformationItem("Author", "New Author")
        Private WithEvents CommentItem As New InformationItem("Comment", "New Comment")

        Public Sub New()
            MyBase.Add(ModelItem)
            MyBase.Add(VendorItem)
            MyBase.Add(AuthorItem)
            MyBase.Add(CommentItem)
        End Sub

        Private Sub Handler_ItemChanged() Handles ModelItem.PropertyChanged,
                                                    VendorItem.PropertyChanged,
                                                    AuthorItem.PropertyChanged,
                                                    CommentItem.PropertyChanged
            RaiseEvent ItemChanged()
        End Sub

    End Class

    Public Class FixtureProfile
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


        Private WithEvents _Information As InformationList
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

        Private Sub Handler_InformationItemChanged() Handles _Information.ItemChanged
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("InformationItem"))
        End Sub

        Public Sub New()
            Information = New InformationList
        End Sub

    End Class

End Namespace

