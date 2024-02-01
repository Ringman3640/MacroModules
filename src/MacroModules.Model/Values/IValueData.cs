namespace MacroModules.Model.Values
{
    /// <summary>
    /// Exposes a property that represents the data of a <see cref="Value"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueData<T>
    {
        public T Data { get; set; }
    }
}
