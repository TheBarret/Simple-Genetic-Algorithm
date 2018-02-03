Imports System.Runtime.CompilerServices

Public Module Extensions
    <Extension>
    Public Function Clone(Of T)(pool As Pool(Of T)) As List(Of Genome(Of T))
        Dim buffer As New List(Of Genome(Of T))
        For Each gn As Genome(Of T) In pool
            buffer.Add(New Genome(Of T)(pool, gn.Genes.Length, gn.GeneMethod, gn.FitnessMethod, True))
        Next
        Return buffer
    End Function
    <Extension>
    Public Function Random(Of T)(pool As Pool(Of T)) As Genome(Of T)
        Return pool(pool.Randomizer.Next(0, pool.Count - 1))
    End Function
End Module
