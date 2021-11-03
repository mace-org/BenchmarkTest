using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest
{
    public class RequestBase
    {
        public string Data { get; set; }
    }

    public class Request : RequestBase
    {
        public int Length { get; set; }
    }

    public class InvokeTest
    {
        private Func<Request, int> _func;
        private Func<RequestBase, int> _baseFunc;
        private Delegate _delegate;
        private Request _request;
     //   private RequestBase _requestBase;

        public InvokeTest()
        {
            _func = Invoker;
            _baseFunc = req => _func((Request)req);
            _delegate = _func;
            _request = new Request { Data = "InvokeTest" };
        }

        private int Invoker(Request request)
        {
            return request.Data.Length + request.Length;
        }

        [Benchmark]
        public void Call() => _func(_request);

        [Benchmark]
        public void Invoke() => _func.Invoke(_request);

        [Benchmark]
        public void BasicCall() => _baseFunc(_request);

        [Benchmark]
        public void BasicInvoke() => _baseFunc.Invoke(_request);

        [Benchmark]
        public void DynamicInvoke() => _delegate.DynamicInvoke(_request);

        [Benchmark]
        public void ConvertCall() => ((Func<Request, int>)_delegate)(_request);

    }
}
