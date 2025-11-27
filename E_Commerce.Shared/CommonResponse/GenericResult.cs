using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResponse
{
    public class GenericResult<TValue>:Result
    {
        private readonly TValue _value;

        public TValue Value => IsSuccess?_value:throw new InvalidOperationException("You Can't Access The Value In Case of Failure Scenario");

        private GenericResult(TValue value):base()
        {
            _value = value;
        }
        private GenericResult(Error error):base(error)
        {
            _value = default!;
        }

        private GenericResult(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        public static GenericResult<TValue> Ok(TValue value) => new(value);

        public static new GenericResult<TValue>Fail (Error error) =>new(error);

        public static new GenericResult<TValue>Fail (List<Error> errors) => new(errors);

    }
}
