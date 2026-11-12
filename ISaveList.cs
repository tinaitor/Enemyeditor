namespace Enemyeditor
{
    /// <summary>Общий контракт для сохранения/загрузки данных.</summary>
    public interface ISaveList<T>
    {
        T Load(string path);
        void Save(T data, string path);
    }
}
