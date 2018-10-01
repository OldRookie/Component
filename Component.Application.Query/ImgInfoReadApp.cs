using Component.Infrastructure.Excel;
using Component.Infrastructure.AttributeExt;
using Component.Infrastructure.Excel;
using Component.Infrastructure.FileUpload;
using Component.Model.ViewModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.SS.Util;

namespace Component.Application.Read
{
    public class ImgInfoReadApp
    {

        public byte[] Export(List<ImgInfoVM> data)
        {
            //var wb = new HSSFWorkbook();
            var wb = new XSSFWorkbook();
            var sheet = wb.CreateSheet("图片列表");
            sheet.CreateFreezePane(0, 1);
            var rowTitle = sheet.CreateRow(0);
            rowTitle.HeightInPoints = 19.5F;
            var indexToTitleName = new Dictionary<int, string>();
            var indexToPropName = new Dictionary<int, string>();

            var dict = typeof(ImgInfoVM).GetEntityPropertyDict();
            for (int i = 0; i < dict.Keys.Count; i++)
            {
                indexToTitleName.Add(i, dict[dict.Keys.ToArray()[i]]);
                indexToPropName.Add(i, dict.Keys.ToArray()[i]);
            }
            var propNameToIndex = indexToPropName.ToDictionary(x => x.Value, y => y.Key);


            #region 设置列头单元格样式                
            var cssTitle = wb.CreateCellStyle(); //创建列头样式
            cssTitle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; //水平居中
            cssTitle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直居中
            var cssTitleFont = wb.CreateFont(); //创建字体
            cssTitleFont.IsBold = true; //字体加粗
            cssTitleFont.FontHeightInPoints = 12; //字体大小
            cssTitle.SetFont(cssTitleFont); //将字体绑定到样式
            #endregion

            //生成列头
            for (int i = 0; i < indexToTitleName.Keys.Count; i++)
            {
                var cellTitle = rowTitle.CreateCell(i); //创建单元格
                cellTitle.CellStyle = cssTitle; //将样式绑定到单元格
                cellTitle.SetCellValue(indexToTitleName[i]);
            }

            for (int i = 0; i < data.Count; i++)
            {
                var dataItem = data[i];

                #region 生成内容单元格
                var rowContent = sheet.CreateRow(sheet.LastRowNum + 1); //创建行
                rowContent.Height = 80 * 20;
                //rowContent.HeightInPoints = 16;

                for (int iProp = 0; iProp < indexToPropName.Keys.Count; iProp++)
                {
                    var cellConent = rowContent.CreateCell(iProp); //创建单元格
                    if (indexToPropName[iProp] == nameof(ImgInfoVM.FileFullPath))
                    {
                        var rowline = sheet.LastRowNum;
                        var prop = dataItem.GetType().GetProperty(indexToPropName[iProp]);
                        var filePath = (string)prop.GetValue(dataItem, null);
                        byte[] bytes = new PathFileUpload().GetFile(filePath);
                        if (bytes != null)
                        {
                            PictureType pictureType = PictureType.None;
                            var extensionName = Path.GetExtension(filePath).Substring(1).ToUpper();
                            Enum.TryParse(extensionName, out pictureType);
                            if (!string.IsNullOrEmpty(extensionName)
                                && extensionName.Equals("jpg", StringComparison.CurrentCultureIgnoreCase))
                            {
                                pictureType = PictureType.JPEG;
                            }

                            if (pictureType != PictureType.None)
                            {
                                int pictureIdx = wb.AddPicture(bytes, pictureType);
                                var patriarch = sheet.CreateDrawingPatriarch();
                                //var anchor = new HSSFClientAnchor(70, 10, 0, 0, iProp, rowline, iProp + 1, rowline + 1);
                                var anchor = new XSSFClientAnchor(70, 10, 0, 0, iProp, rowline, iProp + 1, rowline + 1);
                                var pict = patriarch.CreatePicture(anchor, pictureIdx);
                            }
                        }
                    }
                    else
                    {
                        cellConent.SetCellData(indexToPropName[iProp], dataItem);
                        cellConent.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; //水平居中
                        cellConent.CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直居中
                    }
                }
                #endregion
            }

            sheet.AutoSizeALLColumn(dict.Keys.Count);
            sheet.SetColumnWidth(propNameToIndex[nameof(ImgInfoVM.FileFullPath)], 20 * 256);
            var fileStream = new MemoryStream();
            wb.Write(fileStream);
            fileStream.Close();
            var buf = fileStream.ToArray();
            return buf;
        }


        public byte[] ExportComplexData(List<ImgInfoExportVM> data)
        {
            //var wb = new HSSFWorkbook();
            var wb = new XSSFWorkbook();
            var sheet = wb.CreateSheet("复杂数据");
            sheet.CreateFreezePane(0, 1);
            var rowTitle = sheet.CreateRow(0);
            rowTitle.HeightInPoints = 19.5F;
            var indexToTitleName = new Dictionary<int, string>();
            var indexToPropName = new Dictionary<int, string>();

            var dict = typeof(ImgInfoExportVM).GetEntityPropertyDict();
            for (int i = 0; i < dict.Keys.Count; i++)
            {
                indexToTitleName.Add(i, dict[dict.Keys.ToArray()[i]]);
                indexToPropName.Add(i, dict.Keys.ToArray()[i]);
            }
            var propNameToIndex = indexToPropName.ToDictionary(x => x.Value, y => y.Key);


            #region 设置列头单元格样式                
            var cssTitle = wb.CreateCellStyle(); //创建列头样式
            cssTitle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; //水平居中
            cssTitle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直居中
            var cssTitleFont = wb.CreateFont(); //创建字体
            cssTitleFont.IsBold = true; //字体加粗
            cssTitleFont.FontHeightInPoints = 12; //字体大小
            cssTitle.SetFont(cssTitleFont); //将字体绑定到样式
            #endregion

            //生成列头
            for (int i = 0; i < indexToTitleName.Keys.Count; i++)
            {
                var cellTitle = rowTitle.CreateCell(i); //创建单元格
                cellTitle.CellStyle = cssTitle; //将样式绑定到单元格
                cellTitle.SetCellValue(indexToTitleName[i]);
            }

            for (int i = 0; i < data.Count; i++)
            {
                var dataItem = data[i];

                #region 生成内容单元格
                var rowContent = sheet.CreateRow(sheet.LastRowNum + 1); //创建行
                rowContent.Height = 80 * 20;
                //rowContent.HeightInPoints = 16;

                for (int iProp = 0; iProp < indexToPropName.Keys.Count; iProp++)
                {
                    var cellConent = rowContent.CreateCell(iProp); //创建单元格


                    cellConent.SetCellData(indexToPropName[iProp], dataItem);
                    cellConent.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center; //水平居中
                    cellConent.CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直居中
                }
                #endregion
            }

            //约束
            CellRangeAddressList regions = new CellRangeAddressList(1, 65535,
                         propNameToIndex[nameof(ImgInfoExportVM.IsSelected)],
                         propNameToIndex[nameof(ImgInfoExportVM.IsSelected)]);

            XSSFDataValidationHelper dvHelper = new XSSFDataValidationHelper((XSSFSheet)sheet);
            XSSFDataValidationConstraint dvConstraint = (XSSFDataValidationConstraint)dvHelper
                    .CreateExplicitListConstraint(new string[] { "√", "×" });
            XSSFDataValidation dataValidate = (XSSFDataValidation)dvHelper.CreateValidation(dvConstraint, regions);
            sheet.AddValidationData(dataValidate);

            //数据源
            CellRangeAddressList dataSourceRegions = new CellRangeAddressList(1, 65535,
                 propNameToIndex[nameof(ImgInfoExportVM.DataSource)],
                 propNameToIndex[nameof(ImgInfoExportVM.DataSource)]);
            var dataSource = new List<string>() { "Text1", "Text2" }.ToArray();

            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 3));

            sheet.SetXXSDropDownList(dataSource, wb, dataSourceRegions, "DataSource");

            sheet.AutoSizeALLColumn(dict.Keys.Count);
            sheet.SetColumnWidth(propNameToIndex[nameof(ImgInfoExportVM.FileFullPath)], 20 * 256);
            var fileStream = new MemoryStream();
            wb.Write(fileStream);
            fileStream.Close();
            var buf = fileStream.ToArray();
            return buf;
        }
    }
}
