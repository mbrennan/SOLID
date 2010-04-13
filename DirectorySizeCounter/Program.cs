using System;
using System.Collections.Generic;
using System.IO;

namespace DirectorySizeCounter
{
	internal class Program
	{
		private const uint maximumItemsToDisplay = 10;
		private static int readOnlyFileCount;

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
			var sizes = CalculateSizes(calculationMode, baseDirectory);
			DisplaySizes(sizes);

			if (shouldDisplayReadOnlyFileCountInSummary)
			{
				DisplayReadOnlyFileCount();
			}				
		}

		private static void ShowUsage()
		{
			Console.WriteLine("Usage:  SizeInformationSummary <directory> [/filetype /showreadonly]");
			Console.WriteLine("    /filetype      -  Show by file type instead of directory");
			Console.WriteLine("    /showreadonly  -  Shows the number of files that are readonly");
		}

		private static void DisplayReadOnlyFileCount()
		{
			Console.WriteLine(string.Format("Total readonly files {0}", readOnlyFileCount));
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

		private static void DisplaySizes(IEnumerable<SizeInformation> sizes)
		{
			var enumerator = sizes.GetEnumerator();
			var itemsDisplayed = 0;
			while (enumerator.MoveNext() && itemsDisplayed < maximumItemsToDisplay)
			{
				Console.WriteLine(string.Format("{0} - {1}", enumerator.Current.Item, enumerator.Current.Size));
				itemsDisplayed++;
			}
		}

		private static IEnumerable<SizeInformation> CalculateSizes(CalculationMode calculationMode, string baseDirectory)
		{
			return calculationMode == CalculationMode.ByDirectorySize ?
				CalculateSizesBySizeInformation(baseDirectory) :
				CalculateSizesByFileCategory(baseDirectory);
		}

		private static IEnumerable<SizeInformation> CalculateSizesByFileCategory(string baseDirectory)
		{
			var categorySizes = new Dictionary<string, uint>();
			GetSizeInformationByFileCategory(baseDirectory, categorySizes);

			var sortedFileCategorySizes = SortFileCategorySizes(categorySizes);

			return ExtractSizesFromSortedSizeData(sortedFileCategorySizes);
		}

		private static SortedDictionary<uint, List<string>> SortFileCategorySizes(Dictionary<string, uint> categorySizes)
		{
			var sortedFileCategorySizes = new SortedDictionary<uint, List<string>>(new DescendingComparer<uint>());
			foreach (var keyValuePair in categorySizes)
			{
				if (!sortedFileCategorySizes.ContainsKey(keyValuePair.Value))
				{
					sortedFileCategorySizes[keyValuePair.Value] = new List<string>();
				}

				sortedFileCategorySizes[keyValuePair.Value].Add(keyValuePair.Key);
			}

			return sortedFileCategorySizes;
		}

		private static IEnumerable<SizeInformation> CalculateSizesBySizeInformation(string baseDirectory)
		{
			var directoriesToAnalyze = Directory.GetDirectories(baseDirectory);
			var subSizeInformations = new SortedDictionary<uint, List<string>>(new DescendingComparer<uint>());
			foreach (var subDirectory in directoriesToAnalyze)
			{
				var directorySize = GetDirectorySize(subDirectory);
				if (!subSizeInformations.ContainsKey(directorySize))
				{
					subSizeInformations[directorySize] = new List<string>();
				}

				subSizeInformations[directorySize].Add(subDirectory);
			}

			return ExtractSizesFromSortedSizeData(subSizeInformations);
		}

		private static IEnumerable<SizeInformation> ExtractSizesFromSortedSizeData(SortedDictionary<uint, List<string>> sortedSizeData)
		{
			foreach (var size in sortedSizeData.Keys)
			{
				foreach (var subDirectory in sortedSizeData[size])
				{
					yield return new SizeInformation(subDirectory, size);
				}
			}
		}

		private static void GetSizeInformationByFileCategory(string directory, IDictionary<string, uint> sizes)
		{
			foreach (var subDirectory in Directory.GetDirectories(directory))
			{
				GetSizeInformationByFileCategory(subDirectory, sizes);
			}

			foreach (var file in Directory.GetFiles(directory))
			{
				var fileInfo = new FileInfo(file);
				if (fileInfo.IsReadOnly)
				{
					readOnlyFileCount++;
				}

				if (!sizes.ContainsKey(fileInfo.Extension))
				{
					sizes[fileInfo.Extension] = 0;
				}

				sizes[fileInfo.Extension] += (uint) fileInfo.Length;
			}
		}

		private static uint GetDirectorySize(string directory)
		{
			uint size = 0;

			foreach (var subDir in Directory.GetDirectories(directory))
			{
				size += GetDirectorySize(subDir);
			}

			foreach (var file in Directory.GetFiles(directory))
			{
				var fileInfo = new FileInfo(file);
				if (fileInfo.IsReadOnly)
				{
					readOnlyFileCount++;
				}

				size += (uint) fileInfo.Length;
			}

			return size;
		}
	}
}
