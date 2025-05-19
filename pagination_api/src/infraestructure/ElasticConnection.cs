using Elasticsearch.Net;
using System;

public class ElasticConnection : IDisposable
{
    // Cliente de bajo nivel para interactuar con Elasticsearch
    private readonly ElasticLowLevelClient _client;

    // Bandera para controlar si el objeto ya fue liberado
    private bool _disposed = false;

    public ElasticConnection()
    {
        // Crea un pool de conexión a un solo nodo (localhost)
        var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

        var connectionSettings = new ConnectionConfiguration(pool);

        _client = new ElasticLowLevelClient(connectionSettings);
    }

    // Propiedad para acceder al cliente, solo si no ha sido liberado
    public ElasticLowLevelClient Client
    {
        get
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ElasticConnection));
            return _client;
        }
    }

    // Método público para liberar los recursos usados por la conexión
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    // Lógica principal de liberación de recursos
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            (_client as IDisposable)?.Dispose();

            _disposed = true;
        }
    }

    // Finalizador (por si Dispose no fue llamado explícitamente)
    ~ElasticConnection()
    {
        Dispose(false);
    }
}
