using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Oelco.Common.Utility;

namespace Oelco.Common.Parameter
{
    [Obsolete("XMLシリアライザでDictionaryやType型がシリアライズ出来ない為、使用見合わせ。")]
    public abstract class UISettingManager : ISavePath
    {
        private Dictionary<Type, Object> typeDic = new Dictionary<Type, Object>();

        protected Dictionary<Type, Object> TypeDic
        {
            get
            {
                return this.typeDic;
            }
            set
            {
                this.typeDic = value;
            }
        }

        private Dictionary<Type, Object> settingList = new Dictionary<Type, Object>();

        protected Dictionary<Type, Object> SettingList
        {
            get
            {
                return this.settingList;
            }
            set
            {
                this.settingList = value;
            }
        }             
 

        #region ISavePath メンバー

        public String SavePath
        {
            get
            {
                return "";
            }
        }

        #endregion
    }

  
}
