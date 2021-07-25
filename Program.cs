using System.Net;
using System.Security.AccessControl;
using System;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Text;

namespace infinite
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ulong requestSent = 0; // max value = 18,446,744,073,709,551,615

            List<string> agents = new List<string>();

            agents.Add("Mozilla/5.0 (iPhone; CPU iPhone OS 13_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 [FBAN/FBIOS;FBDV/iPhone11,8;FBMD/iPhone;FBSN/iOS;FBSV/13.3.1;FBSS/2;FBID/phone;FBLC/en_US;FBOP/5;FBCR/]");
            agents.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            agents.Add("Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)");
            agents.Add("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_5) AppleWebKit/605.1.15 (KHTML, like Gecko)");
            agents.Add("Mozilla/5.0 (iPhone; CPU iPhone OS 10_0 like Mac OS X) AppleWebKit/602.1.50 (KHTML, like Gecko) Version/10.0 YaBrowser/17.4.3.195.10 Mobile/14A346 Safari/E7FBAF");
            agents.Add("Mozilla/5.0 (X11; CrOS x86_64 13597.94.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.186 Safari/537.36");
            agents.Add("Roku/DVP-9.10 (519.10E04111A)");
            agents.Add("Mozilla/5.0 (compatible; Codewisebot/2.0; +https://www.nosite.com/somebot.htm)");
            agents.Add("Mozilla/5.0 (iPhone; CPU iPhone OS 13_3_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 [FBAN/FBIOS;FBDV/iPhone10,5;FBMD/iPhone;FBSN/iOS;FBSV/13.3.1;FBSS/3;FBID/phone;FBLC/en_US;FBOP/5;FBCR/]");
            agents.Add("Mozilla/5.0 (compatible; bingbot/2.0; +https://www.bing.com/bingbot.htm)");
            agents.Add("Mozilla/5.0 (X11; Ubuntu; Linux i686; rv:24.0) Gecko/20100101 Firefox/24.0");
            agents.Add("Mozilla/5.0 (Linux; Android 5.0) AppleWebKit/537.36 (KHTML, like Gecko) Mobile Safari/537.36 (compatible; Bytespider; https://zhanzhang.toutiao.com/)");
            agents.Add("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10; rv:33.0) Gecko/20100101 Firefox/33.0");
            agents.Add("Mozilla/5.0 (Linux; Android 5.1.1; KFSUWI) AppleWebKit/537.36 (KHTML, like Gecko) Silk/81.1.233 like Chrome/81.0.4044.117 Safari/537.36");
            agents.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36");
            agents.Add("Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0)");
            agents.Add("Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1)");
            agents.Add("Mozilla/5.0 (Windows NT 10.0; Trident/7.0; rv:11.0) like Gecko");
            agents.Add("Mozilla/5.0 (Macintosh; Intel Mac OS X 11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 OPR/77.0.4054.277");
            agents.Add("Googlebot-Image/1.0");
            agents.Add("Mozilla/5.0 (Linux; Android 6.0.1; Nexus 5X Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.96 Mobile Safari/537.36 (compatible; Googlebot/2.1; +https://www.google.com/bot.html)");
            agents.Add("Mozilla/5.0 (compatible; Yahoo! Slurp; https://help.yahoo.com/help/us/ysearch/slurp)");
            agents.Add("Mozilla/5.0 (compatible; SemrushBot/7~bl; +https://www.semrush.com/bot.html)");
            
            // Use only up to 75% of the CPU.
            var parallelOptions  = new ParallelOptions
            {
                MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0))
            };
                    
            Random rnNum = new Random();
            // My original ID: 517ac;
            const string chars = "abcdefghijklmnopqrstuvwxyz";

            while(true)
            {
                Parallel.ForEach(agents, parallelOptions, (string Agent) =>
                {
                    int rnNumber = rnNum.Next(100, 999);
                    string letters = new string(Enumerable.Repeat(chars, 2).Select(s => s[rnNum.Next(s.Length)]).ToArray());
                    string lettersFB = new string(Enumerable.Repeat(chars, 5).Select(s => s[rnNum.Next(s.Length)]).ToArray());

                    string rand = rnNumber.ToString() + letters;

                    try
                    {
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://7xv.us/" + rand + "?fbclid=IwAR1I1" + lettersFB + "07qA0EFAMIlafYG1yLHnz4wXHxQ5Sl" + lettersFB + "VRBOCRPpS-2g-Y&h=AT1onvMnV3BFroHVoASb8dPQ" + lettersFB + "D6NCETqvQvlPHRQ2KxIBjiD4bFe43CXYAdXWW9I9xAY8t3dd" + lettersFB +"5_BXLmRHsax7-VKWA6RTlG45oPvP6tpllmx-ZbBZg");
                        req.Proxy = null;
                        req.UserAgent = Agent;
                        req.Timeout = 5000;
                        req.Accept = "*/*";
                        req.Method = "GET";
                        req.Headers.Clear();
                        req.ContentType = "text/html";

                        HttpWebResponse resp = (HttpWebResponse) req.GetResponse();
                        Stream dataStream = resp.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);

                        var reply = reader.ReadToEnd();
                        reader.Close();
                        resp.Close();

                        TimeSpan ts = stopWatch.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Request #: " + requestSent.ToString());

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Running time:  " + elapsedTime);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(reply + " Agent: " + Agent);
                        requestSent++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Exception: " + ex.Message);
                    }
                });
            }
        }
    }
}
