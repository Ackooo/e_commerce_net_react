namespace Infrastructure.Excel;

using System.Reflection;
using System.Text.RegularExpressions;
using Domain.Shared.Attributes;
using Domain.Shared.Enums;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming;

public class ExcelExporter
{
    private const int MaxInMemoryRows = 100;
    private readonly List<string> _headers = [];

    public void ExportToExcel<T>(string filePath, string sheetName, List<T> exportData)
    {
        var workbook = GenerateDocument(sheetName, exportData);

        // Save to file
        //string filePath = @"C:\path\to\file.xlsx";
        using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        workbook.Write(fs);
    }

    #region Helper

    private SXSSFWorkbook GenerateDocument<T>(string sheetName, List<T> exportData)
    {
        var workbook = new SXSSFWorkbook(MaxInMemoryRows);
        var sheet = workbook.CreateSheet(sheetName);

        var properties = GetExportableProperties<T>();

        GenerateHeaderCells(workbook, sheet);

        GenerateCells(workbook, sheet, exportData, properties);

        SetColumnWidths(sheet, properties);

        return workbook;
    }

    private List<PropertyInfo> GetExportableProperties<T>()
    {
        var properties = typeof(T).GetProperties();
        var exportableProps = new List<PropertyInfo>();

        foreach(var prop in properties)
        {
            var exportAttr = prop.GetCustomAttribute<ExportAttribute>();
            if(exportAttr == null || exportAttr.ExportAllowed == ExportAllowed.No)
            {
                continue;
            }

            //TODO:
            //Generate regex expression at the compile-time
            var headerName = exportAttr?.Name
                ?? Regex.Replace(prop.Name, "([A-Z])", " $1").Trim();

            _headers.Add(headerName);
            exportableProps.Add(prop);
        }

        return exportableProps;
    }


    private void GenerateHeaderCells(SXSSFWorkbook workbook, ISheet sheet)
    {
        var headerRow = sheet.CreateRow(0);
        for(var i = 0; i < _headers.Count; i++)
        {
            var cell = headerRow.CreateCell(i);
            cell.SetCellValue(_headers[i]);
            cell.CellStyle = CreateHeaderStyle(workbook);
        }
    }

    private static void GenerateCells<T>(SXSSFWorkbook workbook, ISheet sheet, List<T> exportData, List<PropertyInfo> properties)
    {
        var cellStyle = CreateCellStyle(workbook);
        for(var rowIndex = 0; rowIndex < exportData.Count; rowIndex++)
        {
            var item = exportData[rowIndex];
            var sheetRow = sheet.CreateRow(rowIndex + 1);

            for(var columnIndex = 0; columnIndex < properties.Count; columnIndex++)
            {
                var properyInfo = properties[columnIndex];
                var cell = sheetRow.CreateCell(columnIndex);
                cell.CellStyle = cellStyle;

                var value = properyInfo.GetValue(item);
                CreateCell(sheetRow, columnIndex, value, cellStyle);
            }
        }
    }

    private static void CreateCell(IRow row, int columnIndex, object? value, ICellStyle style)
    {
        ICell cell = row.CreateCell(columnIndex);
        if(value is null)
            cell.SetCellValue(string.Empty);
        else if(value is int intVal)
            cell.SetCellValue(intVal);
        else if(value is double doubleVal)
            cell.SetCellValue(doubleVal);
        else if(value is DateTime dateTimeVal)
            cell.SetCellValue(dateTimeVal.ToString("yyyy-MM-dd HH:mm:ss"));
        else if(value.GetType().IsEnum)
            cell.SetCellValue(Enum.GetName(value.GetType(), value));
        else
            cell.SetCellValue(value?.ToString() ?? string.Empty);

        cell.CellStyle = style;
    }

    #endregion

    #region Style

    private static ICellStyle CreateHeaderStyle(SXSSFWorkbook workbook)
    {
        var style = workbook.CreateCellStyle();
        var font = workbook.CreateFont();
        font.IsBold = true;
        font.FontHeightInPoints = 11;
        font.FontName = "Arial";

        style.SetFont(font);
        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        style.FillPattern = FillPattern.SolidForeground;

        return style;
    }

    private static ICellStyle CreateCellStyle(SXSSFWorkbook workbook)
    {
        var style = workbook.CreateCellStyle();
        style.Alignment = HorizontalAlignment.Left;
        style.VerticalAlignment = VerticalAlignment.Center;

        style.BorderTop = BorderStyle.Thin;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;

        return style;
    }

    private void SetColumnWidths(ISheet sheet, List<PropertyInfo> properties)
    {
        const int charWidth = 256; // NPOI units per character            
        const int padding = 1; // extra chars for padding

        for(var i = 0; i < properties.Count; i++)
        {
            var propertyType = properties[i].PropertyType;
            var typeBasedWidth = propertyType switch
            {
                Type t when t == typeof(DateTime) => 20 * charWidth,
                Type t when t == typeof(decimal) || t == typeof(double) => 32 * charWidth,
                Type t when t == typeof(int) || t == typeof(short) => 10 * charWidth,
                Type t when t == typeof(bool) => 8 * charWidth,
                Type t when t == typeof(string) => 25 * charWidth,
                _ => 25 * charWidth
            };

            // Calculate width needed for header text
            var headerWidth = (_headers[i].Length + padding) * charWidth;
            // Use the larger of the two
            var width = Math.Max(typeBasedWidth, headerWidth);

            sheet.SetColumnWidth(i, width);
        }
    }

    #endregion

}
