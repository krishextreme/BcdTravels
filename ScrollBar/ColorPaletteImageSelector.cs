// 
//using ExCSS;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ledger.FileGenerator;

using Theme = Ledger.UIHelper.UIGraphicsHelper;

namespace Ledger.ScrollBar
{

    [ToolboxItem(false)]
    [Description("Internal part of the color picker window. Do not use directly.")]



    public class ColorPaletteImageSelector : PictureBox // was Λא\uFFFD\uFFFD\uFFFDגא\uFFFDH\uFFFD\uD808\uDD0Eב unicode
    {
        private Point lastClickPosition = new Point(-8, -8);
        public UITheme Theme { get; set; } = UITheme.Light;
        private Pen whitepen = new Pen(Color.White, 1f);
        private Pen blackpen = new Pen(Color.Black, 1f);
        private IContainer components;
        public Image Content;
        public ColorPaletteImageSelector() // was \uFFFDܗΘP\uFFFD\uFFFDלΕ\uFFFDܝܠᛋ\uFFFDל\uFFFD unicode
        {
            this.Content = (Image)Ledger.FileGenerator.Resources.all_colours;
        }

        public ColorPaletteImageSelector(
          UITheme inputTheme) // was \uFFFDܗΘP\uFFFD\uFFFDלΕ\uFFFDܝܠᛋ\uFFFDל\uFFFD unicode
        {
            this.Content = (Image)Ledger.FileGenerator.Resources.all_colours;
            this.Theme = inputTheme;
        }

        public void UpdatePos()
        {
            this.lastClickPosition = this.PointToClient(Cursor.Position);
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int num = 8;
            RectangleF rect = new RectangleF((float)(this.lastClickPosition.X - num), (float)(this.lastClickPosition.Y - num), (float)(num * 2), (float)(num * 2));
            e.Graphics.DrawEllipse(this.Theme == UITheme.Dark ? this.whitepen : this.blackpen, rect);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new System.ComponentModel.Container();
            //this.AutoScaleMode = AutoScaleMode.Font;
        }
    }
    public enum UITheme
    {
        Dark,
        Light
    }
}