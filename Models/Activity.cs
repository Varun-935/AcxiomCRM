using System.ComponentModel.DataAnnotations;
namespace AcxiomCRM.Models;
public class Activity{
    public int Id{get;set;}
    [Required] public string ActivityType{get;set;}="Task";
    [Required,MaxLength(120)] public string Subject{get;set;}="";
    [MaxLength(500)] public string Description{get;set;}="";
    public DateTime ActivityDate{get;set;}=DateTime.UtcNow;
    public string Status{get;set;}="Open";
    public string AssignedTo{get;set;}="";
}
