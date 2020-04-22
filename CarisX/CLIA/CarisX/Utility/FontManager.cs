using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// フォントセット管理クラス
    /// </summary>
    static public class FontManager
    {
        #region [定数定義]

        /// <summary>
        /// 印刷用フォント種別
        /// </summary>
        public enum PrintFontKind
        {
            /// <summary>
            /// タイトル文字列
            /// </summary>
            Title,
            /// <summary>
            /// 値文字列
            /// </summary>
            Value
        }
        #endregion

        #region [クラス変数定義]

        /// <summary>
        /// 印刷用フォント設定辞書
        /// </summary>
        static private Dictionary<PrintFontKind, Font> printFont = new Dictionary<PrintFontKind, Font>();
        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static FontManager()
        {
            // 全フォント情報初期化
            allFontDicInitialize();

            // 全フォント情報設定
            allFontDicSetCulture();
        }
        #endregion

        #region [プロパティ]

        /// <summary>
        /// 印刷用フォント設定辞書 取得
        /// </summary>
        static public  Dictionary<PrintFontKind, Font> PrintFont
        {
            get
            {
                return printFont;
            }
        }

        #endregion

        #region [privateメソッド]

        /// <summary>
        /// 全フォント情報初期化
        /// </summary>
        /// <remarks>
        /// 全フォント情報の辞書を初期化します。
        /// </remarks>
        static private void allFontDicInitialize()
        {
            // 印刷用フォントセット初期化
            fontDicInitialize( printFont );
        }
        /// <summary>
        /// 全フォント情報設定
        /// </summary>
        /// <remarks>
        /// 全フォント情報の辞書を設定します。
        /// </remarks>
        static private void allFontDicSetCulture()
        {
            // 印刷用フォントセット設定
            setPrintFontDic();
        }

        /// <summary>
        /// フォント辞書初期化
        /// </summary>
        /// <remarks>
        /// フォント辞書の初期化を行います。
        /// </remarks>
        /// <typeparam name="EnumType">フォントセットタイプ</typeparam>
        /// <param name="fontDic">フォントセット</param>
        static private void fontDicInitialize<EnumType>( Dictionary<EnumType, Font > fontDic )
        {
            var enValues = Enum.GetValues( typeof(EnumType) );
            foreach ( var val in enValues )
            {
                fontDic.Add( (EnumType)val, new Font( FontFamily.GenericMonospace, 10 ) );
            }
        }

        /// <summary>
        /// 印刷用フォント情報設定
        /// </summary>
        /// <remarks>
        /// 印刷用フォント情報を設定します。
        /// </remarks>
        static private void setPrintFontDic()
        {
            // 各種フォントを設定
            printFont[PrintFontKind.Title] = new Font( CarisX.Properties.Resources.STRING_FONTNAME_PRINT_TITLE,
                                                       float.Parse(CarisX.Properties.Resources.STRING_FONTSIZE_PRINT_TITLE ) );
            printFont[PrintFontKind.Value] = new Font( CarisX.Properties.Resources.STRING_FONTNAME_PRINT_DETAIL ,
                                                       float.Parse(CarisX.Properties.Resources.STRING_FONTSIZE_PRINT_DETAIL ) ); 
        }
        #endregion

    }
}
