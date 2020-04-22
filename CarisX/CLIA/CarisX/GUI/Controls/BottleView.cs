using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
using Oelco.CarisX.Const;
using Oelco.CarisX.DB;
using Oelco.CarisX.Parameter;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Common;

namespace Oelco.CarisX.GUI.Controls
{
    /// <summary>
    /// ボトル表示コントロール
    /// </summary>
    public partial class BottleView : UserControl
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// 現在のステータス
        /// </summary>
        private RemainStatus status;

        /// <summary>
        /// 状態変化イベント
        /// </summary>
        public event EventHandler<ChangeStatusEventArgs> StatusChanged;
        #endregion

        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BottleView()
        {
            InitializeComponent();

            // ボトル状態表示アイコン設定
            List<Image> images = new List<Image>();
            var enumGetValues = Enum.GetValues( this.status.GetType() ).OfType<RemainStatus>().OrderBy( ( status ) => (Int32)status );
            foreach ( RemainStatus status in enumGetValues )
            {
                switch (status)
                {
                    case RemainStatus.Full:
                        images.Add( Oelco.CarisX.Properties.Resources.Image_BottleGreenLarge );
                        break;
                    case RemainStatus.Low:
                        images.Add( Oelco.CarisX.Properties.Resources.Image_BottleYellowLarge );
                        break;
                    case RemainStatus.Empty:
                        images.Add(  Oelco.CarisX.Properties.Resources.Image_BottleWhiteLarge );
                        break;
                }
            }
            this.spbReagentKind.ImageList = images;

        }

        #endregion

        #region [プロパティ]
        /// <summary>
        /// 現在のステータスの取得、設定
        /// </summary>
        public RemainStatus Status
        {
            get
            {
                return this.status;
            }
            set
            {
                RemainStatus before = this.status;
                this.status = value;

                // 状態変化イベント発生
                if ( before != this.status )
                {
                    ChangeStatusEventArgs args = new ChangeStatusEventArgs( before, this.status );
                    this.OnStatusChanged( args );
                    this.status = args.AfterStatus;
                }
                this.spbReagentKind.ViewIndex = (Int32)this.status;
            }
        }

        /// <summary>
        /// 現在、選択されているかどうかを取得、設定
        /// </summary>
        public Boolean IsSelected
        {
            get
            {
                return this.pbxUseReagentKind.Visible;
            }
            set
            {
                this.pbxUseReagentKind.Visible = value;
            }
        }

        /// <summary>
        /// 試薬種別
        /// </summary>
        public ReagentKind ReagentKind{get;set;}

        /// <summary>
        /// ポート番号
        /// </summary>
        public Int32 PortNo { get; set; }

        /// <summary>
        /// 試薬種別名
        /// </summary>
        public String ReagentName
        {
            get
            {
                return this.lblReagentName.Text;
            }
            set
            {
                this.lblReagentName.Text = value;
            }
        }

        /// <summary>
        /// 残数単位
        /// </summary>
        public String RemainUnit
        {
            get
            {
                return this.lblRemainUnit.Text;
            }
            set
            {
                this.lblRemainUnit.Text = value;
            }
        }

        #endregion

        #region [protectedメソッド]
        /// <summary>
        /// 状態変化イベント
        /// </summary>
        /// <remarks>
        /// このイベントはボトル表示の状態が変化した後に発生します。
        /// </remarks>
        /// <param name="before">変化前状態</param>
        /// <param name="after">変化後状態</param>
        protected virtual void OnStatusChanged( ChangeStatusEventArgs args )
        {
            if ( this.StatusChanged != null )
            {
                this.StatusChanged( this, args );
            }
        }
        #endregion

        #region [内部クラス]

        /// <summary>
        /// 状態変化イベントデータ
        /// </summary>
        public class ChangeStatusEventArgs : System.EventArgs
        {
            #region [コンストラクタ/デストラクタ]

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="beforeStatus">変化前ステータス</param>
            /// <param name="afterStatus">変化後ステータス</param>
            public ChangeStatusEventArgs( RemainStatus beforeStatus, RemainStatus afterStatus )
            {
                this.BeforeStatus = beforeStatus;
                this.AfterStatus = afterStatus;
            }

            #endregion
            
            #region [プロパティ]

            /// <summary>
            /// 変化前のステータスの取得、設定
            /// </summary>
            public RemainStatus BeforeStatus
            {
                get;
                protected set;
            }

            /// <summary>
            /// 変化後のステータスの取得、設定
            /// </summary>
            public RemainStatus AfterStatus
            {
                get;
                set;
            }

            #endregion

        }

        #endregion

        #region [publicメソッド]
        /// <summary>
        /// 残量の取得・設定
        /// </summary>
        public void setBottleRemain(Int32 moduleId)
        {
            //コントロールに設定されている試薬種別が対象外の場合は処理をしない
            switch (ReagentKind)
            {
                case ReagentKind.Pretrigger:
                case ReagentKind.Trigger:
                case ReagentKind.Diluent:
                    break;
                default:
                    return;
            }

            //残量の取得・設定
            ReagentData data = Singleton<ReagentDB>.Instance.GetData(moduleId: moduleId)
                .FirstOrDefault((reagentDataItem) => reagentDataItem.ReagentKind == (Int32)ReagentKind && (reagentDataItem.PortNo == PortNo || PortNo == 0));
            if (data != null && !String.IsNullOrEmpty(data.LotNo))
            {
                Int32 remain = CarisXSubFunction.GetDispRemainCount(ReagentKind, data.Remain);
                lblRemain.Text = remain.ToString();
                this.Status = Singleton<ReagentRemainStatusInfo>.Instance.GetRemainStatus(ReagentKind, remain);
                this.IsSelected = data.IsUse ?? false;

                data.ExpirationDate = data.ExpirationDate ?? DateTime.MinValue;
                if (DateTime.Today >= data.ExpirationDate.Value.AddDays(1))
                {
                    lblReagentName.Appearance.ForeColor = Color.Red;
                }
                else
                {
                    lblReagentName.Appearance.ForeColor = Color.Black;
                }
            }
            else
            {
                lblRemain.Text = "0";
                this.Status = RemainStatus.Empty;
                this.IsSelected = false;
                lblReagentName.Appearance.ForeColor = Color.Black;
            }

        }
        #endregion

    }
}