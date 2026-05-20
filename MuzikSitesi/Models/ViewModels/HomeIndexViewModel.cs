using System.Collections.Generic;
using MuzikSitesi.Models;

namespace MuzikSitesi.Models.ViewModels
{
    // Ana sayfanin ihtiyac duydugu CD listesini tasir.
    public class HomeIndexViewModel
    {
        public List<Cd> Cdler { get; set; } = new();
    }
}
