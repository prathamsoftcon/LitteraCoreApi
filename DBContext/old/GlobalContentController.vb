Imports System.Web.Mvc
Imports System.Net
Imports System.Web.Http
Imports System.Web.Script.Serialization
Imports log4net
Imports System.Data.SqlClient

Namespace Controllers
    Public Class GlobalContentController
        Inherits Controller
        Dim dm As New Datamanager
        Dim mm As New Meeting
        ' GET: GlobalContent
        Public Function Insupd_Content_Data(ByVal Domain As String, ByVal Isonline As String, ByVal contentid As String, ByVal contenttypeid As String, ByVal contenttitle As String, ByVal tags As String, ByVal folderid As String, ByVal contenttext As String, ByVal filepath As String, ByVal filename As String, ByVal createdby As String, ByVal fwdempid As String, ByVal branchid As String, ByVal docdate As String, ByVal createdbyempid As String, ByVal tat_id As String, ByVal status As String, ByVal remark As String, ByVal thumbnailpath As String, ByVal tcm_content_reading_time As String, ByVal APIKEY As String)
            Dim logger As ILog = log4net.LogManager.GetLogger("ErrorLog")
            If contenttext = "" Then
                contenttext = Nothing
            End If
            If filepath = "" Then
                filepath = Nothing
            End If
            If folderid = "" Then
                folderid = Nothing
            End If
            If thumbnailpath = "" Then
                thumbnailpath = Nothing
            End If

            Dim rows As New List(Of Dictionary(Of String, Object))()
            Dim row As Dictionary(Of String, Object) = Nothing

            '************Move Image to Training_Upload/Content
            If filepath <> Nothing Then
                If System.IO.File.Exists(Server.MapPath("~/Temp_Upload/" + filepath)) Then
                    System.IO.File.Move(Server.MapPath("~/Temp_Upload/" + filepath), Server.MapPath("~/Training_Upload/Content/" + filepath))
                End If
            End If
            If thumbnailpath <> "" Then
                If System.IO.File.Exists(Server.MapPath("~/Temp_Upload/" + thumbnailpath)) Then
                    System.IO.File.Move(Server.MapPath("~/Temp_Upload/" + thumbnailpath), Server.MapPath("~/Training_Upload/Content/" + thumbnailpath))
                End If
            End If


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
                If dm.Ins_Global_Content(Domain, Isonline, contentid, contenttypeid, contenttitle, tags, folderid, contenttext, filepath, filename, createdby, createdbyempid, fwdempid, branchid, docdate, createdbyempid, tat_id, status, remark, thumbnailpath, tcm_content_reading_time) = True Then
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
                logger.Error("GlobalContentController - Insupd_Content_Data" + sqlex.Message)
                Try
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", dm.Show_Error_Message(Domain, Isonline, sqlex.Message, System.Web.HttpContext.Current.Session("Choice")))
                    rows.Add(row)
                Catch exp As Exception
                    logger.Error("GlobalContentController - Insupd_Content_Data" + exp.Message)
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", exp.Message)
                    rows.Add(row)
                End Try
            Catch exce As System.Threading.ThreadAbortException ''to handle when page is redirect to another page(abort exception)
                logger.Error("GlobalContentController - Insupd_Content_Data" + exce.Message)
                'LblError.Text = exce.Message
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", exce.Message)
                rows.Add(row)

            Catch expe As System.Data.OleDb.OleDbException
                logger.Error("GlobalContentController - Insupd_Content_Data" + expe.Message)
                'LblError.Text = expe.Message.ToString
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", expe.Message.ToString)
                rows.Add(row)
            Catch ex As Exception
                logger.Error("GlobalContentController - Insupd_Content_Data" + ex.Message)
                Try
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", dm.Show_Error_Message_Front_end(Domain, Isonline, ex.Message, System.Web.HttpContext.Current.Session("Choice")))
                    rows.Add(row)
                Catch exp As Exception
                    logger.Error("GlobalContentController - Insupd_Content_Data" + exp.Message)
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", exp.Message)
                    rows.Add(row)
                End Try
            End Try
            Dim serializer As New JavaScriptSerializer
            Return serializer.Serialize(rows)

        End Function


        Public Function Get_GLOBAL_FILE_TYPE(ByVal Domain As String, ByVal IsOnline As String, ByVal APIKEY As String) As String
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
                dtTableData = dm.GET_FILE_TYPE_Data(Domain, IsOnline)
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
                logger.Error("GlobalContentController - Get_GLOBAL_FILE_TYPE" + sqlex.Message)
                Try
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", dm.Show_Error_Message(Domain, IsOnline, sqlex.Message, System.Web.HttpContext.Current.Session("Choice")))
                    rows.Add(row)
                Catch exp As Exception
                    logger.Error("GlobalContentController - Get_GLOBAL_FILE_TYPE" + exp.Message)
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", exp.Message)
                    rows.Add(row)
                End Try
            Catch exce As System.Threading.ThreadAbortException ''to handle when page is redirect to another page(abort exception)
                logger.Error("GlobalContentController - Get_GLOBAL_FILE_TYPE" + exce.Message)
                'LblError.Text = exce.Message
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", exce.Message)
                rows.Add(row)

            Catch expe As System.Data.OleDb.OleDbException
                logger.Error("GlobalContentController - Get_GLOBAL_FILE_TYPE" + expe.Message)
                'LblError.Text = expe.Message.ToString
                row = New Dictionary(Of String, Object)()
                row.Add("status", "Error")
                row.Add("msg", expe.Message.ToString)
                rows.Add(row)
            Catch ex As Exception
                logger.Error("GlobalContentController - Get_GLOBAL_FILE_TYPE" + ex.Message)
                Try
                    row = New Dictionary(Of String, Object)()
                    row.Add("status", "Error")
                    row.Add("msg", dm.Show_Error_Message_Front_end(Domain, IsOnline, ex.Message, System.Web.HttpContext.Current.Session("Choice")))
                    rows.Add(row)
                Catch exp As Exception
                    logger.Error("GlobalContentController - Get_GLOBAL_FILE_TYPE" + exp.Message)
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