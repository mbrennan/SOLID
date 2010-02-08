using System.IO;

namespace DirectorySizeCounter
{
	internal class ProxiedSummarizer : ISummarizer
	{
		private readonly ISummarizer realSummarizer;

		public ProxiedSummarizer(ISummarizer realSummarizer)
		{
			this.realSummarizer = realSummarizer;
			HasRealSummarizer = realSummarizer != null;
		}

		public bool HasRealSummarizer
		{
			get;
			private set;
		}

		public void ConsiderFile(FileInfo file)
		{
			if (!HasRealSummarizer)
				return;

			realSummarizer.ConsiderFile(file);
		}

		public uint ReadOnlyFileCount
		{
			get
			{
				if (!HasRealSummarizer)
					return 0;

				return realSummarizer.ReadOnlyFileCount;
			}
		}
	}
}