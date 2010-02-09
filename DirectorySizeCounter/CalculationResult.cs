using System.Collections.Generic;

namespace DirectorySizeCounter
{
	internal class CalculationResult
	{
		public CalculationResult(IEnumerable<SizeResult> sizeResults, string summary)
		{
			Sizes = sizeResults;
			Summary = summary;
		}

		public IEnumerable<SizeResult> Sizes
		{
			get;
			private set;
		}

		public string Summary
		{
			get;
			private set;
		}
	}
}