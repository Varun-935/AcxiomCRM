using System.ComponentModel.DataAnnotations;
namespace AcxiomCRM.Models;
public class FollowUp{
    public int Id{get;set;}
    [Required,MaxLength(120)] public string Subject{get;set;}="";
    [DataType(DataType.Date),Required] public DateTime FollowUpDate{get;set;}=DateTime.Today.AddDays(1);
    [Required] public string Type{get;set;}="Call";
    [Required] public string Status{get;set;}="Planned";
    [MaxLength(500)] public string Notes{get;set;}="";
    public string AssignedTo{get;set;}="";
}
