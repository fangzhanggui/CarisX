using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using Infragistics.Win.UltraWinEditors;
using Oelco.Common.GUI;

namespace Prototype
{

    // ソフトウェアキーボード用テスト画面。
    // ※結局この方法ではやってない。
    public partial class FormUTBase : Form
    {

        TextInputPanelControl tip;
        public static Boolean TipVisivle
        {
            get;
            set;
        }

        public FormUTBase()
        {
            InitializeComponent();
        }

        private void ChildControls_Enter( Object sender, System.EventArgs e )
        {
            
            if ( sender == null )
            {
                return;
            }

            if ( tip == null )
            {
                try
                {
                    //TipVisivle = false;
                    tip = new TextInputPanelControl();
                    tip.AttachControl( (Control)sender, TipVisivle );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine( ex.Message + " " + ex.StackTrace );
                    Singleton<LogManager>.Instance.WriteCommonLog( LogKind.DebugLog, ex.Message + " " + ex.StackTrace );
                }
            }
                        
        }

        private void ChildControls_Leave( Object sender, System.EventArgs e )
        {
            tip = null;
        }

        private void ChildControls_ControlAdded( Object sender, System.Windows.Forms.ControlEventArgs e )
        {
            if ( e == null || e.Control == null )
            {
                return;
            }

            e.Control.Enter += new EventHandler( ChildControls_Enter );
            e.Control.Leave += new EventHandler( ChildControls_Leave );
        }

        private void ChildControls_ControlRemoved( Object sender, System.Windows.Forms.ControlEventArgs e )
        {
            if ( e == null || e.Control == null )
            {
                return;
            }

            e.Control.Enter -= new EventHandler( ChildControls_Enter );
            e.Control.Leave -= new EventHandler( ChildControls_Leave );
        }
    }
}

    


