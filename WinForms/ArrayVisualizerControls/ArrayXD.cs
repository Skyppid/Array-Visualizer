using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsArrayVisualizerControls
{
    public abstract class ArrayXD : PictureBox
    {
        #region Fields

        private Size cellSize;
        private Array data;
        private Font font;

        #endregion

        #region Constructors and Destructors

        protected ArrayXD()
        {
            cellSize = new Size(80, 55);
            CellPadding = 10;
        }

        #endregion

        #region Public Properties

        public int CellHeight
        {
            get { return cellSize.Height; }
            set { cellSize.Height = value; }
        }

        public int CellPadding { get; set; }

        public Size CellSize
        {
            get { return cellSize; }
            set { cellSize = value; }
        }

        public int CellWidth
        {
            get { return cellSize.Width; }
            set { cellSize.Width = value; }
        }

        public Array Data
        {
            get { return data; }
            set
            {
                data = value;
                if (value == null)
                    Refresh();
                else
                {
                    SetAxisSize();
                    Render();
                }
            }
        }

        public string Formatter { get; set; }

        public Font RenderFont
        {
            get
            {
                if (font == null)
                    return base.Font;
                else
                    return font;
            }
            set { font = value; }
        }

        #endregion

        #region Public Methods and Operators

        public void Render()
        {
            RenderBlankGrid();
            DrawContent();
        }

        #endregion

        #region Methods

        protected abstract void DrawContent();

        protected abstract void RenderBlankGrid();

        protected abstract void SetAxisSize();

        #endregion
    }

#if DEBUG

    public class ArrayProxy : ArrayXD
    {
        #region Methods

        protected override void DrawContent()
        {
            throw new NotImplementedException();
        }

        protected override void RenderBlankGrid()
        {
            throw new NotImplementedException();
        }

        protected override void SetAxisSize()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
#endif
}