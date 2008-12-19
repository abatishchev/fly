' RightAdd_WS_Access_System_Remote.vb

' Дата создания 28.02.2007
' Проверено на ron2.xml, revision 893

Imports System
Imports System.Collections.Generic

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Partial Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            Dim right As CCHLOEObject
            ' Если объект WS_Access не сущесвует в корневом массиве right
            If Not c.RootArray("right").ObjectExists("WS_Access") Then
                ' то создать его
                right = c.RootArray("right").AddObject("WS_Access")
                right.Parameter("description") = "Право доступа к локальному веб-сервису"
                right.Save()
            End If

            ' Если существует объкт System_Remote в корневом массиве group
            If c.RootArray("group").ObjectExists("System_Remote") Then
                Dim group As CCHLOEObject = c.RootArray("group").GetObject("System_Remote")
                Dim LinkList As New List(Of String)
                ' то получить список всех связанных с ним прав
                For Each leaf As CCHLOERoleLeaf In group.Relations("group_right", "group").Leafs.Values
                    LinkList.Add(leaf.LinkID)
                Next

                ' и удалить их
                For Each LinkID As String In LinkList
                    c.LinkArray("group_right").DeleteLink(LinkID)
                Next

                right = c.RootArray("right").GetObject("WS_Access")
                ' и связать с правом WS_Access
                c.LinkArray("group_right").AddLink(Guid.NewGuid().ToString(), "group", group, "right", right, True)
            End If
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. Все права защищены.