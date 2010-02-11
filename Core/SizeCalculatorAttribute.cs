namespace DirectorySizeCounter.Core
{
	internal class SizeCalculatorAttribute : CommandLineIdentifierAttribute
	{
		public SizeCalculatorAttribute(string commandLineIdentifier, string description) : base(commandLineIdentifier, description)
		{
		}
	}
}