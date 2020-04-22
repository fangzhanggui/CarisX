using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.Common.GUI;
using Oelco.Common.Utility;
using Oelco.CarisX.Log;
using Oelco.Common.Log;

namespace Prototype
{
    public partial class SyoKomoku : Form
    {
        FormTransparentLayer m_layerForm = new FormTransparentLayer();

        public SyoKomoku()
        {
            InitializeComponent();

            // ultraButtonはButtonを継承しない、ここに入らない。
            //if ( this.ultraButton1 is Button )
            //{
            //    System.Diagnostics.Debug.WriteLine( "ultra!" );
            //}

            // 表示
            m_layerForm.SetClientPanel( this.ultraPanel1 );
        }

        private void button1_Click( object sender, EventArgs e )
        {
            {
                // 表示エリア取得
                Rectangle rect;
                //rect = m_layerForm.ClientRectangle;
                rect = this.ultraPanel1.ClientRectangle;

                Point showPos = this.button1.PointToScreen( this.button1.Location );

                // ボタン横中央配置
                showPos.X += button1.Size.Width;
                showPos.Y -= rect.Size.Height / 2;
                rect.Location = showPos;
                m_layerForm.Location = showPos;
                m_layerForm.StartPosition = FormStartPosition.Manual;

                // オーナー追加
                this.AddOwnedForm( m_layerForm );
                //m_layerForm.AddOwnedForm( trmenu );

                // 背景設定
                //m_layerForm.BackgroundImage = this.ultraPanel1.Appearance.ImageBackground;
//                m_layerForm.BackgroundImage = global::NetAdvantageDemo.Properties.Resources.Panel;


                // 計算適当
                m_layerForm.Show( new Rectangle( this.Location, this.Size) );
//                m_layerForm.Show( rect );
                //m_layerForm.ShowSlide( TransparentLayerForm2.SlideDir.LeftToRight, rect );

            }
        }

        private void ultraButton1_Click( object sender, EventArgs e )
        {
            MessageBox.Show( "ultraButton1_Click" );
            System.Diagnostics.Debug.WriteLine( "ultraButton1_Click" );
            Singleton<CarisXLogManager>.Instance.Write( LogKind.DebugLog, Singleton<Oelco.CarisX.Utility.CarisXUserLevelManager>.Instance.NowUserID
                    , CarisXLogInfoBaseExtention.Empty, "ultraButton1_Click" );
        }

        private void ultraButton1_MouseUp( object sender, MouseEventArgs e )
        {
            MessageBox.Show( "ultraButton1_MouseUp" );
        }

    }
}
