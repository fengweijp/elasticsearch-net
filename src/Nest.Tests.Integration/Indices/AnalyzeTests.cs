﻿using System.Linq;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;

namespace Nest.Tests.Integration.Indices
{
	[TestFixture]
	public class AnalyzeTest : IntegrationTests
	{
		private void _defaultAnalyzeAssertations(IAnalyzeResponse r)
		{
			Assert.True(r.IsValid);
			Assert.NotNull(r.Tokens);
			Assert.True(r.Tokens.Any());
			Assert.True(r.Tokens.All(t=> {
				return !t.Token.IsNullOrEmpty()
					&& !t.Type.IsNullOrEmpty()
					&& t.EndPostion > 0;
			}));
		}

		[Test]
		public void SimpleAnalyze()
		{
			//analyze text using default index settings
			var text = "this is a string with some spaces and stuff";
			var r = this._client.Analyze(a=>a.Text(text));
			this._defaultAnalyzeAssertations(r);
			Assert.False(r.Tokens.Count() == text.Split(new[] { ' ' }).Count());
		}
		[Test]
		public void FieldAnalyze()
		{
			//analyze text using elasticssearchprojects content field settings
			var text = "this is a string with some spaces and stuff";
			var r = this._client.Analyze(a=>a.Index<ElasticSearchProject>().Field<ElasticSearchProject>(p => p.Content).Text(text));
			this._defaultAnalyzeAssertations(r);
			Assert.False(r.Tokens.Count() == text.Split(new[] { ' ' }).Count());
		}
		[Test]
		public void SimplAnalyzeDifferentIndex()
		{
			//analyze text using a different index and custom analyzer
			var text = "this is a string with some spaces and stuff";
			var r = this._client.Analyze(a=>a.Analyzer("whitespace").Text(text).Index(ElasticsearchConfiguration.DefaultIndex));
			this._defaultAnalyzeAssertations(r);
			Assert.True(r.Tokens.Count() == text.Split(new[] { ' ' }).Count());
		}
		[Test]
		public void AnalyzeDifferentIndex()
		{
			//analyze text using elasticssearchprojects content field but on a different index
			var text = "this is a string with some spaces and stuff";
			var index = ElasticsearchConfiguration.DefaultIndex;
			var r = this._client.Analyze(a=>a.Field<ElasticSearchProject>(p => p.Content).Index(index).Text(text));
			this._defaultAnalyzeAssertations(r);
			Assert.False(r.Tokens.Count() == text.Split(new []{' '}).Count());
		}
		[Test]
		public void SimplAnalyzeDifferentIndexAndField()
		{
			//analyze text using a different index and custom analyzer
			var text = "this is a string with some spaces and stuff";
			var analyzer = new AnalyzeParams { Field = "content", Index = ElasticsearchConfiguration.DefaultIndex };
			var r = this._client.Analyze(a=>a.Field("content").Index(ElasticsearchConfiguration.DefaultIndex));
			this._defaultAnalyzeAssertations(r);
			Assert.False(r.Tokens.Count() == text.Split(new[] { ' ' }).Count());
		}
	}
}