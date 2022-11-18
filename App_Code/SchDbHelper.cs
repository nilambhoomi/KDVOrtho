using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for SchDbHelper
/// </summary>
public class SchDbHelper
{
    SqlConnection cn = null;
    int diffmin = 30;
    public SchDbHelper()
    {
        cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
    }
    public static string DataTableToJsonObj(System.Data.DataTable dt)
    {
        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Merge(dt);
        StringBuilder JsonString = new StringBuilder();
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            JsonString.Append("[");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                JsonString.Append("{");
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (j < ds.Tables[0].Columns.Count - 1)
                    {
                        JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                    }
                    else if (j == ds.Tables[0].Columns.Count - 1)
                    {
                        JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                    }
                }
                if (i == ds.Tables[0].Rows.Count - 1)
                {
                    JsonString.Append("}");
                }
                else
                {
                    JsonString.Append("},");
                }
            }
            JsonString.Append("]");
            return JsonString.ToString();
        }
        else
        {
            return null;
        }
    }
    public DataTable GetData(string query)
    {
        try
        {
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            return new DataTable();
        }
    }
    public string GetJson(string query)
    {
        try
        {
            SqlCommand cm = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataTableToJsonObj(ds.Tables[0]);
            }
            return "[{}]";
        }
        catch (Exception ex)
        {
            return "[{}]";
        }

    }
    public string GetJson(SqlCommand cm)
    {
        try
        {
            cm.Connection =cn;
            cm.CommandText = @"GetDateAppoinment";
            cm.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cm);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataTableToJsonObj(ds.Tables[0]);
            }
            return "[{}]";
        }
        catch (Exception ex)
        {
            return "[{}]";
        }

    }
    public  string GetSampleJson()
    {
        DataTable sampleDataTable = new DataTable();

        sampleDataTable.Columns.Add("id", typeof(string));
        sampleDataTable.Columns.Add("title", typeof(string));
        sampleDataTable.Columns.Add("start", typeof(string));
        sampleDataTable.Columns.Add("end", typeof(string));
        DataRow sampleDataRow;
        sampleDataRow = sampleDataTable.NewRow();
        sampleDataRow["id"] = "F_5452_14680";
        sampleDataRow["title"] = "Celia  Arce";
        sampleDataRow["start"] = "2021-07-27T08:00:00";
        sampleDataRow["end"] = "2021-07-27T08:30:00";
        sampleDataTable.Rows.Add(sampleDataRow);
        sampleDataRow = sampleDataTable.NewRow();
        sampleDataRow["id"] = "F_5455_14681";
        sampleDataRow["title"] = "Nigel  Williams";
        sampleDataRow["start"] = "2021-07-27T09:00:00";
        sampleDataRow["end"] = "2021-07-27T09:30:00";
        sampleDataTable.Rows.Add(sampleDataRow);
        sampleDataRow = sampleDataTable.NewRow();
        sampleDataRow["id"] = "F_5459_14683";
        sampleDataRow["title"] = "Sharon  Parsons";
        sampleDataRow["start"] = "2021-07-28T08:00:00";
        sampleDataRow["end"] = "2021-07-28T08:30:00";
        sampleDataTable.Rows.Add(sampleDataRow);
        sampleDataRow = sampleDataTable.NewRow();
        sampleDataRow["id"] = "F_5525_14688";
        sampleDataRow["title"] = "Maria  Noboa";
        sampleDataRow["start"] = "2021-07-28T14:00:00";
        sampleDataRow["end"] = "2021-07-28T14:30:00";

        sampleDataTable.Rows.Add(sampleDataRow);

        return DataTableToJsonObj(sampleDataTable);
    }
    public void UpdateDate(string id,string newdate,string locationId,string note)
    {
        try
        {
            /* SqlCommand cmd=null;
             String []idpart = id.Split('_');
             if (idpart[0]=="F")
             {
                 cmd = new SqlCommand("update  tblFUPatient set FollowedUpOn=@newDate  where PatientFU_ID =@id ", cn);
             }
             else if (idpart[0] == "O")
             {
                 cmd = new SqlCommand("update  tblbpOtherPart set FollowUpInDate=@newDate  where Others_ID =@id ", cn);

             }*/
            SqlCommand cmd = new SqlCommand("update  tblAppointment set AppointmentDate=@newDate,AppointmentStart=@start,AppointmentEnd=@end,Location_Id=@locationid,AppointmentNote=@note   where AppointmentId =@id ", cn);
            DateTime d = DateTime.ParseExact(newdate, "MM/dd/yyyy hh:mm tt", null);
            cmd.Parameters.AddWithValue("@newdate", d.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@start", d.ToString("HH:mm"));
            cmd.Parameters.AddWithValue("@end", d.AddMinutes(diffmin).ToString("HH:mm"));
            cmd.Parameters.AddWithValue("@locationid", locationId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@note", note);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
           
        }
        catch (Exception ex)
        { }
       
    }
    public void Insert(string id, string lid,string newdate,string note)
    {
        try
        {
            SqlCommand cmd = new SqlCommand("Insert into tblAppointment (Patient_Id,Location_Id,AppointmentDate ,AppointmentStart,AppointmentEnd,AppointmentNote) values (@id,@lid,@newDate,@start,@end,@note)", cn);
            DateTime d = DateTime.ParseExact(newdate, "yyyy-MM-dd HH:mm", null);
            cmd.Parameters.AddWithValue("@newdate", d.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@start", d.ToString("HH:mm"));
            cmd.Parameters.AddWithValue("@end", d.AddMinutes(diffmin).ToString("HH:mm"));
            cmd.Parameters.AddWithValue("@lid", lid);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@note", note);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

        }
        catch (Exception ex)
        { }

    }
    public void Delete(string id)
    {
        try
        {
            SqlCommand cmd = new SqlCommand("Delete from tblAppointment where AppointmentId=@AppointmentId", cn);
            cmd.Parameters.AddWithValue("@AppointmentId", id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

        }
        catch (Exception ex)
        { }

    }

    public string TransferDate(string fromdate, string todate,string locationId)
    {
        try
        {
            CultureInfo culture = new CultureInfo("en-US");
            DateTime td = Convert.ToDateTime(todate, culture);
            string adate = td.ToString("yyyy-MM-dd");

            DateTime fd = Convert.ToDateTime(fromdate, culture);
            string fdate = fd.ToString("yyyy-MM-dd");

            // string query1 = "update  tblFollowUp set FollowUpDate='" + adate + "'+' '+substring(convert(varchar,cast(FollowUpDate as datetime),8),1,5)   where convert(varchar,cast(FollowUpDate as datetime),101) =@fromdate ";
            string query1 = "update  tblAppointment set AppointmentDate=@todate where AppointmentDate=@fromdate and Location_Id=@locationid ";
            SqlCommand cmd1 = new SqlCommand(query1, cn);
            cmd1.Parameters.AddWithValue("@todate", adate);
            cmd1.Parameters.AddWithValue("@fromdate", fdate);
            cmd1.Parameters.AddWithValue("@locationid", locationId);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            ////string qry = "update  tblbpOtherPart set FollowUpInDate=cast('" + todate + "'+' '+convert(varchar,FollowUpInDate ,8) as datetime)   where convert(varchar,FollowUpInDate ,101)= =@fromdate ";
            //SqlCommand cmd2 = new SqlCommand("update  tblbpOtherPart set FollowUpInDate=cast('" + todate + "'+' '+convert(varchar,FollowUpInDate ,8) as datetime)   where convert(varchar,FollowUpInDate ,101)=@fromdate", cn);
            //cmd2.Parameters.AddWithValue("@fromdate", fromdate);
            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            //da2.Fill(ds2);
            return "Transfer Made Successfully";
        }catch(Exception ex)
        {
            return "Some Error";
        }
    }
    public bool CheckDate(string AppointmentDate,int LocationId )
    {
        
        try
        {
            SqlCommand cmd1 = new SqlCommand("select * from View_Appointment where [start]=@start and Location_Id=@locationid", cn);
            cmd1.Parameters.AddWithValue("@start", AppointmentDate);
            cmd1.Parameters.AddWithValue("@locationid", LocationId);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            if (ds1.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;

        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public void setTime(string start,  string end)
    {

        try
        {
            SqlCommand cmd1 = new SqlCommand("update  tblSch_Range set SchStart=@SchStart,SchEnd=@SchEnd where id=1", cn);
            cmd1.Parameters.AddWithValue("@SchStart", start);
            cmd1.Parameters.AddWithValue("@SchEnd", end);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);

        }
        catch (Exception ex)
        {

        }
    }

}