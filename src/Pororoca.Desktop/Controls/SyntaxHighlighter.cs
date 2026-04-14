using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Avalonia.Utilities;
using ReactiveUI;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// Syntax highlighter.
/// </summary>
public sealed class SyntaxHighlighter : AvaloniaObject, IDisposable
{
    /// <summary>
    /// Property of <see cref="Background"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, IBrush?> BackgroundProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, IBrush?>(nameof(Background), sh => sh.Background, (sh, b) => sh.Background = b);
    /// <summary>
    /// Property of <see cref="FlowDirection"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, FlowDirection> FlowDirectionProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, FlowDirection>(nameof(FlowDirection), sh => sh.FlowDirection, (sh, d) => sh.FlowDirection = d);
    /// <summary>
    /// Property of <see cref="FontFamily"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, FontFamily> FontFamilyProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, FontFamily>(nameof(FontFamily), sh => sh.FontFamily, (sh, f) => sh.FontFamily = f);
    /// <summary>
    /// Property of <see cref="FontStretch"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, FontStretch> FontStretchProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, FontStretch>(nameof(FontStretch), sh => sh.FontStretch, (sh, s) => sh.FontStretch = s);
    /// <summary>
    /// Property of <see cref="FontSize"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, double> FontSizeProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, double>(nameof(FontSize), sh => sh.FontSize, (sh, s) => sh.FontSize = s);
    /// <summary>
    /// Property of <see cref="FontStyle"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, FontStyle> FontStyleProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, FontStyle>(nameof(FontStyle), sh => sh.FontStyle, (sh, s) => sh.FontStyle = s);
    /// <summary>
    /// Property of <see cref="FontWeight"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, FontWeight> FontWeightProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, FontWeight>(nameof(FontWeight), sh => sh.FontWeight, (sh, w) => sh.FontWeight = w);
    /// <summary>
    /// Property of <see cref="Foreground"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, IBrush?> ForegroundProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, IBrush?>(nameof(Foreground), sh => sh.Foreground, (sh, b) => sh.Foreground = b);
    /// <summary>
    /// Property of <see cref="DefinitionSet"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, SyntaxHighlightingDefinitionSet?> DefinitionSetProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, SyntaxHighlightingDefinitionSet?>(nameof(DefinitionSet), sh => sh.DefinitionSet, (sh, ds) => sh.DefinitionSet = ds);
    /// <summary>
    /// Property of <see cref="IsMaxTokenCountReached"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, bool> IsMaxTokenCountReachedProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, bool>(nameof(IsMaxTokenCountReached), sh => sh.isMaxTokenCountReached);
    /// <summary>
    /// Property of <see cref="LetterSpacing"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, double> LetterSpacingProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, double>(nameof(LetterSpacing), sh => sh.LetterSpacing, (sh, s) => sh.LetterSpacing = s);
    /// <summary>
    /// Property of <see cref="FlowDirection"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, double> LineHeightProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, double>(nameof(LineHeight), sh => sh.LineHeight, (sh, h) => sh.LineHeight = h);
    /// <summary>
    /// Property of <see cref="MaxHeight"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, double> MaxHeightProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, double>(nameof(MaxHeight), sh => sh.MaxHeight, (sh, h) => sh.MaxHeight = h);
    /// <summary>
    /// Property of <see cref="MaxLines"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, int> MaxLinesProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, int>(nameof(MaxLines), sh => sh.MaxLines, (sh, l) => sh.MaxLines = l);
    /// <summary>
    /// Property of <see cref="MaxTokenCount"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, int> MaxTokenCountProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, int>(nameof(MaxTokenCount), sh => sh.MaxTokenCount, (sh, c) => sh.MaxTokenCount = c);
    /// <summary>
    /// Property of <see cref="MaxWidth"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, double> MaxWidthProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, double>(nameof(MaxWidth), sh => sh.MaxWidth, (sh, w) => sh.MaxWidth = w);
    /// <summary>
    /// Property of <see cref="PreeditText"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, string?> PreeditTextProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, string?>(nameof(PreeditText), sh => sh.PreeditText, (sh, t) => sh.PreeditText = t);
    /// <summary>
    /// Property of <see cref="SelectionBackground"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, IBrush?> SelectionBackgroundProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, IBrush?>(nameof(SelectionBackground), sh => sh.SelectionBackground, (sh, b) => sh.SelectionBackground = b);
    /// <summary>
    /// Property of <see cref="SelectionEnd"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, int> SelectionEndProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, int>(nameof(SelectionEnd), sh => sh.SelectionEnd, (sh, i) => sh.SelectionEnd = i);
    /// <summary>
    /// Property of <see cref="SelectionForeground"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, IBrush?> SelectionForegroundProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, IBrush?>(nameof(SelectionForeground), sh => sh.SelectionForeground, (sh, b) => sh.SelectionForeground = b);
    /// <summary>
    /// Property of <see cref="SelectionStart"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, int> SelectionStartProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, int>(nameof(SelectionStart), sh => sh.SelectionStart, (sh, i) => sh.SelectionStart = i);
    /// <summary>
    /// Property of <see cref="SyntaxErrorRange"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, Range> SyntaxErrorRangeProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, Range>(nameof(SyntaxErrorRange), sh => sh.SyntaxErrorRange, (sh, r) => sh.SyntaxErrorRange = r);
    /// <summary>
    /// Property of <see cref="Text"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, string?> TextProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, string?>(nameof(Text), sh => sh.Text, (sh, t) => sh.Text = t);
    /// <summary>
    /// Property of <see cref="TextAlignment"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, TextAlignment> TextAlignmentProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, TextAlignment>(nameof(TextAlignment), sh => sh.TextAlignment, (sh, a) => sh.TextAlignment = a);
    /// <summary>
    /// Property of <see cref="TextDecorations"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, TextDecorationCollection?> TextDecorationProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, TextDecorationCollection?>(nameof(TextDecorations), sh => sh.TextDecorations, (sh, d) => sh.TextDecorations = d);
    /// <summary>
    /// Property of <see cref="TextTrimming"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, TextTrimming> TextTrimmingProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, TextTrimming>(nameof(TextTrimming), sh => sh.TextTrimming, (sh, t) => sh.TextTrimming = t);
    /// <summary>
    /// Property of <see cref="TextWrapping"/>.
    /// </summary>
    public static readonly DirectProperty<SyntaxHighlighter, TextWrapping> TextWrappingProperty = AvaloniaProperty.RegisterDirect<SyntaxHighlighter, TextWrapping>(nameof(TextWrapping), sh => sh.TextWrapping, (sh, w) => sh.TextWrapping = w);


    // Span.
    private record struct Span(SyntaxHighlightingSpan Definition, int Start, int End, int InnerStart, int InnerEnd);


    // Token.
    private record struct Token(SyntaxHighlightingToken Definition, int Start, int End);


    // Static fields.
    private static readonly IList<SyntaxHighlightingToken> EmptyTokenDefinitions = Array.Empty<SyntaxHighlightingToken>();


    // Fields.
    private WeakEventHandlerAdapter<IBrush, AvaloniaPropertyChangedEventArgs>? backgroundPropertyChangedHandlerToken = null;
    private SortedObservableList<Span>? candidateSpans;
    private readonly Dictionary<SyntaxHighlightingSpan, SortedObservableList<Token>> candidateTokens = new();
    private SortedObservableList<Token>? defaultCandidateTokens;
    private Comparison<Token>? defaultTokenComparison;
    private WeakEventHandlerAdapter<IBrush, AvaloniaPropertyChangedEventArgs>? foregroundPropertyChangedHandlerToken;
    private readonly bool isDebugMode =
#if DEBUG
        true;
#else
        false;
#endif
    private bool isMaxTokenCountReached;
    private readonly Dictionary<SyntaxHighlightingSpan, TextRunProperties> runPropertiesMap = new();
    private readonly Dictionary<SyntaxHighlightingToken, TextRunProperties> runPropertiesMapInSpan = new();
    private WeakEventHandlerAdapter<IBrush, AvaloniaPropertyChangedEventArgs>? selectionBackgroundPropertyChangedHandlerToken;
    private WeakEventHandlerAdapter<IBrush, AvaloniaPropertyChangedEventArgs>? selectionForegroundPropertyChangedHandlerToken;
    private readonly Dictionary<SyntaxHighlightingSpan, TextRunProperties> selectionRunPropertiesMap = new();
    private readonly Dictionary<SyntaxHighlightingToken, TextRunProperties> selectionRunPropertiesMapInSpan = new();
    private TextDecorationCollection? syntaxErrorDecorationCollection;
    private TextLayout? textLayout;
    private IReadOnlyList<ValueSpan<TextRunProperties>>? textProperties;
    private readonly Dictionary<SyntaxHighlightingSpan, Comparison<Token>> tokenComparisons = new();

    /// <summary>
    /// Initialize new <see cref="SyntaxHighlighter"/> instance.
    /// </summary>
    public SyntaxHighlighter() { }

    /// <summary>
    /// Get or set base background brush.
    /// </summary>
    public IBrush? Background
    {
        get;
        set
        {
            VerifyAccess();
            if (ReferenceEquals(field, value))
                return;
            this.backgroundPropertyChangedHandlerToken?.Dispose();
            if (value != null)
            {
                this.backgroundPropertyChangedHandlerToken = new(value, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
            }
            SetAndRaise(BackgroundProperty, ref field, value);
            InvalidateTextProperties();
        }
    }

    /// <summary>
    /// Get or set syntax highlighting definition set.
    /// </summary>
    public SyntaxHighlightingDefinitionSet? DefinitionSet
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            if (field != null)
                field.Changed -= OnDefinitionSetChanged;
            if (value != null)
                value.Changed += OnDefinitionSetChanged;
            SetAndRaise(DefinitionSetProperty, ref field, value);
            this.candidateSpans = null;
            this.candidateTokens.Clear();
            this.defaultCandidateTokens = null;
            this.defaultTokenComparison = null;
            this.tokenComparisons.Clear();
            InvalidateTextLayout();
        }
    }

    /// <summary>
    /// Get or set flow direction.
    /// </summary>
    public FlowDirection FlowDirection
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(FlowDirectionProperty, ref field, value);
            InvalidateTextProperties();
        }
    } = FlowDirection.LeftToRight;

    /// <summary>
    /// Get or set base font family.
    /// </summary>
    public FontFamily FontFamily
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(FontFamilyProperty, ref field, value);
            InvalidateTextProperties();
        }
    } = FontManager.Current.DefaultFontFamily;

    /// <summary>
    /// Get or set base font size.
    /// </summary>
    public double FontSize
    {
        get;
        set
        {
            VerifyAccess();
            if (Math.Abs(field - value) <= 0.01)
                return;
            SetAndRaise(FontSizeProperty, ref field, value);
            InvalidateTextProperties();
        }
    } = 12;

    /// <summary>
    /// Get or set base font stretch.
    /// </summary>
    public FontStretch FontStretch
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(FontStretchProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = FontStretch.Normal;

    /// <summary>
    /// Get or set base font style.
    /// </summary>
    public FontStyle FontStyle
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(FontStyleProperty, ref field, value);
            InvalidateTextProperties();
        }
    } = FontStyle.Normal;

    /// <summary>
    /// Get or set base font weight.
    /// </summary>
    public FontWeight FontWeight
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(FontWeightProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = FontWeight.Normal;

    /// <summary>
    /// Get or set base foreground brush.
    /// </summary>
    public IBrush? Foreground
    {
        get;
        set
        {
            VerifyAccess();
            if (ReferenceEquals(field, value))
                return;
            this.foregroundPropertyChangedHandlerToken?.Dispose();
            if (value != null)
            {
                this.foregroundPropertyChangedHandlerToken = new(value, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
            }
            SetAndRaise(ForegroundProperty, ref field, value);
            InvalidateTextProperties();
        }
    }

    /**
     * Check whether maximum number of token to be highlighted reached or not.
     */
    public bool IsMaxTokenCountReached => this.isMaxTokenCountReached;

    /// <summary>
    /// Get or set letter spacing.
    /// </summary>
    public double LetterSpacing
    {
        get;
        set
        {
            VerifyAccess();
            if (!double.IsFinite(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            if (Math.Abs(field - value) <= 0.01)
                return;
            SetAndRaise(LetterSpacingProperty, ref field, value);
            InvalidateTextLayout();
        }
    }


    /// <summary>
    /// Get or set line height
    /// </summary>
    public double LineHeight
    {
        get;
        set
        {
            VerifyAccess();
            if (double.IsNaN(value))
            {
                if (double.IsNaN(field))
                    return;
            }
            else if (!double.IsFinite(value) || value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            else if (double.IsFinite(field) && Math.Abs(field - value) <= 0.01)
                return;
            SetAndRaise(LineHeightProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = double.NaN;


    /// <summary>
    /// Get or set maximum height of text layout.
    /// </summary>
    public double MaxHeight
    {
        get;
        set
        {
            VerifyAccess();
            if (double.IsInfinity(value))
            {
                if (value.Equals(field))
                    return;
            }
            else if (double.IsNaN(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            else if (double.IsFinite(field) && Math.Abs(field - value) <= 0.01)
                return;
            SetAndRaise(MaxHeightProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = double.PositiveInfinity;

    /// <summary>
    /// Get or set maximum number of lines.
    /// </summary>
    public int MaxLines
    {
        get;
        set
        {
            VerifyAccess();
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            else if (field == value)
                return;
            SetAndRaise(MaxLinesProperty, ref field, value);
            InvalidateTextLayout();
        }
    }

    /// <summary>
    /// Get or set maximum number of token should be highlighted. Negative value if there is no limitation.
    /// </summary>
    public int MaxTokenCount
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(MaxTokenCountProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = -1;

    /// <summary>
    /// Get or set maximum width of text layout.
    /// </summary>
    public double MaxWidth
    {
        get;
        set
        {
            VerifyAccess();
            if (double.IsInfinity(value))
            {
                if (value.Equals(field))
                    return;
            }
            else if (double.IsNaN(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            else if (double.IsFinite(field) && Math.Abs(field - value) <= 0.01)
                return;
            SetAndRaise(MaxWidthProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = double.PositiveInfinity;

    /// <summary>
    /// Get or set preedit text.
    /// </summary>
    public string? PreeditText
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(PreeditTextProperty, ref field, value);
            InvalidateTextProperties();
        }
    }

    /// <summary>
    /// Get or set background brush for selected text.
    /// </summary>
    public IBrush? SelectionBackground
    {
        get;
        set
        {
            VerifyAccess();
            if (ReferenceEquals(field, value))
                return;
            this.selectionBackgroundPropertyChangedHandlerToken?.Dispose();
            if (value != null)
            {
                this.selectionBackgroundPropertyChangedHandlerToken = new(value, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
            }
            SetAndRaise(SelectionBackgroundProperty, ref field, value);
            if (SelectionStart != SelectionEnd)
                InvalidateTextProperties();
        }
    }

    /// <summary>
    /// Get or set end (exclusive) index of selected text.
    /// </summary>
    public int SelectionEnd
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(SelectionEndProperty, ref field, value);
            if (SelectionForeground != null)
                InvalidateTextProperties();
        }
    }

    /// <summary>
    /// Get or set foreground brush for selected text.
    /// </summary>
    public IBrush? SelectionForeground
    {
        get;
        set
        {
            VerifyAccess();
            if (ReferenceEquals(field, value))
                return;
            this.selectionForegroundPropertyChangedHandlerToken?.Dispose();
            if (value != null)
            {
                this.selectionForegroundPropertyChangedHandlerToken = new(value, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
            }
            SetAndRaise(SelectionForegroundProperty, ref field, value);
            if (SelectionStart != SelectionEnd)
                InvalidateTextProperties();
        }
    }

    /// <summary>
    /// Get or set start (inclusive) index of selected text.
    /// </summary>
    public int SelectionStart
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(SelectionStartProperty, ref field, value);
            if (SelectionForeground != null)
                InvalidateTextProperties();
        }
    }

    // Get text decoration for syntax error.
    TextDecoration? SyntaxErrorDecoration
    {
        get
        {
            field ??= new TextDecoration()
            {
                Stroke = new SolidColorBrush()
                {
                    Color = Colors.Red
                    //if (Application.Current is Avalonia.Application app)
                    //    brush.Bind(SolidColorBrush.ColorProperty, app, "Color/SyntaxHighlighter.SyntaxError.Underline");
                    //else
                },
                StrokeDashArray = [1, 1],
                StrokeOffset = 3,
                StrokeOffsetUnit = TextDecorationUnit.Pixel,
                StrokeThickness = 2,
                StrokeThicknessUnit = TextDecorationUnit.Pixel
            };
            return field;
        }

        set;
    }

    /// <summary>
    /// Get or set character range of syntax error.
    /// </summary>
    public Range SyntaxErrorRange
    {
        get;
        set
        {
            VerifyAccess();
            SetAndRaise(SyntaxErrorRangeProperty, ref field, value);
            InvalidateTextProperties();
        }
    } = default;

    /// <summary>
    /// Get or set text.
    /// </summary>
    public string? Text
    {
        get;
        set
        {
            VerifyAccess();
            if ((field?.Length ?? 0) < 1024
                && (value?.Length ?? 0) < 1024
                && field == value)
            {
                return;
            }
            SetAndRaise(TextProperty, ref field, value);
            InvalidateTextProperties();
        }
    }

    /// <summary>
    /// Get or set text alignment.
    /// </summary>
    public TextAlignment TextAlignment
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(TextAlignmentProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = TextAlignment.Left;

    /// <summary>
    /// Get or set base text decorations.
    /// </summary>
    public TextDecorationCollection? TextDecorations
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(TextDecorationProperty, ref field, value);
            InvalidateTextProperties();
        }
    }

    /// <summary>
    /// Raised when text layout of the instance was invalidated.
    /// </summary>
    public event EventHandler? TextLayoutInvalidated;

    // Get text with pre-edit text.
    private string TextWithPreeditText
    {
        get
        {
            string? text = Text;
            if (!string.IsNullOrEmpty(PreeditText))
            {
                if (text == null)
                    text = PreeditText;
                else
                {
                    int caretIndex = Math.Min(SelectionStart, SelectionEnd);
                    if (caretIndex < 0)
                        text = PreeditText + text;
                    else if (caretIndex >= text.Length)
                        text += PreeditText;
                    else
                        text = text[..caretIndex] + PreeditText + text[caretIndex..];
                }
            }
            return text ?? "";
        }
    }

    /// <summary>
    /// Get or set text trimming.
    /// </summary>
    public TextTrimming TextTrimming
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(TextTrimmingProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = TextTrimming.CharacterEllipsis;

    /// <summary>
    /// Get or set text wrapping.
    /// </summary>
    public TextWrapping TextWrapping
    {
        get;
        set
        {
            VerifyAccess();
            if (field == value)
                return;
            SetAndRaise(TextWrappingProperty, ref field, value);
            InvalidateTextLayout();
        }
    } = TextWrapping.NoWrap;

    private IReadOnlyList<ValueSpan<TextRunProperties>> CreateTextProperties(ref int tokenCount, TextRunProperties defaultRunProperties)
    {
        // check text
        string? text = Text;
        string? preeditText = PreeditText;
        if (string.IsNullOrEmpty(text))
        {
            if (string.IsNullOrEmpty(preeditText))
                return Array.Empty<ValueSpan<TextRunProperties>>();
            text = "";
        }

        // setup default run properties for selected text
        var defaultSelectionRunProperties = new GenericTextRunProperties(
            defaultRunProperties.Typeface,
            defaultRunProperties.FontRenderingEmSize,
            defaultRunProperties.TextDecorations,
            SelectionForeground ?? defaultRunProperties.ForegroundBrush,
            SelectionBackground ?? defaultRunProperties.BackgroundBrush
        );

        // setup initial candidate spans
        this.candidateSpans ??= new SortedObservableList<Span>((lhs, rhs) =>
        {
            int result = (rhs.Start - lhs.Start);
            if (result != 0)
                return result;
            result = (lhs.End - rhs.End);
            if (result != 0)
                return result;
            if (DefinitionSet != null)
            {
                var sd = DefinitionSet.SpanDefinitions;
                result = sd.IndexOf(rhs.Definition) - sd.IndexOf(lhs.Definition);
            }
            else
            {
                result = 0;
            }
            return result != 0 ? result : (rhs.GetHashCode() - lhs.GetHashCode());
        });
        if (DefinitionSet is not null)
        {
            foreach (var spanDefinition in DefinitionSet.SpanDefinitions)
            {
                if (!spanDefinition.IsValid)
                    continue;
                var startMatch = spanDefinition.StartPattern!.Match(text);
                if (!startMatch.Success || startMatch.Length == 0)
                    continue;
                var endMatch = spanDefinition.EndPattern!.Match(text, startMatch.Index + startMatch.Length);
                if (endMatch.Success && endMatch.Length > 0)
                {
                    this.candidateSpans.Add(new(
                        spanDefinition,
                        startMatch.Index,
                        endMatch.Index + endMatch.Length,
                        startMatch.Index + startMatch.Length,
                        endMatch.Index));
                }
            }
        }
        ;

        // create text properties for each span
        int maxTokenCount = MaxTokenCount;
        var textProperties = new List<ValueSpan<TextRunProperties>>();
        int textStartIndex = 0;
        var runPropertiesMap = this.runPropertiesMap;
        var selectionRunPropertiesMap = this.selectionRunPropertiesMap;
        var defaultTokenDefinitions = DefinitionSet?.TokenDefinitions ?? Array.Empty<SyntaxHighlightingToken>();
        try
        {
            while (this.candidateSpans.IsNotEmpty())
            {
                // get current span
                var span = this.candidateSpans[^1];
                this.candidateSpans.RemoveAt(this.candidateSpans.Count - 1);

                // find next span
                var startMatch = span.Definition.StartPattern!.Match(text, span.End);
                Match? endMatch;
                if (startMatch.Success)
                {
                    endMatch = span.Definition.EndPattern!.Match(text, startMatch.Index + startMatch.Length);
                    if (endMatch.Success)
                    {
                        this.candidateSpans.Add(new(
                            span.Definition,
                            startMatch.Index,
                            endMatch.Index + endMatch.Length,
                            startMatch.Index + startMatch.Length,
                            endMatch.Index));
                    }
                }

                // remove spans which overlaps with current span
                for (int i = this.candidateSpans.Count - 1; i >= 0; --i)
                {
                    // check overlapping
                    var removingSpan = this.candidateSpans[i];
                    if (removingSpan.Start >= span.End)
                        continue;
                    this.candidateSpans.RemoveAt(i);

                    // find next span
                    startMatch = removingSpan.Definition.StartPattern!.Match(text, span.End);
                    if (!startMatch.Success)
                        continue;
                    endMatch = removingSpan.Definition.EndPattern!.Match(text, startMatch.Index + startMatch.Length);
                    if (endMatch.Success)
                    {
                        int j = this.candidateSpans.Add(new(
                            removingSpan.Definition,
                            startMatch.Index,
                            endMatch.Index + endMatch.Length,
                            startMatch.Index + startMatch.Length,
                            endMatch.Index));
                        if (j < i)
                            ++i;
                    }
                }

                // create text properties
                if (!runPropertiesMap.TryGetValue(span.Definition, out var runProperties))
                {
                    var typeface = new Typeface(
                        span.Definition.FontFamily ?? defaultRunProperties.Typeface.FontFamily,
                        span.Definition.FontStyle ?? defaultRunProperties.Typeface.Style,
                        span.Definition.FontWeight ?? defaultRunProperties.Typeface.Weight,
                        FontStretch
                    );
                    runProperties = new GenericTextRunProperties(
                        typeface,
                        double.IsNaN(span.Definition.FontSize) ? defaultRunProperties.FontRenderingEmSize : span.Definition.FontSize,
                        span.Definition.TextDecorations ?? defaultRunProperties.TextDecorations,
                        span.Definition.Foreground ?? defaultRunProperties.ForegroundBrush,
                        span.Definition.Background ?? defaultRunProperties.BackgroundBrush
                    );
                    runPropertiesMap[span.Definition] = runProperties;
                }
                if (!selectionRunPropertiesMap.TryGetValue(span.Definition, out var selectionRunProperties))
                {
                    selectionRunProperties = new GenericTextRunProperties(
                        runProperties.Typeface,
                        runProperties.FontRenderingEmSize,
                        runProperties.TextDecorations,
                        SelectionForeground ?? runProperties.ForegroundBrush,
                        SelectionBackground ?? runProperties.BackgroundBrush
                    );
                    selectionRunPropertiesMap[span.Definition] = selectionRunProperties;
                }
                if (textStartIndex < span.Start)
                    CreateTextPropertiesInSpan(text, textStartIndex, span.Start, ref tokenCount, null, defaultTokenDefinitions, defaultRunProperties, defaultSelectionRunProperties, textProperties);
                CreateTextPropertiesInSpan(text, span.Start, span.InnerStart, ref tokenCount, null, EmptyTokenDefinitions, runProperties, selectionRunProperties, textProperties);
                CreateTextPropertiesInSpan(text, span.InnerStart, span.InnerEnd, ref tokenCount, span.Definition, span.Definition.TokenDefinitions, runProperties, selectionRunProperties, textProperties);
                CreateTextPropertiesInSpan(text, span.InnerEnd, span.End, ref tokenCount, null, EmptyTokenDefinitions, runProperties, selectionRunProperties, textProperties);
                textStartIndex = span.End;
                if (maxTokenCount >= 0 && tokenCount >= maxTokenCount)
                    break;
            }
            if (textStartIndex < text.Length)
                CreateTextPropertiesInSpan(text, textStartIndex, text.Length, ref tokenCount, null, defaultTokenDefinitions, defaultRunProperties, defaultSelectionRunProperties, textProperties);
        }
        finally
        {
            this.candidateSpans.Clear();
            runPropertiesMap.Clear();
            selectionRunPropertiesMap.Clear();
        }

        // insert text style for preedit text
        if (!string.IsNullOrEmpty(preeditText))
        {
            int preeditTextLength = preeditText.Length;
            int caretIndex = Math.Min(SelectionStart, SelectionEnd);
            var runProperties = new GenericTextRunProperties(
                defaultRunProperties.Typeface,
                defaultRunProperties.FontRenderingEmSize,
                Avalonia.Media.TextDecorations.Underline,
                defaultRunProperties.ForegroundBrush
            );
            if (caretIndex <= 0)
            {
                textProperties.Add(new(0, preeditTextLength, runProperties));
                for (int i = textProperties.Count - 1; i > 0; --i)
                {
                    var properties = textProperties[i];
                    textProperties[i] = new(properties.Start + preeditTextLength, properties.Length, properties.Value);
                }
            }
            else if (caretIndex >= text.Length)
                textProperties.Add(new(text.Length, preeditTextLength, runProperties));
            else
            {
                int indexOfTextPropertiesToInsert = textProperties.Count - 1;
                var textPropertiesToInsert = textProperties[indexOfTextPropertiesToInsert];
                if (textPropertiesToInsert.Start > caretIndex)
                {
                    for (int i = textProperties.Count - 2; i >= 0; --i)
                    {
                        var properties = textProperties[i];
                        if (properties.Start <= caretIndex)
                        {
                            indexOfTextPropertiesToInsert = i;
                            textPropertiesToInsert = properties;
                            break;
                        }
                    }
                }
                if (textPropertiesToInsert.Start == caretIndex)
                    textProperties.Insert(indexOfTextPropertiesToInsert, new(caretIndex, preeditTextLength, runProperties));
                else
                {
                    textProperties[indexOfTextPropertiesToInsert++] = new(textPropertiesToInsert.Start, caretIndex - textPropertiesToInsert.Start, textPropertiesToInsert.Value);
                    textProperties.Insert(indexOfTextPropertiesToInsert++, new(caretIndex, preeditTextLength, runProperties));
                    textProperties.Insert(indexOfTextPropertiesToInsert, new(caretIndex + preeditTextLength, textPropertiesToInsert.Length - (caretIndex - textPropertiesToInsert.Start), textPropertiesToInsert.Value));
                }
                for (int i = textProperties.Count - 1; i > indexOfTextPropertiesToInsert; --i)
                {
                    var properties = textProperties[i];
                    textProperties[i] = new(properties.Start + preeditTextLength, properties.Length, properties.Value);
                }
            }
        }

        // complete
        return textProperties;
    }

    private void CreateTextProperties(int start, int end, TextRunProperties runProperties, TextRunProperties selectionRunProperties, IList<ValueSpan<TextRunProperties>> textProperties)
    {
        var syntaxErrorRange = SyntaxErrorRange;
        if (end <= syntaxErrorRange.Start.Value || start >= syntaxErrorRange.End.Value)
            textProperties.Add(new(start, end - start, runProperties));
        else
        {
            int syntaxErrorStart = syntaxErrorRange.Start.Value;
            int syntaxErrorEnd = syntaxErrorRange.End.Value;
            var errorRunProperties = new GenericTextRunProperties(
                typeface: runProperties.Typeface,
                //fontFeatures: runProperties.FontFeatures,
                fontRenderingEmSize: runProperties.FontRenderingEmSize,
                textDecorations: runProperties.TextDecorations?.Any() == true
                    ? [.. runProperties.TextDecorations, SyntaxErrorDecoration!]
                    : (this.syntaxErrorDecorationCollection ??= [SyntaxErrorDecoration!]),
                foregroundBrush: runProperties.ForegroundBrush,
                backgroundBrush: runProperties.BackgroundBrush,
                baselineAlignment: runProperties.BaselineAlignment,
                cultureInfo: runProperties.CultureInfo
            );
            if (start <= syntaxErrorStart)
            {
                if (start < syntaxErrorStart)
                    textProperties.Add(new(start, syntaxErrorStart - start, runProperties));
                if (end <= syntaxErrorEnd)
                    textProperties.Add(new(syntaxErrorStart, end - syntaxErrorStart, errorRunProperties));
                else
                {
                    textProperties.Add(new(syntaxErrorStart, syntaxErrorEnd - syntaxErrorStart, errorRunProperties));
                    textProperties.Add(new(syntaxErrorEnd, end - syntaxErrorEnd, runProperties));
                }
            }
            else
            {
                if (end <= syntaxErrorEnd)
                    textProperties.Add(new(start, end - start, errorRunProperties));
                else
                {
                    textProperties.Add(new(start, syntaxErrorEnd - start, errorRunProperties));
                    textProperties.Add(new(syntaxErrorEnd, end - syntaxErrorEnd, runProperties));
                }
            }
        }
    }

    private void CreateTextPropertiesInSpan(string text, int start, int end, ref int tokenCount, SyntaxHighlightingSpan? spanDefinition, IList<SyntaxHighlightingToken> tokenDefinitions, TextRunProperties defaultRunProperties, TextRunProperties defaultSelectionRunProperties, IList<ValueSpan<TextRunProperties>> textProperties)
    {
        // check state
        int maxTokenCount = MaxTokenCount;
        if (maxTokenCount >= 0 && tokenCount >= maxTokenCount)
        {
            CreateTextProperties(start, end, defaultRunProperties, defaultSelectionRunProperties, textProperties);
            return;
        }

        // setup initial candidate tokens
        var tokenComparison = spanDefinition is not null
            ? this.tokenComparisons.GetValueOrDefault(spanDefinition)
            : this.defaultTokenComparison;
        if (tokenComparison is null)
        {
            tokenComparison = (lhs, rhs) =>
            {
                int result = (rhs.Start - lhs.Start);
                if (result != 0)
                    return result;
                result = (lhs.End - rhs.End);
                if (result != 0)
                    return result;
                result = tokenDefinitions.IndexOf(rhs.Definition) - tokenDefinitions.IndexOf(lhs.Definition);
                return result != 0 ? result : (rhs.GetHashCode() - lhs.GetHashCode());
            };
            if (spanDefinition is not null)
                this.tokenComparisons[spanDefinition] = tokenComparison;
            else
                this.defaultTokenComparison = tokenComparison;
        }
        var candidateTokens = spanDefinition is not null
            ? this.candidateTokens.GetValueOrDefault(spanDefinition)
            : this.defaultCandidateTokens;
        if (candidateTokens is null)
        {
            candidateTokens = new(tokenComparison);
            if (spanDefinition is not null)
                this.candidateTokens[spanDefinition] = candidateTokens;
            else
                this.defaultCandidateTokens = candidateTokens;
        }
        foreach (var tokenDefinition in tokenDefinitions)
        {
            if (!tokenDefinition.IsValid)
                continue;
            var match = tokenDefinition.Pattern!.Match(text, start);
            if (match.Success && match.Length > 0)
            {
                int endIndex = match.Index + match.Length;
                if (endIndex <= end)
                    candidateTokens.Add(new(tokenDefinition, match.Index, endIndex));
            }
        }

        // create text runs
        int textStartIndex = start;
        var runPropertiesMap = this.runPropertiesMapInSpan;
        var selectionRunPropertiesMap = this.selectionRunPropertiesMapInSpan;
        try
        {
            while (candidateTokens.IsNotEmpty())
            {
                // get current token
                var token = candidateTokens[^1];
                candidateTokens.RemoveAt(candidateTokens.Count - 1);

                // find and combine with next token if possible
                while (true)
                {
                    var match = token.Definition.Pattern!.Match(text, token.End);
                    if (!match.Success || match.Length <= 0)
                        break;
                    int endIndex = match.Index + match.Length;
                    if (endIndex > end)
                        break;
                    var nextToken = new Token(token.Definition, match.Index, endIndex);
                    if (match.Index == token.End && (maxTokenCount < 0 || tokenCount < maxTokenCount - 1)) // combine into single token
                    {
                        int nextTokenIndex = candidateTokens.BinarySearch(nextToken, tokenComparison);
                        if (nextTokenIndex == ~candidateTokens.Count)
                        {
                            token = new(token.Definition, token.Start, nextToken.End);
                            ++tokenCount;
                            continue;
                        }
                    }
                    candidateTokens.Add(nextToken);
                    break;
                }

                // remove tokens which overlaps with current token
                for (int i = candidateTokens.Count - 1; i >= 0; --i)
                {
                    // check overlapping
                    var removingToken = candidateTokens[i];
                    if (removingToken.Start >= token.End)
                        continue;
                    candidateTokens.RemoveAt(i);

                    // find next token
                    var match = removingToken.Definition.Pattern!.Match(text, token.End);
                    if (match.Success && match.Length > 0)
                    {
                        int endIndex = match.Index + match.Length;
                        if (endIndex <= end)
                        {
                            int j = candidateTokens.Add(new(removingToken.Definition, match.Index, endIndex));
                            if (j < i)
                                ++i;
                        }
                    }
                }

                // create text style
                if (!runPropertiesMap.TryGetValue(token.Definition, out var runProperties))
                {
                    var typeface = new Typeface(
                        token.Definition.FontFamily ?? defaultRunProperties.Typeface.FontFamily,
                        token.Definition.FontStyle ?? defaultRunProperties.Typeface.Style,
                        token.Definition.FontWeight ?? defaultRunProperties.Typeface.Weight,
                        FontStretch
                    );
                    runProperties = new GenericTextRunProperties(
                        typeface,
                        double.IsNaN(token.Definition.FontSize) ? defaultRunProperties.FontRenderingEmSize : token.Definition.FontSize,
                        token.Definition.TextDecorations ?? defaultRunProperties.TextDecorations,
                        token.Definition.Foreground ?? defaultRunProperties.ForegroundBrush,
                        token.Definition.Background ?? defaultRunProperties.BackgroundBrush
                    );
                    runPropertiesMap[token.Definition] = runProperties;
                }
                if (!selectionRunPropertiesMap.TryGetValue(token.Definition, out var selectionRunProperties))
                {
                    selectionRunProperties = new GenericTextRunProperties(
                        runProperties.Typeface,
                        runProperties.FontRenderingEmSize,
                        runProperties.TextDecorations,
                        SelectionForeground ?? defaultRunProperties.ForegroundBrush,
                        SelectionBackground ?? defaultRunProperties.BackgroundBrush
                    );
                    selectionRunPropertiesMap[token.Definition] = selectionRunProperties;
                }
                if (textStartIndex < token.Start)
                    CreateTextProperties(textStartIndex, token.Start, defaultRunProperties, defaultSelectionRunProperties, textProperties);
                if (maxTokenCount < 0 || tokenCount < maxTokenCount)
                {
                    CreateTextProperties(token.Start, token.End, runProperties, selectionRunProperties, textProperties);
                    ++tokenCount;
                    textStartIndex = token.End;
                    if (maxTokenCount >= 0 && tokenCount >= maxTokenCount)
                        break;
                }
                else
                    break;
            }
            if (textStartIndex < end)
                CreateTextProperties(textStartIndex, end, defaultRunProperties, defaultSelectionRunProperties, textProperties);
        }
        finally
        {
            candidateTokens.Clear();
            runPropertiesMap.Clear();
            selectionRunPropertiesMap.Clear();
        }
    }

    /// <summary>
    /// Create text layout.
    /// </summary>
    /// <returns>Text layout.</returns>
    public TextLayout CreateTextLayout()
    {
        // use created text layout
        if (this.textLayout != null)
            return this.textLayout;

        // get text
        string text = TextWithPreeditText;

        // create type face
        var typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

        // prepare base run properties
        var defaultRunProperties = new GenericTextRunProperties(
            typeface,
            FontSize,
            TextDecorations,
            Foreground,
            Background
        );

        // create text runs and source
        if (this.textProperties is null)
        {
            int tokenCount = 0;
            this.textProperties = CreateTextProperties(ref tokenCount, defaultRunProperties);
            SetAndRaise(IsMaxTokenCountReachedProperty, ref this.isMaxTokenCountReached, MaxTokenCount >= 0 && tokenCount >= MaxTokenCount);
        }

        // create text layout
        this.textLayout = new TextLayout(
            text,
            typeface,
            FontSize,
            Foreground,
            TextAlignment,
            TextWrapping,
            TextTrimming,
            maxLines: MaxLines,
            maxWidth: MaxWidth,
            maxHeight: MaxHeight,
            textStyleOverrides: this.textProperties,
            flowDirection: FlowDirection,
            lineHeight: LineHeight,
            letterSpacing: LetterSpacing
        );
        if (this.isDebugMode)
        {
            var textLines = this.textLayout.TextLines;
            for (int lineIndex = textLines.Count - 1; lineIndex >= 0; --lineIndex)
            {
                var textRuns = textLines[lineIndex].TextRuns;
                for (int runIndex = textRuns.Count - 1; runIndex >= 0; --runIndex)
                {
                    var textRun = textRuns[runIndex];
                    if (textRun is ShapedTextRun shapedTextRun && textRun.Length > 0 && shapedTextRun.ShapedBuffer.Length == 0)
                        throw new InvalidOperationException($"Text run with empty shaped buffer created at line {lineIndex} run {runIndex}, text: '{new string(textRun.Text.ToArray())}'.");
                }
            }
        }
        return this.textLayout;
    }

    // Called when property of attached brush has been changed.
    private void OnBrushPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e) =>
        InvalidateTextProperties();


    // Called when definition set changed.
    private void OnDefinitionSetChanged(object? sender, EventArgs e)
    {
        this.candidateSpans = null;
        this.candidateTokens.Clear();
        this.defaultCandidateTokens = null;
        this.defaultTokenComparison = null;
        this.tokenComparisons.Clear();
        InvalidateTextProperties();
    }

    // Invalidate text layout.
    private void InvalidateTextLayout()
    {
        this.textLayout = null;
        TextLayoutInvalidated?.Invoke(this, EventArgs.Empty);
    }

    // Invalidate text properties.
    private void InvalidateTextProperties()
    {
        this.textProperties = null;
        InvalidateTextLayout();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.backgroundPropertyChangedHandlerToken?.Dispose();
        this.foregroundPropertyChangedHandlerToken?.Dispose();
        this.selectionBackgroundPropertyChangedHandlerToken?.Dispose();
        this.selectionForegroundPropertyChangedHandlerToken?.Dispose();
        SyntaxErrorDecoration = null;
    }
}