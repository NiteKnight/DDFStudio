Imports System.ComponentModel
Imports System.Guid
Imports System.Xml

Namespace Kernel.Data

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

        Private _XMLRenderer As XMLRenderer
        Public Property XmlRenderer() As XMLRenderer
            Get
                Return _XMLRenderer
            End Get
            Set(ByVal value As XMLRenderer)
                _XMLRenderer = value
                XMLDocument = _XMLRenderer.convertProfile2XML(Me)
                OnPropertyChanged("XMLRenderer")
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
            XMLDocument = _XMLRenderer.convertProfile2XML(Me)
            OnPropertyChanged("InformationItem")
        End Sub

        Public Sub New()
            Information = New InformationList
            _GUID = NewGuid()
        End Sub

    End Class

End Namespace

