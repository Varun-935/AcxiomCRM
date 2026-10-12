using System.ComponentModel.DataAnnotations;
namespace AcxiomCRM.Models;
public class Opportunity{
    public int Id{get;set;}
    [Required,MaxLength(150)] public string Name{get;set;}="";
    [Required] public int CustomerId{get;set;}
    [Range(0.01,1000000000,ErrorMessage="Amount must be greater than zero.")] public decimal Amount{get;set;}
    [Range(0,100)] public int Probability{get;set;}
    [Required] public string Stage{get;set;}="Qualification";
    [DataType(DataType.Date)] public DateTime ExpectedCloseDate{get;set;}=DateTime.Today.AddDays(30);
    public string Status{get;set;}="Open";
    public DateTime CreatedDate{get;set;}=DateTime.UtcNow;
    public string AssignedTo{get;set;}="";
    public decimal WeightedPipeline=>Amount*Probability/100m;
}
