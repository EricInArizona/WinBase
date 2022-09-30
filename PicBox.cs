// Copyright Eric Chauvin 2022.


// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html


using System;
// using System.IO;
using System.Windows.Forms;
// using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;



class PicBox : PictureBox
  {
  private MainForm MForm;
  private Bitmap GraphBitmap = null;
  private int MouseX = 0;
  private int MouseY = 0;
  private PicBoxDrawAr picBoxDrawAr;

  private PicBox()
    {
    }


  internal PicBox( MainForm UseForm )
    {
    MForm = UseForm;
    // This has a Width and Height that is set
    // By Dock Fill in the panel.

    // Give it an initial bit map.
    GraphBitmap = new Bitmap( 1024,
                              768 );
                    // PixelFormat.Canonical );
                    // Default 24 bit color.

    Image = GraphBitmap;

    picBoxDrawAr = new PicBoxDrawAr();

    MouseDown += new System.Windows.Forms.
              MouseEventHandler( PicBoxMouseDown );

    // MouseDoubleClick += new System.Windows.
    //   Forms.MouseEventHandler(
    //      PicBoxMouseDoubleClick );
    // MouseMove += new System.Windows.Forms.
    //    MouseEventHandler( PicBoxMouseMove );

    }


  internal bool AddPicBoxDraw( PicBoxDraw toAdd )
    {
    return picBoxDrawAr.AddPicBoxDraw( toAdd );
    }



  internal void ResetSize( int SetWidth,
                           int SetHeight )
    {
    if( GraphBitmap != null )
      {
      GraphBitmap.Dispose();
      GraphBitmap = null;
      }

    GraphBitmap = new Bitmap( SetWidth,
                              SetHeight );
                    // PixelFormat.Canonical );
                    // Default 24 bit color.

    Image = GraphBitmap;
    }



  internal void FreeEverything()
    {
    if( GraphBitmap != null )
      {
      GraphBitmap.Dispose();
      GraphBitmap = null;
      }
    }


  internal void DrawAll()
    {
    if( GraphBitmap == null )
      return;

    try
    {
    using( Graphics BitGraphic = Graphics.
                         FromImage( GraphBitmap ))
      {
      if( BitGraphic == null )
        return;

      BitGraphic.Clear( Color.DarkBlue ); // DarkBlue );

      picBoxDrawAr.Draw( BitGraphic );
      }

    Image = GraphBitmap;

    //    Update();

    }
    catch( Exception ) // Except )
      {
      // MessageBox.Show( "Error: Can't draw to the map bitmap.\r\n" + Except.Message, MainForm.MessageBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop );
      return;
      }
    }



  private void PicBoxMouseDown( object sender,
                                MouseEventArgs e)
    {
    if( e.Button == MouseButtons.Left )
      {
      // These get used for MouseMove too.
      MouseX = e.X;
      MouseY = e.Y;

      MForm.ShowStatusForm();

      // string CommandName = picBoxDrawAr.Get 
      // if( CommandName == "" )
      // mainCommands.DoCommand( 
      //               string CommandName,
      //                int X,
       //              int Y )

      // picBoxDrawAr.
      }
    }



  }
