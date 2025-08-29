namespace Homie.Types;

public abstract record CustomTypeBase<T>(T Value)
{
    public static bool operator ==(CustomTypeBase<T> left, T right)
    {
        if (left is null || left.Value is null) return right == null;
        return left.Value.Equals(right);
    }
    
    public static bool operator !=(CustomTypeBase<T> left, T right) => !(left == right);
    
    public static bool operator ==(T left, CustomTypeBase<T> right) => right == left;
    
    public static bool operator !=(T left, CustomTypeBase<T> right) => !(left == right);
}