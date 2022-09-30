// Copyright Eric Chauvin 2022.


// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;



class TouchRect : PicBoxDraw
  {
  private int LeftX = 100;
  private int TopY = 100;
  private int Width = 500;
  private int Height = 100;


  private TouchRect()
    {
    }


  internal TouchRect( int X, int Y,
                      int W, int H,
                      string UseCommand,
                      string UseLabel )
    {
    LeftX = X;
    TopY = Y;
    Width = W;
    Height = H;
    CommandName = UseCommand;
    DrawLabel = UseLabel;
    }



  internal override bool IsInside( int X, int Y )
    {
    // Being exactly on the line doesn't count
    // as being inside.

    if( X <= LeftX )
      return false;

    if( X >= (LeftX + Width) )
      return false;

    if( Y >= (TopY + Height) )
      return false;

    return true;
    }



  internal override void Draw(
                          Graphics DrawGraphics )
    {
    Font MainFont = new Font( 
                    FontFamily.GenericSansSerif,
                    28.0F,
                    FontStyle.Regular,
                    GraphicsUnit.Pixel );

    SolidBrush FontBrush = new SolidBrush(
                                   Color.White );

    Pen MainPen = new Pen( Brushes.White );
    MainPen.Width = 1.0F;
    MainPen.LineJoin = System.Drawing.Drawing2D.
                                  LineJoin.Bevel;
    MainPen.DashStyle = DashStyle.Solid;
                 // DashDot, DashDotDot, Custom

    DrawGraphics.DrawRectangle( MainPen, LeftX,
                                TopY, Width,
                                Height );

    DrawGraphics.DrawString( DrawLabel,
               MainFont, 
               FontBrush, LeftX + 3, TopY + 5 );

    }


  internal override void FreeEverything()
    {

    }


  }
