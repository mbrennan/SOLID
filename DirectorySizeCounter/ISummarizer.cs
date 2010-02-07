using System.IO;

namespace DirectorySizeCounter
{
	internal interface ISummarizer
	{
		void ConsiderFile(FileInfo file);

		uint ReadOnlyFileCount
		{
			get;
		}
	}
}