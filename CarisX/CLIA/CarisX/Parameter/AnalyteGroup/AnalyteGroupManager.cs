using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Parameter.AnalyteGroup
{
	class AnalyteGroupManager
	{
		#region [publicメソッド]

		/// <summary>
		/// サンプル種別チェック不整合GroupName取得
		/// </summary>
		/// <returns>
		/// サンプル種別が不整合となるAnalyteGroupInfo
		/// </returns>
		/// <remarks>
		/// importした情報をの分析項目設定xmlファイルに反映する事により、サンプル種別の不整合になるAnalyteGroupInfoを返す
		/// </remarks>
		public static List<AnalyteGroupInfo> GetSumpleKindCheckErrorGroupNames(List<Tuple<Int32,Oelco.CarisX.Parameter.MeasureProtocol.SampleTypeKind>> checkProtocols)
		{
			// AnalyteGroupの読込
			Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Load();
			AnalyteGroupInfoManager wkAnalyteGroupInfoManager = Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Param;

			List<AnalyteGroupInfo> deleteGroupInfos = new List<AnalyteGroupInfo>();
			foreach (var prot in checkProtocols)
			{

				// チェック対象分析項目を登録しているAnalyteGroupを取得する
				List<AnalyteGroupInfo> wkGroupInfo = wkAnalyteGroupInfoManager.AnalyteGroupInfos
					.Where(x => x.AnalyteInfos.Count(m => m.ProtocolIndex == prot.Item1) > 0).ToList();

				foreach (AnalyteGroupInfo info in wkGroupInfo)
				{
					// 登録されている分析項目の中で、サンプル種別「血清または血漿」「尿」の両方のフラグが立っている分析項目以外は
					// 同一のサンプル種別になっているはずなので、重複を除いた1件目を取得し、比較すればOK
					var sampleKinds =
						(from m in info.AnalyteInfos
						 where Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(m.ProtocolIndex).SampleKind !=
													(MeasureProtocol.SampleTypeKind.SerumOrPlasma | MeasureProtocol.SampleTypeKind.Urine)
						 select Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromProtocolIndex(m.ProtocolIndex).SampleKind).Distinct();

					if (sampleKinds.Count() > 0)
					{
						if (prot.Item2 != sampleKinds.First())
						{
							if (!deleteGroupInfos.Contains(info))
							{
								deleteGroupInfos.Add(info);
							}
						}
					}
				}
			}
			return deleteGroupInfos;
		}

#if false

		/// 不使用の為無効にしておく

		/// <summary>
		/// 不使用分析項目登録済みGroup取得
		/// </summary>
		/// <returns>
		/// 不使用項目を登録しているAnalyteGroupInfoのリスト
		/// </returns>
		/// <remarks>
		///  import内容の、不使用分析項目を登録しているAnalyteGroupInfoを返します。
		/// </remarks>
		private List<AnalyteGroupInfo> getDeleteGroupsByImport(List<MeasureProtocol> importProtocol)
		{
			// AnalyteGroupの読込
			Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Load();
			AnalyteGroupInfoManager wkAnalyteGroupInfoManager = Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Param;

			List<AnalyteGroupInfo> deleteGroupInfos = new List<AnalyteGroupInfo>();
			foreach (var prot in importProtocol)
			{
				// 読み込んだ分析項目のEnableがfalseの場合、該当分析項目を使用しているGroupがないかチェックする
				if (prot.Enable == false)
				{
					List<AnalyteGroupInfo> wkGroupInfo = wkAnalyteGroupInfoManager.AnalyteGroupInfos
						.Where(x => x.AnalyteInfos.Count(m => m.ProtocolIndex == prot.ProtocolIndex) > 0).ToList();

					foreach (AnalyteGroupInfo info in wkGroupInfo)
					{
						if (!deleteGroupInfos.Contains(info))
						{
							deleteGroupInfos.Add(info);
						}
					}
				}

			}
			return deleteGroupInfos;
		}
#endif

		/// <summary>
		/// AnalyteGroup保存処理
		/// </summary>
		/// <remarks>
		/// AnalyteGroup.xmlに更新内容を保存し、AnalyteGroup変更イベントを発生させます。 
		/// </remarks>
		public static void SaveAnalyteGroup(List<AnalyteGroupInfo> saveInfos)
		{
			// xmlに保存する
			Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Param.AnalyteGroupInfos = saveInfos;
			Singleton<ParameterFilePreserve<AnalyteGroupInfoManager>>.Instance.Save();
			// イベント通知
			Singleton<NotifyManager>.Instance.RaiseSignalQueue((Int32)NotifyKind.ChangeAnalyteGroup, null);
		}

		#endregion
	}
}
