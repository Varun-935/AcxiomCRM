using System.ComponentModel.DataAnnotations;
namespace AcxiomCRM.Models;
public class Customer{
    public int Id{get;set;}
    [Required,MaxLength(100)] public string Name{get;set;}="";
    [Required,EmailAddress,MaxLength(150)] public string Email{get;set;}="";
    [Required,RegularExpression(@"^[0-9+() -]{10,15}$",ErrorMessage="Enter a valid phone number."),MaxLength(20)] public string Phone{get;set;}="";
    [MaxLength(150)] public string CompanyName{get;set;}="";
    [MaxLength(250)] public string Address{get;set;}="";
    [MaxLength(80)] public string City{get;set;}="";
    [MaxLength(80)] public string State{get;set;}="";
    public string Status{get;set;}="Active";
    public DateTime CreatedDate{get;set;}=DateTime.UtcNow;
}
