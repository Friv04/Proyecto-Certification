using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPB.BusinessLogic.Managers.Exceptions
{
    public class JSONValueNotFoundException : Exception
    {
        private const string BaseMessage = "The JSON value/section you're looking for doesn't exist or is not properly written.";
        public JSONValueNotFoundException() : base(BaseMessage) { }

        public JSONValueNotFoundException(string[] sections) : base(BaseMessage + ".\nLittle hint, maybe look for these words: " + string.Join(",", sections)) { }

        public string getError()
        {
            return BaseMessage;
        }
    }
}
