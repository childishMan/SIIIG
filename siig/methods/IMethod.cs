using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace siig.methods
{
    interface IMethod
    {
        bool IsAllChecked();
        void Proceed();
    } 
}
