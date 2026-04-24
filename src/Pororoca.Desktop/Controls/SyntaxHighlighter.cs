using Avalonia;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;
using Avalonia.Threading;
using Avalonia.Utilities;
using Pororoca.Desktop.Others;
using Pororoca.Domain.Features.Common;

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

    private static readonly List<Action> invalidateSyntaxHighlightersTextBoxesTextsCallbacks = new();

    // Fields.
    private WeakEventHandlerAdapter<AvaloniaObject, AvaloniaPropertyChangedEventArgs>? backgroundPropertyChangedHandlerToken = null;
    private WeakEventHandlerAdapter<AvaloniaObject, AvaloniaPropertyChangedEventArgs>? foregroundPropertyChangedHandlerToken;
    private WeakEventHandlerAdapter<AvaloniaObject, AvaloniaPropertyChangedEventArgs>? selectionBackgroundPropertyChangedHandlerToken;
    private WeakEventHandlerAdapter<AvaloniaObject, AvaloniaPropertyChangedEventArgs>? selectionForegroundPropertyChangedHandlerToken;
    private readonly Dictionary<int, TextRunProperties> cachedTextRunProps = new();
    private readonly List<ValueSpan<TextRunProperties>> textProperties = new();
    private TextLayout? textLayout;

    /// <summary>
    /// Initialize new <see cref="SyntaxHighlighter"/> instance.
    /// </summary>
    public SyntaxHighlighter()
    {
        invalidateSyntaxHighlightersTextBoxesTextsCallbacks.Add(this.InvalidateTextProperties);
    }

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
            if (value is AvaloniaObject aobj)
            {
                this.backgroundPropertyChangedHandlerToken = new(aobj, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
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
            SetAndRaise(DefinitionSetProperty, ref field, value);
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
            if (value is AvaloniaObject aobj)
            {
                this.foregroundPropertyChangedHandlerToken = new(aobj, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
            }
            SetAndRaise(ForegroundProperty, ref field, value);
            InvalidateTextProperties();
        }
    }

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
            if (double.IsNaN(value) && double.IsNaN(field))
                return;
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
            if (value is AvaloniaObject aobj)
            {
                this.selectionBackgroundPropertyChangedHandlerToken = new(aobj, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
            }
            SetAndRaise(SelectionBackgroundProperty, ref field, value);
            if (SelectionStart != SelectionEnd)
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
            if (value is AvaloniaObject aobj)
            {
                this.selectionForegroundPropertyChangedHandlerToken = new(aobj, nameof(AvaloniaObject.PropertyChanged), OnBrushPropertyChanged);
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
        if (this.textProperties.Count == 0)
        {
            CreateTextProperties(defaultRunProperties);
        }

/*#if DEBUG
        StringBuilder sb = new("## TEXT PROPERTIES ##\r\n");
        sb.AppendLine($"## SELECTION: {SelectionStart}-{SelectionEnd}");
        foreach (var tp in this.textProperties)
        {
            sb.AppendLine($"“{text[tp.Start..(tp.Start + tp.Length)]}” ({tp.Start}-{tp.Start + tp.Length}): background {tp.Value.BackgroundBrush}, foreground {tp.Value.ForegroundBrush}");
        }
        sb.AppendLine("## TEXT PROPERTIES ##");
        Debug.WriteLine(sb.ToString());
#endif*/

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
#if DEBUG
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
#endif
        return this.textLayout;
    }

    private void CreateTextProperties(TextRunProperties defaultRunProperties)
    {
        // check text
        string text = Text ?? string.Empty;
        string preeditText = PreeditText ?? string.Empty;

        // insert text style for text
        if (!string.IsNullOrEmpty(text))
        {
            CreateTextPropertiesForText(defaultRunProperties, text);
        }

        // insert text style for preedit text
        if (!string.IsNullOrEmpty(preeditText))
        {
            CreateTextPropertiesForPreeditText(defaultRunProperties, text, preeditText);
        }
    }

    private void CreateTextPropertiesForText(TextRunProperties defaultRunProperties, string text)
    {
        var defs = DefinitionSet?.TokenDefinitions ?? [];
        var parts = RegexUtils.DelimitTextPartsOverRegexes(defs, text);
        foreach (var (definition, start, length, match) in parts)
        {
            if (definition == null) // normal text
            {
                AddTextProperties(start, length, defaultRunProperties);
            }
            else // token to be highlighted
            {
                int cacheId, regexMatchId;
                if (definition.RegexMatchIdMapper != null)
                {
                    // style varies according to regex match.
                    regexMatchId = definition.RegexMatchIdMapper(match!);
                    cacheId = definition.Id * 16 + regexMatchId;
                }
                else
                {
                    // fixed style for token. 
                    regexMatchId = -1;
                    cacheId = definition.Id;
                }

                if (!this.cachedTextRunProps.TryGetValue(cacheId, out var tokenRunProperties))
                {
                    tokenRunProperties =
                        this.cachedTextRunProps[cacheId] =
                        definition.MakeTextProperties(defaultRunProperties, FontStretch, regexMatchId);
                }
                AddTextProperties(start, length, tokenRunProperties);
            }
        }
    }

    private void CreateTextPropertiesForPreeditText(TextRunProperties defaultRunProperties, string text, string preeditText)
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
            this.textProperties.Add(new(0, preeditTextLength, runProperties));
            for (int i = this.textProperties.Count - 1; i > 0; --i)
            {
                var properties = this.textProperties[i];
                this.textProperties[i] = new(properties.Start + preeditTextLength, properties.Length, properties.Value);
            }
        }
        else if (caretIndex >= text.Length)
        {
            this.textProperties.Add(new(text.Length, preeditTextLength, runProperties));
        }
        else
        {
            int indexOfTextPropertiesToInsert = this.textProperties.Count - 1;
            var textPropertiesToInsert = this.textProperties[indexOfTextPropertiesToInsert];
            if (textPropertiesToInsert.Start > caretIndex)
            {
                for (int i = this.textProperties.Count - 2; i >= 0; --i)
                {
                    var properties = this.textProperties[i];
                    if (properties.Start <= caretIndex)
                    {
                        indexOfTextPropertiesToInsert = i;
                        textPropertiesToInsert = properties;
                        break;
                    }
                }
            }
            if (textPropertiesToInsert.Start == caretIndex)
                this.textProperties.Insert(indexOfTextPropertiesToInsert, new(caretIndex, preeditTextLength, runProperties));
            else
            {
                this.textProperties[indexOfTextPropertiesToInsert++] = new(textPropertiesToInsert.Start, caretIndex - textPropertiesToInsert.Start, textPropertiesToInsert.Value);
                this.textProperties.Insert(indexOfTextPropertiesToInsert++, new(caretIndex, preeditTextLength, runProperties));
                this.textProperties.Insert(indexOfTextPropertiesToInsert, new(caretIndex + preeditTextLength, textPropertiesToInsert.Length - (caretIndex - textPropertiesToInsert.Start), textPropertiesToInsert.Value));
            }
            for (int i = this.textProperties.Count - 1; i > indexOfTextPropertiesToInsert; --i)
            {
                var properties = this.textProperties[i];
                this.textProperties[i] = new(properties.Start + preeditTextLength, properties.Length, properties.Value);
            }
        }
    }

    private void AddTextProperties(int start, int length, TextRunProperties runProperties) =>
        this.textProperties.Add(new(start, length, runProperties));

    // Called when property of attached brush has been changed.
    private void OnBrushPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e) =>
        InvalidateTextProperties();

    // Invalidate text layout.
    private void InvalidateTextLayout()
    {
        this.textLayout = null;
        TextLayoutInvalidated?.Invoke(this, EventArgs.Empty);
    }

    // Invalidate text properties.
    private void InvalidateTextProperties()
    {
        this.textProperties.Clear();
        InvalidateTextLayout();
    }

    internal static void InvalidateSyntaxHighlighterTextBoxesTexts() =>
        Dispatcher.UIThread.Post(() => invalidateSyntaxHighlightersTextBoxesTextsCallbacks.ForEach(c => c()));

    /// <inheritdoc/>
    public void Dispose()
    {
        this.backgroundPropertyChangedHandlerToken?.Dispose();
        this.foregroundPropertyChangedHandlerToken?.Dispose();
        this.selectionBackgroundPropertyChangedHandlerToken?.Dispose();
        this.selectionForegroundPropertyChangedHandlerToken?.Dispose();
    }
}