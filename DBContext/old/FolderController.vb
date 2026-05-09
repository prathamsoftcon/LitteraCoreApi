Imports System.Web.Mvc
Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Serialization
Imports log4net
Imports System.Data.SqlClient

Namespace Controllers
    Public Class FolderController
        Inherits Controller
        Dim dm As New Datamanager
        Dim mm As New Meeting
        ' GET: Folder
        Public Function Insupd_Folder_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal folderid As String, ByVal FolderName As String, ByVal createdby As String, ByVal APIKEY As String)
            Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")

            If folderid = "" Then
                folderid = Nothing
            End If

            Dim rows As New List(Of Dictionary(Of String, Object))()
            Dim row As Dictionary(Of String, Object) = Nothing

            If dm.TRG_CHECK_TOKEN(APIKEY) = False Then
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", "You Are Using Old Version ! Please Uninstall & Re-install New Version From Play Store")
                rows.Add(row)
                Dim serializer1 As New JavaScriptSerializer
                serializer1.MaxJsonLength = Integer.MaxValue
                Return serializer1.Serialize(rows)
            End If
            Try
                Dim dtParam As New DataTable
                Dim dtXMLparam As New DataTable
                Dim dtReturnstr As New DataTable
                If dm.Insupd_Folder_Data(Domain, IsOnline, folderid, FolderName, createdby) = True Then
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Success")
                    rows.Add(row)
                Else
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", "Data not save try again")
                    rows.Add(row)
                End If


            Catch sqlex As SqlException ''''part to handle sql exceptions
                logger.Error("FolderController - Insupd_Folder_Data" + sqlex.Message)
                Try
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", dm.Show_Error_Message(Domain, IsOnline, sqlex.Message, System.Web.HttpContext.Current.Session("Choice")))
                    rows.Add(row)
                Catch exp As Exception
                    logger.Error("FolderController - Insupd_Folder_Data" + exp.Message)
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", exp.Message)
                    rows.Add(row)
                End Try
            Catch exce As System.Threading.ThreadAbortException ''to handle when page is redirect to another page(abort exception)
                logger.Error("FolderController - Insupd_Folder_Data" + exce.Message)
                'LblError.Text = exce.Message
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", exce.Message)
                rows.Add(row)

            Catch expe As System.Data.OleDb.OleDbException
                logger.Error("FolderController - Insupd_Folder_Data" + expe.Message)
                'LblError.Text = expe.Message.ToString
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", expe.Message.ToString)
                rows.Add(row)
            Catch ex As Exception
                logger.Error("FolderController - Insupd_Folder_Data" + ex.Message)
                Try
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", dm.Show_Error_Message_Front_end(Domain, IsOnline, ex.Message, System.Web.HttpContext.Current.Session("Choice")))
                    rows.Add(row)
                Catch exp As Exception
                    logger.Error("FolderController - Insupd_Folder_Data" + exp.Message)
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", exp.Message)
                    rows.Add(row)
                End Try
            End Try
            Dim serializer As New JavaScriptSerializer
            Return serializer.Serialize(rows)

        End Function


        Public Function Get_Folder_Data(ByVal Domain As String, ByVal IsOnline As String, ByVal Folderid As String, ByVal APIKEY As String) As String
            Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
            Dim sb As New StringBuilder
            Dim dsdata As DataSet
            Dim dtTableData As New DataTable
            Dim rows As New List(Of Dictionary(Of String, Object))()
            Dim row As Dictionary(Of String, Object) = Nothing

            If dm.TRG_CHECK_TOKEN(APIKEY) = False Then
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", "You Are Using Old Version ! Please Uninstall & Re-install New Version From Play Store")
                rows.Add(row)
                Dim serializer1 As New JavaScriptSerializer
                serializer1.MaxJsonLength = Integer.MaxValue
                Return serializer1.Serialize(rows)
            End If


            Try
                Dim dtColXML As DataTable

#Disable Warning BC42104 ' Variable 'dtColXML' is used before it has been assigned a value. A null reference exception could result at runtime.

#Enable Warning BC42104 ' Variable 'dtColXML' is used before it has been assigned a value. A null reference exception could result at runtime.
                dtTableData = dm.GET_Folder_Data(Domain, IsOnline, Folderid)
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Success")
                rows.Add(row)
                If dtTableData.Rows.Count > 0 Then
                    For i = 0 To dtTableData.Rows.Count - 1
                        row = New Dictionary(Of String, Object)()
                        For j = 0 To dtTableData.Columns.Count - 1
                            row.Add(dtTableData.Columns(j).ColumnName.ToString().ToLower(), dtTableData.Rows(i)(dtTableData.Columns(j).ColumnName.ToString()).ToString())
                        Next
                        rows.Add(row)
                    Next
                End If
            Catch sqlex As SqlException ''''part to handle sql exceptions
                logger.Error("FolderController - Get_Folder_Data" + sqlex.Message)
                Try
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", dm.Show_Error_Message(Domain, IsOnline, sqlex.Message, System.Web.HttpContext.Current.Session("Choice")))
                    rows.Add(row)
                Catch exp As Exception
                    logger.Error("FolderController - Get_Folder_Data" + exp.Message)
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", exp.Message)
                    rows.Add(row)
                End Try
            Catch exce As System.Threading.ThreadAbortException ''to handle when page is redirect to another page(abort exception)
                logger.Error("FolderController - Get_Folder_Data" + exce.Message)
                'LblError.Text = exce.Message
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", exce.Message)
                rows.Add(row)

            Catch expe As System.Data.OleDb.OleDbException
                logger.Error("FolderController - Get_Folder_Data" + expe.Message)
                'LblError.Text = expe.Message.ToString
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", expe.Message.ToString)
                rows.Add(row)
            Catch ex As Exception
                logger.Error("FolderController - Get_Folder_Data" + ex.Message)
                Try
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", dm.Show_Error_Message_Front_end(Domain, IsOnline, ex.Message, System.Web.HttpContext.Current.Session("Choice")))
                    rows.Add(row)
                Catch exp As Exception
                    logger.Error("FolderController - Get_Folder_Data" + exp.Message)
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", exp.Message)
                    rows.Add(row)
                End Try
            End Try
            Dim serializer As New JavaScriptSerializer
            serializer.MaxJsonLength = Integer.MaxValue
            Return serializer.Serialize(rows)
        End Function
    End Class
End Namespace