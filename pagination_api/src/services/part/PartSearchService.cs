using PaginationApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaginationApp.Services.ElasticSearch.Contracts;
using PaginationApp.Services.Parts.Contracts;

namespace PaginationApp.Services.Parts
{
    // Servicio de búsqueda de partes usando Elasticsearch como backend
    public class ElasticPartSearchService : IPartSearchService
    {
        private readonly IElasticSearchService _searchService; // Servicio de búsqueda en Elasticsearch
        private readonly IPartMapper _mapper; // Encargado de mapear la respuesta a DTOs paginados

        public ElasticPartSearchService(
            IElasticSearchService searchService,
            IPartMapper mapper)
        {
            _searchService = searchService;
            _mapper = mapper;
        }

        // Ejecuta una búsqueda paginada de partes con filtros opcionales
        public async Task<PaginatedResult<PartDto>> SearchPartsAsync(
            int pageNumber,
            int pageSize,
            Dictionary<string, string>? filters = null)
        {
            // Ejecutar la búsqueda con los filtros proporcionados
            var response = await _searchService.SearchPartsAsync(
                filters ?? new Dictionary<string, string>(), 
                pageNumber,
                pageSize);

            // Mapear los resultados a un formato paginado de DTOs
            return _mapper.MapToPaginatedResult(response, pageNumber, pageSize);
        }
    }
}
