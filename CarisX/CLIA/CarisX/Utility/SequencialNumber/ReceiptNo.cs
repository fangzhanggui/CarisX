using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// 受付番号クラス
    /// </summary>
    /// <remarks>
    /// 各種検体のDB登録時に発番されます。
    /// この番号は再検査の場合、新規に発番を行いません。
    /// この番号は日替わり処理実施時にリセットします。
    /// </remarks>
    public class ReceiptNo : NumberingBase
    {

        /// <summary>
        /// 発番を除外する番号のリスト
        /// </summary>
        List<Int32> skipNumberList = new List<Int32>();

        #region [publicメソッド]

        /// <summary>
        /// 次発番問合せ
        /// </summary>
        /// <remarks>
        /// 実際に保持する値のインクリメントを行わず、次に作成する番号を返します。
        /// </remarks>
        /// <returns>次発番番号</returns>
        public Int32 AskNextNumber()
        {
            Int32 number = this.Number;

            // 次発番で一周する場合は初期値を返す
            // スキップを行う値を飛ばして次の発番を探す
            Boolean isFirst = this.judgeFirstNumber();
            do
            {
                if ( isFirst )
                {
                    isFirst = false;
                }
                else
                {
                    number += this.IncrementCount;
                }

                if ( number > this.EndCount )
                {
                    number = this.StartCount;
                    break;
                }
            } while ( this.skipNumberList.Contains( number ) );


            return number;
        }

        /// <summary>
        /// 発番済み外部生成番号設定
        /// </summary>
        /// <remarks>
        /// 発番済みの外部生成番号を設定します。
        /// </remarks>
        /// <param name="extNumber">発番済み外部生成番号</param>
        /// <returns>結果(true:設定成功/false:設定失敗)</returns>
        public Boolean ThroughExternalCreatedNumber( Int32 extNumber )
        {
            Boolean exist = this.skipNumberList.Contains( extNumber );

            if ( !exist )
            {
                this.skipNumberList.Add( extNumber );

                // ローカル保持の番号より小さい場合、発番済扱いとする。
                exist = extNumber <= this.Number;
            }
            
            // 0の場合無視する
            if ( extNumber == 0 )
            {
                exist = false;
            }

            // Trueなら存在する。Falseなら存在しない。存在する場合は設定失敗なので反転して返す
            return !exist;
        }

        ///// <summary>
        ///// 外部生成番号設定
        ///// </summary>
        ///// <param name="extNumber"></param>
        ///// <returns></returns>
        //public Boolean ThroughExternalCreatedNumberRange( List<Int32> extNumber )
        //{
        //    var result = from v in extNumber
        //                 select this.ThroughExternalCreatedNumber( v );
        //    return result.All( ( v ) => v == true );
        //}

        /// <summary>
        /// データ初期化
        /// </summary>
        /// <remarks>
        /// 設定クラスから受付番号情報を取得し、
        /// 受付番号クラスの情報を初期化します。
        /// </remarks>
        public override void Initialize()
        {
            // 設定クラスから情報を取得
            //this.EndCount = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReceiptNoInfo.CountMax;
            //this.StartCount = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReceiptNoInfo.CountMin;
           
            this.EndCount = CarisXConst.RECEIPT_NUMBER_MAX;
            this.StartCount = 1;

            this.Number = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReceiptNoInfo.CountNow;
            this.LatestNumberingDate = Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReceiptNoInfo.LatestNumberDate;

            // 外部発番データを読み込む。
            Singleton<ParameterFilePreserve<ExternalCreatedReceiptNo>>.Instance.Load();
            this.skipNumberList = Singleton<ParameterFilePreserve<ExternalCreatedReceiptNo>>.Instance.Param.Number;


            base.Initialize();
        }
        #endregion


        #region [protectedメソッド]


        public override void ResetNumber()
        {
            base.ResetNumber();
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReceiptNoInfo.CountNow = this.Number;
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReceiptNoInfo.LatestNumberDate = this.LatestNumberingDate;

        }

        /// <summary>
        /// インクリメント
        /// </summary>
        /// <remarks>
        /// do the increment of receipt number
        /// </remarks>
        protected override void increment()
        {
            // 非発番リストを回避して発番する。
            do
            {
                base.increment();
            } while ( this.skipNumberList.Contains( this.Number ) );

            // 設定クラスへ情報を設定
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReceiptNoInfo.CountNow = this.Number;
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Param.ReceiptNoInfo.LatestNumberDate = this.LatestNumberingDate;

            // アプリケーションのフリーズ等発生した時も保持されているよう、即座に保存を行う。
            Singleton<ParameterFilePreserve<AppSettings>>.Instance.Save(); 
        }

        /// <summary>
        /// 最大値超過処理
        /// </summary>
        /// <remarks>
        /// 現在値が最大値を超過した際に呼び出されます。Current value is I will be called when that exceeded the maximum value.
        /// 現在値を調整する処理を定義します。I define the process of adjusting the current value.
        /// </remarks>
        protected override void overFlow()
        {
            base.overFlow();

            // 非発番リストを削除する。
            this.skipNumberList.Clear();

        }

        #endregion
    }
    /// <summary>
    /// 外部生成受付番号情報クラス
    /// </summary>
    public class ExternalCreatedReceiptNo : ISavePath
    {
        private List<Int32> number = new List<Int32>();

        public List<Int32> Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        #region ISavePath メンバー

        public string SavePath
        {
            get
            {
                return Oelco.CarisX.Const.CarisXConst.PathSystem + @"\externalReceiptNo.xml";
            }
        }

        #endregion
    }
}
