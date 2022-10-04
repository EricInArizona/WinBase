// Copyright Eric Chauvin 2022.


// This is licensed under the GNU General
// Public License (GPL).  It is the
// same license that Linux has.
// https://www.gnu.org/licenses/gpl-3.0.html


using System;
using System.Drawing;
using System.Text;
// using System.IO;
using System.Windows.Forms;


class TabBox : TabControl
  {
  private MainFormBase MForm;
  private TabPage TabPage2;
  private TabPage TabPage3;
  private Font MainFont;


  private TabBox()
    {
    }


  internal TabBox( MainFormBase UseForm )
    {
    MForm = UseForm;
    Dock = System.Windows.Forms.DockStyle.Fill;

    // Items.
    // Items.Add(
    // SelectedIndex

    TabPage2 = new TabPage( "Tab two" );
    TabPage3 = new TabPage( "Tab Three" );
    MainFont = new Font(
               "Microsoft Sans Serif", 28,
               FontStyle.Regular,
               GraphicsUnit.Pixel );

    InitComponents();
    }


  void InitComponents()
    {
    Font = MainFont;

    TabPage2.Font = MainFont;
    TabPage3.Font = MainFont;


    // TabPages is a TabPageCollection class.

    // TabPages.Count
    // TabPages.Clear();

    TabPages.Add( TabPage2 );
    TabPages.Add( TabPage3 );


    SelectedIndex = 0;

    }


  internal void AddTabPage( TabPage toAdd )
    {
    TabPages.Add( toAdd );
    }



/*
  private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
  {
  if( MainTabControl.SelectedIndex == 7 )
    {
    if( ChemicalElementTextBox.CanFocus )
      ChemicalElementTextBox.Focus();

    }
  }
*/


  }
