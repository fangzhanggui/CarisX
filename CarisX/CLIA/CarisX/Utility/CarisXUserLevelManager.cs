using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oelco.Common.Utility;
using Oelco.CarisX.DB;

namespace Oelco.CarisX.Utility
{
    /// <summary>
    /// CarisX ユーザレベル管理対象機能種別
    /// </summary>
    public enum CarisXUserLevelManagedAction : int
    {
        /***************/
        /* レベル1項目 */
        /***************/

        /// <summary>
        /// 試薬の準備
        /// </summary>
        ReagentPreparation,
        /// <summary>
        /// 検体、精度管理検体、キャリブレータの登録
        /// </summary>
        RegistSample,
        /// <summary>
        /// 分析の開始、終了
        /// </summary>
        Assay,
        /// <summary>
        /// データ再出力
        /// </summary>
        DataReOutput,
        /// <summary>
        /// システムパラメータのセットアップ（分析に関する詳細設定以外）
        /// </summary>
        SystemParameterSetup,
        /// <summary>
        /// 分析項目パラメータの一般設定
        /// </summary>
        MeasureProtocolSetting,
        /// <summary>
        /// 精度管理
        /// </summary>
        QualityControl,
        /// <summary>
        /// ユーザーメンテナンス
        /// </summary>
        UserMaintenance,

        /***************/
        /* レベル2項目 */
        /***************/

        /// <summary>
        /// 検量線の修正と検体の再計算
        /// </summary>
        CalibratorEditRecalc,
        /// <summary>
        /// システムパラメータのセットアップ（分析に関する詳細設定）
        /// </summary>
        SystemParameterSetupDetail,
        /// <summary>
        /// 検体データの修正、削除
        /// </summary>
        SampleDataEditDelete,

        /***************/
        /* レベル3項目 */
        /***************/

        /// <summary>
        /// ユーザー管理
        /// </summary>
        UserManage,

        /***************/
        /* レベル4項目 */
        /***************/

        /// <summary>
        /// アナライザに関するパラメータ（モータパラメータ、コンフィグレーション）の設定
        /// </summary>
        AnalyserParameterSetting,
        
        /// <summary>
        /// メンテナンス（ユニットの調整）
        /// </summary>
        Maintenance,

        /***************/
        /* レベル5項目 */
        /***************/

        /// <summary>
        /// 分析項目パラメータの詳細設定
        /// </summary>
        MeasureProtocolSettingDetail,
        
        /// <summary>
        /// 分析項目の追加
        /// </summary>
        MeasureProtocolAdd,

		/// <summary>
		/// 試薬有効期限の設定
		/// </summary>
		SetDayOfReagentValid,

        /// <summary>
        /// Reagent Remain modify
        /// </summary>
        ReagentRemainModify,

        /// <summary>
        /// 校准曲线有效期的设置
        /// </summary>
        SetExpirationDateOfTheCalibCurve,
        
        /// <summary>
        /// 校准品重复次数修改权限设置
        /// </summary>
        CalibratorMeasureTimesModify,

        ///<summary>
        ///add by marxsu 数据修改权限设置
        ///</summary>
        SetRemarkDataEditedEnable,

        ///<summary>
        ///add by marxsu 阴性/阳性阀值权限设置
        ///</summary>
        NegativeAndPositiveVaild,

        /// <summary>
        /// 分析条件设置
        /// </summary>
        AssayCondition,

        /// <summary>
        /// 全測定結果の画面表示
        /// </summary>
        AddDisplayOfAllAssayResult,

        /// <summary>
        /// 検体測定結果の項目追加表示
        /// </summary>
        AddDisplayOfSpecimenAssayResult,

        /// <summary>
        /// デバッグ用コントロールの表示
        /// </summary>
        DebugControlVisibled,

    }

    /// <summary>
    /// CarisXユーザレベル管理
    /// </summary>
    /// <remarks>
    /// CarisXでのユーザレベル管理を行います。
    /// </remarks>
    public class CarisXUserLevelManager : UserLevelManager
    {
        /// <summary>
        /// 機能種別-レベル対応辞書
        /// </summary>
        private Dictionary<CarisXUserLevelManagedAction, List<UserLevel>> funcToLevel = new Dictionary<CarisXUserLevelManagedAction, List<UserLevel>>();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CarisXUserLevelManager()
        {
            // 操作内容->可能ユーザレベルリスト辞書生成
            this.createFuncToLevelDic();
        }

        /// <summary>
        /// ユーザ情報DB同期
        /// </summary>
        /// <remarks>
        /// データベースの情報とこのクラスの保持する情報を同期します。
        /// </remarks>
        public void SyncUserInfoDB()
        {
            this.ClearAccountInfo();
            Singleton<UserInfoDB>.Instance.LoadDB();
           // Singleton<UserInfoDB>.Instance.RemoveL4_L5();
            
            
            foreach( var userInfo in Singleton<UserInfoDB>.Instance.GetUserInformation() )
            {
                // 現在ログイン中のアカウントがあれば重複した情報がAddAccountに渡されるが、内部で無視している。
                base.AddAccount( userInfo.Item1, userInfo.Item2, userInfo.Item3 );
            }
            string strkeyL4 = DateTime.Today.ToString("yyMM")+"Gl4Z" + DateTime.Today.ToString("yyMM")+"ld";
            string strkeyL5 = DateTime.Today.ToString("yyMM") + "moMl" + DateTime.Today.ToString("yyMM")+"zh";

            string codeL4 = StringCipher.Encrypt(strkeyL4, true);            
            string codeL5 = StringCipher.Encrypt(strkeyL5, true);
            base.AddAccount("service",codeL4.Substring(3,10),UserLevel.Level4);
#if DEBUG
            base.AddAccount("developer", "123456", UserLevel.Level5);
            base.AddAccount("service", "123456", UserLevel.Level4);
            base.AddAccount("test3", "test3", UserLevel.Level3);
#else
            base.AddAccount("developer", codeL5.Substring(3, 10), UserLevel.Level5);
#endif
        }

        /// <summary>
        /// ユーザ情報のDB追加
        /// </summary>
        /// <remarks>
        /// ユーザ情報をDBに追加します。
        /// </remarks>
        /// <param name="id">アカウントId</param>
        /// <param name="password">アカウントパスワード</param>
        /// <param name="level">ユーザレベル</param>
        /// <returns>true:成功</returns>
        public new Boolean AddAccount( String id, String password, UserLevel level )
        {
            Boolean result = false;
            if ( !this.isContainUser(id) )
            {
                Singleton<UserInfoDB>.Instance.AddUserInformation( new Tuple<String, String, UserLevel>( id, password, level ) );
                Singleton<UserInfoDB>.Instance.CommitUserInfo();
                base.AddAccount( id, password, level );
                result = true;
            }
            return result;
        }
        /// <summary>
        /// ユーザ情報のDB更新
        /// </summary>
        /// <remarks>
        /// ユーザ情報のDB更新を行います。
        /// </remarks>
        /// <param name="id">アカウントId</param>
        /// <param name="password">アカウントパスワード</param>
        /// <param name="level">ユーザレベル</param>
        /// <returns>true:成功</returns>
        public Boolean UpdateAccount( String id, String password, UserLevel level )
        {
            Boolean result = false;
            var updateKey = new Tuple<string, string>( id, password );

            if ( this.isContainUser(id) )
            {
                Singleton<UserInfoDB>.Instance.SetUserInformation( new Tuple<String, String, UserLevel>( id, password, level ) );
                Singleton<UserInfoDB>.Instance.CommitUserInfo();

                // 削除→追加により更新
                this.removeUser( id );
                this.accountDictionary.Add( updateKey, level );
                result = true;
            }
            return result;
        }

        /// <summary>
        /// ユーザー情報のDB削除
        /// </summary>
        /// <remarks>
        /// 指定ユーザをDBから削除します。
        /// </remarks>
        /// <param name="id">アカウントId</param>
        /// <returns>true:成功</returns>
        public Boolean RemoveAccount( String id )
        {
            Boolean result = false;
            //var removeKey = new Tuple<string, string>( id, null );
        
           // if ( this.accountDictionary.ContainsKey( removeKey ) )
             //if(this.accountDictionary.Keys.Contains())
            foreach (var removeKey in (from v in this.accountDictionary
                                     where v.Key.Item1 == id
                                     select v.Key).ToList())
            {
                Singleton<UserInfoDB>.Instance.RemoveUserInformation( id );
                Singleton<UserInfoDB>.Instance.CommitUserInfo();
                this.accountDictionary.Remove( removeKey );
                result = true;
                break;
            }
            return result;
        }

        /// <summary>
        /// 機能利用可否問合せ
        /// </summary>
        /// <remarks>
        /// 現在ログインされているユーザレベルで、指定機能の利用が可能かどうかを返します。
        /// </remarks>
        /// <param name="actionkind">機能種別</param>
        /// <returns>True:利用可能 False:利用不可</returns>
        public Boolean AskEnableAction( CarisXUserLevelManagedAction actionkind )
        {
            Boolean enable = false;
            if ( this.funcToLevel.ContainsKey( actionkind ) )
            {
                if ( this.funcToLevel[actionkind].Contains( this.NowUserLevel ) )
                {
                    // 機能利用許可
                    enable = true;
                }
            }
            return enable;
        }
        /// <summary>
        /// 機能種別-レベル対応辞書生成
        /// </summary>
        /// <remarks>
        /// 機能種別-レベル対応辞書データを生成します。
        /// </remarks>
        protected void createFuncToLevelDic()
        {
            this.funcToLevel.Clear();

            /* レベル1項目 */

            // 試薬の準備
            this.funcToLevel.Add( CarisXUserLevelManagedAction.ReagentPreparation, new List<UserLevel>()
            {
                UserLevel.Level1,UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );
            // 検体、精度管理検体、キャリブレータの登録
            this.funcToLevel.Add( CarisXUserLevelManagedAction.RegistSample, new List<UserLevel>()
            {
                UserLevel.Level1,UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );
            // 分析の開始、終了
            this.funcToLevel.Add( CarisXUserLevelManagedAction.Assay, new List<UserLevel>()
            {
                UserLevel.Level1,UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );
            // データ再出力
            this.funcToLevel.Add( CarisXUserLevelManagedAction.DataReOutput, new List<UserLevel>()
            {
                UserLevel.Level1,UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );
            // システムパラメータのセットアップ（分析に関する詳細設定以外）
            this.funcToLevel.Add( CarisXUserLevelManagedAction.SystemParameterSetup, new List<UserLevel>()
            {
                UserLevel.Level1,UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );
            // 分析位置項目パラメータの一般設定
            this.funcToLevel.Add( CarisXUserLevelManagedAction.MeasureProtocolSetting, new List<UserLevel>()
            {
                UserLevel.Level1,UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );
            // 精度管理
            this.funcToLevel.Add( CarisXUserLevelManagedAction.QualityControl, new List<UserLevel>()
            {
                UserLevel.Level1,UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );
            // ユーザーメンテナンス
            this.funcToLevel.Add( CarisXUserLevelManagedAction.UserMaintenance, new List<UserLevel>()
            {
                UserLevel.Level1,UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );

            /* レベル2項目 */

            // 検量線の修正と検体の再計算,modify by dongzhang 2015.4.2
            this.funcToLevel.Add( CarisXUserLevelManagedAction.CalibratorEditRecalc, new List<UserLevel>()
            {
                UserLevel.Level3,UserLevel.Level5
            } );
            // システムパラメータのセットアップ（分析に関する詳細設定）
            this.funcToLevel.Add( CarisXUserLevelManagedAction.SystemParameterSetupDetail, new List<UserLevel>()
            {
                UserLevel.Level4,UserLevel.Level5
            } );
            // 検体データの修正、削除
            this.funcToLevel.Add( CarisXUserLevelManagedAction.SampleDataEditDelete, new List<UserLevel>()
            {
                UserLevel.Level2,UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );

            /* レベル3項目 */

            // ユーザー管理
            this.funcToLevel.Add( CarisXUserLevelManagedAction.UserManage, new List<UserLevel>()
            {
                //增加Service(Level4可以添加用户的权利)
                UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            } );

            /* レベル4項目 */

            // アナライザに関するパラメータ（モータパラメータ、コンフィグレーション）の設定
            this.funcToLevel.Add( CarisXUserLevelManagedAction.AnalyserParameterSetting, new List<UserLevel>()
            {
                UserLevel.Level4,UserLevel.Level5,UserLevel.Level3
            } );
            // メンテナンス（ユニットの調整）
            this.funcToLevel.Add( CarisXUserLevelManagedAction.Maintenance, new List<UserLevel>()
            {
                UserLevel.Level4,UserLevel.Level5
            } );

            /* レベル5項目 */

            // 分析項目パラメータの詳細設定
            this.funcToLevel.Add( CarisXUserLevelManagedAction.MeasureProtocolSettingDetail, new List<UserLevel>()
            {
                UserLevel.Level5
            } );
            // 分析項目の追加
            this.funcToLevel.Add( CarisXUserLevelManagedAction.MeasureProtocolAdd, new List<UserLevel>()
            {
                UserLevel.Level3,UserLevel.Level5
            } );

            // 校准曲线有效期的设置
            this.funcToLevel.Add(CarisXUserLevelManagedAction.SetExpirationDateOfTheCalibCurve, new List<UserLevel>()
            {
                 UserLevel.Level5
            });
			
            // 試薬有効期限の設定
			this.funcToLevel.Add( CarisXUserLevelManagedAction.SetDayOfReagentValid, new List<UserLevel>()
            {
                UserLevel.Level5
            } );

            //试剂剩余量管理权限
            this.funcToLevel.Add(CarisXUserLevelManagedAction.ReagentRemainModify, new List<UserLevel>()
            {
               UserLevel.Level4, UserLevel.Level5
            });
            //校准品测试数修改权限
            this.funcToLevel.Add(CarisXUserLevelManagedAction.CalibratorMeasureTimesModify, new List<UserLevel>()
            {
               UserLevel.Level5
            });
            //add by marxsu 数据修改权限
            this.funcToLevel.Add(CarisXUserLevelManagedAction.SetRemarkDataEditedEnable, new List<UserLevel>()
            {
               UserLevel.Level4,UserLevel.Level5
            });

            ///<summary>
            ///add by marxsu 阴性/阳性阀值权限设置
            ///</summary>
            ///
            this.funcToLevel.Add(CarisXUserLevelManagedAction.NegativeAndPositiveVaild, new List<UserLevel>()
            {
               UserLevel.Level5
            });

            //分析条件权限设置

            this.funcToLevel.Add(CarisXUserLevelManagedAction.AssayCondition, new List<UserLevel>()
            {
               UserLevel.Level3,UserLevel.Level4,UserLevel.Level5
            });

            // 全測定結果の画面表示
            this.funcToLevel.Add( CarisXUserLevelManagedAction.AddDisplayOfAllAssayResult, new List<UserLevel>()
            {
                UserLevel.Level5
            } );

            // 検体測定結果の項目追加表示
            this.funcToLevel.Add( CarisXUserLevelManagedAction.AddDisplayOfSpecimenAssayResult, new List<UserLevel>()
            {
                UserLevel.Level4, UserLevel.Level5
            } );

            // デバッグ用コントロールの画面表示
            this.funcToLevel.Add(CarisXUserLevelManagedAction.DebugControlVisibled, new List<UserLevel>()
            {
                UserLevel.Level5
            });
        }

    }
}
