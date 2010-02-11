namespace DirectorySizeCounter.Core
{
	public class SizeResult
	{
		public SizeResult(string item, uint size)
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