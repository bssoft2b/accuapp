using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuApp.Services
{
    public interface IDistanceCounter
    {
        double CalculateDistance(string start, string end);
    }
}
