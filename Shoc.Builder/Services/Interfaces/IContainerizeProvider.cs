namespace Shoc.Builder.Services.Interfaces
{
    /// <summary>
    /// Interface for Providing Containerizes
    /// </summary>
    public interface IContainerizeProvider
    {
        /// <summary>
        /// Gets the Containerize instance from name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IContainerize Create(string name);
    }
}
