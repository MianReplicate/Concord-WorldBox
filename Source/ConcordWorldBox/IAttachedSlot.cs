namespace Concord;

public interface IAttachedSlot
{
    object Get(object target);

    void Set(object target, object value);
}