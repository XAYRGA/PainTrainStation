﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


    static class Helpers
    {
        public static int getUnixTime()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static void writeOut(string tag, string msg, params object[] data)
        {
            var w = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(tag);
            Console.ForegroundColor = w;
            Console.Write(": ");
            Console.WriteLine(msg, data);
        }


        public static void quickFormat(ref string text,string what, string with)
        {
            text = text.Replace(what, with);
        }


        public static void warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string writeStack(string data)
        {
            var q = Guid.NewGuid();
            var gid = q.ToString();
           
            if (!Directory.Exists("exfs"))
            {
                Directory.CreateDirectory("exfs");
            }
            File.WriteAllText("exfs/" + gid + ".stk", data);
        
            return gid;
        }
        //https://stackoverflow.com/questions/11743160/how-do-i-encode-and-decode-a-base64-string
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }

