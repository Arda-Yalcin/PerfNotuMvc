namespace MuzikSitesi.Models;

public class ErrorViewModel
{
    // Hata sayfasinda takip numarasi gosterimi icin kullanilir.
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
