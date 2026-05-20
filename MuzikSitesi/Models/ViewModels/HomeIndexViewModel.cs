using System.Collections.Generic;
using MuzikSitesi.Models;

namespace MuzikSitesi.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Cd> Cdler { get; set; } = new();
    }
}
