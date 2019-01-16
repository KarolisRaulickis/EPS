using System;
using System.IO;

namespace TestLisen
{
    class FileHandlercs
    {
        const string FileName = "/ReadMe.txt";
        static string Header = "";
        static string FileText = "";

        public static void SetDir(string newPath)
        {  
            try
            { 
                Directory.SetCurrentDirectory(newPath);
            }
            catch (Exception e)
            {
                Header = "404 Not Found HTTP/1.1";
                FileText = "File was not found.";
                Console.WriteLine(e.Message);
                Console.WriteLine("File was not found. Appropriate message prepared.");
                return;
            } 
            TryReadFile();
        }
         
        static void TryReadFile()
        { 
            string line;
            FileText = "";  
            
            try
            {
                using (TextReader file = new StreamReader(Environment.CurrentDirectory + FileName))
                {
                    while ((line = file.ReadLine()) != null)
                    { 
                        FileText += line + "\r\n";
                    } 
                    Console.WriteLine("File was found.");
                    Console.WriteLine(FileText);

                    Header = "GET HTTP/1.1 200 OK HTTP/1.1";
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Header = "400 Bad Request HTTP/1.1";
                FileText = "An error has occurred.";
            }             
        }

        public static string GetResponseMessage()
        {  
            string request_url = Header + "\r\n" +
            "Content-Length:" + FileText.Length + "\r\n" +
            "Content-Type: text/html; charset=utf-8\r\n" +
            "\r\n\r\n" + FileText;

            return request_url;
        }


    }
}
