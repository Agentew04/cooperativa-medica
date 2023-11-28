namespace CoopMedica.Views;

public class ServiceView
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; }
    public float ServiceCost { get; set; }
    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; }
    public int MedicId { get; set; }
    public string MedicName { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; }
}
