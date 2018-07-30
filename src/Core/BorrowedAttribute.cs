using System;

namespace Lagan.Core
{
    // TODO: Can an attribute be specified on any variable?
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class BorrowedAttribute : Attribute
    {
    }
}
