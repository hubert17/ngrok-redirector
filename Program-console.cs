using System;
using System.Threading;

class MyNgrok
{
	public static void Main(string[] args)
	{
		
		var redirectHost = "https://ngrok.bernardgabon.com";
		var secret = "YOUR_API_KEY";
		var isErr = false;
		var ngrokMsg = "";
		
		Console.WriteLine("Requesting ngrok tunnel public URL...");
		Thread.Sleep(2000);		
		do {
			try {
				using (var client = new System.Net.WebClient()) 
				{
					string s = client.DownloadString("http://127.0.0.1:4040/api/tunnels");
					var forwardUrl = "https://" + s.Split(':')[5].Split('"')[0].Replace("//", "");
					// var forwardUrl = s.Split(':')[5] + ":" + s.Split(':')[6].Split(',')[0];
					var apikey = args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]) ? args[0] : secret;
					ngrokMsg = client.DownloadString(redirectHost + "/?apikey=" + apikey + "&r=" + forwardUrl);
					Console.WriteLine(ngrokMsg);
					Thread.Sleep(3000);	
					isErr = false;
				}
			} catch 
			{
				Console.WriteLine("Ngrok tunnelling is not yet available...");
				isErr = true;
				Thread.Sleep(1000);
			}
		} while (isErr);					     			  
			
	}
}
