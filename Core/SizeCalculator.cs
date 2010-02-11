using System.Collections.Generic;

namespace DirectorySizeCounter.Core
{
	internal abstract class SizeCalculator : ISizeCalculator
	{
		public abstract string StatusMessage { get; }
		protected abstract IEnumerable<SizeResult> GetSizes(string baseDirectory, ISummarizer summarizer);

		public CalculationResult CalculateSizes(string baseDirectory)
		{
			return CalculateSizes(baseDirectory, null);
		}

		public CalculationResult CalculateSizes(string baseDirectory, ISummarizer summarizer)
		{
			var proxiedSummarizer = new ProxiedSummarizer(summarizer);
			var sizes = GetSizes(baseDirectory, proxiedSummarizer);
			var summary = proxiedSummarizer.Summary;
			var calculationResult = new CalculationResult(sizes, summary);

			return calculationResult;
		}

		protected static IEnumerable<SizeResult> ExtractSizesFromSortedSizeData(SortedDictionary<uint, List<string>> sortedSizeData)
		{
			foreach (var size in sortedSizeData.Keys)
			{
				foreach (var subDirectory in sortedSizeData[size])
				{
					yield return new SizeResult(subDirectory, size);
				}
			}
		}
	}
}