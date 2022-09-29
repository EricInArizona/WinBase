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


// namespace Something

class TouchRect
  {
  private int LeftX = 100;
  private int TopY = 100;
  private int Width = 300;
  private int Height = 200;



  internal void Draw( Graphics DrawGraphics )
    {
    Pen MainPen = new Pen( Brushes.White );
    MainPen.Width = 1.0F;
    MainPen.LineJoin = System.Drawing.Drawing2D.
                                  LineJoin.Bevel;
    MainPen.DashStyle = DashStyle.Solid;
                 // DashDot, DashDotDot, Custom

    DrawGraphics.DrawRectangle( MainPen, LeftX,
                                TopY, Width,
                                Height );

    }

  }
