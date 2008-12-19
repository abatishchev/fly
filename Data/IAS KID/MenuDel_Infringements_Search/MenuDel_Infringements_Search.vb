' MenuDel_Infringements_Search.vb

Imports System

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Partial Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            ' Если существует объект infringements_search в корневом массиве menuHandle
            If c.RootArray("menuHandle").ObjectExists("infringements_search") Then
                ' то получить его.
                Dim handler As CCHLOEObject = c.RootArray("menuHandle").GetObject("infringements_search")
                ' Если его имя 'Нарушения'
                If handler.Parameter("name") = "Нарушения" Then
                    ' то удалить.
                    handler.Delete()
                End If
            End If
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. Все права защищены.