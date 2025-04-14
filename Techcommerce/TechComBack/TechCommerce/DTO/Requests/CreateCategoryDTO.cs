namespace TechCommerce.DTO.Requests;

public record CreateCategoryDTO(string name, string? parentName=null);