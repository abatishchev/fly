' Element_Ghost.vb

Imports System
Imports System.Collections.Generic

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            ' Корневой массив связей element_ghost
            Dim GhostLinkArray As CCHLOELinkRootArray = c.LinkArray("element_ghost")
            ' Классификатор edulevels
            Dim edulevels As CCHLOEObject = c.RootArray("classifier").GetObject("edulevels")
            ' Классификатор inspection_objects
            Dim inspection_objects As CCHLOEObject = c.RootArray("classifier").GetObject("inspection_objects")
            Dim edu As CCHLOEObject = Nothing

            ' Получение объектов связанных с классификатором inspection_objects
            For Each RoleLeaf As CCHLOERoleLeaf In inspection_objects.Relations("class_root_element", "root").Leafs.Values
                Dim RoleObject As CCHLOEObject = RoleLeaf.GetRole.OppositeRole.RoleObject
                ' Если имя такого объекта 'образовательное учреждение', то сохранить его
                If (RoleObject.Parameter("name") = "образовательное учреждение") Then
                    edu = RoleObject
                    Exit For
                End If
            Next
            ' Если объект не существует, то вызвать исключение
            If (edu Is Nothing) Then
                Throw New Exception("Не найден целевой элемент классификатора")
            End If
            ' Иначе, для всех связанных объектов с классификатором edulevels
            For Each RoleLeaf As CCHLOERoleLeaf In edulevels.Relations("class_root_element", "root").Leafs.Values
                ' добавить связь с сохраненным объектом
                Dim RoleObject As CCHLOEObject = RoleLeaf.GetRole.OppositeRole.RoleObject
                GhostLinkArray.AddLink(Guid.NewGuid.ToString, "parent", edu, "child", RoleObject)
            Next
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. Все права защищены.