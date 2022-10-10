// Copyright Eric Chauvin 2022.


// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html


using System;
using System.IO;
using System.Windows.Forms;


class Commands
  {

  internal virtual void DoCommand(
                     string CommandName,
                     int X,
                     int Y )
    {

    }


  internal virtual void FreeEverything()
    {

    }

  }
