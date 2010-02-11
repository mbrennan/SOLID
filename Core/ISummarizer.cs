using System.IO;

namespace DirectorySizeCounter.Core
{
	public interface ISummarizer
	{
		void ConsiderFile(FileInfo file);

		string Summary
		{
			get;
		}
	}
}
