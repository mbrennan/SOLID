using System.IO;

namespace DirectorySizeCounter.Core
{
	internal class ReadOnlyFileSummarizer : ISummarizer
	{
		private uint readOnlyFileCount;

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
