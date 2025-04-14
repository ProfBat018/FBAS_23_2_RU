namespace TechCommerce.DTO.Response;

public record ProductResponseDTO(
    string productName, 
    string productDescription, 
    decimal price, 
    string? imagePath);
    
    