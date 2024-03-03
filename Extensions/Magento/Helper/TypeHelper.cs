using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Extensions.Magento.Helper

{
    internal class TypeHelper
    {

        public bool ConvertBoolean(string stringbool)
        {

            if (stringbool.ToUpper() == "TRUE")
            {
                return true;

            }

            if (stringbool.ToUpper() == "FALSE")
            {
                return false;

            }

            if (stringbool == "1")
            {
                return true;

            }

            if (stringbool == "0")
            {
                return false;

            }


            throw new System.Exception("Not boolean");

        }


    }
}
