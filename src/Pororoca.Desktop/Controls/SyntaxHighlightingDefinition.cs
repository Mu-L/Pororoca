using System.ComponentModel;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Pororoca.Desktop.Others;
using Pororoca.Domain.Features.Common;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// Base class of definition of syntax highlighting.
/// </summary>
public sealed class SyntaxHighlightingDefinition : INotifyPropertyChanged, IDisposable, IRegexDefiner
{
    // Fields.
    private WeakEventHandlerAdapter<AvaloniaObject, AvaloniaPropertyChangedEventArgs>? backgroundPropertyChangedHandlerToken;
    private WeakEventHandlerAdapter<AvaloniaObject, AvaloniaPropertyChangedEventArgs>? foregroundPropertyChangedHandlerToken;

    /// <summary>
    /// Initialize new <see cref="SyntaxHighlightingDefinition"/> instance.
    /// </summary>
    /// <param name="name">Name.</param>
    public SyntaxHighlightingDefinition(int id, string name, Regex pattern)
    {
        Id = id;
        Name = name;
        Pattern = pattern;
    }

    /// <summary>
    /// Raised when property of rule has been changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Get Id of definition.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Get name of definition.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Get or set pattern of the token.
    /// </summary>
    public Regex Pattern
    {
        get;
        set
        {
            if (ArePatternsEqual(field, value))
                return;
            field = value;
            Validate();
            OnPropertyChanged(nameof(Pattern));
        }
    }

    /// <summary>
    /// Check whether the definition is valid or not.
    /// </summary>
    public bool IsValid { get; private set; }

    /// <summary>
    /// Get or set background brush of the definition.
    /// </summary>
    public IBrush? Background
    {
        get;
        set
        {
            if (ReferenceEquals(field, value))
                return;
            this.backgroundPropertyChangedHandlerToken?.Dispose();
            if (value is AvaloniaObject aobj)
            {
                this.backgroundPropertyChangedHandlerToken = new(aobj, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
            }
            field = value;
            Validate();
            OnPropertyChanged(nameof(Background));
        }
    }

    /// <summary>
    /// Get or set font family of the definition.
    /// </summary>
    public FontFamily? FontFamily
    {
        get;
        set
        {
            if (field?.Equals(value) ?? value is null)
                return;
            field = value;
            Validate();
            OnPropertyChanged(nameof(FontFamily));
        }
    }

    /// <summary>
    /// Get or set font size of the definition.
    /// </summary>
    public double FontSize
    {
        get;
        set
        {
            if (double.IsInfinity(value) || value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            if (AreFontSizesEqual(field, value))
                return;
            field = value;
            Validate();
            OnPropertyChanged(nameof(FontSize));
        }
    } = double.NaN;

    /// <summary>
    /// Get or set font style of the definition.
    /// </summary>
    public FontStyle? FontStyle
    {
        get;
        set
        {
            if (field == value)
                return;
            field = value;
            Validate();
            OnPropertyChanged(nameof(FontStyle));
        }
    }

    /// <summary>
    /// Get or set font weight of the definition.
    /// </summary>
    public FontWeight? FontWeight
    {
        get;
        set
        {
            if (field == value)
                return;
            field = value;
            Validate();
            OnPropertyChanged(nameof(FontWeight));
        }
    }

    /// <summary>
    /// Get or set foreground brush of the definition.
    /// </summary>
    public IBrush? Foreground
    {
        get;
        set
        {
            if (ReferenceEquals(field, value))
                return;
            this.foregroundPropertyChangedHandlerToken?.Dispose();
            if (value is AvaloniaObject aobj)
            {
                this.foregroundPropertyChangedHandlerToken = new(aobj, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
            }
            field = value;
            Validate();
            OnPropertyChanged(nameof(Foreground));
        }
    }

    public Func<Match, int>? RegexMatchIdMapper { get; set; }

    public Func<int, IBrush>? RegexMatchIdForegroundMapper { get; set; }

    /// <summary>
    /// Get or set text decorations of the definition.
    /// </summary>
    public TextDecorationCollection? TextDecorations
    {
        get; set
        {
            if (field == value)
                return;
            field = value;
            Validate();
            OnPropertyChanged(nameof(TextDecorations));
        }
    }

    // Check whether two font sizes are equalivent or not.
    private static bool AreFontSizesEqual(double x, double y)
    {
        if (double.IsNaN(x))
            return double.IsNaN(y);
        if (double.IsNaN(y))
            return false;
        return Math.Abs(x - y) <= 0.01;
    }

    // Check equality of two patterns.
    private static bool ArePatternsEqual(Regex? x, Regex? y)
    {
        if (x is null)
            return y is null;
        if (y is null)
            return false;
        return x.ToString() == y.ToString() && x.Options == y.Options;
    }

    // Called when property of attached brush has been changed.
    private void OnBrushPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (ReferenceEquals(sender, Background))
            OnPropertyChanged(nameof(Background));
        else if (ReferenceEquals(sender, Foreground))
            OnPropertyChanged(nameof(Foreground));
    }

    /// <summary>
    /// Raise <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">Name of changed property.</param>
    public void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new(propertyName));

    /// <summary>
    /// Called to validate whether the definition is valid or not.
    /// </summary>
    /// <returns>True if the definition is valid.</returns>
    public bool OnValidate() =>
        (Background is not null
        || FontFamily is not null
        || double.IsFinite(FontSize)
        || FontStyle.HasValue
        || FontWeight.HasValue
        || Foreground is not null
        || (RegexMatchIdMapper is not null && RegexMatchIdForegroundMapper is not null)
        || TextDecorations is not null)
        && Pattern is not null;

    /// <summary>
    /// Validate whether the definition is valid or not.
    /// </summary>
    public void Validate()
    {
        if (IsValid != OnValidate())
        {
            IsValid = !IsValid;
            OnPropertyChanged(nameof(IsValid));
        }
    }

    internal GenericTextRunProperties MakeTextProperties(TextRunProperties defaultRunProperties, FontStretch fontStretch, int regexMatchId)
    {
        var typeface = new Typeface(
            FontFamily ?? defaultRunProperties.Typeface.FontFamily,
            FontStyle ?? defaultRunProperties.Typeface.Style,
            FontWeight ?? defaultRunProperties.Typeface.Weight,
            fontStretch
        );

        var foreground = regexMatchId != -1 && RegexMatchIdForegroundMapper != null ?
                         RegexMatchIdForegroundMapper(regexMatchId) :
                         Foreground ?? defaultRunProperties.ForegroundBrush;

        return new GenericTextRunProperties(
            typeface,
            double.IsNaN(FontSize) ? defaultRunProperties.FontRenderingEmSize : FontSize,
            TextDecorations ?? defaultRunProperties.TextDecorations,
            foreground,
            Background ?? defaultRunProperties.BackgroundBrush
        );
    }

    /// <inheritdoc/>
    public override string ToString() =>
        string.IsNullOrEmpty(Name) ?
            $"{{{Pattern}}}" :
            $"[{Name}]{{{Pattern}}}";

    /// <inheritdoc/>
    public void Dispose()
    {
        this.backgroundPropertyChangedHandlerToken?.Dispose();
        this.foregroundPropertyChangedHandlerToken?.Dispose();
    }
}