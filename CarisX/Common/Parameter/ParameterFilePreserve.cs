#define XML_ENCRYPT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Xml.Serialization;
using Oelco.Common.Utility;
using System.Xml;

namespace Oelco.Common.Parameter
{
    /// <summary>
    /// ファイル保存パラメータ
    /// </summary>
    /// <remarks>
    /// Xmlファイルとしてシリアライズされるパラメータクラスへの共通操作を提供します。
    /// </remarks>
    public class ParameterFilePreserve<TypeT> : IPreserveParameter where TypeT : ISavePath, new()
    {
        #region [クラス変数定義]

        /// <summary>
        /// 暗号化キー
        /// </summary>
        // TODO:CarisXとしたいが旧プロトコルファイルが読み込めないため、暫定処置
        //protected const String ENCRYPT_PASSWORD = "CarisX";
        protected const String ENCRYPT_PASSWORD = "Caris200";
        
        #endregion

        #region [インスタンス変数定義]

        /// <summary>
        /// 現在のパラメータ
        /// </summary>
        TypeT paramObject = default( TypeT );
        /// <summary>
        /// 元のパラメータ
        /// </summary>
        TypeT originalParamObject = default( TypeT );

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ParameterFilePreserve()
        {
            paramObject = new TypeT();
            originalParamObject = new TypeT();
        }

        #endregion

        #region [プロパティ]

        /// <summary>
        /// 現在のパラメータの取得
        /// </summary>
        public TypeT Param
        {
            get
            {
                return this.paramObject;
            }
        }
        /// <summary>
        /// 元のパラメータの取得
        /// </summary>
        public TypeT OriginalParam
        {
            get
            {
                return this.originalParamObject;
            }
        }

        ///// <summary>
        ///// 保存パス
        ///// </summary>
        //protected abstract String FilePath
        //{
        //    get;
        //}
        //private String filePath;
        //public String FilePath
        //{
        //    get;
        //    set;
        //}

        #endregion

        #region [publicメソッド]

        /// <summary>
        /// パラメータ保存(非暗号化)
        /// </summary>
        /// <remarks>
        /// このクラスのシリアライズを暗号化をせずに行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean SaveRaw()
        {
            Boolean saveSuccess = this.saveRaw( paramObject.SavePath );
            return saveSuccess;
        }

        /// <summary>
        /// パラメータ読込(非復号化)
        /// </summary>
        /// <remarks>
        /// このクラスのデシリアライズを復号化せずに行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean LoadRaw()
        {
            Boolean loadSuccess = this.loadRaw( paramObject.SavePath );
            return loadSuccess;
        }

        /// <summary>
        /// パラメータ保存
        /// </summary>
        /// <remarks>
        /// このクラスのシリアライズを行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean Save()
        {
            Boolean saveSuccess = this.saveWithNoEncryption( paramObject.SavePath );
            return saveSuccess;
        }
        public Boolean SaveEncryption()
        {
            Boolean saveSuccess = this.saveWithEncryption(paramObject.SavePath);
            return saveSuccess;
        }
        /// <summary>
        /// パラメータ読込
        /// </summary>
        /// <remarks>
        /// このクラスのデシリアライズを行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean Load()
        {
            Boolean loadSuccess = this.loadWithNoEncryption( paramObject.SavePath );
            return loadSuccess;
        }
        public Boolean LoadEncryption()
        {
            Boolean loadSuccess = this.loadWithEncryption(paramObject.SavePath);
            if (!loadSuccess)
                //復号化して読み込めない場合、そのまま読み込みする
                loadSuccess = this.loadWithNoEncryption(paramObject.SavePath);
            return loadSuccess;
        }

        #endregion


        protected Boolean saveWithNoEncryption(String path)
        {
            Boolean serializeResult = false;
            try
            {
                XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(this.paramObject.GetType());
                File.Delete(path);
                using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.CreateNew)))
                {
                    serializer.Serialize(writer, this.paramObject);
                }


                using (Stream reader = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(reader))
                {
                    Object loadedObjClone = null;
                    loadedObjClone = serializer.Deserialize(sr);
                    this.originalParamObject = (TypeT)loadedObjClone;
                }
                serializeResult = true;
            }
            catch (Exception ex)
            {
                // ファイル削除失敗か、作成失敗(非Debugでも出力する。)
                System.Console.WriteLine(String.Format("{0}:{1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message));
                System.Diagnostics.Debug.WriteLine(String.Format("{0}:{1} {2}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message, ex.StackTrace));
            }
            return serializeResult;

        }

        #region [protectedメソッド]

        /// <summary>
        /// 暗号化ファイル保存
        /// </summary>
        /// <remarks>
        /// 保持するパラメータを、暗号化して指定パスに保存します。
        /// </remarks>
        /// <param name="path">保存パス</param>
        /// <returns>True:成功 False:失敗</returns>
        protected Boolean saveWithEncryption( String path )
        {
            Boolean serializeResult = false;

            //// 通常のXmlSerializerでは、パブリックメンバしかシリアライズ対象とされない為、
            //// SOAPを使う→メンテナンスツールとの連携がしにくいのでXmlSerializerを使う
            //SoapFormatter serializer = new SoapFormatter();// this.GetType() );
            try
            {
               

                XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer( this.paramObject.GetType() );
                File.Delete( path );
                Object loadedObj = null;
                Object loadedObjClone = null;
                XmlCipher chipher = new XmlCipher( ENCRYPT_PASSWORD );

                using (StreamWriter writer = new StreamWriter( new MemoryStream() ))
                {
                    serializer.Serialize( writer, this.paramObject );
                    chipher.EncryptFile( writer.BaseStream, path );
                }


                
                //using (Stream reader = new MemoryStream())
                {
                    Stream reader = new MemoryStream();
                    XmlCipher chipherReader = new XmlCipher( ENCRYPT_PASSWORD );
                    chipherReader.DecryptFile( path, ref reader );

                    loadedObj = getObjectFromStreamData( reader );
                    loadedObjClone = getObjectFromStreamData( reader );
                    this.originalParamObject = (TypeT)loadedObjClone;
                    reader.Close();
                    reader.Dispose();

                }

                serializeResult = true;

                //                chipher.EncryptFile( , path );

                //using ( FileStream fs = new FileStream( path, FileMode.CreateNew ) )
                //{
                //    try
                //    {
                //        serializer.Serialize( fs, this );
                //        serializeResult = true;
                //    }
                //    catch ( Exception ex )
                //    {
                //        // 子クラスにSerialize属性が無いか、ディスク容量不足や消失。
                //        System.Diagnostics.Debug.WriteLine( ex.Message );

                //    }
                //}
            }
            catch ( Exception ex )
            {
                // ファイル削除失敗か、作成失敗(非Debugでも出力する。)
                System.Console.WriteLine( String.Format( "{0}:{1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message ) );
                System.Diagnostics.Debug.WriteLine(String.Format("{0}:{1} {2}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message, ex.StackTrace));
            }

            return serializeResult;
        }

        /// <summary>
        /// ストリームからXMLデシリアライズ結果を取得
        /// </summary>
        /// <remarks>
        /// ストリームからXML形式でデシリアライズし、取得したオブジェクトを返します。
        /// </remarks>
        /// <param name="stream">デシリアライズ対象ストリーム</param>
        /// <returns>取得オブジェクト</returns>
        protected Object getObjectFromStreamData( Stream stream )
        {
            Object loadedObject = null;
            // ファイルポインタの位置を戻す
            stream.Seek( 0, SeekOrigin.Begin );
            //using ( StreamReader sr = new StreamReader( reader ) )
            XmlReaderSettings readerSettings = new XmlReaderSettings();

            // XMLファイルサイズ制限を無効化する
            readerSettings.MaxCharactersFromEntities = 0;
            readerSettings.MaxCharactersInDocument = 0;

            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer( this.paramObject.GetType() );

            using ( XmlReader sr = XmlReader.Create( stream, readerSettings ) )
            {
                //sr.ReadToEnd();
                try
                {
                    sr.Read();
                    loadedObject = serializer.Deserialize( sr );
                }
                catch ( Exception ex )
                {
                    // 形式違反
                    System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
                    System.Diagnostics.Debug.WriteLine(ex.InnerException);
                }
                finally
                {
                    sr.Close();
                }
            }

            return loadedObject;
        }

        protected Boolean loadWithNoEncryption(String path)
        {
            Boolean deserializeResult = false;

            try
            {
                Object loadedObj = null;
                Object loadedObjClone = null;
                using (Stream reader = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    loadedObj = getObjectFromStreamData(reader);
                    loadedObjClone = getObjectFromStreamData(reader);
                }
                if (loadedObj is TypeT)
                {
                    this.paramObject = (TypeT)loadedObj;
                    this.originalParamObject = (TypeT)loadedObjClone;
                    deserializeResult = true;
                }

            }
            catch (Exception ex)
            {
                // ファイルが無い、アクセスできない。
                System.Diagnostics.Debug.WriteLine(String.Format("ファイル：{0} \n {1} {2}", path, ex.Message, ex.StackTrace));
            }


            return deserializeResult;
        }

        /// <summary>
        /// 暗号化ファイル読込
        /// </summary>
        /// <remarks>
        /// 暗号化ファイルを、指定パスからパラメータに読込ます。
        /// </remarks>
        /// <param name="path">保存パス</param>
        /// <returns>True:成功 False:失敗</returns>
        protected Boolean loadWithEncryption( String path )
        {
            Boolean deserializeResult = false;

            //            SoapFormatter serializer = new SoapFormatter();
            try
            {
                Object loadedObj = null;
                Object loadedObjClone = null;
        
                //using (  Stream reader = new MemoryStream() )
                {
                    Stream reader = new MemoryStream();
                    XmlCipher chipher = new XmlCipher( ENCRYPT_PASSWORD );
                    chipher.DecryptFile( path, ref reader );

                    loadedObj = getObjectFromStreamData( reader );
                    loadedObjClone = getObjectFromStreamData( reader );
                    reader.Close();
                    reader.Dispose();
                }
                if ( loadedObj is TypeT )
                {
                    this.paramObject = (TypeT)loadedObj;
                    this.originalParamObject = (TypeT)loadedObjClone;
                    deserializeResult = true;
                }

//                XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer( this.paramObject.GetType() );
//                Object loadedObj = null;
//                Object loadedObjClone = null;
//#if XML_ENCRYPT
//                Stream reader = new MemoryStream();
//                XmlCipher chipher = new XmlCipher( ENCRYPT_PASSWORD );
//                chipher.DecryptFile( path, ref reader );
                
//                try
//                {
//                    loadedObj = serializer.Deserialize( reader );
//                }
//                catch ( Exception ex )
//                {
//                    // 形式違反
//                    System.Diagnostics.Debug.WriteLine( ex.Message );
//                }
//                finally
//                {
//                    reader.Close();
//                    reader.Dispose();
//                }

//                if ( loadedObj is TypeT )
//                {
//                    this.paramObject = (TypeT)loadedObj;
//                }
//#else
//                Stream reader = new FileStream( path, FileMode.Open, FileAccess.Read );
//                //using ( StreamReader sr = new StreamReader( reader ) )
//                XmlReaderSettings readerSettings = new XmlReaderSettings();
//                readerSettings.MaxCharactersFromEntities = 0;
//                readerSettings.MaxCharactersInDocument = 0;
//                using ( XmlReader sr = XmlReader.Create( reader, readerSettings ) )
//                {
//                    //sr.ReadToEnd();
//                    try
//                    {
//                        sr.Read();
//                        loadedObj = serializer.Deserialize( sr );
////                        sr.BaseStream.Seek( 0, SeekOrigin.Begin );
//                        sr.MoveToFirstAttribute();
//                        loadedObjClone = serializer.Deserialize( sr );
//                    }
//                    catch ( Exception ex )
//                    {
//                        // 形式違反
//                        System.Diagnostics.Debug.WriteLine( ex.Message );
//                        System.Diagnostics.Debug.WriteLine( ex.InnerException );
//                    }
//                    finally
//                    {
//                        sr.Close();
//                    }

//                    if ( loadedObj is TypeT )
//                    {
//                        this.paramObject = (TypeT)loadedObj;
//                        this.originalParamObject = (TypeT)loadedObjClone;
//                        deserializeResult = true;
//                    }
//                }
//#endif


                // thisのフィールドに読込んだオブジェクトの値を代入する。
                //if ( this.setThis( ref loadedObj ) )
                //{
                //    deserializeResult = true;
                //}


                //using ( var fs = new System.IO.FileStream( path, System.IO.FileMode.Open ) )
                //{
                //    Object loadedObj = null;
                //    try
                //    {
                //        loadedObj = serializer.Deserialize( fs );
                //    }
                //    catch ( Exception ex )
                //    {
                //        // 形式違反
                //        System.Diagnostics.Debug.WriteLine( ex.Message );
                //    }

                //    // thisのフィールドに読込んだオブジェクトの値を代入する。
                //    if ( this.setThis( ref loadedObj ) )
                //    {
                //        deserializeResult = true;
                //    }
                //}
            }
            catch ( Exception ex )
            {
                // ファイルが無い、アクセスできない。
                System.Diagnostics.Debug.WriteLine(String.Format("ファイル：{0} \n {1} {2}", path, ex.Message, ex.StackTrace));
            }
            return deserializeResult;
        }

        /// <summary>
        /// ファイル保存
        /// </summary>
        /// <remarks>
        /// 保持するパラメータを、指定パスに保存します。
        /// </remarks>
        /// <param name="path">保存パス</param>
        /// <returns>True:成功 False:失敗</returns>
        protected Boolean saveRaw( String path )
        {
            Boolean serializeResult = false;

            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer( this.paramObject.GetType() );
            try
            {
                File.Delete( path );


                using ( StreamWriter writer = new StreamWriter( new FileStream( path, FileMode.CreateNew ) ) )
                {
                    serializer.Serialize( writer, this.paramObject );
                }
                
                using ( Stream reader = new FileStream( path, FileMode.Open, FileAccess.Read ) )
                using ( StreamReader sr = new StreamReader( reader ) )
                {
                    Object loadedObjClone = null;
                    loadedObjClone = serializer.Deserialize( sr );
                    this.originalParamObject = (TypeT)loadedObjClone;
                }
                serializeResult = true;
            }
            catch ( Exception ex )
            {
                // ファイル削除失敗か、作成失敗
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
            }

            return serializeResult;
        }

        /// <summary>
        /// ファイル読込
        /// </summary>
        /// <remarks>
        /// ファイルを、指定パスからパラメータに読込ます。
        /// </remarks>
        /// <param name="path">保存パス</param>
        /// <returns>True:成功 False:失敗</returns>
        protected Boolean loadRaw( String path )
        {
            Boolean deserializeResult = false;

            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer( this.paramObject.GetType() );
            try
            {
                Object loadedObj = null;
                Object loadedObjClone = null;

                using ( Stream reader = new FileStream( path, FileMode.Open, FileAccess.Read ) )
                {
                    loadedObj = getObjectFromStreamData( reader );
                    loadedObjClone = getObjectFromStreamData( reader );
                }

                if ( loadedObj is TypeT )
                {
                    this.paramObject = (TypeT)loadedObj;
                    this.originalParamObject = (TypeT)loadedObjClone;
                    deserializeResult = true;
                }

                //Stream reader = new FileStream( path, FileMode.Open, FileAccess.Read );

                ////using ( StreamReader sr = new StreamReader( reader ) )
                //using ( XmlTextReader sr = new XmlTextReader( reader ) )
                //{
                //    //sr.ReadToEnd();
                //    try
                //    {
                //        sr.Settings.MaxCharactersFromEntities = 0;
                //        sr.Settings.MaxCharactersInDocument = 0;
                //        loadedObj = serializer.Deserialize( sr );
                //        //sr.BaseStream.Seek( 0, SeekOrigin.Begin );
                //        loadedObjClone = serializer.Deserialize( sr );
                //    }
                //    catch ( Exception ex )
                //    {
                //        // 形式違反
                //        System.Diagnostics.Debug.WriteLine( ex.Message );
                //    }
                //    finally
                //    {
                //        reader.Close();
                //        reader.Dispose();
                //    }
                //}

                //if ( loadedObj is TypeT )
                //{
                //    this.paramObject = (TypeT)loadedObj;
                //    this.originalParamObject = (TypeT)loadedObjClone;
                //    deserializeResult = true;
                //}

            }
            catch ( Exception ex )
            {
                // ファイルが無い、アクセスできない。
                System.Diagnostics.Debug.WriteLine(String.Format("{0} {1}", ex.Message, ex.StackTrace));
            }
            return deserializeResult;
        }
        
        //// thisに代入できないので、メンバに対してリフレクション使用で設定する。
        //protected Boolean setThis( ref Object testes )
        //{
        //    // 同一タイプでなければ失敗
        //    Type tesType = testes.GetType();
        //    if ( this.paramObject.GetType() != tesType )
        //    {
        //        return false;
        //    }

        //    // メンバフィールドに代入
        //    foreach ( FieldInfo field in tesType.GetFields() )
        //    {
        //        field.SetValue( this.paramObject, field.GetValue( testes ) );
        //    }
        //    foreach ( PropertyInfo property in tesType.GetProperties() )
        //    {
        //        property.SetValue( this.paramObject, property.GetValue( testes, null ), null );
        //    }

        //    return true;
        //}

        #endregion
    }

    /// <summary>
    /// ファイル保存パス
    /// </summary>
    /// <remarks>
    /// ファイルの保存パスを示す操作を定義します。
    /// </remarks>
    public interface ISavePath
    {
        /// <summary>
        /// ファイル保存パス
        /// </summary>
        String SavePath
        {
            get;
        }
    }
}

