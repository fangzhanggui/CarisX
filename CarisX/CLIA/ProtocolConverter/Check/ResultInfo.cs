using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolConverter.Check
{
    /// <summary>
    /// コンバート処理結果クラス
    /// </summary>
    public class ResultInfo
    {


    /// <summary>
    /// 結果
    /// </summary>
    private bool result = true;

    /// <summary>
    /// データ
    /// </summary>
    private string data = string.Empty;



    /// <summary>
    /// 結果
    /// </summary>
    public bool Result
    {
        get
        {
            return this.result;
        }
        set
        {
            this.result = value;
        }
    }

    /// <summary>
    /// データ
    /// </summary>
    public string Data
    {
        get
        {
            return this.data;
        }
        set
        {
            this.data = value;
        }
    }
    }
}
