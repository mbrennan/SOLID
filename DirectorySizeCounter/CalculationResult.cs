using System.Collections.Generic;

namespace DirectorySizeCounter
{
	internal class CalculationResult
	{
		public CalculationResult(IEnumerable<SizeResult> sizeResults, uint readOnlyFileCount)
		{
			Sizes = sizeResults;
			ReadOnlyFileCount = readOnlyFileCount;
		}

		public IEnumerable<SizeResult> Sizes
		{
			get;
			private set;
		}

		public uint ReadOnlyFileCount
		{
			get;
			private set;
		}
	}
}