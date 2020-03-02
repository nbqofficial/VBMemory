Public Class memory
#Region "WindowsAPI"
    Public Const PROCESS_ALL_ACCESS = &H1F0FF
    Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal dwProcessId As Integer) As IntPtr
    Private Declare Function WriteMemory Lib "ntdll" Alias "NtWriteVirtualMemory" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Integer, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Boolean
    Private Declare Function WriteMemoryF Lib "ntdll" Alias "NtWriteVirtualMemory" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Single, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Boolean
    Private Declare Function WriteMemoryB Lib "ntdll" Alias "NtWriteVirtualMemory" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Boolean, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Boolean
    Private Declare Function ReadMemory Lib "ntdll" Alias "NtReadVirtualMemory" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Integer, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Boolean
    Private Declare Function ReadMemoryF Lib "ntdll" Alias "NtReadVirtualMemory" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Single, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Boolean
    Private Declare Function ReadMemoryB Lib "ntdll" Alias "NtReadVirtualMemory" (ByVal hProcess As IntPtr, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Boolean, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Boolean
    Public Declare Function CloseHandle Lib "kernel32" Alias "CloseHandle" (ByVal hobject As IntPtr) As Boolean
#End Region
    Public Shared Function GetProcessByName(ByVal processname) As Process
        Dim p As Process() = Process.GetProcessesByName(processname)
        If p.Length > 0 Then
            Return p(0)
        End If
        Return Nothing
    End Function
    Public Shared Function GetHandle(ByVal p As Process) As IntPtr
        Try
            Return OpenProcess(PROCESS_ALL_ACCESS, 0, p.Id)
        Catch ex As Exception
            Return IntPtr.Zero
        End Try
    End Function
    Public Shared Function GetModuleBase(ByVal p As Process, ByVal modulename As String) As Integer
        Try
            Dim base As Integer = 0
            For Each m As ProcessModule In p.Modules
                If m.ModuleName = modulename Then
                    base = m.BaseAddress
                End If
            Next
            Return base
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Shared Function RPMInt(ByVal hProcess As IntPtr, ByVal address As Int32) As Integer
        Dim buffer As Integer
        ReadMemory(hProcess, address, buffer, 4, 0)
        Return buffer
    End Function
    Public Shared Function RPMFloat(ByVal hProcess As IntPtr, ByVal address As Int32) As Single
        Dim buffer As Single
        ReadMemoryF(hProcess, address, buffer, 4, 0)
        Return buffer
    End Function
    Public Shared Function RPMBool(ByVal hProcess As IntPtr, ByVal address As Int32) As Boolean
        Dim buffer As Boolean
        ReadMemoryB(hProcess, address, buffer, 1, 0)
        Return buffer
    End Function
    Public Shared Function WPMInt(ByVal hProcess As IntPtr, ByVal address As Int32, ByVal value As Integer) As Boolean
        If WriteMemory(hProcess, address, value, 4, 0) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function WPMFloat(ByVal hProcess As IntPtr, ByVal address As Int32, ByVal value As Single) As Boolean
        If WriteMemoryF(hProcess, address, value, 4, 0) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Shared Function WPMBool(ByVal hProcess As IntPtr, ByVal address As Int32, ByVal value As Boolean) As Boolean
        If WriteMemoryB(hProcess, address, value, 1, 0) Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
