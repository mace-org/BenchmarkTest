using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest
{
    public class Animal
    {
        public Animal(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }

    public class Dog : Animal
    {
        public Dog(string name) : base(name)
        {

        }
    }

    public class CreateInstanceTest
    {
        private string _name;
        private ConstructorInfo _ctor;
        private Func<string, Dog> _func;

        public CreateInstanceTest()
        {
            _name = "wangcai";
            _ctor = typeof(Dog).GetConstructors()[0];

            var paramExpr = Expression.Parameter(typeof(string), "name");
            var bodyExpr = Expression.New(_ctor, paramExpr);
            _func = Expression.Lambda<Func<string, Dog>>(bodyExpr, paramExpr).Compile();
        }

        [Benchmark]
        public Dog UseNew() => new Dog(_name);

        [Benchmark]
        public Dog UseCtor() => (Dog)_ctor.Invoke(new[] { _name });

        [Benchmark]
        public Dog UseExpress() => _func(_name);

        [Benchmark]
        public Dog UseActivator() => (Dog)Activator.CreateInstance(typeof(Dog), new[] { _name });

    }
}
