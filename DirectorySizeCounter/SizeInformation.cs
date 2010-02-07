namespace DirectorySizeCounter
{
	internal class SizeInformation
	{
		public SizeInformation(string item, uint size)
		{
			Item = item;
			Size = size;
		}

		public string Item
		{
			get;
			private set;
		}

		public uint Size
		{
			get;
			private set;
		}
	}
}
