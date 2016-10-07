using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moon.Mvc
{
    public interface ICaptcha
    {
        string Value
        {
            get;
            set;
        }
        byte[] GetImageByte();
    }
}
