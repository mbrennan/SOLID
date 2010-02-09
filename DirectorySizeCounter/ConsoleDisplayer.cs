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

		public void DisplaySummary(string summary)
		{
			if (summary == null)
				return;

			Console.WriteLine(summary);
		}

		public void ShowStatusMessage(ISizeCalculator calculator)
		{
			Console.WriteLine(calculator.StatusMessage);
		}
	}
}