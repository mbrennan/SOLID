using System;
using System.Collections.Generic;

namespace DirectorySizeCounter
{
	internal class Program
	{
		private static void Main(string[] arguments)
		{
			if (arguments.Length == 0 || arguments.Length > 3)
			{
				ShowUsage();
				return;
			}

			var baseDirectory = arguments[0];
			var calculationMode = DetermineCalcuationMode(arguments);
			var shouldDisplayReadOnlyFileCountInSummary = ShouldReadOnlyFileCountBeDisplayedInSummary(arguments);
			var sizeCalculator = new SizeCalculator(calculationMode, baseDirectory);
			uint readOnlyFileCount;
			var sizes = sizeCalculator.CalculateSizes(out readOnlyFileCount);
			var sizeDisplayer = new ConsoleSizeDisplayer(shouldDisplayReadOnlyFileCountInSummary);
			sizeDisplayer.DisplaySizes(sizes, readOnlyFileCount);
		}

		private static void ShowUsage()
		{
			Console.WriteLine("Usage:  SizeInformationSummary <directory> [/filetype /showreadonly]");
			Console.WriteLine("    /filetype      -  Show by file type instead of directory");
			Console.WriteLine("    /showreadonly  -  Shows the number of files that are readonly");
		}

		private static bool ShouldReadOnlyFileCountBeDisplayedInSummary(IEnumerable<string> arguments)
		{
			foreach (var argument in arguments)
			{
				if (argument == "/showreadonly")
					return true;
			}

			return false;
		}

		private static CalculationMode DetermineCalcuationMode(IEnumerable<string> consoleArguments)
		{
			foreach (var consoleArgument in consoleArguments)
			{
				if (consoleArgument == "/filetype")
					return CalculationMode.ByFileType;
			}

			return CalculationMode.ByDirectorySize;
		}
	}
}
