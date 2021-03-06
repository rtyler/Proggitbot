using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Proggitbot
{
	[Serializable]
	public class Root
	{
		#region "Member Variables"
		private string kind = null;
		private RootData data = null;

		///	<summary>
		///		Separate variable for pulling EntryData objects
		///		up out of the Root.RootData.Children.Data objects
		///	</summary>
		private List<EntryData> entries = null;
		#endregion

		#region "Public Properties"
		public string Kind
		{
			get { return this.kind; }
			set { this.kind = value; }
		}
		
		public RootData Data
		{
			get { return this.data; }
			set { this.data = value; }
		}

		public List<EntryData> Entries
		{
			get 
			{
				if (this.entries != null)
					return this.entries;

				if (this.Data == null)
					return null;

				this.entries = new List<EntryData>();

				foreach (Entry entry in this.Data.Children)
				{
					this.entries.Add(entry.Data);
				}
				return this.entries;
			}
		}
		#endregion
	}

	[Serializable]
	public class RootData
	{
		#region "Member Variables"
		private string after = null;
		private string before = null;
		private List<Entry> children = null;
		#endregion

		#region "Public Properties"
		public string After
		{
			get { return this.after; }
			set { this.after = value; }
		}

		public string Before
		{
			get { return this.before; }
			set { this.before = value; }
		}

		public List<Entry> Children
		{
			get { return this.children; }
			set { this.children = value; }
		}
		#endregion
	}

	[Serializable]
	public class Entry
	{
		#region "Member Variables"
		private string kind = null;
		private EntryData data = null; 
		#endregion

		#region "Public Properties"
		public string Kind
		{
			get { return this.kind; }
			set { this.kind = value; }
		}
	
		public EntryData Data
		{
			get { return this.data; }
			set { this.data = value; }
		}
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
		public string Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		public Int64 Ups
		{
			get { return this.ups; }
			set { this.ups = value; }
		}

		public Int64 Downs
		{
			get { return this.downs; }
			set { this.downs = value; }
		}

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

		#region "Public Methods"
		public bool Equals(EntryData other)
		{
			if (this.Title == other.Title)
				return true;
			return false;
		}
		#endregion
	}

	public class EntryDataComparer : IEqualityComparer<EntryData>
	{
		public bool Equals(EntryData left, EntryData right)
		{
			if (left.Title == right.Title)
				return true;
			return false;
		}

		public int GetHashCode(EntryData item)
		{
			return item.Title.GetHashCode();
		}
	}
}

