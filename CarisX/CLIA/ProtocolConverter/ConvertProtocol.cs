using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtocolConverter.Check;
using ProtocolConverter.File;

namespace ProtocolConverter
{
    class ConvertProtocol
    {
        #region [コンストラクタ/デストラクタ]
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConvertProtocol()
        {
            // 初期処理
            this.initialize();
        }
        #endregion

        #region [publicメソッド]
        /// <summary>
        /// プロトコルコンバータ実行処理
        /// </summary>
        /// <remarks>
        /// プロトコルコンバータ処理を実行します。
        /// </remarks>
        /// <returns></returns>
        public Boolean Execute( Dictionary<String, String> args )
        {
            Boolean rtn = true;
            try
            {

                String sourceFile = args["Excel"];
                String destFile = System.IO.Path.GetTempPath() + System.IO.Path.GetFileName(sourceFile);
                XmlCipher decryptExcel = new XmlCipher(Const.ENCRYPT_PASSWORD);
                decryptExcel.DecryptExcelFile(sourceFile, destFile);
                args["Excel"] = destFile;

                // Excelデータの読込
                Boolean open = Singleton<ExcelControl>.Instance.SetExcelBuff( args["Excel"], Singleton<ConvertXmlControl>.Instance.SettingStartColumn );
                if ( open == false )
                {
                    Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal1"],
                        "Excel file data acquisition failed.");
                    return false;
                }

                // 重複チェック
                if ( !Singleton<DataCheckerRelationCheck>.Instance.duplicatCheck() )
                {
                    return false;
                }

                //删除临时的EXCEL文件 add by marxsu
                System.IO.File.Delete(args["Excel"]);


                // データチェック & 出力処理
                foreach ( var data in Singleton<ExcelControl>.Instance.ExcelData )
                {
                    // XML定義に基づくチェック処理、相関チェック共にチェックOKの場合はXML出力処理
                    if ( this.ConditionCheck( data ) && Singleton<DataCheckerRelationCheck>.Instance.Check( data ) )                    
                    {                        
                        // 出力内容編集処理
                        Int32 index = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLINDEX )] );                       
                        ParameterFilePreserve<MeasureProtocol> protocol = Singleton<MeasureProtocolManager>.Instance.AddMeasureProtocol( index );
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.ExportProtocolIndex.Add( index );
                        if ( protocol != null )
                        {
                            this.EditOutputData( protocol, data );
                        }                          
                    }
                }

                // XMLを保存
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.ExportCount = Singleton<MeasureProtocolManager>.Instance.SaveAllMeasureProtocol( args[Const.FOLDER] );
            }
            catch(Exception ex)
            {
                // エラーログ出力
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace ); 
                rtn = false;
            }

            return rtn;
            
        }
        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 出力データ編集
        /// </summary>
        /// <remarks>
        /// 出力データを編集します。
        /// </remarks
        /// <param name="protocol"></param>
        /// <returns></returns>
        private Boolean EditOutputData( ParameterFilePreserve<MeasureProtocol> protocol, List<String> data )
        {
            Boolean rtn = true ;
            
            // 検体多重測定回数
            protocol.Param.RepNoForSample = 1;//default Setting

            // 精度管理検体多重測定回数        
            protocol.Param.RepNoForControl = 1;//default Setting

            // キャリブレータ多重測定回数        
            protocol.Param.RepNoForCalib = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_REPNOFORCALIB )] );

            // 検量線有効期間        
            protocol.Param.ValidityOfCurve = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_VALIDITYOFCURVE )] );

            // 陽性判定閾値        
            protocol.Param.PosiLine = Convert.ToDouble( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_POSILINE )] );

            // 陰性判定閾値        
            protocol.Param.NegaLine = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_NEGALINE)]);

            // 分析項目番号        
            protocol.Param.ProtocolNo = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLNO )] );

            // 分析項目インデックス        
            protocol.Param.ProtocolIndex = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLINDEX )] );

            // 分析項目名称        
            protocol.Param.ProtocolName = data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLNAME )];

            // 試薬名称        
            protocol.Param.ReagentName = data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_REAGENTNAME )];

            // アッセイシーケンス 　
            protocol.Param.AssaySequence = (MeasureProtocol.AssaySequenceKind)Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_ASSAYSEQ )] );
			
            // 前処理シーケンス    
			protocol.Param.PreProcessSequence = (MeasureProtocol.PreProcessSequenceKind)Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_PREPROCESS_SEQ)]);
			
            // サンプル種別  
			protocol.Param.SampleKind = (MeasureProtocol.SampleTypeKind)Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_SAMPLEKIND)]);
			
            // 自動希釈再検使用有無        
            protocol.Param.UseAfterDil = Convert.ToBoolean( Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_USEAFTERDIL )] ) );

            //  自動再検使用有無        
            protocol.Param.UseAutoReTest = Convert.ToBoolean( Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_USEAUTORETEST )] ) );

            // 自動希釈再検条件           
            protocol.Param.AutoDilutionReTest.Max = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_AUTODILUTIONRETEST_MAX )] );
            protocol.Param.AutoDilutionReTest.Min  = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_AUTODILUTIONRETEST_MIN )] );

            // 自動希釈再検条件(希釈倍率) 
			protocol.Param.AutoDilutionReTestRatio = (MeasureProtocol.AutoDilutionReTestRatioKind)Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_AUTODILUTIONRETESTRATIO)]);

            //  自動再検条件          
            protocol.Param.AutoReTest.Max = Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_AUTORETEST_MAX)]);
            protocol.Param.AutoReTest.Min = Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_AUTORETEST_MIN)]);

            // 手希釈使用有無  
            protocol.Param.UseManualDil = Convert.ToBoolean( Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_USEMANUALDIL )] ) );

            // 試薬コード        
            protocol.Param.ReagentCode = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_REAGENTCODE )] );

            // サンプル分注量        
            protocol.Param.SmpDispenseVolume = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_SMP_DISPENSE_VOL )] );

            // M試薬分注量        
            protocol.Param.MReagDispenseVolume = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_MREAGDISPENSEVOLUME )] );

            // R1試薬分注量        
            protocol.Param.R1DispenseVolume = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_R1DISPENSEVOLUME )] );

            // R2試薬分注量        
            protocol.Param.R2DispenseVolume = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_R2DISPENSEVOLUME )] );

            // 前処理液1分注量        
            protocol.Param.PreProsess1DispenseVolume = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PREPROSESS_1_DISPENSEVOLUME )] );

            // 前処理液2分注量        
            protocol.Param.PreProsess2DispenseVolume = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PREPROSESS_2_DISPENSEVOLUME )] );

            // キャリブレーションタイプ    
			protocol.Param.CalibType = (MeasureProtocol.CalibrationType)Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_CALIBTYPE)]);

            // 相関係数A        
            protocol.Param.GainOfCorrelation = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_COEFAOFLOG)]);

            // 相関係数B        
            protocol.Param.OffsetOfCorrelation = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_COEFBOFLOG)]);

            //【IssuesNo:1】质控品相关系数A赋值
            protocol.Param.ControlGainOfCorrelation = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_CONTROL_COEFAOFLOG)]);

            //【IssuesNo:1】质控品相关系数B赋值
            protocol.Param.ControlOffsetOfCorrelation = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_CONTROL_COEFBOFLOG)]);

            // キャリブレーション方法   
            protocol.Param.CalibMethod = (MeasureProtocol.CalibrationMethod)Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_CALIBMETHOD)]);
			
            // キャリブレーションポイント数        
            protocol.Param.NumOfMeasPointInCalib = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_NUMOFMEASPOINTINCALIB )] );

            // 濃度
            for ( int i = 1; i <= Const.MAX_CONCS; i++ )
            {
                String conc = data[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CONCSOFEACH, i ) )];
                protocol.Param.ConcsOfEach[i - 1] = Convert.ToDouble(String.IsNullOrEmpty(conc) ? "0" : conc);
            }                       

            // 測定ポイント  
            for ( int i = 1; i <= Const.MAX_CALIB_MEAS_POINT; i++ )
            {
                String point = data[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i ) )];
                protocol.Param.CalibMeasPointOfEach[i - 1] = Convert.ToBoolean( Convert.ToInt32( String.IsNullOrEmpty( point ) ? "0" : point ) );
            }            

            // カウントチェック範囲        
            for ( int i = 1; i <= Const.MAX_COUNT_RANGES; i++ )
            {
				protocol.Param.CountRangesOfEach[i - 1] = new MeasureProtocol.ItemRange();
                String max = data[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MAX, i ) )];
                String min = data[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MIN, i ) )];
                protocol.Param.CountRangesOfEach[i-1].Max = Convert.ToInt32( String.IsNullOrEmpty(max) ? "0" : max );
                protocol.Param.CountRangesOfEach[i - 1].Min = Convert.ToInt32( String.IsNullOrEmpty( min ) ? "0" : min );
            }                       

            // 濃度単位        
            protocol.Param.ConcUnit = data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CONCUNIT )];

            // 濃度値小数点以下桁数        
            protocol.Param.LengthAfterDemPoint = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_LENGTHAFTERDEMPOINT )] );

            // 濃度ダイナミックレンジ     
            protocol.Param.ConcDynamicRange.Max = Convert.ToDouble( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_CONCDYNAMICRANGE_MAX )] );
            protocol.Param.ConcDynamicRange.Min = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_CONCDYNAMICRANGE_MIN)]);

            // 多重測定内乖離限界CV%        
            protocol.Param.MulMeasDevLimitCV = Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_MULMEASDEVLIMITCV )] );

            // 分析項目使用フラグ（読込対象プロトコルはTrueにする）        
            protocol.Param.DisplayProtocol = Convert.ToBoolean(Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_DISPLAYPROTOCOL)]));

            // 自動希釈倍率演算可否        
            protocol.Param.UseAfterDilAtCalcu = Convert.ToBoolean( Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_USEAFTERDILATCALCU )]) );

            // 手希釈倍率演算可否        
            protocol.Param.UseManualDilAtCalcu = Convert.ToBoolean( Convert.ToInt32 ( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_USEMANUALDILATCALCU )] ));

            // 係数A         
            protocol.Param.Coef_A = Convert.ToDouble( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_COEF_A )] );

            // 係数B        
            protocol.Param.Coef_B = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_COEF_B)]);

            // 係数C        
            protocol.Param.Coef_C = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_COEF_C)]);

            // 係数D        
            protocol.Param.Coef_D = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_COEF_D)]);

            // 係数E        
            protocol.Param.Coef_E = Convert.ToDouble(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_COEF_E)]);

			// 試薬開封後有効期限
			protocol.Param.DayOfReagentValid = Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_DAY_OF_REAGENT_VALID)]);

            protocol.Param.RetestRange.UseLow = Convert.ToBoolean( Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_RETESTRANGE_LOW )] ) );
            protocol.Param.RetestRange.UseMiddle = Convert.ToBoolean(Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_RETESTRANGE_MIDDLE)]));
            protocol.Param.RetestRange.UseHigh = Convert.ToBoolean(Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_RETESTRANGE_HIGH)]));
            
            // 独立CV的实现
            protocol.Param.UseCVIndependence = Convert.ToBoolean(Convert.ToInt32( data[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_USECVINDEPENDENCE )]));
            for ( int i = 1; i <= Const.MAX_CONCS; i++ )
            {
                String cv = data[Singleton<ConvertXmlControl>.Instance.GetListIndex( String.Format( ConvertXmlControl.NM_CVOFEACHPOINT, i ) )];
                protocol.Param.CVofEachPoint[i - 1] = Convert.ToDouble(String.IsNullOrEmpty(cv) ? "0" : cv);
            } 

            // 4参数加权K值
            protocol.Param.FourPrameterMethodKValue = Convert.ToInt32(Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_FOURPRAMETERMETHODKVALUE)]));

            // 4参数加权类型
            protocol.Param.FourPrameterMethodType = Convert.ToInt32(Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_FOURPRAMETERMETHODTYPE)]));

            // 对校准品或质控品进行稀释控制选项
            protocol.Param.DiluCalibOrControl = Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_DILUCALIBORCONTROL)]);
             
            // 是否是IGRA项目
            protocol.Param.IsIGRA = Convert.ToBoolean(Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_ISIGRA)]));

            // 检测顺序
            protocol.Param.TurnOrder = Convert.ToInt32(data[Singleton<ConvertXmlControl>.Instance.GetListIndex(ConvertXmlControl.NM_TURNORDER)]);
            
            return rtn;
        }

        /// <summary>
        /// 単項目チェック処理
        /// </summary>
        /// <remarks>
        /// 単項目チェック定義に基づくチェック処理を行います。
        /// </remarks>
        /// <param name="chkData">チェック対象データ</param>
        /// <returns></returns>
        private Boolean ConditionCheck( List<String> chkData )
        {
            Boolean rtn = true ; 
 
            // 単項目の型チェック
            foreach ( var chkitem in Singleton<ConvertXmlControl>.Instance.ConvertList )
            {
                // Excel読込データよりチェック対象データを取得する
                String value = chkData[chkitem.RowNo - 1];

                // 定義されているチェックを行う
                foreach ( var list in chkitem.Validation )
                {
                    ResultInfo result = list.Check( value );
                    if ( !result.Result )
                    {
                        // エラーログ出力
                        Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddErrorList( 
                            chkData[Singleton<ConvertXmlControl>.Instance.GetListIndex( ConvertXmlControl.NM_PROTOCOLINDEX )],
                            chkitem.RowNo.ToString(), result.Data, value );                        
                        rtn = false;
                        break;
                    }
                }
            }
            return rtn;
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        /// <remarks>
        /// 初期処理を行います。
        /// </remarks
        private void initialize()
        {
            //// EXCEL取得処理に必要な情報を設定する
            //// ※将来的にはXMLより取得する予定の部分をPG内で設定する

            // 読込開始列番号
            Singleton<ConvertXmlControl>.Instance.SettingStartColumn = 3;   // TODO:仮の開始列になっている。Excelのフォーマットが確定すれば対応する事

            // エラー情報
            for ( int i = 1; i <= 15; i++ )
            {                
                Singleton<ConvertXmlControl>.Instance.ErrorNoList.Add( String.Format( "Other{0}", i ), ( 7 + i ).ToString() );
            }
            for ( int i = 1; i <= 4; i++ )
            {
                Singleton<ConvertXmlControl>.Instance.ErrorNoList.Add( String.Format( "Internal{0}", i ), ( 3 + i ).ToString() );
            }

			// 例外エラー
            Singleton<ConvertXmlControl>.Instance.ErrorNoList.Add( String.Format( "Exception" ), "99" );

            // 読込定義設定
            this.initializeConvertList();

        }

        /// <summary>
        /// 読込定義設定
        /// </summary>
        /// <remarks>
        /// 単項目チェック処理の定義を設定します。
        /// </remarks
        public void initializeConvertList()
        {            
            ConvertList conv;
            TypeRequiredChecker reqChkInfo;
            TypeNumberChecker numChkInfo;
            TypeRangeChecker rangeChkInfo;
            TypeSmalldigitsChecker dgtChkInfo;
            TypeLengthChecker lenChkInfo;
            TypeBoolChecker boolChkInfo;

            const String reqErrString = "3";
            const String numErrString = "2";
            const String rngErrString = "1";
            const String dgtErrString = "6";
            const String boolErrString = "6";
            
            // プロトコルコンバーターフォーマット.xlsの行数カウンタ
            // 先頭項目の行数を初期値とする
            int iRowNoCnt = 2;

            // 2.分析項目インデックス
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_PROTOCOLINDEX ;
            conv.RowNo = iRowNoCnt; // ※先頭行はインクリメント禁止

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 1;
            rangeChkInfo.MaxValue = 200;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add( rangeChkInfo );

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 3.分析項目番号
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_PROTOCOLNO ;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 1;
            rangeChkInfo.MaxValue =200;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add( rangeChkInfo );

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 4.分析項目名称
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_PROTOCOLNAME ;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            lenChkInfo = new TypeLengthChecker();
            lenChkInfo.MaxLength = 11;//just for test dongzhang
            conv.Validation.Add( lenChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 5.試薬コードReagent code
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_REAGENTCODE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 200;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add( rangeChkInfo );

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 6.試薬名称
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_REAGENTNAME;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            lenChkInfo = new TypeLengthChecker();
            lenChkInfo.MaxLength = 11;//just for test dongzhang
            conv.Validation.Add( lenChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 7.キャリブレータ多重測定回数
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_REPNOFORCALIB;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 1;
            rangeChkInfo.MaxValue = 5;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add( rangeChkInfo );

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );           

			// 8.アッセイシーケンス
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_ASSAYSEQ;
            conv.RowNo = ++iRowNoCnt;
            
            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 1;
            rangeChkInfo.MaxValue = 9;//add dil8000 2step
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 9前処理シーケンス
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_PREPROCESS_SEQ;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 5;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 10サンプル種別Sample type
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_SAMPLEKIND;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 1;
            rangeChkInfo.MaxValue = 4;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 11 サンプル分注量 （μL)
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_SMP_DISPENSE_VOL;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			//12 M試薬分注量 （μL)
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_MREAGDISPENSEVOLUME;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 13.R1試薬分注量 （μL)
            conv = new ConvertList();
            conv.Name =ConvertXmlControl.NM_R1DISPENSEVOLUME;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            //14. R2試薬分注量 （μL)
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_R2DISPENSEVOLUME;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 25;
            rangeChkInfo.MaxValue = 999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 15.前処理液1分注量 （μL)
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_PREPROSESS_1_DISPENSEVOLUME;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 16.前処理液2分注量 （μL)
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_PREPROSESS_2_DISPENSEVOLUME;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 17.多重測定内乖離限界（CV%）
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_MULMEASDEVLIMITCV;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);
            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 100;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add( rangeChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            // 18.手希釈使用有無
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_USEMANUALDIL;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add( boolChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            // 19.キャリブレーションタイプCalibration type
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_CALIBTYPE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 1;
            rangeChkInfo.MaxValue = 7;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 20.キャリブレーションポイント数Calibration points
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_NUMOFMEASPOINTINCALIB;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 2;
            rangeChkInfo.MaxValue = 8;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 21.キャリブレーション方法Calibration method
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_CALIBMETHOD;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add( boolChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //22-29 濃度		
			for ( int i = 1; i <= Const.MAX_CONCS; i++ )
			{
                string strName = String.Format( ConvertXmlControl.NM_CONCSOFEACH, i );
				
	            conv = new ConvertList();
	            conv.Name = strName;
	            conv.RowNo = ++iRowNoCnt;
	            
	            numChkInfo = new TypeNumberChecker();
	            numChkInfo.ErrString = numErrString;
	            conv.Validation.Add( numChkInfo );

	            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

        	}
        	
			// 30.係数A
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_COEF_A;
            conv.RowNo = ++iRowNoCnt;

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = -100.0;
            rangeChkInfo.MaxValue = 100.0;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 2;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 31.係数B
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_COEF_B;
            conv.RowNo = ++iRowNoCnt;

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = -100.0;
            rangeChkInfo.MaxValue = 100.0;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 2;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 32.係数C
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_COEF_C;
            conv.RowNo = ++iRowNoCnt;

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = -100.0;
            rangeChkInfo.MaxValue = 100.0;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 2;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 33.係数D
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_COEF_D;
            conv.RowNo = ++iRowNoCnt;

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = -100.0;
            rangeChkInfo.MaxValue = 100.0;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 3;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 34.係数E
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_COEF_E;
            conv.RowNo = ++iRowNoCnt;

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0.000;
            rangeChkInfo.MaxValue = 10.000;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 3;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 35.陽性判定閾値
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_POSILINE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0.0;
            rangeChkInfo.MaxValue = 9999.9;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 1;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 36.陰性判定閾値
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_NEGALINE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0.0;
            rangeChkInfo.MaxValue = 9999.9;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 1;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );


            // 37-52カウントチェック範囲Count check range
			for ( int i = 1; i <= Const.MAX_COUNT_RANGES; i++ )
			{
				// 上限
                string strName = String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MAX, i );

	            conv = new ConvertList();
	            conv.Name = strName;
	            conv.RowNo = ++iRowNoCnt;
	            
	            numChkInfo = new TypeNumberChecker();
	            numChkInfo.ErrString = numErrString;
	            conv.Validation.Add( numChkInfo );

	            rangeChkInfo = new TypeRangeChecker();
	            rangeChkInfo.MinValue = 0;
	            rangeChkInfo.ErrString = rngErrString;
	            conv.Validation.Add(rangeChkInfo);

	            dgtChkInfo = new TypeSmalldigitsChecker();
	            dgtChkInfo.Digit = 0;
	            dgtChkInfo.ErrString = dgtErrString;
	            conv.Validation.Add( dgtChkInfo );

	            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

				// 下限
                strName = String.Format( ConvertXmlControl.NM_COUNTRANGESOFEACH_MIN, i );

	            conv = new ConvertList();
	            conv.Name = strName;
	            conv.RowNo = ++iRowNoCnt;

	            numChkInfo = new TypeNumberChecker();
	            numChkInfo.ErrString = numErrString;
	            conv.Validation.Add( numChkInfo );

	            rangeChkInfo = new TypeRangeChecker();
	            rangeChkInfo.MinValue = 0;
	            rangeChkInfo.ErrString = rngErrString;
	            conv.Validation.Add(rangeChkInfo);

	            dgtChkInfo = new TypeSmalldigitsChecker();
	            dgtChkInfo.Digit = 0;
	            dgtChkInfo.ErrString = dgtErrString;
	            conv.Validation.Add( dgtChkInfo );

	            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );
	            
	        }

            // 53.濃度ダイナミックレンジ　上限Concentration dynamic range upper limit
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_CONCDYNAMICRANGE_MAX;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0.000;
            rangeChkInfo.MaxValue = 999999.000;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 3;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			//54. 濃度ダイナミックレンジ　下限
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_CONCDYNAMICRANGE_MIN;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0.000;
            rangeChkInfo.MaxValue = 99999.000;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 3;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 55.係数A  (-99 - 99)
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_COEFAOFLOG;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            //【IssuesNo:2】将系数A、B的范围改为（-99~99）
            rangeChkInfo.MinValue = -99;
            rangeChkInfo.MaxValue = 99;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 8;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 56.係数B  (-99 - 99)
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_COEFBOFLOG;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            //【IssuesNo:2】将系数A、B的范围改为（-99~99）
            rangeChkInfo.MinValue = -99;
            rangeChkInfo.MaxValue = 99;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 8;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 57.濃度単位
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_CONCUNIT;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            lenChkInfo = new TypeLengthChecker();
            lenChkInfo.MaxLength = 9;
            
            conv.Validation.Add( lenChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 58.濃度値小数点以下桁数
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_LENGTHAFTERDEMPOINT;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 4;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add( dgtChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

			// 59-66測定ポイント
			for ( int i = 1; i <= Const.MAX_CALIB_MEAS_POINT; i++ )
			{
                string strName = String.Format( ConvertXmlControl.NM_CALIBMEASPOINTOFEACH, i );

	            conv = new ConvertList();
	            conv.Name = strName;
	            conv.RowNo = ++iRowNoCnt;

                numChkInfo = new TypeNumberChecker();
                numChkInfo.ErrString = numErrString;
                conv.Validation.Add(numChkInfo);

                rangeChkInfo = new TypeRangeChecker();
                rangeChkInfo.MinValue = 0;
                rangeChkInfo.MaxValue = 1;
                rangeChkInfo.ErrString = rngErrString;
                conv.Validation.Add(rangeChkInfo);

                dgtChkInfo = new TypeSmalldigitsChecker();
                dgtChkInfo.Digit = 0;
                dgtChkInfo.ErrString = dgtErrString;
                conv.Validation.Add(dgtChkInfo);

                Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);
            }
			
			// 67.分析項目使用フラグ
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_DISPLAYPROTOCOL;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add( boolChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //68. 自動希釈倍率演算可否
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_USEAFTERDILATCALCU;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add( boolChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );
            
            // 69.手希釈倍率演算可否
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_USEMANUALDILATCALCU;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add( boolChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);
            
            // 70.自動希釈再検使用有無
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_USEAFTERDIL;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add( boolChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);
            
            // 71.自動再検使用有無
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_USEAUTORETEST;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add( reqChkInfo );

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add( numChkInfo );

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add( boolChkInfo );

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);
            
            // 72.自動希釈再検条件　上限
	        conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_AUTODILUTIONRETEST_MAX;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 1;
            rangeChkInfo.MaxValue = 99999999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add(dgtChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);
            
			// 73.自動希釈再検条件　下限
	        conv = new ConvertList();
            conv.Name = "AutoDilutionReTest(Min)";
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 99999999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add(dgtChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

			// 74.自動希釈再検条件(希釈倍率)
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_AUTODILUTIONRETESTRATIO;
            conv.RowNo = ++iRowNoCnt;

            TypeEnumChecker enumchkInfo = new TypeEnumChecker();
			enumchkInfo.CheckEnum = typeof(MeasureProtocol.AutoDilutionReTestRatioKind);
            conv.Validation.Add( enumchkInfo );
            Singleton<ConvertXmlControl>.Instance.ConvertList.Add( conv );

            // 75.自動再検条件　上限
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_AUTORETEST_MAX;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 1;
            rangeChkInfo.MaxValue = 99999999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add(dgtChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);


            // 76.自動再検条件　下限
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_AUTORETEST_MIN;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 99999999;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add(dgtChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

			//77 試薬開封後有効期限
			conv = new ConvertList();
			conv.Name = ConvertXmlControl.NM_DAY_OF_REAGENT_VALID;
            iRowNoCnt = ++iRowNoCnt;
			conv.RowNo = iRowNoCnt;

			reqChkInfo = new TypeRequiredChecker();
			reqChkInfo.ErrString = reqErrString;
			conv.Validation.Add(reqChkInfo);

			numChkInfo = new TypeNumberChecker();
			numChkInfo.ErrString = numErrString;
			conv.Validation.Add(numChkInfo);

			rangeChkInfo = new TypeRangeChecker();
			rangeChkInfo.MinValue = 1;
			rangeChkInfo.MaxValue = 9999;
			rangeChkInfo.ErrString = rngErrString;
			conv.Validation.Add(rangeChkInfo);

			Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //78是否选择重测下限
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_RETESTRANGE_LOW;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add(boolChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //79是否选择重测中间(灰区)区域
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_RETESTRANGE_MIDDLE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add(boolChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //80是否选择重测上限
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_RETESTRANGE_HIGH;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add(boolChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);


            // 81.ｷｬﾘﾌﾞﾚｰｼｮﾝ有効期限（日）
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_VALIDITYOFCURVE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0.0;
            rangeChkInfo.MaxValue = 100.0;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 1;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add(dgtChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //82是否选择独立ＣＶ
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_USECVINDEPENDENCE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            boolChkInfo = new TypeBoolChecker();
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add(boolChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //每个点的独立CV值得设置		
            for (int i = 1; i <= Const.MAX_CONCS; i++)
            {
                string strName = String.Format(ConvertXmlControl.NM_CVOFEACHPOINT, i);

                conv = new ConvertList();
                conv.Name = strName;
                conv.RowNo = ++iRowNoCnt;

                reqChkInfo = new TypeRequiredChecker();
                reqChkInfo.ErrString = reqErrString;
                conv.Validation.Add(reqChkInfo);

                numChkInfo = new TypeNumberChecker();
                numChkInfo.ErrString = numErrString;
                conv.Validation.Add(numChkInfo);
                rangeChkInfo = new TypeRangeChecker();
                rangeChkInfo.MinValue = 0;
                rangeChkInfo.MaxValue = 100;              
                rangeChkInfo.ErrString = rngErrString;
                conv.Validation.Add(rangeChkInfo);

                Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);
            }



            //83 4参数加权K值
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_FOURPRAMETERMETHODKVALUE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);


            //84 4参数加权类型
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_FOURPRAMETERMETHODTYPE;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 5;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //85 自控品、校准品是否稀释
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_DILUCALIBORCONTROL;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = 0;
            rangeChkInfo.MaxValue = 3;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add(dgtChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //86 ISIGRA
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_ISIGRA;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            boolChkInfo = new TypeBoolChecker();           
            boolChkInfo.ErrString = boolErrString;
            conv.Validation.Add(boolChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //87 TurnOrder 【IssuesNo:4】补充TurnOrder解析
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_TURNORDER;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //88 质控相关系数A（-99~99）【IssuesNo:1】
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_CONTROL_COEFAOFLOG;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = -99;
            rangeChkInfo.MaxValue = 99;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add(dgtChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);

            //89 质控相关系数B（-99~99）【IssuesNo:1】
            conv = new ConvertList();
            conv.Name = ConvertXmlControl.NM_CONTROL_COEFBOFLOG;
            conv.RowNo = ++iRowNoCnt;

            reqChkInfo = new TypeRequiredChecker();
            reqChkInfo.ErrString = reqErrString;
            conv.Validation.Add(reqChkInfo);

            numChkInfo = new TypeNumberChecker();
            numChkInfo.ErrString = numErrString;
            conv.Validation.Add(numChkInfo);

            rangeChkInfo = new TypeRangeChecker();
            rangeChkInfo.MinValue = -99;
            rangeChkInfo.MaxValue = 99;
            rangeChkInfo.ErrString = rngErrString;
            conv.Validation.Add(rangeChkInfo);

            dgtChkInfo = new TypeSmalldigitsChecker();
            dgtChkInfo.Digit = 0;
            dgtChkInfo.ErrString = dgtErrString;
            conv.Validation.Add(dgtChkInfo);

            Singleton<ConvertXmlControl>.Instance.ConvertList.Add(conv);
        }
  
        #endregion
    }
}
