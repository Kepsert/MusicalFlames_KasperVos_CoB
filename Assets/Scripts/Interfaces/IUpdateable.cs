public interface IUpdateable<T>
{
    void UpdateValue(T value);
    void RefreshOverTime(float duration);
}
