using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using DynamicData;

namespace Pororoca.Desktop.Controls;

/// <summary>
/// Set of syntax highlighting definition.
/// </summary>
public sealed class SyntaxHighlightingDefinitionSet
{
    // Fields.
    private readonly Dictionary<object, HashSet<SyntaxHighlightingToken>> attachedTokenDefinitions = new();
    private readonly ObservableCollection<SyntaxHighlightingSpan> spanDefinitions = new();
    private readonly ObservableCollection<SyntaxHighlightingToken> tokenDefinitions = new();
    private int validDefinitionsCount;

    /// <summary>
    /// Initialize new <see cref="SyntaxHighlightingDefinitionSet"/> instance.
    /// </summary>
    /// <param name="name">Name.</param>
    public SyntaxHighlightingDefinitionSet(string name)
    {
        Name = name;
        this.spanDefinitions.CollectionChanged += OnSpanDefinitionsChanged;
        this.tokenDefinitions.CollectionChanged += OnTokenDefinitionsChanged;
        this.attachedTokenDefinitions[this] = new();
    }

    // Attach to given span definition.
    private void AttachToSpanDefinition(SyntaxHighlightingSpan spanDefinition)
    {
        if (!this.attachedTokenDefinitions.TryGetValue(spanDefinition, out var attachedTokenDefinitions))
        {
            attachedTokenDefinitions = new();
            this.attachedTokenDefinitions[spanDefinition] = attachedTokenDefinitions;
        }
        else
            throw new InvalidOperationException("Duplicated span definition.");
        if (spanDefinition.IsValid)
            ++this.validDefinitionsCount;
        spanDefinition.PropertyChanged += OnDefinitionPropertyChanged;
        ((INotifyCollectionChanged)spanDefinition.TokenDefinitions).CollectionChanged += OnTokenDefinitionsChanged;
        foreach (var tokenDefinition in spanDefinition.TokenDefinitions)
            AttachToTokenDefinition(tokenDefinition, attachedTokenDefinitions);
    }


    // Attach to given token definition.
    private void AttachToTokenDefinition(SyntaxHighlightingToken tokenDefinition, HashSet<SyntaxHighlightingToken> attachedTokenDefinitions)
    {
        if (!attachedTokenDefinitions.Add(tokenDefinition))
            throw new InvalidOperationException("Duplicated token definition.");
        if (tokenDefinition.IsValid)
            ++this.validDefinitionsCount;
        tokenDefinition.PropertyChanged += OnDefinitionPropertyChanged;
    }


    // Detach from given span definition.
    private void DetachFromSpanDefinition(SyntaxHighlightingSpan spanDefinition)
    {
        if (spanDefinition.IsValid)
            --this.validDefinitionsCount;
        spanDefinition.PropertyChanged -= OnDefinitionPropertyChanged;
        ((INotifyCollectionChanged)spanDefinition.TokenDefinitions).CollectionChanged -= OnTokenDefinitionsChanged;
        foreach (var tokenDefinition in spanDefinition.TokenDefinitions)
            DetachFromTokenDefinition(tokenDefinition, null);
        this.attachedTokenDefinitions.Remove(spanDefinition);
    }


    // Detach from given token definition.
    private void DetachFromTokenDefinition(SyntaxHighlightingToken tokenDefinition, HashSet<SyntaxHighlightingToken>? attachedTokenDefinitions)
    {
        if (tokenDefinition.IsValid)
            --this.validDefinitionsCount;
        tokenDefinition.PropertyChanged -= OnDefinitionPropertyChanged;
        attachedTokenDefinitions?.Remove(tokenDefinition);
    }

    /// <summary>
    /// Raised when one of definitions in the set has been changed.
    /// </summary>
    public event EventHandler? Changed;

    /// <summary>
    /// Check whether at least one token or span definition has been added to the set or not.
    /// </summary>
    public bool HasDefinitions => this.tokenDefinitions.Any() || this.spanDefinitions.Any();

    /// <summary>
    /// Check whether at least one token or span definition in the set is valid or not.
    /// </summary>
    public bool HasValidDefinitions => this.validDefinitionsCount > 0;

    /// <summary>
    /// Get name of the set.
    /// </summary>
    public string Name { get; }

    // Raise changed event.
    private void OnChanged() =>
        Changed?.Invoke(this, EventArgs.Empty);

    // Called when property of definition changed.
    private void OnDefinitionPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not SyntaxHighlightingDefinition definition)
            return;
        if (e.PropertyName == nameof(SyntaxHighlightingDefinition.IsValid))
        {
            if (definition.IsValid)
                ++this.validDefinitionsCount;
            else
                --this.validDefinitionsCount;
            OnChanged();
        }
        else if (definition.IsValid)
            OnChanged();
    }

    // Called when collection of span definitions changed.
    private void OnSpanDefinitionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var definition in e.NewItems!.Cast<SyntaxHighlightingSpan>())
                {
                    AttachToSpanDefinition(definition);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (var definition in e.OldItems!.Cast<SyntaxHighlightingSpan>())
                {
                    DetachFromSpanDefinition(definition);
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                foreach (var definition in e.OldItems!.Cast<SyntaxHighlightingSpan>())
                {
                    DetachFromSpanDefinition(definition);
                }
                foreach (var definition in e.NewItems!.Cast<SyntaxHighlightingSpan>())
                {
                    AttachToSpanDefinition(definition);
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                foreach (object key in this.attachedTokenDefinitions.Keys.ToArray())
                {
                    if (key is SyntaxHighlightingSpan span)
                        DetachFromSpanDefinition(span);
                }
                foreach (var definition in this.spanDefinitions)
                {
                    AttachToSpanDefinition(definition);
                }
                break;
        }
        OnChanged();
    }

    // Called when collection of token definitions changed.
    private void OnTokenDefinitionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (sender is not IList<SyntaxHighlightingToken> tokenDefinitions)
            return;
        var owner = ReferenceEquals(tokenDefinitions, this.tokenDefinitions)
            ? (object?)this
            : this.spanDefinitions.FirstOrDefault(it => ReferenceEquals(tokenDefinitions, it.TokenDefinitions));
        if (owner == null || !this.attachedTokenDefinitions.TryGetValue(owner, out var attachedTokenDefinitions))
            return;
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var definition in e.NewItems!.Cast<SyntaxHighlightingToken>())
                {
                    AttachToTokenDefinition(definition, attachedTokenDefinitions);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                foreach (var definition in e.OldItems!.Cast<SyntaxHighlightingToken>())
                {
                    DetachFromTokenDefinition(definition, attachedTokenDefinitions);
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                foreach (var definition in e.OldItems!.Cast<SyntaxHighlightingToken>())
                {
                    DetachFromTokenDefinition(definition, attachedTokenDefinitions);
                }
                foreach (var definition in e.NewItems!.Cast<SyntaxHighlightingToken>())
                {
                    AttachToTokenDefinition(definition, attachedTokenDefinitions);
                }
                break;
            case NotifyCollectionChangedAction.Reset:
                foreach (var definition in attachedTokenDefinitions.ToArray())
                {
                    DetachFromTokenDefinition(definition, attachedTokenDefinitions);
                }
                if (owner is SyntaxHighlightingSpan spanDefinition)
                {
                    foreach (var definition in spanDefinition.TokenDefinitions)
                    {
                        AttachToTokenDefinition(definition, attachedTokenDefinitions);
                    }
                }
                else
                {
                    foreach (var definition in this.tokenDefinitions)
                    {
                        AttachToTokenDefinition(definition, attachedTokenDefinitions);
                    }
                }
                break;
        }
        OnChanged();
    }

    /// <summary>
    /// Get list of span definitions.
    /// </summary>
    public IList<SyntaxHighlightingSpan> SpanDefinitions => this.spanDefinitions;

    /// <summary>
    /// Get list of token definitions.
    /// </summary>
    public IList<SyntaxHighlightingToken> TokenDefinitions => this.tokenDefinitions;

    /// <inheritdoc/>
    public override string ToString() =>
        $"{{{Name}}}";
}