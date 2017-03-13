Imports System.Net.Sockets

Module StreamEx
    Public Class PackageBody
        Public Property Length As Short
        Public Property DataType As Protocol.Protocol
        Public Property Data As Byte()
        Public Sub New(len As Short, dt As Protocol.Protocol, d() As Byte)
            Length = len
            DataType = dt
            Data = d
        End Sub
    End Class
    '流处理部分
    Public Function ParsePackage(buff() As Byte) As PackageBody
        If (buff Is Nothing) Then Return Nothing
        Dim MIO As New IO.MemoryStream(buff)
        If (MIO.ReadByte = Protocol.Protocol.PACK_HEAD) Then
            Dim b(1) As Byte
            MIO.Read(b, 0, b.Length)
            Dim len As Short = BitConverter.ToInt16(b, 0)
            ReDim b(1)
            MIO.Read(b, 0, b.Length)
            Dim dt As Protocol.Protocol = BitConverter.ToInt16(b, 0)
            len -= 2
            ReDim b(len - 1)
            MIO.Read(b, 0, b.Length)
            If (MIO.ReadByte = Protocol.Protocol.PACK_EOF) Then
                Dim PB As New PackageBody(len, dt, b)
                MIO.Close()
                Return PB
            End If
        End If
        Return Nothing
    End Function
    Public Function ParsePackage(reader As IO.BinaryReader) As PackageBody
        If (reader Is Nothing) Then Return Nothing
        'Dim MIO As New IO.MemoryStream(buff)
        Dim bb As Byte = reader.ReadByte
        If (bb = Protocol.Protocol.PACK_HEAD) Then
            Dim len As Short = reader.ReadUInt16
            Dim dt As Protocol.Protocol = reader.ReadUInt16
            Dim b() As Byte = Nothing
            'ReDim b(len - 1)
            b = reader.ReadBytes(len - 2)
            ' MIO.Read(b, 0, b.Length)
            If (reader.ReadByte = Protocol.Protocol.PACK_EOF) Then
                Dim PB As New PackageBody(len, dt, b)
                Return PB
            End If
        End If
        Return Nothing
    End Function
    Public Function GetBytes(ParamArray Objs()() As Byte) As Byte()
        Dim MIO As New IO.MemoryStream()
        Dim b() As Byte = Nothing
        For Each obj() As Byte In Objs
            MIO.Write(obj, 0, obj.Length)
        Next
        b = MIO.ToArray
        MIO.Close()
        Return b
    End Function
    Public Function getByteNumber(ParamArray Objs() As Object) As Byte()
        Dim MIO As New IO.MemoryStream()
        Dim b() As Byte = Nothing
        For Each obj As Object In Objs
            If (TypeOf obj Is Integer) Then
                b = BitConverter.GetBytes(Integer.Parse(obj))
            ElseIf (TypeOf obj Is Short) Then
                b = BitConverter.GetBytes(Short.Parse(obj))
            ElseIf (TypeOf obj Is Double) Then
                b = BitConverter.GetBytes(Double.Parse(obj))
            ElseIf (TypeOf obj Is Byte) Then
                ReDim b(0)
                b(0) = obj
            End If
            MIO.Write(b, 0, b.Length)
        Next
        b = MIO.ToArray
        MIO.Close()
        Return b
    End Function
    Public Function GetBytesEx(ParamArray Str()) As Byte()
        '将字符串数组返回成字节并附带长度标识（Short）
        Dim MIO As New IO.MemoryStream
        Dim b() As Byte = Nothing
        For Each s As String In Str
            If (s.Length <= Short.MaxValue) Then
                b = BitConverter.GetBytes(Short.Parse(s.Length))
                MIO.Write(b, 0, b.Length)
                b = System.Text.Encoding.UTF8.GetBytes(s)
                MIO.Write(b, 0, b.Length)
            End If
        Next
        b = MIO.ToArray
        MIO.Close()
        Return b
    End Function
    Public Sub CliSend(Cli As Socket, Data() As Byte, DataType As Protocol.Protocol)
        If (Cli Is Nothing) Then Exit Sub
        If (Cli.Connected) Then
            Cli.Send(Format_Package(Data, DataType))
        Else
            Throw New Exception("未连接到服务器。")
        End If
    End Sub
    Public Function Format_Package(Data() As Byte, DataType As Protocol.Protocol)
        Dim MIO As New IO.MemoryStream
        Dim b() As Byte = Nothing
        MIO.WriteByte(Protocol.Protocol.PACK_HEAD)
        b = BitConverter.GetBytes(Short.Parse(Data.Length + 2))
        MIO.Write(b, 0, b.Length)
        b = BitConverter.GetBytes(Short.Parse(DataType))
        MIO.Write(b, 0, b.Length)
        MIO.Write(Data, 0, Data.Length)
        MIO.WriteByte(Protocol.Protocol.PACK_EOF)
        b = MIO.ToArray
        MIO.Close()
        Return b
    End Function
End Module
