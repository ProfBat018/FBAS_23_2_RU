namespace TechCommerce.DTO.Response;

public record CategoryResponseDTO(string categoryName, string? parentCategoryName = null);