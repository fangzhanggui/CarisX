using System;
using System.Collections.Generic;
using System.Linq;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Common
{

    /// <summary>
    /// キャリブレータ情報
    /// </summary>
    public class CalibratorInfo
    {
        #region [プロパティ]

        /// <summary>
        /// モジュールID
        /// </summary>
        public Int32 ModuleId { get; set; }

        /// <summary>
        /// ポート番号
        /// </summary>
        public Int32 PortNo { get; set; }

        /// <summary>
        /// 試薬コード
        /// </summary>
        public Int32 ReagentCode { get; set; }

        /// <summary>
        /// キャリブレータ本数
        /// </summary>
        public Int32 CalibratorLotCount { get; set; }

        /// <summary>
        /// キャリブレータロット
        /// </summary>
        public List<CalibratorLot> CalibratorLot { get; set; }

        #endregion
    }

    /// <summary>
    /// キャリブレータ情報管理
    /// </summary>
    public class CalibratorInfoManager
	{
        #region [インスタンス変数定義]

        /// <summary>
        /// 初期化完了フラグ
        /// </summary>
        public Boolean blnInitialized = false;

        #endregion

        #region [プロパティ]

        /// <summary>
        /// キャリブレータ情報
        /// </summary>
        public List<CalibratorInfo> CalibratorLot { get; set; } = new List<CalibratorInfo>();

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// ラック情報初期化
        /// </summary>
        /// <remarks>
        /// ラック情報初期化します
        /// </remarks>
        public void CalibratorInfoInitialize()
        {
            if (!this.blnInitialized)
            {
                //キャリブレータ情報をクリア
                CalibratorLot.Clear();

                //キャリブレータ情報初期化
                CalibratorLot = new List<CalibratorInfo>();

                this.blnInitialized = true;
            }
        }

        /// <summary>
        /// キャリブレータ情報設定
        /// </summary>
        /// <remarks>
        /// キャリブレータ情報を設定します。
        /// </remarks>
        /// <param name="info">キャリブレータ情報のインスタンス</param>
        public void SetCalibratorInfo(CalibratorInfo info)
        {
            //セットしようとしているキャリブレータ情報がすでに存在する場合は置き換える。
            if (this.CalibratorLot.Exists(v => v.ModuleId == info.ModuleId && v.PortNo == info.PortNo))
            {
                this.CalibratorLot.RemoveAt(this.CalibratorLot.FindIndex(v => v.ModuleId == info.ModuleId && v.PortNo == info.PortNo));
            }

            this.CalibratorLot.Add(info);
        }

        /// <summary>
        /// 保持データクリア
        /// </summary>
        /// <remarks>
        /// 保持データをクリアします。
        /// </remarks>
        public void Clear()
        {
            // ラックステータスクリア処理
            this.CalibratorLot.Clear();
            this.blnInitialized = false;
        }

        #endregion
    }
}
 
