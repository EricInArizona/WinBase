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



class PicBoxDraw
  {
  protected string CommandName = "";
  protected string DrawLabel = "";


  internal string GetCommandName()
    {
    return CommandName;
    }


  internal virtual bool IsInside( int X, int Y )
    {
    return false;
    }


  internal virtual void Draw(
                          Graphics DrawGraphics )
    {
    Pen MainPen = new Pen( Brushes.White );
    MainPen.Width = 1.0F;
    MainPen.LineJoin = System.Drawing.Drawing2D.
                                  LineJoin.Bevel;
    MainPen.DashStyle = DashStyle.Solid;
                 // DashDot, DashDotDot, Custom

    DrawGraphics.DrawRectangle( MainPen, 100,
                                100, 300, 200 );
    }



  internal virtual void FreeEverything()
    {

    }



  }
