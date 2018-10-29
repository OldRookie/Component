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

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="oraginalimg">原图数组</param>
        /// <param name="width">缩略图宽</param>
        /// <param name="height">缩略图高</param>
        /// <param name="mode">哪种方式生成缩略图，默认为百分比压缩</param>
        /// <returns></returns>
        public static byte[] ResizeImageBytesToBytes(byte[] oraginalimg, int width, int height, string mode = "DB")
        {
            byte[] s = null;
            MemoryStream ms = new MemoryStream(oraginalimg);
            Bitmap bm = new Bitmap(ms);
            //System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = bm.Width;
            int oh = bm.Height;
            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = bm.Height * width / bm.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = bm.Width * height / bm.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    if ((double)bm.Width / (double)bm.Height > (double)towidth / (double)toheight)
                    {
                        oh = bm.Height;
                        ow = bm.Height * towidth / toheight;
                        y = 0;
                        x = (bm.Width - ow) / 2;
                    }
                    else
                    {
                        ow = bm.Width;
                        oh = bm.Width * height / towidth;
                        x = 0;
                        y = (bm.Height - oh) / 2;
                    }
                    break;
                case "DB"://等比缩放（不变形，如果高大按高，宽大按宽缩放） 
                    if ((double)bm.Width / (double)towidth < (double)bm.Height / (double)toheight)
                    {
                        toheight = height;
                        towidth = bm.Width * height / bm.Height;
                    }
                    else
                    {
                        towidth = width;
                        toheight = bm.Height * width / bm.Width;
                    }
                    break;
                default:
                    break;
            }
            try
            {
                //新建一个bmp图片
                System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
                //新建一个画板
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //清空画布并以透明背景色填充
                g.Clear(System.Drawing.Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(bm, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

                //把图片转byte数组
                MemoryStream ms1 = new MemoryStream();
                bitmap.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
                s = ms1.ToArray();
                bm.Dispose();
                bitmap.Dispose();
            }
            catch (Exception ex)
            {

            }
            return s;
        }

        public static byte[] ResizeImage(Stream fileStream, int width, int height, string mode = "DB")
        {
            using (MemoryStream ms = new MemoryStream())
            {
                fileStream.Position = 0;
                fileStream.CopyTo(ms);
                return ResizeImageBytesToBytes(ms.ToArray(), width, height, mode); ;
            }
        }
    }
}
