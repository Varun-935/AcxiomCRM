using System.ComponentModel.DataAnnotations;
namespace AcxiomCRM.Models;
public class Lead{
    public int Id{get;set;}
    [Required,MaxLength(100)] public string Name{get;set;}="";
    [EmailAddress] public string Email{get;set;}="";
    [RegularExpression(@"^[0-9+() -]{10,15}$",ErrorMessage="Enter a valid phone number.")] public string Phone{get;set;}="";
    [MaxLength(100)] public string CompanyName{get;set;}="";
    [Required] public string Source{get;set;}="Website";
    [Required] public string Status{get;set;}="New";
    [Range(0,100000000)] public decimal ExpectedValue{get;set;}
    public DateTime CreatedDate{get;set;}=DateTime.UtcNow;
    public string AssignedTo{get;set;}="";
}
