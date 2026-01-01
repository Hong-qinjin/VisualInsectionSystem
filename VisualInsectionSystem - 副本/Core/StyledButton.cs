// StyledButton.cs
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VMControls.RenderInterface;

namespace VisualInsectionSystem.Core
{
    public class StyledButton : Button
    {
        
        #region 按钮图标类型枚举
        /// <summary>
        /// 扩展图标类型枚举（包含操作按钮）
        /// </summary>
        public enum IconType
        {
            // 执行按钮
            SingleExecute, ContinuousExecute, StopExecute,
            // 操作按钮
            Load, Save, Import, Export, Delete,            
            // 新增扩展类型
            Refresh, Settings
        }
        // 私有属性
        private Color _normalColor = Color.FromArgb(74, 144, 226); // 主色
        private Color _hoverColor = Color.FromArgb(58, 120, 194);  // 悬停色
        private Color _pressedColor = Color.FromArgb(42, 96, 162); // 点击色
        private Color _disabledColor = Color.FromArgb(150, 150, 150); // 禁用色
        private IconType _iconType;
        private int _iconSize = 21;
        #endregion
        #region 公共属性
        public Color NormalColor
        {
            get { return _normalColor; }
            set { _normalColor = value; Invalidate(); }
        }
        public Color HoverColor
        {
            get { return _hoverColor; }
            set { _hoverColor = value; Invalidate(); }
        }
        public Color PressedColor
        {
            get { return _pressedColor; }
            set { _pressedColor = value; Invalidate(); }
        }
        public Color DisabledColor
        {
            get { return _disabledColor; }
            set { _disabledColor = value; Invalidate(); }
        }
        
        public IconType ButtonIconType
        {
            get { return _iconType; }
            set
            {
                _iconType = value;
                UpdateButtonIcon();
            }
        }
        public int IconSize
        {
            get { return _iconSize; }
            set
            {
                _iconSize = value;
                UpdateButtonIcon();
            }
        }
        #endregion
        #region 构造函数
        public StyledButton()
        {
            InitializeStyle();
        }
        public StyledButton(IconType iconType) : this()
        {
            _iconType = iconType;
            UpdateButtonIcon();
        }
        #endregion
        #region
        private void InitializeStyle()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = _normalColor;
            ForeColor = Color.White;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            Font = new Font("宋体", 9F, FontStyle.Regular);
            Padding = new Padding(4, 2, 4, 2);
            Size = new Size(45, 36);
            Margin = new Padding(3);
            Cursor = Cursors.Hand;
            ImageAlign = ContentAlignment.MiddleCenter;
            TextImageRelation = TextImageRelation.ImageBeforeText;
        }
        /// <summary>
        /// 更新按钮图标
        /// </summary>
        public void UpdateButtonIcon()
        {
            Image = CreateButtonIcon(_iconType, _iconSize);
        }
        #endregion
        #region 图标生成方法
        /// <summary>
        /// 生成按钮图标
        /// </summary>
        public Bitmap CreateButtonIcon(IconType type, int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                // 高质量渲染
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.Clear(Color.Transparent);

                // 绘制图标
                switch (type)
                {
                    case IconType.SingleExecute:
                        DrawPlayIcon(g, size);
                        break;
                    case IconType.ContinuousExecute:
                        DrawLoopIcon(g, size);
                        break;
                    case IconType.StopExecute:
                        DrawStopIcon(g, size);
                        break;
                    case IconType.Load:
                        DrawLoadIcon(g, size);
                        break;
                    case IconType.Save:
                        DrawSaveIcon(g, size);
                        break;
                    case IconType.Import:
                        DrawImportIcon(g, size);
                        break;
                    case IconType.Export:
                        DrawExportIcon(g, size);
                        break;
                    case IconType.Delete:
                        DrawDeleteIcon(g, size);
                        break;
                    case IconType.Refresh:
                        DrawRefreshIcon(g, size);
                        break;
                    case IconType.Settings:
                        DrawSettingsIcon(g, size);
                        break;
                }
            }
            return bmp;
        }
        #endregion
        #region 绘制各类图标的方法
        private void DrawPlayIcon(Graphics g, int size)
        {
            var points = new Point[]
            {
                new Point(size / 4, size / 4),
                new Point(size * 3 / 4, size / 2),
                new Point(size / 4, size * 3 / 4)
            };
            using (var brush = new SolidBrush(ForeColor))
            {
                g.FillPolygon(brush, points);
            }
        }

        private void DrawLoopIcon(Graphics g, int size)
        {
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawArc(pen, 2, 2, size - 4, size - 4, -45, 270);
            }
            // 绘制箭头
            var arrow = new Point[]
            {
                new Point(size * 3 / 4, size / 4),
                new Point(size * 3 / 4 - 3, size / 4 - 3),
                new Point(size * 3 / 4 + 1, size / 4 - 1)
            };
            using (var brush = new SolidBrush(ForeColor))
            {
                g.FillPolygon(brush, arrow);
            }
        }

        private void DrawStopIcon(Graphics g, int size)
        {
            var rect = new Rectangle(size / 4, size / 4, size / 2, size / 2);
            using (var brush = new SolidBrush(ForeColor))
            {
                g.FillRectangle(brush, rect);
            }
        }

        private void DrawLoadIcon(Graphics g, int size)
        {
            // 绘制文件夹
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawRectangle(pen, size / 4, size / 3, size / 2, size / 2);
                g.DrawLine(pen, size / 4, size / 3, size / 3, size / 4);
                g.DrawLine(pen, size / 3, size / 4, size / 3, size / 3);
            }
        }

        private void DrawSaveIcon(Graphics g, int size)
        {
            // 绘制保存图标
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawRectangle(pen, size / 4, size / 3, size / 2, size / 2);
                g.DrawRectangle(pen, size / 3, size / 4, size / 3, size / 6);
            }
        }

        private void DrawImportIcon(Graphics g, int size)
        {
            // 绘制导入图标
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawRectangle(pen, size / 4, size / 3, size / 2, size / 2);
            }
            // 左箭头
            var arrow = new Point[]
            {
                new Point(size / 3, size / 2),
                new Point(size / 2, size / 3),
                new Point(size / 2, size * 2 / 3)
            };
            using (var brush = new SolidBrush(ForeColor))
            {
                g.FillPolygon(brush, arrow);
            }
        }

        private void DrawExportIcon(Graphics g, int size)
        {
            // 绘制导出图标
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawRectangle(pen, size / 4, size / 3, size / 2, size / 2);
            }
            // 右箭头
            var arrow = new Point[]
            {
                new Point(size * 2 / 3, size / 2),
                new Point(size / 2, size / 3),
                new Point(size / 2, size * 2 / 3)
            };
            using (var brush = new SolidBrush(ForeColor))
            {
                g.FillPolygon(brush, arrow);
            }
        }

        private void DrawDeleteIcon(Graphics g, int size)
        {
            // 绘制删除图标
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawRectangle(pen, size / 4, size / 3, size / 2, size / 2);
                g.DrawLine(pen, size / 4 - 1, size / 3, size * 3 / 4 + 1, size / 3);
            }
            // 叉号
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawLine(pen, size / 3, size / 2, size * 2 / 3, size / 2);
            }
        }

        private void DrawRefreshIcon(Graphics g, int size)
        {
            // 绘制刷新图标
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawArc(pen, size / 4, size / 4, size / 2, size / 2, 45, 270);
            }
            var arrow = new Point[]
            {
                new Point(size / 2, size / 4),
                new Point(size / 3, size / 5),
                new Point(size / 2, size / 6)
            };
            using (var brush = new SolidBrush(ForeColor))
            {
                g.FillPolygon(brush, arrow);
            }
        }

        private void DrawSettingsIcon(Graphics g, int size)
        {
            // 绘制设置图标
            using (var pen = new Pen(ForeColor, 2))
            {
                g.DrawEllipse(pen, size / 4, size / 4, size / 2, size / 2);
                g.DrawLine(pen, size / 2, size / 4, size / 2, size / 3);
                g.DrawLine(pen, size / 2, size * 2 / 3, size / 2, size * 3 / 4);
                g.DrawLine(pen, size / 4, size / 2, size / 3, size / 2);
                g.DrawLine(pen, size * 2 / 3, size / 2, size * 3 / 4, size / 2);
            }
        }
        #endregion
        #region 鼠标状态处理
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (Enabled) BackColor = _hoverColor;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (Enabled) BackColor = _normalColor;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (Enabled) BackColor = _pressedColor;
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (Enabled)
                BackColor = ClientRectangle.Contains(mevent.Location) ? _hoverColor : _normalColor;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            BackColor = Enabled ? _normalColor : _disabledColor;
        }
        #endregion
        #region
        #endregion
    }
}
