namespace AcxiomCRM.Models;
public class AuditLog{
    public int Id{get;set;}
    public string UserName{get;set;}="system";
    public string Action{get;set;}="";
    public string EntityName{get;set;}="";
    public string RecordId{get;set;}="";
    public string Details{get;set;}="";
    public DateTime CreatedDate{get;set;}=DateTime.UtcNow;
}
