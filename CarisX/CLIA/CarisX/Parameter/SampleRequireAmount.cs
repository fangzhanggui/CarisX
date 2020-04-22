using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// サンプル必要量設定クラス
    /// </summary>
    public class SampleRequireAmount : ISavePath,ISampleReqAmount
    {
        /// <summary>
        /// サンプル必要量テーブルA
        /// </summary>
        private SampleAmountReqTableA tableA = new SampleAmountReqTableA();
        /// <summary>
        /// サンプル必要量テーブルB
        /// </summary>
        private SampleAmountReqTableB tableB = new SampleAmountReqTableB();

        // TODO:サンプル必要量
        // 初期シーケンスでの送信にのみ読みこまれる。
        // 参照↓↓
        // G SampleRequiredAmount.ini
        // G コマンドリスト　0084
        // C コマンドリスト　0085

        #region ISavePath メンバー

        /// <summary>
        /// 保存パス
        /// </summary>
        public String SavePath
        {
            get
            {
                return CarisXConst.PathSystem + @"\SampleRequireAmount.xml";
            }
        }

        #endregion

        #region ISampleReqAmount メンバー

        /// <summary>
        /// Aテーブル
        /// </summary>
        public SampleAmountReqTableA TableA
        {
            get
            {
                return this.tableA;
            }
            set
            {
                this.tableA = value;
            }
        }

        /// <summary>
        /// Bテーブル
        /// </summary>
        public SampleAmountReqTableB TableB
        {
            get
            {
                return this.tableB;
            }
            set
            {
                this.tableB = value;
            }
        }

        #endregion
    }
}
