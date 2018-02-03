Public Class Pool(Of T)
    Inherits List(Of Genome(Of T))
    Public Property Generation As Integer
    Public Property BestGenes As T()
    Public Property BestFitness As Single
    Private Property NewPool As List(Of Genome(Of T))
    Private Property Elitism As Integer
    Private Property FitnessSum As Single
    Private Property GenomeSize As Integer
    Private Property GeneMethod As Func(Of T)
    Private Property FitnessMethod As Func(Of Integer, Single)
    Sub New(populationSize As Integer, genomeSize As Integer, geneFunc As Func(Of T), fitnessFunc As Func(Of Integer, Single), elitism As Integer)
        Me.Generation = 1
        Me.Elitism = elitism
        Me.NewPool = New List(Of Genome(Of T))(populationSize)
        Me.GenomeSize = genomeSize
        Me.GeneMethod = geneFunc
        Me.FitnessMethod = fitnessFunc
        Me.BestGenes = New T(genomeSize - 1) {}
        For i As Integer = 0 To populationSize - 1
            Me.Add(New Genome(Of T)(Me, genomeSize, geneFunc, fitnessFunc, True))
        Next
    End Sub
    Public Sub CreateGeneration(Optional additional As Integer = 0, Optional crossover As Boolean = False)
        Dim count As Integer = Me.Count + additional
        If (count > 0) Then
            If Me.Count > 0 Then
                Me.ComputeFitness()
                Me.Sort(AddressOf Me.Compare)
            End If
            Me.NewPool.Clear()
            For i As Integer = 0 To Me.Count - 1
                If (i < Me.Elitism AndAlso i < Me.Count) Then
                    Me.NewPool.Add(Me(i))
                ElseIf (i < Me.Count OrElse crossover) Then
                    Dim genome As Genome(Of T) = Me.Random.Crossover(Me.Random)
                    Me.NewPool.Add(genome.Mutate)
                Else
                    Me.NewPool.Add(New Genome(Of T)(Me, Me.GenomeSize, Me.GeneMethod, Me.FitnessMethod, True))
                End If
            Next
            Me.SwapPool()
            Me.Generation += 1
        End If
    End Sub
    Public Function Randomizer() As Random
        Static rnd As New Random(Me.GetHashCode + Environment.TickCount)
        Return rnd
    End Function
    Private Sub SwapPool()
        Dim buffer As List(Of Genome(Of T)) = Me.Clone
        Me.Clear()
        Me.AddRange(Me.NewPool)
        Me.NewPool = buffer
    End Sub
    Private Function Compare(ByVal a As Genome(Of T), ByVal b As Genome(Of T)) As Integer
        If a.Fitness > b.Fitness Then
            Return -1
        ElseIf a.Fitness < b.Fitness Then
            Return 1
        Else
            Return 0
        End If
    End Function
    Private Sub ComputeFitness()
        Me.FitnessSum = 0
        Dim best As Genome(Of T) = Me(0)
        For i As Integer = 0 To Me.Count - 1
            Me.FitnessSum += Me(i).ComputeFitness(i)
            If (Me(i).Fitness > best.Fitness) Then
                best = Me(i)
            End If
        Next
        Me.BestFitness = best.Fitness
        best.Genes.CopyTo(Me.BestGenes, 0)
    End Sub
End Class
