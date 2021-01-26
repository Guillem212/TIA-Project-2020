public struct AttStruct
{
    public string name;
    public int power;
    public int priority;
    public Type type;
    public Category category;
    public StatusModified statusModified;

/// <summary>
/// Struct to send across the network
/// </summary>
/// <param name="name"></param>
/// <param name="power"></param>
/// <param name="priority"></param>
/// <param name="type"></param>
/// <param name="category"></param>
/// <param name="statusModified"></param>
    public AttStruct(string name, int power, int priority, Type type, Category category, StatusModified statusModified)
    {
        this.name = name;
        this.power = power;
        this.priority = priority;
        this.type = type;
        this.category = category;
        this.statusModified = statusModified;
    }
}
