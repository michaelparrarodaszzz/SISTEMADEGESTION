
Option Strict On
Option Infer On

Public Module AppSession
    Public Class UserInfo
        Public Property Id As Integer
        Public Property Username As String
        Public Property Nombre As String
        Public Property Rol As String
    End Class

    Public CurrentUser As UserInfo = Nothing

    Public ReadOnly Property IsLoggedIn As Boolean
        Get
            Return CurrentUser IsNot Nothing
        End Get
    End Property

    Public Sub Logout()
        CurrentUser = Nothing
    End Sub
End Module
