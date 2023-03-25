# TimeGuid #

TimeGuid is a simple class used to represent a time down to the seconds level in only 3 characters.

## Use



            // Create a TimeGuid
            DateTime today = DateTime.Now;
            TimeGuid x = new TimeGuid (today);

            // Create a timeGuid 3 minutes in future.
            TimeGuid x = new TimeGuid (today.AddMinutes (3));



