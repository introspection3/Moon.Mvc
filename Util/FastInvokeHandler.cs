using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Moon.Mvc
{
	public delegate object FastInvokeHandler(object target, object[] paramters);
}
