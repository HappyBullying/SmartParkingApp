using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingApp.ClassLibrary.Models
{
    public class ResponseResult<T>
    {
        public T ResultData { get; set; }
        public string Message { get; set; }
    }
}
