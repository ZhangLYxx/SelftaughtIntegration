using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Excel
{
    public class WriteImage
    {
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="imageBytes"></param>
        /// <param name="rowNum"></param>照片
        /// <param name="columnNum"></param>
        /// <param name="autofit"></param>
        public static void InsertImage(ExcelWorksheet worksheet, byte[] imageBytes, int rowNum, int columnNum, bool autofit)
        {
            using (var image = Image.FromStream(new MemoryStream(imageBytes)))
            {                
                var picture = worksheet.Drawings.AddPicture($"image_{DateTime.Now.Ticks}", new FileInfo($"image_{DateTime.Now.Ticks}"));
                var cell = worksheet.Cells[rowNum, columnNum];
                int cellColumnWidthInPix = GetWidthInPixels(cell);
                int cellRowHeightInPix = GetHeightInPixels(cell);
                int adjustImageWidthInPix = cellColumnWidthInPix;
                int adjustImageHeightInPix = cellRowHeightInPix;
                if (autofit)
                {
                    //图片尺寸适应单元格
                    var adjustImageSize = GetAdjustImageSize(image, cellColumnWidthInPix, cellRowHeightInPix);
                    adjustImageWidthInPix = adjustImageSize.Item1;
                    adjustImageHeightInPix = adjustImageSize.Item2;
                }
                //设置为居中显示
                int columnOffsetPixels = (int)((cellColumnWidthInPix - adjustImageWidthInPix) / 2.0);
                int rowOffsetPixels = (int)((cellRowHeightInPix - adjustImageHeightInPix) / 2.0);
                picture.SetSize(adjustImageWidthInPix, adjustImageHeightInPix);
                picture.SetPosition(rowNum - 1, rowOffsetPixels, columnNum - 1, columnOffsetPixels);
            }
        }

        /// <summary>
        /// 获取自适应调整后的图片尺寸
        /// </summary>
        /// <param name="image"></param>
        /// <param name="cellColumnWidthInPix"></param>
        /// <param name="cellRowHeightInPix"></param>
        /// <returns>item1:调整后的图片宽度; item2:调整后的图片高度</returns>
        private static Tuple<int, int> GetAdjustImageSize(Image image, int cellColumnWidthInPix, int cellRowHeightInPix)
        {
            int imageWidthInPix = image.Width;
            int imageHeightInPix = image.Height;
            //调整图片尺寸,适应单元格
            int adjustImageWidthInPix;
            int adjustImageHeightInPix;
            if (imageHeightInPix * cellColumnWidthInPix > imageWidthInPix * cellRowHeightInPix)
            {
                //图片高度固定,宽度自适应
                adjustImageHeightInPix = cellRowHeightInPix;
                double ratio = (1.0) * adjustImageHeightInPix / imageHeightInPix;
                adjustImageWidthInPix = (int)(imageWidthInPix * ratio);
            }
            else
            {
                //图片宽度固定,高度自适应
                adjustImageWidthInPix = cellColumnWidthInPix;
                double ratio = (1.0) * adjustImageWidthInPix / imageWidthInPix;
                adjustImageHeightInPix = (int)(imageHeightInPix * ratio);
            }
            return new Tuple<int, int>(adjustImageWidthInPix, adjustImageHeightInPix);
        }

        /// <summary>
        /// 获取单元格的宽度(像素)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static int GetWidthInPixels(ExcelRange cell)
        {
            double columnWidth = cell.Worksheet.Column(cell.Start.Column).Width;
            Font font = new Font(cell.Style.Font.Name, cell.Style.Font.Size, FontStyle.Regular);
            double pxBaseline = Math.Round(MeasureString("1234567890", font) / 10);
            return (int)(columnWidth * pxBaseline);
        }

        /// <summary>
        /// 获取单元格的高度(像素)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static int GetHeightInPixels(ExcelRange cell)
        {
            double rowHeight = cell.Worksheet.Row(cell.Start.Row).Height;
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiY = graphics.DpiY;
                return (int)(rowHeight * (1.0 / 800) * dpiY);
            }
        }

        /// <summary>
        /// MeasureString
        /// </summary>
        /// <param name="s"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        private static float MeasureString(string s, Font font)
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                return g.MeasureString(s, font, int.MaxValue, StringFormat.GenericTypographic).Width;
            }
        }
    }
}
