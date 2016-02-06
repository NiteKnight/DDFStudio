Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Guid
Imports System.Xml

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

    Public Class FixtureProfile
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _GUID As Guid
        Public ReadOnly Property GUID() As Guid
            Get
                Return _GUID
            End Get
        End Property

        Private _Filename As String = "unnamed.xml"
        Public Property Filename() As String
            Get
                Return _Filename
            End Get
            Set(ByVal value As String)
                _Filename = value
                OnPropertyChanged("Filename")
            End Set
        End Property

        Private _XMLDocument As XmlDocument
        Public Property XMLDocument() As XmlDocument
            Get
                Return _XMLDocument
            End Get
            Set(ByVal value As XmlDocument)
                _XMLDocument = value
                OnPropertyChanged("XMLDocument")
            End Set
        End Property

        Private _hasbeenChanged As Boolean = False
        Public Property HasBeenChanged() As Boolean
            Get
                Return _hasbeenChanged
            End Get
            Set(ByVal value As Boolean)
                _hasbeenChanged = value
                OnPropertyChanged("HasBeenChanged")
            End Set
        End Property

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
            HasBeenChanged = True
            OnPropertyChanged("InformationItem")
        End Sub

        Public Sub New()
            Information = New InformationList
            _GUID = NewGuid()
        End Sub

    End Class

End Namespace

