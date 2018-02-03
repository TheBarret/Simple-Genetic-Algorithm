
Public Class Genome(Of T)
    Public Property Genes As T()
    Public Property Fitness As Single
    Public Property GeneMethod As Func(Of T)
    Public Property FitnessMethod As Func(Of Integer, Single)
    Private Property Pool As Pool(Of T)
    Sub New(Pool As Pool(Of T), size As Integer, geneMethod As Func(Of T), fitnessMethod As Func(Of Integer, Single), Optional initialize As Boolean = True)
        Me.Pool = Pool
        Me.Genes = New T(size - 1) {}
        Me.GeneMethod = geneMethod
        Me.FitnessMethod = fitnessMethod
        If (initialize) Then
            For i As Integer = 0 To Genes.Length - 1
                Me.Genes(i) = Me.GeneMethod.Invoke
            Next
        End If
    End Sub
    Public Function ComputeFitness(index As Integer) As Single
        Me.Fitness = Me.FitnessMethod.Invoke(index)
        Return Fitness
    End Function
    ''' <summary>
    ''' Probability of (n)% to inherit parent genes
    ''' </summary>
    Public Function Crossover(Parent As Genome(Of T), Optional probability As Integer = 75) As Genome(Of T)
        Dim current As Genome(Of T) = New Genome(Of T)(Me.Pool, Me.Genes.Length, Me.GeneMethod, Me.FitnessMethod, False)
        For i As Integer = 0 To Me.Genes.Length - 1
            If (Me.Pool.Randomizer.Next(0, 100) <= probability) Then
                current.Genes(i) = Me.Genes(i)
            Else
                current.Genes(i) = Parent.Genes(i)
            End If
        Next
        Return current
    End Function
    ''' <summary>
    ''' Probability of (n)% to get mutate genes
    ''' </summary>
    Public Function Mutate(Optional probability As Integer = 10) As Genome(Of T)
        For i As Integer = 0 To Me.Genes.Length - 1
            If (Me.Pool.Randomizer.Next(0, 100) <= probability) Then
                Me.Genes(i) = Me.GeneMethod.Invoke
            End If
        Next
        Return Me
    End Function
    Public Overrides Function ToString() As String
        Return String.Format("{0} [{1}]", String.Concat(Me.Genes), Me.Fitness)
    End Function
    Public ReadOnly Property Index As Integer
        Get
            Return Me.Pool.IndexOf(Me)
        End Get
    End Property
End Class