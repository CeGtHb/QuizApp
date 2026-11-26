using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace QuizApp.Components;

public partial class MathFormula : ComponentBase
{
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Latex { get; set; } = string.Empty;

    private ElementReference element;
    private string? renderedLatex;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (renderedLatex == Latex)
        {
            return;
        }

        var latex = Latex;

        await JS.InvokeVoidAsync(
            "mathJaxInterop.render",
            element,
            latex);

        renderedLatex = latex;
    }
}