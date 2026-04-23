using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pororoca.Desktop.Controls;
using Pororoca.Desktop.Others;
using Pororoca.Desktop.ViewModels;
using Pororoca.Domain.Features.VariableResolution;

namespace Pororoca.Desktop.Views;

public sealed class RequestAuthView : UserControl, IPororocaVariableResolverProvider
{
    private readonly PororocaVariableSyntaxHighlightingDefinitionSet syntaxHighlightingDefinitionSet;

    public RequestAuthView()
    {
        InitializeComponent();

        this.syntaxHighlightingDefinitionSet = new(ProvideVariableResolver);

        var tbBasicAuthLogin = this.FindControl<SyntaxHighlightingTextBox>("tbBasicAuthLogin")!;
        var tbBasicAuthPassword = this.FindControl<SyntaxHighlightingTextBox>("tbBasicAuthPassword")!;
        var tbBearerAuthToken = this.FindControl<SyntaxHighlightingTextBox>("tbBearerAuthToken")!;
        var tbWindowsAuthLogin = this.FindControl<SyntaxHighlightingTextBox>("tbWindowsAuthLogin")!;
        var tbWindowsAuthPassword = this.FindControl<SyntaxHighlightingTextBox>("tbWindowsAuthPassword")!;
        var tbWindowsAuthDomain = this.FindControl<SyntaxHighlightingTextBox>("tbWindowsAuthDomain")!;
        var tbClientCertificatePkcs12FilePath = this.FindControl<SyntaxHighlightingTextBox>("tbClientCertificatePkcs12FilePath")!;
        var tbClientCertificatePkcs12FilePassword = this.FindControl<SyntaxHighlightingTextBox>("tbClientCertificatePkcs12FilePassword")!;
        var tbClientCertificatePemCertificateFilePath = this.FindControl<SyntaxHighlightingTextBox>("tbClientCertificatePemCertificateFilePath")!;
        var tbClientCertificatePemPrivateKeyFilePath = this.FindControl<SyntaxHighlightingTextBox>("tbClientCertificatePemPrivateKeyFilePath")!;
        var tbClientCertificatePemPrivateKeyPassword = this.FindControl<SyntaxHighlightingTextBox>("tbClientCertificatePemPrivateKeyPassword")!;

        tbBasicAuthLogin.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbBasicAuthPassword.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbBearerAuthToken.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbWindowsAuthLogin.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbWindowsAuthPassword.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbWindowsAuthDomain.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbClientCertificatePkcs12FilePath.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbClientCertificatePkcs12FilePassword.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbClientCertificatePemCertificateFilePath.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbClientCertificatePemPrivateKeyFilePath.DefinitionSet = this.syntaxHighlightingDefinitionSet;
        tbClientCertificatePemPrivateKeyPassword.DefinitionSet = this.syntaxHighlightingDefinitionSet;
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    // IMPORTANTE: este método deve retornar um CollectionViewModel,
    // e não simplesmente uma coleção, pois senão não vai atualizar
    // as variáveis de coleção e de ambiente.
    public IPororocaVariableResolver ProvideVariableResolver() =>
        ((RequestAuthViewModel)DataContext!).col;
}