using Component.Infrastructure.AttributeExt;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Component.Infrastructure.Excel
{
    public static class ExcelHelper
    {
        public static List<T> ReadExcel<T>(Stream excelStream, string sheetName, Dictionary<string, int> nameToindex,
            int startRow = 0) where T : new()
        {
            var list = new List<T>();
            excelStream.Position = 0;
            IWorkbook workbook = WorkbookFactory.Create(excelStream);

            return ReadExcel<T>(workbook, sheetName, nameToindex, startRow);
        }
        public static List<T> ReadExcel<T>(IWorkbook workbook, string sheetName, Dictionary<string, int> nameToindex,
            int startRow = 0) where T : new()
        {
            var list = new List<T>();
            var sheet = workbook.GetSheet(sheetName);
            var GetRowEnumerator = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(startRow);
            for (int i = startRow + 1; i <= sheet.PhysicalNumberOfRows; i++)
            {
                var row = sheet.GetRow(i);
                var t = new T();
                if (row == null) continue;

                foreach (var prop in typeof(T).GetProperties())
                {
                    if (nameToindex.ContainsKey(prop.Name))
                    {
                        var columnInex = nameToindex[prop.Name];
                        Type type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        var cellVal = GetCellData(row.GetCell(columnInex), type.FullName);
                        if (cellVal != null
                            && !string.IsNullOrEmpty(cellVal.ToString()))
                        {
                            var val = Convert.ChangeType(cellVal, type);
                            prop.SetValue(t, val);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }

        public static List<T> ReadExcel<T>(IWorkbook workbook, string sheetName,
            int startRow = 0) where T : new()
        {
            Dictionary<string, int> nameToindex = new Dictionary<string, int>();

            var dict = typeof(T).GetEntityPropertyDict();
            for (int i = 0; i < dict.Keys.Count; i++)
            {
                nameToindex.Add(dict.Keys.ToArray()[i], i);
            }
            return ReadExcel<T>(workbook, sheetName, nameToindex, startRow);
        }

        private static object GetCellData(ICell cell, string typeName)
        {
            if (cell == null || cell.CellType == CellType.Blank)
            {
                return null;
            }
            if (typeName.IndexOf("DateTime") != -1)
            {
                if (cell.CellType == CellType.String)
                {
                    DateTime dt;
                    if (DateTime.TryParse(cell.StringCellValue, out dt))
                    {
                        return dt;
                    }
                    return null;
                }
                else
                {
                    return cell.DateCellValue;
                }
            }
            switch (cell.CellType)
            {
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;

                default: return cell.StringCellValue;
            }
        }

        public static void SetCellData(this ICell cell, string propName, object obj)
        {
            var prop = obj.GetType().GetProperty(propName);
            if (prop != null)
            {
                var value = prop.GetValue(obj, null);

                if (value == null)
                {
                    return;
                }
                cell.CellStyle.DataFormat = 0;

                Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                if (prop.PropertyType.FullName.Contains("Int"))
                {
                    cell.SetCellType(CellType.String);
                    if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                    {
                        cell.SetCellValue(((int?)value).Value.ToString());
                    }
                    else
                    {
                        cell.SetCellValue(((int)value).ToString());
                    }
                }
                else if (prop.PropertyType.FullName.Contains("Double"))
                {
                    cell.SetCellType(CellType.String);
                    if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                    {
                        cell.SetCellValue(((double?)value).Value);
                    }
                    else
                    {
                        cell.SetCellValue((double)value);
                    }
                }
                else if (prop.PropertyType.FullName.Contains("DateTime")
                    && Nullable.GetUnderlyingType(prop.PropertyType) != null)
                {
                    var dateFormat = "yyyy/MM/dd HH:mm:ss";
                    var builtinFormatId = HSSFDataFormat.GetBuiltinFormat(dateFormat);

                    if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                    {
                        cell.SetCellValue(((DateTime?)value).Value);
                    }
                    else
                    {
                        cell.SetCellValue((DateTime)value);
                    }
                    cell.SetCellType(CellType.Numeric);

                    var prevCell = cell.Sheet.GetRow(cell.RowIndex - 1).GetCell(cell.ColumnIndex);
                    if (prevCell == null || prevCell.CellStyle.DataFormat == 0)
                    {
                        cell.CellStyle = cell.Sheet.Workbook.CreateCellStyle();
                    }
                    else
                    {
                        cell.CellStyle = prevCell.CellStyle;
                    }
                    if (builtinFormatId != -1)
                    {
                        cell.CellStyle.DataFormat = builtinFormatId;
                    }
                    else
                    {
                        // not a built-in format, so create a new one
                        var dataformat = cell.Sheet.Workbook.CreateDataFormat();
                        cell.CellStyle.DataFormat = dataformat.GetFormat(dateFormat);
                    }
                }
                else
                {
                    cell.SetCellValue(value.ToString());
                }
            }
        }

        public static void AutoSizeALLColumn(this ISheet sheet, int maxColumn)
        {
            for (int i = 0; i <= maxColumn; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            //获取当前列的宽度，然后对比本列的长度，取最大值
            for (int columnNum = 0; columnNum <= maxColumn; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 0; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, (columnWidth + 1) * 256);
            }
        }

        public static void SetDropDownList(this ISheet sheet, string[] datas, XSSFWorkbook workbook,
            CellRangeAddressList addressList, string formulaName)
        {
            var hiddenSheetName = "HiddenDataSource" + DateTime.Now.ToString("yyyyMMddHHmmss");
            ISheet CourseSheet = workbook.CreateSheet(hiddenSheetName);
            workbook.SetSheetHidden(workbook.GetSheetIndex(hiddenSheetName), true);
            //CourseSheet.CreateRow(0).CreateCell(0).SetCellValue("");
            IRow row = null;
            ICell cell = null;
            for (int i = 0; i < datas.Length; i++)
            {
                row = CourseSheet.CreateRow(i);
                cell = row.CreateCell(0);
                cell.SetCellValue(datas[i]);
            }

            IName range = workbook.CreateName();
            range.RefersToFormula = string.Format("{0}!$A$1:$A${1}", hiddenSheetName, datas.Length);
            range.NameName = formulaName;
            DVConstraint constraint = DVConstraint.CreateFormulaListConstraint(formulaName);
            HSSFDataValidation dataValidate = new HSSFDataValidation(addressList, constraint);
            sheet.AddValidationData(dataValidate);
        }

        public static void SetXXSDropDownList(this ISheet sheet, string[] datas, XSSFWorkbook workbook,
           CellRangeAddressList addressList, string formulaName)
        {
            var hiddenSheetName = "HiddenDataSource" + DateTime.Now.ToString("yyyyMMddHHmmss");
            ISheet CourseSheet = workbook.CreateSheet(hiddenSheetName);
            workbook.SetSheetHidden(workbook.GetSheetIndex(hiddenSheetName), true);
            //CourseSheet.CreateRow(0).CreateCell(0).SetCellValue("");
            IRow row = null;
            ICell cell = null;
            for (int i = 0; i < datas.Length; i++)
            {
                row = CourseSheet.CreateRow(i);
                cell = row.CreateCell(0);
                cell.SetCellValue(datas[i]);
            }

            XSSFDataValidationHelper dvHelper = new XSSFDataValidationHelper((XSSFSheet)sheet);
            XSSFDataValidationConstraint dvConstraint = (XSSFDataValidationConstraint)dvHelper
                    .CreateFormulaListConstraint(formulaName);

            IName range = workbook.CreateName();
            range.RefersToFormula = string.Format("{0}!$A$1:$A${1}", hiddenSheetName, datas.Length);
            range.NameName = formulaName;

            XSSFDataValidation validation = (XSSFDataValidation)dvHelper.CreateValidation(dvConstraint, addressList);
            sheet.AddValidationData(validation);
        }
    }
}
