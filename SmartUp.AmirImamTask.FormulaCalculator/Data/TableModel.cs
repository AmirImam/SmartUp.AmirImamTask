namespace SmartUp.AmirImamTask.FormulaCalculator.Data;

public class TableModel
{
    public string FieldName { get; set; } = string.Empty;
    public string? FieldDescription { get; set; }
    public string? Formula { get; set; }
    public double Value { get; set; }
    public int ColumnIndex { get; set; }
    public int RowIndex { get; set; }
}
