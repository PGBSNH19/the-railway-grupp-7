using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSimulator
{
    public enum TrainState
    {
        atStartStation,
        onWayToClosingCrossing,
        onWayToOpenCrossing,
        onWayToFirstSwitch,
        onWayToMiddleStation,
        atMiddleStation,
        onWayToSecondSwitch,
        onWayToEndStation,
        atEndStation
    }
}
