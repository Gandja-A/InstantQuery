using System;

namespace InstantQuery
{
    internal static class Utils
    {
        public static bool IsNullable(Type type)
        {
            if(!type.IsValueType)
            {
                return true;
            }

            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}
