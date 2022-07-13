using System.Text;

namespace Life.AutoTest
{
    internal static class Formatter
    {
        internal static string MakeConsoleTable(IEnumerable<string> items, int columnsCount)
        {
            int maxColLength = items.Max((item) => item.Length);
            int itemsCount = items.Count();
            string rowSample = GetRowSample(columnsCount, maxColLength);

            var builder = new StringBuilder();
            int skip = 0;

            for(int i = 0; i < itemsCount; i++)
            {
                var itemsToPrint = items.Skip(skip).Take(columnsCount).ToArray<object?>();
                var fullItemsArrayToPrint = itemsToPrint.Concat(itemsToPrint.Length >= columnsCount
                    ? Array.Empty<object?>()
                    : (object?[])Array.CreateInstance(typeof(object), columnsCount - itemsToPrint.Length))
                    .ToArray<object?>();

                builder.AppendLine(string.Format(rowSample, fullItemsArrayToPrint));
                skip += columnsCount;
            }

            return builder.ToString();
        }

        private static string GetRowSample(int columnsCount, int colWidth)
        {
            string result = string.Empty;

            for(int i = 0; i < columnsCount; i++)
            {
                result += $"{{{i},-{colWidth}}} ";
            }

            return result;
        }
    }
}
