Public Class Pert
    Inherits ContinuousDistribution
    Private _A As Double
    Private _B As Double
    Private _C As Double
    Sub New(ByVal a As Double, ByVal b As Double, ByVal c As Double)

    End Sub

    Public Overrides Function Clone() As ContinuousDistribution
        Return New Pert(_A, _B, _C)
    End Function

    Public Overrides Function GetCDF(value As Double) As Double
        Throw New NotImplementedException
    End Function

    Public Overrides ReadOnly Property GetCentralTendency As Double
        Get
            Return (_A + 4 * _B + _C) / 6
        End Get
    End Property

    Public Overrides Function getDistributedVariable(probability As Double) As Double
        Dim _alpha1 As Double = (GetCentralTendency - _A) * (2 * _B - _A - _C) / (_B - GetCentralTendency) * (_C - _A)
        Dim _alpha2 As Double = _alpha1 * (_C - GetCentralTendency) / (GetCentralTendency - _A)
        Return BetaFunction(_alpha1, _alpha2) * (_C - _A) + _A
    End Function

    Public Overrides ReadOnly Property GetNumberOfParameters As Short
        Get
            Return 3
        End Get
    End Property

    Public Overrides Function GetPDF(Value As Double) As Double
        Throw New NotImplementedException
    End Function

    Public Overrides ReadOnly Property GetSampleSize As Integer
        Get
            Return 0 ' cause this is made for guessing!
        End Get
    End Property

    Public Overrides Sub SetParameters(data() As Double)
        Throw New NotImplementedException
    End Sub

    Public Overrides Function Validate() As String
        Throw New NotImplementedException
    End Function
End Class
