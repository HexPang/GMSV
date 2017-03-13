Imports System.Net.Sockets

Public Class GMSV
    Private GMSV As SocketEx = Nothing

    Public Sub New()
        GMSV = New SocketEx
    End Sub
    Public Sub Start()
        GMSV.ListenAsync(4504, AddressOf ClientConnected)
    End Sub
    Private Sub ClientConnected(sock As Socket)
        Try
            Call Kernel.LogIt("GMSV Connected...")
            Dim PB As PackageBody = Nothing
            ' Dim receivebuffer As Byte() = New Byte(1024) {}
            ' Dim receivedcount = sock.Receive(receivebuffer)
            Dim stream As NetworkStream = New NetworkStream(sock, True)
            Dim reader As New IO.BinaryReader(stream)
            'Dim writer As New IO.BinaryWriter(stream)
            While True
                PB = ParsePackage(reader)
                If (PB IsNot Nothing) Then
                    Select Case PB.DataType
                        Case Protocol.Protocol.LOGIN
                            Dim MIO As New IO.MemoryStream(PB.Data)
                            Dim bReader As New IO.BinaryReader(MIO)
                            Dim aCcLen As Short = bReader.ReadInt16
                            Dim aCc() As Byte = bReader.ReadBytes(aCcLen)
                            Dim pSwLen As Short = bReader.ReadInt16
                            Dim pSw() As Byte = bReader.ReadBytes(pSwLen)
                            Dim aCcO As String = System.Text.Encoding.UTF8.GetString(aCc)
                            Dim pSwO As String = System.Text.Encoding.UTF8.GetString(pSw)
                            Dim b() As Byte = Nothing
                            If (aCcO = "hexpang" And pSwO = "hexpang") Then
                                b = BitConverter.GetBytes(Short.Parse(Protocol.Protocol.RET_SUCCESS))
                            Else
                                b = BitConverter.GetBytes(Short.Parse(Protocol.Protocol.RET_FAILED))
                            End If
                            CliSend(sock, b, Protocol.Protocol.LOGIN)
                        Case Protocol.Protocol.START_WALK

                        Case Protocol.Protocol.STOP_WALK

                        Case Protocol.Protocol.GET_CHARACTERS
                            Dim MIO As New IO.MemoryStream
                            Dim Writer As New IO.BinaryWriter(MIO)
                            Writer.Write(Integer.Parse(0))
                            Writer.Write(Integer.Parse(10001))
                            Dim uName As String = "大刀姐姐"
                            Dim bName() As Byte = System.Text.Encoding.UTF8.GetBytes(uName)
                            Writer.Write(Int16.Parse(bName.Length))
                            Writer.Write(bName)
                            Writer.Write(Integer.Parse(1000))
                            Writer.Flush()
                            CliSend(sock, MIO.ToArray, Protocol.Protocol.GET_CHARACTERS)
                            MIO.Close()
                            'ModID = BitConverter.ToInt32(PB.Data, 0)
                            'UID = BitConverter.ToInt32(PB.Data, 4)
                            'uLength = BitConverter.ToInt16(PB.Data, 8)
                            'uName = System.Text.Encoding.UTF8.GetString(PB.Data, 10, uLength)
                            'MapID = BitConverter.ToInt32(PB.Data, 10 + uLength)
                            'writer.BaseStream.Flush()
                        Case Protocol.Protocol.LOGIN_CHARACTER
                            Dim MIO As New IO.MemoryStream
                            Dim Writer As New IO.BinaryWriter(MIO)
                            Writer.Write(Integer.Parse(0))
                            Writer.Write(Integer.Parse(10001))
                            Dim uName As String = "大刀姐姐"
                            Dim bName() As Byte = System.Text.Encoding.UTF8.GetBytes(uName)
                            Writer.Write(Int16.Parse(bName.Length))
                            Writer.Write(bName)
                            Writer.Write(Double.Parse(351))
                            Writer.Write(Double.Parse(378))
                            Writer.Write(Integer.Parse(1000))
                            Writer.Flush()
                            CliSend(sock, MIO.ToArray, Protocol.Protocol.LOGIN_CHARACTER)
                            MIO.Close()
                            'ModID = BitConverter.ToInt32(PB.Data, 0)
                            'UID = BitConverter.ToInt32(PB.Data, 4)
                            'uLength = BitConverter.ToInt16(PB.Data, 8)
                            'uName = System.Text.Encoding.UTF8.GetString(PB.Data, 10, uLength)
                            'X = BitConverter.ToDouble(PB.Data, 10 + uLength)
                            'Y = BitConverter.ToDouble(PB.Data, 18 + uLength)
                            'MapID = BitConverter.ToInt32(PB.Data, 26 + uLength)
                    End Select
                End If
            End While
        Catch ex As Exception
            Console.WriteLine("GMSV socket client get an error.")
        Finally
            sock.Close()
            Console.WriteLine("GMSV client disconnected.")
        End Try
    End Sub
End Class
