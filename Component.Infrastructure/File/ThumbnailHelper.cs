using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure
{
    public class ThumbnailHelper
    {
        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param>     
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            // 生成的缩略图在上述"画布"上的位置
            int X = 0;
            int Y = 0;

            string extension = Path.GetExtension(originalImagePath);
            if (!PathHelper.IsValidImageFormat(extension))
            {
                throw new BadImageFormatException($"图片格式不正确，不支持{extension}文件格式");
            }

            using (Image imageFrom = Image.FromFile(originalImagePath))
            {
                //根据图片exif调整方向 
                RotateImage(imageFrom);

                // 源图宽度及高度 
                int imageFromWidth = imageFrom.Width;
                int imageFromHeight = imageFrom.Height;

                // 创建画布
                using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
                {
                    bmp.SetResolution(imageFrom.HorizontalResolution, imageFrom.VerticalResolution);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        // 用白色清空 
                        g.Clear(Color.White);

                        // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        // 指定高质量、低速度呈现。 
                        g.SmoothingMode = SmoothingMode.HighQuality;

                        // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
                        g.DrawImage(imageFrom, new Rectangle(X, Y, width, height),
                            new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);

                        //以jpg格式保存缩略图 
                        bmp.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }

        /// <summary>  
        /// 根据图片exif调整方向  
        /// </summary>  
        /// <param name="img"></param>  
        /// <returns></returns>  
        public static Image RotateImage(Image img)
        {
            var exif = img.PropertyItems;
            byte orien = 0;
            var item = exif.Where(m => m.Id == 274).ToArray();
            if (item.Length > 0)
                orien = item[0].Value[0];
            switch (orien)
            {
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);//horizontal flip  
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);//right-top  
                    break;
                case 4:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);//vertical flip  
                    break;
                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);//right-top  
                    break;
                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);//left-bottom  
                    break;
                default:
                    break;
            }
            return img;
        }


        /// <summary>
        /// 生成设计作品缩略图
        /// </summary>
        /// <param name="originalImagePath">原始图片文件地址</param>
        /// <param name="thumbnailPath">缩略图文件地址</param>
        public static void MakeDesignWork(string originalImagePath, string thumbnailPath)
        {
            MakeThumbnail(originalImagePath, thumbnailPath, 140, 140, "HW");
        }
    }
}
