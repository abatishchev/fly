' MenuDel_Infringements_Search.vb

Imports System

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Partial Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            ' ���� ���������� ������ infringements_search � �������� ������� menuHandle
            If c.RootArray("menuHandle").ObjectExists("infringements_search") Then
                ' �� �������� ���.
                Dim handler As CCHLOEObject = c.RootArray("menuHandle").GetObject("infringements_search")
                ' ���� ��� ��� '���������'
                If handler.Parameter("name") = "���������" Then
                    ' �� �������.
                    handler.Delete()
                End If
            End If
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. ��� ����� ��������.