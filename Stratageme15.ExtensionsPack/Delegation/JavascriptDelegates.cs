using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stratageme15.ExtensionsPack.Delegation
{
    public delegate void JavascriptAction<in TThis>(TThis thisContext);

    public delegate void JavascriptAction<in TThis, in TArg1>(TThis thisContext, TArg1 arg1);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2>(TThis thisContext, TArg1 arg1, TArg2 arg2);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12, in TArg13>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12, in TArg13, in TArg14>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12, in TArg13, in TArg14, in TArg15>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15);
    
    public delegate void JavascriptAction<in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12, in TArg13, in TArg14, in TArg15, in TArg16>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, TArg16 arg16);

    public delegate TResult JavascriptFunc<out TResult, in TThis>(TThis thisContext);

    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1>(TThis thisContext, TArg1 arg1);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2>(TThis thisContext, TArg1 arg1, TArg2 arg2);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12, in TArg13>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12, in TArg13, in TArg14>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12, in TArg13, in TArg14, in TArg15>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15);
    
    public delegate TResult JavascriptFunc<out TResult, in TThis, in TArg1, in TArg2, in TArg3, in TArg4, in TArg5, in TArg6, in TArg7, in TArg8, in TArg9, in TArg10, in TArg11, in TArg12, in TArg13, in TArg14, in TArg15, in TArg16>(TThis thisContext, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6, TArg7 arg7, TArg8 arg8, TArg9 arg9, TArg10 arg10, TArg11 arg11, TArg12 arg12, TArg13 arg13, TArg14 arg14, TArg15 arg15, TArg16 arg16);
}
