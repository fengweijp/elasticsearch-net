﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace Nest
{
	//TODO OIS Version

	[DescriptorFor("CountPercolate")]
	public partial class PercolateCountDescriptor<T,K> : IndexTypePathDescriptor<PercolateCountDescriptor<T, K>, PercolateCountRequestParameters, T> 
		where T : class
		where K : class
	{
		[JsonProperty(PropertyName = "query")]
		internal IQueryContainer _Query { get; set; }

		[JsonProperty(PropertyName = "doc")]
		internal K _Document { get; set; }

		/// <summary>
		/// The object to perculate
		/// </summary>
		public PercolateCountDescriptor<T, K> Object(K @object)
		{
			this._Document = @object;
			return this;
		}

		/// <summary>
		/// Optionally specify more search options such as facets, from/to etcetera.
		/// </summary>
		public PercolateCountDescriptor<T, K> Query(Func<QueryDescriptor<T>, QueryContainer> querySelector)
		{
			querySelector.ThrowIfNull("querySelector");
			var d = querySelector(new QueryDescriptor<T>());
			this._Query = d;
			return this;
		}

		protected override void UpdatePathInfo(IConnectionSettingsValues settings, ElasticsearchPathInfo<PercolateCountRequestParameters> pathInfo)
		{
			//.NET does not like sending data using get so we use POST
			pathInfo.HttpMethod = PathInfoHttpMethod.POST;
		}
	}
}
