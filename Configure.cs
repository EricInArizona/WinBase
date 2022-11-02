// Copyright Eric Chauvin 2022.


// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;
using System.Collections.Generic;
using System.Text;
// using System.Windows.Forms; // MessageBox, etc.
using System.IO;



class Configure
  {
  private Dictionary<string, string> CDictionary;
  private string FileName = "";
  // private UTF8Encoding UEncoding;
  private const int MaxLength = 1024 * 4;



  internal Configure( string FileToUseName )
   {
    FileName = FileToUseName;
    // UEncoding = new UTF8Encoding();
    CDictionary = new Dictionary<string, string>();

    ReadFromTextFile();
    }




  internal int GetCount()
    {
    return CDictionary.Count;
    }



  internal string GetString( string KeyWord,
                             int Length )
    {
    KeyWord = KeyWord.ToLower().Trim();

    string Value;
    if( CDictionary.TryGetValue(
                             KeyWord, out Value ))
      {
      Value = Utility.getCleanAscii( Value,
                               false, Length );
      return Value;
      }
    else
      return "";

    }




  internal void SetString( string KeyWord,
                           string Value )
    {
    KeyWord = Utility.getCleanAscii(
         KeyWord.ToLower().Trim(), false, 100 );

    if( KeyWord == "" )
      return;
      // Can't add an empty keyword.

    Value = Utility.getCleanAscii(
                       Value, false, MaxLength );
    CDictionary[KeyWord] = Value;
    }




  internal void ClearAllOptions()
    {
    CDictionary.Clear();

    SetString( "File Cleared", "Nada" );
    WriteToTextFile2();
    }




  private bool ReadFromTextFile()
    {
    CDictionary.Clear();

    if( !File.Exists( FileName ))
      return false;

    try
    {
    string Line;
    using( StreamReader SReader = new
                       StreamReader( FileName  ))
      {
      while( SReader.Peek() >= 0 )
        {
        Line = SReader.ReadLine();
        if( Line == null )
          continue;

        Line = Line.Trim();
        if( Line == "" )
          continue;

        if( !Line.Contains( "\t" ))
          continue;

        string[] SplitString = Line.Split(
                            new Char[] { '\t' } );
        if( SplitString.Length < 2 )
          continue;

        string KeyWord = SplitString[0].Trim();
        string Value = SplitString[1].Trim();
        KeyWord = Utility.getCleanAscii(
                          KeyWord, false, 100 );
        Value = Utility.getCleanAscii(
                        Value, false, MaxLength );

        if( KeyWord == "" )
          continue;

        CDictionary[KeyWord] = Value;
        // try
        // CDictionary.Add( KeyWord, Value );
        }
      }

    return true;

    }
    catch( Exception ) // Except )
      {
      // Could not write the configuration data
      //  to the file.\r\n" + Except.Message );
      return false;
      }
    }





  internal bool WriteToTextFile2()
    {
    try
    {
    using( StreamWriter SWriter = new
                      StreamWriter( FileName  ))
      {
      foreach( KeyValuePair<string,
                     string> Kvp in CDictionary )
        {
        string Line = Kvp.Key + "\t" + Kvp.Value;
        // if( Encrypt != null )
          // Line = Encrypt.EncryptString( Line );

        SWriter.WriteLine( Line );
        }

      SWriter.WriteLine( " " );
      }

    return true;

    }
    catch( Exception ) // Except )
      {
      // Could not write the configuration data
      // to the file.\r\n" + Except.Message );
      return false;
      }
    }





  }
