namespace DirectorySizeCounter
{
	internal class CommandLineOption
	{
		public CommandLineOption(string identifier, string description)
		{
			Identifier = identifier;
			Description = description;
		}

		public string Identifier
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}
	}
}