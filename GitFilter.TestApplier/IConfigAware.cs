namespace GitFilter.TestApplier
{
    public interface IConfigAware<in TConfig>
    {
        void AssignConfig(TConfig config);
    }
}