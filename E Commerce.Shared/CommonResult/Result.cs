using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    public class Result
    {
        private readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count == 0;
        public bool ISFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors => _errors;
        // Ok - Success
        protected Result()
        {
            
        }
        // Fail with Error
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        // Fail with Errors
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }
        // Ok - Success
        public static Result Ok() => new Result();
        // Fail with Error
        public static Result Fail(Error error) => new Result(error);
        // Fail with Errors
        public static Result Fail(List<Error> errors) => new Result(errors);
    }

    public class Result<TValue> : Result 
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Can Not Access The Value");
        // Ok - Success
        private Result(TValue value):base()
        {
            _value = value;
        }
        // Fail with Error
        private Result(Error error):base(error)
        {
            _value = default!;   
        }
        // Fail with Errors
        private Result(List<Error>errors) : base(errors)
        {
            _value = default!;
        }
        public static Result<TValue> Ok(TValue value) => new(value);
        public static new Result<TValue> Fail(Error error) => new(error);
        public static new Result<TValue> Fail(List<Error> errors) => new(errors);

        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}
