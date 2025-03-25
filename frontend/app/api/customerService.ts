
import type { CustomerCreateDTO, CustomerDTO } from "~/models/customer";
import { apiService } from "./apiService";


export const customerService = {
  fetchCustomerByProjectId: async (projectId: string): Promise<CustomerDTO> => {
    return await apiService.get<CustomerDTO>(`/customer/${projectId}`);
  },

//   fetchProjectsByFreelancerId: async (freelancerId: string): Promise<ProjectDTO[]> => {
//     return await apiService.get<ProjectDTO[]>(`/project/freelancer/${freelancerId}`);
//   },

  createCustomer: async (customerCreateDTO: CustomerCreateDTO): Promise<string> => {
    return await apiService.post('/customer/create', customerCreateDTO);
  },

//   updateProject: async (projectId: string, updatedProject: ProjectDTO): Promise<void> => {
//     return await apiService.put(`/project/${projectId}`, updatedProject);
//   },

//   deleteProject: async (projectId: string): Promise<void> => {
//     return await apiService.delete(`/project/${projectId}`);
//   },
};
