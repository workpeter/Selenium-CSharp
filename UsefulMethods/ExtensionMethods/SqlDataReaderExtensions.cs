using System;
using System.Data.SqlClient;

namespace UsefulMethods.ExtensionMethods
{
    public static class SqlDataReaderExtensions
    {
        private static readonly object lockObj = new object();

        public static string SafeGetString(this SqlDataReader reader, int colIndex)
        {
            lock (lockObj)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetString(colIndex) : string.Empty;
            }
        }

        public static DateTime SafeGetDate(this SqlDataReader reader, int colIndex)
        {
            lock (lockObj)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetDateTime(colIndex) : DateTime.MinValue;
            }
        }

        public static int SafeGetInt32(this SqlDataReader reader, int colIndex)
        {
            lock (lockObj)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetInt32(colIndex) : int.MinValue;
            }
        }

        public static long SafeGetInt64(this SqlDataReader reader, int colIndex)
        {
            lock (lockObj)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetInt64(colIndex) : long.MinValue;
            }
        }

        public static decimal SafeGetDecimal(this SqlDataReader reader, int colIndex)
        {
            lock (lockObj)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetDecimal(colIndex) : decimal.MinValue;
            }
        }

        public static double SafeGetDouble(this SqlDataReader reader, int colIndex)
        {
            lock (lockObj)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetDouble(colIndex) : double.MinValue;
            }
        }

        public static byte SafeGetByte(this SqlDataReader reader, int colIndex)
        {
            lock (lockObj)
            {
                return !reader.IsDBNull(colIndex) ? reader.GetByte(colIndex) : byte.MinValue;
            }
        }
    }
}