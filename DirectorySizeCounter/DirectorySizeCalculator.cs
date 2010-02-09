using System;
using System.Collections.Generic;
using System.IO;

namespace DirectorySizeCounter
{
	[SizeCalculator(CommandLineIdentifier, Description)]
	internal class DirectorySizeCalculator : SizeCalculator
	{
		public const string CommandLineIdentifier = "directorysize";
		private const string statusMessage = "calculating directory sizes...";
		private const string Description = "calculate by directory size";

		public override string StatusMessage
		{
			get
			{
				return statusMessage;
			}
		}

		protected override IEnumerable<SizeResult> GetSizes(string baseDirectory, ISummarizer summarizer)
		{
			var directoriesToAnalyze = Directory.GetDirectories(baseDirectory);
			var subDirectorySizeResults = new SortedDictionary<uint, List<string>>(new DescendingComparer<uint>());
			foreach (var subDirectory in directoriesToAnalyze)
			{
				var directorySize = GetDirectorySize(subDirectory, summarizer);
				if (!subDirectorySizeResults.ContainsKey(directorySize))
				{
					subDirectorySizeResults[directorySize] = new List<string>();
				}

				subDirectorySizeResults[directorySize].Add(subDirectory);
			}

			return ExtractSizesFromSortedSizeData(subDirectorySizeResults);
		}

		private static uint GetDirectorySize(string directory, ISummarizer summarizer)
		{
			uint size = 0;

			foreach (var subDir in Directory.GetDirectories(directory))
			{
				size += GetDirectorySize(subDir, summarizer);
			}

			foreach (var file in Directory.GetFiles(directory))
			{
				var fileInfo = new FileInfo(file);
				summarizer.ConsiderFile(fileInfo);

				size += (uint) fileInfo.Length;
			}

			return size;
		}
	}
}