using System;
using System.Net;

namespace MyNgrok
{
	class Program
	{
		public static void Main(string[] args)
		{
			using(var client = new WebClient()) 
			{
			    string s = client.DownloadString("http://127.0.0.1:4040/api/tunnels");
			    var forwardUrl = s.Split(':')[4] + ":" + s.Split(':')[5].Split(',')[0];
			    forwardUrl = forwardUrl.Replace("\"", "");
			    var apikey = args.Length > 0 ? args[0] : "YOUR_HARDCODED_API_KEY";
			    string ngrok = client.DownloadString("https://ngrok.bernardgabon.com/?apikey="+ apikey +"&r=" + forwardUrl);
			    Console.WriteLine(ngrok);			     			   
			    Console.ReadKey();
			}
		}
	}
}