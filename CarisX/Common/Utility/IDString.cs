using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Oelco.Common.Utility
{

    /// <summary>
    /// ID文字列
    /// </summary>
    public abstract class IDString:IIDString// PrecharType : IIDString, new()
    {
        // protected IIDString precharType = null;// new PrecharType();
        //PrecharType precharType = new PrecharType();
        /// <summary>
        /// ID値
        /// </summary>
        private Int32 value = 0;
        /// <summary>
        /// ID値の取得
        /// </summary>
        public Int32 Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// 表示用ID文字列の取得、設定
        /// </summary>
        public String DispPreCharString
        {
            get
            {
                return String.Format( "{0}{1}", this.PreChar, this.value.ToString( this.StringFormat ) );
            }
            set
            {
                String removedPrechar = value;
                if ( this.PreChar != "" )
                {
                    removedPrechar = value.Replace( this.PreChar, "" );
                }
                
                Int32 parsedVal = 0;
                if ( removedPrechar.Length != 0 )
                {
                    Int32.TryParse( removedPrechar, out parsedVal );
                }
                this.value = parsedVal;
            }
        }
        /// <summary>
        /// 表示用ID値文字列の取得
        /// </summary>
        public String DispValueString
        {
            get
            {
                return String.Format( "{0}", this.value.ToString( this.StringFormat ) );
            }
            set
            {
                String removedPrechar = value.Replace( this.PreChar, "" );
                Int32 parsedVal = 0;
                Int32.TryParse( removedPrechar, out parsedVal );
                this.value = parsedVal;
            }
        }
        
        //public IDString( IIDString typeIF )
        //{
        //    this.precharType = typeIF;
        //}

        /// <summary>
        /// Int32への暗黙的変換
        /// </summary>
        /// <remarks>
        /// Int32へ暗黙的に型変換します。
        /// </remarks>
        /// <param name="precharId">ID文字列</param>
        /// <returns>ID値</returns>
        public static implicit operator Int32( IDString precharId )
        {
            return precharId.value;
        }

        /// <summary>
        /// 表示用ID文字列変換
        /// </summary>
        /// <remarks>
        /// 表示用ID文字列の文字列を返します。
        /// </remarks>
        /// <returns>表示用ID文字列</returns>
        public override String ToString()
        {
            if ( this.value > 0 )
            {
                return this.DispPreCharString;
            }
            return String.Empty;
        }

        //public static implicit operator IDString( Int32 value )
        //{
        //    var id = new IDString<PrecharType>();
        //    id.value = value;
        //    return id;
        //}
        //public static implicit operator IDString( String value )
        //{
        //    var id = new IDString<PrecharType>();
        //    id.DispPreCharString = value;
        //    return id;
        //}

        #region IIDString メンバー

        /// <summary>
        /// 文字列フォーマット
        /// </summary>
        public abstract String StringFormat
        {
            get;
        }

        /// <summary>
        /// 前置文字
        /// </summary>
        public abstract String PreChar
        {
            get;
        }


        #endregion
    }
    /// <summary>
    /// ID文字列インターフェース
    /// </summary>
    public interface IIDString
    {
        /// <summary>
        /// 前置文字
        /// </summary>
        String PreChar
        {
            get;
        }
        /// <summary>
        /// 文字列フォーマット
        /// </summary>
        String StringFormat
        {
            get;
        }
    }
}
