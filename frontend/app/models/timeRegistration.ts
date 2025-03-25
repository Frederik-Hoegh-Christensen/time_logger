// src/types/TimeRegistration.ts

export interface TimeRegistrationCreateDTO {
    projectId: string;       // Guid is mapped to string in TypeScript
    freelancerId: string;    // Guid is mapped to string in TypeScript
    workDate: string;        // DateOnly is mapped to string in TypeScript (ISO 8601 format)
    hoursWorked: number;
    description: string;
}
export interface TimeRegistrationDTO {
    id: string;
    projectId: string;       // Guid is mapped to string in TypeScript
    freelancerId: string;    // Guid is mapped to string in TypeScript
    workDate: string;        // DateOnly is mapped to string in TypeScript (ISO 8601 format)
    hoursWorked: number;
    description: string;
    projectName: string;
}

  
