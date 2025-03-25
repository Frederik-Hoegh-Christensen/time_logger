// src/types/Project.ts

export interface ProjectCreateDTO {
    freelancerId: string;
    name: string;
    customerId: string;
    deadline: string; 
    isCompleted: boolean | undefined;
    
  }
  
  export interface ProjectDTO {
    id: string; 
    freelancerId: string;
    name: string;
    client: string;
    deadline: string;
    customerName: string;
    isCompleted: boolean;
  }
  