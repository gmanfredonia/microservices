namespace Building.Base.DTOs;

public class DTOKeyValuePair<TKey> where TKey : struct
{
    public TKey Key { get; set; }
    public String Description { get; set; }
}