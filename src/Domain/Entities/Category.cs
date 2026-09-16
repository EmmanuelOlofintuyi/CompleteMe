namespace CompleteMe.Domain.Entities;

// A Category provides a simple way to group related planning items.
public class Category
{
    // Primary key for the category.
    public Guid Id {get; set;}

    // Category names are required; an empty default avoids a null string.
    public String Name {get; set;} = String.Empty;

    // Optional explanation of how the category is used.
    public String ? Description {get; set;}
}