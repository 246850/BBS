using BBS.Spider.Core;
using BBS.Spider.Crawls;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBS.Spider
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            while (true)
            {
                ICrawl crawl = new xiaohua_zol_com_cn();
                var items = crawl.Execute();
                using (var connection = GetConnection())
                {
                    int rows = connection.Execute("INSERT INTO topic(Title, AccountId, CatalogId, Contents, ThumbsUpCount, ThumbsDownCount, TrailCount, CreateTime, LastUpdateTime) VALUES (@Title, @AccountId, @CatalogId, @Contents, 0, 0, 0, SYSDATE(), SYSDATE());", items);
                    Console.WriteLine("插入条数：{0}", rows);
                }

                Thread.Sleep(60 * 60 * 1000);
            }

        }

        static IDbConnection GetConnection()
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json").Build();
            string connectionString = configuration.GetConnectionString("MySQL");
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
