using System.Collections.Generic;
using System.IO;

namespace DirectorySizeCounter.Core
{
	[SizeCalculator(commandLineIdentifier, description)]
	internal class FileCategorySizeCalculator : SizeCalculator
	{
		private const string commandLineIdentifier = "filecategory";
		private const string description = "calculate by file category";
		private const string statusMessage = "calcuting by file categories...";

		public override string StatusMessage
		{
			get
			{
				return statusMessage;
			}
		}

		protected override IEnumerable<SizeResult> GetSizes(string baseDirectory, ISummarizer summarizer)
		{
			var categorySizes = new Dictionary<string, uint>();
			GetSizeInformationByFileCategory(baseDirectory, categorySizes, summarizer);

			var sortedFileCategorySizes = SortFileCategorySizes(categorySizes);

			return ExtractSizesFromSortedSizeData(sortedFileCategorySizes);
		}

		private static void GetSizeInformationByFileCategory(string directory, IDictionary<string, uint> sizes, ISummarizer summarizer)
		{
			foreach (var subDirectory in Directory.GetDirectories(directory))
			{
				GetSizeInformationByFileCategory(subDirectory, sizes, summarizer);
			}

			foreach (var file in Directory.GetFiles(directory))
			{
				var fileInfo = new FileInfo(file);
				summarizer.ConsiderFile(fileInfo);

				if (!sizes.ContainsKey(fileInfo.Extension))
				{
					sizes[fileInfo.Extension] = 0;
				}

				sizes[fileInfo.Extension] += (uint) fileInfo.Length;
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
