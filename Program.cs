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
            bool useProxy = false;

            if (args.Length > 0)
            {
                bool.TryParse(args[0].ToString().ToLower(), out useProxy);
            }

            List<Tuple<string, int>> proxies = new List<Tuple<string, int>>();

            if (useProxy)
            {
                string proxiesString = String.Empty;

                using (WebClient wc = new WebClient())
                {
                    Console.WriteLine("Downloading the list of proxies ...");

                    // This list is updated daily according to the github repo information.
                    proxiesString = wc.DownloadString("https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt");
                }

                List<string> cleanList = proxiesString.Split(new string[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
                
                cleanList.RemoveRange(0, 8); // Remove explenatory lines.
                cleanList.RemoveAt(cleanList.Count - 1); // Remove explenatory line.

                Console.WriteLine("Parsing the list of proxies ...");
                
                foreach(string proxy in cleanList)
                {
                    try
                    {
                        // Add only proxies which do not expose client IP address. -N are non-anonymous proxies, we don't want them in the list.
                        if (!proxy.Contains("-N"))
                        {
                            int index = proxy.IndexOf(' ', 0);

                            string item = proxy.Substring(0, index);
                            string[] splitted = item.Split(':');

                            proxies.Add(new Tuple<string, int>(splitted[0], int.Parse(splitted[1])));
                        }
                    }
                    catch {}
                }
            }

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
            agents.Add("Mozilla/5.0 (Linux; U; Android 2.3; en-us) AppleWebKit/999+ (KHTML, like Gecko) Safari/999.9");
            agents.Add("Mozilla/5.0 (Linux; U; Android 2.3.4; fr-fr; HTC Desire Build/GRJ22) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1");
            agents.Add("Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; Media Center PC 6.0; InfoPath.3; MS-RTC LM 8; Zune 4.7)");
            agents.Add("Mozilla/4.0 (compatible; MSIE 4.0; Windows 95; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
            agents.Add("Mozilla/2.0 (compatible; MSIE 3.0; Windows 3.1)");
            agents.Add("Mozilla/5.0 (Windows; U; Windows NT 6.1; tr-TR) AppleWebKit/533.20.25 (KHTML, like Gecko) Version/5.0.4 Safari/533.20.27");
            agents.Add("Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_6; zh-cn) AppleWebKit/533.20.25 (KHTML, like Gecko) Version/5.0.4 Safari/533.20.27");
            agents.Add("Opera/9.80 (X11; Linux i686; Ubuntu/14.10) Presto/2.12.388 Version/12.16.2");
            agents.Add("Opera/9.60 (Windows NT 6.0; U; bg) Presto/2.1.1");
            agents.Add("Opera/9.80 (Windows NT 5.1; U; cs) Presto/2.2.15 Version/10.10");
            agents.Add("Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");
            agents.Add("Mozilla/5.0 (Macintosh; U; PPC Mac OS X 10_5_8; ja-jp) AppleWebKit/533.20.25 (KHTML, like Gecko) Version/5.0.4 Safari/533.20.27");
            agents.Add("Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_6; en-us) AppleWebKit/533.20.25 (KHTML, like Gecko) Version/5.0.4 Safari/533.20.27");
            agents.Add("Mozilla/5.0 (Linux; Android 6.0.1; Nexus 5X Build/MMB29P) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.96 Mobile Safari/537.36 (compatible; Googlebot/2.1; +https://www.google.com/bot.html)");
            agents.Add("Mozilla/5.0 (compatible; Yahoo! Slurp; https://help.yahoo.com/help/us/ysearch/slurp)");
            agents.Add("Mozilla/5.0 (compatible; SemrushBot/7~bl; +https://www.semrush.com/bot.html)");
            agents.Add("Mozilla/5.0 (Linux; U; Android 4.0.3; ko-kr; LG-L160L Build/IML74K) AppleWebkit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30)");
            
            // Use only up to 75% of the CPU.
            var parallelOptions  = new ParallelOptions
            {
                MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0))
            };
                    
            Random rnNum = new Random();
            // My original ID: 517ac;
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            List<Tuple<string, int>> currentProxy = new List<Tuple<string, int>>();

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

                        if(useProxy)
                        {
                            if (proxies.Count > 0)
                            {
                                currentProxy.Clear();
                                currentProxy.Add(proxies[rnNum.Next(0, proxies.Count - 1)]);

                                req.Proxy = new WebProxy(currentProxy[0].Item1, currentProxy[0].Item2);
                                req.Timeout = 10000;
                            }
                            else
                            {
                                req.Proxy = null;
                                req.Timeout = 5000;
                            }
                        }
                        else
                        {
                            req.Proxy = null;
                            req.Timeout = 5000;
                        }
                        
                        req.UserAgent = Agent;
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

                        if(useProxy)
                        {
                            if (proxies.Count > 0)
                            {
                                Console.WriteLine("Reply: " + reply + Environment.NewLine + 
                                                "Proxy: " + currentProxy[0].Item1 + ":" + currentProxy[0].Item2.ToString() + Environment.NewLine + 
                                                "Agent: " + Agent);
                            }
                            else
                            {
                                Console.WriteLine("Reply: " + reply +  Environment.NewLine + "Agent: " + Agent);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Reply: " + reply +  Environment.NewLine + "Agent: " + Agent);
                        }

                        requestSent++;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.White;

                        if (useProxy)
                        {
                            proxies.RemoveAll(item => item.Item1 == currentProxy[0].Item1);
                            Console.WriteLine("Exception: " + ex.Message + Environment.NewLine + 
                                              "Removed proxy: " + currentProxy[0].Item1 + Environment.NewLine + 
                                              "Left proxies: " + proxies.Count.ToString());

                        }
                        else
                        {
                            Console.WriteLine("Exception: " + ex.Message);
                        }
                    }
                });
            }
        }
    }
}
