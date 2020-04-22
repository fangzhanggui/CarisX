using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oelco.Common.Parameter;

// TODO:コメント不十分

namespace Oelco.Common.GUI
{
    /// <summary>
    /// 背景暗転メッセージボックス
    /// </summary>
    public partial class FormBlackOut : FormTransitionBase
    {
        #region [クラス変数定義]

        /// <summary>
        /// ダミー画面
        /// </summary>
        private FormBackScreen formBackScreen = null;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormBlackOut()
        {
            InitializeComponent();
        }

        #endregion

        #region [publicメソッド]

        ///// <summary>
        ///// メッセージボックス表示
        ///// </summary>
        //public override void Show()
        //{
        //    // 背景を表示する

        //    // ダミー画面表示
        //    // ダミーをZオーダー2位,自身を1位にする。
        //    dummy.BringToFront();
        //    dummy.Opacity = 0.65d;
        //    dummy.Visible = true;
        //    this.BringToFront();
        //    this.TopMost = true;

        //    base.Show();
        //}

        /// <summary>
        /// メッセージボックスダイアログ表示
        /// </summary>
        /// <remarks>
        /// フェード付メッセージボックスダイアログの表示</br>
        /// 本ダイアログより背面のウィンドウは黒透過背面となり、操作不能とします。
        /// </remarks>
        /// <returns>押下ボタン</returns>
        public override DialogResult ShowDialog()
        {
            // 背景を表示する

            formBackScreen = new FormBackScreen();
            formBackScreen.Size = Screen.PrimaryScreen.Bounds.Size;
            formBackScreen.Location = new Point( 0, 0 );
            formBackScreen.StartPosition = FormStartPosition.Manual;
            formBackScreen.Opacity = 0.0d;
            formBackScreen.Visible = false;
            formBackScreen.ShowInTaskbar = false;
            formBackScreen.Show();

            // ダミー画面表示
            // ダミーをZオーダー2位,自身を1位にする。
            formBackScreen.BringToFront();
            // NS-Primeの場合、ブラックアウト画面が重なっても極端に濃くならないようにする
            if ( GlobalParameter.myApplicationKind == Oelco.Common.Parameter.GlobalParameter.ApplicationKind.NS_Prime )
            {
                // AFT
                Int32 openBlackoutCount = 0;
                foreach ( Form frm in Application.OpenForms )
                {
                    if ( frm.Name.ToUpper().Equals( "Dummy".ToUpper() ) )
                    {
                        openBlackoutCount++;
                    }
                }
                if ( openBlackoutCount > 1 )
                {
                    // すでに１画面以上ブラックアウト画面表示済み
                    formBackScreen.Opacity = 0.01d;
                }
                else
                {
                    // まだブラックアウト画面表示していない
                    formBackScreen.Opacity = 0.65d;
                }
            }
            else
            {
                // CarisX
                formBackScreen.Opacity = 0.65d;
            }

            formBackScreen.Visible = true;

#if !DEBUG
            // FIXED:不適合No.143対応(AFT)
            // 一旦フェード画面を最前面にして、フェード画面を表示するダイアログのオーナーにしてから表示する
            formBackScreen.TopMost = true;
            return base.ShowDialog( formBackScreen );
#else
            return base.ShowDialog();
#endif
        }

        /// <summary>
        /// 画面クローズ
        /// </summary>
        /// <remarks>
        /// 本画面をクローズします。
        /// </remarks>
        public override void Close()
        {
            base.Close();
        }

        /// <summary>
        /// フォームクローズ前イベントハンドラ
        /// </summary>
        /// <remarks>
        /// フォームクローズ前イベント処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnClosing( CancelEventArgs e )
        {
            this.Opacity = 0; //@@@ 仮対応

            //dummy.Dispose();
            //dummy = null;

            base.OnClosing( e );
        }

        /// <summary>
        /// フォームクローズ後イベントハンドラ
        /// </summary>
        /// <remarks>
        /// フォームクローズ後イベント処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnClosed( EventArgs e )
        {
            base.OnClosed( e );

            this.Owner = null;
            // ダミー画面も閉じる
            formBackScreen.Close();
        }

        #endregion

        #region [privateメソッド]

        #endregion

        #region [内部クラス]

        /// <summary>
        /// 背景黒画面
        /// </summary>
        protected class FormBackScreen : Form
        {
            #region [インスタンス変数定義]

            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            #endregion

            #region [コンストラクタ/デストラクタ]

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public FormBackScreen()
            {
                InitializeComponent();
            }
            
            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <remarks>
            /// Dispose処理を行います。
            /// </remarks>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose( Boolean disposing )
            {
                if ( disposing && ( components != null ) )
                {
                    components.Dispose();
                }
                base.Dispose( disposing );
            }

            #endregion

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.SuspendLayout();
                // 
                // Dummy
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 12F );
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.BackColor = System.Drawing.Color.Black;
                this.ClientSize = new System.Drawing.Size( 650, 482 );
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Name = "Dummy";
                this.Opacity = 0.65D;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Text = "Dummy";
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.ResumeLayout( false );

            }

            #endregion

        }
        
        #endregion
    }
}
