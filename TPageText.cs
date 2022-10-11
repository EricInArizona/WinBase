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


  internal void SetReadOnly( bool SetTo )
    {
    MainTextBox.ReadOnly = SetTo;
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



  internal bool WriteToTextFile( string FileName )
    {
/*
    try
    {
    using( StreamWriter SWriter = new
                     StreamWriter( FileName  ))
      {
      int Last = ;
      for( int Count = 0; Count < Last; Count++ )
        {
        ECTime ShowTime = new ECTime(
                              TimeSequence >> 8 );

        string Line = ShowTime.
                ToLocalTimeString() + " on " +
                ShowTime.ToLocalDateString();

        SWriter.WriteLine( Line );
        SWriter.WriteLine( " " );
        }
      }

    }
    catch( Exception ) // Except )
      {
      return false;
      }
*/
    return true;
    }


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


  }
