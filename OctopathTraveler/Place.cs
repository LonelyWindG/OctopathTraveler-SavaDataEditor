namespace OctopathTraveler
{
	class Place
	{
		private readonly uint mAddress;
		private readonly uint mBit;

		public Place(uint address, uint bit, PlaceInfo info)
		{
			mAddress = address;
			mBit = bit;
            Info = info;
		}

		public PlaceInfo Info { get;}

        public bool Visit
		{
			get => IsVisit(mAddress, mBit);
			set => SaveData.Instance().WriteBit(mAddress, mBit, value);
		}

		public static bool IsVisit(uint address, uint bit)
		{
            return SaveData.Instance().ReadBit(address, bit);
        }
	}
}
