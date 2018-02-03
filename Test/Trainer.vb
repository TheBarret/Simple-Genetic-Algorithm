Imports Genetics
Imports System.Text.RegularExpressions

Public Class Trainer
    Public Property Pool As Pool(Of Char)
    Public Property Target As String
    Public Property Sequence As String
    Public Property Busy As Boolean
    Public Property Interval As Integer
    Private Property LastFitness As Single
    Public Event ExceptionCaught(ex As Exception)
    Public Event Training(Pool As Pool(Of Char), Generation As Integer, Fitness As Single, Best As Genome(Of Char))
    Public Event TrainingStarted()
    Public Event TrainingStopped()
    Sub New(target As String, sequence As String, poolSize As Integer, elitism As Integer, interval As Integer)
        Me.Target = target
        Me.Sequence = sequence
        Me.Interval = interval
        Me.Pool = New Pool(Of Char)(poolSize, Me.Target.Length, AddressOf Me.GetRandom, AddressOf Me.Fitness, elitism)
    End Sub
    Public Sub Teach()
        Try
            Me.Busy = True
            RaiseEvent TrainingStarted()
            Do
                Me.Pool.CreateGeneration()
                If (Me.Pool.BestFitness = 1) Then
                    Me.Abort()
                ElseIf (Me.Pool.Generation Mod Me.Interval = 0 Or Not Me.Busy) Then
                    RaiseEvent Training(Me.Pool, Me.Pool.Generation, Me.Pool.BestFitness, Me.Pool.First)
                End If
            Loop While Me.Busy
        Catch ex As Exception
            Me.Busy = False
            RaiseEvent ExceptionCaught(ex)
        Finally
            RaiseEvent TrainingStopped()
        End Try
    End Sub
    Public Sub Abort()
        Me.Busy = False
    End Sub
    Private Function Fitness(index As Integer, Optional amplify As Boolean = False, Optional threshold As Single = 0.1F) As Single
        Dim value As Single = 0.0F, current As Genome(Of Char) = Me.Pool(index)
        For i As Integer = 0 To Me.Target.Length - 1
            If (Trainer.GetDistance(current.Genes(i), Me.Target(i)) >= threshold) Then
                value += 1
            End If
        Next
        value /= Me.Target.Length
        If (amplify) Then
            value = CSng((Math.Pow(2, value) - 1) / (2 - 1))
        End If
        Return value
    End Function
    Private Function GetRandom() As Char
        Static rnd As New Random(Me.GetHashCode)
        Return Me.Sequence(rnd.Next(Me.Sequence.Length))
    End Function
    Public Shared Function GetDistance(phrase As String, target As String) As Single
        Return CSng(1 - Trainer.LDistance(phrase, target) / Math.Max(phrase.Length, target.Length))
    End Function
    Public Shared Function LDistance(src As String, dest As String) As Integer
        Dim a As Char() = src.ToCharArray, b As Char() = dest.ToCharArray
        Dim cost As Integer, matrix As Integer(,) = New Integer(src.Length + 1 - 1, dest.Length + 1 - 1) {}
        For i As Integer = 0 To a.Length
            matrix(i, 0) = i
        Next
        For j As Integer = 0 To b.Length
            matrix(0, j) = j
        Next
        For i As Integer = 1 To a.Length
            For j As Integer = 1 To b.Length
                If a(i - 1) = b(j - 1) Then cost = 0 Else cost = 1
                matrix(i, j) = Math.Min(matrix(i - 1, j) + 1, Math.Min(matrix(i, j - 1) + 1, matrix(i - 1, j - 1) + cost))
                If (i > 1) AndAlso (j > 1) AndAlso (a(i - 1) = b(j - 2)) AndAlso (a(i - 2) = b(j - 1)) Then
                    matrix(i, j) = Math.Min(matrix(i, j), matrix(i - 2, j - 2) + cost)
                End If
            Next
        Next
        Return matrix(a.Length, b.Length)
    End Function
End Class