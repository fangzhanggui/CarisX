using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



// TODO:コメント不十分

namespace Oelco.Common.GUI
{
	/// <summary>
	/// X軸最小値取得処理
	/// </summary>
	/// <returns>X軸最小値</returns>
	public delegate Int32 DlgGetXMinValue();
	/// <summary>
	/// X軸最大値取得処理
	/// </summary>
	/// <returns>X軸最大値</returns>
	public delegate Int32 DlgGetXMaxValue();
	/// <summary>
	/// X軸値取得処理
	/// </summary>
	/// <returns>X軸値</returns>
	public delegate Int32 DlgGetXValue();
	/// <summary>
	/// X軸値設定処理
	/// </summary>
	/// <param name="value">X軸値</param>
	public delegate void DlgSetXValue( Int32 value );

	/// <summary>
	/// Y軸最小値取得処理
	/// </summary>
	/// <returns>Y軸最小値</returns>
	public delegate Int32 DlgGetYMinValue();
	/// <summary>
	/// Y軸最大値取得処理
	/// </summary>
	/// <returns>Y軸最大値</returns>
	public delegate Int32 DlgGetYMaxValue();
	/// <summary>
	/// Y軸値取得処理
	/// </summary>
	/// <returns>Y軸値</returns>
	public delegate Int32 DlgGetYValue();
	/// <summary>
	/// Y軸値設定処理
	/// </summary>
	/// <param name="value">Y軸値</param>
	public delegate void DlgSetYValue( Int32 value );

	/// <summary>
	/// スクロール処理代行クラス
	/// </summary>
	public class ScrollProxy
	{

		/// <summary>
		/// 最小値取得(X軸)
		/// </summary>
		public DlgGetXMinValue GetXMinValue = null;

		/// <summary>
		/// 最大値取得(X軸)
		/// </summary>
		public DlgGetXMaxValue GetXMaxValue = null;

		/// <summary>
		/// 値取得(X軸)
		/// </summary>
		public DlgGetXValue GetXValue = null;
		
		/// <summary>
		/// 値設定(X軸)
		/// </summary>
		public DlgSetXValue SetXValue = null;

		/// <summary>
		/// 最小値取得(Y軸)
		/// </summary>
		public DlgGetYMinValue GetYMinValue = null;

		/// <summary>
		/// 最大値取得(Y軸)
		/// </summary>
		public DlgGetYMaxValue GetYMaxValue = null;

		/// <summary>
		/// 値取得(Y軸)
		/// </summary>
		public DlgGetYValue GetYValue = null;

		/// <summary>
		/// 値設定(Y軸)
		/// </summary>
		public DlgSetYValue SetYValue = null;
		
	}
}
