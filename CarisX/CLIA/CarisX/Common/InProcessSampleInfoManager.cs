using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.CarisX.Utility;
using Oelco.CarisX.Const;
using Oelco.CarisX.Parameter;

namespace Oelco.CarisX.Common
{
    /// <summary>
    /// 分析項目識別キーデータクラス
    /// </summary>
    /// <remarks>
    /// ユニーク番号の代替情報です。
    /// この情報は、自動再検の再登録抑止で使用します（ユニーク番号は再検査時に新規発行される為）
    /// </remarks>
    public class MeasDetailKey
    {
        /// <summary>
        /// 検体識別番号
        /// </summary>
        Int32 IndividuallyNumber = 0;
        /// <summary>
        /// 分析項目インデックスAnalysis Index
        /// </summary>
        Int32 MeasProtocolIndex = 0;
        public MeasDetailKey( Int32 indviduallyNumber, Int32 measProtocolIndex )
        {
            this.IndividuallyNumber = indviduallyNumber;
            this.MeasProtocolIndex = measProtocolIndex;
        }

        /// <summary>
        /// 値の比較
        /// </summary>
        /// <remarks>
        ///　指定したオブジェクトの値が同一かどうかを比較します。
        /// </remarks>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object obj )
        {
            MeasDetailKey key = obj as MeasDetailKey  ;
            if ( key != null )
            {
                return this.IndividuallyNumber == key.IndividuallyNumber && this.MeasProtocolIndex == key.MeasProtocolIndex;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// リアルタイム印刷データ種別
    /// </summary>
    public enum RealtimePrintDataType
    {
        /// <summary>
        /// 検体
        /// </summary>
        Specimen,
        /// <summary>
        /// キャリブレータ
        /// </summary>
        Calibrator,
        /// <summary>
        /// 精度管理検体
        /// </summary>
        Control
    }

    /// <summary>
    /// 分析中検体情報管理
    /// </summary>
    /// <remarks>
    /// 現在分析を行っている検体情報を管理します。
    /// 各種コマンドの情報と検体の詳細データ間の中継を行います。
    /// この情報は分析開始時にクリアします。
    /// </remarks>
    public class InProcessSampleInfoManager
    {
        #region [インスタンス変数定義]
        /// <summary>
        /// 再検登録済分析リスト
        /// </summary>
        List<MeasDetailKey> addedReMeasList = new List<MeasDetailKey>();

        /// <summary>
        /// 分析中検体情報
        /// </summary>
        List<SampleInfo> inProcessSampleList = new List<SampleInfo>();

        ///// <summary>
        ///// 分析中検体情報履歴
        ///// </summary>
        //List<SampleInfo> inProcessSampleLogList = new List<SampleInfo>();

        /// <summary>
        /// リアルタイム印刷キュー（検体）(IndividuallyNo,UniqueNo,RepNo)
        /// </summary>
        List<Tuple<Int32, Int32, Int32>> realtimePrintQueueSpecimen = new List<Tuple<Int32, Int32, Int32>>();
        /// <summary>
        /// リアルタイム印刷キュー（キャリブレータ）(IndividuallyNo,UniqueNo,RepNo)
        /// </summary>
        List<Tuple<Int32, Int32, Int32>> realtimePrintQueueCalibrator = new List<Tuple<Int32, Int32, Int32>>();
        /// <summary>
        /// リアルタイム印刷キュー（精度管理検体）(IndividuallyNo,UniqueNo,RepNo)
        /// </summary>
        List<Tuple<Int32, Int32, Int32>> realtimePrintQueueControl = new List<Tuple<Int32,Int32,Int32>>();

        /// <summary>
        /// リアルタイム印刷現表示ページ数(Specimen)
        /// </summary>
        Int32 realtimePrintPageCountSpecimen = 0;
        /// <summary>
        /// リアルタイム印刷現表示ページ数(Calibrator)
        /// </summary>
        Int32 realtimePrintPageCountCalibrator = 0;
        /// <summary>
        /// リアルタイム印刷現表示ページ数(Control)
        /// </summary>
        Int32 realtimePrintPageCountControl = 0;
        #endregion

        #region [プロパティ]

        /// <summary>
        /// リアルタイム印刷キュー（検体）
        /// </summary>
        public Int32 RealtimePrintQueueSpecimenCount
        {
            get
            {
                return this.realtimePrintQueueSpecimen.Count;
            }
        }
        /// <summary>
        /// リアルタイム印刷キュー（キャリブレータ）
        /// </summary>
        public Int32 RealtimePrintQueueCalibratorCount
        {
            get
            {
                return this.realtimePrintQueueCalibrator.Count;
            }
        }
        /// <summary>
        /// リアルタイム印刷キュー（精度管理検体）
        /// </summary>
        public Int32 RealtimePrintQueueControlCount
        {
            get
            {
                return this.realtimePrintQueueControl.Count;
            }
        }
        /// <summary>
        /// 分析中検体情報
        /// </summary>
        public List<SampleInfo> InProcessSampleList
        {
            get
            {
                return this.inProcessSampleList;
            }
        }
        #endregion

        #region [publicメソッド]

        public Int32 GetNextRealtimePrintPageNumber( RealtimePrintDataType type )
        {
            Int32 pageCount = 1;

            // ページ番号の取得及びインクリメントを行う。
            switch ( type )
            {
            case RealtimePrintDataType.Specimen:
                pageCount = ++this.realtimePrintPageCountSpecimen;
                break;
            case RealtimePrintDataType.Calibrator:
                pageCount = ++this.realtimePrintPageCountCalibrator;
                break;
            case RealtimePrintDataType.Control:
                pageCount = ++this.realtimePrintPageCountControl;
                break;
            default:
                break;
            }

            return pageCount;
        }

        /// <summary>
        /// リアルタイム印刷キュー件数問合せ
        /// </summary>
        /// <remarks>
        /// リアルタイム印刷対象データの件数を確認します。
        /// </remarks>
        /// <param name="type">リアルタイム印刷種別</param>
        /// <returns>印刷対象件数</returns>
        public List<Tuple<Int32,Int32,Int32>> AskRealTimePrintQueue(RealtimePrintDataType type )
        {
            // IndividuallyNo,UniqueNo,ReplicationNo,
            List<Tuple<Int32, Int32, Int32>> list = new List<Tuple<int, int, int>>();

            // データの簡易コピーをとる。
            switch( type  )
            {
            case RealtimePrintDataType.Specimen:
                list = new List<Tuple<int, int, int>>( this.realtimePrintQueueSpecimen );
                break;
            case RealtimePrintDataType.Calibrator:
                list = new List<Tuple<int, int, int>>( this.realtimePrintQueueCalibrator );
                break;
            case RealtimePrintDataType.Control:
                list = new List<Tuple<int, int, int>>( this.realtimePrintQueueControl );
                break;
            default:break;

            }

            return list;
            
        }

        /// <summary>
        /// リアルタイム印刷対象データキュー取り出し
        /// </summary>
        /// <remarks>
        /// リアルタイム印刷対象データを取り出します。
        /// </remarks>
        /// <param name="type">データ種別</param>
        /// <param name="count">取り出し件数(0を指定した場合、全件取得となります）</param>
        /// <returns>印刷対象リスト(IndividuallyNo,UniqueNo,RepNo)</returns>
        public List<Tuple<Int32,Int32, Int32>> PopRalTimePrintQueueData( RealtimePrintDataType type, Int32 count = 0)
        {
            List<Tuple<Int32, Int32, Int32>> uniqueRepList = new List<Tuple<Int32, Int32,Int32>>();
            List<Tuple<Int32, Int32, Int32>> uniqueRepListSrc = null;
            Int32 getCount = 0;

            switch ( type )
            {
            case RealtimePrintDataType.Specimen:
                uniqueRepListSrc = this.realtimePrintQueueSpecimen;
                break;

            case RealtimePrintDataType.Calibrator:
                uniqueRepListSrc = this.realtimePrintQueueCalibrator;
                break;

            case RealtimePrintDataType.Control:
                uniqueRepListSrc = this.realtimePrintQueueControl;
                break;

            default:
                break;
            }

            // 印刷キューから取り出す
            if ( ( uniqueRepListSrc != null ) && ( count <= uniqueRepListSrc.Count ) )
            {
                getCount = count == 0 ? uniqueRepListSrc.Count : count;
                uniqueRepList = uniqueRepListSrc.GetRange( 0, getCount );
                uniqueRepListSrc.RemoveRange( 0, getCount );
            }

            return uniqueRepList;
        }


        /// <summary>
        /// 再検登録分析データ設定
        /// </summary>
        /// <remarks>
        /// 再検を行ったデータとして、MeasDetailKeyを設定します。
        /// このデータは分析中状態を終了した場合、クリアされます。
        /// </remarks>
        /// <param name="keyData">再検登録データ</param>
        /// <returns>True:設定完了 False:設定済</returns>
        public Boolean SetAddedReMeasList( MeasDetailKey keyData )
        {
            Boolean added = false;
            if ( !this.addedReMeasList.Contains( keyData ) )
            {
                this.addedReMeasList.Add( keyData );
                added = true;
            }
            return added;
        }

        /// <summary>
        /// 検体情報クリア
        /// </summary>
        /// <remarks>
        /// 保持している検体情報をクリアします。
        /// </remarks>
        public void Clear()
        {
            this.inProcessSampleList.Clear();
            this.addedReMeasList.Clear();

            this.realtimePrintQueueSpecimen.Clear();
            this.realtimePrintQueueControl.Clear();
            this.realtimePrintQueueCalibrator.Clear();
            this.realtimePrintPageCountSpecimen = 0;
            this.realtimePrintPageCountCalibrator = 0;
            this.realtimePrintPageCountControl = 0;
        }

        ///// <summary>
        ///// 分析履歴を消去します
        ///// </summary>
        ///// <remarks>
        ///// この処理は日替わり時に呼び出され、
        ///// 分析DBのクリアと同タイミングで分析履歴を消去します。
        ///// </remarks>
        //public void ClearInProcessLog()
        //{
        //    // 分析履歴消去
        //    this.inProcessSampleLogList.Clear();
        //}

        /// <summary>
        /// 検体情報追加
        /// </summary>
        /// <remarks>
        /// 分析中となる検体の情報を追加します。
        /// </remarks>
        /// <param name="info"></param>
        public void AddSampleInfo( SampleInfo info )
        {
            this.inProcessSampleList.Add( info );
            //this.inProcessSampleLogList.Add( info );
        }


        //public List<SampleInfo> GetInprocessLog()
        //{
        //    return this.inProcessSampleLogList;
        //}

        /// <summary>
        /// 検体情報生成
        /// </summary>
        /// <remarks>
        /// 新規に検体情報データクラスを生成します。
        /// </remarks>
        /// <returns>検体情報</returns>
        public SampleInfo CreateSampleInfo( SampleKind kind )
        {
            var info = new SampleInfo();

            // リアルタイム印刷キュー設定
            switch ( kind )
            {
            case SampleKind.Sample:
            case SampleKind.Priority:
            case SampleKind.Line:
                info.SetRealtimePrintQueue( this.realtimePrintQueueSpecimen );
                break;
            case SampleKind.Calibrator:
                info.SetRealtimePrintQueue( this.realtimePrintQueueCalibrator );
                break;
            case SampleKind.Control:
                info.SetRealtimePrintQueue( this.realtimePrintQueueControl );
                break;
            default:
                break;
            }

            return info;
        }

        /// <summary>
        /// 検体情報追加
        /// </summary>
        /// <remarks>
        /// 分析中となる検体の情報を追加します。
        /// </remarks>
        /// <param name="info"></param>
        public void RemoveSampleInfo( SampleInfo info )
        {
            if ( this.inProcessSampleList.Contains( info ) )
            {
                this.inProcessSampleList.Remove( info );
            }
        }

        /// <summary>
        /// 非エラー検体情報取得
        /// </summary>
        /// <remarks>
        /// 分析を行っている検体情報から、エラーでないものを全て返します。
        /// この関数は分析終了時、登録DBクリアを行う為呼び出されます。
        /// </remarks>
        /// <returns>非エラー検体情報</returns>
        public List<SampleInfo> GetNoErrorSampleInfo( SampleKind kind )
        {
            List<SampleInfo> noErrorList = new List<SampleInfo>();

            // 検索種別でフィルタする。
            var kindSearched = from v in this.inProcessSampleList
                               where v.SampleKind == kind
                               select v;

            // エラー発生を確認
            foreach ( var sampleInfo in kindSearched )
            {
                var registerd = sampleInfo.GetRegisterdProtocols();

                // 登録されている分析項目の全レプリケーションで、エラーが発生しているものを抽出
                var errorProtocolList = registerd.Where( ( v ) =>
                {
                    Boolean errorData = false;
                    for ( int replicationNumber = 1; replicationNumber <= v.Item2; replicationNumber++ )
                    {
                        if ( sampleInfo.GetMeasureProtocolStatusFromProtocolRep( v.Item1, replicationNumber ) == SampleInfo.SampleMeasureStatus.Error )
                        {
                            errorData = true;
                        }
                    }
                    return errorData;
                } );

                // エラーが発生していない検体をリストに追加
                if ( errorProtocolList.Count() == 0 )
                {
                    noErrorList.Add( sampleInfo );
                }
            }

            return noErrorList;
        }

        /// <summary>
        /// 分析中検体検索
        /// </summary>
        /// <remarks>
        /// ラックIDによる分析中検体検索を行います。
        /// </remarks>
        /// <param name="rackId">ラックID</param>
        /// <returns>検索結果</returns>
        public List<SampleInfo> SearchInProcessSampleFromRackId( CarisXIDString rackId )
        {
            // ラックIDで検索
            IEnumerable<SampleInfo> searched = from v in this.inProcessSampleList
                                               where v.RackId.DispPreCharString == rackId.DispPreCharString
                                               select v;
            List<SampleInfo> result = searched.ToList();
            return result;
        }

        /// <summary>
        /// 分析中検体検索
        /// </summary>
        /// <remarks>
        /// ユニーク番号による分析中検体検索を行います。
        /// </remarks>
        /// <param name="individuallyNumber">ユニーク番号</param>
        /// <returns>検索結果</returns>
        public SampleInfo SearchInProcessSampleFromUniqueNo( Int32 uniqueNo )
        {
            var searched = from v in this.inProcessSampleList
                           where v.IsContainUniqueNumber( uniqueNo )
                           select v;

            SampleInfo result = searched.Count() == 0 ? null : searched.First();
            return result;
        }

        /// <summary>
        /// 分析中検体検索
        /// </summary>
        /// <remarks>
        /// 検体識別番号による分析中検体検索を行います。
        /// </remarks>
        /// <param name="individuallyNumber">検体識別番号</param>
        /// <returns>検索結果</returns>
        public List<SampleInfo> SearchInProcessSampleFromIndividuallyNumber( Int32 individuallyNumber )
        {
            // TODO:全ロット影響
            // 検体識別番号番号で検索
            IEnumerable<SampleInfo> searched = from v in this.inProcessSampleList
                                               where v.IndividuallyNumber == individuallyNumber
                                               select v;
            List<SampleInfo> result = searched.Count() == 0 ? null : searched.ToList();
            return result;

        }

        /// <summary>
        /// 検体分析中確認
        /// </summary>
        /// <remarks>
        /// 指定のラックID＆ラックポジション、若しくは検体IDから、
        /// 分析中データを検索し、結果を返します。
        /// いずれかの分析項目が分析中であれば結果はTrueとなります。
        /// この関数は各登録画面等で、対象の検体が分析中かどうか（編集してよいか）を確認する為に呼び出されます。
        /// </remarks>
        /// <returns>True:分析中 False:非分析中</returns>
        public Boolean IsInProcess( CarisXIDString rackId, Int32 rackPos, String sampleId = "" )
        {
            Boolean find = false;

            // 指定の検体が分析中であるか確認する。
            var searched = from v in this.inProcessSampleList
                           where ( ( v.RackId.DispPreCharString == rackId.DispPreCharString && v.RackPos == rackPos ) || ( v.SampleId == sampleId ) )
                           select v;
            find = searched.Count() > 0;

            return find;
        }
        #endregion
    }

    /// <summary>
    /// 検体情報
    /// </summary>
    /// <remarks>
    /// InProcessSampleInfoManagerにて検体情報管理を行うデータクラスです。
    /// このクラスの生成はInProcessSampleInfoManagerからのみ生成を行います。
    /// </remarks>
    public class SampleInfo
    {

        #region [定数定義]

        /// <summary>
        /// 分析項目ステータス
        /// </summary>
        public enum SampleMeasureStatus : int
        {
            /// <summary>
            /// 待機中
            /// </summary>
            Wait = 0,
            /// <summary>
            /// 分析中
            /// </summary>
            InProcess = 1,
            /// <summary>
            /// 終了
            /// </summary>
            End = 2,
            /// <summary>
            /// エラー
            /// </summary>
            Error = 3
        }

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 検体Id
        /// </summary>
        private string sampleId;

        /// <summary>
        /// 検体種別
        /// </summary>
        private SampleKind sampleKind;
        
        /// <summary>
        /// ラックID
        /// </summary>
        private CarisXIDString rackId;

        /// <summary>
        /// ラックポジション
        /// </summary>
        private  Int32 rackPos;

        /// <summary>
        /// 検体識別番号
        /// </summary>
        private Int32 individuallyNumber;

        /// <summary>
        /// シーケンス番号
        /// </summary>
        private Int32 sequenceNumber;

        /// <summary>
        /// 受付番号
        /// </summary>
        private Int32 receiptNumber;

        /// <summary>
        /// コメント
        /// </summary>
        private String comment;

        /// <summary>
        /// モジュールID
        /// </summary>
        private Int32 moduleId;

        /// <summary>
        /// 分析項目状態辞書(分析項目Index→(多重測定Index→分析状態))
        /// </summary>
        private Dictionary<Int32, Dictionary<Int32, SampleMeasureStatus>> measureProtocolIndexStatusDic = new Dictionary<Int32, Dictionary<Int32, SampleMeasureStatus>>();

		/// <summary>
		/// 測定時刻辞書(分析項目Index→(多重測定Index→測定時刻))
		/// </summary>
		private Dictionary<Int32, Dictionary<Int32, DateTime>> measureProtocolIndexMeasureTimeDic = new Dictionary<Int32, Dictionary<Int32, DateTime>>();

        /// <summary>
        /// 分析項目,ユニーク番号対応リスト
        /// </summary>
        private List<Tuple<Int32, Int32>> measureProtocolIndexAndUniqueNoList = new List<Tuple<Int32, Int32>>();

        /// <summary>
        /// 反応槽位置
        /// </summary>
        private Int32 reactorPosition = 0;

        /// <summary>
        /// リアルタイム印刷キュー(individuallyNo,UniqueNo,RepNo)
        /// </summary>
        List<Tuple<Int32, Int32,Int32>> realtimePrintQueue = new List<Tuple<int, int,int>>();

        #endregion

        /// <summary>
        /// リアルタイム印刷キュー設定
        /// </summary>
        /// <remarks>
        /// リアルタイム印刷に使用するキューの設定を行います。
        /// この関数はInProcessSampleInfoManager内部以外では利用しません。
        /// </remarks>
        /// <param name="queue">リアルタイム印刷検体キュー（検体番号、ユニーク番号、レプリケーション番号）individuallyNo , unique number, replication number</param>
        public void SetRealtimePrintQueue( List<Tuple<Int32,Int32, Int32>> queue )
        {
            this.realtimePrintQueue = queue;
        }

        #region [プロパティ]

        /// <summary>
        /// 検体Id
        /// </summary>
        public string SampleId
        {
            get
            {
                return sampleId;
            }
            set
            {
                sampleId = value;
            }
        }

        /// <summary>
        /// 検体種別 取得/設定
        /// </summary>
        public SampleKind SampleKind
        {
            get
            {
                return sampleKind;
            }
            set
            {
                sampleKind = value;
            }
        }

        /// <summary>
        /// ラックID 取得/設定
        /// </summary>
        public CarisXIDString RackId
        {
            get
            {
                return rackId;
            }
            set
            {
                rackId = value;
            }
        }        

        /// <summary>
        /// ラックポジション 取得/設定
        /// </summary>
        public Int32 RackPos
        {
            get
            {
                return rackPos;
            }
            set
            {
                rackPos = value;
            }
        }

        /// <summary>
        /// 検体識別番号 取得/設定
        /// </summary>
        public Int32 IndividuallyNumber
        {
            get
            {
                return individuallyNumber;
            }
            set
            {
                individuallyNumber = value;
            }
        }

        /// <summary>
        /// シーケンス番号
        /// </summary>
        public Int32 SequenceNumber
        {
            get
            {
                return sequenceNumber;
            }
            set
            {
                sequenceNumber = value;
            }
        }

        /// <summary>
        /// 受付番号 設定/取得
        /// </summary>
        public Int32 ReceiptNumber
        {
            get
            {
                return this.receiptNumber;
            }
            set
            {
                this.receiptNumber = value;
            }
        }

        /// <summary>
        /// コメント 設定/取得
        /// </summary>
        public String Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
            }
        }

        /// <summary>
        /// 測定結果がエラーかどうかを取得、設定(計算処理後の結果)
        /// </summary>
        public bool IsError
        {
            get;
            set;
        }

        /// <summary>
        /// 反応槽位置(分析ステータスコマンド内の配列位置)Reaction vessel position (sequence positions in the analysis status command)
        /// </summary>
        public Int32 ReactorPosition
        {
            get
            {
                return reactorPosition;
            }
            set
            {
                reactorPosition = value;
            }
        }

        /// <summary>
        /// モジュールID 設定/取得
        /// </summary>
        public Int32 ModuleID
        {
            get
            {
                return this.moduleId;
            }
            set
            {
                this.moduleId = value;
            }
        }

        /// <summary>
        /// 全分析項目状態辞書(分析項目Index→(多重測定Index→分析状態))
        /// </summary>
        public Dictionary<Int32, Dictionary<Int32, SampleMeasureStatus>> ProtocolStatusDictionary
        {
            get
            {
                return this.measureProtocolIndexStatusDic;
            }
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 登録済分析項目取得
        /// </summary>
        /// <remarks>
        /// この検体情報に登録されている分析項目インデックスを取得します。
        /// </remarks>
        /// <returns>分析項目インデックス,多重測定回数リストAnalysis item index, multiple number of measurements list</returns>
        public List<Tuple<Int32, Int32>> GetRegisterdProtocols()
        {
            var result = from v in this.measureProtocolIndexStatusDic
                         select new
                         Tuple<Int32, Int32>(
                            v.Key,
                             v.Value.Count
                         );

            return result.ToList();
        }

        /// <summary>
        /// 登録済分析項目取得
        /// </summary>
        /// <remarks>
        /// この検体情報に登録されている分析項目インデックスを取得します。
        /// </remarks>
        /// <returns>分析項目インデックス,多重測定回数リスト</returns>
        public List<Int32> GetRegisterdProtocolIndexList()
        {
            var result = from v in this.measureProtocolIndexStatusDic
                         select v.Key;

            return result.ToList();
        }
        /// <summary>
        /// ユニーク番号が存在有無取得
        /// </summary>
        /// <remarks>
        /// ユニーク番号が存在するかどうかを取得します。
        /// </remarks>
        /// <param name="uniqueNo"></param>
        /// <returns></returns>
        public Boolean IsContainUniqueNumber( Int32 uniqueNo )
        {
            var result = from v in this.measureProtocolIndexAndUniqueNoList
                         where v.Item2 == uniqueNo
                         select v.Item1;
            Boolean find = result.Count() != 0;
            return find;
        }

        /// <summary>
        /// ユニーク番号取得
        /// </summary>
        /// <remarks>
        /// ユニーク番号を取得します。
        /// </remarks>
        /// <returns>ユニーク番号取得結果</returns>
        public List<Int32> GetUniqueNumbers()
        {
            var result = from v in this.measureProtocolIndexAndUniqueNoList
                         select v.Item2;
            return result.ToList();
        }

        /// <summary>
        /// 分析項目追加
        /// </summary>
        /// <remarks>
        /// 分析項目の設定を行います。
        /// </remarks>
        /// <param name="measureProtocolIndex">分析項目インデックス</param>
        /// <param name="uniqueNumber">ユニーク番号</param>
        /// <param name="repCount">多重測定回数</param>
        public void AddMeasureProtocol( Int32 measureProtocolIndex, Int32 uniqueNumber, Int32 repCount )
        {
            // 項目追加
            if ( !this.measureProtocolIndexStatusDic.ContainsKey( measureProtocolIndex ) )
            {
                this.measureProtocolIndexStatusDic.Add( measureProtocolIndex, new Dictionary<Int32, SampleMeasureStatus>() );
            }
            Tuple<Int32, Int32> indexAndUnique = new Tuple<Int32, Int32>( measureProtocolIndex, uniqueNumber );
            if ( !this.measureProtocolIndexAndUniqueNoList.Contains( indexAndUnique ) )
            {
                this.measureProtocolIndexAndUniqueNoList.Add( indexAndUnique );
            }

            // 多重測定毎のステータス初期化
            for ( Int32 repIndex = 1; repIndex <= repCount; repIndex++ )
            {
                this.measureProtocolIndexStatusDic[measureProtocolIndex][repIndex] = SampleMeasureStatus.Wait;
            }
        }

        /// <summary>
        /// 分析項目状態取得
        /// </summary>
        /// <remarks>
        /// 分析項目の分析状態をレプリケーション単位で取得します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目Index</param>
        /// <param name="repNo">多重測定インデックス</param>
        public SampleMeasureStatus GetMeasureProtocolStatusFromProtocolRep( Int32 protocolIndex, Int32 repNo )
        {
            SampleMeasureStatus result = SampleMeasureStatus.Error;

            if ( this.measureProtocolIndexStatusDic.ContainsKey( protocolIndex ) )
            {
                if ( this.measureProtocolIndexStatusDic[protocolIndex].ContainsKey( repNo ) )
                {
                    result = this.measureProtocolIndexStatusDic[protocolIndex][repNo];
                }
            }

            return result;
        }

        /// <summary>
        /// 分析項目状態取得
        /// </summary>
        /// <remarks>
        /// 分析項目の分析状態をレプリケーション単位で取得します。
        /// </remarks>
        /// <param name="protocolIndex">ユニーク番号</param>
        /// <param name="repNo">多重測定インデックス</param>
        public SampleMeasureStatus GetMeasureProtocolStatusFromUniqueRep( Int32 uniqueNo, Int32 repNo )
        {
            SampleMeasureStatus result = SampleMeasureStatus.Error;

            // ユニーク番号から分析項目を取得し、その分析項目の測定状態を返す
            var findUnique = from v in this.measureProtocolIndexAndUniqueNoList
                             where v.Item2 == uniqueNo
                             select v;
            if ( findUnique.Count() != 0 )
            {
                Int32 protocolIndex = findUnique.First().Item1;
                if ( this.measureProtocolIndexStatusDic.ContainsKey( protocolIndex ) )
                {
                    if ( this.measureProtocolIndexStatusDic[protocolIndex].ContainsKey( repNo ) )
                    {
                        result = this.measureProtocolIndexStatusDic[protocolIndex][repNo];
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 分析項目状態取得
        /// </summary>
        /// <remarks>
        /// 分析項目の分析状態をレプリケーション単位で取得します。The analysis state of analysis items I get in the replication unit.
        /// </remarks>
        /// <param name="protocolIndex">分析項目Index</param>
        /// <param name="repNo">多重測定インデックス</param>
        public List<SampleMeasureStatus> GetMeasureProtocolStatusList( Int32 protocolIndex )
        {
            List<SampleMeasureStatus> list = new List<SampleMeasureStatus>();
            if ( this.measureProtocolIndexStatusDic.ContainsKey( protocolIndex ) )
            {
                list = this.measureProtocolIndexStatusDic[protocolIndex].Select( ( keyvalue ) => keyvalue.Value ).ToList();
            }

            return list;
        }

		/// <summary>
		/// 測定時刻設定
		/// </summary>
		/// <remarks>
		/// 測定時刻を設定します。
		/// </remarks>
		/// <param name="protocolIndex">分析項目Index</param>
		/// <param name="repNo">多重測定インデックス</param>
		/// <param name="measureTime">測定時刻設定</param>
		public void SetMeasureEndTime( Int32 protocolIndex, Int32 repNo, DateTime measureEndTime )
		{
            // 検体情報がメモリに存在するか確認する。Specimen information I want to ensure that they exist in the memory.
			Boolean isExist = false;
			if ( this.measureProtocolIndexStatusDic.ContainsKey( protocolIndex ) )
			{
				if ( this.measureProtocolIndexStatusDic[protocolIndex].ContainsKey( repNo ) )
				{
					isExist = true;
				}
			}

			// 検体情報がメモリに存在する場合、終了時刻を設定する。
			if ( isExist )
			{
				if ( !this.measureProtocolIndexMeasureTimeDic.ContainsKey( protocolIndex ) )
				{
					this.measureProtocolIndexMeasureTimeDic.Add( protocolIndex, new Dictionary<int,DateTime>());
				}
				if ( !this.measureProtocolIndexMeasureTimeDic[protocolIndex].ContainsKey( repNo )  )
				{
					this.measureProtocolIndexMeasureTimeDic[protocolIndex].Add(repNo, measureEndTime);
				}
				else
				{
					this.measureProtocolIndexMeasureTimeDic[protocolIndex][repNo] = measureEndTime;
				}
			}
		}
		/// <summary>
		/// 測定時刻取得
		/// </summary>
		/// <remarks>
		/// 測定時刻を取得します。
		/// </remarks>
		/// <returns>測定時刻 ( 測定時刻情報が存在しない場合、DateTime.MinValueが返却されます ) </returns>
		/// <param name="protocolIndex">分析項目Index</param>
		/// <param name="repNo">多重測定インデックス</param>
		public DateTime GetMeasureEndTime( Int32 protocolIndex, Int32 repNo)
		{
			// 検体情報がメモリに存在するか確認する。
			DateTime measureEndTime = DateTime.MinValue;
			if ( this.measureProtocolIndexMeasureTimeDic.ContainsKey( protocolIndex ) )
			{
				if ( this.measureProtocolIndexMeasureTimeDic[protocolIndex].ContainsKey( repNo ) )
				{
					measureEndTime = this.measureProtocolIndexMeasureTimeDic[protocolIndex][repNo];
				}
			}

			return measureEndTime;
		}

        /// <summary>
        /// 分析項目状態設定
        /// </summary>
        /// <remarks>
        /// 分析項目の分析状態をレプリケーション単位で設定します。
        /// </remarks>
        /// <param name="protocolIndex">分析項目Index</param>
        /// <param name="repNo">多重測定インデックス</param>
        /// <param name="status">分析項目状態</param>
        public void SetMeasureProtocolStatus( Int32 protocolIndex, Int32 repNo, SampleMeasureStatus status )
        {
            if ( this.measureProtocolIndexStatusDic.ContainsKey( protocolIndex ) )
            {
                if ( this.measureProtocolIndexStatusDic[protocolIndex].ContainsKey( repNo ) )
                {
                    this.measureProtocolIndexStatusDic[protocolIndex][repNo] = status;

                    // 分析終了したデータをリアルタイム印刷向けに記録する。
                    if ( ( status != SampleMeasureStatus.InProcess ) && ( status != SampleMeasureStatus.Wait ) )
                    {
                        var searchUniq = from v in this.measureProtocolIndexAndUniqueNoList
                                         where v.Item1 == protocolIndex
                                         select v.Item2;
                        if ( searchUniq.Count() != 0 )
                        {
                            Tuple<int, int, int> queueData = new Tuple<Int32, Int32, Int32>( this.individuallyNumber, searchUniq.First(), repNo );
                            if ( !this.realtimePrintQueue.Contains( queueData ) )
                            {
                                this.realtimePrintQueue.Add( queueData );
                            }
                        }
                    }

                    if ( status == SampleMeasureStatus.Error )
                    {
                        this.IsError = true;
                    }
                }
            }
        }

        /// <summary>
        /// 待機中確認
        /// </summary>
        /// <remarks>
        /// いずれかにWaitingを含んでいるかを返します。
        /// </remarks>
        /// <returns></returns>
        public Boolean IsWaiting()
        {
            Boolean waiting = false;
            var vala = from v in this.measureProtocolIndexStatusDic.Values
                       from vv in v.Values
                       where vv == SampleMeasureStatus.Wait
                       select vv;
            waiting = vala.Count() != 0;

            return waiting;
        }

        public Boolean isWaitingOrInProcess()
        {
            Boolean isWaitingOrInProcess = false;
            var vala = from v in this.measureProtocolIndexStatusDic.Values
                       from vv in v.Values
                       where vv == SampleMeasureStatus.Wait || vv == SampleMeasureStatus.InProcess
                       select vv;
            isWaitingOrInProcess = vala.Count() != 0;
            return isWaitingOrInProcess;
        }

        /// <summary>
        /// 待機中確認
        /// </summary>
        /// <remarks>
        /// 全てがWaitingであるかを返します。
        /// </remarks>
        /// <returns></returns>
        public Boolean IsAllWaiting()
        {
            Boolean allWaiting = false;
            var vala = from v in this.measureProtocolIndexStatusDic.Values
                       from vv in v.Values
                       where vv != SampleMeasureStatus.Wait
                       select vv;
            allWaiting = vala.Count() == 0;

            return allWaiting;
        }

        /// <summary>
        /// 分析項目状態設定
        /// </summary>
        /// <remarks>
        /// 全分析項目全多重測定に対して一括でステータスを設定します。
        /// </remarks>
        /// <param name="status">分析項目状態</param>
        public void SetMeasureProtocolStatus( SampleMeasureStatus status )
        {
            foreach ( var repList in this.measureProtocolIndexStatusDic.Values )
            {
                for ( int repNo = 1; repNo <= repList.Values.Count; repNo++ )
                {
                    // Error,Endのステータスに対しては変化させない
                    if ( ( repList[repNo] != SampleMeasureStatus.Error ) &&
                        ( repList[repNo] != SampleMeasureStatus.End ) )
                    {
                        repList[repNo] = status;
                    }
                }
            }
            if ( status == SampleMeasureStatus.Error )
            {
                this.IsError = true;
            }
        }

        #endregion
               
        ///// <summary>
        ///// ユニーク番号
        ///// </summary>
        //Int32 uniqueNumber;

        ///// <summary>
        ///// ユニーク番号 取得/設定
        ///// </summary>
        //public Int32 UniqueNumber
        //{
        //    get
        //    {
        //        return uniqueNumber;
        //    }
        //    set
        //    {
        //        uniqueNumber = value;
        //    }
        //}


        //protected class RepStatPair
        //{
        //    public Int32 repNo = 0;
        //    public SampleMeasureStatus status = SampleMeasureStatus.Wait;
        //}

//        /// <summary>
//        /// 分析項目状態取得
//        /// </summary>
//        /// <remarks>
//        /// 検体の分析状態を全て取得します。
//        /// </remarks>
//        /// <param name="protocolIndex">分析項目Index</param>
//        /// <param name="repNo">多重測定インデックス</param>
//        public List<SampleMeasureStatus> GetMeasureProtocolStatusList( Int32 protocolIndex )
//        {
////            Dictionary<Int32, Dictionary<Int32, SampleMeasureStatus>>

//            var allList = ( from v in this.measureProtocolIndexStatusDic
//                            select v ).ToDictionary( v => v.Key, v => v.Value );

//            List<SampleMeasureStatus> list = new List<SampleMeasureStatus>();
//            if ( this.measureProtocolIndexStatusDic.ContainsKey( protocolIndex ) )
//            {
//                list = this.measureProtocolIndexStatusDic[protocolIndex].Select( ( keyvalue ) => keyvalue.Value ).ToList();
//            }

//            return list;
//        }


        ///// <summary>
        ///// 分析項目状態辞書(分析項目Index→多重測定Index,分析状態)
        ///// </summary>
        //private Dictionary<Int32, Tuple<Int32, SampleMeasureStatus>> measureProtocolIndexStatusDic = new Dictionary<Int32, Tuple<Int32, SampleMeasureStatus>();

        ///// <summary>
        ///// 分析項目状態辞書(分析項目Index→多重測定Index,分析状態)
        ///// </summary>
        //public Dictionary<Int32, Tuple<Int32, SampleMeasureStatus>> MeasureProtocolIndexStatusDic
        //{
        //    get
        //    {
        //        return measureProtocolIndexStatusDic;
        //    }
        //    //set
        //    //{
        //    //    measureProtocolIndexStatusDic = value;
        //    //}
        //}

        ///// <summary>
        ///// 分析項目Index
        ///// </summary>
        //private List<Int32> measureProtocolIndexList = new List<Int32>();

        ///// <summary>
        ///// 分析項目Indexリスト
        ///// </summary>
        //public List<Int32> MeasureProtocolIndexList
        //{
        //    get
        //    {
        //        return measureProtocolIndexList;
        //    }
        //    set
        //    {
        //        measureProtocolIndexList = value;
        //    }
        //}
        // 他随時追加
    }
}
