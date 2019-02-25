using System;
using System.Collections.Generic;
using System.Configuration;

namespace SinaExpoBot.Utility
{
    public static class Variables
    {
        public static string GetValue(string key) => ConfigurationManager.AppSettings[key];

        public static string ConnectionString => ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public static string ReplyTimeout => GetValue("ReplyTimeout");
        public static string BotKey => GetValue("BotKey");
        public static string BotApi => GetValue("BotApi");
        public static string BaseUrl => GetValue("BaseUrl");

        public static readonly Dictionary<string, string> BotHeader = new Dictionary<string, string> { { "bot_key", GetValue("BotKey") } };
        public static int PlanId => Int32.Parse(GetValue("PlanId"));

        public static string ReportFolderName => "/Report/";

        public static string DownloadAddress = "~/Downloads/";


        public static string DoneImageUrl = GetValue("DoneImageUrl");

        public static List<string> ValidVideoExtensions = new List<string>() { ".mp4", ".m4a", ".m4v", ".f4v", ".f4a", ".m4b", ".m4r", ".f4b", ".mov", ".3gp", ".3gp2", ".3g2", ".3gpp", ".3gpp2", ".ogg", ".oga", ".ogv", ".ogx", ".flv", ".mkv", ".avi", ".wav", ".mpeg" };
    }
}