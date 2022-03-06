using System;

namespace UsefulMethods
{
    public class Converters
    {
        public T ConvertToEnum<T>(string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }

        public string ConvertToString<T>(T enumValue)
        {
            return Enum.GetName(enumValue.GetType(), enumValue);
        }
    }
}