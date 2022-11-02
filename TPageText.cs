// Copyright Eric Chauvin 2022.


// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html


using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;



// This is a property of the tab page.
   //  if( SoAndSoTabPage.Visible )


class TPageText : TabPage
  {
  private MainFormBase MForm;
  private TextBox MainTextBox;
  private string fileName = "";
  private string status = "";



  private TPageText()
    {
    }



  internal TPageText( MainFormBase UseForm,
                      string TabTitle ) :
                      base( TabTitle )
    {
    MForm = UseForm;
    Dock = DockStyle.Fill;
    Font = MForm.GetMainFont();

    MainTextBox = new TextBox();
    MainTextBox.Font = MForm.GetMainFont();
    MainTextBox.Text = "";
    MainTextBox.Dock = DockStyle.Fill;
    // MainTextBox.MaxLength = 6000;
    MainTextBox.AllowDrop = false;
    MainTextBox.WordWrap = true;
    MainTextBox.Multiline = true;
    MainTextBox.ReadOnly = true;
    MainTextBox.ScrollBars = System.Windows.
                       Forms.ScrollBars.Vertical;

    MainTextBox.BackColor = System.Drawing.
                             Color.Black;
    MainTextBox.ForeColor = System.Drawing.
                             Color.White;

    // MainTextBox.KeyDown += new System.Windows.
    //                Forms.KeyEventHandler(
    //                MainTextBox_KeyDown );

    // MainTextBox.KeyUp += new System.Windows.
    //                    Forms.KeyEventHandler(
    //                    MainTextBox_KeyUp );

    this.Controls.Add( MainTextBox );

    this.Enter += new System.
        EventHandler( TPageText_Enter );

    }



  internal void setFileName( string setTo )
    {
    fileName = setTo;
    }


  internal void SetReadOnly( bool setTo )
    {
    MainTextBox.ReadOnly = setTo;
    }


  internal void AppendLine( string Line )
    {
    if( MForm.GetShuttingDown() )
      return;

    // if( MainTextBox.Text.Length > (80 * 10000))
      // MainTextBox.Text = "";

    MainTextBox.AppendText( Line + "\r\n" );
    }


/*
  private void MainTextBox_KeyDown(
                   object sender, KeyEventArgs e)
    {
    if( e.KeyCode == Keys.Enter )
      {
      string Line = Utility.GetCleanAsciiString(
                MainTextBox.Text, 6000 );
      MainTextBox.Text = "";
      // See Keyup().
      // MainTextBox.Select( 0, 0 );
             // Set the cursor to the beginning.
      Line = Line.Trim();
      if( Line.Length < 2 )
        return;

      }
    }
*/



/*
  private void MainTextBox_KeyUp(
                 object sender, KeyEventArgs e)
    {
    if( e.KeyCode == Keys.Enter )
      {
      MainTextBox.Text = "";
      MainTextBox.Select( 0, 0 );
      }
    }
*/


/*
  private void StatusForm_Activated(
                     object sender, EventArgs e)
    {
    MainTextBox.Select();
    MainTextBox.SelectionStart = MainTextBox.
                                 Text.Length;
    MainTextBox.ScrollToCaret();
    TypingTextBox.Select();
    }
*/


/*
  private void BottomPanelMouseDown( object sender,
                                   MouseEventArgs e)
    {
    if( e.Button == MouseButtons.Left )
      {
      // These get used for MouseMove too.
      // MouseX = e.X;
      // MouseY = e.Y;

      Close();
      }
    }
*/


  internal void FreeEverything()
    {
    if( MainTextBox != null )
      {
      MainTextBox.Dispose();
      MainTextBox = null;
      }
    }



  private void TPageText_Enter( object sender,
                                EventArgs e )
    {
    MainTextBox.Select();
    }


/*
  private bool ReadFromTextFile( string FileName,
                                bool AsciiOnly )
    {
    try
    {
    if( !File.Exists( FileName ))
      {
      // Might be adding a new file that doesn't
      // exist yet.
      MainTextBox.Text = "";
      return false;
      }

    // This opens the file, reads or writes all the
    // bytes, then closes the file.
    byte[] FileBytes = File.ReadAllBytes( FileName );
    if( FileBytes == null )
      {
      MainTextBox.Text = "FileBytes was null.";
      return false;
      }

    string FileS = UTF8Strings.BytesToString(
                       FileBytes, 1000000000 );

    StringBuilder SBuilder = new StringBuilder();
    StringBuilder FileSBuilder = new StringBuilder();

    int Last = FileS.Length;
    for( int Count = 0; Count < Last; Count++ )
      {
      char OneChar = FileS[Count];
      if( OneChar == '\r' )
        continue; // Ignore it.

      if( OneChar == '\n' )
        {
        string Line = SBuilder.ToString();
        SBuilder.Clear();
        Line = Line.Replace( "\t", "  " );
        Line = StringsEC.GetCleanUnicodeString(
                            Line, 4000, false );
        Line = Line.TrimEnd();

        // if( Line == "" )
          // continue;

        FileSBuilder.Append( Line + "\r\n" );
        continue;
        }

      SBuilder.Append( OneChar );
      }

    MainTextBox.Text = FileSBuilder.ToString().
                                  TrimEnd();
    return true;
    }
    catch( Exception Except )
      {
      MForm.ShowStatus(
               "Could not read the file: \r\n" +
                 FileName );
      MForm.ShowStatus( Except.Message );
      return false;
      }
    }
*/




  internal bool writeToTextFile()
    {
    try
    {
    status += "Saving: " + fileName + "\r\n";

    Encoding Encode = Encoding.ASCII;
                            // Encoding.UTF8;

    using( StreamWriter SWriter = new
            StreamWriter( fileName, false, Encode ))
      {
      string[] Lines = MainTextBox.Lines;

      foreach( string Line in Lines )
        {
        // SWriter.WriteLine( Line.TrimEnd() +
        //                      "\r\n" );
        SWriter.WriteLine( Line.TrimEnd() );
        }

      // SWriter.WriteLine( " " );
      }

    return true;
    }
    catch( Exception Except )
      {
      status += "Could not write to the file:\r\n";
      status += fileName + "\r\n";
      status += Except.Message + "\r\n";
      return false;
      }
    }


  }
