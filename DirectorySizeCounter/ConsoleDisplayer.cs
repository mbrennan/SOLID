using System;
using System.Collections.Generic;

namespace DirectorySizeCounter
{
	internal class ConsoleDisplayer
	{
		private const uint maximumItemsToDisplay = 10;

		public void DisplaySizes(IEnumerable<SizeResult> sizes)
		{
			var enumerator = sizes.GetEnumerator();
			var itemsDisplayed = 0;
			while (enumerator.MoveNext() && itemsDisplayed < maximumItemsToDisplay)
			{
				Console.WriteLine(string.Format("{0} - {1}", enumerator.Current.Item, enumerator.Current.Size));
				itemsDisplayed++;
			}
		}

		public void DisplaySummary(uint readOnlyFileCount)
		{
			Console.WriteLine(string.Format("Read-only file count:  {0}", readOnlyFileCount));
		}

		public void ShowStatusMessage(ISizeCalculator calculator)
		{
			if (calculator is DirectorySizeCalculator)
			{
				Console.WriteLine("Calcuating directory sizes...");
			}
			else if (calculator is FileCategorySizeCalculator)
			{
				Console.WriteLine("Calculating sizes by file category...");
			}
		}
	}
}