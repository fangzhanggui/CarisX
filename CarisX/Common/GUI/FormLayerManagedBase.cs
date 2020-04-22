using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// 表示階層管理フォーム
    /// </summary>
    /// <remarks>
    /// 表示されるフォームの表示階層を、
    /// Owner設定により行います。
    /// </remarks>
    public class FormLayerManagedBase : Form
    {
        #region [定数定義]

        /// <summary>
        /// 表示階層
        /// </summary>
        /// <remarks>
        /// レイヤは上位レベルをOwnedに設定します。
        /// レベル2のフォームはレベル1のフォーム全てOwner設定されます。
        /// </remarks>
        public enum Layer
        {
            /// <summary>
            /// 管理対象外
            /// </summary>
            None,
            /// <summary>
            /// レイヤ1
            /// </summary>
            Layer1,
            /// <summary>
            /// レイヤ2
            /// </summary>
            Layer2,
            /// <summary>
            /// レイヤ3
            /// </summary>
            Layer3
        }

        #endregion

        #region [クラス変数定義]

        /// <summary>
        /// 階層辞書
        /// </summary>
        static private Dictionary<Layer, List<Form>> layerFormDic = null;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 自身のレイヤ
        /// </summary>
        private Layer thisLayer = Layer.None;

        //Boolean reActivate = false;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormLayerManagedBase()
        {
            //InitializeComponent();
            if ( layerFormDic == null )
            {
                // 初回インスタンス生成時、辞書内容生成
                layerFormDic = new Dictionary<Layer, List<Form>>();
                layerFormDic.Add( Layer.Layer1, new List<Form>() );
                layerFormDic.Add( Layer.Layer2, new List<Form>() );
                layerFormDic.Add( Layer.Layer3, new List<Form>() );
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// レイヤ設定
        /// </summary>
        /// <remarks>
        /// 自身の表示階層を設定する。
        /// </remarks>
        /// <param name="layer">レイヤ</param>
        public void SetLayer( Layer layer )
        {
            // 階層辞書から自身を除外
            this.excludeThisFromLayerDic();

            // 自身を追加し、保持状態を更新する。
            layerFormDic[layer].Add( this );
            this.thisLayer = layer;

            refleshLayer();
        }
        
        //public new void Activate()
        //{
        //    base.Activate();
        //}

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// 階層更新
        /// </summary>
        /// <remarks>
        /// 階層の更新を行います。
        /// </remarks>
        static protected void refleshLayer()
        {
            // 無効になっている要素があれば削除
            foreach ( var list in layerFormDic )
            {
                list.Value.RemoveAll( (Predicate<Form>)( ( Form f ) => f.IsDisposed ) );
            }
                        
            // 上位レベルのフォームに対して、下位レベルのフォームをオーナー設定する。
            // レベル設定の際に自身を再登録するので、同一フォームが複数レベルと絡む事は無い。
            List<Form> formList = null;
            
            var enumerator = layerFormDic.GetEnumerator();
            while ( enumerator.MoveNext() )
            {
                formList = enumerator.Current.Value;
                var enCopy = enumerator;
                while ( enCopy.MoveNext() )
                {
                    foreach ( Form ownerForm in formList )
                    {
                        foreach ( Form ownedForm in enCopy.Current.Value )
                        {
                            ownerForm.AddOwnedForm( ownedForm );
                        }
                    }
                    break; // 循環参照エラー抑制 
                }
            }
            
            //foreach ( var list in layerFormDic )
            //{
            //    if ( formList != null )
            //    {
            //    }
                
            //}

            //foreach ( var list in layerFormDic )
            //{
            //    if ( formList != null )
            //    {
            //        foreach ( var ownedForm in list.Value )
            //        {
            //            foreach ( var ownerForm in formList )
            //            {
            //                ownerForm.AddOwnedForm( ownedForm );
            //            }
            //        }
            //    }
            //    formList = list.Value;
            //}
        }
       
        /// <summary>
        /// 階層辞書から自身を除外
        /// </summary>
        /// <remarks>
        /// 保持している階層辞書から、自身を除外する。
        /// </remarks>
        protected void excludeThisFromLayerDic()
        {
            // 自身がリストにあれば削除
            foreach ( var list in layerFormDic )
            {
                list.Value.RemoveAll( (Predicate<Form>)( ( Form f ) => f == this ) );
            }
        }



        //protected override void WndProc( ref Message m )
        //{
        //    const Int32 WM_ACTIVATE = 0x0006;
        //    const Int32 WA_INACTIVE = 0;
        //    switch (m.Msg )
        //    {
        //    case WM_ACTIVATE:
        //        Int32 activatStatus = ( (Int32)m.WParam & 0x0000FFFF );
        //        if ( activatStatus != WA_INACTIVE )
        //        {
        //            System.Diagnostics.Debug.WriteLine( "Active" );

        //            if ( this.thisLayer != Layer.None )
        //            {
        //                // 自身を管理対象から削除
        //                this.excludeThisFromLayerDic();

        //                layerFormDic[this.thisLayer].Add( this );

        //                refleshLayer();

        //            }

        //        }
        //        break;
        //    }
        //    base.WndProc( ref m );
        //}

        /// <summary>
        /// Active時処理
        /// </summary>
        /// <remarks>
        /// 画面がアクティブになった時の処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnActivated( EventArgs e )
        {

            if ( ( this.thisLayer != Layer.None )
                && ( Form.ActiveForm == this ) )
            {
                // 自身を管理対象から削除
                this.excludeThisFromLayerDic();

                layerFormDic[this.thisLayer].Add( this );

                refleshLayer();

                // 同じレイヤ同士の付け替えが、一度の表示に反映されない。
                // タイトルバークリックでチラつきあるが反映される為、
                // メッセージの流れを単純に模倣したがうまくいかない。
                // 実運用では起こるケースは無いはずなので、現在保留とする。

                //APIHelp.RECT rec =  new APIHelp.RECT(10,10,100,100);
                //APIHelp.SendMessage( this.Handle, 0x0216, (IntPtr)0x9, ref rec );
                //APIHelp.SendMessage( this.Handle, 0x0046, (IntPtr)0, (IntPtr)0 );
                //APIHelp.SendMessage( this.Handle, 0x0232, (IntPtr)0, (IntPtr)0 );
            }
            //var x = this.Location.X;
            //var y = this.Location.Y;
            //var xy =
            base.OnActivated( e );
        }

        /// <summary>
        /// VisibleChanged時処理
        /// </summary>
        /// <remarks>
        /// VisibleChangedイベント発生時の処理を行います。
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnVisibleChanged( EventArgs e )
        {
            base.OnVisibleChanged( e );


            if ( this.thisLayer != Layer.None )
            {
                // 自身を管理対象から削除
                this.excludeThisFromLayerDic();

                // 表示の場合、再登録とする
                if ( this.Visible )
                {
                    layerFormDic[this.thisLayer].Add( this );
                }

                refleshLayer();
            }
        }

        #endregion

    }
    
    //public partial class FormLayer : Form
    //{
    //    public FormLayer()
    //    {
    //        InitializeComponent();
    //    }
    //}
}
