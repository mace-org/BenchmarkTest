using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest
{
    public class ExpressionTest
    {
        Func<object[], int> _func;
        Func<object[], int> _exprFunc;
        object[] _values;
        public ExpressionTest()
        {
            _func = MakeFunc(TargetFunc);
            _exprFunc = MakeExprFunc(TargetFunc);
            _values = new[] { "str1", "str2", "str3" };
        }

        private int TargetFunc(string str1, string str2, string str3)
        {
            return str1?.Length ?? 0 + str2?.Length ?? 0 + str3?.Length ?? 0;
        }

        private Func<object[], int> MakeFunc(Func<string, string, string, int> func)
        {
            return objs => func((string)objs[0], (string)objs[1], (string)objs[2]);
        }

        private Func<object[], int> MakeExprFunc(Func<string, string, string, int> func)
        {
            var paramExpr = Expression.Parameter(typeof(object[]), "objs");
            var argExprs = new[]
            {
                Expression.Convert(Expression.ArrayIndex(paramExpr, Expression.Constant(0)), typeof(string)),
                Expression.Convert(Expression.ArrayIndex(paramExpr, Expression.Constant(1)), typeof(string)),
                Expression.Convert(Expression.ArrayIndex(paramExpr, Expression.Constant(2)), typeof(string))
            };
            var bodyExpr = Expression.Call(Expression.Constant(func.Target), func.Method, argExprs);
            var lambdaExpr = Expression.Lambda<Func<object[], int>>(bodyExpr, paramExpr);
            return lambdaExpr.Compile();
        }

        [Benchmark]
        public int CallFunc() => _func(_values);

        [Benchmark]
        public int CallExprFunc() => _exprFunc(_values);
    }
}
