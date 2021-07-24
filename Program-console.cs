using System;
using System.Net;
using System.Threading;

class MyNgrok
{
	public static void Main(string[] args)
	{
		var isErr = false;
		var ngrokMsg = "";		
		do {
			try 
			{
				using (var client = new WebClient())
				{
					string s = client.DownloadString("http://127.0.0.1:4040/api/tunnels");
					var forwardUrl = s.Split(':')[4] + ":" + s.Split(':')[5].Split(',')[0];
					forwardUrl = forwardUrl.Replace("\"", "");
					var apikey = args.Length > 0 ? args[0] : "YOUR_HARDCODED_API_KEY";
					ngrokMsg = client.DownloadString("https://ngrok.bernardgabon.com/?apikey=" + apikey + "&r=" + forwardUrl);
					isErr = false;
				}
			} catch 
			{
				Console.WriteLine("Ngrok tunnelling is not yet available...");
				isErr = true;
			 	Thread.Sleep(1000);
			}
		} while (isErr);
			
		Console.WriteLine(ngrokMsg);			     			   
		Thread.Sleep(5000);		
	}
}
