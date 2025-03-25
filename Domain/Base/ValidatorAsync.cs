namespace Domain.Base
{
    /// <summary>
    /// The async validator abstract class
    /// This validator is used to implement an async validation class to the entity T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValidatorAsync<T> where T : class
    {
        public abstract Task<Operation> ValidateAsync(T model);
    }
}
