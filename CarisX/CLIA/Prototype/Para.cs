using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Oelco.Common.Utility;
using Oelco.Common.Parameter;
using Oelco.CarisX.Parameter;


using System.Xml.Serialization;

namespace Prototype
{
    public partial class Para : Form
    {
        public Para()
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            //Type ty = typeof(Int32);
            //FormSpecimenResistrationSettings inter = new FormSpecimenResistrationSettings();// = 3;
            //XmlSerializer lizer = new XmlSerializer( typeof( FormSpecimenResistrationSettings ) );
            //using ( System.IO.StreamWriter iter = new System.IO.StreamWriter( @"c:\pa.xml" ) )
            //{
            //    lizer.Serialize( iter, inter );
            //}

            

            //// TODO:Enumeratorを持つメンバをシリアライズできない！（Dictionaryが引っかかる）
            //Tesset tes = new Tesset();
            //Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Save();
            //Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Load();
            //Singleton<ParameterFilePreserve<CarisXUISettingManager>>.Instance.Param.SetUISettings( tes );
        }

  
    }
}
