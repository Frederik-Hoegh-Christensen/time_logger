export interface CustomerDTO {
    id: string;  // Guid is represented as a string in TypeScript
    name: string;
    email: string;
}
export interface CustomerCreateDTO {
    name: string;
    email: string;
}