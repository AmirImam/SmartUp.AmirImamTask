using Microsoft.AspNetCore.Components;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SmartUp.AmirImamTask.FormulaCalculator.Data;

public class ErrorsCollection : IDictionary<string, List<string>>
{
    private Dictionary<string, List<string>> source = new();
    public List<string> this[string key] { get => source[key]; set => source[key] = value; }

    public ICollection<string> Keys => source.Keys;

    public ICollection<List<string>> Values => source.Values;

    public int Count => source.Count;

    public bool IsReadOnly => false;

    public void Add(string key, List<string> value)
        => source.Add(key, value);

    public void Add(KeyValuePair<string, List<string>> item)
        => source.Add(item.Key, item.Value);

    public void Add(string key, string error)
    {
        if (source.ContainsKey(key) == false)
        {
            source[key] = new List<string>();
        }
        source[key].Add(error);
    }

    public RenderFragment GetErrorsListAsHtml(string key) =>
        source.ContainsKey(key) == false || source[key] == null || source[key].Count == 0 ? SuccessFragment() :
        builder =>
    {
        var errors = source[key].ToList();
        builder.OpenElement(0, "ul");
        foreach (string error in errors)
        {
            builder.OpenElement(1, "li");
                builder.OpenElement(2, "small");
                builder.AddAttribute(2, "class", "text-danger");
                builder.AddContent(2, error);
                builder.CloseElement();
            builder.CloseElement();
        }
        builder.CloseElement();


    };

    private RenderFragment SuccessFragment() => builder =>
    {
        builder.OpenElement(0, "p");
        builder.AddAttribute(1, "class", "text-success");
        builder.CloseElement();
    };
    public void Clear() => source.Clear();

    public bool Contains(KeyValuePair<string, List<string>> item) => source.Contains(item);

    public bool ContainsKey(string key) => source.ContainsKey(key);

    public void CopyTo(KeyValuePair<string, List<string>>[] array, int arrayIndex) =>
        array.CopyTo(source.ToArray(), arrayIndex);


    public IEnumerator<KeyValuePair<string, List<string>>> GetEnumerator() => source.GetEnumerator();

    public bool Remove(string key) => source.Remove(key);

    public bool Remove(KeyValuePair<string, List<string>> item) => source.Remove(item.Key);

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out List<string> value) => source.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();
}
