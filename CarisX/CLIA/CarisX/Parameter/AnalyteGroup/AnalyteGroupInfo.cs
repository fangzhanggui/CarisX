using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.CarisX.Parameter.AnalyteGroup
{
	/// <summary>
	/// 分析項目グループ情報
	/// </summary>
	public class AnalyteGroupInfo
	{
		/// <summary>
		/// グループ名
		/// </summary>
		private String groupName;

		public String GroupName
		{
			get { return groupName; }
			set { groupName = value; }
		}

		/// <summary>
		/// 分析情報リスト
		/// </summary>
		private List<AnalyteInfo> analyteInfos = new List<AnalyteInfo>();

		public List<AnalyteInfo> AnalyteInfos
		{
			get { return analyteInfos; }
			set { analyteInfos = value; }
		}


		/// <summary>
		/// 本クラスの内容をコピーした別インスタンスを返します。
		/// </summary>
		/// <returns>コピーしたインスタンス</returns>
		public AnalyteGroupInfo Copy()
		{
			AnalyteGroupInfo returnObj = new AnalyteGroupInfo();

			returnObj.GroupName = this.GroupName;

			foreach (AnalyteInfo analyteInfo in AnalyteInfos)
			{
				AnalyteInfo wkInfo = new AnalyteInfo();
				wkInfo.ProtocolIndex = analyteInfo.ProtocolIndex;
				wkInfo.AutoDilution = analyteInfo.AutoDilution;
                wkInfo.MeasTimes = analyteInfo.MeasTimes;
                returnObj.analyteInfos.Add(wkInfo);
			}

			return returnObj;
		}
	}

	/// <summary>
	/// 分析情報
	/// </summary>
	public class AnalyteInfo
	{

		/// <summary>
		/// 分析項目インデックス
		/// </summary>
		public Int32 ProtocolIndex;

		/// <summary>
		/// 自動希釈倍率
		/// </summary>
		public Int32 AutoDilution;

        /// <summary>
        /// 測定回数
        /// </summary>
        public Int32 MeasTimes;

        #region [コンストラクタ]
        public AnalyteInfo()
		{
		}
		public AnalyteInfo(Int32 protocolIndex, Int32 autoDilution, Int32 measTimes)
		{
			ProtocolIndex = protocolIndex;
			AutoDilution = autoDilution;
            MeasTimes = measTimes;

        }
		#endregion

	}
}
