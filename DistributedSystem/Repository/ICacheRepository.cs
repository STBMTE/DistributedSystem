namespace DistributedSystem.Repository
{
    public interface ICacheRepository
    {
        public Task<T> GetData<T>(string id, CancellationToken cancellation = default)
            where T : class;

        public Task SetData<T>(string id, T value, CancellationToken cancellation = default);

        public Task RemoveData(string id, CancellationToken cancellation = default);
    }
}
}
