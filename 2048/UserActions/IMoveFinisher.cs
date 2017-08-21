using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2048
{
    internal interface IMoveFinisher
    {
        Vector2 Direction { get; }
        void DeactivateFinishedCells();
    }
}