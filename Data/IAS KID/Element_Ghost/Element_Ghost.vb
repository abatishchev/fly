' Element_Ghost.vb

Imports System
Imports System.Collections.Generic

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            ' �������� ������ ������ element_ghost
            Dim GhostLinkArray As CCHLOELinkRootArray = c.LinkArray("element_ghost")
            ' ������������� edulevels
            Dim edulevels As CCHLOEObject = c.RootArray("classifier").GetObject("edulevels")
            ' ������������� inspection_objects
            Dim inspection_objects As CCHLOEObject = c.RootArray("classifier").GetObject("inspection_objects")
            Dim edu As CCHLOEObject = Nothing

            ' ��������� �������� ��������� � ��������������� inspection_objects
            For Each RoleLeaf As CCHLOERoleLeaf In inspection_objects.Relations("class_root_element", "root").Leafs.Values
                Dim RoleObject As CCHLOEObject = RoleLeaf.GetRole.OppositeRole.RoleObject
                ' ���� ��� ������ ������� '��������������� ����������', �� ��������� ���
                If (RoleObject.Parameter("name") = "��������������� ����������") Then
                    edu = RoleObject
                    Exit For
                End If
            Next
            ' ���� ������ �� ����������, �� ������� ����������
            If (edu Is Nothing) Then
                Throw New Exception("�� ������ ������� ������� ��������������")
            End If
            ' �����, ��� ���� ��������� �������� � ��������������� edulevels
            For Each RoleLeaf As CCHLOERoleLeaf In edulevels.Relations("class_root_element", "root").Leafs.Values
                ' �������� ����� � ����������� ��������
                Dim RoleObject As CCHLOEObject = RoleLeaf.GetRole.OppositeRole.RoleObject
                GhostLinkArray.AddLink(Guid.NewGuid.ToString, "parent", edu, "child", RoleObject)
            Next
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. ��� ����� ��������.