// Copyright Eric Chauvin 2022.



// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html



using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
// using System.Drawing.Drawing2D;
using System.Text;
// using System.Threading;
using System.Windows.Forms;
// using System.IO;
// using System.Resources;
// using System.Globalization;


// Any Windows Forms app has a main form with
// some basic things that any app has.



public class MainFormBase : Form
  {
  protected string VersionDate = "";
  protected int VersionNumber = 10; // 1.0
  protected string MainDataDirectory = "";
  protected string MessageBoxTitle = "";
  internal Commands commands;


  internal string GetMainDataDirectory()
    {
    return MainDataDirectory;
    }

  internal string GetVersionDate()
    {
    return VersionDate;
    }

  internal int GetVersionNumber()
    {
    return VersionNumber;
    }


  internal string GetMessageBoxTitle()
    {
    return MessageBoxTitle;
    }

  }
