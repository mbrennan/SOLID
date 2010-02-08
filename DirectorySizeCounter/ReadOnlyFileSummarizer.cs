using System.IO;

namespace DirectorySizeCounter
{
	[Summarizer(commandLineIdentifier, description)]
	internal class ReadOnlyFileSummarizer : ISummarizer
	{
		private const string commandLineIdentifier = "readonly";
		private const string description = "display total # of readonly files";

		public string GetSummary()
		{
			return string.Format("Total read-only file count: {0}", ReadOnlyFileCount);	
		}

		public void ConsiderFile(FileInfo file)
		{
			if (!file.IsReadOnly)
				return;

			ReadOnlyFileCount++;
		}

		public uint ReadOnlyFileCount
		{
			get;
			private set;
		}
	}
}