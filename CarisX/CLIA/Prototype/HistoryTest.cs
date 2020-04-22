using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.Common.Utility;
using System.Diagnostics.Contracts;

namespace Prototype
{
    public partial class HistoryTest : Form
    {

        public HistoryTest()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            // 追加、実行
            Singleton<HistoryManager>.Instance.AddNew( HistTestKind.GetKind( HistTestKind.Kind.ButtonA ), () =>
                {
                    lblAction.Text = "button1_Click";
                } );
            Singleton<HistoryManager>.Instance.ExecRecent();
            //this.refleshRirekiDisp();
        }

        private void button2_Click( object sender, EventArgs e )
        {
            // 追加、実行
            Singleton<HistoryManager>.Instance.AddNew( HistTestKind.GetKind( HistTestKind.Kind.ButtonB ), () =>
            {
                lblAction.Text = "button2_Click";
            } );
            Singleton<HistoryManager>.Instance.ExecRecent();
            //this.refleshRirekiDisp();
        }


        //class A
        //{
        //    public virtual Boolean Eqal( A a )
        //    {
        //        return true;
        //    }
        //}
        //class B : A
        //{
        //    private Kind k;
        //    public enum Kind
        //    {
        //        a,b,c,d
        //    }
        //    public B(Kind kind)
        //    {
        //        k=kind;
        //    }


        //    public override Boolean Eqal( A a )
        //    {
        //        Boolean sameClass = a is B;
        //        Boolean same = false;
        //        if ( sameClass )
        //        {
        //            same = this.k == ( a as B ).k;
        //        }
        //        return same;
        //    }

        //    //public static override Boolean operator == (B b, B c)
        //    //{
        //    //    return b.k == c.k;
        //    //}
        //    //public static override Boolean operator !=( B b, B c )
        //    //{
        //    //    return b.k != c.k;
        //    //}

        //}
        private void HistoryTest_Load( object sender, EventArgs e )
        {
            //            A a = new A();
            //            B b = new B( B.Kind.a );
            //            B c = new B( B.Kind.c );
            //            A d = new B( B.Kind.a );

            ////            Boolean eqAB = a == b;
            //  //          Boolean eqBC = c == b;
            //    //        Boolean eqBD = d == b;
            //            Boolean eqBC = c.Eqal(b);
            //            Boolean eqBD = d.Eqal(b);

            this.refleshRirekiDisp();

            // イベント関連付け
            Singleton<HistoryManager>.Instance.HistChanged += this.OnHistChanged;
        }
        private void HistoryTest_FormClosed( object sender, FormClosedEventArgs e )
        {            
            // イベント関連付け解除
            Singleton<HistoryManager>.Instance.HistChanged -= this.OnHistChanged;
        }

        private void OnHistChanged( Object sender, EventArgs args )
        {
            this.refleshRirekiDisp();
        }
        class HistTestKind : HistoryActionKind
        {
            // 履歴種別クラスサンプル。文字列化も要りそう？（もしファイルに落とすのであれば）
            // Caris,AFTでそれぞれ一つずつこのクラスと同様のものを実装する。
            public enum Kind
            {
                ButtonA,
                ButtonB
            }
            private Kind kind;
            private readonly static Dictionary<Kind, HistTestKind> thisDic = new Dictionary<Kind, HistTestKind>();

            static HistTestKind()
            {
                var kinds = Enum.GetValues(typeof(Kind));
                foreach ( var knd in kinds )
                {
                    thisDic.Add( (Kind)knd, new HistTestKind( (Kind)knd ) );
                }
            }

            static public HistTestKind GetKind( Kind knd )
            {
                return thisDic[knd];
            }

            protected HistTestKind( Kind kind )
            {
                this.kind = kind;
            }
            public Kind ActionKind
            {
                get
                {
                    return kind;
                }
                //set
                //{
                //    kind = value;
                //}
            }

        }

        private void button3_Click( object sender, EventArgs e )
        {
            // 戻る、実行
            Singleton<HistoryManager>.Instance.MovePrev();
            Singleton<HistoryManager>.Instance.ExecCurrent();
        }

        private void button4_Click( object sender, EventArgs e )
        {
            // 進む、実行
            Singleton<HistoryManager>.Instance.MoveNext();
            Singleton<HistoryManager>.Instance.ExecCurrent();
        }

        private void button5_Click( object sender, EventArgs e )
        {
            // クリア
            Singleton<HistoryManager>.Instance.ClearHistory();
        }

        private void refleshRirekiDisp()
        {
            // 履歴を「現在位置/最大数」で表示
            this.lblHistStat.Text = String.Format(
                    "{0}/{1}",
                    Singleton<HistoryManager>.Instance.CurrentPos,
                    Singleton<HistoryManager>.Instance.HistoryCount );

            // ボタンの押下可不可を設定
            this.button3.Enabled = Singleton<HistoryManager>.Instance.ExistPrev(); // ←
            this.button4.Enabled = Singleton<HistoryManager>.Instance.ExistNext(); // →

        }

        private void customUStateButton1_Click( object sender, EventArgs e )
        {

        }

    }
}
