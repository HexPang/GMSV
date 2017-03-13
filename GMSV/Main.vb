Imports System.Threading
Imports System.Net.Sockets

Module Main
    Private LogPrinter As Threading.Thread
    Private SLCD As New SLCD()
    Private GMSV As New GMSV
    Sub Main()
        LogPrinter = New Threading.Thread(AddressOf Kernel.DoLogBuffer)
        LogPrinter.Start()
        Console.Title = "GMSV"
        SLCD.Start()
        GMSV.Start()
        While True
            Dim CMD As String = Console.ReadLine
        End While
    End Sub

End Module
