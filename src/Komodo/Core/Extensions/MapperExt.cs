using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Komodo.Core.Extensions
{
    public static class ConditionExtensions
    {
        public static void IgnoreIfSourceIsNull<T>(this IMemberConfigurationExpression<T> expression)
        {
            expression.Condition(IgnoreIfSourceIsNull);
        }

        static bool IgnoreIfSourceIsNull(ResolutionContext context)
        {
            if (!context.IsSourceValueNull)
            {
                return true;
            }
            var result = context.GetContextPropertyMap().ResolveValue(context.Parent);
            return result.Value != null;
        }
    }
}
