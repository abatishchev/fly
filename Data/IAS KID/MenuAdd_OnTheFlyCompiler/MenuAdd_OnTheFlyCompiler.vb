' MenuAdd_OnTheFlyCompiler.vb

Imports System

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Partial Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            Dim defaultOption As CCHLOEObject = c.RootArray("menuHandle").GetObject("default_options")
            Dim flyBatchHandle As CCHLOEObject
            Dim flyCompilerHandle As CCHLOEObject
            Dim flyMenuHandle As CCHLOEObject

            ' Если такой handle уже существует, то удалить его
            If c.RootArray("menuHandle").ObjectExists("flyHandle") Then
                c.RootArray("menuHandle").GetObject("flyHandle").Delete()
            End If

            If c.RootArray("menuHandle").ObjectExists("flyBatchHandle") Then
                c.RootArray("menuHandle").GetObject("flyBatchHandle").Delete()
            End If

            If c.RootArray("menuHandle").ObjectExists("flyCompilerHandle") Then
                c.RootArray("menuHandle").GetObject("flyCompilerHandle").Delete()
            End If

            If c.RootArray("menuHandle").ObjectExists("flyMenuHandle") Then
                c.RootArray("menuHandle").GetObject("flyMenuHandle").Delete()
            End If

            ' Если такой пункт меню уже существует, то удалить его
            If c.RootArray("menuItem").ObjectExists("flyItem") Then
                c.RootArray("menuItem").GetObject("flyItem").Delete()
            End If

            If c.RootArray("menuItem").ObjectExists("flyBatchItem") Then
                c.RootArray("menuItem").GetObject("flyBatchItem").Delete()
            End If

            If c.RootArray("menuItem").ObjectExists("flyCompilerItem") Then
                c.RootArray("menuItem").GetObject("flyCompilerItem").Delete()
            End If

            ' Если flyMenuHandle существует, то создать и связать с меню
            If Not c.RootArray("menuHandle").ObjectExists("flyMenuHandle") Then
                flyMenuHandle = c.RootArray("menuHandle").AddObject("flyMenuHandle")
                flyMenuHandle.Parameter("name") = "Сценарии"
                flyMenuHandle.Parameter("rightRequire") = "r_admin_menu"
                flyMenuHandle.Parameter("ordering") = "40"
                flyMenuHandle.Save()

                c.LinkArray("menuHandle_menuHandle").AddLink(Guid.NewGuid().ToString(), "parent", defaultOption, "child", flyMenuHandle, True)
            End If

            ' Если flyCompilerHandle не существует, то создать связать с flyMenuHandle
            If Not c.RootArray("menuHandle").ObjectExists("flyCompilerHandle") Then
                flyCompilerHandle = c.RootArray("menuHandle").AddObject("flyCompilerHandle")
                flyCompilerHandle.Parameter("name") = "Создание"
                flyCompilerHandle.Parameter("rightRequire") = "r_admin_menu"
                flyCompilerHandle.Parameter("ordering") = "40"
                flyCompilerHandle.Save()

                flyMenuHandle = c.RootArray("menuHandle").GetObject("flyMenuHandle")
                c.LinkArray("menuHandle_menuHandle").AddLink(Guid.NewGuid().ToString(), "parent", flyMenuHandle, "child", flyCompilerHandle, True)
            End If

            ' Если flyBatchHandle не существует, то создать и связать с flyMenuHandle
            If Not c.RootArray("menuHandle").ObjectExists("flyBatchHandle") Then
                flyBatchHandle = c.RootArray("menuHandle").AddObject("flyBatchHandle")
                flyBatchHandle.Parameter("name") = "Пакетная загрузка"
                flyBatchHandle.Parameter("rightRequire") = "r_admin_menu"
                flyBatchHandle.Parameter("ordering") = "50"
                flyBatchHandle.Save()

                flyMenuHandle = c.RootArray("menuHandle").GetObject("flyMenuHandle")
                c.LinkArray("menuHandle_menuHandle").AddLink(Guid.NewGuid().ToString(), "parent", flyMenuHandle, "child", flyBatchHandle, True)
            End If

            ' Если flyCompilerItem не существует, то создать и связать с flyCompilerHandle
            If Not c.RootArray("menuItem").ObjectExists("flyCompilerItem") Then
                Dim flyCompilerItem As CCHLOEObject = c.RootArray("menuItem").AddObject("flyCompilerItem")
                flyCompilerItem.Parameter("text") = "On-the-Fly Compiler"
                flyCompilerItem.Parameter("target") = "Compiler.aspx"
                flyCompilerItem.Parameter("alt") = "On-the-Fly .NET Compiler"
                flyCompilerItem.Save()

                flyCompilerHandle = c.RootArray("menuHandle").GetObject("flyCompilerHandle")
                c.LinkArray("menuHandle_menuItem").AddLink(Guid.NewGuid().ToString(), "handle", flyCompilerHandle, "item", flyCompilerItem, True)
            End If

            ' Если flyBatchItem не существует, то создать и связать с flyBatchHandle
            If Not c.RootArray("menuItem").ObjectExists("flyBatchItem") Then
                Dim flyBatchItem As CCHLOEObject = c.RootArray("menuItem").AddObject("flyBatchItem")
                flyBatchItem.Parameter("text") = "On-the-Fly Compiler"
                flyBatchItem.Parameter("target") = "Batch.aspx"
                flyBatchItem.Parameter("alt") = "On-the-Fly .NET Compiler"
                flyBatchItem.Save()

                flyBatchHandle = c.RootArray("menuHandle").GetObject("flyBatchHandle")
                c.LinkArray("menuHandle_menuItem").AddLink(Guid.NewGuid().ToString(), "handle", flyBatchHandle, "item", flyBatchItem, True)
            End If
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. Все права защищены.