using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using TwoCaptcha.Captcha;

public class Token
{
    public string token;
    public string mail;
    public string password;

    public Token(string token, string mail, string password)
    {
        this.token = token;
        this.mail = mail;
        this.password = password;

    }
}
public class TokenWithResponseList
{
    public List<TokenWithResponse> rp = new List<TokenWithResponse>();
}
public class TokenWithResponse
{
    public string messageID;
    public enum ResponseType { InvalidToken, Liked, CantLike, Processing };
    public ResponseType status;
    public string token;
    public string emoji;


    public TokenWithResponse(string token, string messageID, string emoji)
    {
        this.token = token;
        this.messageID = messageID;
    }
}
public class UserWithMessage
{

    public ulong userID
    {
        get;
        set;
    }

    public Emoji emoji = null;

    public RestUserMessage message = null;

    public SocketTextChannel channel = null;
    public bool isAwaiting = false;


    public ulong messageID = 0;
    public bool endOfProcess = false;

    public string countText = null;
    public int countNum = -1;
}
internal static class Program1
{
    public static DiscordSocketClient _client;
    public static UserWithMessage userProcess;
    private static TokenWithResponseList TG = new();
    public static bool isWorking = false;
    public static int numOfFiles = 0;
    public static List<Token> tokens = new List<Token>();


    public static TokenWithResponseList tokenResponseList { get => TG; set => TG=value; }

    public static void Main()
    {
        Console.WriteLine(Environment.CurrentDirectory);
        var tp = new TokenWithResponseList();
        tp.rp.Add(new TokenWithResponse("0", "0", "0"));
        tokenResponseList =tp;

        #region TokensWithAccounts
        //  var list = File.ReadAllLines(Environment.CurrentDirectory + @"\nTokens.txt");
        //
        //  foreach(var line in list)
        //  {
        //      var tkstring = line.Split(':');
        //      var password = tkstring[0].Replace(":", "").Trim();
        //
        //      var mail = tkstring[1].Replace(":", "").Trim();
        //
        //      var token = tkstring[2].Replace(":", "").Trim();
        //
        //      tokens.Add(new Token(token, mail, password));
        //  }
        // 
        //
        //  foreach(var token in tokens)
        //  {
        //
        //      Console.WriteLine(token.token);
        //  }
        #endregion

        MainAsync().Wait();



    }
    static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
    public static async Task MainAsync()
    {

        _client = new DiscordSocketClient();

        _client.Log += Log;

        var cfg = new DiscordSocketConfig();



        var bottoken = "";
        await _client.LoginAsync(TokenType.Bot, bottoken);
        await _client.StartAsync();


        await _client.SetGameAsync("Developed by Linda Mosep!", "https://github.com/LindaMosep", ActivityType.Playing);

        _client.Ready += _client_Ready;
        _client.MessageReceived += MessageTask;
        _client.ReactionAdded += ReactionAdded;
        await Task.Delay(-1);
    }
    private static async Task _client_Ready()
    {
        Console.WriteLine("Signed to "+ _client.CurrentUser.Username + "#" + _client.CurrentUser.DiscriminatorValue);

        var tokens = File.ReadAllLines(Environment.CurrentDirectory + @"\tokens.txt").ToList();
        var ips = File.ReadAllLines(Environment.CurrentDirectory + @"\ips.txt").ToList();
        Console.WriteLine(tokens.Count);

        var randoms = File.ReadAllLines(Environment.CurrentDirectory + @"\randoms.txt");
        Console.WriteLine("Completed");
        int m = 0;
        int cc = 0;

        int keke = 0;

        //JoinStart();
        // VerifyStart();
        // CheckGuildStart();
       // CheckChannelsStart();
        await _client.SetStatusAsync(UserStatus.Offline);


    }

    #region ChangeUserNameForServer
    public static async Task ChangeUserNameForServer(string username, string guildID, string token, string proxySt)
    {
        Console.WriteLine("Started");
        var url = $"https://discord.com/api/v9/guilds/{guildID}/members/@me";
        RestClient client = new RestClient();
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PATCH";
        request.Headers.Add("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6InRyLVRSIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzk3LjAuNDY5Mi45OSBTYWZhcmkvNTM3LjM2IiwiYnJvd3Nlcl92ZXJzaW9uIjoiOTcuMC40NjkyLjk5Iiwib3NfdmVyc2lvbiI6IjEwIiwicmVmZXJyZXIiOiJodHRwczovL3d3dy5nb29nbGUuY29tLyIsInJlZmVycmluZ19kb21haW4iOiJ3d3cuZ29vZ2xlLmNvbSIsInNlYXJjaF9lbmdpbmUiOiJnb29nbGUiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6MTEyODI0LCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==");
        request.Headers.Add("Authorization", token);
        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";
        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        {
            string json = "{\"nick\": \""+username+"\"}";

            streamWriter.Write(json);
        }
        IWebProxy proxy = WebRequest.DefaultWebProxy;
        proxy.Credentials = CredentialCache.DefaultCredentials;
        var sp = proxySt.Split(':');
        string proxyUser = sp[2].Replace(":", "").Trim();
        string proxyPass = sp[3].Replace(":", "").Trim();
        WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

        request.Proxy = proxy;
    }
    #endregion

    #region ChannelChecker
    public static async Task CheckChannelsStart()
    {
        Console.WriteLine("Started");
        var ips = File.ReadAllLines(Environment.CurrentDirectory + @"\ips.txt").ToList();
        var tokens = File.ReadAllLines(Environment.CurrentDirectory + @"\tokens.txt").ToList();
        var ipstet = 0;
        for (int i = 0; i < tokens.Count; i++)
        {



            if (tokens.Count > ips.Count)
            {
                var divide = tokens.Count / ips.Count;
                if (i % divide == 0)
                {
                    ipstet++;
                }
            }
            else
            {
                ipstet = i;
            }


            CheckChannels(tokens[i], ips[i], "935037674513903686");

            await Task.Delay(50);

        }

    }
    public static async Task CheckChannels(string token, string proxySt, string channelID)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://discord.com/api/v9/channels/" + channelID);
        request.Headers.Add("Authorization", token);
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");
        IWebProxy proxy = WebRequest.DefaultWebProxy;
        request.Method = "GET";
        proxy.Credentials = CredentialCache.DefaultCredentials;
        var sp = proxySt.Split(':');
        string proxyUser = sp[2].Replace(":", "").Trim();
        string proxyPass = sp[3].Replace(":", "").Trim();
        WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

        request.Proxy = proxy;

        try
        {
            var rp = await request.GetResponseAsync();
            var txt = new StreamReader(rp.GetResponseStream()).ReadToEnd().Trim();

        

            if (!txt.Contains(channelID))
            {
                Console.WriteLine(token);
            }
            else
            {

            }
        }
        catch (Exception ex)
        {



            Console.WriteLine(token);


            // Console.WriteLine("Reason: " + ex.Message);
            // Console.WriteLine("============");

        }

    }

    #endregion
    
    #region Verify
    public static async Task Verify(string token, string proxySt, string i)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v9/interactions");
        request.Headers.Add("Authorization", token);
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");
        request.Headers.Add("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6InRyLVRSIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzk3LjAuNDY5Mi45OSBTYWZhcmkvNTM3LjM2IiwiYnJvd3Nlcl92ZXJzaW9uIjoiOTcuMC40NjkyLjk5Iiwib3NfdmVyc2lvbiI6IjEwIiwicmVmZXJyZXIiOiJodHRwczovL3d3dy5nb29nbGUuY29tLyIsInJlZmVycmluZ19kb21haW4iOiJ3d3cuZ29vZ2xlLmNvbSIsInNlYXJjaF9lbmdpbmUiOiJnb29nbGUiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6MTEyODMzLCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==");

        request.Headers.Add("Content-Type", "application/json");
        var body = "{    \"type\": 3,    \"nonce\": \"940945362087051264\",    \"guild_id\": \"915303139949838416\",    \"channel_id\": \"915315912968663050\",    \"message_flags\": 4,    \"message_id\": \"940909134931451944\",    \"application_id\": \"155149108183695360\",    \"session_id\": \"e428f42f666847a79053cebc0c54cae8\",    \"data\": {        \"component_type\": 2,        \"custom_id\": \"rr:btn:0\"    }}";
        request.Method = "POST";




        byte[] bytes = Encoding.UTF8.GetBytes(body);
        Stream stream = request.GetRequestStream();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
       

        IWebProxy proxy = WebRequest.DefaultWebProxy;
        proxy.Credentials = CredentialCache.DefaultCredentials;
        var sp = proxySt.Split(':');
        string proxyUser = sp[2].Replace(":", "").Trim();
        string proxyPass = sp[3].Replace(":", "").Trim();
        WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);
   
        request.Proxy = proxy;
        try
        {
           var rp =  await request.GetResponseAsync();

            Console.WriteLine(i);
          
            // Liked for token: " + token);
            // Console.WriteLine("============");
        }
        catch (Exception ex)
        {
            // Console.WriteLine("Reason: " + ex.Message);
            // Console.WriteLine("============");

        }


    }
    public static async Task VerifyStart()
    {
        Console.WriteLine("Started");
        var ips = File.ReadAllLines(Environment.CurrentDirectory + @"\ips.txt").ToList();
        var tokens = File.ReadAllLines(Environment.CurrentDirectory + @"\tokens.txt").ToList();
        var ipstet = 0;
        for (int i = 0; i < tokens.Count; i++)
        {



            if (tokens.Count > ips.Count)
            {
                var divide = tokens.Count / ips.Count;
                if (i % divide == 0)
                {
                    ipstet++;
                }
            }
            else
            {
                ipstet = i;
            }


            Verify(tokens[i], ips[i], i.ToString());

            await Task.Delay(50);

        }
    }
    #endregion

    #region CheckGuilds
    public static async Task CheckGuildStart()
    {
        Console.WriteLine("Started");
        var ips = File.ReadAllLines(Environment.CurrentDirectory + @"\ips.txt").ToList();
        var tokens = File.ReadAllLines(Environment.CurrentDirectory + @"\tokens.txt").ToList();
        var ipstet = 0;
        for (int i = 0; i < tokens.Count; i++)
        {



            if (tokens.Count > ips.Count)
            {
                var divide = tokens.Count / ips.Count;
                if (i % divide == 0)
                {
                    ipstet++;
                }
            }
            else
            {
                ipstet = i;
            }


            CheckGuilds(tokens[i], ips[i]);

            await Task.Delay(50);

        }
    }
    public static async Task CheckGuilds(string token, string proxySt)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v9/users/@me/guilds");
        request.Headers.Add("Authorization", token);
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");
        IWebProxy proxy = WebRequest.DefaultWebProxy;
        request.Method = "GET";
        proxy.Credentials = CredentialCache.DefaultCredentials;
        var sp = proxySt.Split(':');
        string proxyUser = sp[2].Replace(":", "").Trim();
        string proxyPass = sp[3].Replace(":", "").Trim();
        WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

        request.Proxy = proxy;

        try
        {
            var rp = await request.GetResponseAsync();
            var txt = new StreamReader(rp.GetResponseStream()).ReadToEnd().Trim();



            if (!txt.Contains("Space"))
            {
                Console.WriteLine(token);
            }
            else
            {

            }
        }
        catch (Exception ex)
        {



            Console.WriteLine("*" + token);


            // Console.WriteLine("Reason: " + ex.Message);
            // Console.WriteLine("============");

        }

    }
    #endregion

    #region Join
    public static async Task JoinStart()
    {
        Console.WriteLine("Started");
        var ips = File.ReadAllLines(Environment.CurrentDirectory + @"\ips.txt").ToList();
        var tokens = File.ReadAllLines(Environment.CurrentDirectory + @"\tokens.txt").ToList();
        var ipstet = 0;
        for (int i = 0; i < tokens.Count; i++)
        {



            if (tokens.Count > ips.Count)
            {
                var divide = tokens.Count / ips.Count;
                if (i % divide == 0)
                {
                    ipstet++;
                }
            }
            else
            {
                ipstet = i;
            }


            join(tokens[i], "spacedoge", ips[i],i.ToString());

            await Task.Delay(100);

        }
    }
    public static async Task join(string token, string url, string proxySt, string i)
    {

        #region Proxy

        var sp = proxySt.Split(':');

        string proxyUser = sp[2].Replace(":", "").Trim();
        string proxyPass = sp[3].Replace(":", "").Trim();


        WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim());

        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

        #endregion

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://discord.com/api/v9/invites/WXQ8dFS7");
        request.Method = "POST";

        request.Headers.Add("authorization", token.Trim());
        request.UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.87 Mobile Safari/537.36";
        request.Headers.Add("x-context-properties", "eyJsb2NhdGlvbiI6IkpvaW4gR3VpbGQiLCJsb2NhdGlvbl9ndWlsZF9pZCI6IjkxNTMwMzEzOTk0OTgzODQxNiIsImxvY2F0aW9uX2NoYW5uZWxfaWQiOiI5MTUzMTU5MTI5Njg2NjMwNTAiLCJsb2NhdGlvbl9jaGFubmVsX3R5cGUiOjB9");
        request.Headers.Add("accept-language", "tr-TR,tr;q=0.9,en-US;q=0.8,en;q=0.7");
        request.Headers.Add("accept", "*/*");
        request.Headers.Add("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6InRyLVRSIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzk4LjAuNDc1OC44NyBTYWZhcmkvNTM3LjM2IiwiYnJvd3Nlcl92ZXJzaW9uIjoiOTguMC40NzU4Ljg3Iiwib3NfdmVyc2lvbiI6IjEwIiwicmVmZXJyZXIiOiIiLCJyZWZlcnJpbmdfZG9tYWluIjoiIiwicmVmZXJyZXJfY3VycmVudCI6IiIsInJlZmVycmluZ19kb21haW5fY3VycmVudCI6IiIsInJlbGVhc2VfY2hhbm5lbCI6InN0YWJsZSIsImNsaWVudF9idWlsZF9udW1iZXIiOjExNDQwNywiY2xpZW50X2V2ZW50X3NvdXJjZSI6bnVsbH0=");
        request.Headers.Add("cookie", "__dcfduid=0214d34088ff11ecbefb770fdfec52a9; __sdcfduid=0214d34188ff11ecbefb770fdfec52a996964547316afd72d42cd8897bb667ee6582bcab026f769a38119512a9fe1cd8; OptanonConsent=isIABGlobal=false&datestamp=Fri+Feb+11+2022+18%3A47%3A57+GMT%2B0300+(GMT%2B03%3A00)&version=6.17.0&hosts=&landingPath=NotLandingPage&groups=C0001%3A1%2CC0002%3A1%2CC0003%3A1&AwaitingReconsent=false; __cf_bm=wwIrLJUDJ2OBgOXwuMrtVWqUgZ.XQEhA0eQfW5Oh5HI-1644595005-0-ARFu7k92aHo45vPzfuJtk13y6awozNwe7NyrkZqL8WTUscXFwt2gJZH+vBbQrmz0iXBTvtAZarxadNCENzEHKmQQERMjNuN9QGPuAnG9R27+lYLyQ6HNqBk5v3MQUEoOMg==; locale=en-US");
        request.Referer = "https://discord.com/channels/@me";
        request.ContentType = "application/json";


        request.Proxy = myproxy;

        TwoCaptcha.TwoCaptcha solver = new TwoCaptcha.TwoCaptcha("b9d54eedc012d87e816a77617326b4cd");
        HCaptcha captcha = new HCaptcha();


        captcha.SetSiteKey("4c672d35-0701-42b2-88c3-78380b0db560");
        captcha.SetUrl("https://discord.com/channels/@me");
        await solver.Solve(captcha);

        var body = "{\"captcha_key\":\""+captcha.Code+"\"}";

        byte[] bytes = Encoding.UTF8.GetBytes(body);
        Stream stream = request.GetRequestStream();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();

        try
        {
            var rp = await request.GetResponseAsync();
            if (new StreamReader(rp.GetResponseStream()).ReadToEnd().ToLower().Contains("proxy"))
            {

            }
            else
            {
                Console.WriteLine(i);

            }


        }
        catch (Exception ex)
        {


        }



    }
    #endregion

    #region Leave
    public static async Task leave(string token, string guildID, string proxySt)
    {

        var url = "https://discord.com/api/v9/users/@me/guilds/214409644913852416";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add("Authorization", token);
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");

        request.Headers.Add("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6InRyLVRSIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzk3LjAuNDY5Mi45OSBTYWZhcmkvNTM3LjM2IiwiYnJvd3Nlcl92ZXJzaW9uIjoiOTcuMC40NjkyLjk5Iiwib3NfdmVyc2lvbiI6IjEwIiwicmVmZXJyZXIiOiJodHRwczovL3d3dy5nb29nbGUuY29tLyIsInJlZmVycmluZ19kb21haW4iOiJ3d3cuZ29vZ2xlLmNvbSIsInNlYXJjaF9lbmdpbmUiOiJnb29nbGUiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6MTEyODMzLCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==");


        request.Method = "DELETE";
        IWebProxy proxy = WebRequest.DefaultWebProxy;

        proxy.Credentials = CredentialCache.DefaultCredentials;
        var sp = proxySt.Split(':');
        string proxyUser = sp[2].Replace(":", "").Trim();
        string proxyPass = sp[3].Replace(":", "").Trim();
        WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

        request.Proxy = proxy;

        try
        {
            await request.GetResponseAsync();
            Console.WriteLine("Leaved");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    #endregion

    #region SendMessage
    private static async Task SendMessage(string channelID, string token, string proxySt, string randNonce)
    {

        Console.WriteLine("Started");
        var client = new RestClient($"https://discord.com/api/v9/channels/{channelID}/messages");

        var url = $"https://discord.com/api/v9/channels/357869756625190915/messages";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Headers.Add("Authorization", token);
        request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36");
        request.Headers.Add("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6InRyLVRSIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzk3LjAuNDY5Mi45OSBTYWZhcmkvNTM3LjM2IiwiYnJvd3Nlcl92ZXJzaW9uIjoiOTcuMC40NjkyLjk5Iiwib3NfdmVyc2lvbiI6IjEwIiwicmVmZXJyZXIiOiJodHRwczovL3d3dy5nb29nbGUuY29tLyIsInJlZmVycmluZ19kb21haW4iOiJ3d3cuZ29vZ2xlLmNvbSIsInNlYXJjaF9lbmdpbmUiOiJnb29nbGUiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6MTEyODMzLCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==");

        request.Headers.Add("Content-Type", "application/json");
        var body = "{\"content\":\"Sevgilin gerçek değil. <@!214408099505766404>\",\"nonce\":\"xx9931\",\"tts\":false}";
        request.Method = "POST";
        IWebProxy proxy = WebRequest.DefaultWebProxy;
        body = body.Replace("xx9931", randNonce.Trim());
        proxy.Credentials = CredentialCache.DefaultCredentials;
        var sp = proxySt.Split(':');
        string proxyUser = sp[2].Replace(":", "").Trim();
        string proxyPass = sp[3].Replace(":", "").Trim();
        WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);

        request.Proxy = proxy;
        byte[] bytes = Encoding.UTF8.GetBytes(body);
        Stream stream = request.GetRequestStream();
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        try
        {
            await request.GetResponseAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }



    }
    #endregion

    #region React
    public static async Task React(string channelID, string messageID, string emoji, string token, string proxySt, TokenWithResponse tp)
    {

        var url = $"https://discord.com/api/v9/channels/{channelID}/messages/{messageID}/reactions/{emoji}/%40me";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "PUT";
        request.Headers.Add("x-super-properties", "eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6InRyLVRSIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzk3LjAuNDY5Mi45OSBTYWZhcmkvNTM3LjM2IiwiYnJvd3Nlcl92ZXJzaW9uIjoiOTcuMC40NjkyLjk5Iiwib3NfdmVyc2lvbiI6IjEwIiwicmVmZXJyZXIiOiJodHRwczovL3d3dy5nb29nbGUuY29tLyIsInJlZmVycmluZ19kb21haW4iOiJ3d3cuZ29vZ2xlLmNvbSIsInNlYXJjaF9lbmdpbmUiOiJnb29nbGUiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6MTEyODI0LCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==");
        request.Headers.Add("Authorization", token.Trim());
        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36";

        IWebProxy proxy = WebRequest.DefaultWebProxy;
        proxy.Credentials = CredentialCache.DefaultCredentials;
        var sp = proxySt.Split(':');
        string proxyUser = sp[2].Replace(":", "").Trim();
        string proxyPass = sp[3].Replace(":", "").Trim();
        WebProxy myproxy = new WebProxy(sp[0].Replace(":", "").Trim() + ":" + sp[1].Replace(":", "").Trim(), true);
        myproxy.Credentials= new NetworkCredential(proxyUser, proxyPass);
  
        request.Proxy = myproxy;

        try
        {
            var rp = await request.GetResponseAsync();

       

            // Liked for token: " + token);
            // Console.WriteLine("============");
        }
        catch (WebException ex)
        {
            Console.WriteLine(proxySt);





            // Console.WriteLine("Reason: " + ex.Message);
            // Console.WriteLine("============");

        }


    }
    private static async Task MessageRecieved(SocketMessage msg)
    {
        if (!msg.Author.IsBot)
        {
            if (userProcess != null)
            {
                if (userProcess.message != null)
                {

                }
            }
            if (msg.Channel.Id == 0)
            {
                if (userProcess != null)
                {
                    if (msg.Content == "!killprocess")
                    {
                        await userProcess.message.ModifyAsync(m => m.Content = "**I finished process**. Thank you for using me :( (This message will be vanished in 15 second.)" + MentionUtils.MentionUser(msg.Author.Id));
                        MessageKiller(userProcess.message, 15000);
                        userProcess = null;
                    }
                    if (userProcess != null)
                    {

                        if (!userProcess.isAwaiting)
                        {

                            if (userProcess.message != null)
                            {

                                await userProcess.message.UpdateAsync();

                                if (userProcess.message == null)
                                {

                                    userProcess = null;

                                }
                                else
                                {
                                    if (msg.Reference != null)
                                    {


                                        if (msg.Reference.MessageId.Value == userProcess.message.Id)
                                        {

                                            if (userProcess.channel == null)
                                            {

                                                if (msg.MentionedChannels.Count > 0)
                                                {

                                                    if (msg.MentionedChannels.ToList()[0] as SocketTextChannel != null)
                                                    {


                                                        userProcess.channel = msg.MentionedChannels.ToList()[0] as SocketTextChannel;




                                                        await userProcess.message.UpdateAsync();

                                                        msg.DeleteAsync();
                                                        await userProcess.message.ModifyAsync(m => m.Content ="I set channel to " + MentionUtils.MentionChannel(msg.MentionedChannels.ToList()[0].Id) + ". Now tell me the message ID.");

                                                        var no = new Emoji("❌");
                                                        userProcess.message.AddReactionAsync(no);
                                                        await userProcess.message.UpdateAsync();
                                                    }
                                                    else
                                                    {
                                                        msg.DeleteAsync();
                                                        await userProcess.message.ModifyAsync(m => m.Content ="You didn't give me a channel. **Process finished**.");
                                                        userProcess = null;

                                                    }




                                                }
                                                else
                                                {
                                                    msg.DeleteAsync();
                                                    await userProcess.message.ModifyAsync(m => m.Content ="You didn't give me a valid text channel. **Process finished**.");
                                                    userProcess = null;

                                                }


                                            }
                                            else
                                            {
                                                if (msg.Reference != null)
                                                {
                                                    if (msg.Reference.MessageId.Value == userProcess.message.Id)
                                                    {
                                                        if (userProcess.messageID == 0)
                                                        {
                                                            ulong id = 0;
                                                            if (ulong.TryParse(msg.Content.Trim(), out id))
                                                            {
                                                                var message = await userProcess.channel.GetMessageAsync(id);

                                                                if (message != null)
                                                                {
                                                                    msg.DeleteAsync();
                                                                    var msgUrl = $"https://discord.com/channels/{userProcess.channel.Guild.Id}/{userProcess.channel.Id}/{message.Id}";
                                                                    await userProcess.message.ModifyAsync(m => m.Content = "I set message. You can go message with this URL: **" + msgUrl +"**\n Now tell me the emoji you want to add or react.");
                                                                    userProcess.messageID = id;


                                                                    if (userProcess.message != null)
                                                                    {


                                                                    }

                                                                    await userProcess.message.UpdateAsync();

                                                                    var no = new Emoji("❌");
                                                                    userProcess.message.AddReactionAsync(no);
                                                                }
                                                                else
                                                                {
                                                                    msg.DeleteAsync();
                                                                    await userProcess.message.ModifyAsync(m => m.Content = "You provided wrong message ID or I can't see that channel. **Process finished**.");
                                                                    userProcess = null;

                                                                }
                                                            }
                                                            else
                                                            {
                                                                msg.DeleteAsync();
                                                                await userProcess.message.ModifyAsync(m => m.Content = "This is not a number. **Process finished**.");
                                                                userProcess = null;

                                                            }


                                                        }
                                                        else
                                                        {
                                                            if (userProcess.emoji != null)
                                                            {
                                                                int count = -1;

                                                                if (int.TryParse(msg.Content.Trim(), out count))
                                                                {
                                                                    if (count > -1)
                                                                    {



                                                                        var embed = new EmbedBuilder();
                                                                        embed.Title = "**You okay with this settings?**";
                                                                        var yes = new Emoji("✅");
                                                                        var no = new Emoji("❌");
                                                                        embed.Description = "If you okay with this settings, react to " + yes + ". And if you are not okay react to " + no;
                                                                        embed.AddField("**Channel**", MentionUtils.MentionChannel(userProcess.channel.Id), true);
                                                                        var url = $"https://discord.com/channels/{userProcess.channel.Guild.Id}/{userProcess.channel.Id}/{userProcess.messageID}";
                                                                        embed.AddField("Message", $"**[Go to message]({url})**", true);
                                                                        embed.AddField("Emoji", $"**{userProcess.emoji.Name}**", true);
                                                                        embed.Color = Color.Gold;


                                                                        if (count == 0)
                                                                        {
                                                                            msg.DeleteAsync();
                                                                            count = int.Parse(msg.Content.Trim());


                                                                            embed.AddField("Count", "**Maximum**", true);
                                                                            await userProcess.message.ModifyAsync(m => { m.Content = $""; m.Embed = embed.Build(); });


                                                                            userProcess.message.AddReactionAsync(yes);
                                                                            userProcess.message.AddReactionAsync(no);
                                                                            userProcess.endOfProcess = true;
                                                                            userProcess.countNum = count;

                                                                            await userProcess.message.UpdateAsync();

                                                                        }
                                                                        else
                                                                        {
                                                                            msg.DeleteAsync();
                                                                            count = int.Parse(msg.Content.Trim());
                                                                            embed.AddField("Count", $"**{count}**", true);
                                                                            await userProcess.message.ModifyAsync(m => { m.Content = $""; m.Embed = embed.Build(); });
                                                                            userProcess.message.AddReactionAsync(yes);
                                                                            userProcess.message.AddReactionAsync(no);
                                                                            userProcess.endOfProcess = true;
                                                                            userProcess.countNum = count;

                                                                            await userProcess.message.UpdateAsync();


                                                                        }



                                                                    }
                                                                    else
                                                                    {
                                                                        msg.DeleteAsync();
                                                                        await userProcess.message.ModifyAsync(m => m.Content ="This is not a number that I can set. **Process finished**.");
                                                                        userProcess =null;

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    msg.DeleteAsync();
                                                                    await userProcess.message.ModifyAsync(m => m.Content ="This is not a number. **Process finished**.");
                                                                    userProcess =null;

                                                                }
                                                            }
                                                            else
                                                            {
                                                                var emoji = new Emoji(msg.Content.Trim());

                                                                if (emoji != null)
                                                                {
                                                                    string regex = "(?:0\x20E3|1\x20E3|2\x20E3|3\x20E3|4\x20E3|5\x20E3|6\x20E3|7\x20E3|8\x20E3|9\x20E3|#\x20E3|\\*\x20E3|\xD83C(?:\xDDE6\xD83C(?:\xDDE8|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDEE|\xDDF1|\xDDF2|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFC|\xDDFD|\xDDFF)|\xDDE7\xD83C(?:\xDDE6|\xDDE7|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDEF|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFB|\xDDFC|\xDDFE|\xDDFF)|\xDDE8\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF5|\xDDF7|\xDDFA|\xDDFB|\xDDFC|\xDDFD|\xDDFE|\xDDFF)|\xDDE9\xD83C(?:\xDDEA|\xDDEC|\xDDEF|\xDDF0|\xDDF2|\xDDF4|\xDDFF)|\xDDEA\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEC|\xDDED|\xDDF7|\xDDF8|\xDDF9|\xDDFA)|\xDDEB\xD83C(?:\xDDEE|\xDDEF|\xDDF0|\xDDF2|\xDDF4|\xDDF7)|\xDDEC\xD83C(?:\xDDE6|\xDDE7|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDEE|\xDDF1|\xDDF2|\xDDF3|\xDDF5|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFC|\xDDFE)|\xDDED\xD83C(?:\xDDF0|\xDDF2|\xDDF3|\xDDF7|\xDDF9|\xDDFA)|\xDDEE\xD83C(?:\xDDE8|\xDDE9|\xDDEA|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF6|\xDDF7|\xDDF8|\xDDF9)|\xDDEF\xD83C(?:\xDDEA|\xDDF2|\xDDF4|\xDDF5)|\xDDF0\xD83C(?:\xDDEA|\xDDEC|\xDDED|\xDDEE|\xDDF2|\xDDF3|\xDDF5|\xDDF7|\xDDFC|\xDDFE|\xDDFF)|\xDDF1\xD83C(?:\xDDE6|\xDDE7|\xDDE8|\xDDEE|\xDDF0|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFB|\xDDFE)|\xDDF2\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF5|\xDDF6|\xDDF7|\xDDF8|\xDDF9|\xDDFA|\xDDFB|\xDDFC|\xDDFD|\xDDFE|\xDDFF)|\xDDF3\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEB|\xDDEC|\xDDEE|\xDDF1|\xDDF4|\xDDF5|\xDDF7|\xDDFA|\xDDFF)|\xDDF4\xD83C\xDDF2|\xDDF5\xD83C(?:\xDDE6|\xDDEA|\xDDEB|\xDDEC|\xDDED|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF7|\xDDF8|\xDDF9|\xDDFC|\xDDFE)|\xDDF6\xD83C\xDDE6|\xDDF7\xD83C(?:\xDDEA|\xDDF4|\xDDF8|\xDDFA|\xDDFC)|\xDDF8\xD83C(?:\xDDE6|\xDDE7|\xDDE8|\xDDE9|\xDDEA|\xDDEC|\xDDED|\xDDEE|\xDDEF|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF7|\xDDF8|\xDDF9|\xDDFB|\xDDFD|\xDDFE|\xDDFF)|\xDDF9\xD83C(?:\xDDE6|\xDDE8|\xDDE9|\xDDEB|\xDDEC|\xDDED|\xDDEF|\xDDF0|\xDDF1|\xDDF2|\xDDF3|\xDDF4|\xDDF7|\xDDF9|\xDDFB|\xDDFC|\xDDFF)|\xDDFA\xD83C(?:\xDDE6|\xDDEC|\xDDF2|\xDDF8|\xDDFE|\xDDFF)|\xDDFB\xD83C(?:\xDDE6|\xDDE8|\xDDEA|\xDDEC|\xDDEE|\xDDF3|\xDDFA)|\xDDFC\xD83C(?:\xDDEB|\xDDF8)|\xDDFD\xD83C\xDDF0|\xDDFE\xD83C(?:\xDDEA|\xDDF9)|\xDDFF\xD83C(?:\xDDE6|\xDDF2|\xDDFC)))|[\xA9\xAE\x203C\x2049\x2122\x2139\x2194-\x2199\x21A9\x21AA\x231A\x231B\x2328\x23CF\x23E9-\x23F3\x23F8-\x23FA\x24C2\x25AA\x25AB\x25B6\x25C0\x25FB-\x25FE\x2600-\x2604\x260E\x2611\x2614\x2615\x2618\x261D\x2620\x2622\x2623\x2626\x262A\x262E\x262F\x2638-\x263A\x2648-\x2653\x2660\x2663\x2665\x2666\x2668\x267B\x267F\x2692-\x2694\x2696\x2697\x2699\x269B\x269C\x26A0\x26A1\x26AA\x26AB\x26B0\x26B1\x26BD\x26BE\x26C4\x26C5\x26C8\x26CE\x26CF\x26D1\x26D3\x26D4\x26E9\x26EA\x26F0-\x26F5\x26F7-\x26FA\x26FD\x2702\x2705\x2708-\x270D\x270F\x2712\x2714\x2716\x271D\x2721\x2728\x2733\x2734\x2744\x2747\x274C\x274E\x2753-\x2755\x2757\x2763\x2764\x2795-\x2797\x27A1\x27B0\x27BF\x2934\x2935\x2B05-\x2B07\x2B1B\x2B1C\x2B50\x2B55\x3030\x303D\x3297\x3299]|\xD83C[\xDC04\xDCCF\xDD70\xDD71\xDD7E\xDD7F\xDD8E\xDD91-\xDD9A\xDE01\xDE02\xDE1A\xDE2F\xDE32-\xDE3A\xDE50\xDE51\xDF00-\xDF21\xDF24-\xDF93\xDF96\xDF97\xDF99-\xDF9B\xDF9E-\xDFF0\xDFF3-\xDFF5\xDFF7-\xDFFF]|\xD83D[\xDC00-\xDCFD\xDCFF-\xDD3D\xDD49-\xDD4E\xDD50-\xDD67\xDD6F\xDD70\xDD73-\xDD79\xDD87\xDD8A-\xDD8D\xDD90\xDD95\xDD96\xDDA5\xDDA8\xDDB1\xDDB2\xDDBC\xDDC2-\xDDC4\xDDD1-\xDDD3\xDDDC-\xDDDE\xDDE1\xDDE3\xDDEF\xDDF3\xDDFA-\xDE4F\xDE80-\xDEC5\xDECB-\xDED0\xDEE0-\xDEE5\xDEE9\xDEEB\xDEEC\xDEF0\xDEF3]|\xD83E[\xDD10-\xDD18\xDD80-\xDD84\xDDC0]";
                                                                    string input = msg.Content.Trim();
                                                                    var result = Regex.Match(input, regex);
                                                                    var customRegex = Regex.Match(input, "(<a?)?:\\w+:(\\d{18}>)?");
                                                                    if (customRegex.Success)
                                                                    {
                                                                        userProcess.emoji = emoji;

                                                                        if (userProcess.message != null)
                                                                        {


                                                                        }
                                                                        await userProcess.message.ModifyAsync(m => m.Content ="I set a emoji as a **" + emoji.Name +"** but I think this is a custom emoji. Then you have to make sure you reacted this emoji before to message we set. Now tell me how much you want to add reactions to this message. (If you want to set a maximum, you can tell me the 0.)");
                                                                        var no = new Emoji("❌");
                                                                        Console.WriteLine(emoji);
                                                                        userProcess.message.AddReactionAsync(no);

                                                                        await msg.DeleteAsync();
                                                                        await userProcess.message.UpdateAsync();





                                                                    }
                                                                    else
                                                                    {

                                                                        if (result.Success)
                                                                        {
                                                                            userProcess.emoji = emoji;


                                                                            await userProcess.message.ModifyAsync(m => m.Content ="I set a emoji as a **" + emoji.Name +"**. Now tell me how much you want to add reactions to this message. (If you want to set a maximum, you can tell me the 0.)");
                                                                            var no = new Emoji("❌");
                                                                            userProcess.message.AddReactionAsync(no);

                                                                            await msg.DeleteAsync();
                                                                            await userProcess.message.UpdateAsync();





                                                                        }
                                                                        else
                                                                        {
                                                                            var mg = userProcess.message;
                                                                            mg.ModifyAsync(m => m.Content = "This is not a valid emoji. **Process finished**.");
                                                                            userProcess =null;


                                                                        }

                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    var mg = userProcess.message;
                                                                    mg.ModifyAsync(m => m.Content = "This is not a valid emoji. **Process finished**.");
                                                                    userProcess =null;
                                                                    await msg.DeleteAsync();
                                                                }
                                                            }

                                                        }
                                                    }
                                                }

                                            }

                                        }


                                    }
                                }
                            }


                        }
                    }
                }
                else
                {

                    if (msg.Content.Trim().ToLower() == "!react")
                    {
                        userProcess = new UserWithMessage();
                        var embed = new EmbedBuilder();
                        embed.Title = "**Settings**";

                        embed.Description = "I'll update the settings everytime you put new informations.";
                        embed.AddField("**Channel**", "Undefined", true);

                        embed.AddField("Message", $"Undefined", true);
                        embed.AddField("Emoji", $"Undefined", true);
                        embed.AddField("Count", "Undefined");
                        embed.Color = Color.Gold;




                        var question = await msg.Channel.SendMessageAsync("Process starting, please wait.", false, null, null, null, new MessageReference(msg.Id));


                        await question.ModifyAsync(m => m.Content = "Please mention the channel you want to add react to message'.");
                        await question.UpdateAsync();
                        userProcess.userID = msg.Author.Id;
                        userProcess.message = question;

                    }
                }

            }

        }







    }
    private static async Task MessageKiller(RestMessage msg, int cooldown)
    {
        await Task.Delay(cooldown);
        if (msg != null)
        {
            await msg.UpdateAsync();
            if (msg != null)
            {
                try
                {
                    await msg.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
    private static Task MessageTask(SocketMessage msg)
    {
        MessageRecieved(msg);
        return Task.CompletedTask;
    }
    private static async Task ReactionAdded(Cacheable<IUserMessage, ulong> msg, Cacheable<IMessageChannel, ulong> channel, SocketReaction react)
    {
        if (userProcess != null)
        {
            var msgNoCache = await msg.GetOrDownloadAsync();

            if (msgNoCache.Id == userProcess.message.Id)
            {
                if (userProcess.endOfProcess)
                {
                    if (msgNoCache.Id == userProcess.message.Id)
                    {
                        if (react.User.Value.Id == userProcess.userID)
                        {
                            var yes = new Emoji("✅");
                            var no = new Emoji("❌");
                            if (react.Emote.Name == yes.Name)
                            {
                                await userProcess.message.ModifyAsync(m => m.Content = "Process started, please wait until process finish.");
                                ReactStart();
                                userProcess = null;
                            }
                            else
                            {
                                await userProcess.message.ModifyAsync(m => m.Content =   "**I finished process**. Thank you for using me :( (This message will be vanished in 15 second.)");

                                MessageKiller(userProcess.message, 15000);

                                userProcess = null;
                            }
                        }
                    }

                }
                else
                {

                    if (react.User.Value.Id == userProcess.userID)
                    {
                        Console.WriteLine("Zilli");
                        var no = new Emoji("❌");
                        if (react.Emote.Name == no.Name)
                        {

                            userProcess.isAwaiting = true;
                            await userProcess.message.ModifyAsync(m => m.Content = "**I finished process**. Thank you for using me :( (This message will be vanished in 15 second.)" + MentionUtils.MentionUser(react.UserId));
                            MessageKiller(userProcess.message, 15000);

                            userProcess = null;
                        }

                    }
                }

            }

        }
    }
    public static async Task ReactTask(UserWithMessage mg, int c, string emoji)
    {
        Console.WriteLine("Neyzen teyfik");

        var tokens = File.ReadAllLines(Environment.CurrentDirectory + @"\tokens.txt").ToList();
        var ips = File.ReadAllLines(Environment.CurrentDirectory + @"\ips.txt").ToList();
        int ipstet = 0;
        if (tokenResponseList.rp.FindAll(m => m.messageID == mg.messageID.ToString()).FindAll(m => m.status != TokenWithResponse.ResponseType.CantLike).Count != 0)
        {

            if (tokenResponseList.rp.FindAll(m => m.messageID == mg.messageID.ToString()).FindAll(m => m.status != TokenWithResponse.ResponseType.Liked).Count != mg.countNum)
            {

                if (c != 15)
                {
                    var keke = tokenResponseList.rp.FindAll(c => c.messageID == mg.messageID.ToString());
                    var usedtokens = new List<string>();
                    foreach (var m in keke)
                    {
                        usedtokens.Add(m.token.Trim());
                    }
                    var notusedtoken = tokens.FindAll(m => !usedtokens.Contains(m.Trim()));
                    Console.WriteLine(notusedtoken.Count);

                    while (tokenResponseList.rp.FindAll(m => m.messageID == mg.messageID.ToString()).FindAll(m => m.status != TokenWithResponse.ResponseType.Processing).Count == 0)
                    {
                        int jk = 0;
                    }
                    for (int i = 0; i < tokenResponseList.rp.FindAll(m => m.messageID == mg.messageID.ToString()).FindAll(m => m.status != TokenWithResponse.ResponseType.CantLike).Count; i++)
                    {

                        if (notusedtoken.Count > ips.Count)
                        {
                            var divide = notusedtoken.Count / ips.Count;
                            if (i % divide == 0)
                            {
                                ipstet++;
                            }
                        }
                        else
                        {
                            ipstet = i;
                        }


                        var tp = new TokenWithResponse(tokens[i], mg.messageID.ToString(), emoji);
                        tokenResponseList.rp.Add(tp);
                        React(mg.channel.Id.ToString(), mg.messageID.ToString(), emoji, notusedtoken[i], ips[ipstet], tp);

                        await Task.Delay(200);
                    }

                    ReactTask(mg, c + 1, emoji);
                }
            }
        }


    }
    public static async Task ReactStart()
    {
        var mg = userProcess;
        int count = 0;

        var tokens = File.ReadAllLines(Environment.CurrentDirectory + @"\tokens.txt").ToList();


        var ips = File.ReadAllLines(Environment.CurrentDirectory + @"\ips.txt").ToList();
  
        bool ismax = false;

        if (mg.countNum == 0)
        {
            count = tokens.Count;
            ismax = true;
        }
        else
        {
            count = mg.countNum;
        }
        var memoji = "";

        if (!mg.emoji.ToString().Contains("<"))
        {
            memoji = HttpUtility.UrlEncode(mg.emoji.ToString());
        }
        else
        {
            var mgst = mg.emoji.ToString().Replace("<", "").Replace(">", "").Trim().Remove(0, 1).Trim();

            memoji = HttpUtility.UrlEncode(mgst);
            Console.WriteLine(memoji);
        }




        var ggs = tokenResponseList.rp.FindAll(m => m.messageID == mg.messageID.ToString().Trim() && m.emoji == memoji).ToList();
        var notusedtokens = new List<string>();
        Console.WriteLine(ggs.Count);
        if (ggs.Count > 0)
        {
            foreach (var m in ggs)
            {
                if (!tokens.Contains(m.token.Trim()))
                {
                    notusedtokens.Add(m.token);
                }
            }
            tokens = notusedtokens;
        }

        Console.WriteLine(tokens.Count);
        int ipstet = 0;
        var tc = await _client.GetChannelAsync(938077288325607448);
        var kc = tc as SocketTextChannel;

        for (int i = 0; i < tokens.Count; i++)
        {
 

          //  if (tokens.Count > ips.Count)
          //  {
          //      var divide = tokens.Count / ips.Count;
          //      if (i % divide == 0)
          //      {
          //          ipstet++;
          //      }
          //  }
          //  else
          //  {
          //      ipstet = i;
          //
          //  }
          //

           // var tp = new TokenWithResponse(tokens[i], mg.messageID.ToString(), memoji);
           
            React(mg.channel.Id.ToString(), mg.messageID.ToString(), memoji, tokens[i], ips[i], new TokenWithResponse("","",""));

            await Task.Delay(50);
   

        }






    }
    #endregion


}











