using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DirectorySizeCounter
{
	internal class CommandLineArguments
	{
		private const string calculatorSwitch = "/calculator:";
		private const string summarizerSwitch = "/summarizer:";
		private static readonly string defaultCalculator = CommandLineOptions.DefaultCalculatorName;
		private static readonly string defaultSummarizer = CommandLineOptions.DefaultSummarizerName;

		public CommandLineArguments(IEnumerable<string> arguments)
		{
			BaseDirectory = FindBaseDirectory(arguments);
			var calculatorSelector = DetermineSelector(arguments, calculatorSwitch) ?? defaultCalculator;
			var summarizerSelector = DetermineSelector(arguments, summarizerSwitch) ?? defaultSummarizer;

			SizeCalculatorType = CommandLineOptions.FindCalculatorTypeByIdentifier(calculatorSelector);
			SummarizerType = summarizerSelector == null ? null :
							 CommandLineOptions.FindSummarizerTypeByIdentifier(summarizerSelector);
		}

		public string SizeCalculatorType
		{
			get;
			private set;
		}

		public string SummarizerType
		{
			get;
			private set;
		}

		public string BaseDirectory
		{
			get;
			private set;
		}

		public static void ShowUsage()
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

		private static string FindBaseDirectory(IEnumerable<string> arguments)
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
	}
}
