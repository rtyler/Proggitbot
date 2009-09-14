using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

using Meebey.SmartIrc4net;

namespace Proggitbot
{
	public class Proggitbot
	{
		#region "Member Variables"
		private readonly string jsonUrl = "http://www.reddit.com/r/programming/.json";
		protected JavaScriptSerializer json = new JavaScriptSerializer();
		protected List<EntryData> recentEntries = null;
		#endregion

		#region "Public Methods"
		///	<summary>
		///		You should be using this method, this will fetch
		///		the latest Proggit front-pagers and return them 
		///		as a List<EntryData>. 
		///
		///		I might return null if I don't have anything for you :)
		///	</summary>
		public List<EntryData> FetchNewEntries()
		{
			return this.FetchNewEntries(this.FetchJson(this.jsonUrl));
		}

		///	<summary>
		///		Process a string of JSON, this method is really meant
		///		for testing
		///	</summary>
		///	<param name="json">
		///		Properly formatted JSON retrieved from:
		///			http://reddit.com/r/programming/.json
		///	</param>
		public List<EntryData> FetchNewEntries(string json)
		{
			Root root = null;

			if (String.IsNullOrEmpty(json))
			{
				throw new Exception("No JSON from Reddit!");
			}

			root = this.json.Deserialize<Root>(json);

			if (root == null)
			{
				throw new Exception("Null object deserialized?");
			}

			if (this.recentEntries == null)
			{
				this.recentEntries = root.Entries;
				return this.recentEntries;
			}

			IEnumerable<EntryData> diff = root.Entries.Except(this.recentEntries, 
						new EntryDataComparer());

			List<EntryData> difference = diff.ToList<EntryData>();

			if (difference.Count != 0)
			{
				/// This is a new list, so let's save it
				this.recentEntries = root.Entries;
				return difference;
			}

			return null;
		}
		#endregion

		#region "Internal Methods"
		internal string FetchJson(string fullUrl)
		{
			HttpWebRequest request = null;
			HttpWebResponse response = null;
			StreamReader reader = null;

			try
			{
				request = WebRequest.Create(fullUrl) as HttpWebRequest;
				request.UserAgent = "Proggitbot";

				if (request == null)
				{
					return null;
				}

				using (response = request.GetResponse() as HttpWebResponse)
				{
					if ( (response == null) || (!request.HaveResponse) )
					{
						return null;
					}

					reader = new StreamReader(response.GetResponseStream());
					return reader.ReadToEnd();
				}
			}
			catch (WebException exc)
			{
				if (exc == null)
				{
					return null;
				}

				using (HttpWebResponse errorResponse = exc.Response as HttpWebResponse)
				{
					if (errorResponse == null)
					{
						/*
						 * If we don't have an error response, that means we likely
						 * have a SocketException from the underlying layer, and we 
						 * should likely propogate that up
						 */
						throw;
					}
					Console.WriteLine("The server returned \"{0}\", status {1}",
							errorResponse.StatusDescription, errorResponse.StatusCode);
				}
			}
			return null;
		}
		#endregion
	}
}
