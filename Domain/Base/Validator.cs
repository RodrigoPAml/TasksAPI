namespace Domain.Base
{
    /// <summary>
    /// The validator abstract class
    /// This validator is used to implement a validation class to the entity T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Validator<T> where T : class
    {
        public abstract Operation Validate(T model);
    }
}
