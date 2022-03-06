using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Security;

namespace UsefulMethods.GenCommonData
{
    public class CommonDataGenerator
    {
        private static readonly Random rnd = new Random();

        public string RandomNumbersLetters(int length, StringInput stringInput)
        {
            string chars = null;

            switch (stringInput)
            {
                case StringInput.Numbers:
                    chars = "0123456789";
                    break;

                case StringInput.Letters:
                    chars = "abcdefghijklmnopqrstuvwxyz";
                    break;

                case StringInput.NumbersAndLetters:
                    chars = "0123456789abcdefghijklmnopqrstuvwxyz";
                    break;
            }

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public string GetRandomTitle(bool isUseExtendedTitleList)
        {
            if (isUseExtendedTitleList)
                return Faker.Name.Prefix();
            else
            {
                var titles = new List<string> { "Mr", "Mrs", "Miss", "Ms", "Dr" };

                return titles[rnd.Next(titles.Count)];
            }
        }

        public List<PostcodeInfo> GetUKPostcodes(int fromRangeCount)
        {
            var defaultPathToCsv = Path.Combine(Helper.ExecutingDirectory, "GenCommonData", "ukpostcodes.csv");

            return UkPostcodes.Generate(defaultPathToCsv, fromRangeCount);
        }

        public PostcodeInfo GetRandomUKPostcode(int fromRangeCount)
        {
            var postcodes = GetUKPostcodes(fromRangeCount);

            return postcodes[rnd.Next(postcodes.Count)];
        }

        public Address GenRandomUKAddress()
        {
            return new Address
            {
                AddressLine1 = Faker.Address.StreetName(),
                AddressLine2 = null,
                AddressLine3 = null,
                City = Faker.Address.City(),
                Postcode = GetRandomUKPostcode(10000).Postcode,
                County = Faker.Address.UkCounty(),
                Country = "United Kingdom",
            };
        }

        public Person GenRandomUKPerson()
        {
            var person = new Person
            {
                Title = GetRandomTitle(false),
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                DateOfBirth = Faker.Identification.DateOfBirth(),
                PhoneNumber = $"02{RandomNumbersLetters(9, StringInput.Numbers)}",
                MobileNumber = $"07{RandomNumbersLetters(9, StringInput.Numbers)}",
                UkNationalInsuranceNumber = Faker.Identification.UkNationalInsuranceNumber(),
                UkPassportNumber = Faker.Identification.UkPassportNumber(),
                UkNhsNumber = Faker.Identification.UkNhsNumber()
            };

            person.EmailAddress =
                $"{person.FirstName}.{person.LastName}.{person.MobileNumber}@test.com";

            return person;
        }

        public string GeneratePassword(int length)
        {
            if (length < 4)
                throw new Exception("Please enter value >= 4");

            var str1 = RandomNumbersLetters(1, StringInput.Letters).ToUpper();
            var str2 = RandomNumbersLetters(1, StringInput.Letters);
            var str3 = RandomNumbersLetters(1, StringInput.Numbers);

            var strPasswordMinRequirements = string.Concat(str1, str2, str3);

            return string.Concat(strPasswordMinRequirements, Membership.GeneratePassword(length - 3, 1));
        }

        public class Address
        {
            public string AddressLine1 { get; set; }

            public string AddressLine2 { get; set; }

            public string AddressLine3 { get; set; }

            public string City { get; set; }

            public string Postcode { get; set; }

            public string County { get; set; }

            public string Country { get; set; }
        }

        public class Person
        {
            public string Title { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string FullName => $"{FirstName} {LastName}";

            public DateTime DateOfBirth { get; set; }

            public string PhoneNumber { get; set; }

            public string MobileNumber { get; set; }

            public string EmailAddress { get; set; }

            public string UkNationalInsuranceNumber { get; set; }

            public string UkPassportNumber { get; set; }

            public string UkNhsNumber { get; set; }
        }

        public enum StringInput
        {
            Numbers,
            Letters,
            NumbersAndLetters,
        }
    }
}