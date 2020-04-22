using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Log;
using Oelco.Common.Utility;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 一般・優先検体
    /// </summary>
    /// <remarks>
    /// 一般検体・優先検体のシーケンス番号を認識する為の情報を保持します。
    /// このクラスへの設定・操作はGUIから行いません。
    /// </remarks>
    public class SampleSequenceNumberHistory
    {
        /// <summary>
        /// シーケンス番号情報
        /// </summary>
        public class SequenceNumberInfo
        {
            /// <summary>
            /// 検体識別番号
            /// </summary>
            public Int32 IndividuallyNumber;
            /// <summary>
            /// 優先フラグ
            /// </summary>
            public Boolean IsPriority;
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public SequenceNumberInfo()
            {
            }
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="individuallyNumber">検体識別番号</param>
            /// <param name="isPriority">優先検体フラグ</param>
            public SequenceNumberInfo( Int32 individuallyNumber, Boolean isPriority )
            {
                this.IndividuallyNumber = individuallyNumber;
                this.IsPriority = isPriority;
            }
        }

        /// <summary>
        /// シーケンス番号発番履歴
        /// </summary>
        private List<SequenceNumberInfo> sequenceNumberHistory = new List<SequenceNumberInfo>();

        /// <summary>
        /// シーケンス番号発番履歴 設定/取得
        /// </summary>
        public List<SequenceNumberInfo> History
        {
            get
            {
                return this.sequenceNumberHistory;
            }
            set
            {
                this.sequenceNumberHistory = value;
            }
        }

        /// <summary>
        /// 要素単一化
        /// </summary>
        /// <remarks>
        /// システム立上げ直後の、プログラム以外から編集した情報を扱う前に呼び出します。
        /// </remarks>
        public void Distinct()
        {
            // メンバ内容確認
            // 同一要素排除する。
            List<SequenceNumberInfo> temp = new List<SequenceNumberInfo>();
            foreach ( var data in this.sequenceNumberHistory )
            {
                if ( temp.Find((v)=>v.IndividuallyNumber == data.IndividuallyNumber ) == null )
                {
                    temp.Add( data );
                }
            }
            this.sequenceNumberHistory = temp;
        }

        /// <summary>
        /// 発番履歴クリア
        /// </summary>
        /// <remarks>
        /// この関数は日替わり時とシーケンス番号範囲設定変更時に呼び出します。
        /// </remarks>
        public void ClearHistory()
        {
            this.sequenceNumberHistory.Clear();
        }
        public void SelectClearHistory( List<Int32> individuallyNumberList )
        {
            foreach( var individuallyNumber in individuallyNumberList )
            {
                Int32 removeIndex = this.sequenceNumberHistory.FindIndex( (v)=>v.IndividuallyNumber == individuallyNumber ) ;
                if ( removeIndex >= 0 )
                {
                    this.sequenceNumberHistory.RemoveAt( removeIndex );
                }
            }
        }

        /// <summary>
        /// 履歴追加
        /// </summary>
        /// 分析DBと同等のタイミングで情報を追加します。
        /// <param name="individuallyNumber">検体識別番号</param>
        /// <param name="isPriority">優先検体フラグ</param>
        public void AddHistory( Int32 individuallyNumber, Boolean isPriority )
        {

            if ( !this.sequenceNumberHistory.Any( ( v ) => v.IndividuallyNumber == individuallyNumber ) )
            {
                this.sequenceNumberHistory.Add( new SequenceNumberInfo( individuallyNumber, isPriority ) );
            }
            else
            {
                Singleton<CarisXLogManager>.Instance.Write( Oelco.Common.Log.LogKind.DebugLog, String.Empty,
                    String.Format( "既に存在する検体識別番号が履歴に追加されようとしました。 Ind={0}", individuallyNumber ) );
            }

            // 最大値のみ残すと、分析中終了された場合に再起動した際にDBからデータの同期が必要になるが、
            // 優先検体かどうかは区別が付かなくなってしまう為、全て残す。
        }
    }
}
