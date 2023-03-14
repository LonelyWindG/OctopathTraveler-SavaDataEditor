using System;
using System.Globalization;
using System.Windows.Data;

namespace OctopathTraveler
{
    class ItemNameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Info.GetNameOrID2Hex(Info.Instance().Items, (uint)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
