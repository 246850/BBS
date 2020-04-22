using System;
using System.Net;
using System.Threading.Tasks;

namespace BBS.ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            for(int i = 0; i <10; i++)
            {
                Task.Run(() => {
                    WebClient client = new WebClient();
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                    string json = client.UploadString("https://localhost:44339/Comment/ThumbsUp", "ItemId=82&IsThumb=true");
                    client.Dispose();
                    Console.WriteLine(json);
                });
            }
            Console.ReadLine();
        }
    }
}
