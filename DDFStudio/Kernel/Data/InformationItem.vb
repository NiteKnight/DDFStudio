Imports System.ComponentModel

Namespace Kernel.Data
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

        Private _orderNumber As Byte
        Public Property OrderNumber() As Byte
            Get
                Return _orderNumber
            End Get
            Set(ByVal value As Byte)
                _orderNumber = value
            End Set
        End Property

        Protected Sub OnPropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Sub New(ByVal key As String, ByVal value As String, number As Byte)
            Me.Key = key
            Me.Value = value
            _orderNumber = number
        End Sub
    End Class
End Namespace