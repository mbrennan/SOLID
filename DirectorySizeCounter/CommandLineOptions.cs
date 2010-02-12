using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DirectorySizeCounter
{
	internal static class CommandLineOptions
	{
		private const string documentFileName = "CommandLineOptions.xml";
		private const string calculatorElementName = "Calculator";
		private const string calculatersElementName = "Calculators";
		private const string summarizerElementName = "Summarizer";
		private const string summarizersElementName = "Summarizers";
		private const string nameAttribute = "name";
		private const string descriptionAttribute = "description";
		private const string typeAttribute = "type";
		private const string defaultAttributeName = "default";

		private static readonly XElement optionsDocument = XElement.Load(documentFileName);

		public static string DefaultCalculatorName
		{
			get
			{
				return FindDefaultFromElement(calculatersElementName);
			}
		}

		public static string DefaultSummarizerName
		{
			get
			{
				return FindDefaultFromElement(summarizersElementName);
			}
		}

		public static string FindCalculatorTypeByIdentifier(string identifier)
		{
			return FindTypeFromElementWithSpecificIdentifier(calculatorElementName, identifier);
		}

		public static string FindSummarizerTypeByIdentifier(string identifier)
		{
			return FindTypeFromElementWithSpecificIdentifier(summarizerElementName, identifier);
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
					   element.Attribute(nameAttribute).Value,
					   element.Attribute(descriptionAttribute).Value);
		}

		private static string FindTypeFromElementWithSpecificIdentifier(string elementName, string identifier)
		{
			return (from element in optionsDocument.Descendants(elementName)
					where element.Attribute(nameAttribute).Value == identifier
					select element.Attribute(typeAttribute).Value).First();
		}

		private static string FindDefaultFromElement(string elementName)
		{
			var defaults = from element in optionsDocument.Descendants(elementName)
						   where element.Attribute(defaultAttributeName) != null
						   select element.Attribute(defaultAttributeName).Value;

			return !defaults.Any() ? null : defaults.First();
		}
	}
}