using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
=======
using DirectorySizeCounter.Core;
>>>>>>> Refactor to remove DIP violations

namespace DirectorySizeCounter
{
	internal class Program
	{
<<<<<<< HEAD
		private const string calculatorSwitch = "/calculator:";
		private const string summarizerSwitch = "/summarizer:";
		private const string defaultCalculator = DirectorySizeCalculator.CommandLineIdentifier;
		private const string defaultSummarizer = null; // no default summarizer type
		private static readonly ConsoleDisplayer displayer = new ConsoleDisplayer();
=======
>>>>>>> Refactor to remove DIP violations

		private static void Main(string[] arguments)
		{
			var commandLineArguments = new CommandLineArguments(arguments);

			if (arguments.Length == 0 || arguments.Length > 3)
			{
				CommandLineArguments.ShowUsage();
				return;
			}

<<<<<<< HEAD
			var baseDirectory = DetermineBaseDirectory(arguments);
			var calculator = CreateSizeCalculatorFromArguments(arguments);
			var summarizer = CreateSummarizerFromArguments(arguments);
			displayer.ShowStatusMessage(calculator);
			var calculationResult = summarizer != null ?
                calculator.CalculateSizes(baseDirectory, summarizer) :
			    calculator.CalculateSizes(baseDirectory);

			displayer.DisplaySizes(calculationResult.Sizes);
			displayer.DisplaySummary(calculationResult.Summary);
		}

		private static ISizeCalculator CreateSizeCalculatorFromArguments(IEnumerable<string> arguments)
		{
			var calculatorIdentifier = DetermineSelector(arguments, calculatorSwitch, defaultCalculator);

			return ObjectFactory.CreateObjectMatchingAttribute<ISizeCalculator, SizeCalculatorAttribute>(
				sizeCalculatorAttribute => sizeCalculatorAttribute.CommandLineIdentifier == calculatorIdentifier);
		}

		private static ISummarizer CreateSummarizerFromArguments(IEnumerable<string> arguments)
		{
			var summarizerIdentifier = DetermineSelector(arguments, summarizerSwitch, defaultSummarizer);
			if (summarizerIdentifier == null)
				return null;

			return ObjectFactory.CreateObjectMatchingAttribute<ISummarizer, SummarizerAttribute>(
				summarizer => summarizer.CommandLineIdentifier == summarizerIdentifier);
		}

		private static string DetermineSelector(IEnumerable<string> arguments, string commandLineSwitch, string defaultSelector)
		{
			var selector = defaultSelector;

			foreach (var argument in arguments)
			{
				if (!argument.ToLower().StartsWith(commandLineSwitch))
					continue;

				selector = argument.Replace(commandLineSwitch, string.Empty);
			}

			return selector;
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

		private static void ShowUsage()
		{
			Console.WriteLine(string.Format("Usage:  {0} [directory] [{1}<calculator>] [{2}<summarizer>]",
			                                Assembly.GetExecutingAssembly().GetName().Name,
			                                calculatorSwitch,
			                                summarizerSwitch));
			Console.WriteLine("    Calculators:");
			Console.WriteLine(GetUsagesForAttributeType<SizeCalculatorAttribute>());
			Console.WriteLine(string.Empty);
			Console.WriteLine("    Summarizers:");
			Console.WriteLine(GetUsagesForAttributeType<SummarizerAttribute>());
		}

		private static string GetUsagesForAttributeType<AttributeType>() where AttributeType : CommandLineIdentifierAttribute
		{
			var usages = new StringBuilder();
			foreach (var attributeInfo in AttributesRegistrar.FindAttributeInfo<AttributeType>())
			{
				var attribute = (CommandLineIdentifierAttribute) attributeInfo.Attribute;
				usages.Append("    ");
				usages.Append(attribute.CommandLineIdentifier);
				usages.Append(":  ");
				usages.Append(attribute.Description);
				usages.Append(Environment.NewLine);
			}

			usages.Remove(usages.Length - Environment.NewLine.Length, Environment.NewLine.Length);

			return usages.ToString();
		}
=======
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
>>>>>>> Refactor to remove DIP violations
	}
}
