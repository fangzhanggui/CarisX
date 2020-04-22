using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;



// TODO:コメント不十分

namespace Oelco.Common.Utility
{
    /// <summary>
    /// 透過処理補助クラス
    /// </summary>
    public static class Transparency
    {
        /// <summary>
        /// 透過色リージョン排除
        /// </summary>
        /// <remarks>
        /// フォームのTransparencykeyに設定された色は、本来そのフォーム上に描画される該当色全てに対して透過が行われるが、
        /// 環境により、その機能が正常動作しないことがある為、透過を行うフォームではこの関数を適宜呼び出す。
        /// </remarks>
        /// <param name="tgtForm">対象フォーム</param>
        static public void ExcludeRegion( Form tgtForm )
        {
            // 画面の描画内容を取得
            using ( Bitmap foregroundBitmap = new Bitmap( tgtForm.ClientSize.Width, tgtForm.ClientSize.Height ) )
            {
                tgtForm.DrawToBitmap( foregroundBitmap, tgtForm.ClientRectangle );

                Int32 width = foregroundBitmap.Width;
                Int32 height = foregroundBitmap.Height;

                // リージョン作成
                Rectangle rect = new Rectangle( 0, 0, width, height );
                Region region = new Region( rect );


                // stopwatch
                //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                //sw.Start();

                // 透過設定されている箇所を取得
                // 全pixelをサーチする。
                using ( GraphicsPath gp = new GraphicsPath() )
                {
                    for ( Int32 y = 0; y < height; y++ )
                    {
                        for ( Int32 x = 0; x < width; x++ )
                        {
                            // 透過色と一致しているピクセル座標を記録する
                            if ( foregroundBitmap.GetPixel( x, y ).ToArgb() == tgtForm.TransparencyKey.ToArgb() )//Color.FromArgb( 0xff, 0x00, 0x80, 0x00 ) )//Color.Green )
                            {
                                gp.AddRectangle( new Rectangle( x, y, 1, 1 ) );
                            }
                        }
                    }

                    // stopwatch
                    //sw.Stop();
                    //Console.WriteLine("経過時間 透過設定　={0}", sw.Elapsed);

                    region.Exclude( gp );
                }
              

                // Region に描画された領域を設定します。
                tgtForm.Region = region;
            }
        }

        /// <summary>
        /// 指定サイズのイメージ描画Bitmapの取得
        /// </summary>
        /// <remarks>
        /// 指定サイズ領域に指定の表示状態に従い指定のイメージ一覧を描画後のBitmapを作成します。
        /// </remarks>
        /// <param name="imageSize">イメージサイズ</param>
        /// <param name="imageList">イメージ一覧(各表示位置、関連コントロール含む)</param>
        /// <param name="visibleInfo">コントロールの表示状態辞書</param>
        /// <returns></returns>
        static public Bitmap GetAlphaImage( Size imageSize, List<Tuple<Image, Point,Control>> imageList, Dictionary<Control, Boolean> visibleInfo )
        {
            Bitmap foregroundBitmap = new Bitmap( imageSize.Width, imageSize.Height );
            
            using ( Graphics g = Graphics.FromImage( foregroundBitmap ) )
            {
                foreach ( var image in imageList )
                {
                    Boolean visible = visibleInfo.ContainsKey( image.Item3 ) ? visibleInfo[image.Item3] : false;
                    if ( visible )
                    {
                        g.DrawImage( image.Item1, image.Item2 );
                    }
                }
            }

            return foregroundBitmap;
        }

        /// <summary>
        /// 透過色リージョン排除
        /// </summary>
        /// <remarks>
        /// フォームのTransparencykeyに設定された色は、本来そのフォーム上に描画される該当色全てに対して透過が行われるが、
        /// 環境により、その機能が正常動作しないことがある為、透過を行うフォームではこの関数を適宜呼び出す。
        /// </remarks>
        /// <param name="tgtForm">対象フォーム</param>
        static public Bitmap GetAlphaImage( Form tgtForm )
        {
            // 画面の描画内容を取得
            //using ( Bitmap foregroundBitmap = new Bitmap( tgtForm.ClientSize.Width, tgtForm.ClientSize.Height ) )
            //{
            Bitmap foregroundBitmap = new Bitmap( tgtForm.ClientSize.Width, tgtForm.ClientSize.Height );
            tgtForm.DrawToBitmap( foregroundBitmap, tgtForm.ClientRectangle );

            Int32 width = foregroundBitmap.Width;
            Int32 height = foregroundBitmap.Height;

            // リージョン作成
            Rectangle rect = new Rectangle( 0, 0, width, height );
            //Region region = new Region( rect );

            // 透過設定されている箇所を取得
            // 全pixelをサーチする。
            //using ( GraphicsPath gp = new GraphicsPath() )
            //{
            //System.Drawing.Color col = new System.Drawing.Color();
            for (Int32 y = 0; y < height; y++)
            {
                for (Int32 x = 0; x < width; x++)
                {
                    // 透過色と一致しているピクセル座標を記録する
                    if (foregroundBitmap.GetPixel(x, y).ToArgb() == tgtForm.TransparencyKey.ToArgb())//Color.FromArgb( 0xff, 0x00, 0x80, 0x00 ) )//Color.Green )
                    {
                        foregroundBitmap.SetPixel(x, y, System.Drawing.Color.Transparent);
                    }
                }
            }

            //}
            //}

            return foregroundBitmap;

        }
    }
}
