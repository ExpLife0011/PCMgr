﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PCMgr.Ctls
{
    public class PerformanceInfos : Control
    {
        public PerformanceInfos()
        {
            brushText = new SolidBrush(Color.Black);
            brushTitle = new SolidBrush(Color.Gray);
            LoadAllFonts();
            MaxSpeicalItemsWidth = 200;

            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void LoadAllFonts()
        {
            FontTextSpeical = new Font(Font.FontFamily, 15);
            FontText = new Font(Font.FontFamily, 9f);
            FontTitle = new Font(Font.FontFamily, 9f);
        }

        public Color ColorTitle
        {
            get { return brushTitle.Color; }
            set { brushTitle.Color = value; }
        }
        public Color ColorText
        {
            get { return brushText.Color; }
            set { brushText.Color = value; }
        }

        public Font FontText
        {
            get { return _FontText; }
            set
            {
                _FontText = value;
                FontTextHeight = _FontText.Height;
            }
        }
        public Font FontTextSpeical
        {
            get { return _FontTextSpeical; }
            set
            {
                _FontTextSpeical = value;
                FontTextSpeicalHeight = _FontTextSpeical.Height;
            }
        }
        public Font FontTitle
        {
            get { return _FontTitle; }
            set
            {
                _FontTitle = value;
                FontTitleHeight = _FontTitle.Height;
            }
        }
        public int MaxSpeicalItemsWidthLimit { get; set; }
        public int MaxSpeicalItemsWidth { get; set; }
        public int ItemMargan { get; set; }
        public int LineOffest { get; set; }

        private bool NeedRefeshStaticItems = false;
        private int FontTitleHeight = 0;
        private int FontTextSpeicalHeight = 0;
        private int FontTextHeight = 0;
        private Font _FontText { get; set; }
        private Font _FontTextSpeical { get; set; }
        private Font _FontTitle { get; set; }
        private SolidBrush brushText = null;
        private SolidBrush brushTitle = null;

        public class PerformanceInfoStaticItem
        {
            public PerformanceInfoStaticItem()
            {
                IsIconItem = false;
                LineSp = false;
            }
            public PerformanceInfoStaticItem(string name, string value)
            {
                Value = value;
                Name = name;
                LineSp = false;
                IsIconItem = false;
            }
            public PerformanceInfoStaticItem(string name, Icon value)
            {
                IconValue = value;
                Name = name;
                LineSp = false;
                IsIconItem = true;
            }

            public bool IsIconItem { get; set; }
            public Icon IconValue { get; set; }
            public string Value { get; set; }
            public string Name { get; set; }
            public bool LineSp { get; set; }
        }
        public class PerformanceInfoSpeicalItem : PerformanceInfoStaticItem
        {
            public PerformanceInfoSpeicalItem()
            {
                DrawFrontLine = false;
                FrontLineColor = Color.White;
                FrontLineIsDotted = false;
                FrontLineWidth = 2;
            }

            public bool DrawFrontLine { get; set; }
            public Color FrontLineColor { get; set; }
            public bool FrontLineIsDotted { get; set; }
            public float FrontLineWidth { get; set; }
        }

        private List<PerformanceInfoStaticItem> listStaticItems = new List<PerformanceInfoStaticItem>();
        private List<PerformanceInfoSpeicalItem> listSpeicalItems = new List<PerformanceInfoSpeicalItem>();

        public List<PerformanceInfoStaticItem> StaticItems { get { return listStaticItems; } }
        public List<PerformanceInfoSpeicalItem> SpeicalItems { get { return listSpeicalItems; } }

        public string GetCopyString()
        {
            string rs = "";
            foreach (PerformanceInfoStaticItem s in listStaticItems)
                rs += "\n    " + s.Name + " : " + s.Value;
            foreach (PerformanceInfoSpeicalItem s in listSpeicalItems)
                rs += "\n    " + s.Name + " : " + s.Value;
            return rs;
        }
        public void UpdateSpeicalItems()
        {
            if (NeedRefeshStaticItems) {
                Invalidate();
                NeedRefeshStaticItems = false;
            }
            else Invalidate(new Rectangle(0, 0, MaxSpeicalItemsWidth, Height));
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            LoadAllFonts();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            int curY = 0;
            int drawedX = ItemMargan;
            for (int i = 0; i < SpeicalItems.Count; i++)
            {
                DRAWSTART:
                PerformanceInfoSpeicalItem it = SpeicalItems[i];
                int w1 = (int)(g.MeasureString(it.Name, FontTitle).Width);
                int w2 = (int)(g.MeasureString(it.Value, FontTextSpeical).Width);
                int w = w1 > w2 ? w1 : w2;
                REDRAW:
                if (drawedX + w < MaxSpeicalItemsWidth)
                {
                    if (SpeicalItems[i].LineSp)
                    {
                        curY += (FontTitleHeight + FontTextSpeicalHeight + 10);
                        drawedX = ItemMargan;
                    }
                    g.DrawString(it.Name, FontTitle, brushTitle, drawedX, curY);
                    g.DrawString(it.Value, FontTextSpeical, brushText, drawedX, curY + FontTitleHeight + 3);
                    if (it.DrawFrontLine)
                    {
                        using (Pen p = new Pen(it.FrontLineColor, it.FrontLineWidth))
                        {
                            if (it.FrontLineIsDotted) p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            g.DrawLine(p, drawedX - LineOffest, curY + 2, drawedX - LineOffest, curY + FontTitleHeight + FontTextSpeicalHeight + 2);
                        }
                        drawedX += 5;
                    }
                    drawedX += w + ItemMargan;
                }
                else
                {
                    if (SpeicalItems[i].LineSp)
                    {
                        drawedX = ItemMargan;
                        goto REDRAW;
                    }
                    if (drawedX + w < MaxSpeicalItemsWidthLimit)
                    {
                        MaxSpeicalItemsWidth = drawedX + w + 20;
                        NeedRefeshStaticItems = true;
                        goto REDRAW;
                    }

                    curY += (FontTitleHeight + FontTextSpeicalHeight + 10);
                    if (curY < Height)
                    {
                        drawedX = ItemMargan;
                        goto DRAWSTART;
                    }
                    else break;
                }
            }

            if (e.ClipRectangle.Right > MaxSpeicalItemsWidth)
            {
                curY = 0;
                int maxWidth = 0;
                for (int i = 0; i < StaticItems.Count; i++)
                {
                    PerformanceInfoStaticItem it = StaticItems[i];
                    int w = (int)(g.MeasureString(it.Name, FontTitle).Width);
                    if (w > maxWidth) maxWidth = w;
                    g.DrawString(it.Name, FontText, brushTitle, MaxSpeicalItemsWidth, curY);
                    curY += FontTextHeight;
                }
                maxWidth += MaxSpeicalItemsWidth + 5;
                curY = 0;
                for (int i = 0; i < StaticItems.Count; i++)
                {
                    PerformanceInfoStaticItem it = StaticItems[i];
                    if (it.IsIconItem) g.DrawIcon(it.IconValue, maxWidth, curY + (FontTextHeight - it.IconValue.Height));
                    else g.DrawString(it.Value, FontText, brushText, maxWidth, curY);
                    curY += FontTextHeight;
                }
            }
        }
    }
}
