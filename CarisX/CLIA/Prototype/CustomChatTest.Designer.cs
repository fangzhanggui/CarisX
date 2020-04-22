namespace Prototype
{
    partial class CustomChartTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            this.customChart1 = new Oelco.Common.GUI.CustomChart();
            ((System.ComponentModel.ISupportInitialize)(this.customChart1)).BeginInit();
            this.SuspendLayout();
            // 
//			'CustomChart' プロパティのシリアル化: 'ChartType' は軸の外観を変更するので、
//			デザインタイムに軸の変更を行った前に'ChartType'は持続しなければなりません。
//		
            this.customChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.StackSplineChart;
            // 
            // customChart1
            // 
            this.customChart1.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement1.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement1.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.customChart1.Axis.PE = paintElement1;
            this.customChart1.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.customChart1.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.customChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.customChart1.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.customChart1.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.customChart1.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.customChart1.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.customChart1.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.X.MajorGridLines.Visible = true;
            this.customChart1.Axis.X.Margin.Far.Value = 0.975609756097561D;
            this.customChart1.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.customChart1.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.X.MinorGridLines.Visible = false;
            this.customChart1.Axis.X.Visible = true;
            this.customChart1.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.customChart1.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.customChart1.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.customChart1.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.customChart1.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.customChart1.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
            this.customChart1.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.X2.Labels.Visible = false;
            this.customChart1.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.customChart1.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.X2.MajorGridLines.Visible = true;
            this.customChart1.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.customChart1.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.X2.MinorGridLines.Visible = false;
            this.customChart1.Axis.X2.Visible = false;
            this.customChart1.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.customChart1.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.customChart1.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.customChart1.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.customChart1.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
            this.customChart1.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.customChart1.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.customChart1.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.Y.MajorGridLines.Visible = true;
            this.customChart1.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.customChart1.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.Y.MinorGridLines.Visible = false;
            this.customChart1.Axis.Y.Visible = true;
            this.customChart1.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.customChart1.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.customChart1.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.customChart1.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.customChart1.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.customChart1.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.customChart1.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.Y2.Labels.Visible = false;
            this.customChart1.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.customChart1.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.Y2.MajorGridLines.Visible = true;
            this.customChart1.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.customChart1.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.Y2.MinorGridLines.Visible = false;
            this.customChart1.Axis.Y2.Visible = false;
            this.customChart1.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.customChart1.Axis.Z.Labels.ItemFormatString = "";
            this.customChart1.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.customChart1.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.customChart1.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.customChart1.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.customChart1.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.Z.MajorGridLines.Visible = true;
            this.customChart1.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.customChart1.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.Z.MinorGridLines.Visible = false;
            this.customChart1.Axis.Z.Visible = false;
            this.customChart1.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.customChart1.Axis.Z2.Labels.ItemFormatString = "";
            this.customChart1.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.customChart1.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.customChart1.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.customChart1.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.customChart1.Axis.Z2.Labels.Visible = false;
            this.customChart1.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.customChart1.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.Z2.MajorGridLines.Visible = true;
            this.customChart1.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.customChart1.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.customChart1.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.customChart1.Axis.Z2.MinorGridLines.Visible = false;
            this.customChart1.Axis.Z2.Visible = false;
            this.customChart1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.customChart1.ColorModel.AlphaLevel = ((byte)(150));
            this.customChart1.Location = new System.Drawing.Point(24, 31);
            this.customChart1.Name = "customChart1";
            this.customChart1.Size = new System.Drawing.Size(422, 325);
            this.customChart1.TabIndex = 0;
            // 
            // CustomChartTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 402);
            this.Controls.Add(this.customChart1);
            this.Name = "CustomChartTest";
            this.Text = "CustomChatTest";
            ((System.ComponentModel.ISupportInitialize)(this.customChart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Oelco.Common.GUI.CustomChart customChart1;
    }
}