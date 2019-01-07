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
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SlugEnt {
    /// <summary>
    /// Converts a time - 15:30:56 into a TimeGuid object represented as 3 recognizable and easily displayed characters - for instance - Ld7
    /// </summary>
    public struct TimeGuid : IEquatable<TimeGuid>, IComparable<TimeGuid> {
        private static short rA = 65;
        private static short rZ = 90;
        private static short ra = 97;
        private static short rz = 122;
        private static short r2 = 50;
        private static short r9 = 57;


        private string _value;


        // Creates a TimeGuid Value from a string.  Note, the string is validated and will throw an ArgumentException if it 
        // is not a proper TimeGuid String representation.
        public TimeGuid (string timeGuidValue) {
            int length = timeGuidValue.Length;

            if (length != 3) { throw new ArgumentOutOfRangeException("A TimeGuid object MUST be 3 characters long.");}

            // Validate Hour Character
            char hour = timeGuidValue [0];
            if (!IsValidHourSymbol(hour))  { throw new ArgumentOutOfRangeException("The Hour [First Character] is invalid.");}


            // Validate Minute Character
            char minute = timeGuidValue [1];
            if (!IsValidMinuteSecondSymbol(minute)) { throw new ArgumentOutOfRangeException("The minute value was invalid."); }


            // validate Second
            char second = timeGuidValue[2];
            if (!IsValidMinuteSecondSymbol(second)) { throw new ArgumentOutOfRangeException("The second value was invalid."); }

            _value = timeGuidValue;
        }



        /// <summary>
        /// Creates a TimeGuid based upon the DateTime parameter passed in.  
        /// </summary>
        /// <param name="time">The Datetime object that contains the Time we want to create a timeGuid for.</param>
        public TimeGuid (DateTime time) { _value = ConvertDateTimeToTimeGuid (time); }



        /// <summary>
        /// Retrieves the Value of this TimeGuid object as a string.
        /// </summary>
        public string ToString {
            get => _value;
        }


        #region "Static Functions"


        /// <summary>
        /// Converts Hours into a single character.  We use the upper case letters to represent hours.
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        private static char ConvertHoursToChar (int hours) {
            int start = rA + hours;
            return Convert.ToChar (start);
        }


        /// <summary>
        /// Converts Minutes into a single character.  Because there are 60 minutes, we have to use Uppercase, Lowercase and numbers
        /// to represent the minutes.
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        private static char ConvertMinutesToChar (int minutes) { return ConvertTimeUnit60ToChar (minutes); }



        /// <summary>
        /// Converts Seconds into a single character.  Because there are 60 seconds, we have to use Uppercase, Lowercase and numbers
        /// to represent the seconds.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private static char ConvertSecondsToChar (int seconds) { return ConvertTimeUnit60ToChar (seconds); }



        /// <summary>
        /// Converts a 60 number value into a single character.  In order to preserve proper sorting (at least via ascii) we start with
        /// numbers and then use Uppercase letters and then finally lowercase numbers.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static char ConvertTimeUnit60ToChar (int value) {
            if ( value < 8 ) { return Convert.ToChar (r2 + value); }
            else if ( value < 34 ) { return Convert.ToChar (rA - 8 + value); }
            else { return Convert.ToChar (ra - 34 + value); }
        }



        /// <summary>
        /// Converts the time part of the dateTime to a TimeGuid object (HMS)
        /// </summary>
        /// <param name="d">DateTime object containing the time you wish to convert.</param>
        /// <returns></returns>
        public static string ConvertDateTimeToTimeGuid (DateTime d) {
            int hr = d.Hour;
            int min = d.Minute;
            int sec = d.Second;

            return (ConvertHoursToChar (hr).ToString() + ConvertMinutesToChar (min).ToString() + ConvertSecondsToChar (sec).ToString());
        }



        /// <summary>
        /// Returns true if the specified character value is a valid symbol for the hour field.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidHourSymbol (char value) {
            if (value < rA || value > (88)) { return false; }

            return true;
        }



        /// <summary>
        /// Returns true if the specified character value is a valid symbol for a minute or second value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidMinuteSecondSymbol (char value) {
            if ( (value >= r2 && value <= r9) || (value >= rA && value <= rZ) || (value >= ra && value <= rz) ) { return true; }

            return false;
        }



        /// <summary>
        /// Validates a string to determine if it is a valid TimeGuid string.  Returns True if valid TimeGuid string.
        /// </summary>
        /// <param name="timeGuidValue">The string to check.</param>
        /// <returns></returns>
        public static bool IsValidTimeGuidString (string timeGuidValue) {
            int length = timeGuidValue.Length;

            if ( length != 3 ) { return false; }

            // Validate Hour Character
            char hour = timeGuidValue[0];
            if (!IsValidHourSymbol(hour)) { return false; }


            // Validate Minute Character
            char minute = timeGuidValue[1];
            if (!IsValidMinuteSecondSymbol(minute)) { return false; }


            // validate Second
            char second = timeGuidValue[2];
            if (!IsValidMinuteSecondSymbol(second)) { return false; }

            return true;
        }

        #endregion



        #region "Equality Methods"


        /// <summary>
        /// Are the 2 TimeGuids equal to each other?  True if they are, false otherwise.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals (TimeGuid other) { return (_value.Equals (other._value, StringComparison.Ordinal)); }


        // Math comparison functions
        public static bool operator == (TimeGuid x, TimeGuid y) { return x._value == y._value; }
        public static bool operator != (TimeGuid x, TimeGuid y) { return x._value != y._value; }
        public static bool operator > (TimeGuid x, TimeGuid y) { return x.CompareTo (y) > 0; }
        public static bool operator < (TimeGuid x, TimeGuid y) { return x.CompareTo (y) < 0; }
        public static bool operator >= (TimeGuid x, TimeGuid y) { return x.CompareTo (y) >= 0; }
        public static bool operator <= (TimeGuid x, TimeGuid y) { return x.CompareTo (y) <= 0; }


        /// <summary>
        /// Retrieves a hashcode for this TimeGuid.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode () { return this._value.GetHashCode(); }



        /// <summary>
        /// Returns the order of one TimeGuid to another.
        /// < 0 = This object is less than other object.
        /// = 0 = The two objects are equivalent.
        /// > 0 = This object is greater than the other object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo (TimeGuid other) {
            return string.CompareOrdinal (this._value, other._value);
        }


        #endregion
    }
}