Imports System.Net.Sockets
Imports System.Text

Public Class SLCD
    Private Sck As SocketEx = Nothing
    Private Policy As String = ""
    Shared ReadOnly POLICY_REQUEST As String = "<policy-file-request/>"
    Shared policybytes As Byte()
    Public Sub New()
        Sck = New SocketEx()
        Policy = My.Resources.Policy
        policybytes = System.Text.Encoding.UTF8.GetBytes(Policy)
    End Sub
    Public Sub Start()
        Sck.ListenAsync(943, AddressOf SLCD_Connected)
    End Sub
    Private Sub SLCD_Connected(ByVal sock As Socket)
        Dim thread As New Threading.Thread(AddressOf CreatePolicySocket)
        thread.Start(sock)
    End Sub
    Private Shared Sub CreatePolicySocket(ByVal sock As Socket)
        Try
            Console.WriteLine("Policy client connected.")
            Dim receivebuffer As Byte() = New Byte(999) {}
            Dim receivedcount = sock.Receive(receivebuffer)
            Dim requeststr As String = Encoding.UTF8.GetString(receivebuffer, 0, receivedcount)
            If requeststr = POLICY_REQUEST Then
                sock.Send(policybytes, 0, policybytes.Length, SocketFlags.None)
            End If
        Catch
            Console.WriteLine("Policy socket client get an error.")
        Finally
            sock.Close()
            Console.WriteLine("Policy client disconnected.")
        End Try
    End Sub
End Class
