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
  // private MainForm MForm;
  private TabPage TabPage1;
  private TabPage TabPage2;
  private TabPage TabPage3;
  private TextBox StatTextBox;
  private Font MainFont;


  internal TabBox()
    {
    Dock = System.Windows.Forms.DockStyle.Fill;

    // Items.
    // Items.Add( 
    // SelectedIndex

    TabPage1 = new TabPage( "Tab 1");
    TabPage2 = new TabPage( "Tab two" );
    TabPage3 = new TabPage( "Tab Three" );
    StatTextBox = new TextBox();
    MainFont = new Font(
               "Microsoft Sans Serif", 28,
               FontStyle.Regular,
               GraphicsUnit.Pixel );

    InitComponents();
    }


  void InitComponents()
    {
    Font = MainFont;
    StatTextBox.Font = MainFont;

    TabPage1.Font = MainFont;
    TabPage2.Font = MainFont;
    TabPage3.Font = MainFont;
    StatTextBox.Font = MainFont;
    StatTextBox.Text = "Testing...";
    StatTextBox.Dock = DockStyle.Fill;
    // StatTextBox.MaxLength = 6000;
    StatTextBox.Multiline = true;

    TabPage1.Controls.Add( StatTextBox );

    TabPage1.Dock = DockStyle.Fill;

    // TabPages is a TabPageCollection class.

    // TabPages.Count
    // TabPages.Clear();

    TabPages.Add( TabPage1 );
    TabPages.Add( TabPage2 );
    TabPages.Add( TabPage3 );
 

    SelectedIndex = 0;

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
