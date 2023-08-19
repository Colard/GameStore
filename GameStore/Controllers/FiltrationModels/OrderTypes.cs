using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Controllers.FiltrationModels
{
    public enum OrderTypes : int
    {
        newer_one = 0,
        cheaper = 1,
        expensive = 2,
        by_name = 3
    }
}