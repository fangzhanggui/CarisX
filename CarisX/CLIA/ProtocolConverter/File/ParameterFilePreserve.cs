using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;


namespace ProtocolConverter.File
{
    /// <summary>
    /// ファイル保存パラメータ
    /// </summary>
    /// <remarks>
    /// Xmlファイルとしてシリアライズされるパラメータクラスへの共通操作を提供します。
    /// </remarks>
    public class ParameterFilePreserve<TypeT> : IPreserveParameter where TypeT : ISavePath, new()
    {
        TypeT paramObject = default( TypeT );
        TypeT originalParamObject = default( TypeT );
        public ParameterFilePreserve()
        {
            paramObject = new TypeT();
            originalParamObject = new TypeT();
        }


        public TypeT Param
        {
            get
            {
                return this.paramObject;
            }
        }        
        
        #region IPreserveParameter メンバー

        /// <summary>
        /// パラメータ保存
        /// </summary>
        /// <remarks>
        /// このクラスのシリアライズを行います。
        /// </remarks>
        /// <returns>True:成功 False:失敗</returns>
        public Boolean Save()
        {
            Boolean saveSuccess = this.saveWithEncryption( paramObject.SavePath );
            return saveSuccess;
        }        

        #endregion

        /// <summary>
        /// 暗号化キー
        /// </summary>
        protected const String ENCRYPT_PASSWORD = "CarisX";

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

            try
            {
                XmlCipher chipher = new XmlCipher(ENCRYPT_PASSWORD);

          
                XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer( this.paramObject.GetType() );
                System.IO.File.Delete( path );

                //using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.CreateNew)))
                //{
                //    serializer.Serialize(writer, this.paramObject);

                //}

                //using (Stream reader = new FileStream(path, FileMode.Open, FileAccess.Read))
                //using (StreamReader sr = new StreamReader(reader))
                //{
                //    Object loadedObjClone = null;
                //    loadedObjClone = serializer.Deserialize(sr);
                //    this.originalParamObject = (TypeT)loadedObjClone;
                //}

                Object loadedObj = null;
                Object loadedObjClone = null;
                using (StreamWriter writer = new StreamWriter(new MemoryStream()))
                {
                    serializer.Serialize(writer, this.paramObject);
                    chipher.EncryptFile(writer.BaseStream, path);
                }

                Stream reader = new MemoryStream();
                XmlCipher chipherReader = new XmlCipher(ENCRYPT_PASSWORD);
                chipherReader.DecryptFile(path, ref reader);

                loadedObj = getObjectFromStreamData(reader);
                loadedObjClone = getObjectFromStreamData(reader);
                this.originalParamObject = (TypeT)loadedObjClone;
                reader.Close();
                reader.Dispose();
             
                serializeResult = true;

            }
            catch ( Exception ex )
            {
                Singleton<ParameterFilePreserve<ProtocolConverterLogInfo>>.Instance.Param.AddExceptionLog( Singleton<ConvertXmlControl>.Instance.ErrorNoList["Exception"], ex.Message + "\n" + ex.StackTrace ); 
                // ファイル削除失敗か、作成失敗(非Debugでも出力する。)
                System.Console.WriteLine( String.Format( "{0}:{1}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message ) );
                System.Diagnostics.Debug.WriteLine(String.Format("{0}:{1} {2}", ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message, ex.StackTrace));
            }

            return serializeResult;
        }

        protected Object getObjectFromStreamData(Stream stream)
        {
            Object loadedObject = null;
            // ファイルポインタの位置を戻す
            stream.Seek(0, SeekOrigin.Begin);
            //using ( StreamReader sr = new StreamReader( reader ) )
            XmlReaderSettings readerSettings = new XmlReaderSettings();

            // XMLファイルサイズ制限を無効化する
            readerSettings.MaxCharactersFromEntities = 0;
            readerSettings.MaxCharactersInDocument = 0;

            XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(this.paramObject.GetType());

            using (XmlReader sr = XmlReader.Create(stream, readerSettings))
            {
                //sr.ReadToEnd();
                try
                {
                    sr.Read();
                    loadedObject = serializer.Deserialize(sr);
                }
                catch (Exception ex)
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

