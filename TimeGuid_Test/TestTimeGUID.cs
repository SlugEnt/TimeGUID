/*
 * Copyright 2018 Scott Herrmann

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	https://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/


using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SlugEnt;


namespace SlugEnt.UnitTest {
    [TestFixture]
    [Parallelizable]
    public class TestTimeGuid {
        #region "Value Object Tests"


        // Validates two TimeGuids set to same value are indeed equal.
        [Test]
        public void Equals_2TimeGuids_SameTime_EqualEachOther () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today);

            Assert.True (x.Equals (y), "A10:  The two values were the same and should have equaled each other");
        }



        // Validate that the first argument being less than the 2nd returns false.
        [Test]
        public void Equals_2TimeGuids_FirstLessThanSecond_NotEqual () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today.AddMinutes (+2));

            Assert.False (x.Equals (y), "A10:  The two values were not the same and should NOT have equaled each other");
        }



        // Validate that the first argument being greater than the second returns false.
        [Test]
        public void Equals_2TimeGuids_FirstMoreThanSecond_NotEqual () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today.AddMinutes (3));
            TimeGuid y = new TimeGuid (today);

            Assert.False (x.Equals (y), "A10:  The two values were not the same and should NOT have equaled each other");
        }



        // Test 2 TimeGuids with same values, test equality signs == !=
        [Test]
        public void EqualitySigns_2TimeGuides_SameValue () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today);

            Assert.True ((x == y), "A10:  The two values were the same and should have equaled each other");
            Assert.False ((x != y), "A20:  The two values were the same and should have returned False for the != test");
        }


        // Test 2 TimeGuids with different values, test equality signs == !=
        [Test]
        public void EqualitySigns_2TimeGuides_DifferentValues () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today.AddMinutes (2));

            Assert.True ((x != y), "A10:  The two values were not the same and should have returned true");
            Assert.False ((x == y), "A20:  The two values were different and should have returned false for == comparison");
        }


        [Test]
        public void CompareTo_FirstLessThanSecond () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today.AddMinutes (2));

            Assert.Less (x.CompareTo (y), 0, "A10:  First value should have indicated it was less than second value.");
        }


        [Test]
        public void CompareTo_FirstGreaterThanSecond () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today.AddMinutes (-2));

            Assert.Greater (x.CompareTo (y), 0, "A10:  First value should have indicated it was more than second value.");
        }


        // Validates < works
        [Test]
        public void CompareTo_LessThanSymbol () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today.AddMinutes (2));

            Assert.True (x < y, "A10:  x should have been less than y");
            Assert.False (y < x, "A20:  Comparison should have been false.");
        }



        // Validates > works
        [Test]
        public void CompareTo_GreaterThanSymbol () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today.AddMinutes (-2));

            Assert.True (x > y, "A10:  x should have been greater than y");
            Assert.False (y > x, "A20:  Comparison should have been false.");
        }



        // Validates that <= symbol works
        [Test]
        public void CompareTo_LessThanOrEqualSymbol () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today.AddMinutes (2));
            TimeGuid z = new TimeGuid (today);

            Assert.True (x <= y, "A10:  x should have been less than y");
            Assert.True (x <= z, "A20:  x should have been equal to z");
            Assert.False (y <= x, "A30:  Comparison should have been false.");
        }



        // Validates >= works
        [Test]
        public void CompareTo_GreaterThanOrEqualSymbol () {
            DateTime today = DateTime.Now;

            TimeGuid x = new TimeGuid (today);
            TimeGuid y = new TimeGuid (today.AddMinutes (-2));
            TimeGuid z = new TimeGuid (today);

            Assert.True (x >= y, "A10:  x should have been greater than y");
            Assert.True (x >= z, "A11:  x should have been equal to z");
            Assert.False (y >= x, "A20:  Comparison should have been false.");
        }


        #endregion



        #region StaticFunction Tests"


        // Validate that Hour Conversion works.
        [Test]
        public void HourConversion_Works ([Range (0, 23)] int hr) {
            string val = CalculateTimeGuidPositionalValue (hr, "h");

            DateTime d = new DateTime (2000, 01, 01, hr, 0, 0);

            string guidValue = TimeGuid.ConvertDateTimeToTimeGuid (d);
            string guidChar = guidValue.Substring (0, 1);

            Assert.AreEqual (val, guidChar, "A1:  The hour did not convert to the expected ascii character.  Exp: " + val + "  Actual: " + guidValue);
        }


        // Validate that minute conversion works.
        [Test]
        public void MinuteConversion_Works ([Range (0, 59)] int minute) {
            string val = CalculateTimeGuidPositionalValue (minute, "m");

            DateTime d = new DateTime (2000, 01, 01, 0, minute, 0);
            string guidValue = TimeGuid.ConvertDateTimeToTimeGuid (d);
            string guidChar = guidValue.Substring (1, 1);

            Assert.AreEqual (val, guidChar, "A1:  The minute did not convert to the expected ascii character.  Exp: " + val + "  Actual: " + guidValue);
        }



        // Validate that Second conversion works.
        [Test]
        public void SecondConversion_Works ([Range (0, 59)] int second) {
            string val = CalculateTimeGuidPositionalValue (second, "s");

            DateTime d = new DateTime (2000, 01, 01, 0, 0, second);
            string guidValue = TimeGuid.ConvertDateTimeToTimeGuid (d);
            string guidChar = guidValue.Substring (2, 1);

            Assert.AreEqual (val, guidChar, "A1:  The second did not convert to the expected ascii character.  Exp: " + val + "  Actual: " + guidValue);
        }



        // Validate across a random sample of Hours, minutes and seconds that proper TimeGuid's are generated.
        // We also then generate a second random TimeGuid that is greater than the first and compare them to ensure 
        // equality operators are working correctly.
        [Test]
        public void TimeGUIDConversion_Works ([Random (0, 23, 5)] int hr, [Random (0, 59, 5)] int minute, [Random (0, 59, 5)] int second) {
            string valSec = CalculateTimeGuidPositionalValue (second, "s");
            string valMin = CalculateTimeGuidPositionalValue (minute, "m");
            string valHr = CalculateTimeGuidPositionalValue (hr, "h");
            string valCombo = valHr + valMin + valSec;


            DateTime d = new DateTime (2000, 01, 01, hr, minute, second);
            string guidValue = TimeGuid.ConvertDateTimeToTimeGuid (d);


            Assert.AreEqual (valCombo, guidValue, "A1:  The TimeGuid was not converted correctly. Exp: " + valCombo + "  Actual: " + guidValue);
            TestContext.WriteLine ("TimeGuid Value produced was:  " + guidValue);

            // Now perform the random equality comparison.

            Random rnd = new Random();
            int hr1 = rnd.Next (hr, 23);
            int min1;
            int sec1;
            if ( hr1 == hr ) {
                min1 = rnd.Next (minute, 59);
                if ( minute == min1 ) { sec1 = rnd.Next (second, 59); }
                else { sec1 = rnd.Next (0, 59); }
            }
            else {
                min1 = rnd.Next (0, 59);
                if ( minute == min1 ) { sec1 = rnd.Next (second, 59); }
                else { sec1 = rnd.Next (0, 59); }
            }


            // This just exits in the rare case that the random values chosen happen to be the exact same as the initial ones.  This
            // prevents a test failure.
            if ( hr1 == hr && min1 == minute && sec1 == second ) { return;}


            DateTime d2 = new DateTime (2000, 01, 01, hr1, min1, sec1);
            TimeGuid y = new TimeGuid (d2);
            TimeGuid x = new TimeGuid (d);

            Assert.True (x < y, "Expected the comparison of y[" + y.ToString + "] > x[" + x.ToString + "] to be true.  It was not.  Something Wrong! ");
            TestContext.WriteLine ("Comparing x[" + x.ToString + "] < y[" + y.ToString + "] was successful");
        }



        // Internal routine that performs same calculation as the TimeGuid engine should be running.
        private string CalculateTimeGuidPositionalValue (int digit, string type) {
            int startingValue;

            if ( type == "h" ) { startingValue = 65 + digit; }
            else {
                if ( digit < 8 ) { startingValue = 50 + digit; }
                else if ( digit < 34 ) { startingValue = 57 + digit; }
                else { startingValue = 97 + digit - 34; }
            }

            string val = Convert.ToChar (startingValue).ToString();
            return val;
        }


        #endregion


        #region "Misc Tests"


        // This test runs through all 60 minutes ensuring that from a sorting and comparison standpoint the next minute is considered greater than the previous.
        // Since seconds uses the same conversion math, we are also validating the seconds.
        [Test]
        public void ValidateMinuteSorting ([Range (0, 58)] int minute) {
            DateTime d = new DateTime (2019, 01, 11, 0, minute, 5);

            TimeGuid x = new TimeGuid (d);
            TimeGuid y = new TimeGuid (d.AddMinutes (1));

            Assert.True (y > x, "Expected the comparison of y[" + y.ToString + "] > x[" + x.ToString + "] to be true.  It was not.  Something Wrong! ");
            TestContext.WriteLine ("Comparing y[" + y.ToString + "] > x[" + x.ToString + "] was successful");
        }



        [Test]
        [TestCase("@BC",Description = "This is the first character for Hour that is not valid.")]
        [TestCase("9BC", Description = "Hour must start with an uppercase letter.  Start at A.")]
        [TestCase("YBC", Description = "Testing Hour value - Y is the first uppercase letter that is not valid for an hour.")]
        [TestCase("A1C", Description = "Testing Minute value - 1 is the first number that is not valid for a minute.")]
        [TestCase("A:C", Description = "Testing Minute value - : is the first character after Ascii 9 that is not valid for a minute.")]
        [TestCase("A@C", Description = "Testing Minute value - @ is the last character before Ascii A that is not valid for a minute.")]
        [TestCase("A[C", Description = "Testing Minute value - [ is the first character after Ascii Z that is not valid for a minute.")]
        [TestCase("A`C", Description = "Testing Minute value - ` is the last character before Ascii a that is not valid for a minute.")]
        [TestCase("A{C", Description = "Testing Minute value - { is the first character after Ascii z that is not valid for a minute.")]
        [TestCase("AB1", Description = "Testing second value - 1 is the first number that is not valid for a second.")]
        [TestCase("AB:", Description = "Testing second value - : is the first character after Ascii 9 that is not valid for a second.")]
        [TestCase("AB@", Description = "Testing second value - @ is the last character before Ascii A that is not valid for a second.")]
        [TestCase("AB[", Description = "Testing second value - [ is the first character after Ascii Z that is not valid for a second.")]
        [TestCase("AB`", Description = "Testing second value - ` is the last character before Ascii a that is not valid for a second.")]
        [TestCase("AB{", Description = "Testing second value - { is the first character after Ascii z that is not valid for a second.")]

        public void StringConstructorValidation_Failures_Work (string testString) {

            TimeGuid x;
            Assert.Throws<ArgumentOutOfRangeException> (() => x = new TimeGuid (testString),"Expected an invalid String value to throw an exception.");
        }



        // Validate the last values in the string ranges.
        [Test]
        [TestCase("A22")]
        [TestCase("X22")]
        [TestCase("A92")]
        [TestCase("A99")]
        [TestCase("Aa2")]
        [TestCase("Az2")]
        [TestCase("Azz")]
        [TestCase("Xzz")]
        public void StringConstructorValidation_Success_Work (string testString) {
            TimeGuid x;
            Assert.DoesNotThrow( () => x = new TimeGuid(testString), "Expected a valid string to succeed - String was: " + testString);
            
        }



        // This validates that the IsValidTimeGuidString works correctly.
        [Test]
        [TestCase("@BC", false, Description = "This is the first character for Hour that is not valid.")]
        [TestCase("9BC", false, Description = "Hour must start with an uppercase letter.  Start at A.")]
        [TestCase("YBC", false, Description = "Testing Hour value - Y is the first uppercase letter that is not valid for an hour.")]
        [TestCase("A1C", false, Description = "Testing Minute value - 1 is the first number that is not valid for a minute.")]
        [TestCase("A:C", false, Description = "Testing Minute value - : is the first character after Ascii 9 that is not valid for a minute.")]
        [TestCase("A@C", false, Description = "Testing Minute value - @ is the last character before Ascii A that is not valid for a minute.")]
        [TestCase("A[C", false, Description = "Testing Minute value - [ is the first character after Ascii Z that is not valid for a minute.")]
        [TestCase("A`C", false, Description = "Testing Minute value - ` is the last character before Ascii a that is not valid for a minute.")]
        [TestCase("A{C", false, Description = "Testing Minute value - { is the first character after Ascii z that is not valid for a minute.")]
        [TestCase("AB1", false, Description = "Testing second value - 1 is the first number that is not valid for a second.")]
        [TestCase("AB:", false, Description = "Testing second value - : is the first character after Ascii 9 that is not valid for a second.")]
        [TestCase("AB@", false, Description = "Testing second value - @ is the last character before Ascii A that is not valid for a second.")]
        [TestCase("AB[", false, Description = "Testing second value - [ is the first character after Ascii Z that is not valid for a second.")]
        [TestCase("AB`", false, Description = "Testing second value - ` is the last character before Ascii a that is not valid for a second.")]
        [TestCase("AB{", false, Description = "Testing second value - { is the first character after Ascii z that is not valid for a second.")]

        public void IsValidTimeGuidString_MethodWorksCorrectly(string testString, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, TimeGuid.IsValidTimeGuidString(testString),"Expected the method to return :" + expectedResult + " but it returned " + !expectedResult);
        }
        #endregion
    }
}