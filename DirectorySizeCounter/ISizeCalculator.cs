namespace DirectorySizeCounter
{
	internal interface ISizeCalculator
	{
		CalculationResult CalculateSizes(string baseDirectory);
		CalculationResult CalculateSizes(string baseDirectory, ISummarizer summarizer);

		string StatusMessage
		{
			get;
		}
	}
}