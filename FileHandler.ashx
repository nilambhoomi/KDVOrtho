<%@ WebHandler Language="C#" Class="FileHandler" %>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;

public class FileHandler : IHttpHandler
{
    DBHelperClass db = new DBHelperClass();
    public void ProcessRequest(HttpContext context)
    {
        string method = context.Request.QueryString["method"].ToString();
        switch (method.ToLower())
        {
            case "deletefilefromdirectory":
                DeleteFileFromDirectory(context);
                break;
            case "savefiletodirectory":
                SaveFileToDirectory(context);
                break;
        }
        context.Response.ContentType = "text/plain";
    }
    void SaveFileToDirectory(HttpContext context)
    {
        if (context.Request.Files.Count > 0)
        {
            for (int i = 0; i <= context.Request.Files.Count - 1; i++)
            {
                var file = context.Request.Files[i];

                string path = "", IEID = "0", FUID = "0", fileName = "";
                bool _isNew = true;
                string type = context.Request.QueryString["type"];

                IEID = context.Request.QueryString["IEID"];
                FUID = context.Request.QueryString["FUID"];



                DataTable dt = db.selectDatatable("select * from tblbpOtherPartRecording where PatientIE_ID=" + IEID);
                if (dt != null && dt.Rows.Count > 0)
                    _isNew = false;

                if (type.ToLower() == "cc")
                    path = @"OtherParts\CC\";
                else if (type.ToLower() == "pe")
                    path = @"OtherParts\PE\";
                else if (type.ToLower() == "ad")
                    path = @"OtherParts\ASSESMENT\";
                else if (type.ToLower() == "plan")
                    path = @"OtherParts\PLAN\";


                FileInfo objFileInfo = new FileInfo(HttpContext.Current.Server.MapPath("~/RecordingFiles/" + path + file.FileName));

                var bufferSize = file.ContentLength;
                byte[] buffer = new byte[file.ContentLength];

                string Extension = string.Empty;
                if (!string.IsNullOrEmpty(objFileInfo.Extension))
                {
                    Extension = objFileInfo.Extension;
                }
                else if (!string.IsNullOrEmpty(file.ContentType) && file.ContentType.ToLower().Equals("image/png"))
                {
                    Extension = ".png";
                }
                else if (!string.IsNullOrEmpty(file.ContentType) && file.ContentType.ToLower().Equals("audio/ogg"))
                {
                    Extension = ".wav";
                }
                else if (!string.IsNullOrEmpty(file.ContentType) && file.ContentType.ToLower().Equals("audio/mp3"))
                {
                    Extension = ".mp3";
                }

                if (IEID != "0")
                    fileName = IEID + "_" + type + "_" + context.Request.QueryString["filename"] + Extension;
                else
                    fileName = FUID + "_" + type + "_" + context.Request.QueryString["filename"] + Extension;

                using (Stream readStream = file.InputStream)
                {
                    using (Stream streamWriter = new StreamWriter(objFileInfo.DirectoryName + "/" + fileName).BaseStream)
                    {

                        while (readStream.Position < readStream.Length)
                        {
                            buffer.Initialize();

                            // Reading stream
                            int bytesRead = readStream.Read(buffer, 0, bufferSize);
                            // Writing stream
                            //writeStream.Write(buffer, 0, bytesRead);
                            streamWriter.Write(buffer, 0, bytesRead);
                        }

                        // flushing stream.
                        streamWriter.Flush();
                    }
                }


                if (type.ToLower() == "cc")
                {
                    if (_isNew)
                        this.executeQuery("insert into tblbpOtherPartRecording(PatientIE_ID,OthersCC)values(" + IEID + ",'" + fileName + "')");
                    else
                        this.executeQuery("update tblbpOtherPartRecording set OthersCC='" + fileName + "' where PatientIE_ID=" + IEID);
                }
                else if (type.ToLower() == "pe")
                {
                    if (_isNew)
                        this.executeQuery("insert into tblbpOtherPartRecording(PatientIE_ID,OthersPE)values(" + IEID + ",'" + fileName + "')");
                    else
                        this.executeQuery("update tblbpOtherPartRecording set OthersPE='" + fileName + "' where PatientIE_ID=" + IEID);
                }
                else if (type.ToLower() == "ad")
                {
                    if (_isNew)
                        this.executeQuery("insert into tblbpOtherPartRecording(PatientIE_ID,OthersA)values(" + IEID + ",'" + fileName + "')");
                    else
                        this.executeQuery("update tblbpOtherPartRecording set OthersA='" + fileName + "' where PatientIE_ID=" + IEID);
                }
                else if (type.ToLower() == "plan")
                {
                    if (_isNew)
                        this.executeQuery("insert into tblbpOtherPartRecording(PatientIE_ID,OthersP)values(" + IEID + ",'" + fileName + "')");
                    else
                        this.executeQuery("update tblbpOtherPartRecording set OthersP='" + fileName + "' where PatientIE_ID=" + IEID);
                }

                //StreamWriter streamWriter = new StreamWriter(objFileInfo.FullName);
                //streamWriter.Write(true);
            }
            context.Response.Write("Uploaded");
        }
        else
        {
            context.Response.Write("File not found for uploading.");
        }

    }

    void DeleteFileFromDirectory(HttpContext context)
    {
        string fileName = context.Request.QueryString["p"].ToString();
        string uniqueFileName = context.Request.QueryString["uniqueFileName"].ToString();
        var fileArray = fileName.Split('/');
        int eventId = 0;

        if (fileArray.Count() > 1) // If file Array contains eventId tehn deleting file from event folder.
        {
            FileInfo objFileInfo = new FileInfo(HttpContext.Current.Server.MapPath("~/UploadFiles/" + fileName));
            objFileInfo.Delete();
            eventId = Convert.ToInt32(fileName.Split('/')[0]);

        }
        else // Deleting file from neweventimages if document do not exists in event folder.
        {
            FileInfo objFile1 = new FileInfo(HttpContext.Current.Server.MapPath("~/UploadFiles/" + fileName));
            FileInfo objFileInfo = new FileInfo(HttpContext.Current.Server.MapPath("~/UploadFiles/" + uniqueFileName + objFile1.Extension));
            if (objFileInfo.Exists)
                objFileInfo.Delete();
        }


        HttpContext.Current.Response.Write("Deleted");

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public void executeQuery(string query)
    {
        try
        {
            DBHelperClass db = new DBHelperClass();
            db.executeQuery(query);
        }
        catch (Exception ex)
        {
        }
    }



}