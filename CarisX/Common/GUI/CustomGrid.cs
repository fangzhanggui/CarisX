using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using Infragistics.Win.UltraWinGrid;
using System.Windows.Forms;
using System.Globalization;
using Oelco.Common.Utility;
using Oelco.Common.Log;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Oelco.Common.GUI
{
    /// <summary>
    /// カスタムグリッド
    /// </summary>
    public class CustomGrid : UltraGrid
    {
        // Windows10対応
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        // Windows10対応
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        #region [定数定義]

        /// <summary>
        /// ズームステップ値 デフォルト値(%)
        /// </summary>
        protected const Int32 ZOOM_STEP_DEFAULT = 10;

        /// <summary>
        /// グリッドのズーム最大値(%)
        /// </summary>
        public const Int32 ZOOM_MAX = 500;

        /// <summary>
        /// デフォルトフォントサイズ
        /// </summary>
        private const Single DEFAULT_FONT_SIZE = 12f;

        /// <summary>
        /// デフォルト拡大率
        /// </summary>
        private const Int32 DEFAULT_ZOOM_PERCENT = 100;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// ズームステップ値
        /// </summary>
        private Int32 zoomStep = ZOOM_STEP_DEFAULT;

        /// <summary>
        /// カラム色設定
        /// </summary>
        private Dictionary<Int32, ColorStatus> columnColorSetting = new Dictionary<Int32, ColorStatus>();

        /// <summary>
        /// デフォルトフォントサイズ
        /// </summary>
        private Single defaultFontSize = DEFAULT_FONT_SIZE;

        /// <summary>
        /// サイズ変更前列幅サイズ比率
        /// </summary>
        private Int32 preColumnWidthPer = 100;

        /// <summary>
        /// グリッド行背景色ルール
        /// </summary>
        private GridRowBackgroundColorRule rowColorRule = new GridRowBackgroundColorRule();

        /// <summary>
        /// スクロール処理代行
        /// </summary>
        private ScrollProxy scrollFunc = new ScrollProxy();

        /// <summary>
        /// 最終セル
        /// </summary>
        private Dictionary<String, UltraGridCell> lastCells = new Dictionary<String, UltraGridCell>();

        /// <summary>
        /// 最終行
        /// </summary>
        private UltraGridRow lastRow;
        
        /// <summary>
        /// 最終行列インデックス
        /// </summary>
        private Int32 lastRowColIndex = 0;

        /// <summary>
        /// 行データ比較
        /// </summary>
        private Func<Object, Object, Int32> gridRowDataCompare;

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CustomGrid()
        {
            // タッチ用 スクロール処理設定
            this.scrollFunc.GetXMinValue = (DlgGetXMinValue)( () => 0 );
            this.scrollFunc.GetXMaxValue = (DlgGetXMaxValue)( () => this.ActiveColScrollRegion.Range );
            this.scrollFunc.GetXValue = (DlgGetXValue)( () => this.ActiveColScrollRegion.Position );
            this.scrollFunc.SetXValue = (DlgSetXValue)( ( val ) => this.ActiveColScrollRegion.Position = val );
            this.scrollFunc.GetYMinValue = (DlgGetYMinValue)( () => 0 );
            this.scrollFunc.GetYMaxValue = (DlgGetYMaxValue)( () => this.Rows.Count * 10 );
            this.scrollFunc.GetYValue = (DlgGetYValue)( () => this.ActiveRowScrollRegion.ScrollPosition * 10 );
            this.scrollFunc.SetYValue = (DlgSetYValue)( ( val ) => ActiveRowScrollRegion.ScrollPosition = val / 10 );

            //// 列幅の自動調整抑制
            //this.DisplayLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.None;
            //this.DisplayLayout.Bands.SubObjectPropChanged += ( propChange ) =>
            //{
            //    if ( propChange.Source == this.DisplayLayout.Bands && this.DisplayLayout.Bands.Count > 0 )
            //    {
            //        foreach ( var band in this.DisplayLayout.Bands.OfType<UltraGridBand>() )
            //        {
            //            if ( band.Override.ColumnAutoSizeMode != ColumnAutoSizeMode.None )
            //            {
            //                band.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.None;
            //            }
            //        }
            //    }
            //};

            // Windows10の場合のみ処理を行う
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
            {
                // ソフトウェアキーボードを開く・閉じるためのイベント登録
                this.ClickCell += new ClickCellEventHandler(this.CustomGrid_ClickCell);
                this.AfterExitEditMode += new EventHandler(this.CustomGrid_AfterExitEditMode);
            }
        }

        /// <summary>
        /// セルがクリックされた
        /// </summary>
        /// <param name="sender">発生元オブジェクト</param>
        /// <param name="e">イベントパラメーター</param>
        private void CustomGrid_ClickCell(object sender, ClickCellEventArgs e)
        {
            // Windows10の場合のみ処理を行う
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
            {
                // アクティブなセルであり、編集状態の場合
                if ((this.ActiveCell != null) && (this.ActiveCell.Activated == true) && (this.ActiveCell.IsInEditMode == true))
                {
                    // ソフトウェアキーボードを開く
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(startInfo);
                }
                else
                {
                    // ソフトウェアキーボードを閉じる
                    uint WM_SYSCOMMAND = 274;
                    uint SC_CLOSE = 61536;
                    IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
                    PostMessage(KeyboardWnd.ToInt32(), WM_SYSCOMMAND, (int)SC_CLOSE, 0);
                }
            }
        }
        
        /// <summary>
        /// セルの編集状態が解除された
        /// </summary>
        /// <param name="sender">発生元オブジェクト</param>
        /// <param name="e">イベントパラメーター</param>
        private void CustomGrid_AfterExitEditMode(object sender, EventArgs e)
        {
            // Windows10の場合のみ処理を行う
            System.OperatingSystem os = System.Environment.OSVersion;
            if (os.Platform == System.PlatformID.Win32NT && os.Version.Major == 6 && os.Version.Minor == 2)
            {
                // ソフトウェアキーボードを閉じる
                uint WM_SYSCOMMAND = 274;
                uint SC_CLOSE = 61536;
                IntPtr KeyboardWnd = FindWindow("IPTip_Main_Window", null);
                PostMessage(KeyboardWnd.ToInt32(), WM_SYSCOMMAND, (int)SC_CLOSE, 0);
            }
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// ズームステップ値 取得/設定
        /// </summary>
        public Int32 ZoomStep
        {
            get
            {
                return zoomStep;
            }
            set
            {
                zoomStep = value;
            }
        }

        /// <summary>
        /// スクロール処理代行 取得/設定
        /// </summary>
        public ScrollProxy ScrollProxy
        {
            get
            {
                return this.scrollFunc;
            }
        }
        
        #endregion

        #region [publicメソッド]

        /// <summary>
        /// グリッドの倍率を変更する
        /// </summary>
        /// <remarks>
        /// グリッドの表示倍率を変更します。
        /// </remarks>
        /// <param name="percentage"></param>
        public void SetGridZoom( Int32 percentage )
        {
            // 範囲外かどうか
            if ( ( ZOOM_MAX < percentage ) || ( 0 >= percentage ) )
            {
                return;
            }

            // フォントサイズを取得し、グリッドへ適用
            float fontSize = this.defaultFontSize * ( (float)percentage / 100 );
            var oldFontSize = this.Font.SizeInPoints;
            this.Font = new Font( this.Font.FontFamily, fontSize );
            this.DisplayLayout.Override.HeaderAppearance.FontData.SizeInPoints *= fontSize / oldFontSize;

            //表の高さ調整
            Int32 rowHeight = (Int32)((Double)DisplayLayout.Override.DefaultRowHeight * ((Double)percentage / (Double)this.preColumnWidthPer));
            this.DisplayLayout.Override.DefaultRowHeight = rowHeight;

            // 表示カラム幅調整（設定前の比率と設定比率の差分を伸縮する）
            foreach ( var band in this.DisplayLayout.Bands )
            {
                foreach ( var column in band.Columns )
                {
                    Double colWidth = ( (Double)column.Width ) * ( (Double)( percentage ) / (Double)( this.preColumnWidthPer ) );
                    Int32 colTemp = (Int32)colWidth;
                    if ( ( colWidth - (Double)colTemp ) >= 0.5 )
                    {
                        colWidth += 1f;
                    }
                    column.RowLayoutColumnInfo.PreferredCellSize = new Size( (Int32)colWidth, column.RowLayoutColumnInfo.PreferredCellSize.Height );
                    column.Width = column.RowLayoutColumnInfo.PreferredCellSize.Width;
                }
            }
            this.preColumnWidthPer = percentage;

        }

        /// <summary>
        /// カラム色設定
        /// </summary>
        /// <remarks>
        /// 該当カラムに適用される色設定を行います。
        /// </remarks>
        /// <param name="colIdx">カラムインデックス</param>
        /// <param name="colorStatus">色設定</param>
        public void SetColorStatusForColumn( Int32 colIdx, ColorStatus colorStatus )
        {
            // 登録がなければ新規、あれば更新を行う。
            if ( !this.columnColorSetting.ContainsKey( colIdx ) )
            {
                this.columnColorSetting.Add( colIdx, colorStatus );
            }
            else
            {
                this.columnColorSetting[colIdx] = colorStatus;
            }
        }

        /// <summary>
        /// 行インデックスによる背景色ルール設定
        /// </summary>
        /// <remarks>
        /// 行インデックスによる背景色ルール設定を行います。
        /// このルールは、設定以後の行追加時に適用されます。
        /// 色設定間隔行と色パターンリストは以下の方式で適用されます。
        /// 色パターンリスト使用インデックス = ( グリッド行インデックス / 色設定行間隔 ) % 色パターンリストカウント ) 
        /// </remarks>
        /// <param name="setCount">色設定行間隔</param>
        /// <param name="colorPatternList">色パターンリスト</param>
        public void SetGridRowBackgroundColorRuleFromIndex( Int32 setCount, List<Color> colorPatternList )
        {
            this.rowColorRule.ColorRuleKind = GridRowBackgroundColorRule.RuleKind.FromIndex; // 行インデックスによる背景色ルール適用フラグ
            this.rowColorRule.SetCount = setCount;
            this.rowColorRule.ColorPatternList = colorPatternList;
        }

        /// <summary>
        /// セルデータによる背景色ルール設定
        /// </summary>
        /// <remarks>
        /// セルデータによる背景色ルール設定を行います。
        /// このルールは、設定以後の行追加時に適用されます。
        /// 列名リストと色パターンリストは以下の方式で適用されます。
        /// 色パターンリスト使用インデックス = ( グリッド行インデックス / 色設定行間隔 ) % 色パターンリストカウント ) 
        /// </remarks>
        /// <param name="columnNameList">列名リスト</param>
        /// <param name="colorPatternList">色パターンリスト</param>
        public void SetGridRowBackgroundColorRuleFromCellData( List<String> columnNameList, List<Color> colorPatternList )
        {
            this.rowColorRule.ColorRuleKind = GridRowBackgroundColorRule.RuleKind.FromCellData; // セル内容による背景色ルール適用フラグ
            this.rowColorRule.SetColName = columnNameList;
            this.rowColorRule.ColorPatternList = colorPatternList;
        }

        /// <summary>
        /// 行データによる背景色ルール設定
        /// </summary>
        /// <param name="compare"> 最初のオブジェクト と 2番目のオブジェクト の相対的な値を示す符号付き整数 (次の表を参照)。<br/>
        /// 0より小:最初のオブジェクト が 2番目のオブジェクト より小さい。<br/>
        /// 0      :最初のオブジェクト と 2番目のオブジェクト は等しい。<br/>
        /// 0より大:最初のオブジェクトが 2番目のオブジェクト より大きい。</param>
        /// <param name="colorPatternList"></param>
        public void SetGridRowBackgroundColorRuleFromRowData( Func<Object,Object,Int32> compare, List<Color> colorPatternList )
        {
            this.rowColorRule.ColorRuleKind = GridRowBackgroundColorRule.RuleKind.FromRowData; // セル内容による背景色ルール適用フラグ
            this.gridRowDataCompare = compare;
            this.rowColorRule.ColorPatternList = colorPatternList;
        }

        
        /// <summary>
        /// 選択行検索
        /// </summary>
        /// <remarks>
        /// 選択された行と、選択されたセルの属する行の集合を返します。
        /// </remarks>
        /// <returns>選択行</returns>
        public List<UltraGridRow> SearchSelectRow()
        {
            List<UltraGridRow> searched = new List<UltraGridRow>();

            if ( this.ActiveCell != null )
            {
                UltraGridCell activeCell = this.ActiveCell;
                this.ActiveCell.Activated = false;
                activeCell.Selected = true;
            }
            else if ( this.ActiveRow != null )
            {
                UltraGridRow activeRow = this.ActiveRow;
                this.ActiveRow.Activated = false;
                activeRow.Selected = true;
            }

            searched.AddRange(this.Rows.Where(rows => rows.Selected || rows.Activated).OfType<UltraGridRow>());

            return searched;
        }

        /// <summary>
        /// グリッド列順取得
        /// </summary>
        /// <remarks>
        /// グリッドの列順を返します。
        /// </remarks>
        /// <returns></returns>
        public List<String> GetGridColumnOrder()
        {
            SortedDictionary<int, String> wkGridColOrder = new SortedDictionary<int, String>();
            List<String> rtnList = new List<String>();


            foreach ( UltraGridColumn aColumn in this.DisplayLayout.Bands[0].Columns )
            {
                wkGridColOrder.Add( aColumn.Header.VisiblePosition, aColumn.Key.ToString() );
            }

            foreach ( String colName in wkGridColOrder.Values )
            {
                rtnList.Add( colName );
            }
            return rtnList;
        }

        /// <summary>
        /// グリッド列順設定
        /// </summary>
        /// <remarks>
        /// グリッドの列順を設定します。
        /// </remarks>
        /// <returns></returns>
        public Boolean SetGridColumnOrder( List<String> orderCol )
        {
            if ( orderCol == null || orderCol.Count == 0 )
            {
                return true;
            }
            Int32 idx = 0;
            var columns = this.DisplayLayout.Bands[0].Columns.OfType<UltraGridColumn>().Select( ( column ) => column.Key );
            foreach ( string key in orderCol )
            {
                if ( columns.Contains( key ) )
                {
                    this.DisplayLayout.Bands[0].Columns[key].Header.VisiblePosition = idx;
                }
                idx += 1;
            }

            return true;
        }

        /// <summary>
        /// 列幅取得
        /// </summary>
        /// <remarks>
        /// 列幅を取得します。
        /// </remarks>
        /// <returns></returns>
        public List<Int32> GetGridColmnWidth()
        {
            List<Int32> wkGridWidth = new List<Int32>();

            foreach ( UltraGridColumn aColumn in this.DisplayLayout.Bands[0].Columns )
            {
                wkGridWidth.Add( aColumn.Width );
            }
            return wkGridWidth;
        }

        /// <summary>
        /// 列幅設定
        /// </summary>
        /// <remarks>
        /// 列幅を設定します。
        /// </remarks>
        /// <param name="widthCol"></param>
        /// <returns></returns>
        public Boolean SetGridColmnWidth( List<Int32> widthCol )
        {
            if ( widthCol == null || widthCol.Count == 0 )
            {
                return true;
            }
            Int32 idx = 0;
            foreach ( var col in widthCol )
            {
                if ( idx >= this.DisplayLayout.Bands[0].Columns.Count )
                {
                    Singleton<LogManager>.Instance.WriteCommonLog(LogKind.DebugLog, String.Format("[CustomGrid] SetGridColmnWidthIn setting Column number did not match.ColumnCount={0},SetDataCount={1}", this.DisplayLayout.Bands[0].Columns.Count, widthCol.Count));
                    break;
                }
                //this.DisplayLayout.Bands[0].Columns[idx].Width = col;
                this.DisplayLayout.Bands[0].Columns[idx].RowLayoutColumnInfo.PreferredCellSize =
                    new Size( col, this.DisplayLayout.Bands[0].Columns[idx].RowLayoutColumnInfo.PreferredCellSize.Height );
                this.DisplayLayout.Bands[0].Columns[idx].Width = this.DisplayLayout.Bands[0].Columns[idx].RowLayoutColumnInfo.PreferredCellSize.Width;

                idx++;
            }

            return true;
        }

        /// <summary>
        /// フォントサイズ取得
        /// </summary>
        /// <remarks>
        /// グリッドのフォントサイズを返します。
        /// </remarks>
        /// <returns></returns>
        public float GetGridFontSize()
        {
            // フォントサイズは常に１００％時の値とする
            return this.defaultFontSize;
        }

        /// <summary>
        /// フォントサイズ設定
        /// </summary>
        /// <remarks>
        /// グリッドのフォントサイズを設定します
        /// </remarks>
        /// <param name="widthCol"></param>
        /// <returns></returns>
        public Boolean SetGridFontSize( float fontSize )
        {
            this.defaultFontSize = fontSize;    // ズームの計算のために、インスタンス変数へ退避
            this.DisplayLayout.Override.HeaderAppearance.FontData.SizeInPoints = fontSize;
            return true;
        }

        /// <summary>
        /// 行高さ取得
        /// </summary>
        /// <remarks>
        /// グリッドの行の高さを返します。
        /// </remarks>
        /// <returns></returns>
        public Int32 GetGridRowHeight()
        {
            return this.DisplayLayout.Bands[0].Override.DefaultRowHeight;
        }

        /// <summary>
        /// 行高さ設定
        /// </summary>
        /// <remarks>
        /// グリッドの行の高さを設定します。
        /// </remarks>
        /// <param name="heightRow">行高さ</param>
        /// <returns></returns>
        public Boolean SetGridRowHeight( Int32 heightRow )
        {
            this.DisplayLayout.Bands[0].Override.DefaultRowHeight = heightRow;

            return true;
        }

        #endregion

        #region [protectedメソッド]

        /// <summary>
        /// セルが起動された後に呼び出されます。
        /// </summary>
        /// <remarks>
        /// 基底クラスのOnAfterCellActivateを実行します。
        /// </remarks>
        protected override void OnAfterCellActivate()
        {
            base.OnAfterCellActivate();
        }
        
        /// <summary>
        /// 行初期化イベントオーバーライド
        /// </summary>
        /// <remarks>
        /// グリッドの行データが初期化される際のイベントです。
        /// 色ルール適用を行います。
        /// </remarks>
        /// <param name="e">初期化行情報</param>
        protected override void OnInitializeRow( Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e )
        {
            base.OnInitializeRow( e );
            // 背景色ルール適用
            // 途中行にインサートする動作は考慮しない、常に最後尾に追加されるものとする。
            if ( this.rowColorRule.ColorPatternList != null && this.rowColorRule.ColorPatternList.Count != 0 )
            {
                Int32 patternNo = 0;
                switch ( rowColorRule.ColorRuleKind )
                {
                case GridRowBackgroundColorRule.RuleKind.FromCellData:
                    if ( e.Row.Index == 0 )
                    {
                        this.lastRowColIndex = 0;
                        this.lastCells = this.DisplayLayout.Bands[0].Columns.OfType<UltraGridColumn>().ToDictionary( ( column ) => column.Key, ( column ) => e.Row.Cells[column] );
                    }
                    patternNo = this.getRowColorPattern( e.Row );
                    e.Row.Appearance.BackColor = this.rowColorRule.ColorPatternList[patternNo];
                    break;
                case GridRowBackgroundColorRule.RuleKind.FromIndex:
                    patternNo = ( e.Row.Index / this.rowColorRule.SetCount ) % this.rowColorRule.ColorPatternList.Count;
                    e.Row.Appearance.BackColor = this.rowColorRule.ColorPatternList[patternNo];

                    break;
                case GridRowBackgroundColorRule.RuleKind.FromRowData:
                    if ( e.Row.Index == 0 )
                    {
                        this.lastRowColIndex = 0;
                        this.lastRow = e.Row;
                    }
                    patternNo = this.getRowColorPatternRowData( e.Row );
                    e.Row.Appearance.BackColor = this.rowColorRule.ColorPatternList[patternNo];
                    break;
                default:
                    break;
                }
            }

            foreach ( var cell in e.Row.Cells )
            {
                if ( this.columnColorSetting.ContainsKey( cell.Column.Index ) )
                {
                    if ( !String.IsNullOrEmpty( cell.Text ) )
                    {
                        cell.Appearance.BackColor = this.columnColorSetting[cell.Column.Index].GetColor( Convert.ToInt64( cell.Value ) );
                    }
                    else
                    {
                        cell.Appearance.BackColor = this.columnColorSetting[cell.Column.Index].GetColor( 0 );
                    }
                    cell.Appearance.ForeColor = cell.Appearance.BackColor.GetBrightness() > 0.5 ? Color.Black : Color.White;
                }
            }
        }

        /// <summary>
        /// 行背景色インデックス取得
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="row"></param>
        /// <returns></returns>
        protected Int32 getRowColorPattern( UltraGridRow row )
        {
            Boolean isChanged = false;
            foreach ( var name in this.rowColorRule.SetColName )
            {
                if ( !isChanged )
                {
                    if ( this.lastCells.ContainsKey( name ) )
                    {
                        isChanged = this.lastCells[name].Text != row.Cells[name].Text;
                    }
                }
                this.lastCells[name] = row.Cells[name];
            }

            // 指定部全部が同じでなければ、変更ありとする。
            // 初回の比較は回避され、変化なしとなるので番号は0から開始する。
            if ( isChanged )
            {
                this.lastRowColIndex++;
            }

            return this.lastRowColIndex % this.rowColorRule.ColorPatternList.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">グリッド行</param>
        /// <returns></returns>
        protected Int32 getRowColorPatternRowData( UltraGridRow row )
        {
            // 初回の比較は回避され、変化なしとなるので番号は0から開始する。
            if ( 0 != this.gridRowDataCompare( this.lastRow.ListObject, row.ListObject ) )
            {
                this.lastRowColIndex++;
            }
            this.lastRow = row;
            return this.lastRowColIndex % this.rowColorRule.ColorPatternList.Count;
        }

        /// <summary>
        /// データソース設定後、レイアウトが最初に初期化されると呼び出されます。
        /// </summary>
        /// <remarks>
        /// データ型が日時でFormat未指定の列に対し、規定の日時フォーマットを設定します。
        /// </remarks>
        /// <param name="e"></param>
        protected override void FireInitializeLayout( InitializeLayoutEventArgs e )
        {
            foreach ( var band in this.DisplayLayout.Bands.OfType<UltraGridBand>() )
            {
                foreach ( var column in band.Columns.OfType<UltraGridColumn>() )
                {
                    // Format未設定DateTime型列にDateTimeの"G(一般の日付と時刻のパターン)"を設定
                    if ( String.IsNullOrEmpty( column.Format ) && ( column.DataType == typeof( DateTime ) || column.DataType == typeof( DateTime? ) ) )
                    {
                        //column.MaskInput = "{date} {longtime}";
                        column.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern + " " + DateTimeFormatInfo.CurrentInfo.LongTimePattern;
                    }
                }
            }
            base.FireInitializeLayout( e );
        }

        #endregion

        #region [privateメソッド]
        /// <summary>
        /// セル編集データエラーイベントハンドラ
        /// </summary>
        /// <remarks>
        /// ユーザーが無効な入力値でセルの編集モードを終了しようとすると呼び出されます。
        /// </remarks>
        /// <param name="e">イベントデータ</param>
        protected override void OnCellDataError( CellDataErrorEventArgs e )
        {
            base.OnCellDataError( e );
        }

        /// <summary>
        /// キー押下時イベントハンドラ
        /// </summary>
        /// <remarks>
        /// KeyDownイベントを呼び出す為に使用します。
        /// </remarks>
        /// <param name="e">イベントデータ</param>
        protected override void OnKeyDown( KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Enter && this.ActiveCell != null && this.ActiveCell.IsInEditMode )
            {
                this.ActiveRow.Selected = true;
                this.ActiveCell.Activated = false;
                return;
            }
            base.OnKeyDown( e );
        }

        #endregion

        #region [内部クラス]

        /// <summary>
        /// グリッド行背景色ルールクラス
        /// </summary>
        protected class GridRowBackgroundColorRule
        {
            #region [プロパティ]

            /// <summary>
            /// ルール種別の取得、設定
            /// </summary>
            public RuleKind ColorRuleKind
            {
                get;
                set;
            }

            /// <summary>
            /// 同色行単位(ルール種別：インデックス時)の取得、設定
            /// </summary>
            public Int32 SetCount
            {
                get;
                set;
            }

            /// <summary>
            /// 設定列名の取得、設定
            /// </summary>
            public List<String> SetColName
            {
                get;
                set;
            }

            /// <summary>
            /// 色パターンリストの取得、設定
            /// </summary>
            public List<Color> ColorPatternList
            {
                get;
                set;
            }

            #endregion

            #region [コンストラクタ/デストラクタ]
            
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public GridRowBackgroundColorRule()
            {
                this.ColorRuleKind = RuleKind.None;
                this.SetCount = 0;
                this.ColorPatternList = null;
                this.SetColName = new List<String>();
            }

            #endregion

            #region [内部クラス]

            /// <summary>
            /// ルール種別
            /// </summary>
            public enum RuleKind
            {
                /// <summary>
                /// なし
                /// </summary>
                None,
                /// <summary>
                /// インデックス基点
                /// </summary>
                FromIndex,
                /// <summary>
                /// セルデータ基点
                /// </summary>
                FromCellData,
                /// <summary>
                /// 行データ基点
                /// </summary>
                FromRowData
            }

            #endregion

        }

        #endregion

    }

    /// <summary>
    /// 色状態
    /// </summary>
    /// <remarks>
    /// 値の範囲と色のセットを保持します。
    /// </remarks>
    public class ColorStatus
    {
        #region [定数定義]

        /// <summary>
        /// デフォルト色
        /// </summary>
        private Color DEFAULT_COLOR = Color.White;

        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 値域リスト
        /// </summary>
        SortedList<Int64, Color> rangeList = new SortedList<Int64, Color>();

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// 色範囲設定追加
        /// </summary>
        /// <remarks>
        /// 値の下限と色を設定します、
        /// 設定された下限＜＝次に設定されている下限 で、指定色が有効となります。
        /// </remarks>
        /// <param name="rangeMin">色範囲下限</param>
        /// <param name="color">設定色</param>
        public void AddColorRangePair( Int64 rangeMin, Color color )
        {
            // 重複値でなければリスト追加する。
            if ( !this.rangeList.ContainsKey( rangeMin ) )
            {
                this.rangeList.Add( rangeMin, color );
            }
        }

        /// <summary>
        /// 指定値該当色取得
        /// </summary>
        /// <remarks>
        /// 指定値に該当する色を保持リストから検索し、取得します。
        /// 該当する色が無い場合、デフォルト色が取得されます。
        /// </remarks>
        /// <param name="value">指定値</param>
        /// <returns>指定値該当色</returns>
        public Color GetColor( Int64 value )
        {
            Color color = DEFAULT_COLOR;

            // 保持値域リストから条件検索、
            // 指定値より小さな値のリストを取得し、
            // 存在する場合は該当リストの最後尾を使用する。
            // "検索結果要素 <= 指定値 < 検索結果除外要素"で取れるようにする。
            IEnumerable<Color> selected = from valPair in this.rangeList
                                          where valPair.Key <= value
                                          orderby valPair.Key ascending
                                          select valPair.Value;
            if ( selected.Count() != 0 )
            {
                color = selected.Last();
            }

            return color;
        }

        #endregion

    }
}
