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
            ' Для каждого объекта в корневом массиве npd
            For Each npd As CCHLOEObject In c.RootArray("npd").GetAllObjects()
                ' Ветвь ролей npd и doc_abody
                Dim role_doc As CCHLOERoleBranch = npd.Relations("npd_doc_abody", "npd")
                ' Если первый листок ветви существует
                If Not (role_doc.FirstLeaf) Is Nothing Then
                    ' ID объекта, соответствующего противоположной роле
                    Dim doc_aBody_ID As String = role_doc.FirstLeaf.GetRole().OppositeRole.RoleObjectID
                    ' ID сязи первого листка ветви
                    Dim doc_aBody_LinkID As String = role_doc.FirstLeaf.LinkID
                    ' Объект типа abody полученный по ID
                    Dim doc_aBody As CCHLOEObject = c.RootArray("aBody").GetObject(doc_aBody_ID)
                    ' Декодированное содержание
                    Dim bBody As Byte() = Convert.FromBase64String(doc_aBody.Parameter("body"))
                    ' Прочитанный параметр filname
                    Dim doc_aBody_filename As String = doc_aBody.Parameter("filename")

                    ' Системная переменная %TEMP%
                    Dim varTemp As String = Environment.GetEnvironmentVariable("TEMP") + "\"
                    ' Системная перменная расположения Интернет-кеша
                    Dim tempPath As String = Environment.SpecialFolder.InternetCache.ToString()
                    ' Если переменная %TEMP% не задана, то использовтаь кеш
                    If Not varTemp Is Nothing Then
                        tempPath = varTemp + "\"
                    End If

                    ' Генерация имени doc-файла для разгрузки во временную папку
                    Dim docPath As String = tempPath + Guid.NewGuid().ToString() + ".doc"

                    ' Запись данных
                    Dim stream As New FileStream(docPath, FileMode.Create)
                    Dim byteWriter As New BinaryWriter(stream)

                    byteWriter.Write(bBody)
                    byteWriter.Close()
                    stream.Close()
                    doc_aBody.Delete()

                    ' Генерация имени rtf-файла для разгрузки во временную папку
                    Dim rtfPath As String = tempPath + Guid.NewGuid().ToString() + ".rtf"

                    ' Открытие doc-файла и сохраение его в формате rtf
                    Dim wordDoc As Word.Document
                    Dim wordFormat As Word.WdSaveFormat = Word.WdSaveFormat.wdFormatRTF

                    wordDoc = wordApp.Documents.Open(docPath)
                    wordDoc.SaveAs(rtfPath, wordFormat)
                    wordDoc.Close()

                    ' Загрузка rtf-файла в RichTextBox-контрол и получение из него чистого текта
                    Dim rtBox As RichTextBox = New RichTextBox()
                    rtBox.LoadFile(rtfPath)

                    ' Создание объекта и задача его свойств
                    Dim txt_aBody As CCHLOEObject
                    Dim txt_aBody_ID As String = Guid.NewGuid().ToString()
                    Dim txt_aBody_LinkID As String = Guid.NewGuid().ToString()
                    Dim txt_aBody_filename As String = Path.GetFileNameWithoutExtension(rtfPath) + ".txt"

                    ' Если такой обеъкт уже прикреплен, то удалить
                    Dim role_txt As CCHLOERoleBranch = npd.Relations("npd_txt_abody", "npd")
                    If Not (role_txt.FirstLeaf) Is Nothing Then
                        txt_aBody_ID = role_txt.FirstLeaf.GetRole().OppositeRole.RoleObjectID
                        txt_aBody_LinkID = role_txt.FirstLeaf.LinkID
                        txt_aBody = c.RootArray("aBody").GetObject(txt_aBody_ID)
                        txt_aBody_filename = txt_aBody.Parameter("filename")
                        txt_aBody.Delete()
                    End If

                    ' Добавление связи
                    txt_aBody = c.RootArray("aBody").AddObject(txt_aBody_ID)
                    txt_aBody.Parameter("filename") = txt_aBody_filename
                    txt_aBody.Parameter("body") = rtBox.Text
                    txt_aBody.Save()
                    c.LinkArray("npd_txt_abody").AddLink(txt_aBody_LinkID, "npd", npd, "abody", txt_aBody, True)

                    ' Чтение rtf-файда и кодирование его содержимого

                    stream = New FileStream(rtfPath, FileMode.Open)
                    Dim byteReader As BinaryReader = New BinaryReader(stream)
                    Dim doc_base64 As Byte() = byteReader.ReadBytes(stream.Length)
                    byteReader.Close()
                    stream.Close()

                    ' Добавление связи

                    doc_aBody = c.RootArray("aBody").AddObject(doc_aBody_ID)
                    doc_aBody.Parameter("filename") = doc_aBody_filename
                    doc_aBody.Parameter("body") = Convert.ToBase64String(doc_base64)
                    doc_aBody.Save()
                    c.LinkArray("npd_doc_abody").AddLink(doc_aBody_LinkID, "npd", npd, "abody", doc_aBody, True)

                    ' Удаление файлов из временной папки
                    File.Delete(docPath)
                    File.Delete(rtfPath)
                End If
            Next npd
            wordApp.Application.Quit()
        End Sub
    End Class
End Namespace

' Copyright (C) 2007 ICIE. All Rights Reserved. Все права защищены.