Imports System
Imports NCHLOE
Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            Dim root As CCHLOELinkRootArray = c.LinkArray("inspection_objects")
        End Sub
    End Class
End Namespace