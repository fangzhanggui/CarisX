using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using Oelco.CarisX.Const;
using Oelco.Common.Parameter;
using Oelco.Common.Utility;
using Oelco.CarisX.Log;
using Oelco.Common.Log;


namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// DPR管理のプロトコル名称-ホスト向けプロトコル番号クラス
    /// </summary>
    public class DPRProtocolNameAndHostProtocolNumber
    {
        /// <summary>
        /// DPR管理のプロトコル名称
        /// </summary>
        private String dprProtocolName = String.Empty;
        /// <summary>
        /// ホスト向けプロトコル番号
        /// </summary>
        private Int32 hostProtocolNumber = 0;

        /// <summary>
        /// DPR管理のプロトコル名称の取得、設定
        /// </summary>
        [XmlAttribute]
        public String DPRProtocolName
        {
            get
            {
                return this.dprProtocolName;
            }
            set
            {
                this.dprProtocolName = value;
            }
        }
        /// <summary>
        /// ホスト向けプロトコル番号の取得、設定
        /// </summary>
        public Int32 HostProtocolNumber
        {
            get
            {
                return hostProtocolNumber;
            }
            set
            {
                hostProtocolNumber = value;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DPRProtocolNameAndHostProtocolNumber()
        {
            
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Number"></param>
        public DPRProtocolNameAndHostProtocolNumber( String name, Int32 Number )
        {
            this.DPRProtocolName = name;
            this.HostProtocolNumber = Number;
        }
    }
    /// <summary>
    /// 分析項目情報
    /// </summary>
    public class MeasureProtocolInfo : ISavePath
    {
        /// <summary>
        /// DPR管理のプロトコル名称-ホスト向けプロトコル番号リスト
        /// </summary>
        private List<DPRProtocolNameAndHostProtocolNumber> dprNameAndHostNumber = new List<DPRProtocolNameAndHostProtocolNumber>();

        /// <summary>
        /// DPR管理のプロトコル名称-ホスト向けプロトコル番号リストの取得、設定
        /// </summary>
        public List<DPRProtocolNameAndHostProtocolNumber> DPRNameAndHostNumber
        {
            get
            {
                return dprNameAndHostNumber;
            }
            set
            {
                dprNameAndHostNumber = value;
            }
        }

        /// <summary>
        /// ホスト用プロトコル番号重複確認
        /// </summary>
        /// <remarks>
        /// ホスト用プロトコル番号設定で、重複があるか調べます。
        /// </remarks>
        /// <returns>True:重複あり False:重複なし</returns>
        public Boolean IsConflictHostProtocolNumber()
        {
            Boolean conflict = false;

            // 番号重複チェック
            conflict = this.dprNameAndHostNumber.IsConflict( ( v ) => v.HostProtocolNumber );

            //// 番号重複チェック
            //var numbers = from v in this.dprNameAndHostNumber select v.HostProtocolNumber;
            //foreach( var num in numbers )
            //{
            //    if ( numbers.Contains(num) )
            //    {
            //        conflict = true;
            //        break;
            //    }
            //}

            return conflict;
        }

        /// <summary>
        /// 分析項目順番リスト
        /// </summary>
        private List<String> measureProtocolTurnList = new List<String>();
        /// <summary>
        /// 分析項目順番リストの取得、設定
        /// </summary>
        public List<String> MeasureProtocolTurnList
        {
            get
            {
                return this.measureProtocolTurnList;
            }
            set
            {
                this.measureProtocolTurnList = value;
            }
        }




        /// <summary>
        /// 分析項目同期
        /// </summary>
        /// <remarks>
        /// この関数は、MeasureProtocolInfoがファイルからデシリアライズされた直後、
        /// 有効なMeasurProtocolManagerを指定して呼び出します。
        /// ファイルに定義された名称と実際のMeasureProtocolManagerの定義に差異がある場合、
        /// 自身の情報に存在しないものは追加され、余分に存在するものは削除されます。
        /// </remarks>
        /// <param name="measProtocol">分析項目管理</param>
        public void SyncMeasProtocolManager( MeasureProtocolManager measProtocol )
        {
            // MeasureProtocolManagerに存在する分析項目名称を抽出
            IEnumerable<String> useNameList = from v in measProtocol.UseMeasureProtocolList
                                              select v.ProtocolName;        

            for ( Int32 index = 0; index < this.measureProtocolTurnList.Count; index++ )
            {
                // MeasureProtocolManagerに存在しない名前があれば削除する
                if ( !useNameList.Contains( this.measureProtocolTurnList[index] ) )
                {
                    this.measureProtocolTurnList.RemoveAt( index );
                    index--;
                }
            }
            foreach ( String protocolName in useNameList )
            {
                // 自身に含まれていないリスト内容を追加する。
                if ( !this.measureProtocolTurnList.Contains( protocolName ) )
                {
                    this.measureProtocolTurnList.Add( protocolName );
                }
            }

            // プロトコル番号にホスト設定が無ければ、デフォルト値としてDPR設定を持ってくる。
            var fromDprNameNoList = (from v in measProtocol.UseMeasureProtocolList select new DPRProtocolNameAndHostProtocolNumber( v.ProtocolName, v.ProtocolNo ));
            foreach ( var dprSetting in fromDprNameNoList )
            {
                var dprProtocolName = dprSetting.DPRProtocolName;
                if ( !this.dprNameAndHostNumber.Exists( ( nameAndNumber ) => nameAndNumber.DPRProtocolName == dprProtocolName ) )
                {
                    this.dprNameAndHostNumber.Add( dprSetting );
                }
            }

        }

        /// <summary>
        /// ホスト用プロトコル番号取得
        /// </summary>
        /// <remarks>
        /// プロトコル名称からホスト用プロトコル番号を取得します。
        /// </remarks>
        /// <param name="protocolName">プロトコル名称</param>
        /// <returns>プロトコル番号</returns>
        public Int32 GetHostProtocolNumber( String protocolName )
        {
            Int32 protocolNumber = 0;
            var searched = from v in this.dprNameAndHostNumber
                           where v.DPRProtocolName == protocolName
                           select v.HostProtocolNumber;
            if ( searched.Count() != 0 )
            {
                protocolNumber = searched.First();
            }

            return protocolNumber;
        }

        //        public Int32 GetDPRProtocolNumber( String protocolName
        /// <summary>
        /// 分析項目取得
        /// </summary>
        /// <remarks>
        /// 分析項目を取得します
        /// </remarks>
        /// <param name="hostProtocolNumber"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        public MeasureProtocol GetDPRProtocolFromHostProtocolNumber( Int32 hostProtocolNumber, MeasureProtocolManager manager )
        {
            var searched = from v in this.dprNameAndHostNumber
                           where v.HostProtocolNumber == hostProtocolNumber
                           select manager.GetMeasureProtocolFromName( v.DPRProtocolName );
            MeasureProtocol result = null;
            if ( searched.Count() != 0 )
            {
                result = searched.First();
            }

            return result;
        }

        /// <summary>
        /// 測定順序設定
        /// </summary>
        /// <remarks>
        /// 測定順序を設定します
        /// </remarks>
        /// <param name="protocolName">対象分析項目名称リスト</param>
        /// <returns>測定順序</returns>
        public void SetTurnOrder(List<String> nameList)
        {
            this.measureProtocolTurnList.Clear();
            foreach (String protocolName in nameList)
            {
                this.measureProtocolTurnList.Add(protocolName);
            }
        }
        /// <summary>
        /// 初始化检测顺序列表
        /// </summary>
        public void InitTurnOrder()
        {
            try
            {
                measureProtocolTurnList.Sort(
                delegate(String protocolName1, String protocolName2)
                {
                    if (String.IsNullOrEmpty(protocolName1) || String.IsNullOrEmpty(protocolName2))
                    {
                        return 0;
                    }
                    MeasureProtocol prevProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(protocolName1);
                    MeasureProtocol nextProtocol = Singleton<MeasureProtocolManager>.Instance.GetMeasureProtocolFromName(protocolName2);
                    if (prevProtocol == null || nextProtocol == null)
                    {
                        return 0;
                    }
                    if (prevProtocol.TurnOrder >= nextProtocol.TurnOrder)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                });
            }
            catch (Exception ex)
            {
                Singleton<CarisXLogManager>.Instance.Write(LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID,
                                                                                               CarisXLogInfoBaseExtention.Empty, ex.StackTrace);
            }
        }
        /// <summary>
        /// 測定順序取得
        /// </summary>
        /// <remarks>
        /// 測定順序取得します
        /// </remarks>
        /// <param name="protocolName">対象分析項目名称</param>
        /// <returns>測定順序</returns>
        public Int32 GetTurnOrder( String protocolName )
        {
            Int32 turn = 0;

            // 対象分析項目の測定順序を探す
            if ( this.measureProtocolTurnList.Contains( protocolName ) )
            {
                // 測定順番が1~50のためインデックス + 1の値を測定順序とする
                turn = this.measureProtocolTurnList.FindIndex( ( s ) => s == protocolName ) + 1;
            }

            //// MeasureProtocolManager経由で名前を検索する。
            //MeasureProtocol protocol = manager.GetMeasureProtocolFromProtocolIndex( protocolIndex );
            //if ( measureProtocolTurnList.Contains( protocol.ProtocolName ) )
            //{
            //    turn = this.measureProtocolTurnList.IndexOf( protocol.ProtocolName );
            //}

            return turn;
        }



        #region ISavePath メンバー

        /// <summary>
        /// 保存パス
        /// </summary>
        [XmlIgnore()]
        public String SavePath
        {
            get
            {
                return String.Format( @"{0}\MeasureProtocolInfo.xml", CarisXConst.PathProtocol );
            }
        }

        #endregion

    }
}
