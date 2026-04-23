using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.CodeCompletion;
using Pororoca.Desktop.Controls;
using Pororoca.Desktop.Others;
using Pororoca.Desktop.TextEditorConfig;
using Pororoca.Desktop.ViewModels;
using Pororoca.Domain.Features.Common;
using Pororoca.Domain.Features.VariableResolution;

namespace Pororoca.Desktop.Views;

public sealed class HttpRepeaterView : UserControl, IPororocaVariableResolverProvider
{
    private readonly AvaloniaEdit.TextMate.TextMate.Installation rawInputDataEditorTextMateInstallation;
    private string? currentRawInputDataSyntaxLangId;

    private CompletionWindow? rawInputDataCompletionWindow;

    private readonly AvaloniaEdit.TextMate.TextMate.Installation httpResRawBodyEditorTextMateInstallation;
    private string? currentHttpResRawBodySyntaxLangId;

    private readonly PororocaVariableSyntaxHighlightingDefinitionSet syntaxHighlightingDefinitionSet;

    public HttpRepeaterView()
    {
        InitializeComponent();

        this.syntaxHighlightingDefinitionSet = new(ProvideVariableResolver);

        var tbRepetitionInputDataFileSrcPath = this.FindControl<SyntaxHighlightingTextBox>("tbRepetitionInputDataFileSrcPath")!;
        tbRepetitionInputDataFileSrcPath.DefinitionSet = this.syntaxHighlightingDefinitionSet;

        var rawInputDataEditor = this.FindControl<TextEditor>("teRepetitionInputDataRaw")!;
        this.rawInputDataEditorTextMateInstallation = TextEditorConfiguration.Setup(rawInputDataEditor, true, ProvideVariableResolver);
        this.rawInputDataEditorTextMateInstallation.SetEditorSyntax(ref this.currentRawInputDataSyntaxLangId, MimeTypesDetector.DefaultMimeTypeForJson);
        rawInputDataEditor.TextArea.TextEntering += (sender, e) => TextEditorConfiguration.OnTextEnteringInEditorWithVariables(rawInputDataEditor, e, ref this.rawInputDataCompletionWindow);
        rawInputDataEditor.TextArea.TextEntered += (sender, e) => TextEditorConfiguration.OnTextEnteredInEditorWithVariables(rawInputDataEditor, e, ProvideVariableResolver, ref this.rawInputDataCompletionWindow, (sender, e) => this.rawInputDataCompletionWindow = null);

        var httpResRawBodyEditor = this.FindControl<TextEditor>("ResponseBodyRawContentEditor")!;
        this.httpResRawBodyEditorTextMateInstallation = TextEditorConfiguration.Setup(httpResRawBodyEditor!, false, null);
        httpResRawBodyEditor.DocumentChanged += OnResponseRawBodyEditorDocumentChanged;
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public void StartOrStopRepetition(object sender, RoutedEventArgs args)
    {
        var vm = (HttpRepeaterViewModel)DataContext!;
        Dispatcher.UIThread.Post(async () => await vm.StartOrStopRepetitionAsync());
    }

    private void OnResponseRawBodyEditorDocumentChanged(object? sender, EventArgs e)
    {
        var vm = (HttpRepeaterViewModel)DataContext!;
        var resVm = vm.ResponseDataCtx;
        this.httpResRawBodyEditorTextMateInstallation.SetEditorSyntax(ref this.currentHttpResRawBodySyntaxLangId, resVm.ResponseRawContentType);
    }

    // IMPORTANTE: este método deve retornar um CollectionViewModel,
    // e não simplesmente uma coleção, pois senão não vai atualizar
    // as variáveis de coleção e de ambiente.
    public IPororocaVariableResolver ProvideVariableResolver() =>
        ((HttpRepeaterViewModel)DataContext!).Collection;
}