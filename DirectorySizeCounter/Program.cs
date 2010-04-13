using System;
using DirectorySizeCounter.Core;

namespace DirectorySizeCounter
{
	internal class Program
	{
		private static void Main(string[] arguments)
		{
			var commandLineArguments = new CommandLineArguments(arguments);

			if (arguments.Length == 0 || arguments.Length > 3)
			{
				CommandLineArguments.ShowUsage();
				return;
			}

			var calculator = CreateObjectFromType<ISizeCalculator>(commandLineArguments.SizeCalculatorType);
			var summarizer = commandLineArguments.SummarizerType == null ? null :
							 CreateObjectFromType<ISummarizer>(commandLineArguments.SummarizerType);
			ConsoleDisplayer.ShowStatusMessage(calculator);
			var calculationResult = summarizer != null ?
			    calculator.CalculateSizes(commandLineArguments.BaseDirectory, summarizer) :
			    calculator.CalculateSizes(commandLineArguments.BaseDirectory);
			ConsoleDisplayer.DisplaySizes(calculationResult.Sizes);
			ConsoleDisplayer.DisplaySummary(calculationResult.Summary);
		}

		private static TypeName CreateObjectFromType<TypeName>(string typeName)
		{
			var type = Type.GetType(typeName);
			var constructor = type.GetConstructor(new Type[]{});
			return (TypeName) constructor.Invoke(new object[]{});
		}
	}
}
