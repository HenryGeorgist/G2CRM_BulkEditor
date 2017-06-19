Public Class ParseTreeNodeResult
    Private _Result As Object
    Private _SingleResult As Single
    Private _type As TypeEnum
    Sub New(ByVal value As Object, ByVal type As TypeEnum)
        _Result = value
        _type = type
    End Sub
    Sub New(ByVal value As Single)
        _SingleResult = value
        _type = TypeEnum.Sng
    End Sub
    Public ReadOnly Property Type As TypeEnum
        Get
            Return _type
        End Get
    End Property
    Public ReadOnly Property GetResult As Object 'not sure if i like this
        Get
            Return _Result
        End Get
    End Property
    Public ReadOnly Property GetSingleResult As Single
        Get
            Return _SingleResult
        End Get
    End Property
End Class
