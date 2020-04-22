using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Parameter;
using Oelco.CarisX.Const;

namespace Oelco.CarisX.Parameter
{
    /// <summary>
    /// 精度管理検体情報データクラス
    /// </summary>
    public class ControlQCData
    {
        /// <summary>
        /// 全て
        /// </summary>
        public const String ALL = "";

        /// <summary>
        /// 分析項目インデックス
        /// </summary>
        private Int32 measureProtocolIndex;
        /// <summary>
        /// 精度管理検体名
        /// </summary>
        private String controlName;
        /// <summary>
        /// 精度管理検体ロット番号
        /// </summary>
        private String controlLotNo;
        /// <summary>
        /// 管理平均値
        /// </summary>
        private Double? mean;
        /// <summary>
        /// 管理濃度値
        /// </summary>
        private Double? concentrationWidth;
        /// <summary>
        /// 管理R値
        /// </summary>
        private Double? controlR;

        /// <summary>
        /// 分析項目インデックスの取得、設定
        /// </summary>
        public Int32 MeasureProtocolIndex
        {
            get
            {
                return measureProtocolIndex;
            }
            set
            {
                measureProtocolIndex = value;
            }
        }

        /// <summary>
        /// 精度管理検体名の取得、設定
        /// </summary>
        public String ControlName
        {
            get
            {
                return controlName;
            }
            set
            {
                controlName = value;
            }
        }

        /// <summary>
        /// 精度管理検体ロット番号の取得、設定
        /// </summary>
        public String ControlLotNo
        {
            get
            {
                return controlLotNo;
            }
            set
            {
                controlLotNo = value;
            }
        }

        /// <summary>
        /// 管理平均値の取得、設定
        /// </summary>
        public Double? Mean
        {
            get
            {
                return mean;
            }
            set
            {
                mean = value;
            }
        }

        /// <summary>
        /// 管理濃度値の取得、設定
        /// </summary>
        public Double? ConcentrationWidth
        {
            get
            {
                return concentrationWidth;
            }
            set
            {
                concentrationWidth = value;
            }
        }

        /// <summary>
        /// 管理R値の取得、設定
        /// </summary>
        public Double? ControlR
        {
            get
            {
                return controlR;
            }
            set
            {
                controlR = value;
            }
        }
    }

    /// <summary>
    /// 精度管理情報クラス
    /// </summary>
    public class ControlQC : ISavePath
    {
        /// <summary>
        /// 精度管理情報データリスト
        /// </summary>
        private List<ControlQCData> controlQCList = new List<ControlQCData>();
        /// <summary>
        /// 保存フォルダパス
        /// </summary>
        private String folderPath = CarisXConst.PathSystem;
        /// <summary>
        /// XML保存パス
        /// </summary>
        private String savePath = CarisXConst.PathSystem + @"\ControlQC.xml";
        /// <summary>
        /// デフォルトのパス
        /// </summary>
        private String defaultFolderPath = CarisXConst.PathSystem;

        /// <summary>
        /// 精度管理情報データリストの取得、設定
        /// </summary>
        public List<ControlQCData> ControlQCList
        {
            get
            {
                return controlQCList;
            }
            set
            {
                controlQCList = value;
            }
        }

        /// <summary>
        /// 保存フォルダパスの取得、設定
        /// </summary>
		public String FolderPath
		{
			get
			{
				return this.folderPath;
			}
			set
			{
				this.folderPath = value;
			}
		}

        /// <summary>
        /// 保存ファイルパス
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public String SavePath
        {
            get
            {
                return savePath;
            }
            set
            {
                savePath = value;
            }
        }

        /// <summary>
        /// 保存パスをデフォルトのパスに戻す
        /// </summary>
        /// <remarks>
        /// 保存パスをデフォルトのパスに戻します
        /// </remarks>
        public void SetDefaultSavePath()
        {
            this.FolderPath = this.defaultFolderPath;
        }
    }
}

