using System;
using System.Collections.Generic;
using System.Numerics;

namespace RockContent.Common.DAL
{
    public interface IClassMapper
    {
        string SchemaName { get; }
        string TableName { get; }
        IList<IPropertyMap> Properties { get; }
        Type EntityType { get; }
    }

    public interface IClassMapper<T> : IClassMapper where T : class
    {
    }

    /// <summary>
    /// Maps an entity to a table through a collection of property maps.
    /// </summary>
    public class ClassMapper<T> : IClassMapper<T> where T : class
    {
        /// <summary>
        /// Gets or sets the schema to use when referring to the corresponding table name in the database.
        /// </summary>
        public string SchemaName { get; protected set; }

        /// <summary>
        /// Gets or sets the table to use in the database.
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// A collection of properties that will map to columns in the database table.
        /// </summary>
        public IList<IPropertyMap> Properties { get; private set; }

        public Type EntityType
        {
            get { return typeof(T); }
        }

        public ClassMapper()
        {
            PropertyTypeKeyTypeMapping = new Dictionary<Type, KeyType>
                      {
                        { typeof(byte), KeyType.Identity }, { typeof(byte?), KeyType.Identity },
                        { typeof(sbyte), KeyType.Identity }, { typeof(sbyte?), KeyType.Identity },
                        { typeof(short), KeyType.Identity }, { typeof(short?), KeyType.Identity },
                        { typeof(ushort), KeyType.Identity }, { typeof(ushort?), KeyType.Identity },
                        { typeof(int), KeyType.Identity }, { typeof(int?), KeyType.Identity },
                        { typeof(uint), KeyType.Identity}, { typeof(uint?), KeyType.Identity },
                        { typeof(long), KeyType.Identity }, { typeof(long?), KeyType.Identity },
                        { typeof(ulong), KeyType.Identity }, { typeof(ulong?), KeyType.Identity },
                        { typeof(BigInteger), KeyType.Identity }, { typeof(BigInteger?), KeyType.Identity },
                        { typeof(Guid), KeyType.Guid }, { typeof(Guid?), KeyType.Guid },
                      };

            Properties = new List<IPropertyMap>();
            Table(typeof(T).Name);
        }

        protected Dictionary<Type, KeyType> PropertyTypeKeyTypeMapping { get; private set; }

        public virtual void Table(string tableName)
        {
            TableName = tableName;
        }
 
    }
}
