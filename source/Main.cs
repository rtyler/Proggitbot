using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Meebey.SmartIrc4net;

namespace Proggitbot
{
	public class Runner
	{
		#region "Member Variables"
		protected static IrcClient irc = new IrcClient();
		protected static string channel = "#proggitbot";
		protected static string nick = "Proggitbot";
		protected static string fullname = "Mono-driven Proggitbot";
		protected static Proggitbot bot = new Proggitbot();
		protected static Timer timer = null;
		protected static Int64 pingBackPeriod = 60000;
		#endregion

		#region "Public Static Methods"
		public static void OnQueryMessage(object sender, IrcEventArgs e)
		{
			irc.SendMessage(SendType.Message, e.Data.Nick, "Sorry, I don't talk to strangers");
			return;
		}

		public static void OnError(object sender, ErrorEventArgs e)
		{
			Console.WriteLine("Error: " + e.ErrorMessage);
			Exit(-1);
		}

		public static void OnRawMessage(object sender, IrcEventArgs e)
		{
			Console.WriteLine("Received: " + e.Data.RawMessage);
			Console.WriteLine(e.Data.Nick);
			Console.WriteLine(e.Data.Message);
		}

		public static void TimedCheckup(object state)
		{
			List<EntryData> entries = bot.FetchNewEntries();
			if ( (entries == null) || (entries.Count == 0) )
			{
				Console.WriteLine("Nothing new to report");
				return;
			}

			foreach (EntryData entry in entries)
			{
				irc.SendMessage(SendType.Message, channel, 
						String.Format("[+{0}/-{1}] {2} {3}", entry.Ups, entry.Downs, entry.Title,
									String.Format("http://reddit.com/comments/{0}", entry.Id)
								));
			}
		}

		public static void Main(string[] args)
		{
			bot.FetchNewEntries();

			timer = new Timer(new TimerCallback(TimedCheckup), null, 0, pingBackPeriod);

			Thread.CurrentThread.Name = "Main";
			
			irc.Encoding = Encoding.UTF8;
			irc.SendDelay = 200;
			irc.ActiveChannelSyncing = true;
			
			// here we connect the events of the API to our written methods
			// most have own event handler types, because they ship different data
			irc.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
			irc.OnError += new ErrorEventHandler(OnError);
			irc.OnRawMessage += new IrcEventHandler(OnRawMessage);

			string[] serverlist;
			serverlist = new string[] {"irc.freenode.org"};
			int port = 6667;
			try {
				// here we try to connect to the server and exceptions get handled
				irc.Connect(serverlist, port);
			} catch (ConnectionException e) {
				// something went wrong, the reason will be shown
				Console.WriteLine("couldn't connect! Reason: "+e.Message);
				Exit();
			}
			
			try {
				// here we logon and register our nickname and so on 
				irc.Login(nick, fullname);
				// join the channel
				irc.RfcJoin(channel);

				// spawn a new thread to read the stdin of the console, this we use
				// for reading IRC commands from the keyboard while the IRC connection
				// stays in its own thread
				new Thread(new ThreadStart(ReadCommands)).Start();
				
				// here we tell the IRC API to go into a receive mode, all events
				// will be triggered by _this_ thread (main thread in this case)
				// Listen() blocks by default, you can also use ListenOnce() if you
				// need that does one IRC operation and then returns, so you need then 
				// an own loop 
				irc.Listen();
				
				// when Listen() returns our IRC session is over, to be sure we call
				// disconnect manually
				irc.Disconnect();
			} catch (ConnectionException) {
				// this exception is handled because Disconnect() can throw a not
				// connected exception
				Exit();
			} catch (Exception e) {
				// this should not happen by just in case we handle it nicely
				Console.WriteLine("Error occurred! Message: "+e.Message);
				Console.WriteLine("Exception: "+e.StackTrace);
				Exit();
			}
		}

		public static void ReadCommands()
		{
			// here we read the commands from the stdin and send it to the IRC API
			// WARNING, it uses WriteLine() means you need to enter RFC commands
			// like "JOIN #test" and then "PRIVMSG #test :hello to you"
			while (true) {
				string cmd = Console.ReadLine();
				if (cmd.StartsWith("/list")) {
					int pos = cmd.IndexOf(" ");
					string channel = null;
					if (pos != -1) {
						channel = cmd.Substring(pos + 1);
					}
					
					IList<ChannelInfo> channelInfos = irc.GetChannelList(channel);
					Console.WriteLine("channel count: {0}", channelInfos.Count);
					foreach (ChannelInfo channelInfo in channelInfos) {
						Console.WriteLine("channel: {0} user count: {1} topic: {2}",
										  channelInfo.Channel,
										  channelInfo.UserCount,
										  channelInfo.Topic);
					}
				} else {
					irc.WriteLine(cmd);
				}
			}
		}

		public static void Exit()
		{
			Exit(0);
		}

		public static void Exit(int code)
		{
			Console.WriteLine("Exiting..");
			Environment.Exit(code);
		}
		#endregion
	}
}
