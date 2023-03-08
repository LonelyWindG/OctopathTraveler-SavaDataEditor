namespace OctopathTraveler
{
    class Place
	{
		private readonly uint mAddress;
		private readonly uint mBit;

		public Place(uint address, uint bit, string name)
		{
			mAddress = address;
			mBit = bit;
			Name = name;
		}

		public string Name { get; }

		public bool Visit
		{
			get { return SaveData.Instance().ReadBit(mAddress, mBit); }
			set { SaveData.Instance().WriteBit(mAddress, mBit, value); }
		}
	}
}
