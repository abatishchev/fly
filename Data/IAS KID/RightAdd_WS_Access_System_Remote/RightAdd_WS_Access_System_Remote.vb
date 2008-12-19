' RightAdd_WS_Access_System_Remote.vb

' ���� �������� 28.02.2007
' ��������� �� ron2.xml, revision 893

Imports System
Imports System.Collections.Generic

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Partial Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            Dim right As CCHLOEObject
            ' ���� ������ WS_Access �� ��������� � �������� ������� right
            If Not c.RootArray("right").ObjectExists("WS_Access") Then
                ' �� ������� ���
                right = c.RootArray("right").AddObject("WS_Access")
                right.Parameter("description") = "����� ������� � ���������� ���-�������"
                right.Save()
            End If

            ' ���� ���������� ����� System_Remote � �������� ������� group
            If c.RootArray("group").ObjectExists("System_Remote") Then
                Dim group As CCHLOEObject = c.RootArray("group").GetObject("System_Remote")
                Dim LinkList As New List(Of String)
                ' �� �������� ������ ���� ��������� � ��� ����
                For Each leaf As CCHLOERoleLeaf In group.Relations("group_right", "group").Leafs.Values
                    LinkList.Add(leaf.LinkID)
                Next

                ' � ������� ��
                For Each LinkID As String In LinkList
                    c.LinkArray("group_right").DeleteLink(LinkID)
                Next

                right = c.RootArray("right").GetObject("WS_Access")
                ' � ������� � ������ WS_Access
                c.LinkArray("group_right").AddLink(Guid.NewGuid().ToString(), "group", group, "right", right, True)
            End If
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. ��� ����� ��������.