using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Parameter.AnalyteGroup
{
	public class AnalyteGroupInfoManager:ISavePath
	{

		#region [インスタンス変数定義]
		
		/// <summary>
		/// AnalyteGroupInfoリスト
		/// </summary>
		public List<AnalyteGroupInfo> AnalyteGroupInfos = new List<AnalyteGroupInfo>();

		#endregion

		#region [コンストラクタ]

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public AnalyteGroupInfoManager()
		{ 
		
		}

		#endregion

		#region ISavePath メンバー

		/// <summary>
		/// 保存パス
		/// </summary>
		public String SavePath
		{
			get
			{
				return System.IO.Path.Combine(CarisXConst.PathProtocol, "AnalyteGroup.xml");
			}
		}

		#endregion
	}

}
