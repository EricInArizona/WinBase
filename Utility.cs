// Copyright Eric Chauvin 2022



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms; // MessageBox, etc.
using System.IO;



  static class Utility
  {


  internal static void ParseCsvString(
             string InString, List<string> Fields )
    {
    StringBuilder SBuilder = new StringBuilder();

    Fields.Clear();
    bool InsideQuote = false;
    for( int Count = 0; Count < InString.Length;
                                         Count++ )
      {
      if( InString[Count] == '"' )
        {
        InsideQuote = !InsideQuote;
        continue; // Don't want the quote character.
        }

      if( InString[Count] == ',' )
        {
        if( !InsideQuote )
          {
          Fields.Add( SBuilder.ToString() );
          SBuilder.Length = 0;
          }

        continue; // Don't want this comma.
        }

      SBuilder.Append( Char.ToString(
                               InString[Count] ) );
      }

    // Get that last string.
    if( SBuilder.Length > 0 )
      Fields.Add( SBuilder.ToString() );

    }



/*
  internal static void ParseNonAscii(
           string InString, List<string> Fields )
    {
    StringBuilder SBuilder = new StringBuilder();

    Fields.Clear();
    for( int Count = 0; Count < InString.Length;
                                          Count++ )
      {
      if( InString[Count] > 127 )
        {
        Fields.Add( SBuilder.ToString() );
        SBuilder.Length = 0;
              // Clear it to start a new string.
        continue; // Don't want this character.
        }

      SBuilder.Append( Char.ToString(
                               InString[Count] ) );
      }

    // Get that last string.
    if( SBuilder.Length > 0 )
      Fields.Add( SBuilder.ToString() );

    }
*/



  internal static string CleanAsciiTextField(
                                string InString,
                                bool KeepTab,
                                int HowLong )
    {
    StringBuilder SBuilder = new StringBuilder();

    for( int Count = 0; Count < InString.Length;
                                        Count++ )
      {
      if( InString[Count] == '\t' )
        {
        if( KeepTab )
          SBuilder.Append( Char.ToString(
                            InString[Count] ) );

        continue;
        }

      if( InString[Count] >= 127 )
        continue; // Don't want this character.

      if( InString[Count] < ' ' )
        continue;

      SBuilder.Append( Char.ToString(
                             InString[Count] ) );
      }

    string Result = SBuilder.ToString().Trim();
    if( Result.Length > HowLong )
      Result = Result.Remove( HowLong );

    return Result;
    }





  /*
  internal static int CompareByASCIIString(
                string InString1, string InString2 )
    {
    int HowFar = InString1.Length;
    if( InString2.Length < HowFar )
      HowFar = InString2.Length;

    for( int Count = 0; Count < HowFar; Count++ )
      {
      if( InString1[Count] == InString2[Count] )
        continue;

      if( InString1[Count] < InString2[Count] )
        return -1;
      else
        return 1;

      }

    if( InString1.Length == InString2.Length )
      return 0;

    if( InString1.Length < InString2.Length )
      return -1;
    else
      return 1;

    }
  */




  internal static string
              ShowLongitudeInDegreeMinSec(
                       double Longitude )
    {
    if( Math.Abs( Longitude ) > 200 )
      return "";

    int Degrees = (int)Math.Abs( Math.Truncate(
                                   Longitude ));
    string Direction = "W";
    if( Longitude > 0 )
      Direction = "E";

    double Temp = Math.Abs( Longitude ) - Degrees;
    Temp = Temp * 60;

    int Minutes = (int)Math.Truncate( Temp );

    Temp = Temp - Minutes;
    double Seconds = Temp * 60;

    return Degrees.ToString() + "d " +
           Minutes.ToString() + "' " +
           Seconds.ToString( "N2" ) + "\" " +
           Direction;
    }



  internal static string
                ShowLatitudeInDegreeMinSec(
                      double Latitude )
    {
    if( Math.Abs( Latitude ) > 200 )
      return "";

    int Degrees = (int)Math.Abs( Math.Truncate(
                                      Latitude ));
    string Direction = "N";
    if( Latitude < 0 )
      Direction = "S";

    double Temp = Math.Abs( Latitude ) - Degrees;
    Temp = Temp * 60;

    int Minutes = (int)Math.Truncate( Temp );

    Temp = Temp - Minutes;
    double Seconds = Temp * 60;

    return Degrees.ToString() + "d " +
           Minutes.ToString() + "' " +
           Seconds.ToString( "N2" ) + "\" " +
           Direction;
    }




  internal static double GetLongFromDegreeMinSec(
                                string InString )
    {
    double Degrees;
    double Minutes;
    double Seconds;

    // 157 28' 24.3640" W
    string CleanS = CleanAsciiTextField(
                          InString, false, 100 );
    // string CleanS = InString.Replace( "", "" );
    CleanS = CleanS.Replace( "\'", "" );
    CleanS = CleanS.Replace( "\"", "" );
    CleanS = CleanS.Replace( "d", "" );

    string[] SplitS = CleanS.Split(
                          new Char[] { ' ' } );
    if( SplitS.Length < 4 )
      return -180.0;

    try
    {
    Degrees = Double.Parse( SplitS[0] );
    }
    catch
      {
      return -180.0;
      }

    try
    {
    Minutes = Double.Parse( SplitS[1] );
    }
    catch
      {
      return -180.0;
      }

    try
    {
    Seconds = Double.Parse( SplitS[2] );
    }
    catch
      {
      return -180.0;
      }

    // return 5000;

    Minutes = Minutes / 60.0;
    Seconds = Seconds / (60 * 60);

    double Result = Degrees + Minutes + Seconds;

    if( (SplitS[3].Contains( "W" )) ||
                       (SplitS[3].Contains( "w" )) )
      Result = -Result;

    return Result;
    }




  internal static double GetLatFromDegreeMinSec(
                                string InString )
    {
    // 70 57' 32.7283" N

    double Degrees;
    double Minutes;
    double Seconds;

    string CleanS = CleanAsciiTextField(
                        InString, false, 100 );
    // CleanS = InString.Replace( "", "" );
    CleanS = CleanS.Replace( "\'", "" );
    CleanS = CleanS.Replace( "\"", "" );
    CleanS = CleanS.Replace( "d", "" );

    string[] SplitS = CleanS.Split( new
                               Char[] { ' ' } );
    if( SplitS.Length < 4 )
      return 90.0;

    try
    {
    Degrees = Double.Parse( SplitS[0] );
    }
    catch
      {
      return 90.0;
      }

    try
    {
    Minutes = Double.Parse( SplitS[1] );
    }
    catch
      {
      return 90.0;
      }

    try
    {
    Seconds = Double.Parse( SplitS[2] );
    }
    catch
      {
      return 90.0;
      }


    Minutes = Minutes / 60.0;
    Seconds = Seconds / (60 * 60);

    double Result = Degrees + Minutes + Seconds;
    if( (SplitS[3].Contains( "S" )) ||
                    (SplitS[3].Contains( "s" )) )
      Result = -Result;

    return Result;
    }



  internal static string GetAsciiOnly(
                               string InString )
    {
    StringBuilder SBuilder = new StringBuilder();

    for( int Count = 0; Count < InString.Length;
                                          Count++ )
      {
      if( InString[Count] >= 127 )
        continue; // Don't want this character.

      // if( InString[Count] < ' ' )
        // continue; // Space is lowest ASCII char.

      SBuilder.Append( Char.ToString(
                              InString[Count] ) );
      }

    return SBuilder.ToString();
    }


  internal static string RemoveTabAndTrim(
                                string InString )
    {
    if( InString == null )
      return "";

    StringBuilder SBuilder = new StringBuilder();

    for( int Count = 0; Count < InString.Length;
                                          Count++ )
      {
      if( InString[Count] == '\t' )
        continue; // Don't want this character.

      // if( InString[Count] < ' ' )
        // continue;

      SBuilder.Append( Char.ToString(
                              InString[Count] ) );
      }

    return SBuilder.ToString().Trim();
    }



  /*
  internal static string RemoveBelowAscii32(
                                  string InString )
    {
    if( InString == null )
      return "";

    StringBuilder SBuilder = new StringBuilder();

    for( int Count = 0; Count < InString.Length;
                                         Count++ )
      {
      if( InString[Count] < ' ' )
        continue; // Don't want this character.

      // if( InString[Count] < ' ' )
        // continue; // Space is lowest ASCII char.

      SBuilder.Append( Char.ToString(
                             InString[Count] ) );
      }

    return SBuilder.ToString().Trim();
    }
  */


/*
  internal static void ProjectToOrtho2D(
               Vector3 InVect, ref Vector3 OutVect )
    {
    // This is in a right handed coordinate system.
    // X is to the right, Y is up, and positive
    // Z goes toward you.
    // if( InVect.Z < 1 )
      // InVect.Z = 1;

    // OutVect = InVect;

    // ZFactor = 10.0 / OutVect.Z;
    // OutVect.X = ZFactor * OutVect.X * Scale;
              // Const_EyeToScreen = 10.0
    // OutVect.Y = -ZFactor * OutVect.Y * Scale;

    // double ZFactor = 2.0 / InVect.Z;
    OutVect.X = InVect.X - InVect.Z;
    OutVect.Y = InVect.Y + InVect.Z;
    OutVect.Z = 0;
    }
*/


/*
  internal static string GetDomainFromSMTPServer(
                                   string Server )
    {
    // "smtpout.secureserver.net"
    int Position = -1;
    int HowMany = 0;
    // Count down from the end.
    for( int Count = Server.Length - 1; Count >= 0;
                                        Count-- )
      {
      if( Server[Count] == '.' )
        {
        Position = Count;
        HowMany++;
        }
      }

    if( HowMany >= 2 )
      {
      if( Position > 0 )
        {
        try
        {
        // Remove the beginning part.
        Server = Server.Remove( 0, Position + 1 );
        }
        catch( Exception )
          {

          }
        }
      }

    return Server;
    }



  internal static string GetCleanAsciiString(
                    string InString, int HowLong )
    {
    if( InString == null )
      return "";

    if( InString.Length > HowLong )
      InString = InString.Remove( HowLong );

    // InString = InString.Trim();

    StringBuilder SBuilder = new StringBuilder();

    for( int Count = 0; Count < InString.Length;
                                          Count++ )
      {
      char ToCheck = InString[Count];

      if( ToCheck < ' ' )
        continue; // Don't want this character.

      //  Don't go higher than D800 (Surrogates).
      if( ToCheck > '~' )
        continue;

      SBuilder.Append( Char.ToString( ToCheck ));
      }

    String Result = SBuilder.ToString();
    return Result;
    }
*/


/*

  internal static string GetCleanUnicodeString(
                    string InString, int HowLong )
    {
    // http://en.wikipedia.org/wiki/UTF-16

    // Basic Multilingual Plane

    // The High Surrogates (U+D800..U+DBFF) and
    //                Low Surrogate
    // "(U+DC00..U+DFFF) codes are reserved for
    //               encoding non-BMP
    //    characters in UTF-16 by using a pair of
    //    16-bit codes."

    if( InString == null )
      return "";

    InString = InString.Trim();

    StringBuilder SBuilder = new StringBuilder();

    for( int Count = 0; Count < InString.Length;
                                         Count++ )
      {
      char ToCheck = InString[Count];

      if( ToCheck < ' ' )
        continue; // Don't want this character.

      //  Don't go higher than D800 (Surrogates).
      if( ToCheck >= 0xD800 )
        continue;

      // &ndash; maps to the Unicode character
      // U+02013.
      // Braille Patterns (280028FF)
      // if( (ToCheck > 0x2800) &&
                              (ToCheck < 0x28FF))
        // continue;

      SBuilder.Append( Char.ToString( ToCheck ));
      }

    String Result = SBuilder.ToString();
    if( Result.Length > HowLong )
      Result = Result.Remove( HowLong );

    return Result;
    }
*/


/*
  internal static string CleanFileName(
                              string InString )
    {
    if( InString == null )
      return "";

    InString = InString.Trim();

    StringBuilder SBuilder = new StringBuilder();

    for( int Count = 0; Count < InString.Length;
                                     Count++ )
      {
      char ToCheck = InString[Count];

      // if( ToCheck == ' ' )
        // ToCheck = '_';

      if( ToCheck != ' ' )
        {
        if( ToCheck < '0' )
          continue; // Don't want this character.

        if( ToCheck > 'z' )
          continue;

        if( (ToCheck > '9') && (ToCheck < 'A') )
          continue;

        if( (ToCheck > 'Z') && (ToCheck < 'a') )
          continue;

        }

      // If it's going to be the first character.
      if( SBuilder.Length == 0 )
        {
        // If it's a number.
        if( (ToCheck >= '0') && (ToCheck <= '9') )
          continue;

        if( ToCheck == ' ' )
          continue;

        }

      SBuilder.Append( Char.ToString( ToCheck ));
      }

    String Result = SBuilder.ToString();
    if( Result.Length > 50 )
      Result = Result.Remove( 50 );

    return Result;
    }
*/



  internal static double GetLatitudeFromString(
                                string InString )
    {
    string ValS = InString.Trim();
    double Val = 37.0;
    if( ValS.Length < 2 )
      return 37.0;

    try
    {
    Val = Double.Parse( ValS );
    }
    catch( Exception )
      {
      Val = 37.0;
      }

    if( Math.Abs( Val ) > 90 )
      Val = 37.0;

    return Val;
    }




  internal static double GetLongitudeFromString(
                                 string InString )
    {
    string ValS = InString.Trim();
    double Val = -98.0;
    if( ValS.Length < 2 )
      return -98.0;

    try
    {
    Val = Double.Parse( ValS );
    }
    catch( Exception )
      {
      Val = -98.0;
      }

    if( Math.Abs( Val ) > 200 )
      Val = -98.0;

    return Val;
    }



  internal static double GetScaleFromString(
                                 string InString )
    {
    string ValS = InString.Trim();
    double Val = 60.1;
    if( ValS.Length < 2 )
      return 60.1;

    try
    {
    Val = Double.Parse( ValS );
    }
    catch( Exception )
      {
      Val = 60.1;
      }

    if( Val > 10000000 )
      Val = 60.1;

    if( Val < 1 )
      Val = 60.1;

    return Val;
    }



  internal static string BooleToString( bool Val )
    {
    if( Val )
      return "Yes";
    else
      return "No";

    }




  internal static bool StringToBooleDefaultYes(
                                     string Val )
    {
    if( Val == "No" )
      return false;
    else
      return true;

    }



  internal static bool StringToBooleDefaultNo(
                                     string Val )
    {
    if( Val == "Yes" )
      return true;
    else
      return false;

    }



  internal static string CleanWebFileName(
                                string InString )
    {
    // This has to be characters that can be used
    // in _any_ file system,

    if( InString == null )
      return "";

    InString = InString.Trim();
    if( InString == "" )
      return "";

    StringBuilder SBuilder = new StringBuilder();

    for( int Count = 0; Count < InString.Length;
                                         Count++ )
      {
      char ToCheck = InString[Count];

      if( !((ToCheck == '.') || (ToCheck == '_')))
        {
        if( ToCheck < '0' )
          continue; // Don't want this character.

        if( ToCheck > 'z' )
          continue;

        if( (ToCheck > '9') && (ToCheck < 'A') )
          continue;

        if( (ToCheck > 'Z') && (ToCheck < 'a') )
          continue;

        }

      // If it's going to be the first character.
      if( SBuilder.Length == 0 )
        {
        // If it's a number.
        if( (ToCheck >= '0') && (ToCheck <= '9') )
          continue;

        if( ToCheck == '.' )
          continue;

        }

      SBuilder.Append( Char.ToString( ToCheck ));
      }

    String Result = SBuilder.ToString();
    if( Result.Length > 50 )
      Result = Result.Remove( 50 );

    if( Result.Contains( ".." ))
      return "";

    if( Result.Contains( ".exe" ))
      return "";

    if( Result.Contains( ".dll" ))
      return "";

    if( Result.Contains( ".bat" ))
      return "";

    if( Result.Contains( ".apk" ))
      return "";

    // If it has no extension.
    if( !Result.Contains( "." ))
      return "";

    return Result;
    }




  internal static bool IsAllUpperCase(
                                 string ToCheck )
    {
    for( int Count = 0; Count < ToCheck.Length;
                                        Count++ )
      {
      if( (ToCheck[Count] < 'A') ||
                            (ToCheck[Count] > 'Z'))
        return false;

      }

    return true;
    }


  }
