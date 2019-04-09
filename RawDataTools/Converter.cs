using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RawDataTools
{
    /// <summary>
    /// Double 与 string 保留精度的转换
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class DoubleStringConverter : IValueConverter
    {
        /// <summary>
        /// 当值从绑定源传播到目标时，调用方法Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format("{0:F6}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            double num = 0.0;
            if (double.TryParse(str, out num))
            {
                return num;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 根据value 设置背景色保留精度的转换
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class ValueToColor : IValueConverter
    {
        /// <summary>
        /// 当值从绑定源传播到目标时，调用方法Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double num = (double)value;
            if (num >= 2.0 || num <= 0.0)
            {
                return "red";
            }
            else
            {
                return "black";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            double num = 0.0;
            if (double.TryParse(str, out num))
            {
                return num;
            }
            else
            {
                return null;
            }
        }
    }

}
