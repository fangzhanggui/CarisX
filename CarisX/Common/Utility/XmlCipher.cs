using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace Oelco.Common.Utility
{


    public class StringCipher
    {
        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            // Get the key from config file
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        } 

    }
    /// <summary>
    /// XML暗号化
    /// </summary>
    public class XmlCipher
    {
        #region [インスタンス変数定義]

        /// <summary>
        /// Saltキー（暗号化復号化用）
        /// </summary>
        private Byte[] salt = Encoding.UTF8.GetBytes( "saltは必ず8バイト以上" );  // Todo:未決定

        /// <summary>
        /// 高度暗号化標準 (AES: Advanced Encryption Standard) 対称アルゴリズム
        /// </summary>
        private AesManaged aesManaged = new AesManaged();

        #endregion

        #region [コンストラクタ/デストラクタ]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="password">暗号化パスワード</param>
        public XmlCipher( String password )
        {
            // AesManagedオブジェクトの設定 ※AESはブロックサイズ、キー長ともに128bit
            this.aesManaged.BlockSize = 128;               // ブロックサイズ
            this.aesManaged.KeySize = 128;                 // キー最大長
            this.aesManaged.Mode = CipherMode.CBC;         // CBCモード
            this.aesManaged.Padding = PaddingMode.PKCS7;   // パディングモード

            // 共有キーの設定
            // パスワードから共有キーと初期化ベクタを作成する

            // Rfc2898DeriveBytesオブジェクトを作成する
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes( password, salt )
            {
                IterationCount = 1000   // 反復処理回数を指定する デフォルトで1000回
            };
            
            // 共有キーと初期化ベクタを生成する
            this.aesManaged.Key = deriveBytes.GetBytes( this.aesManaged.KeySize / 8 );
            this.aesManaged.IV = deriveBytes.GetBytes( this.aesManaged.BlockSize / 8 );
        }
      

        #endregion
        
        #region [publicメソッド]

        /// <summary>
        /// AES暗号化処理
        /// </summary>
        /// <remarks>AES暗号によるストリームの暗号化を行います。</remarks>
        /// <param name="sourceStream">暗号化するストリーム</param>
        /// <param name="destFile">暗号化されたデータを保存するファイルパス</param>
        public void EncryptFile( Stream sourceStream, String destFile )
        {
            // 暗号化されたファイルの書き出すためのFileStream
            using (FileStream outFs = new System.IO.FileStream( destFile, System.IO.FileMode.Create, System.IO.FileAccess.Write ))
            {
                // IVを先頭に書き込む(128bit=16bytes)
                this.aesManaged.GenerateIV();
                outFs.Write( this.aesManaged.IV, 0, 16 );

                // 対称暗号化オブジェクトの作成
                using (ICryptoTransform encryptor = this.aesManaged.CreateEncryptor())
                {
                    // 暗号化されたデータを書き出すためのCryptoStreamの作成
                    using (CryptoStream cryptStrm = new CryptoStream( outFs, encryptor, CryptoStreamMode.Write ))
                    {
                        // 暗号化されたデータを書き出す
                        sourceStream.Position = 0;
                        //byte[] bs = new byte[1024];
                        // 暗号化に失敗すると例外CryptographicExceptionが発生
                        // TODO:例外ガード（finallyでリソース開放）
                        StreamWriter writer = new StreamWriter( cryptStrm );
                        StreamReader reader = new StreamReader( sourceStream );
                        writer.Write( reader.ReadToEnd() );
                        writer.Flush();
                        //for (Int32 readLen = sourceStream.Read( bs, 0, bs.Length ); readLen > 0; readLen = sourceStream.Read( bs, 0, bs.Length ))
                        //{
                        //    cryptStrm.Write( bs, 0, readLen );
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// AES復号化処理
        /// </summary>
        /// <remarks>AES暗号により暗号化されたファイルの復号化</remarks>
        /// <param name="sourceFile">復号化するファイルパス</param>
        /// <param name="stream">復号化されたストリーム</param>
        public void DecryptFile( String sourceFile, ref Stream stream )
        {
            // 暗号化されたファイルを読み込み先
            using (FileStream inFs = new FileStream( sourceFile, FileMode.Open, FileAccess.Read ))
            {
                // IVを先頭から取り出してAesCryptoServiceProviderオブジェクトにセット
                byte[] b = new byte[16];
                inFs.Read( b, 0, 16 );
                this.aesManaged.IV = b;

                // 対称復号化オブジェクトの作成
                using (ICryptoTransform decryptor = this.aesManaged.CreateDecryptor())
                {
                    // 暗号化されたデータを読み込むためのCryptoStreamの作成
                    using (CryptoStream cryptStrm = new CryptoStream( inFs, decryptor, CryptoStreamMode.Read ))
                    {
                        // 復号化されたデータを書き出す
                        //byte[] bs = new byte[1024];

                        // 復号化に失敗すると例外CryptographicExceptionが発生
                        // TODO:例外ガード（finallyでリソース開放）
                        StreamWriter writer = new StreamWriter( stream );
                        StreamReader reader = new StreamReader( cryptStrm );
                        writer.Write( reader.ReadToEnd() );
                        writer.Flush();
                        //for (Int32 readLen = cryptStrm.Read( bs, 0, bs.Length ); readLen > 0; readLen = cryptStrm.Read( bs, 0, bs.Length ))
                        //{
                        //    stream.Write( bs, 0, readLen );
                        //}
                        stream.Position = 0;
                    }
                }
            }
        }

        #endregion
    }
}
