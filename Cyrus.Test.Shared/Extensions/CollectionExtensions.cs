using System.Data;
using System.Reflection;

namespace Cyrus.Test.Shared.Extensions
{
    public static class CollectionExtensions
    {
        private static readonly IDictionary<Type, ICollection<PropertyInfo>> _properties =
            new Dictionary<Type, ICollection<PropertyInfo>>();

        public static IEnumerable<T> ToList<T>(this DataTable dataTable) where T : class, new()
        {
            var typeOfT = typeof(T);
            ICollection<PropertyInfo> properties;

            lock (_properties)
            {
                if (!_properties.TryGetValue(typeOfT, out properties))
                {
                    properties = typeOfT.GetProperties().Where(p => p.CanWrite).ToList();
                    _properties.Add(typeOfT, properties);
                }
            }

            var list = new List<T>(dataTable.Rows.Count);
            foreach (var row in dataTable.AsEnumerable())
            {
                var obj = new T();
                foreach (var prop in properties)
                {
                    if (dataTable.Columns.Contains(prop.Name))
                    {
                        var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        var safeValue = row[prop.Name] == null ? null : Convert.ChangeType(row[prop.Name], propType);

                        prop.SetValue(obj, safeValue, null);
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        public static bool NotNullOrEmpty<T>(this IEnumerable<T> source)
            => source != null && source.Any();
    }
}
