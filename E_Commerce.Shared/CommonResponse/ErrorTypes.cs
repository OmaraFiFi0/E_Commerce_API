using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResponse
{
    public enum ErrorTypes
    {
        Faliure = 0 , 
        Validation = 1 , 
        NotFound=2 ,
        UnAuthorized = 3 ,
        Forbidden = 4 ,
        InValidCredintals =5 ,  
    }
}
