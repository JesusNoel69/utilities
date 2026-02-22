using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace utilities.Interfaces
{
    public interface IScreenColorPicker
    {
        bool IsSupported { get; }
        Color GetColorUnderCursor();
    }
}