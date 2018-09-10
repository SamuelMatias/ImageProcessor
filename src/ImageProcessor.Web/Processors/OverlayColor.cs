﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OverlayColor.cs" company="James Jackson-South">
//   Copyright (c) James Jackson-South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   Adds a color overlay to the current image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Web.Processors
{
	using System.Collections.Specialized;
	using System.Drawing;
	using System.Text.RegularExpressions;
	using System.Web;

	using ImageProcessor.Processors;
	using ImageProcessor.Web.Helpers;

	/// <summary>
	/// Adds a color overlay to the current image.
	/// </summary>
	public class OverlayColor : IWebGraphicsProcessor
	{
		/// <summary>
		/// The regular expression to search strings for.
		/// </summary>
		private static readonly Regex QueryRegex = new Regex("color=[^&]", RegexOptions.Compiled);

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlayColor"/> class.
		/// </summary>
		public OverlayColor() => this.Processor = new ImageProcessor.Processors.OverlayColor();

		/// <summary>
		/// Gets the regular expression to search strings for.
		/// </summary>
		public Regex RegexPattern => QueryRegex;

		/// <summary>
		/// Gets the order in which this processor is to be used in a chain.
		/// </summary>
		public int SortOrder { get; private set; }

		/// <summary>
		/// Gets the associated graphics processor.
		/// </summary>
		public IGraphicsProcessor Processor { get; }

		/// <summary>
		/// The position in the original string where the first character of the captured substring was found.
		/// </summary>
		/// <param name="queryString">The query string to search.</param>
		/// <returns>
		/// The zero-based starting position in the original string where the captured substring was found.
		/// </returns>
		public int MatchRegexIndex(string queryString)
		{
			this.SortOrder = int.MaxValue;
			Match match = this.RegexPattern.Match(queryString);

			if (match.Success)
			{
				this.SortOrder = match.Index;
				NameValueCollection queryCollection = HttpUtility.ParseQueryString(queryString);
				this.Processor.DynamicParameter = (Color)QueryParamParser.Instance.ParseValue<Color>(queryCollection["color"]);
			}

			return this.SortOrder;
		}
	}
}