using UsefulMethods;
namespace UsefulMethods.GenCommonData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
using UsefulMethods.GenCommonData;

    public static class UkPostcodes
    {
        public static List<PostcodeInfo> Generate(string csvLocation, int numberOfPostcodes)
        {
            if (string.IsNullOrEmpty(csvLocation))
                throw new ArgumentException("Value cannot be null or empty.", nameof(csvLocation));

            if (numberOfPostcodes == int.MaxValue)
            {
                return File.ReadAllLines(csvLocation)
                    .Skip(1)
                    .Select(PostcodeInfo.FromCsv)
                    .ToList();
            }

            return File.ReadLines(csvLocation).Take(numberOfPostcodes + 1)
                .Skip(1)
                .Select(PostcodeInfo.FromCsv)
                .ToList();
        }
    }

    public class PostcodeInfo
    {
        public int ID { get; private set; }
        public string Postcode { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public static PostcodeInfo FromCsv(string csvLine)
        {
            var values = csvLine.Split(',');

            return new PostcodeInfo
            {
                ID = Convert.ToInt32(values[0]),
                Postcode = Convert.ToString(values[1]),
                Latitude = Convert.ToDouble(values[2]),
                Longitude = Convert.ToDouble(values[3])
            };
        }
    }
}
