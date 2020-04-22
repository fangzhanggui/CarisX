using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
//using Microsoft.Ink.TextInput;

namespace Prototype
{
    class TextInputPanelControl
    {
        private Boolean enable = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextInputPanelControl()
        {
            //プラットフォームの取得
            System.OperatingSystem os = System.Environment.OSVersion;
            if ( os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 1 )
            {
                enable = true;
            }
        }

        public void AttachControl(Control ctrl , Boolean visible)
        {   
            if ( enable )
            {
                // Windows 7
                //var domain = AppDomain.CreateDomain("名前（任意）");
                //object instance = domain.CreateInstance( "クラス名", "クラス名（ネームスペース含むフルネーム）" );
                //var domain = AppDomain.CreateDomain( "MicrosoftInk" );
                //object instance = domain.CreateInstance( "TextInputPanel", "Microsoft.Ink.TextInput.TextInputPanel" );

                ////Assembly asm = Assembly.LoadFrom( @"C:\Program Files\Reference Assemblies\Microsoft\Tablet PC\v6.0\Microsoft.Ink.dll" );
                ////Type tipType = asm.GetType( "Microsoft.Ink.TextInput.TextInputPanel" );              
                ////Object tip = Activator.CreateInstance( tipType );
                ////Type type = tip.GetType();
                ////// TextInputPanelが結合されるウィンドウハンドルを設定
                ////type.InvokeMember( "AttachedEditWindow", System.Reflection.BindingFlags.SetProperty, null, tip, new Object[] { ctrl.Handle } );
                ////// Visibleの設定
                ////type.InvokeMember( "InPlaceVisibleOnFocus", System.Reflection.BindingFlags.SetProperty, null, tip, new Object[] { visible } );
                ////if ( visible )
                ////{
                ////    type.InvokeMember( "SetInPlacePosition", System.Reflection.BindingFlags.InvokeMethod, null , tip, new Object[] { 10, 10, 1 } );
                ////}

                //TextInputPanel tip = new TextInputPanel();
                
                ////tip.AttachedEditControl = ctrl;
                //tip.AttachedEditWindow = ctrl.Handle;
                //tip.InPlaceVisibleOnFocus = visible;
                //tip.SetInPlaceVisibility( visible );
                //if ( visible )
                //{
                //    tip.SetInPlacePosition( ctrl.Location.X, ctrl.Location.Y, CorrectionPosition.Top );
                //}

            }
        }
    }
}
