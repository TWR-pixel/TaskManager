namespace TaskManager.Core.Entities.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class EntityAttribute : Attribute
{
    public EntityAttribute()
    {
    }
}