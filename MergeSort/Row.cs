namespace MergeSort
{
    struct Row : IComparable<Row>
    {
        private readonly int _dotPos;
        private readonly int _number;
        private string _separator = ".";
        private string _row { get; }

        public int Number { get { return _number; } }
        public StreamReader Reader { get; } = default!;

        public Row(string row)
        {
            _row = row;
            _dotPos = row.IndexOf(_separator, StringComparison.Ordinal);
            _number = int.Parse(row.AsSpan(0, _dotPos));
        }

        public Row(string row, StreamReader reader) : this(row)
        {
            Reader = reader;
        }

        public ReadOnlySpan<char> GetWord()
        {
            return _row.AsSpan(_dotPos + 2);
        }

        public override string ToString()
        {
            return _row;
        }

        public int CompareTo(Row other)
        {
            var result = GetWord().CompareTo(other.GetWord(), StringComparison.Ordinal);
            if (result != 0)
            {
                return result;
            }
            else
            {
                return _number.CompareTo(other._number);
            }
        }
    }
}
