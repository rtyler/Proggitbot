using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Proggitbot
{
	[Serializable]
	public class Entry
	{
		#region "Member Variables"
		private string kind = null;
		private EntryData data = null; 
		#endregion
	}

	[Serializable]
	public class EntryData
	{
		#region "Member Variables"
		private string domain = null;
		private string subreddit = null;
		private string id = null;
		private string author = null;
		private Int64 score = 0;
		private string subreddit_id = null;
		private Int64 downs = 0;
		private Int64 ups = 0;
		private string name = null;
		private DateTime created;
		private DateTime created_utc;
		private string url = null;
		private string title = null;
		private Int64 num_comments = 0;
		#endregion

		#region "Public Properties"
		public string Domain 
		{
			get { return this.domain; }
			set { this.domain = value; }
		}

		public string Author
		{
			get { return this.author; }
			set { this.author = value; }
		}

		public string Url
		{
			get { return this.url; }
			set { this.url = value; }
		}

		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}
		#endregion
	}
}

