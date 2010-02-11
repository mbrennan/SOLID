using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DirectorySizeCounter
{
	internal static class CommandLineOptions
	{
		private const string documentFileName = "CommandLineOptions.xml";
		private const string calculatorElementName = "Calculator";
		private const string summarizerElementName = "Summarizer";
		private const string nameAttribute = "name";
		private const string descriptionAttribute = "description";
		private const string typeAttribute = "type";

		private static readonly XElement optionsDocument = XElement.Load(documentFileName);

		public static string FindCalculatorTypeByIdentifier(string identifier)
		{
			return FindTypeFromElementWithSpecificTypeName(calculatorElementName, identifier);
		}

		public static string FindSummarizerTypeByIdentifier(string identifier)
		{
			return FindTypeFromElementWithSpecificTypeName(summarizerElementName, identifier);
		}

		public static IEnumerable<CommandLineOption> FindCalculatorCommandLineOptionsWithDescriptions()
		{
			return FindCommandLineOptionsForElementType(calculatorElementName);
		}

		public static IEnumerable<CommandLineOption> FindSummarizerCommandLineOptionsWithDescriptions()
		{
			return FindCommandLineOptionsForElementType(summarizerElementName);
		}

		private static IEnumerable<CommandLineOption> FindCommandLineOptionsForElementType(string elementName)
		{
			return from element in optionsDocument.Descendants(elementName)
				   select new CommandLineOption(
					   (string) element.Attribute(nameAttribute),
					   (string) element.Attribute(descriptionAttribute));
		}

		private static string FindTypeFromElementWithSpecificTypeName(string elementName, string typeName)
		{
			return (from element in optionsDocument.Descendants(elementName)
			        where (string) element.Attribute(CommandLineOptions.typeAttribute) == typeName
			        select (string) element).First();
		}
	}
}