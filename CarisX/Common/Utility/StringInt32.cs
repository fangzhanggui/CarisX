using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oelco.Common.Utility
{
    public class StringInt32
    {
        private String fillChar = "";
        private Int32 paddingCount = 0;
        private Int32 value = 0;
        private String formatString = "";

        public String FormatString
        {
          get { return this.formatString; }
          set { this.formatString = value; }
        }

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
        public String String
        {
            get
            {
                return this.value.ToString( formatString );
            }
            set
            {
                analyze( this, value );
            }

        }
        //public String ToSting()
        //{
        //    return this.value.ToString(formatString);
        //}

        public static implicit operator Int32( StringInt32 precharId )
        {
            return precharId.value;
        }
        //public static implicit operator StringInt32( Int32 value )
        //{
        //    var id = new StringInt32();
        //    id.value = value;
        //    return id;
        //}
        public static implicit operator StringInt32( String value )
        {
            StringInt32 id = new StringInt32();
            analyze( id, value );
            return id;
        }

        static protected void analyze( StringInt32 target, String value )
        {
            //target = new StringInt32();

            // 変換失敗時は上位へ例外
            target.value = Int32.Parse( value );

            target.paddingCount = value.Length;

            switch ( value.First() )
            {
            case '0':
                target.fillChar = "0";
                break;
            case ' ':
                target.fillChar = " ";
                break;
            default :
                target.fillChar = "";
                break;
            }

            target.formatString = "";
            for ( Int32 i = 0; i < target.paddingCount; i++ )
            {
                target.formatString += target.fillChar;
            }

        }
    }
}
