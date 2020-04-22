using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Microsoft.Ink.TextInput;

namespace Prototype
{
    public partial class FormSoftKeyBord : Form
    {
        //List<TextInputPanel> tips = new List<TextInputPanel>();
        //TextInputPanel tipTest = new TextInputPanel();

        public Boolean flg = false;
        
        public FormSoftKeyBord()
        {
            InitializeComponent();            
        }
        private void FormSoftKeyBord_Load( object sender, EventArgs e )
        {
            //if ( flg )
            //{
            //    foreach ( var ctrl in this.Controls )
            //    {
            //        if ( ctrl.GetType().Equals( typeof( TextBox ) ) )
            //        {
            //            TextInputPanel tip = new TextInputPanel();
            //            tip.AttachedEditWindow = ( (TextBox)ctrl ).Handle;
            //            tip.InPlaceVisibleOnFocus = false;
            //            tips.Add( tip );
            //        }
            //    }
            //}
        }

        private void button1_Click( object sender, EventArgs e )
        {
            //foreach ( var tip in tips )
            //{
            //    tip.InPlaceVisibleOnFocus = true;
            //}
            FormUTBase.TipVisivle = true;

            // 操作するレジストリ・キーの名前
            String rKeyName = @"Software\Microsoft\TabletTip\1.7";

            // 設定処理を行う対象となるレジストリの値の名前
            String rSetValueName = "ShowIPTipTouchTarget";

            // 設定する値のデータ
            Int32 location = 1;  // REG_DWORD型

            // レジストリの設定と削除
            try
            {
                // レジストリ・キーを新規作成して開く
                Microsoft.Win32.RegistryKey rKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( rKeyName );

                // レジストリの値を設定
                rKey.SetValue( rSetValueName, location );

                // レジストリの値を取得
                var aaa = (Int32)rKey.GetValue( rSetValueName );

                // 開いたレジストリを閉じる
                rKey.Close();

                // サービス再起動
                this.RebootTabletInputService();
            }
            catch ( Exception ex )
            {
                // レジストリ・キーが存在しない
                Console.WriteLine( ex.Message );
            }
 
        }

        private void button2_Click( object sender, EventArgs e )
        {
            //foreach ( var tip in tips )
            //{
            //    tip.InPlaceVisibleOnFocus = false;
            //}
            FormUTBase.TipVisivle = false;

            // 操作するレジストリ・キーの名前
            String rKeyName = @"Software\Microsoft\TabletTip\1.7";

            // 設定処理を行う対象となるレジストリの値の名前
            String rSetValueName = "ShowIPTipTouchTarget";

            // 設定する値のデータ
            Int32 location = 0;  // REG_DWORD型

            // レジストリの設定と削除
            try
            {
                // レジストリ・キーを新規作成して開く
                Microsoft.Win32.RegistryKey rKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey( rKeyName );

                // レジストリの値を設定
                rKey.SetValue( rSetValueName, location );

                // レジストリの値を取得
                var aaa = (Int32)rKey.GetValue( rSetValueName );

                // 開いたレジストリを閉じる
                rKey.Close();

                // サービス再起動
                this.RebootTabletInputService();
                
            }
            catch ( Exception ex )
            {
                // TODO:
                // レジストリ・キーが存在しない
                Console.WriteLine( ex.Message );
            }
        }

        /// <summary>
        /// TabletInputService再起動
        /// </summary>
        private void RebootTabletInputService()
        {
            try
            {
                System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController( "TabletInputService" );
                
                // 停止できるか調べる
                if ( sc.CanStop )
                {
                    //サービスを停止する
                    sc.Stop();
                    //サービスが停止するまで待機する
                    sc.WaitForStatus( System.ServiceProcess.ServiceControllerStatus.Stopped );
                }
                if ( sc.Status == System.ServiceProcess.ServiceControllerStatus.Stopped )
                {
                    //サービスが停止していれば、開始する
                    sc.Start();
                    //サービスが開始されるまで待機する
                    sc.WaitForStatus( System.ServiceProcess.ServiceControllerStatus.Running );
                }
                MessageBox.Show( "OK!!" );

            }
            catch(Exception ex)
            {
                Console.WriteLine( ex.Message );
            }
        }
         

        private void FormSoftKeyBord_FormClosed( object sender, FormClosedEventArgs e )
        {
            //foreach ( var tip in tips )
            //{
            //    tip.Dispose();
            //}
            //tips.Clear();
            //tips = null;
        } 
    }
}
