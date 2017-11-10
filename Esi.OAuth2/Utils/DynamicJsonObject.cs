using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Esi.OAuth2.Utils
{
    public class DynamicJsonObject : DynamicObject
    {

        private IDictionary<string, object> Dictionary { get; set; }

        public DynamicJsonObject(IDictionary<string, object> dictionary)
        {
            Dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Dictionary[binder.Name];

            if (result is IDictionary<string, object>)
            {
                result = new DynamicJsonObject(result as IDictionary<string, object>);
            }
            else if (result is ArrayList)
            {
                result = new List<object>((result as ArrayList).ToArray());
            }

            return Dictionary.ContainsKey(binder.Name);
        }

    }
}
