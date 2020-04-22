using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using ProtocolConverter.File;

namespace ProtocolConverter.Check
{    
    /// <summary>
    /// インポート項目単独チェック
    /// </summary>
    /// <remarks>
    /// インポート項目毎のチェック処理を行います。
    /// </remarks>
    public class DataCheckerTypeCheck 
    {
        /// <summary>
        /// 判定種別
        /// </summary>
        public enum DecisionType
        {
            equal,
            not
        }

        /// <summary>
        /// エラー文字列
        /// </summary>
        public String ErrString
        {
            get;
            set;
        }

        /// <summary>
        /// 出力文字列
        /// </summary>
        public String OutPutValue
        {
            get;
            set;
        }

        /// <summary>
        /// チェック処理
        /// </summary>
        /// <remarks>
        /// 派生先で行われるチェック処理です。
        /// </remarks>
        /// <param name="value">チェック対象項目の値</param>
        /// <returns></returns>
        public virtual ResultInfo Check( String value )
        {
            return new ResultInfo();
        }
    }

    /// <summary>
    /// 必須入力チェッククラス
    /// </summary>
    public class TypeRequiredChecker : DataCheckerTypeCheck
    {
        /// <summary>
        /// 必須入力チェック処理
        /// </summary>
        /// <remarks>
        /// 必須入力項目に対し、値が設定されているかどうかをチェックします。
        /// </remarks>
        /// <param name="value">チェック対象項目の値</param>
        /// <returns></returns>
        public override ResultInfo Check( String value )
        {
            ResultInfo resultInfo = new ResultInfo();
            bool checkResult = true;

            //空白チェック
            if ( String.IsNullOrEmpty(value) )
            {
                checkResult = false;
            }

            if ( checkResult == false )
            {
                //error要素ならエラーとしてエラー番号を格納
                if ( String.IsNullOrEmpty( this.ErrString ) == false )
                {
                    resultInfo.Result = false;
                    resultInfo.Data = this.ErrString;
                }
                //output要素ならエラーとせず、output要素値を格納
                else if ( String.IsNullOrEmpty(this.OutPutValue) == false )
                {
                    resultInfo.Result = true;
                    resultInfo.Data = this.OutPutValue;
                }
                else
                {
                    resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal3"];
                    resultInfo.Result = false;
                }
            }
            else
            {
                resultInfo.Data = value;
            }

            return resultInfo;
        }
    }

    /// <summary>
    /// 数値チェッククラスNumerical check class
    /// </summary>
    public class TypeNumberChecker : DataCheckerTypeCheck
    {
        /// <summary>
        /// 数値チェック処理
        /// </summary>
        /// <remarks>
        /// チェック対象項目が数値かどうかをチェックします。
        /// </remarks>
        /// <param name="value">チェック対象項目の値</param>
        /// <returns></returns>
        public override ResultInfo Check( String value )
        {
            double dNullable;
            ResultInfo resultInfo = new ResultInfo();
            bool checkResult = true;

            if ( !String.IsNullOrEmpty( value ) )
            {
                //数値チェックNumerical check
                checkResult = double.TryParse( value, NumberStyles.Any, null, out dNullable );

                if ( checkResult == false )
                {
                    //error要素ならエラーとしてエラー番号を格納
                    if ( String.IsNullOrEmpty( this.ErrString ) == false )
                    {
                        resultInfo.Result = false;
                        resultInfo.Data = this.ErrString;
                    }
                    //output要素ならエラーとせず、output要素値を格納
                    else if ( String.IsNullOrEmpty( this.OutPutValue ) == false )
                    {
                        resultInfo.Result = true;
                        resultInfo.Data = this.OutPutValue;
                    }
                    else
                    {
                        resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal3"];
                        resultInfo.Result = false;
                    }
                }
                else
                {
                    resultInfo.Data = value;
                }
            }           
            return resultInfo;
        }
    }

    /// <summary>
    /// 桁数チェッククラス
    /// </summary>
    public class TypeLengthChecker : DataCheckerTypeCheck
    {
        /// <summary>
        /// MAX桁数
        /// </summary>
        public Int32 MaxLength
        {
            get;
            set;
        }

        /// <summary>
        /// 桁数チェック処理
        /// </summary>
        /// <remarks>
        /// チェック対象項目の桁数がMaxLength以内かどうかをチェックします。
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public override ResultInfo Check( String value )
        {
            ResultInfo resultInfo = new ResultInfo();
            bool checkResult = true;

            //文字数チェック。
            if ( value.Length > this.MaxLength )
            {
                checkResult = false;
            }


            if ( checkResult == false )
            {
                //error要素ならエラーとしてエラー番号を格納
                if ( String.IsNullOrEmpty( this.ErrString ) == false )
                {
                    resultInfo.Result = false;
                    resultInfo.Data = this.ErrString;
                }
                //output要素ならエラーとせず、output要素値を格納
                else if ( String.IsNullOrEmpty(this.OutPutValue) == false )
                {
                    resultInfo.Result = true;
                    resultInfo.Data = this.OutPutValue;
                }
                else
                {
                    resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal3"];
                    resultInfo.Result = false;
                }
            }
            else
            {
                resultInfo.Data = value;
            }


            return resultInfo;
        }
    } 

    /// <summary>
    /// 範囲チェッククラス
    /// </summary>
    public class TypeRangeChecker : DataCheckerTypeCheck
    {        

        /// <summary>
        /// 下限値
        /// </summary>
        private Double minValue = Double.MinValue; 
        public Double MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
            }
        }

        /// <summary>
        /// 上限値
        /// </summary>
        private Double maxValue = Double.MaxValue; 
        public Double MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }

        }

        /// <summary>
        ///　範囲チェック処理 
        /// </summary>
        /// <remarks>
        /// チェック対象項目が MinValue ～ MaxValue の範囲内かどうかをチェックします
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public override ResultInfo Check( String value )
        {
            ResultInfo resultInfo = new ResultInfo();
            bool checkResult = true;
            double dblData = 0;

            if ( !String.IsNullOrEmpty( value ) )
            {
                //数値チェック
                checkResult = double.TryParse( value, NumberStyles.Any, null, out dblData );
                if ( checkResult == false )
                {
                    resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal2"];
                    resultInfo.Result = false;
                }
                else
                {
                    //数値範囲チェック。decision属性値により範囲が変化
                    if ( ( this.MaxValue < dblData ) | ( this.MinValue > dblData ) )
                    {
                        checkResult = false;
                    }


                    if ( checkResult == false )
                    {
                        //error要素ならエラーとしてエラー番号を格納
                        if ( String.IsNullOrEmpty( this.ErrString ) == false )
                        {
                            resultInfo.Result = false;
                            resultInfo.Data = this.ErrString;
                        }
                        //output要素ならエラーとせず、output要素値を格納
                        else if ( String.IsNullOrEmpty( this.OutPutValue ) == false )
                        {
                            resultInfo.Result = true;
                            resultInfo.Data = this.OutPutValue;
                        }
                        else
                        {
                            resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal3"];
                            resultInfo.Result = false;
                        }
                    }
                    else
                    {
                        resultInfo.Data = value;
                    }
                }            
            } 
            return resultInfo;
        }

    }

    /// <summary>
    /// 小数部桁数チェッククラスNumber of decimals check class
    /// </summary>
    public class TypeSmalldigitsChecker : DataCheckerTypeCheck
    {
        /// <summary>
        /// 小数部桁数
        /// </summary>
        public Int32 Digit
        {
            get;
            set;
        }

        /// <summary>
        /// 小数部桁数チェック処理
        /// </summary>
        /// <remarks>
        /// チェック対象項目の小数部桁数が Digit 設定値以下かどうかをチェックします。
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public override ResultInfo Check( String value )
        {
            ResultInfo resultInfo = new ResultInfo();
            bool checkResult = true;
            string buf = string.Empty;
            string convertData = string.Empty;
            //int intDummy = 0;
            //bool smalldigitFlg = true;

            double dblData = 0;

            if ( !String.IsNullOrEmpty( value ) )
            {
                // 検査データが数値か判断
                if ( double.TryParse( value, NumberStyles.Any, null, out dblData ) == true )
                {
                    convertData = Convert.ToString( dblData );
                }
                else
                {
                    // エラー出力
                    resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal2"];
                    resultInfo.Result = false;
                    return resultInfo;
                }

                // 小数部桁数チェック
                String[] chkstr = value.Split( '.' );
                if ( chkstr.Length == 2 && chkstr[1].Length > this.Digit )
                {
                    checkResult = false;
                }

                if ( checkResult == false )
                {
                    // error要素ならエラーとしてエラー番号を格納
                    if ( String.IsNullOrEmpty( this.ErrString ) == false )
                    {
                        resultInfo.Result = false;
                        resultInfo.Data = this.ErrString;
                    }
                    // output要素ならエラーとせず、output要素値を格納
                    else if ( String.IsNullOrEmpty( this.OutPutValue ) == false )
                    {
                        resultInfo.Result = true;
                        resultInfo.Data = this.OutPutValue;
                    }
                    else
                    {
                        // output、error要素どちらも無い場合はXML定義エラー
                        resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal3"];
                        resultInfo.Result = false;
                    }
                }
                else
                {
                    resultInfo.Data = value;
                }
            }            

            return resultInfo;
        }
    }

    /// <summary>
    /// Bool値チェッククラス
    /// </summary>
    public class TypeBoolChecker : DataCheckerTypeCheck
    {
        /// <summary>
        /// Bool値チェック処理
        /// </summary>
        /// <remarks>
        /// チェック対象項目の値が 0 か 1　以外でないかをチェックします。
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public override ResultInfo Check( String value )
        {
            ResultInfo resultInfo = new ResultInfo();
            if ( value != "0" && value != "1" )
            {
                // error要素ならエラーとしてエラー番号を格納
                if ( String.IsNullOrEmpty( this.ErrString ) == false )
                {
                    resultInfo.Result = false;
                    resultInfo.Data = this.ErrString;
                }
                // output要素ならエラーとせず、output要素値を格納
                else if ( String.IsNullOrEmpty( this.OutPutValue ) == false )
                {
                    resultInfo.Result = true;
                    resultInfo.Data = this.OutPutValue;
                }
                else
                {
                    resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal3"];
                    resultInfo.Result = false;
                }
            }
            else
            {
                resultInfo.Data = value;
            }

            return resultInfo;
        }
    }

    /// <summary>
    /// Enum型項目チェッククラス
    /// </summary>
    public class TypeEnumChecker : DataCheckerTypeCheck
    {
        /// <summary>
        /// チェック対象Enum定義
        /// </summary>
        public Type CheckEnum
        {
            get;
            set;
        }
 
        /// <summary>
        /// Enum型項目チェック処理
        /// </summary>
        /// <remarks>
        /// チェック対象項目の値がEnumに定義されている値かどうかをチェックします。
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public override ResultInfo Check( String value )
        {
            ResultInfo resultInfo = new ResultInfo();
            bool checkResult = true;

            Int32 chk ;

            if ( !String.IsNullOrEmpty( value ) )
            {
                // 数値に変換できなければエラー
                if ( !Int32.TryParse( value, out chk ) )
                {
                    resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal2"];
                    resultInfo.Result = false;
                }
                else
                {
                    if ( Enum.IsDefined( CheckEnum, chk ) )
                    {
                        resultInfo.Data = value;
                        checkResult = true;
                    }
                    else
                    {
                        // Enumの定義に存在しない値ならエラー
                        checkResult = false;
                    }
                }

                if ( checkResult == false )
                {
                    // error要素ならエラーとしてエラー番号を格納
                    if ( String.IsNullOrEmpty( this.ErrString ) == false )
                    {
                        resultInfo.Result = false;
                        resultInfo.Data = this.ErrString;
                    }
                    // output要素ならエラーとせず、output要素値を格納
                    else if ( String.IsNullOrEmpty( this.OutPutValue ) == false )
                    {
                        resultInfo.Result = true;
                        resultInfo.Data = this.OutPutValue;
                    }
                    else
                    {
                        resultInfo.Data = Singleton<ConvertXmlControl>.Instance.ErrorNoList["Internal3"];
                        resultInfo.Result = false;
                    }
                }       
            }
            return resultInfo;
        }
    }
}
