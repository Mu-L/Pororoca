namespace Pororoca.Desktop.Others;

internal sealed class SortedList<T> : List<T> where T : notnull
{
    private readonly IComparer<T> comparer;

    public SortedList(IComparer<T> comparer, IEnumerable<T>? initialElements = null) : base()
    {
        this.comparer = comparer;
        SetupInitialElements(initialElements);
    }

    public SortedList(Comparison<T> comparison, IEnumerable<T>? initialElements = null)
        : this(Comparer<T>.Create(comparison), initialElements) { }

    public SortedList(IEnumerable<T>? initialElements = null)
    {
        if (!typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
            throw new ArgumentException($"Element type '{typeof(T).Name}' doesn't implement IComparable.");

        this.comparer = Comparer<T>.Default;
        SetupInitialElements(initialElements);
    }

    private void SetupInitialElements(IEnumerable<T>? initialElements)
    {
        if (initialElements != null)
        {
            AddRange(initialElements.OrderBy(x => x, this.comparer));
        }
    }

    /// <summary>
    /// Add element to list, returning its insertion index.
    /// </summary>
    /// <param name="element">Element to add.</param>
    /// <returns>Index of added element in list.</returns>
    public int AddReturningInsertionIndex(T element)
    {
        // insert at the end of list
        if (Count == 0 || this.comparer.Compare(element, this[Count - 1]) >= 0)
        {
            Add(element);
            return Count;
        }

        // insert at the start of list
        if (this.comparer.Compare(element, this[0]) <= 0)
        {
            Insert(0, element);
            return 0;
        }

        // find position and insert
        int insertionIndex = BinarySearch(element, this.comparer);
        if (insertionIndex < 0)
        {
            insertionIndex = ~insertionIndex;
        }
        Insert(insertionIndex, element);
        return insertionIndex;
    }

    public int BinarySearch(T element, Comparison<T> comparison) =>
        BinarySearch(this, 0, Count, element, comparison);

    private static int BinarySearch(IList<T> list, int start, int end, T element, Comparison<T> comparison)
    {
        if (start >= end)
            return ~start;

        int middle = (start + end) / 2;
        int result = comparison(element, list[middle]);

        if (result == 0)
            return middle;

        if (result < 0)
        {
            return BinarySearch(list, start, middle, element, comparison);
        }
        else
        {
            return BinarySearch(list, middle + 1, end, element, comparison);
        }
    }
}