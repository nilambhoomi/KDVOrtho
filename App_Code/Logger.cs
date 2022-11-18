using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Logger
/// </summary>
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
public static class Logger
{
    //  private static log4net.ILog Log { get; set; }
    // private static log4net.ILog DAL_Log { get; set; }
    private static log4net.ILog User_Log { get; set; }
    private static log4net.ILog BankUser_Log { get; set; }
    private static log4net.ILog Log { get; set; }
    private static log4net.ILog Auction_Log { get; set; }

    static Logger()
    {
        // Log = log4net.LogManager.GetLogger(typeof(Logger));
        //BankUser_Log = log4net.LogManager.GetLogger("BankUserLogInfo");
        User_Log = log4net.LogManager.GetLogger("log");
        //Log = log4net.LogManager.GetLogger("CommonLogInfo");
       // Auction_Log = log4net.LogManager.GetLogger("AuctionLogInfo");
    }


    ///// <summary>
    ///// Bank User Logger
    ///// </summary>
    ///// <param name="msg"></param>
    //public static void BankUserError(object msg)
    //{
    //    BankUser_Log.Error(msg);

    //}
    //public static void BankUserError(object msg, Exception ex)
    //{
    //    BankUser_Log.Error(msg, ex);
    //}



    //public static void BankUserError(Exception ex)
    //{
    //    BankUser_Log.Error(ex.Message, ex);
    //}

    //public static void BankUserInfo(object msg)
    //{
    //    BankUser_Log.Error(msg);
    //}


    /// <summary>
    /// User Logger
    /// </summary>
    /// <param name="msg"></param>      

    public static void Error(object msg)
    {
        User_Log.Error(msg);

    }
    public static void Error(object msg, Exception ex)
    {
        User_Log.Error(msg, ex);
    }

    public static void Error(Exception ex)
    {
        User_Log.Error(ex.Message, ex);
    }
    public static void Info(object msg)
    {
        User_Log.Info(msg);
    }


    ///// <summary>
    ///// Admin Logger
    ///// </summary>
    ///// <param name="msg"></param>
    //public static void Error(object msg)
    //{
    //    Log.Error(msg);

    //}
    //public static void Error(object msg, Exception ex)
    //{
    //    Log.Error(msg, ex);
    //}

    //public static void Error(Exception ex)
    //{
    //    Log.Error(ex.Message, ex);
    //}
    //public static void Info(object msg)
    //{
    //    Log.Info(msg);
    //}

    ///// <summary>
    ///// Auction Log
    ///// </summary>
    ///// <param name="msg"></param>
    //public static void AuctionLog_Error(object msg)
    //{
    //    Auction_Log.Error(msg);

    //}
    //public static void AuctionLog_Error(object msg, Exception ex)
    //{
    //    Auction_Log.Error(msg, ex);
    //}

    //public static void AuctionLog_Error(Exception ex)
    //{
    //    Auction_Log.Error(ex.Message, ex);
    //}
    //public static void AuctionLog_Info(object msg)
    //{
    //    Auction_Log.Info(msg);
    //}

}