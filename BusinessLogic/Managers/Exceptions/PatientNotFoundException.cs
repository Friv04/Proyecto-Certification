using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPB.BusinessLogic.Managers.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        private const string BaseMessage = "The Patient you're looking for was not found.";
        public PatientNotFoundException() : base(BaseMessage) { }
        public PatientNotFoundException(string method) : base("In the " + method + " method. " + BaseMessage) { }

        public string GetErrorType()
        {
            return BaseMessage;
        }
    }
}
