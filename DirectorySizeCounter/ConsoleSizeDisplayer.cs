using System;
using System.Collections.Generic;

namespace DirectorySizeCounter
{
	internal class ConsoleSizeDisplayer
	{
		private const uint maximumItemsToDisplay = 10;
		private readonly bool showSummary;
		private readonly uint itemsToDisplay;

		public ConsoleSizeDisplayer(bool showSummary) : this(showSummary, maximumItemsToDisplay)
		{
		}

		public ConsoleSizeDisplayer(bool showSummary, uint itemsToDisplay)
		{
			this.showSummary = showSummary;
			this.itemsToDisplay = itemsToDisplay;
		}

		public void DisplaySizes(IEnumerable<SizeInformation> sizes, uint totalReadOnlyFiles)
		{
			var enumerator = sizes.GetEnumerator();
			var itemsDisplayed = 0;
			while (enumerator.MoveNext() && itemsDisplayed < itemsToDisplay)
			{
				Console.WriteLine(string.Format("{0} - {1}", enumerator.Current.Item, enumerator.Current.Size));
				itemsDisplayed++;
			}

			if (showSummary)
			{
				DisplayReadOnlyFileCount(totalReadOnlyFiles);
			}				
		}

		private static void DisplayReadOnlyFileCount(uint readOnlyFileCount)
		{
			Console.WriteLine(string.Format("Total readonly files {0}", readOnlyFileCount));
		}

	}
}