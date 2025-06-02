namespace ControllerFirst.Services.Interfaces;

public interface IElasticReindexService<T>
{
    Task ReindexAllAsync();
}