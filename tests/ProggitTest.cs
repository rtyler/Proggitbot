using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

using Proggitbot;
using NUnit.Framework;

namespace Proggitbot.Tests
{
	[TestFixture]
	public class ParseJson
	{
		protected readonly string sampleDataField = "{\"domain\": \"ted.com\",\"media_embed\": { },\"subreddit\": \"programming\",\"selftext_html\": null,\"selftext\": \"\",\"likes\": null,\"saved\": false,\"id\": \"9k30b\",\"clicked\": false,\"author\": \"scientologist2\",\"media\": null,\"score\": 353,\"hidden\": false,\"thumbnail\": \"\",\"subreddit_id\": \"t5_2fwo\",\"downs\": 97,\"name\": \"t3_9k30b\",\"created\": 1252871681.0,\"url\": \"http://www.ted.com/talks/dan_pink_on_motivation.html\",\"title\": \"The science of motivation vs. problem solving\",\"created_utc\": 1252846481.0,\"num_comments\": 82,\"ups\": 450}";

		protected readonly string sampleEntireEntry = "{\"kind\": \"Listing\",\"data\": {\"after\": \"t3_9jxt6\",\"children\": [{\"kind\": \"t3\",\"data\": {\"domain\": \"ted.com\",\"media_embed\": {},\"subreddit\": \"programming\",\"selftext_html\": null,\"selftext\": \"\",\"likes\": null,\"saved\": false,\"id\": \"9k30b\",\"clicked\": false,\"author\": \"scientologist2\",\"media\": null,\"score\": 353,\"hidden\": false,\"thumbnail\": \"\",\"subreddit_id\": \"t5_2fwo\",\"downs\": 97,\"name\": \"t3_9k30b\",\"created\": 1252871681.0,\"url\": \"http://www.ted.com/talks/dan_pink_on_motivation.html\",\"title\": \"The science of motivation vs. problem solving\",\"created_utc\": 1252846481.0,\"num_comments\": 82,\"ups\": 450}}],\"before\": null}}";

		protected JavaScriptSerializer json = null;

		[SetUp]
		public void SetUp()
		{
			this.json = new JavaScriptSerializer();
		}

		[Test]
		public void DeserializeIntoEntryData()
		{
			EntryData data = this.json.Deserialize<EntryData>(this.sampleDataField);

			Assert.IsNotNull(data, "EntryData object is null");
			Assert.AreEqual("The science of motivation vs. problem solving", data.Title, "Title mismatch");
			Assert.AreEqual("scientologist2", data.Author, "Author mismatch");
			Assert.AreEqual("ted.com", data.Domain, "Domain mismatch");
			Assert.AreEqual("9k30b", data.Id, "ID mismatch");
			Assert.AreEqual(450, data.Ups, "Ups mismatch");
			Assert.AreEqual(97, data.Downs, "Downs mismatch");
		}

		[Test]
		public void DeserializeIntoRoot()
		{
			Root root = this.json.Deserialize<Root>(this.sampleEntireEntry);
			Assert.IsNotNull(root, "Root object is null");
			Assert.AreEqual("Listing", root.Kind, "Kind mismatch");
		}

		[Test] 
		public void PullEntriesThroughRoot()
		{
			Root root = this.json.Deserialize<Root>(this.sampleEntireEntry);
			Assert.IsNotNull(root, "Root object is null");
			
			Assert.IsNotNull(root.Entries, "Root.Entries is null");
			Assert.AreEqual(1, root.Entries.Count, "Root.Entries should have one node in it");
		}
	}

	[TestFixture]
	public class ListComparisionTests
	{
		protected readonly string sampleEntireEntry1 = "{\"kind\": \"Listing\",\"data\": {\"after\": \"t3_9jxt6\",\"children\": [{\"kind\": \"t3\",\"data\": {\"domain\": \"ted.com\",\"media_embed\": {},\"subreddit\": \"programming\",\"selftext_html\": null,\"selftext\": \"\",\"likes\": null,\"saved\": false,\"id\": \"9k30b\",\"clicked\": false,\"author\": \"scientologist2\",\"media\": null,\"score\": 353,\"hidden\": false,\"thumbnail\": \"\",\"subreddit_id\": \"t5_2fwo\",\"downs\": 97,\"name\": \"t3_9k30b\",\"created\": 1252871681.0,\"url\": \"http://www.ted.com/talks/dan_pink_on_motivation.html\",\"title\": \"The science of motivation vs. problem solving\",\"created_utc\": 1252846481.0,\"num_comments\": 82,\"ups\": 450}}],\"before\": null}}";

		protected readonly string sampleEntireEntry2 = "{\"kind\": \"Listing\",\"data\": {\"after\": \"t3_9jxt6\",\"children\": [{\"kind\": \"t3\",\"data\": {\"domain\": \"ted.com\",\"media_embed\": {},\"subreddit\": \"programming\",\"selftext_html\": null,\"selftext\": \"\",\"likes\": null,\"saved\": false,\"id\": \"9k30b\",\"clicked\": false,\"author\": \"scientologist2\",\"media\": null,\"score\": 353,\"hidden\": false,\"thumbnail\": \"\",\"subreddit_id\": \"t5_2fwo\",\"downs\": 97,\"name\": \"t3_9k30b\",\"created\": 1252871681.0,\"url\": \"http://www.ted.com/talks/dan_pink_on_motivation.html\",\"title\": \"The science of motivation vs. problem solving\",\"created_utc\": 1252846481.0,\"num_comments\": 82,\"ups\": 450}},{\"kind\": \"t3\", \"data\": {\"domain\": \"haxney.org\", \"media_embed\": {}, \"subreddit\": \"programming\", \"selftext_html\": null, \"selftext\": \"\", \"likes\": null, \"saved\": false, \"id\": \"9jxf6\", \"clicked\": false, \"author\": \"jonromero\", \"media\": null, \"score\": 110, \"hidden\": false, \"thumbnail\": \"\", \"subreddit_id\": \"t5_2fwo\", \"downs\": 94, \"name\": \"t3_9jxf6\", \"created\": 1252809159.0, \"url\": \"http://www.haxney.org/2009/08/its-alive.html\", \"title\": \"Doing the impossible: A fully-featured web browser in Emacs\", \"created_utc\": 1252783959.0, \"num_comments\": 101, \"ups\": 204}}],\"before\": null}}";

		protected JavaScriptSerializer json = null;
		protected Proggitbot pg = null;

		[SetUp]
		public void SetUp()
		{
			this.json = new JavaScriptSerializer();
			this.pg = new Proggitbot();
		}

		[Test]
		public void TestDifferenceInJson()
		{
			List<EntryData> first = this.pg.FetchNewEntries(this.sampleEntireEntry1);
			Assert.IsNotNull(first, "first list is null");

			List<EntryData> second = this.pg.FetchNewEntries(this.sampleEntireEntry2);
			Assert.IsNotNull(second, "second list is null");

			Assert.AreEqual(1, second.Count, "Second list should only have one item");
		}
	}
}
