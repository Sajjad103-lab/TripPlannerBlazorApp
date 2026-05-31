⚠️  IMPORTANT — One-Time Setup Step
=====================================

If you see "no such table: Bookings" or any login issues,
your existing tripplanner.db file is outdated (missing the Bookings table).

FIX (do this once):
--------------------
1. Stop the application in Visual Studio (Shift+F5)
2. Delete the file:  tripplanner.db
   (it lives in the same folder as TripPlannerApp.csproj)
3. Restart the app (F5)

The app will automatically recreate the database with ALL tables
including the new Bookings table, and seed the demo account.

Demo credentials:
  Email:    demo@tripplanner.com
  Password: Demo@1234!
