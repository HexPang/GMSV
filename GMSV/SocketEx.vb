Imports System.Net.Sockets
Imports System.Net
Public Class SocketEx
    Public Delegate Sub GetSocketCallBack(ByVal sock As Socket)
    Public Sub ListenAsync(ByVal port As Integer, ByVal callback As GetSocketCallBack)
        ' Run on another thread.
        Dim thread As New Threading.Thread(AddressOf Listen)
        thread.Start(New Object() {port, callback})
    End Sub
    Public Sub Listen(ByVal param As Object)
        Dim params As Object() = param
        Dim port As Integer = param(0)
        Dim callback As GetSocketCallBack = param(1)
        ' As a matter of convenience, we use 127.0.0.1 as server socket
        ' address. This address is can only be accessed from local.
        ' To let server accessible from network, try use machine's network
        ' address.
        ' 127.0.0.1 address
        Dim localEP As New IPEndPoint(IPAddress.Any, port)
        ' network ip address.
        'IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        'IPEndPoint localEP = new IPEndPoint(ipHostInfo.AddressList[0], port);
        Dim listener As New Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp)

        Try
            listener.Bind(localEP)
            Call Kernel.LogIt("Socket Listener opened: " & localEP.ToString(), LogType.LOG_INFO)
            While True
                listener.Listen(10)
                Dim socket As Socket = listener.Accept()
                ' Return connected socket through callback function.
                If callback IsNot Nothing Then
                    callback(socket)
                Else
                    socket.Close()
                    socket = Nothing
                End If
            End While
        Catch ex As Exception
            Call Kernel.LogIt("Exception occured:" & ex.Message, LogType.LOG_ERROR)
        End Try
        Call Kernel.LogIt("Listener closed: " & localEP.ToString(), LogType.LOG_INFO)
        listener.Close()
    End Sub
End Class
