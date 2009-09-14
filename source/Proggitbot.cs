using System;
using System.Collections.Generic;
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
		#endregion

		#region "Public Methods"
		public Root Load()
		{
			string data = this.FetchJson(this.jsonUrl);

			if (String.IsNullOrEmpty(data))
			{
				throw new Exception("No JSON from Reddit!");
			}

			return this.json.Deserialize<Root>(data);
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
