using System.Reflection;

namespace Domain.Utilities
{
    /// <summary>
    /// Helper for the reflexion Functions
    /// </summary>
    public static class ReflexionUtility
    {
        /// <summary>
        /// Get the collection of the TypeInfo from the Assembly, filtered by the predicate
        /// </summary>
        /// <param name="assembly">Assembly for check</param>
        /// <param name="predicate">Filtering predicate</param>
        /// <returns>Collection of the TypeInfo objects</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public static IEnumerable<TypeInfo> GetObjectsTypeInfo(Assembly assembly,
            Func<TypeInfo, bool> predicate)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            var types = assembly.DefinedTypes;

            if (types.Count() == 0)
                throw new Exception("Assembly is empty!");

            var objects = types.Where(predicate);

            if (objects.Count() == 0)
                throw new Exception("Fail to find some TypeInfo objects!");

            return objects;
        }
    }
}
