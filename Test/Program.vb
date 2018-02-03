Imports Genetics

Module Program
    Sub Main()

        
        Dim target As String = "Hello, World!"
        Dim sequence As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ,!"

        Dim trainer As New Trainer(target, sequence, 200, 2, 300)
        AddHandler trainer.Training, AddressOf Program.TrainEvent
        AddHandler trainer.ExceptionCaught, AddressOf Program.ExceptionCaught

        Console.Clear()
        Console.WriteLine("Target: {0}", target)

        trainer.Teach()
        RemoveHandler trainer.Training, AddressOf Program.TrainEvent
        RemoveHandler trainer.ExceptionCaught, AddressOf Program.ExceptionCaught

        Console.WriteLine("Total of {0} generations, result: {1}", trainer.Pool.Generation, trainer.Pool.First.ToString)
        Console.Read()

    End Sub
    Private Sub TrainEvent(Pool As Pool(Of Char), Generation As Integer, Fitness As Single, Best As Genome(Of Char))
        Console.WriteLine("{0}", Best.ToString)
    End Sub
    Private Sub ExceptionCaught(ex As Exception)
        Console.WriteLine("Exception Caught: {0}", ex.Message)
    End Sub

End Module
