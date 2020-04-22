using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolConverter.File
{
    /// <summary>
    /// 分析項目ファイル操作クラス
    /// </summary>
    /// <remarks>
    /// 分析項目ファイルの読込みや書込みを行うクラスです。
    /// </remarks>
	public class MeasureProtocolManager
    {

        #region [定数定義]

        /// <summary>
        /// 分析項目最大数
        /// </summary>
        private const Int32 PROTOCOL_MAX_COUNT = 200;
 
        #endregion

        #region [クラス変数定義]

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 分析項目
        /// </summary>
		private List<ParameterFilePreserve< MeasureProtocol >> measureProtocol = new List<ParameterFilePreserve< MeasureProtocol >>();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MeasureProtocolManager()
        {           
        }

        #endregion

        #region [プロパティ]
        
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 分析項目退避領域の追加
        /// </summary>
        /// <param name="index">プロトコルインデックス</param>
        /// <returns>追加した領域</returns>
        public ParameterFilePreserve<MeasureProtocol> AddMeasureProtocol( Int32 index )
        {            
            ParameterFilePreserve<MeasureProtocol> protocol = new ParameterFilePreserve<MeasureProtocol>();                          

            // 分析項目番号を作成
            if ( this.GetMeasureProtocolFromProtocolIndex( index ) == null )
            {
                protocol.Param.ProtocolIndex = index;
                this.measureProtocol.Add( protocol );
            }
            else
            {
                protocol = null;
            }
              
            return protocol; 
        }

        /// <summary>
        /// 全プロトコルの一括保存
        /// </summary>
        /// <param name="path">保存パス</param>
        /// <returns></returns>
        public Int32 SaveAllMeasureProtocol(String path)
        {
            Int32 result =0;

            // 分析項目をファイルへ全て書き込む
            foreach ( var protocol in this.measureProtocol )
            {
                protocol.Param.SetSaveProtocolPath(path);
                if ( protocol.Save())
                {
                   result++;  
                }
            }
            return result;
        }       

        /// <summary>
        /// 分析項目設定取得
        /// </summary>
        /// <remarks>
        /// 分析項目設定情報を分析項目インデックスから取得します。
        /// </remarks>
        /// <param name="name">分析項目インデックス</param>
        /// <returns>分析項目情報</returns>
        public MeasureProtocol GetMeasureProtocolFromProtocolIndex( Int32 protocolIndex )
        {
            MeasureProtocol protocol = null;

            // 分析項目インデックスから測定項目設定を検索
            IEnumerable<MeasureProtocol> searchResult = from p in this.measureProtocol
                                                        where p.Param.ProtocolIndex == protocolIndex
                                                        select p.Param;

            // 検索結果を取得
            if ( searchResult.Count() != 0 )
            {
                protocol = searchResult.First();
            }

            return protocol;
        }       

     
        #endregion

        #region [protectedメソッド]

        #endregion

        #region [privateメソッド]

        #endregion 
        
	}
	 
}
 
