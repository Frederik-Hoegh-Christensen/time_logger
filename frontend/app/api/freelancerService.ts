// src/api/taskService.ts

import type { FreelancerCreateDTO, FreelancerDTO } from "~/models/freelancer";
import { apiService } from "./apiService";

export const freelancerService = {
    getFreelancer: async (id: string) => {
      return await apiService.get<FreelancerDTO>(`/freelancer/get/${id}`);
    },
  
    createFreelancer: async (freelancerData: FreelancerCreateDTO) => {
      return await apiService.post('/freelancer/create', freelancerData);
    },
  
    updateFreelancer: async (id: string, freelancerData: FreelancerDTO) => {
      return await apiService.put(`/freelancer/update/${id}`, freelancerData);
    },
  
    deleteFreelancer: async (id: string) => {
      return await apiService.delete(`/freelancer/delete/${id}`);
    },
  
  };