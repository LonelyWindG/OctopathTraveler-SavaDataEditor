using System.Linq;

namespace OctopathTraveler
{
	static class Util
	{
		public static void WriteNumber(uint address, uint size, uint value, uint min, uint max)
		{
			if (value < min) value = min;
			if (value > max) value = max;
			SaveData.Instance().WriteNumber(address, size, value);
		}

		public static uint FindFirstAddress(string name, uint index)
		{
			return SaveData.Instance().FindAddress(name, index).First();
		}

		public static bool TryFindFirstAddress(string name, uint index, out uint address)
		{
			var addresses = SaveData.Instance().FindAddress(name, index);
			bool found = addresses.Count > 0;
			address = found ? addresses[0] : uint.MaxValue;
			return found;
        }

		public static uint ReadNumber(this GVASData gvasData)
		{
			return SaveData.Instance().ReadNumber(gvasData.Address, gvasData.Size);
		}

		public static uint ReadNumber(this GVAS gvas, string key)
		{
			return ReadNumber(gvas.Key(key));
		}
	}
}
