using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    public class Error
    {
        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }
        public string Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }

        // Static Factory Methods To Create Error
        public static Error Failure(string code = "General.Failure", string description = "General Failure Has Occured")
        {
            return new Error(code, description, ErrorType.Failure);
        }
        public static Error Validation(string code = "General.Validation", string description = "Validation Error Has Occured")
        {
            return new Error(code, description, ErrorType.Validation);
        }
        public static Error NotFound(string code = "General.NotFound", string description = "The Requested Resource Has Not Found")
        {
            return new Error(code, description, ErrorType.NotFound);
        }
        public static Error Unauthrized(string code = "General.Unauthrized", string description = "You are Not Authrized")
        {
            return new Error(code, description, ErrorType.Unauthrized);
        }
        public static Error Forbidden(string code = "General.Forbidden", string description = "You Do Not Have Permissions")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }
        public static Error InvalidCrendentials(string code = "General.InvalidCrendentials", string description = "The Provided Crendential Not Valid")
        {
            return new Error(code, description, ErrorType.InvalidCrendentials);
        }


    }
}
