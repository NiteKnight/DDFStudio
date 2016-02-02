
Module Commands

        Private _SaveAllCommand As ICommand
        Public ReadOnly Property SaveAllCommand As ICommand
            Get
                If _SaveAllCommand Is Nothing Then _SaveAllCommand = New RoutedCommand()
                Return _SaveAllCommand
            End Get
        End Property

    End Module

