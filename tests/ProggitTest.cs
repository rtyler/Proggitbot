using System;
using System.Web.Script.Serialization;

using Proggitbot;
using NUnit.Framework;

namespace Proggitbot.Tests
{
	[TestFixture]
	public class ParseJson
	{
		protected readonly string sampleDataField = "{\"domain\": \"ted.com\",\"media_embed\": { },\"subreddit\": \"programming\",\"selftext_html\": null,\"selftext\": \"\",\"likes\": null,\"saved\": false,\"id\": \"9k30b\",\"clicked\": false,\"author\": \"scientologist2\",\"media\": null,\"score\": 353,\"hidden\": false,\"thumbnail\": \"\",\"subreddit_id\": \"t5_2fwo\",\"downs\": 97,\"name\": \"t3_9k30b\",\"created\": 1252871681.0,\"url\": \"http://www.ted.com/talks/dan_pink_on_motivation.html\",\"title\": \"The science of motivation vs. problem solving\",\"created_utc\": 1252846481.0,\"num_comments\": 82,\"ups\": 450}";

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
		}
	}
}
