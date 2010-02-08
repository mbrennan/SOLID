﻿using System.IO;

namespace DirectorySizeCounter
{
	[Summarizer(commandLineIdentifier, description)]
	internal class ReadOnlyFileSummarizer : ISummarizer
	{
		private uint readOnlyFileCount;
		private const string commandLineIdentifier = "readonly";
		private const string description = "display total # of readonly files";

		public void ConsiderFile(FileInfo file)
		{
			if (!file.IsReadOnly)
				return;

			readOnlyFileCount++;
		}

		public string Summary
		{
			get
			{
				return string.Format("Total read-only file count: {0}", readOnlyFileCount);	
			}
		}
	}
}
