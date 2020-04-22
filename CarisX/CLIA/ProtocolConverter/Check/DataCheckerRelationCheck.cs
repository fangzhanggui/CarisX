using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtocolConverter.File;
using System.Globalization;

namespace ProtocolConverter.Check
{
    /// <summary>
    /// インポート項目関連チェック
    /// </summary>
    class DataCheckerRelationCheck 
    {       
        /// <summary>
        /// プロトコルインデックス
        /// </summary>
        public Int32 ProtocolIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 関連チェック処理メイン
        /// </summary>
        /// <remarks>
        /// 関連チェックのメイン処理です。
        /// </remarks>
        /// <param name="excelData"></param>
        /// <returns></returns>
        public Boolean Check( List<String> excelData )
        {
            Boolean rtn = true;            
         
            // プロトコルインデックスの設定
            this.ProtocolIndex = Convert.ToInt32( excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLINDEX )] );

            //// 重複チェック
            //rtn = this.duplicatCheck();               
            // 大小チェック
            rtn = this.limitCheck( excelData );
            // キャリブレーションタイプチェック／キャリブポイント数チェック
            if (!( this.calibTypeCheck( excelData ) && this.calibPointValueCheck( excelData ) && calibMeasPointOfEachCheck( excelData ) ))
            {
                rtn = false;
            }
        
			// 濃度値昇順チェック　手前までのチェック処理がtureの場合のみチェック処理を実施
			rtn = rtn && this.CalibConcentrationAscendingCheck(excelData);

            return rtn;
        }
        
        /// <summary>
        /// 重複チェック
        /// </summary>
        /// <remarks>
        /// 重複チェック処理を行います。
        /// </remarks>
        /// <param name="excelData">チェック対象データ</param>
        public Boolean  duplicatCheck()
        {
            List<String> rowData ;
            Int32 index;
            Int32 col;

            Boolean rtn = true;

            // 分析項目インデックス重複チェック
            index = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLINDEX );
            rowData = Singleton<ExcelControl>.Instance.GetExcelRowData( index );
            col = 1;
            foreach ( String chkStr in rowData )
            {
                // チェック対象文字のデータの件数が2件以上の場合エラー
                //var cnt = from m in rowData
                //          where m.Contains(chkStr)
                //          select m;
                var cnt = from m in rowData
                          where m.Equals(chkStr)
                          select m;

                if ( cnt.Count() > 1 )
                {
                    // エラーログ追加
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( chkStr, ( index + 1 ).ToString(), Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other2"], chkStr );
                    rtn = false;
                }
                col++;
            }

            // 分析項目番号重複チェック
            index = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLNO );
            rowData = Singleton<ExcelControl>.Instance.GetExcelRowData( index );
            col = 0;
            foreach ( String chkStr in rowData )
            {
                // チェック対象文字のデータの件数が2件以上の場合エラー
                var cnt = from m in rowData
                          where m.Equals(chkStr)
                          select m;

                if ( cnt.Count() > 1 )
                {
                    // 出力対象の分析項目インデックスを取得する
                    String protIdx = Singleton<ExcelControl>.Instance.GetData( col, Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLINDEX ) );
                    // エラーログ追加
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( protIdx, ( index + 1 ).ToString(), Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other2"], chkStr );
                    rtn = false;
                }
                col++;
            }

            // 分析項目名重複チェック
            index = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLNAME );
            rowData = Singleton<ExcelControl>.Instance.GetExcelRowData( index );
            col = 0;
            foreach ( String chkStr in rowData )
            {
                // チェック対象文字のデータの件数が2件以上の場合エラー
                var cnt = from m in rowData              
                          where m.Contains( chkStr )
                          select m;
                
                if ( cnt.Count() > 1 )
                {
                    List<string> strSameWords = new List<string>(cnt);
                    //foreach (string s in cnt)
                    //{

                    //}
                   
                    for (int i = 0; i < cnt.Count()-1;i++)
                    {
                        for (int j = i+1; j < cnt.Count();j++ )
                        {
                            if (strSameWords[i] == strSameWords[j])
                            {
                                // 出力対象の分析項目インデックスを取得する
                                String protIdx = Singleton<ExcelControl>.Instance.GetData(col, Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_PROTOCOLINDEX));
                                // エラーログ追加
                                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList(protIdx, (index + 1).ToString(), Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other2"], chkStr);
                                rtn = false;
                            }
                        }
                    }
                    
                }
                col++;
            }

            return rtn;
        }

        /// <summary>
        /// 大小チェック
        /// </summary>
        /// <remarks>
        ///  関連項目の大小チェックを行います。
        /// </remarks>
        /// <param name="excelData">チェック対象データ</param>
        /// <returns></returns>
        private Boolean  limitCheck( List<String> excelData )
        {
            Double low;
            Double Up;
            String lowStr = String.Empty;
            String UpStr = String.Empty;

            Boolean rtn = true;

            // カウントチェック範囲
            for (int i = 1 ; i < 9 ; i++)
            {
                lowStr = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MIN, i ) )];
                UpStr = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MAX, i ) )];
                low = Double.Parse( String.IsNullOrEmpty( lowStr ) ? "0" : lowStr );
                Up = Double.Parse( String.IsNullOrEmpty( UpStr )? "0" : UpStr );
                if ( low > Up )
                {
                    // エラーログ追加
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                        Singleton<ConvertXmlControl>.Instance.GetRowNo( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MIN, i ) ).ToString() , 
                        Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other14"], low.ToString() );
                    rtn = false;
                } 
            }

            // ダイナミックレンジ
            lowStr = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CONCDYNAMICRANGE_MIN )];
            UpStr = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CONCDYNAMICRANGE_MAX )];
            low = Double.Parse( String.IsNullOrEmpty( lowStr ) ? "0" : lowStr );
            Up = Double.Parse( String.IsNullOrEmpty( UpStr ) ? "0" : UpStr );
            if ( low > Up )
            {
                // エラーログ追加
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                    Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_CONCDYNAMICRANGE_MIN ).ToString(),
                 Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other14"], low.ToString() );
                rtn = false;
            }

            // 自動希釈再検条件
            lowStr = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_AUTODILUTIONRETEST_MIN )];
            UpStr = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_AUTODILUTIONRETEST_MAX )];
            low = Double.Parse( String.IsNullOrEmpty( lowStr ) ? "0" : lowStr );
            Up = Double.Parse( String.IsNullOrEmpty( UpStr ) ? "0" : UpStr );
            if ( low > Up )
            {
                // エラーログ追加
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                    Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_AUTODILUTIONRETEST_MIN ).ToString(), 
                    Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other14"], low.ToString() );
                rtn = false;
            }

            // 自動再検条件
            lowStr = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_AUTORETEST_MIN)];
            UpStr = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_AUTORETEST_MAX)];
            low = Double.Parse(String.IsNullOrEmpty(lowStr) ? "0" : lowStr);
            Up = Double.Parse(String.IsNullOrEmpty(UpStr) ? "0" : UpStr);
            if (low > Up)
            {
                // エラーログ追加
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList(ProtocolIndex.ToString(),
                    Singleton<ConvertXmlControl>.Instance.GetRowNo(ConvertXmlControl.NM_AUTORETEST_MIN).ToString(),
                    Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other14"], low.ToString());
                rtn = false;
            }

            return rtn;
        }

        /// <summary>
        /// キャリブレーションタイプ関連チェック
        /// </summary>
        /// <remarks>
        /// キャリブレーションタイプ関連のチェックを行います。
        /// </remarks>
        private Boolean  calibTypeCheck( List<String> excelData )
        {
            // キャリブレーションタイプをExcelデータより取得
            String calibType = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CALIBTYPE )];
            String data = String.Empty;
            Int32 dataIndex;

            Boolean rtn = true;

            // キャリブレーションタイプが「カットオフタイプ」「抑制率タイプ」の場合
			if (calibType == MeasureProtocol.CalibrationType.CutOff.ValueToString() || calibType == MeasureProtocol.CalibrationType.INH.ValueToString())
            {
                
                // 濃度単位のチェック
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CONCUNIT );
                data = excelData[dataIndex];
				if (calibType == MeasureProtocol.CalibrationType.CutOff.ValueToString())
                {
                    if ( data != Const.UNIT_COI )
                    {
                        // 「カットオフタイプ」で、濃度単位が「COI」以外はエラー
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                            Singleton<ConvertXmlControl>.Instance.GetRowNo ( ConvertXmlControl.NM_CONCUNIT )
                            , Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                        rtn = false;
                    }
                }
				else if (calibType == MeasureProtocol.CalibrationType.INH.ValueToString())
                {
                    //凯瑞新需求,INH method 可以同时使用INH%和COI两种单位。
                    if ( data != Const.UNIT_PERCENT && data != Const.UNIT_COI )
                    {
                        // 「抑制率タイプ」で、濃度単位が「%」以外はエラー
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                            Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_CONCUNIT ),
                            Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                        rtn = false;
                    }
                }

                // カウントチェック範囲P3～P8に値が設定されていたらエラー                
                for ( int i = 3; i <= Const.MAX_COUNT_RANGES; i++ )
                {
                    // 上限
                    dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MAX, i ) );
                    data = excelData[dataIndex];
                    if ( data != String.Empty && data != "0" )
                    {
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                            Singleton<ConvertXmlControl>.Instance.GetRowNo( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MAX, i ) ),
                            Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                        rtn = false;
                    }

                    // 下限
                    dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MIN, i ) );
                    data = excelData[dataIndex];
                    if ( data != String.Empty && data != "0" )
                    {
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                            Singleton<ConvertXmlControl>.Instance.GetRowNo( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MIN, i ) ),
                            Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                        rtn = false;
                    }
                }
      
                // 係数A
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_COEF_A );
                data = excelData[dataIndex];

				if (calibType == MeasureProtocol.CalibrationType.CutOff.ValueToString())
                {
                    //if ( data != "0" )
                    //{
                    //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                    //        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_COEF_A )
                    //        , Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    //    rtn = false;
                    //}
                }
				else if (calibType == MeasureProtocol.CalibrationType.INH.ValueToString())
                {
                    //if ( data != "1" )
                    //{
                    //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                    //        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_COEF_A ),
                    //        Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    //    rtn = false;
                    //}
                }

                // 係数B
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_COEF_B );
                data = excelData[dataIndex];

				if (calibType == MeasureProtocol.CalibrationType.CutOff.ValueToString())
                {
                    //if ( data != "1" )
                    //{
                    //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                    //        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_COEF_B )
                    //        , Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    //    rtn = false;
                    //}
                }
				else if (calibType == MeasureProtocol.CalibrationType.INH.ValueToString())
                {
                    //if ( data != "-1" )
                    //{
                    //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                    //        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_COEF_B )
                    //        , Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    //    rtn = false;
                    //}
                }

                // 係数C
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_COEF_C );
                data = excelData[dataIndex];

                if (calibType == MeasureProtocol.CalibrationType.CutOff.ValueToString())
                {
                    //if ( data != "0" )
                    //{
                    //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                    //        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_COEF_C )
                    //        , Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    //    rtn = false;
                    //}
                }
				else if (calibType == MeasureProtocol.CalibrationType.INH.ValueToString())
                {
                    //if ( data != "1" )
                    //{
                    //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                    //        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_COEF_C ),
                    //        Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    //    rtn = false;
                    //}
                }

                // 係数D
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_COEF_D );
                data = excelData[dataIndex];

                if (calibType == MeasureProtocol.CalibrationType.CutOff.ValueToString())
                {
                    //if ( data != "1" )
                    //{
                    //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                    //        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_COEF_D ),
                    //        Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    //    rtn = false;
                    //}
                }
				else if (calibType == MeasureProtocol.CalibrationType.INH.ValueToString())
                {
                    //if ( data != "-1" )
                    //{
                    //    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                    //        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_COEF_D ),
                    //        Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    //    rtn = false;
                    //}
                }

                // 係数E(チェック処理なし)
               
                // キャリブレーション方法
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CALIBMETHOD );
                data = excelData[dataIndex];
                if ( data != string.Empty && data != "0" )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_CALIBMETHOD ),
                        Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    rtn = false;
                }

                // 濃度に値が設定されていたらエラー
                for ( int i = 1; i < Const.MAX_CONCS; i++ )
                {
                    dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CONCSOFEACH, i ) );
                    data = excelData[dataIndex];
                    if ( data != String.Empty )
                    {
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                            Singleton<ConvertXmlControl>.Instance.GetRowNo( String.Format( ConvertXmlControl.NM_CONCSOFEACH, i ) ),
                            Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                        rtn = false;
                    }
                }   
            }
            // キャリブレーションタイプが「カットオフ」「抑制率」以外の場合
            else
            {
                // 係数A～E
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_COEF_A );
                for ( int i = dataIndex; i < dataIndex + 5; i++ )
                {
                    data = excelData[i];
                    if ( data != string.Empty && data != "0" )
                    {
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), (i + 1).ToString() , Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                        rtn = false;
                    }
                }

                // 陽性判定閾値
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_POSILINE );
                data = excelData[dataIndex];
                if ( data != string.Empty && data != "0" )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_POSILINE ),
                        Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    rtn = false;
                }

                // 陰性判定閾値
                dataIndex = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_NEGALINE );
                data = excelData[dataIndex];
                if ( data != string.Empty && data != "0" )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(), 
                        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_NEGALINE )
                        , Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    rtn = false;
                }
            }

            return rtn;
        }

        /// <summary>
        /// キャリブレーションポイント数チェック
        /// </summary>
        /// <remarks>
        /// キャリブレーションポイント数のチェックを行います。
        /// </remarks>
        private bool calibPointValueCheck( List<String> excelData )
        {
            // キャリブレーションタイプをExcelデータより取得
            String calibType = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CALIBTYPE )];

            // キャリブレーションタイプが「カットオフタイプ」「抑制率タイプ」の場合
			if (calibType == MeasureProtocol.CalibrationType.CutOff.ValueToString() || calibType == MeasureProtocol.CalibrationType.INH.ValueToString())            
            {
                Int32 rowNo = Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_NUMOFMEASPOINTINCALIB );
                String data = excelData[rowNo];
                if (data !="2")
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                        Singleton<ConvertXmlControl>.Instance.GetRowNo( ConvertXmlControl.NM_NUMOFMEASPOINTINCALIB ).ToString(), 
                        Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// キャリブレーションポイント関連チェック
        /// </summary>
        /// <remarks>
        /// キャリブレーションポイント関連のチェックを行います。
        /// </remarks>
        /// <param name="data">チェック対象文字列</param>
        /// <param name="pointIndex">チェック対象ポイント数</param>
        /// <param name="pointCount">キャリブレーションポイント数</param>
        /// <returns></returns>
        private Boolean CalibPointCheck( String data, Int32 pointIndex , Int32 pointCount )
        {
            Boolean rtn = true;

            //ポイント外の項目は空白のみ設定可能
            if ( pointIndex > pointCount )
            {
                if ( data != string.Empty )
                {
                    //エラー
                    rtn = false;
                }
            }           
            return rtn;
        } 

        /// <summary>
        /// 測定ポイントチェック
        /// </summary>
        /// <remarks>
        /// 測定ポイントのチェックを行います。
        /// </remarks>
        /// <param name="excelData">チェック対象データ</param>
        private Boolean  calibMeasPointOfEachCheck( List<String> excelData )
        {
            Boolean rtn = true;
            // キャリブレーションタイプをExcelデータより取得
            String calibType = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CALIBTYPE )];
            String calibMethod = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CALIBMETHOD )];
            String data;

            // キャリブレーションタイプが「カットオフタイプ」「抑制率タイプ」の場合
			if (calibType == MeasureProtocol.CalibrationType.CutOff.ValueToString() || calibType == MeasureProtocol.CalibrationType.INH.ValueToString())
            {               
                for ( int i = 1; i <= Const.MAX_CALIB_MEAS_POINT; i++ )
                {                  
                    // 測定ポイントは2ポイント目まで「測定」で、以降は「補正」以外はエラー
                    data = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) )];
                    if ( i == 1 || i == 2 )
                    {
                        // 1ポイント目、2ポイント目は1以外エラー                    
                        if ( data != String.Empty && data != "1" )
                        {
                            Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                            Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) ).ToString(), 
                            Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                            rtn = false;
                        }
                        else
                        {
                            excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) )] = "1";
                        }
                    }
                    else
                    {
                        // ポイント目以降はゼロ以外エラー                    
                        if ( data != String.Empty && data != "0" )
                        {
                            Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                                Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) ).ToString(), 
                                Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                            rtn = false;
                        }
                        else
                        {
                            excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) )] = "0";
                        }
                    }
                }
            }
            // キャリブレーションタイプが「カットオフタイプ」「抑制率タイプ」以外の場合
            else
            {
                // キャリブレーションポイント数を取得
                Int32 pointCnt = Convert.ToInt32(excelData[Convert.ToInt32( Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_NUMOFMEASPOINTINCALIB ) )]);

                // キャリブレーション方法が「マスターキャリブ」の場合
				if (calibMethod == Convert.ToInt32(MeasureProtocol.CalibrationMethod.MasterCalibration).ToString())
                {
                    // 測定ポイントが「測定」の数を確認する
                    int checkCnt = 0;                    
                    for ( int i = 1; i <= Const.MAX_CALIB_MEAS_POINT; i++ )
                    {
                        data = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) )];
                        if ( Convert.ToBoolean( Convert.ToInt32(data) ) )
                        {
                            checkCnt++;
                        }
                        // キャリブレーションポイント数と「測定」の数が一致しないとエラー
                        if ( pointCnt < checkCnt )
                        {
                            Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                                Singleton<ConvertXmlControl>.Instance.GetRowNo( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) ).ToString(), 
                                Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other5"], pointCnt.ToString() );
                            rtn = false;
                        }
                    }
                    
                }
                // キャリブレーション方法が「フルキャリブ」の場合
				else if (calibMethod == Convert.ToInt32(MeasureProtocol.CalibrationMethod.FullCalibration).ToString())
                {               
                    // 上から順にキャリブレーションポイント数分「測定」、以降が「補正」でないとエラー
                    for (int i = 1 ; i <= Const.MAX_CALIB_MEAS_POINT ; i++)
                    {
                        Int32 rowNo = Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) );
                        data = excelData[rowNo];
                        if ( i <= pointCnt && data != "1" )
                        {                          
                            Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                                Singleton<ConvertXmlControl>.Instance.GetRowNo( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) ).ToString(), 
                                Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                            rtn = false;                           
                        }
                        else if ( i > pointCnt && data !="0")
                        {                              
                            Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( ProtocolIndex.ToString(),
                                Singleton<ConvertXmlControl>.Instance.GetRowNo( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) ).ToString(), 
                                Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other4"], data );
                            rtn = false;                            
                        }                        
                    }
                }
            }

            return rtn;            
            
        }

		/// <summary>
		/// キャリブレータ濃度値昇順チェック
		/// </summary>
		/// <returns></returns>
		private bool CalibConcentrationAscendingCheck(List<String> excelData)
		{
            //if calibrationType  is CutOFF or INH ,return true
            MeasureProtocol.CalibrationType calibrationType = (MeasureProtocol.CalibrationType)Convert.ToInt32(excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_CALIBTYPE)]);
            if (calibrationType == MeasureProtocol.CalibrationType.CutOff || calibrationType == MeasureProtocol.CalibrationType.INH)
            {
                return true;
            }

			// キャリブレーション方法を取得する
			MeasureProtocol.CalibrationMethod calibMethod = 
				(MeasureProtocol.CalibrationMethod)Convert.ToInt32(excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_CALIBMETHOD)]);

			// キャリブレータ濃度値で活性の項目の値をリストに格納する。
			List<double> checkConcentrations = new List<double>();
			if (calibMethod == MeasureProtocol.CalibrationMethod.FullCalibration)
			{
				// キャリブレーションポイント数を取得
				Int32 pointCnt = Convert.ToInt32(excelData[Convert.ToInt32(Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_NUMOFMEASPOINTINCALIB))]);

				// フルキャリブレーションのときは、キャリブレーションポイント数までの濃度をリストに入れる
				for (int i = 1; i <= pointCnt ; i++)
				{
					String conc = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex(String.Format(ConvertXmlControl.NM_CONCSOFEACH, i))];
					checkConcentrations.Add(Convert.ToDouble(String.IsNullOrEmpty(conc) ? "0" : conc));
				} 
			}
			else
			{
				// マスタキャリブレーションのときは測定ポイントが1になっている項目だけリストに積む
				for (int i = 1; i <= Const.MAX_CONCS; i++)
				{
					String point = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex(String.Format(ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i))];
					if (point == "1")
					{
						String conc = excelData[Singleton<ConvertXmlControl>.Instance.GetListIndex(String.Format(ConvertXmlControl.NM_CONCSOFEACH, i))];
						//checkConcentrations.Add(Convert.ToInt32(String.IsNullOrEmpty(conc) ? "0" : conc));
                        checkConcentrations.Add(Convert.ToDouble(String.IsNullOrEmpty(conc) ? "0" : conc));
					}					
				}                 
			}

			bool askCheckErr = false;

			// 重複値が存在する場合はエラー
            if (checkConcentrations.Count() != checkConcentrations.Distinct().Count())
            {
                askCheckErr = true;
            }
            else
			{
				//　キャリブレータ濃度値を昇順で並び替えた比較用のリストを作成する。
				List<double> compareConcentrations = checkConcentrations.OrderBy(x => x).ToList();

				// 昇順チェック処理実施
				// 濃度値をソートしたリストと、比較対象の順番が違っていればエラーにする
				for (int i = 0; i < checkConcentrations.Count(); i++)
				{
					if (checkConcentrations[i] != compareConcentrations[i])
					{
						askCheckErr = true;
						break;
					}
				}
			}

			if (askCheckErr)
			{
				//エラーログ出力
				Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList(ProtocolIndex.ToString(),
								Singleton<ConvertXmlControl>.Instance.GetRowNo(String.Format(ConvertXmlControl.NM_CONCSOFEACH, 1)).ToString(),
								Singleton<ConvertXmlControl>.Instance.ErrorNoList["Other15"], checkConcentrations[1].ToString());

				return false;
			}
			
			return true;
		}
    }
}
