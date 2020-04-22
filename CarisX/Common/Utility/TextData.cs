using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Parameter;

namespace Oelco.Common.Utility
{

    /// <summary>
    /// テキストデータクラス
    /// </summary>
    /// <remarks>
    /// テキストデータから値を取り出す処理を定義します。
    /// </remarks>
    public class TextData
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// テキスト処理位置
        /// </summary>
        Int32 currentPos = 0;

        /// <summary>
        /// 処理対象テキスト
        /// </summary>
        String targetString;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="textData">処理対象文字列</param>
        public TextData( String textData )
        {
            this.currentPos = 0;
            this.targetString = textData;
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 処理位置初期化
        /// </summary>
        /// <remarks>
        /// 処理位置を初期化します。
        /// </remarks>
        public void ResetPos()
        {
            this.currentPos = 0;
        }

        // TODO:spoil関連先頭大文字にする。（調整部でも使用しているため統合の後。）

        /// <summary>
        /// 数値(Int32)を抽出
        /// </summary>
        /// <remarks>
        /// 現在のテキスト処理位置より指定の長さの文字列分を数値(Int32)として取得します。
        /// </remarks>
        /// <param name="outData">抽出値</param>
        /// <param name="len">長さ</param>
        /// <returns>抽出結果(true:成功/false:失敗)</returns>
        public Boolean spoilInt( out Int32 outData, Int32 len )
        {
            if ( targetString.Length < ( currentPos + len ) )
            {
                outData = 0;
                return false;
            }
            String strOut = targetString.Substring( currentPos, len );
            currentPos += len;
            return Int32.TryParse( strOut, out outData );
        }

        /// <summary>
        /// 数値(Int64)を抽出
        /// </summary>
        /// <remarks>
        /// 現在のテキスト処理位置より指定の長さの文字列分を数値(Int64)として取得します。
        /// </remarks>
        /// <param name="outData">抽出値</param>
        /// <param name="len">長さ</param>
        /// <returns>抽出結果(true:成功/false:失敗)</returns>
        public Boolean spoilLong( out Int64 outData, Int32 len )
        {
            if ( targetString.Length < ( currentPos + len ) )
            {
                outData = 0;
                return false;
            }
            String strOut = targetString.Substring( currentPos, len );
            currentPos += len;
            return Int64.TryParse( strOut, out outData );
        }

        /// <summary>
        /// 数値(Double)を抽出
        /// </summary>
        /// <remarks>
        /// 現在のテキスト処理位置より指定の長さの文字列分を数値(Double)として取得します。
        /// </remarks>
        /// <param name="outData">抽出値</param>
        /// <param name="len">長さ</param>
        /// <returns>抽出結果(true:成功/false:失敗)</returns>
        public Boolean spoilDouble( out Double outData, Int32 len )
        {
            if ( targetString.Length < ( currentPos + len ) )
            {
                outData = 0.0;
                return false;
            }
            String strOut = targetString.Substring( currentPos, len );
            currentPos += len;
            return Double.TryParse( strOut, out outData );
        }

        /// <summary>
        /// バイト値(Byte)を抽出
        /// </summary>
        /// <remarks>
        /// 現在のテキスト処理位置より指定の長さの文字列分をバイト値(Byte)として取得します。
        /// </remarks>
        /// <param name="outData">抽出値</param>
        /// <param name="len">長さ</param>
        /// <returns>抽出結果(true:成功/false:失敗)</returns>
        public Boolean spoilByte( out Byte outData, Int32 len )
        {
            if ( targetString.Length < ( currentPos + len ) )
            {
                outData = 0;
                return false;
            }
            String strOut = targetString.Substring( currentPos, len );
            currentPos += len;
            return Byte.TryParse( strOut, out outData );
        }

        /// <summary>
        /// 文字列を抽出
        /// </summary>
        /// <remarks>
        /// 現在のテキスト処理位置より指定の長さの文字列分を文字列として取得します。
        /// </remarks>
        /// <param name="outData">抽出値</param>
        /// <param name="len">長さ</param>
        /// <returns>抽出結果(true:成功/false:失敗)</returns>
        public Boolean spoilString( out String outData, Int32 len )
        {
            if ( targetString.Length < ( currentPos + len ) )
            {
                outData = "";
                return false;
            }
            String strOut = targetString.Substring( currentPos, len );
            currentPos += len;
            if (GlobalParameter.myApplicationKind == GlobalParameter.ApplicationKind.CarisX)
            {
                // CarisX
                outData = strOut.Trim();    // 空白削除
            }
            else
            {
                // CarisX以外（NS-Prime（AFT)等）
                outData = strOut;           // 空白削除しない
            }
            return true;
        }

        /// <summary>
        /// ブール値(Boolean)を抽出
        /// </summary>
        /// <remarks>
        /// 現在のテキスト処理位置より指定の長さの文字列分をブール値(Boolean)として取得します。
        /// </remarks>
        /// <param name="outData">抽出値</param>
        /// <param name="len">長さ</param>
        /// <returns>抽出結果(true:成功/false:失敗)</returns>
        public Boolean spoilBoolean( out Boolean outData )
        {
            if ( targetString.Length < ( currentPos + 1 ) )
            {
                outData = false;
                return false;
            }
            String strOut = targetString.Substring( currentPos, 1 );
            currentPos += 1;

            Boolean result = true;
            switch (strOut)
            {
                case "0":
                    outData = false;
                    break;
                case "1":
                    outData = true;
                    break;
                default:
                    outData = false;
                    result = false;
                    break;
            }
            return result;
        }

        #endregion

    }

    /// <summary>
    /// テキストデータ連結クラス
    /// </summary>
    /// <remarks>
    /// テキストデータを連結します。
    /// </remarks>
    public class TextDataBuilder
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// 処理対象テキスト
        /// </summary>
        StringBuilder builder;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextDataBuilder()
        {
            this.builder = new StringBuilder();
        }

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 文字列追加
        /// </summary>
        /// <remarks>
        /// 左に指定の文字を詰めた値を返します。
        /// </remarks>
        /// <param name="value">対象の文字列</param>
        /// <param name="totalWidth">文字列の長さ</param>
        /// <param name="paddingChar">詰める文字</param>
        public void Append(String value, Int32 totalWidth = 0, Char paddingChar = ' ')
        {
            value = value ?? "";
            this.builder.Append(value.PadLeft(totalWidth, paddingChar).Substring(0, totalWidth));
        }

        /// <summary>
        /// 文字列追加
        /// </summary>
        /// <remarks>
        /// 右に指定の文字を詰めた値を返します。
        /// </remarks>
        /// <param name="value">対象の文字列</param>
        /// <param name="totalWidth">文字列の長さ</param>
        /// <param name="paddingChar">詰める文字</param>
        public void AppendRight(String value, Int32 totalWidth = 0, Char paddingChar = ' ')
        {
            value = value ?? "";
            String appendStr = value.PadRight(totalWidth, paddingChar);
            Int32 startPos = appendStr.Length - totalWidth;
            this.builder.Append(appendStr.Substring(startPos));
        }

        /// <summary>
        /// 連結文字列取得
        /// </summary>
        /// <remarks>
        /// 連結文字列を返します。
        /// </remarks>
        public String GetAppendString()
        {
            return this.builder.ToString();
        }

        #endregion
    }

}
