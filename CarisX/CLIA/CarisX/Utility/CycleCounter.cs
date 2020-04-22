using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.CarisX.Utility
{

    /// <summary>
    /// 周期カウントクラス
    /// </summary>
    /// <remarks>
    /// 単純値による周期のカウントを行います。
    /// </remarks>
    class CycleCounter
    {

#region [定数定義]

#endregion

#region [クラス変数定義]

#endregion

#region [インスタンス変数定義]

        /// <summary>
        /// 周期設定
        /// </summary>
        Int64 cycle = 0;
        /// <summary>
        /// 値設定
        /// </summary>
        Int64 value = 0;

        /// <summary>
        /// 有効状態
        /// </summary>
        Boolean enable = true;
#endregion

#region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// クラスメンバの初期化を行います
        /// </remarks>
        /// <param name="cycle">周期設定値</param>
        public CycleCounter( Int64 cycle )
        {
            // サイクル設定
            this.cycle = cycle;
        }

#endregion

#region [プロパティ]

        /// <summary>
        /// 有効状態 取得/設定
        /// </summary>
        public Boolean Enable
        {
            get
            {
                return this.enable;
            }
            set
            {
                this.enable = value;
            }
        }
        /// <summary>
        /// 周期設定 取得/設定
        /// </summary>
        public Int64 Cycle
        {
            get
            {
                return this.cycle;
            }
            set
            {
                this.cycle = value;
            }
        }
#endregion

#region [publicメソッド]
        /// <summary>
        /// 周期値設定
        /// </summary>
        /// <remarks>
        /// 周期判定に使用する値に加算を行います。
        /// </remarks>
        /// <param name="value">加算値</param>
        public void AddValue( Int64 value )
        {
            if ( this.enable )
            {
                this.value += value;
            }
        }
        /// <summary>
        /// 周期判定にしようする値に代入を行います。
        /// </summary>
        /// <param name="value">代入値</param>
        public void SetCycle( Int64 value )
        {
            if ( this.enable )
            {
                this.value = value;
            }
        }
        /// <summary>
        /// 次周期取得
        /// </summary>
        /// <remarks>
        /// この関数の呼び出しにより、次の周期に入っているかを確認します。
        /// 周期を満たす場合、周期判定に利用する値から周期設定の値を減算し、TRUEを返します。
        /// 一度の呼び出しにより1周期に対して判定を行い、複数回の周期を満たしている場合、
        /// 内部カウンタはリセットされます。
        /// </remarks>
        /// <returns>True:周期を満たした False:周期を満たしていない</returns>
        public Boolean NextCycle()
        {
            Boolean isOverCycle = false;
            if ( this.enable )
            {
                if ( this.value >= this.cycle )
                {
                    this.value = 0;
                    isOverCycle = true;
                }
            }
            return isOverCycle;
        }
#endregion

#region [protectedメソッド]

#endregion

#region [privateメソッド]

#endregion
    }
}
