namespace DirectorySizeCounter.Core
{
	public interface ISizeCalculator
	{
		CalculationResult CalculateSizes(string baseDirectory);
		CalculationResult CalculateSizes(string baseDirectory, ISummarizer summarizer);

		string StatusMessage
		{
			get;
		}
	}
}
