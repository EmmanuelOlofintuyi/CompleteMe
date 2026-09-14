namespace CompleteMe.Domain.Entities;
public class Category
{
    public Guid Id {get; set;}
    public String Name {get; set;} = String.Empty;
    public String ? Description {get; set;}
}