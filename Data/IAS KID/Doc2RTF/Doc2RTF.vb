' Doc2RTF.vb

Imports System
Imports System.IO
Imports System.Windows.Forms

Imports NCHLOE.Core
Imports NCHLOE.Elements

Namespace CHLOE
    Partial Class Compiler
        Public Shared Sub EntryPoint(ByVal c As CCHLOECore)
            Dim wordApp As New Word.Application
            ' ��� ������� ������� � �������� ������� npd
            For Each npd As CCHLOEObject In c.RootArray("npd").GetAllObjects()
                ' ����� ����� npd � doc_abody
                Dim role_doc As CCHLOERoleBranch = npd.Relations("npd_doc_abody", "npd")
                ' ���� ������ ������ ����� ����������
                If Not (role_doc.FirstLeaf) Is Nothing Then
                    ' ID �������, ���������������� ��������������� ����
                    Dim doc_aBody_ID As String = role_doc.FirstLeaf.GetRole().OppositeRole.RoleObjectID
                    ' ID ���� ������� ������ �����
                    Dim doc_aBody_LinkID As String = role_doc.FirstLeaf.LinkID
                    ' ������ ���� abody ���������� �� ID
                    Dim doc_aBody As CCHLOEObject = c.RootArray("aBody").GetObject(doc_aBody_ID)
                    ' �������������� ����������
                    Dim bBody As Byte() = Convert.FromBase64String(doc_aBody.Parameter("body"))
                    ' ����������� �������� filname
                    Dim doc_aBody_filename As String = doc_aBody.Parameter("filename")

                    ' ��������� ���������� %TEMP%
                    Dim varTemp As String = Environment.GetEnvironmentVariable("TEMP") + "\"
                    ' ��������� ��������� ������������ ��������-����
                    Dim tempPath As String = Environment.SpecialFolder.InternetCache.ToString()
                    ' ���� ���������� %TEMP% �� ������, �� ������������ ���
                    If Not varTemp Is Nothing Then
                        tempPath = varTemp + "\"
                    End If

                    ' ��������� ����� doc-����� ��� ��������� �� ��������� �����
                    Dim docPath As String = tempPath + Guid.NewGuid().ToString() + ".doc"

                    ' ������ ������
                    Dim stream As New FileStream(docPath, FileMode.Create)
                    Dim byteWriter As New BinaryWriter(stream)

                    byteWriter.Write(bBody)
                    byteWriter.Close()
                    stream.Close()
                    doc_aBody.Delete()

                    ' ��������� ����� rtf-����� ��� ��������� �� ��������� �����
                    Dim rtfPath As String = tempPath + Guid.NewGuid().ToString() + ".rtf"

                    ' �������� doc-����� � ��������� ��� � ������� rtf
                    Dim wordDoc As Word.Document
                    Dim wordFormat As Word.WdSaveFormat = Word.WdSaveFormat.wdFormatRTF

                    wordDoc = wordApp.Documents.Open(docPath)
                    wordDoc.SaveAs(rtfPath, wordFormat)
                    wordDoc.Close()

                    ' �������� rtf-����� � RichTextBox-������� � ��������� �� ���� ������� �����
                    Dim rtBox As RichTextBox = New RichTextBox()
                    rtBox.LoadFile(rtfPath)

                    ' �������� ������� � ������ ��� �������
                    Dim txt_aBody As CCHLOEObject
                    Dim txt_aBody_ID As String = Guid.NewGuid().ToString()
                    Dim txt_aBody_LinkID As String = Guid.NewGuid().ToString()
                    Dim txt_aBody_filename As String = Path.GetFileNameWithoutExtension(rtfPath) + ".txt"

                    ' ���� ����� ������ ��� ����������, �� �������
                    Dim role_txt As CCHLOERoleBranch = npd.Relations("npd_txt_abody", "npd")
                    If Not (role_txt.FirstLeaf) Is Nothing Then
                        txt_aBody_ID = role_txt.FirstLeaf.GetRole().OppositeRole.RoleObjectID
                        txt_aBody_LinkID = role_txt.FirstLeaf.LinkID
                        txt_aBody = c.RootArray("aBody").GetObject(txt_aBody_ID)
                        txt_aBody_filename = txt_aBody.Parameter("filename")
                        txt_aBody.Delete()
                    End If

                    ' ���������� �����
                    txt_aBody = c.RootArray("aBody").AddObject(txt_aBody_ID)
                    txt_aBody.Parameter("filename") = txt_aBody_filename
                    txt_aBody.Parameter("body") = rtBox.Text
                    txt_aBody.Save()
                    c.LinkArray("npd_txt_abody").AddLink(txt_aBody_LinkID, "npd", npd, "abody", txt_aBody, True)

                    ' ������ rtf-����� � ����������� ��� �����������

                    stream = New FileStream(rtfPath, FileMode.Open)
                    Dim byteReader As BinaryReader = New BinaryReader(stream)
                    Dim doc_base64 As Byte() = byteReader.ReadBytes(stream.Length)
                    byteReader.Close()
                    stream.Close()

                    ' ���������� �����

                    doc_aBody = c.RootArray("aBody").AddObject(doc_aBody_ID)
                    doc_aBody.Parameter("filename") = doc_aBody_filename
                    doc_aBody.Parameter("body") = Convert.ToBase64String(doc_base64)
                    doc_aBody.Save()
                    c.LinkArray("npd_doc_abody").AddLink(doc_aBody_LinkID, "npd", npd, "abody", doc_aBody, True)

                    ' �������� ������ �� ��������� �����
                    File.Delete(docPath)
                    File.Delete(rtfPath)
                End If
            Next npd
            wordApp.Application.Quit()
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. ��� ����� ��������.