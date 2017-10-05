namespace ColoursTest.Domain.Interfaces
{
    public interface IWriteableRepository<T> where T : class
    {
        T Insert(T item);
        T Update(T item);
    }
}