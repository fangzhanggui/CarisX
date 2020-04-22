using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Parameter.AnalyteGroup;
using Oelco.Common.GUI;

namespace Oelco.CarisX.GUI.Controls
{
	public partial class GroupSelectAnalysisSettingPanel:AnalysisSettingPanel
	{
		#region [インスタンス変数定義]

		/// <summary>
		/// 分析項目選択状態変更時イベントハンドラ
		/// </summary>
		public event Action<Int32, Int32, Int32, Boolean> ProtocolCheckChangedByGroup;

		/// <summary>
		/// AnalyteGroupボタンのインスタンスをリスト化する
		/// </summary>
		private List<CustomUStateButton> analyteGroupButtons = new List<CustomUStateButton>();

		#endregion

		#region [コンストラクタ/デストラクタ]

		/// <summary>
        /// コンストラクタ
        /// </summary>
		public GroupSelectAnalysisSettingPanel()
        {
            InitializeComponent();

			// AnalyteGroupと、インデックスの関連付け
			this.analyteGroupButtons.Add(this.btnAnalyteGroup1);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup2);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup3);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup4);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup5);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup6);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup7);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup8);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup9);
			this.analyteGroupButtons.Add(this.btnAnalyteGroup10);

			// 項目タイトルの設定
			this.gbxAnalyteGroup.Text = CarisX.Properties.Resources.STRING_GROUP_SELECT_ANALYSIS_SETTINGPANEL_001;
		}

		#endregion

		#region [protectedメソッド]

		/// <summary>
		/// ボタン情報を初期化
		/// </summary>
		/// <remarks>
		/// ボタン情報を初期化します
		/// </remarks>
		protected override void initializeProtocolButtonInfo()
		{
			// base機能の呼び出し
			base.initializeProtocolButtonInfo();
			// Group選択ボタン情報設定
			this.SetGroupButton();
		}
		
		/// <summary>
		/// 分析項目情報更新
		/// </summary>
		/// <remarks>
		/// 分析項目情報更新します
		/// </remarks>
		protected override void refleshProtocolItems()
		{
			base.refleshProtocolItems();
		}
		
		#endregion

		#region [privateメソッド]

		/// <summary>
		/// Group選択ボタン情報設定
		/// </summary>
		/// <remarks>
		/// AnalyteGroup.xmlの内容を参照し、AnalyteGroupボタンの設定を行います。
		/// </remarks>
		public void SetGroupButton()
		{
			// ボタン設定のクリア
			foreach (var btn in this.analyteGroupButtons)
			{
				btn.Text = String.Empty;
				btn.Tag = null;
				btn.Enabled = false;

			}

			// Group選択ボタン情報設定
			Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Load();
			List<AnalyteGroupInfo> wkGroupInfos = Singleton<ParameterFilePreserve<Parameter.AnalyteGroup.AnalyteGroupInfoManager>>.Instance.Param.AnalyteGroupInfos;
			Int32 btnIndex = 0;
			foreach (var groupInfo in wkGroupInfos)
			{
				this.analyteGroupButtons[btnIndex].Enabled = true;
				this.analyteGroupButtons[btnIndex].Text = groupInfo.GroupName;
				this.analyteGroupButtons[btnIndex].Tag = groupInfo.AnalyteInfos;
				btnIndex++;
			}
		}

		/// <summary>
		/// AnalyteGroupボタン押下時処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>
		/// 選択されたAnalyteGroupに登録されている分析項目をONの状態に設定します。
		/// 設定時、ONになる分析項目のサンプル種別に矛盾があれば、エラーメッセージを表示し、AnalyteGroup選択をキャンセルします。
		/// </remarks>
		private void btnAnalyteGroup_Click(object sender, EventArgs e)
		{
			// 選択したボタンの取得
			CustomUStateButton selButton = (CustomUStateButton)sender;
            Boolean isCalcRoma = false;
            if (selButton.Text == "ROMA")
            {
                isCalcRoma = true;
            }

			List<Tuple<Int32, Int32, Int32>> selectProtocols = new List<Tuple<Int32, Int32, Int32>>();

			bool isCancel = false;

            //　急診モードの分析項目追加フラグ
            bool enabledFlag = Singleton<ParameterFilePreserve<CarisXSystemParameter>>.Instance.Param.AssayModeParameter.IsProtocolEnabledChangedInEmergencyMode();
    
            // 指定グループに登録されている項目を全て設定する。			
            foreach (AnalyteInfo info in ((List<AnalyteInfo>)selButton.Tag))
			{
				// 設定の際、1件ずつに対して適用前にProtocolCheckChangingイベントを発生させる。
				// 親画面にて、不正な組み合わせの登録である場合、ProtocolCheckChangingイベントの処理結果より
				// 登録内容をキャンセル指定される、その場合記憶していた元の選択状態を設定する。
				AnalisisSettingPanelSelectChangingData item = new AnalisisSettingPanelSelectChangingData(((CustomUStateButton)sender).CurrentState, false,isCalcRoma);
				base.OnProtocolCheckChanging(info.ProtocolIndex, ref item);
				if (item.Cancel)
				{
					isCancel = true;						
					break;
				}
				else
				{
                    // 急診モードの分析項目を追加してはいけない場合
                    if (enabledFlag)
                    {
                        var protocal = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(info.ProtocolIndex);
                        // 分析項目の急診使用がなしの場合は追加
                        if (!protocal.UseEmergencyMode)
                        {
                            selectProtocols.Add(new Tuple<Int32, Int32, Int32>(info.ProtocolIndex, info.AutoDilution, info.MeasTimes));
                        }

                    }
                    else
                    {
                        selectProtocols.Add(new Tuple<Int32, Int32, Int32>(info.ProtocolIndex, info.AutoDilution, info.MeasTimes));

                    }                       
				}
			}

			// ProtocolCheckChangingイベントでキャンセルされなかったら選択したGroupの分析項目のボタンをONにする
			if (!isCancel)
			{
				// 選択状態を一旦クリアする
				this.ClearSelectButtons();
				// 対象分析項目のボタンをONにする
				base.SetProtocolSettingState(selectProtocols);
				foreach (var info in selectProtocols)
				{
					// ProtocolCheckChangedイベントを発生させる
					this.ProtocolCheckChangedByGroup(info.Item1, info.Item2, info.Item3, true);
				}
			}
			

			selButton.CurrentState = false;
		}

		/// <summary>
		/// 分析項目ボタン選択状態クリア
		/// </summary>
		/// <remarks>
		/// 分析項目ボタンの選択状態をクリアします。
		/// </remarks>
		private void ClearSelectButtons()
		{
			foreach (var btn in base.ProtocolIndexToButtonDic)
			{
				btn.Value.CurrentState = false;
				this.ProtocolCheckChangedByGroup(btn.Key, 1, 1, false);
			}
		}

		#endregion
	}
}
