using System.Collections.Generic;
using System.IO;

namespace DirectorySizeCounter
{
	internal class SizeCalculator
	{
		private readonly CalculationMode calculationMode;
		private readonly string baseDirectory;
		private int readOnlyFileCount;

		public SizeCalculator(CalculationMode calculationMode, string baseDirectory)
		{
			this.calculationMode = calculationMode;
			this.baseDirectory = baseDirectory;
		}

		public IEnumerable<SizeInformation> CalculateSizes(out uint totalReadOnlyFiles)
		{
			readOnlyFileCount = 0;

			var sizes = calculationMode == CalculationMode.ByDirectorySize ?
			                                                               	CalculateSizesBySizeInformation() :
			                                                               	                                  	CalculateSizesByFileCategory();

			totalReadOnlyFiles = (uint) readOnlyFileCount;

			return sizes;
		}

		private IEnumerable<SizeInformation> CalculateSizesBySizeInformation()
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

		private IEnumerable<SizeInformation> CalculateSizesByFileCategory()
		{
			var categorySizes = new Dictionary<string, uint>();
			GetSizeInformationByFileCategory(baseDirectory, categorySizes);

			var sortedFileCategorySizes = SortFileCategorySizes(categorySizes);

			return ExtractSizesFromSortedSizeData(sortedFileCategorySizes);
		}

		private uint GetDirectorySize(string directory)
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

		private void GetSizeInformationByFileCategory(string directory, IDictionary<string, uint> sizes)
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
	}
}
