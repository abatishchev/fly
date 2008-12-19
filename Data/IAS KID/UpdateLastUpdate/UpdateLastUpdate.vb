' UpdateLastUpdate.vb

Imports System
Imports System.Collections.Generic

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Partial Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            For Each npd As ICHLOEObject In c.RootArray("npd").GetAllObjects()
                If npd.ParameterExists("last_update") Then
                    If npd.Parameter("last_update") = "" Then
                        npd.Parameter("last_update") = DateTime.Now
                        npd.Save()
                    End If
                End If
            Next

            For Each complaint As ICHLOEObject In c.RootArray("complaint").GetAllObjects()
                If complaint.ParameterExists("last_update") Then
                    If complaint.Parameter("last_update") = "" Then
                        complaint.Parameter("last_update") = DateTime.Now
                        complaint.Save()
                    End If
                End If
            Next

            For Each inspection As ICHLOEObject In c.RootArray("inspection").GetAllObjects()
                If inspection.ParameterExists("last_update") Then
                    If inspection.Parameter("last_update") = "" Then
                        inspection.Parameter("last_update") = DateTime.Now
                        inspection.Save()
                    End If
                End If
            Next

            For Each infObject As ICHLOEObject In c.RootArray("infObject").GetAllObjects()
                If infObject.ParameterExists("last_update") Then
                    If infObject.Parameter("last_update") = "" Then
                        infObject.Parameter("last_update") = DateTime.Now
                        infObject.Save()
                    End If
                End If
            Next
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. Все права защищены.