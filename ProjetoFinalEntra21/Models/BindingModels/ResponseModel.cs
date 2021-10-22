using ProjetoFinalEntra21.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Enum;

namespace ProjetoFinalEntra21.Models
{
    public class ResponseModel
    {
        public ResponseModel(ResponseCode responseCode, string responseMessage, object dataSet)
        {
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
            DateSet = dataSet;
        }
        public ResponseCode ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object DateSet { get; set; }
    }
}

