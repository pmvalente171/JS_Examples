namespace GameArchitecture.Store
{
    public interface IRuntimeStore
    {
        /// <summary>
        /// Method for starting
        /// the store
        /// </summary>
        void InitiateStore();
        
        /// <summary>
        /// method for destroying a store
        /// </summary>
        void Destroy();

        /// <summary>
        /// method for getting all tags
        /// associated with this store
        /// </summary>
        void GetTags();
    }
}