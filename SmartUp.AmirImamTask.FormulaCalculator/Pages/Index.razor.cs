using SmartUp.AmirImamTask.FormulaCalculator.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;

namespace SmartUp.AmirImamTask.FormulaCalculator.Pages;

public partial class Index
{
    [Inject]
    private IJSRuntime? Js { get; set; }
    
    private TableModel model = new();
    private ErrorsCollection errorsList = new();
    private List<TableModel> modelsList = new();
    private Dictionary<int, List<TableModel>> reflectedList = new();
    protected override Task OnInitializedAsync()
    {
        //Uncomment this if you want to fill modelsList with the same example values
        //InitialzeModelsList();
        return base.OnInitializedAsync();
    }
    
    private void InitialzeModelsList()
    {
        modelsList = new()
        {

        new()
        {
            FieldName = "F1",
            FieldDescription = "Tax%",
            Formula = null,
            ColumnIndex = 0,
            RowIndex = 0
        },
        new()
        {
            FieldName = "F2",
            FieldDescription = "Sales",
            Formula = null,
            ColumnIndex = 1,
            RowIndex = 0
        },
        new()
        {
            FieldName = "F3",
            FieldDescription = "Tax Value",
            Formula = "(F1/100) * F2",
            ColumnIndex = 2,
            RowIndex = 0
        },
        };
    }
    
    
    private void AddModel()
    {
        errorsList.Clear();
        if (String.IsNullOrEmpty(model.FieldName))
        {
            errorsList.Add("FieldName", "Field name is required");
        }
        if (String.IsNullOrEmpty(model.FieldDescription))
        {
            errorsList.Add("FieldDescription", "Field description is required");
        }
        if (modelsList.Select(it => it.FieldName.ToLower()).Contains(model.FieldName.ToLower()))
        {
            errorsList.Add("FieldName", "Field name must be unique");
        }
        Regex regex = new(@"[A-Za-z][\w$]*(\.[\w$]+)?(\[\d+])?");
        if (regex.IsMatch(model.FieldName) == false || model.FieldName.Contains(" "))
        {
            errorsList.Add("FieldName", "Invalid name");
        }

        if (errorsList.Count > 0)
        {
            return;
        }

        modelsList.Add(new()
        {
            FieldName = model.FieldName,
            FieldDescription = model.FieldDescription,
            ColumnIndex = model.ColumnIndex,
            Formula = model.Formula,
            RowIndex = model.RowIndex,
        });
        model = new();
    }

    private void Reflect()
    {
        errorsList.Clear();
        if (modelsList.Any(it => String.IsNullOrEmpty(it.Formula) == false) == false)
        {
            errorsList.Add("EmptyFormula", "Must add formula");
            return;
        }
        reflectedList = modelsList
            .OrderBy(it => it.RowIndex)
            .GroupBy(it => it.RowIndex)
            .ToDictionary(k => k.Key, v => v.OrderBy(it => it.ColumnIndex).ToList());
    }

    private async void Calculate()
    {
        errorsList.Clear();
        if (reflectedList.Count == 0)
        {
            errorsList.Add("ReflectedList", "You must reflect list first");
            return;
        }
        var formulaModel = modelsList.FirstOrDefault(it => !String.IsNullOrEmpty(it.Formula));
        string? formula = formulaModel?.Formula;
        foreach (var item in modelsList)
        {
            if (String.IsNullOrEmpty(item.Formula) == false)
            {
                continue;
            }
            formula = formula!.Replace(item.FieldName, item.Value.ToString());
        }
        var result = await Js!.InvokeAsync<double>("eval", formula);
        formulaModel!.Value = result;
        StateHasChanged();
    }
}