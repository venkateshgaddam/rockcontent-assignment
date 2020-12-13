using System;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RockContent.Common.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private bool _disposed;

        private readonly IDbConnection dbConnection;
        private readonly IDapperExtensionsConfiguration dapperExtensionsConfiguration;

        public GenericRepository(IDbConnection connection)
        {
            dbConnection = connection;
            dapperExtensionsConfiguration = new DapperExtensionsConfiguration();
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                if (disposing && dbConnection != null && dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<T> GetListAsync(object name, dynamic id)
        {
            string sqlQuery = $"SELECT * FROM features.Activity WHERE {name} = '{id}'";
            return await dbConnection.QuerySingleOrDefaultAsync<T>(sqlQuery, commandTimeout: 30);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity)
        {
            IClassMapper classMapper = dapperExtensionsConfiguration.GetMap<T>();
            string sql = InsertAndReturn(classMapper);
            var result = await dbConnection.QuerySingleOrDefaultAsync<T>(sql, entity, commandTimeout: 30, commandType: CommandType.Text);

            return result;

        }

        /// <summary>
        /// The asynchronous counterpart to <see cref="IDapperImplementor.Update{T}(IDbConnection, T, IDbTransaction, int?)"/>.
        /// </summary>
        public async Task<T> UpdateAsync(T entity)
        {
            var classMap = dapperExtensionsConfiguration.GetMap<T>();

            var parameters = new Dictionary<string, object>();
            var existsParameters = new Dictionary<string, object>();

            var sql = UpdateAndReturn(classMap, parameters, existsParameters, "Likes");
            var dynamicParameters = new DynamicParameters();

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity || p.KeyType == KeyType.Assigned));

            IDictionary<string, object> entityObjectValues = new Dictionary<string, object>();

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(entity, null);
                entityObjectValues[name] = value;
            }

            foreach (var property in entityObjectValues.Where(property => columns.Any(c => c.Name == property.Key)))
            {
                dynamicParameters.Add(property.Key, property.Value);
            }

            foreach (var parameter in parameters)
            {
                dynamicParameters.Add(parameter.Key, parameter.Value);
            }

            return await dbConnection.QuerySingleOrDefaultAsync<T>(sql, dynamicParameters, commandTimeout: 30, commandType: CommandType.Text).ConfigureAwait(false);
        }



        public virtual string UpdateAndReturn(IClassMapper classMap, IDictionary<string, object> parameters, IDictionary<string, object> existsParameters, string schema, IEnumerable<string> cols = null)
        {

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            IEnumerable<IPropertyMap> columns = null;

            if (cols == null)
            {
                columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            }
            else
            {
                columns = cols?.Join(classMap.Properties, x => x.ToLowerInvariant(), y => y.ColumnName.ToLowerInvariant(), (x, y) => y)?.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            }

            if (!columns?.Any() ?? true)
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var setSql =
              columns.Select(
                p =>
                string.Format(
                  "{0} = {1}{2}", p.Name, "@", p.Name));

            return GetUpdateAndReturn("features.Like",
              AppendStrings(setSql), $"ArticleId={parameters["ArticleId"]} AND UserId = {parameters["UserId"]}");
        }


        private string GetUpdateAndReturn(string tableName, string setString, string conditionString)
        {
            return string.Format("UPDATE {0} SET {1} WHERE {2} RETURNING *;",
               tableName, setString, conditionString);
        }

        private string InsertAndReturn(IClassMapper classMap)
        {
            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var columnNames = columns.Select(p => p.Name);
            var parameters = columns.Select(p => "@" + p.Name);

            return GetInsertAndReturnSql("features.Like", AppendStrings(columnNames), AppendStrings(parameters));
        }

        private string AppendStrings(IEnumerable<string> list, string seperator = ",")
        {
            return list.Aggregate(
                new StringBuilder(),
                (sb, s) => (sb.Length == 0 ? sb : sb.Append(seperator)).Append(s),
                sb => sb.ToString());
        }

        private string GetInsertAndReturnSql(string tableName, string columnNames, string valueString)
        {
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2}) RETURNING *;", tableName, columnNames, valueString);
        }
    }

    public interface IDapperExtensionsConfiguration
    {
        Type DefaultMapper { get; }
        IList<Assembly> MappingAssemblies { get; }
        IClassMapper GetMap(Type entityType);
        IClassMapper GetMap<T>() where T : class;
    }

    public class DapperExtensionsConfiguration : IDapperExtensionsConfiguration
    {
        private readonly ConcurrentDictionary<Type, IClassMapper> _classMaps = new ConcurrentDictionary<Type, IClassMapper>();

        public DapperExtensionsConfiguration()
        {
        }

        public DapperExtensionsConfiguration(Type defaultMapper, IList<Assembly> mappingAssemblies)
        {
            DefaultMapper = defaultMapper;
            MappingAssemblies = mappingAssemblies ?? new List<Assembly>();
        }

        public Type DefaultMapper { get; private set; }
        public IList<Assembly> MappingAssemblies { get; private set; }

        public IClassMapper GetMap(Type entityType)
        {
            if (!_classMaps.TryGetValue(entityType, out IClassMapper map))
            {
                Type mapType = GetMapType(entityType);
                if (mapType == null)
                {
                    mapType = DefaultMapper.MakeGenericType(entityType);
                }

                map = Activator.CreateInstance(mapType) as IClassMapper;
                _classMaps[entityType] = map;
            }

            return map;
        }

        public IClassMapper GetMap<T>() where T : class
        {
            return GetMap(typeof(T));
        }

        protected virtual Type GetMapType(Type entityType)
        {
            Func<Assembly, Type> getType = a =>
            {
                Type[] types = a.GetTypes();
                return (from type in types
                        let interfaceType = type.GetInterfaces().Where(x => x.Name == typeof(IClassMapper<>).Name && x.Namespace == typeof(IClassMapper<>).Namespace).FirstOrDefault()
                        where
                          interfaceType != null &&
                          interfaceType.GetGenericArguments()[0] == entityType
                        select type).SingleOrDefault();
            };

            Type result = getType(entityType.GetTypeInfo().Assembly);
            if (result != null)
            {
                return result;
            }

            foreach (var mappingAssembly in MappingAssemblies)
            {
                result = getType(mappingAssembly);
                if (result != null)
                {
                    return result;
                }
            }

            return getType(entityType.GetTypeInfo().Assembly);
        }
    }

    /// <summary>
        /// Maps an entity property to its corresponding column in the database.
        /// </summary>
    public interface IPropertyMap
    {
        string Name { get; }
        string ColumnName { get; }
        bool Ignored { get; }
        bool IsReadOnly { get; }
        KeyType KeyType { get; }
        PropertyInfo PropertyInfo { get; }
    }

    /// <summary>
        /// Maps an entity property to its corresponding column in the database.
        /// </summary>
    public class PropertyMap : IPropertyMap
    {
        public PropertyMap(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            ColumnName = PropertyInfo.Name;
        }

        /// <summary>
                /// Gets the name of the property by using the specified propertyInfo.
                /// </summary>
        public string Name
        {
            get { return PropertyInfo.Name; }
        }

        /// <summary>
                /// Gets the column name for the current property.
                /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
                /// Gets the key type for the current property.
                /// </summary>
        public KeyType KeyType { get; private set; }

        /// <summary>
                /// Gets the ignore status of the current property. If ignored, the current property will not be included in queries.
                /// </summary>
        public bool Ignored { get; private set; }

        /// <summary>
                /// Gets the read-only status of the current property. If read-only, the current property will not be included in INSERT and UPDATE queries.
                /// </summary>
        public bool IsReadOnly { get; private set; }

        /// <summary>
                /// Gets the property info for the current property.
                /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }
    }

    /// <summary>
        /// Used by ClassMapper to determine which entity property represents the key.
        /// </summary>
    public enum KeyType
    {
        /// <summary>
                /// The property is not a key and is not automatically managed.
                /// </summary>
        NotAKey,

        /// <summary>
                /// The property is an integery-based identity generated from the database.
                /// </summary>
        Identity,

        /// <summary>
                /// The property is an identity generated by the database trigger.
                /// </summary>
        TriggerIdentity,

        /// <summary>
                /// The property is a Guid identity which is automatically managed.
                /// </summary>
        Guid,

        /// <summary>
                /// The property is a key that is not automatically managed.
                /// </summary>
        Assigned
    }

}
