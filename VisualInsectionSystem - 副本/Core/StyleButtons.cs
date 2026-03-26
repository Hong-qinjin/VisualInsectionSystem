using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VMControls.RenderInterface;

//namespace YourProjectsystem
namespace VisualInsectionSystem
{      
    public class StyleButtons : Button
    {
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
        /// <summary>
        /// 设置默认按钮样式
        /// </summary>
        /// <param name="button">目标按钮</param>      
        private     Color   _backgroundColor = Color.WhiteSmoke;
        private     Color   _foregroundColor = Color.Black;
        private     Color   _normalColor = Color.FromArgb(74, 144, 226);  // 主色   
        private     Color   _hoverBackgroundColor = Color.LightGray;    // Color.FromArgb(58, 120, 194);  // 悬停色
        private     Color   _pressedBackgroundColor = Color.DimGray;    // Color.FromArgb(42, 96, 162); // 点击色
        private     Color   _disabledColor = Color.FromArgb(150, 150, 150); // 禁用色
        private     int     _borderRadius = 4;
        private     Padding _padding = new Padding(4, 2, 4, 2);        
        private     Font    _font = new Font("微软雅黑", 9F, FontStyle.Regular);
        private     bool    _showBorder = true;
        private     Color   _borderColor = Color.Gray;
        private     int     _borderWidth = 1;        
        private     bool    _isHovered;
        private     bool    _isPressed;
        private     bool    _isFocused;

        private IconType _iconType;
        private int _iconSize = 21;

        // 构造函数
        public StyleButtons()
        {
            SetStyle( ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.SupportsTransparentBackColor, 
                      true );
            BackColor = _backgroundColor;
            ForeColor = _foregroundColor;
            FlatStyle = FlatStyle.Flat;
            Cursor = Cursors.Hand;
            AutoSize = false;
            DoubleBuffered = true;

            // 事件处理           
            MouseEnter += StyleButtons_MouseEnter;
            MouseLeave += StyleButtons_MouseLeave;
            MouseDown += StyleButtons_MouseDown;
            MouseUp += StyleButtons_MouseUp;
            GotFocus += StyleButtons_GotFocus;
            LostFocus += StyleButtons_LostFocus;

        }

        // 背景色
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
               if ( _backgroundColor != value )
                {
                    _backgroundColor = value;
                    Invalidate();
                }
            }
        }
        // 前景色
        public Color ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                if ( _foregroundColor != value )
                {
                    _foregroundColor = value;
                    Invalidate();
                }
            }
        }
        // 主色
        public Color NormalColor
        {
            get { return _normalColor; }
            set { _normalColor = value; Invalidate(); }
        }
        // 禁用
        public Color DisabledColor
        {
            get { return _disabledColor; }
            set { _disabledColor = value; Invalidate(); }
        }
        // 属性：悬停背景色
        public Color HoverBackgroundColor
        {
            get => _hoverBackgroundColor;
            set
            {
                _hoverBackgroundColor = value;
                Invalidate();
            }
        }
        // 属性：按下背景色
        public Color PressedBackgroundColor
        {
            get => _pressedBackgroundColor;
            set
            {
                _pressedBackgroundColor = value;
                Invalidate();
            }
        }
        // 属性：圆角半径
        public int BorderRadius
        {
            get => _borderRadius;
            set
            {
                _borderRadius = value;
                Invalidate();
            }
        }
        // 属性：内边距
        public Padding Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                Invalidate();
            }
        }
        // 属性：字体
        public Font Font
        {
            get => _font;
            set
            {
                _font = value;
                Invalidate();
            }
        }
        // 属性：是否显示边框
        public bool ShowBorder
        {
            get => _showBorder;
            set
            {
                _showBorder = value;
                Invalidate();
            }
        }
        // 属性：边框颜色
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }
        // 属性：边框宽度
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                Invalidate();
            }
        }

        // OnPaint方法重写，绘制自定义按钮
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 绘制背景
            using (GraphicsPath path =new GraphicsPath())
            {
                int radius = _borderRadius;
                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
                path.AddArc(rect.Left, rect.Top, radius * 2, radius * 2, 180, 90);
                path.AddArc(rect.Right - radius * 2, rect.Top, radius * 2, radius * 2, 270, 90);
                path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(rect.Left, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();

                using(SolidBrush brush = new SolidBrush(GetBackgroundColor()))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // 绘制边框
                if(_showBorder)
                {
                    using (Pen pen = new Pen(_borderColor, _borderWidth))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }
            //// 绘制文本
            //TextRenderer.DrawText(e.Graphics, Text, _font, 
            //    new Rectangle(_padding.Left, _padding.Top, Width - _padding.Horizontal, Height - _padding.Vertical), 
            //    _foregroundColor, _padding, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            //// 绘制文本 - 修正后的代码
            Rectangle textRect = new Rectangle(_padding.Left, _padding.Top,
                                              Width - _padding.Horizontal,
                                              Height - _padding.Vertical);
            TextRenderer.DrawText(e.Graphics, Text, _font, textRect, _foregroundColor,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);            
        }
        
        private Color GetBackgroundColor()
        {
            if (!Enabled)
                return _backgroundColor;
            if(_isHovered ||  _isPressed)
                return _hoverBackgroundColor;
            if (_isPressed)
                return _pressedBackgroundColor;

            return _backgroundColor;
        }

        private void StyleButtons_MouseEnter(object sender, EventArgs e)
        {
            _isHovered = true;
            Invalidate();
        }
        private void StyleButtons_MouseLeave(object sender, EventArgs e)
        {
            _isHovered = true;
            Invalidate();
        }
        private void StyleButtons_MouseDown(object sender, MouseEventArgs e)
        {
            _isPressed = true;
            Invalidate();
        }
        private void StyleButtons_MouseUp(object sender, MouseEventArgs e)
        {
            _isPressed = false;
            Invalidate();
        }
        private void StyleButtons_GotFocus(object sender, EventArgs e)
        {
            _isFocused = true;
            Invalidate();
        }
        private void StyleButtons_LostFocus(object sender, EventArgs e)
        {
            _isFocused = false;
            Invalidate();
        }

        /// <summary>
        /// 更新按钮图标
        /// </summary>
        public void UpdateButtonIcon()
        {
            Image = CreateButtonIcon(_iconType, _iconSize);
        }

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

    }
}
