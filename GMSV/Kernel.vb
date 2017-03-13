Imports System.IO

Module Kernel
    Private LastCurrLeft As Integer = 0
    Private LastCurrTop As Integer = 0
    Private LogBuffer As New ArrayList
    Public AllDone As Boolean = False
    Public RND As New Random(Now.Millisecond)
    Public Enum LogType As Byte
        LOG_INFO = 1
        LOG_MSG = 2
        LOG_NOMORE = 0
        LOG_ERROR = 3
    End Enum
    Private Structure LogItem
        Dim Log As String
        Dim LogTypeID As LogType
        Dim Color As ConsoleColor
    End Structure
    Public Function RandString(ByVal len As Int16) As String
        Dim r As String = ""
        Dim rnd As New Random(Now.Millisecond)
        Randomize()
        While r.Length < len
            r &= Chr(rnd.Next(97, 122))
        End While
        Return r
    End Function
    Public Function GetByteS(ByVal B As String) As Byte()
        Dim S() As String = B.Split(" ")
        Dim Buffer(0) As Byte
        Dim i As Integer = 0
        For Each SS As String In S
            Buffer(i) = Val("&H" & SS)
            i += 1
            Array.Resize(Buffer, i + 1)
            '74 00 0A 00 0C 00 10 A1 D5 00 10 A1 D5 7F
        Next
        Array.Resize(Buffer, i)
        Return Buffer
    End Function
    Public Function GetSize(ByVal xGoalSize As Long, ByVal xDecimal As Integer) As String
        Dim Bx As Double
        Dim FSx As Double = Nothing
        Dim xDW As String = Nothing
        Bx = xGoalSize
        Select Case Bx ' FileSizeX.Length 
            Case Is < 1024 ^ 1 - 2048
                FSx = Bx : xDW = "b"
            Case Is < 1024 ^ 2 - 2048
                FSx = Bx / 1024 : xDW = "kb"
            Case Is < 1024 ^ 3 - 2048
                FSx = Bx / (1024 ^ 2) : xDW = "mb"
            Case Is < 1024 ^ 4 - 2048
                FSx = Bx / (1024 ^ 3) : xDW = "gb"
            Case Is < 1024 ^ 5 - 2048
                FSx = Bx / (1024 ^ 4) : xDW = "tb"
            Case Else
                FSx = "?" : xDW = "?"
        End Select
        Dim xFormat As String = "#.".PadRight(2 + xDecimal, "#")
        xFormat = Format(FSx, xFormat) & xDW
        If (xFormat.Substring(0, 1) = ".") Then
            xFormat = "0" & xFormat
        End If
        Return xFormat 'Format(FSx, xFormat) & xDW
    End Function
    Public Function FillZero(ByVal str As String, ByVal zeroMe As Byte) As String
        While str.Length < zeroMe
            str = "0" & str
        End While
        Return str
    End Function
    Private Sub PrintLog(ByVal Log As String, Optional ByVal LogTypeID As LogType = LogType.LOG_NOMORE, Optional ByVal Color As ConsoleColor = ConsoleColor.White)
        Console.CursorLeft = 0 ' LastCurrLeft
        'If (LastCurrTop > 0) Then
        'Console.CursorTop = LastCurrTop
        'End If
        Dim C As ConsoleColor = ConsoleColor.White
        Console.Write("[")
        Dim LogTitle As String = ""
        If (LogTypeID = LogType.LOG_INFO) Then
            Console.ForegroundColor = ConsoleColor.Green
            LogTitle = "INFO"
        ElseIf (LogTypeID = LogType.LOG_MSG) Then
            Console.ForegroundColor = ConsoleColor.Blue
            LogTitle = "MSG"
        ElseIf (LogTypeID = LogType.LOG_NOMORE) Then
            Console.ForegroundColor = ConsoleColor.Cyan
            LogTitle = "NOMORE"
        ElseIf (LogTypeID = LogType.LOG_ERROR) Then
            Console.ForegroundColor = ConsoleColor.Red
            LogTitle = "ERROR"
        End If
        Console.Write(LogTitle)
        Console.ForegroundColor = C
        Console.Write("] ")
        Console.ForegroundColor = Color
        Console.WriteLine(Log)
        Console.ForegroundColor = C
        'If (LastCurrLeft >= 0 And LastCurrTop > 0) Then
        'Call PutIt("Command : ")
        'End If
    End Sub
    Public Sub DoLogBuffer()
        Dim LogDid As Boolean = False
        While True
            If (LogBuffer.Count > 0) Then
                Dim LG As LogItem = LogBuffer(0)
                Try
                    LogBuffer.RemoveAt(0)
                Catch ex As Exception

                End Try

                If (Not LogDid) Then LogDid = True
                Call PrintLog(LG.Log, LG.LogTypeID, LG.Color)
            Else
                If (LogDid) Then
                    LogDid = False
                    If (Console.CursorLeft = 0 And AllDone) Then
                        Call PutIt("Command : ")
                    End If
                End If
            End If
            Threading.Thread.Sleep(1)
        End While
    End Sub
    Public Sub LogIt(ByVal Log As String, Optional ByVal LogTypeID As LogType = LogType.LOG_NOMORE, Optional ByVal Color As ConsoleColor = ConsoleColor.White)
        On Error Resume Next
        Dim LG As New LogItem
        LG.Color = Color
        LG.LogTypeID = LogTypeID
        LG.Log = Log
        LogBuffer.Add(LG)
    End Sub
    Public Sub PutIt(ByVal Msg As String, Optional ByVal Color As ConsoleColor = ConsoleColor.White)
        Console.CursorLeft = 0
        'LastCurrLeft = 0 'Console.CursorLeft
        LastCurrTop = Console.CursorTop
        Dim C As ConsoleColor = ConsoleColor.White
        Console.ForegroundColor = Color
        Console.Write(Msg)
        Console.ForegroundColor = C
    End Sub
End Module
