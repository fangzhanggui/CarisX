using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Utility
{
	/// <summary>
	/// 動作内容
	/// </summary>
	public delegate void HistoryAction();

    /// <summary>
    /// 記録動作種別
    /// </summary>
    /// <remarks>
    /// 記録動作種別は、各分類毎にこのクラスを継承して定義します。
    /// </remarks>
    public class HistoryActionKind
    {

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="target"></param>
        ///// <returns></returns>
        //public virtual Boolean IsSame( HistoryActionKind target )
        //{
        //    return true;
        //}
    }

	/// <summary>
	/// 履歴管理クラス
	/// </summary>
	/// <remarks>
	/// 動作の履歴保持、再実行を行うクラスです。
	/// </remarks>
	public class HistoryManager
	{

		/// <summary>
		/// 履歴状態に変化があった場合発生するイベント
		/// </summary>
		public event EventHandler<EventArgs> HistChanged;
		
		#region [定数定義]

        ///// <summary>
        ///// 履歴種別
        ///// </summary>
        ///// <remarks>
        ///// 履歴を取る種別をここに追加します。
        ///// </remarks>
        //public enum ActionKind
        //{
        //    ShowFormA,
        //    ShowFormB,
        //    ShowFormC
        //}

		#endregion

		#region [インスタンス変数定義]

		/// <summary>
		/// 履歴リスト
		/// </summary>
        private List<Tuple<HistoryActionKind, HistoryAction>> m_History = new List<Tuple<HistoryActionKind, HistoryAction>>();

		/// <summary>
		/// 動作カウント
		/// </summary>
        private Dictionary<HistoryActionKind, Int32> m_ActionCount = new Dictionary<HistoryActionKind, Int32>();

		/// <summary>
		/// 現在位置
		/// </summary>
		private Int32 m_CurrentPos = -1;

		/// <summary>
		/// 履歴保持数
		/// </summary>
		private UInt32 m_Preserv = 10;

        ///// <summary>
        ///// 履歴存在フラグ
        ///// </summary>
        //private Boolean hasHistory = false;
		#endregion

		#region [プロパティ]

		/// <summary>
		/// 履歴保持数
		/// </summary>
		public UInt32 Preserv
		{
			get
			{
				return m_Preserv;
			}
			set
			{
				m_Preserv = value;

				// 保持数オーバー時、差分を削除
				limitCheck();

				//@@@ 現在、Preserv変更時のlimitCheckによりカレント位置変化した場合、履歴更新イベントを発生させない。
			}
		}
		/// <summary>
		/// 現在位置
		/// </summary>
		public Int32 CurrentPos
		{
			get
			{
				return m_CurrentPos;
			}
		}
        /// <summary>
        /// 履歴リスト数
        /// </summary>
        public Int32 HistoryCount
        {
            get
            {
                return this.m_History.Count;
            }
        }
		#endregion


		#region [publicメソッド]

		/// <summary>
		/// 履歴リスト追加
		/// </summary>
        /// <remarks>
        /// 履歴リストを追加します。
        /// </remarks>
        /// <param name="kind">動作種別</param>
		/// <param name="act">動作内容</param>
        public void AddNew( HistoryActionKind kind, HistoryAction act )
		{

			// 現在位置以降を削除し、末尾に履歴を追加する。
			if ( m_CurrentPos >= 0 )
			{
				m_History.RemoveRange( m_CurrentPos, m_History.Count - m_CurrentPos );
			}
			else
			{
				m_CurrentPos = 0;
			}

			// 履歴リストに追加する
            m_History.Add( new Tuple<HistoryActionKind, HistoryAction>( kind, act ) );
			m_CurrentPos++; // カレント位置移動

			// 保持数オーバー時、差分を削除
			limitCheck();

            // 履歴状態変化イベント
            OnHistoryChanged();

		}


		/// <summary>
		/// 最新内容実施
		/// </summary>
        /// <remarks>
        /// 履歴リストの最新内容を実行します。
        /// </remarks>
        public void ExecRecent()
		{
			if ( m_History.Count != 0 )
			{
				execHistory( m_History.Count() - 1 );
			}
		}

		/// <summary>
		/// カレント位置前進
		/// </summary>
        /// <remarks>
        /// １つ前の履歴をカレント位置に設定します。
        /// </remarks>
        /// <returns>True:移動成功 False:移動失敗</returns>
		public Boolean MoveNext()
		{
			Boolean result = true;


            Int32 nextPos = m_CurrentPos + 1;

            if ( nextPos > m_History.Count )
            {
                // 履歴が無ければ-1になる。
                if ( m_History.Count != 0 )
                {
                    m_CurrentPos = m_History.Count;
                }
                else
                {
                    m_CurrentPos = -1;
                }
                result = false;
            }
            else
            {
                m_CurrentPos = nextPos;

                // 履歴状態変化イベント
                OnHistoryChanged();
            }


			return result;

		}
		/// <summary>
		/// 次位置存在確認
		/// </summary>
        /// <remarks>
        /// カレント位置より後の履歴が存在するかどうかを返します。
        /// </remarks>
        /// <returns>True:存在 False:不在</returns>
		public Boolean ExistNext()
		{

			Boolean result = true;

            // 履歴が無ければfalseを返す
            if ( m_History.Count == 0 )
            {
                result = false;
            }
            else
            {
                Int32 nextPos = m_CurrentPos + 1;

                // 1件しか履歴にない場合、進むも戻るも不可
                if ( nextPos > m_History.Count )
                {
                    result = false;
                }
            }

			return result;
		}

		/// <summary>
		/// カレント位置後退
		/// </summary>
        /// <remarks>
        /// １つ後の履歴をカレント位置に設定します。
        /// </remarks>
        /// <returns>True:移動成功 False:移動失敗</returns>
		public Boolean MovePrev()
		{
			Boolean result = true;

            Int32 prevPos = m_CurrentPos - 1;

			// 履歴が無ければ-1、あれば1で止まる。
			// 1件しか履歴にない場合、進むも戻るも不可
            if ( prevPos <= 0 )
            {
                if ( m_History.Count == 0 )
                {
                    m_CurrentPos = -1;
                }
                else
                {
                    m_CurrentPos = 1;
                }

                result = false;
            }
            else
            {
                m_CurrentPos = prevPos;
             
                // 履歴状態変化イベント
                OnHistoryChanged();

            }

			return result;
		}

		/// <summary>
		/// 前位置存在確認
		/// </summary>
        /// <remarks>
        /// カレント位置より前に履歴が存在するかどうかを返します。
        /// </remarks>
        /// <returns>True:存在 False:不在</returns>
		public Boolean ExistPrev()
		{
			Boolean result = true;

			Int32 prevPos = m_CurrentPos - 1;

			if ( prevPos <= 0 )
			{

				result = false;
			}

			return result;
		}

		/// <summary>
		/// カレント位置実施
		/// </summary>
        /// <remarks>
        /// カレント位置が存在すれば該当履歴を実施します。
        /// </remarks>
        public void ExecCurrent()
		{
			// カレント位置が存在すれば実施
			if ( m_CurrentPos > 0 )
			{
				execHistory( m_CurrentPos - 1 );
			}
		}

		/// <summary>
		/// 履歴消去
		/// </summary>
        /// <remarks>
        /// 履歴リストをクリアします。
        /// </remarks>
        public void ClearHistory()
		{
			m_ActionCount.Clear();
			m_History.Clear();
			m_CurrentPos = -1;

            // 履歴状態変化イベント
            OnHistoryChanged();
		}

		/// <summary>
		/// 動作回数取得
		/// </summary>
		/// <remarks>
		/// 指定した種別の動作回数を取得します。
		/// </remarks>
		/// <param name="kind">実行種別</param>
		/// <returns>動作回数</returns>
        public Int32 GetExecCount( HistoryActionKind kind )
		{
			Int32 count = 0;

			// 履歴カウント取得
			if ( m_ActionCount.ContainsKey( kind ) )
			{
				count = m_ActionCount[kind];
			}

			return count;
		}

        public HistoryActionKind getCurrentActionKind()
        {
            if (m_CurrentPos > 0)
            {
                return this.m_History[m_CurrentPos - 1].Item1;
            }
            else
            {
                return null;
            }
        }

		/// <summary>
		/// 履歴実行
		/// </summary>
        /// <remarks>
        /// 指定されたインデックスの履歴を実行します。
        /// </remarks>
        /// <param name="index">実行インデックス</param>
		protected void execHistory( Int32 index )
		{
            HistoryActionKind kind = this.m_History[index].Item1;

			// 回数カウント
			if ( m_ActionCount.ContainsKey( kind ) == false )
			{
				m_ActionCount.Add( kind, 0 );
			}
			m_ActionCount[kind]++;

            // 動作実施
#if DEBUG

            this.m_History[index].Item2();
#else
            try
            {
                this.m_History[index].Item2();
            }
            catch ( Exception ex )
            {
                // 履歴デリゲートの実行時に例外発生。
                // もしここに来た場合、履歴に取っている内容から関連する動作に必要なリソースが開放されている等、
                // コーディング上の問題が予想される。
                System.Diagnostics.Debug.WriteLine("履歴実行時にエラーが発生しました Message = {0} StackTrace = {1}", ex.Message, ex.StackTrace);
            }
#endif
		}

		#endregion


		#region [protectedメソッド]


		/// <summary>
		/// 履歴数の保持数整理を行います。
		/// </summary>
        /// <remarks>
        /// 履歴の数が、保持数を超過している際、
        /// 古いものから削除します。
        /// </remarks>
		protected void limitCheck()
		{
			Int32 diff = m_History.Count - (Int32)this.Preserv;
			Int32 oldCount = m_History.Count;

			// 保持数超過時削除
			if ( diff > 0 )
			{
				m_History.RemoveRange( 0, diff );
				if ( m_CurrentPos > m_History.Count )
				{
					m_CurrentPos = m_History.Count;
				}
			}
		}

        /// <summary>
        /// 履歴状態変化イベント
        /// </summary>
        /// <remarks>
        /// 履歴状態変化イベントの通知を行います。
        /// </remarks>
        protected void OnHistoryChanged()
        {
            // 履歴状態変化イベント
            if ( HistChanged != null )
            {
                HistChanged( this, new EventArgs() );
            }
        }

		#endregion


	}
		
}
