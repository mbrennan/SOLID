using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using DirectorySizeCounter.Core;

namespace DirectorySizeCounter
{
	internal class Program
	{
		private const string calculatorSwitch = "/calculator:";
		private const string summarizerSwitch = "/summarizer:";
		private const string defaultCalculator = CommandLineOptions.DefaultCalculatorName;
		private const string defaultSummarizer = CommandLineOptions.DefaultSummarizerName;

		private static void Main(string[] arguments)
		{
			if (arguments.Length == 0 || arguments.Length > 3)
			{
				ShowUsage();
				return;
			}

			var baseDirectory = DetermineBaseDirectory(arguments);
			var calculator = CreateSizeCalculatorFromArguments(arguments);
			var summarizer = CreateSummarizerFromArguments(arguments);
			ConsoleDisplayer.ShowStatusMessage(calculator);
			var calculationResult = summarizer != null ?
			    calculator.CalculateSizes(baseDirectory, summarizer) :
			    calculator.CalculateSizes(baseDirectory);
			ConsoleDisplayer.DisplaySizes(calculationResult.Sizes);
			ConsoleDisplayer.DisplaySummary(calculationResult.Summary);
		}

		private static string DetermineBaseDirectory(IEnumerable<string> arguments)
		{
			var baseDirectory = Environment.CurrentDirectory;

			foreach (var argument in arguments)
			{
				if (argument.StartsWith("/"))
					continue;

				if (Directory.Exists(argument))
					return argument;

				var subDirectory = Path.Combine(Environment.CurrentDirectory, argument);
				if (Directory.Exists(subDirectory))
					return subDirectory;
			}

			return baseDirectory;
		}

		private static ISizeCalculator CreateSizeCalculatorFromArguments(IEnumerable<string> arguments)
		{
			var calculatorIdentifier = DetermineSelector(arguments, calculatorSwitch) ?? defaultCalculator;

			return CreateCalculatorFromIdentifier(calculatorIdentifier);
		}

		private static ISummarizer CreateSummarizerFromArguments(IEnumerable<string> arguments)
		{
			var summarizerIdentifier = DetermineSelector(arguments, summarizerSwitch);
			if (summarizerIdentifier == null)
				return null;

			return CreateSummarizerFromIdentifier(summarizerIdentifier);
		}

		private static string DetermineSelector(IEnumerable<string> arguments, string commandLineSwitch)
		{
			foreach (var argument in arguments)
			{
				if (!argument.ToLower().StartsWith(commandLineSwitch))
					continue;

				return argument.Replace(commandLineSwitch, string.Empty);
			}

			return null;
		}

		private static ISizeCalculator CreateCalculatorFromIdentifier(string identifier)
		{
			return CreateType<ISizeCalculator>(CommandLineOptions.FindCalculatorTypeByIdentifier(identifier));
		}

		private static ISummarizer CreateSummarizerFromIdentifier(string identifier)
		{
			return CreateType<ISummarizer>(CommandLineOptions.FindSummarizerTypeByIdentifier(identifier));
		}

		private static TypeName CreateType<TypeName>(string typeName)
		{
			var type = Type.GetType(typeName);
			var constructor = type.GetConstructor(new Type[]{});
			return (TypeName) constructor.Invoke(new object[]{});
		}

		private static void ShowUsage()
		{
			Console.WriteLine(string.Format("Usage:  {0} [directory] [{1}<calculator>] [{2}<summarizer>]",
			                                Assembly.GetExecutingAssembly().GetName().Name,
			                                calculatorSwitch,
			                                summarizerSwitch));
			Console.WriteLine("    Calculators:");
			Console.WriteLine(GetUsagesForCalculators());
			Console.WriteLine(string.Empty);
			Console.WriteLine("    Summarizers:");
			Console.WriteLine(GetUsagesForSummarizers());
		}

		private static string GetUsagesForCalculators()
		{
			return GetUsagesForCommandLineOptions(CommandLineOptions.FindCalculatorCommandLineOptionsWithDescriptions());
		}

		private static string GetUsagesForSummarizers()
		{
			return GetUsagesForCommandLineOptions(CommandLineOptions.FindSummarizerCommandLineOptionsWithDescriptions());
		}

		private static string GetUsagesForCommandLineOptions(IEnumerable<CommandLineOption> commandLineOptions)
		{
			var usages = new StringBuilder();
			foreach (var commandLineOption in commandLineOptions)
			{
				usages.Append("    ");
				usages.Append(commandLineOption.Identifier);
				usages.Append(":  ");
				usages.Append(commandLineOption.Description);
				usages.Append(Environment.NewLine);
				
			}

			usages.Remove(usages.Length - Environment.NewLine.Length, Environment.NewLine.Length);

			return usages.ToString();
		}
	}
}