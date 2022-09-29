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



// namespace EarthScience
// {

class PicBox : PictureBox
  {
  private Bitmap GraphBitmap = null;


  internal PicBox()
    {
    // This has a Width and Height that is set
    // By Dock Fill in the panel.

    // Give it an initial bit map.
    GraphBitmap = new Bitmap( 1024,
                              768 );
                    // PixelFormat.Canonical );
                    // Default 24 bit color.

    Image = GraphBitmap;
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

=== Do I put this TouchRect in here?

/*
      TouchRect tRect = new TouchRect();
      tRect.Draw( BitGraphic );


      WMap.Draw( Sz.Width,
                 Sz.Height,
                 BitGraph,
                 ShowCrossHairs,
                 UseEqualization,
                 true, // ShowBottomLegend
                 ShowCityNames,
                 ShowNuclear,
                 ShowNuclearNames,
                 ShowAirports,
                 ShowLakes,
                 //  ShowInterstate,
                 ShowRoads,
                 ShowCounties,
                 ShowRailRoads,
                 ShowRivers,
                 ShowRivers2,
                 ShowStreets,
                 UseGPSReaderForm,
                 true );
*/

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


/*
======
      this.PIP1PictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.PIP1DrawPanel_MouseDoubleClick);
      this.PIP1PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PIP1DrawPanel_MouseDown);
      this.PIP1PictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PIP1DrawPanel_MouseMove);

  private void PicBoxMouseDown( object sender,
                               MouseEventArgs e)
    {

    MouseDownWasOnPIP = true;

    if( e.Button == MouseButtons.Left )
      {
      // These get used for MouseMove too.
      MouseX = e.X;
      MouseY = e.Y;

    }


*/


  }
