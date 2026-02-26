using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AIS.Warehouse.UI.Views
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status)
                {
                    case "IN": return Brushes.Green; // Приход
                    case "OUT": return Brushes.Red;  // Расход
                    default: return Brushes.Black;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UserRoleToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string role)
            {
                switch (role)
                {
                    case "Admin": return Brushes.DarkBlue;
                    case "Employee": return Brushes.DarkGreen;
                    default: return Brushes.Black;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LogTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string action)
            {
                if (action.Contains("добавил") || action.Contains("создал"))
                    return Brushes.Green;
                if (action.Contains("изменил") || action.Contains("обновил"))
                    return Brushes.Orange;
                if (action.Contains("удалил"))
                    return Brushes.Red;
                if (action.Contains("вошел") || action.Contains("вышел"))
                    return Brushes.Blue;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}