using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPB.BusinessLogic.Managers.Exceptions
{
    public class PatientAlreadyExistsException : Exception
    {
        private const string BaseMessage = "The CI provided is already registered, please check if the provided information is correct, or update the patient data.";

        public PatientAlreadyExistsException() : base(BaseMessage) { }
    }
}
