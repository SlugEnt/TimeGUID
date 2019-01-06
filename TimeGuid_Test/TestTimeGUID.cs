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
using SlugEnt;


namespace SlugEnt.UnitTest
{
	[TestFixture]
	[Parallelizable]
	public class TestTimeGuid
	{
		// Validate that Hour Conversion works.
		[Test]
		public void HourConversion_Works([Range(0, 23)] int hr) {
			string val = CalculateTimeGuidPositionalValue(hr, "h");

			DateTime d = new DateTime(2000, 01, 01, hr, 0, 0);

			string guidValue = TimeGuid.ConvertTimeToChar(d);
			string guidChar = guidValue.Substring(0, 1);

			Assert.AreEqual(val, guidChar, "A1:  The hour did not convert to the expected ascii character.  Exp: " + val + "  Actual: " + guidValue);
		}


		// Validate that minute conversion works.
		[Test]
		public void MinuteConversion_Works([Range(0, 59)] int minute) {
			string val = CalculateTimeGuidPositionalValue(minute, "m");

			DateTime d = new DateTime(2000, 01, 01, 0, minute, 0);
			string guidValue = TimeGuid.ConvertTimeToChar(d);
			string guidChar = guidValue.Substring(1, 1);

			Assert.AreEqual(val, guidChar, "A1:  The minute did not convert to the expected ascii character.  Exp: " + val + "  Actual: " + guidValue);
		}



		// Validate that Second conversion works.
		[Test]
		public void SecondConversion_Works([Range(0, 59)] int second) {
			string val = CalculateTimeGuidPositionalValue(second, "s");

			DateTime d = new DateTime(2000, 01, 01, 0, 0, second);
			string guidValue = TimeGuid.ConvertTimeToChar(d);
			string guidChar = guidValue.Substring(2, 1);

			Assert.AreEqual(val, guidChar, "A1:  The second did not convert to the expected ascii character.  Exp: " + val + "  Actual: " + guidValue);
		}



		// Validate across a random sample of Hours, minutes and seconds that proper TimeGuid's are generated.
		[Test]
		public void TimeGUIDConversion_Works([Random(0, 23, 5)] int hr, [Random(0, 59, 5)] int minute,
			[Random(0, 59, 5)] int second) {
			string valSec = CalculateTimeGuidPositionalValue(second, "s");
			string valMin = CalculateTimeGuidPositionalValue(minute, "m");
			string valHr = CalculateTimeGuidPositionalValue(hr, "h");
			string valCombo = valHr + valMin + valSec;


			DateTime d = new DateTime(2000, 01, 01, hr, minute, second);
			string guidValue = TimeGuid.ConvertTimeToChar(d);


			Assert.AreEqual(valCombo, guidValue, "A1:  The TimeGuid was not converted correctly. Exp: " + valCombo + "  Actual: " + guidValue);
			TestContext.WriteLine("Time TimeGuid Value produced was:  " + guidValue);
		}



		// Internal routine that performs same calculation as the TimeGuid engine should be running.
		private string CalculateTimeGuidPositionalValue(int digit, string type) {
			int startingValue;

			if ( type == "h" ) { startingValue = 65 + digit; }
			else {
				if ( digit < 8 ) { startingValue = 50 + digit; }
				else if ( digit < 34 ) { startingValue = 57 + digit; }
				else { startingValue = 97 + digit - 34;}
/*				if ( digit < 26 ) { startingValue = 65 + digit; }
				else if ( digit < 52 ) { startingValue = 97 + (digit - 26); }
				else { startingValue = 48 + (digit - 52); }
				*/
			}

			string val = Convert.ToChar(startingValue).ToString();
			return val;
		}

	}
}
