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




class PicBoxDrawAr
  {
  private PicBoxDraw[] PicBoxDrawArray;
  private int PicBoxLast = 0;
  // private int[] SortIndexArray;


  internal PicBoxDrawAr()
    {
    }



  internal void FreeEverything()
    {
    if( PicBoxDrawArray != null )
      {
      for( int Count = 0; Count < PicBoxLast;
                                         Count++ )
        {
        PicBoxDrawArray[Count].FreeEverything();
        PicBoxDrawArray[Count] = null;
        }
      }

    PicBoxLast = 0;
    }




  internal bool AddPicBoxDraw( PicBoxDraw toAdd )
    {
    if( PicBoxDrawArray == null )
      {
      PicBoxLast = 0;

      try
      {
      PicBoxDrawArray = new PicBoxDraw[256];
      // SortIndexArray = new
      //            int[PicBoxDrawArray.Length];
      }
      catch( Exception ) // Except )
        {
        // MessageBox.Show(

        return false;
        }
      }

    PicBoxDrawArray[PicBoxLast] = toAdd;
    // SortIndexArray[PicBoxLast] =
    //                        PicBoxLast;
    PicBoxLast++;
    if( (PicBoxLast + 1) >= PicBoxDrawArray.Length )
      {
      try
      {
      Array.Resize( ref PicBoxDrawArray,
                 PicBoxDrawArray.Length + 256 );
      // Array.Resize( ref SortIndexArray,  );
      }
      catch( Exception ) // Except )
        {
        // MessageBox.Show( "Not enough memory
        return false;
        }
      }

    return true;
    }



  internal void Draw( Graphics DrawGraphics )
    {
    if( PicBoxDrawArray == null )
      return;

    for( int Count = 0; Count < PicBoxLast;
                                         Count++ )
      {
      PicBoxDrawArray[Count].Draw( DrawGraphics );
      }
    }


  }
