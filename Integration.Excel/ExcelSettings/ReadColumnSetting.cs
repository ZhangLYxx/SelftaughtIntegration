using System;
using System.Reflection;

namespace Integration.Excel.ExcelSettings
{
    public class ReadColumnSetting<T> : IReadColumnSetting<T> where T : class, IReadData, new()
    {
        public ReadColumnSetting()
        {
            Set = SetByProperty;
            IsRequired = true;
        }
        private Type _realType;

        public Action<object, T> Set { get; set; }

        public bool IsRequired { get; set; }

        public int Index { get; set; }


        public object DefaultValue { get; set; }


        public string Title { get; set; }


        public Type DataType { get; set; }


        public PropertyInfo PropertyInfo { get; set; }


        private Type GetRealType()
        {
            if (_realType == null)
            {
                _realType = DataType.IsGenericType ? DataType.GetGenericArguments()[0] : DataType;
            }
            return _realType;
        }

        /// <summary>
        /// 标题
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public ReadColumnSetting<T> HasTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public ReadColumnSetting<T> HasDefaultValue(object defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        /// <summary>
        /// 赋值方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public ReadColumnSetting<T> HasSetAction(Action<object, T> action)
        {
            Set = action;
            return this;
        }

        /// <summary>
        /// 必须
        /// </summary>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public ReadColumnSetting<T> Require(bool isRequired = true)
        {
            IsRequired = isRequired;
            return this;
        }

        private void SetByProperty(object v, T data)
        {
            if (IsRequired)
            {
                if (v is string s && string.IsNullOrWhiteSpace(s) || v is null)
                {
                    data.ErrorMessages.Add($"{Title}必须。");
                    return;
                }
            }

            var type = GetRealType();

            if (v.GetType() == type)
            {
                PropertyInfo.SetValue(data, v);
                return;
            }

            if (TryParse(v.ToString(), type, out var val))
            {
                PropertyInfo.SetValue(data, val);
            }
            else
            {
                data.ErrorMessages.Add($"{Title}数据格式不正确({v})。");
            }
        }

        private static bool TryParse(string input, Type dataType, out object value)
        {
            var isOk = false;
            switch (dataType.FullName)
            {
                case "System.String":
                    isOk = true;
                    value = input;
                    break;

                case "System.Int16":
                    isOk = short.TryParse(input, out var s);
                    value = s;
                    break;
                case "System.UInt16":
                    isOk = ushort.TryParse(input, out var s1);
                    value = s1;
                    break;

                case "System.Int32":
                    isOk = int.TryParse(input, out var i);
                    value = i;
                    break;

                case "System.UInt32":
                    isOk = uint.TryParse(input, out var i1);
                    value = i1;
                    break;

                case "System.Int64":
                    isOk = long.TryParse(input, out var l);
                    value = l;
                    break;

                case "System.UInt64":
                    isOk = ulong.TryParse(input, out var l1);
                    value = l1;
                    break;

                case "System.Decimal":
                    isOk = decimal.TryParse(input, out var d);
                    value = d;
                    break;

                case "System.Float":
                    isOk = float.TryParse(input, out var f);
                    value = f;
                    break;

                case "System.DateTime":
                    isOk = DateTime.TryParse(input, out var date);
                    value = date;
                    break;

                default:
                    value = null;
                    break;
            }
            return isOk;
        }
    }
}
